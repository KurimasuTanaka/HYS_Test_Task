using System;
using DataAccess;

namespace HYStest.Services.MeetingSchedulerService;

public class MeetingSchedulerService : IMeetingSchedulerService
{
    private readonly MeetingRepository _meetingRepository;
    private readonly UserRepository _userRepository;

    public MeetingSchedulerService(MeetingRepository meetingRepository, UserRepository userRepository)
    {
        _meetingRepository = meetingRepository;
        _userRepository = userRepository;
    }

    public async Task<Meeting> ScheduleMeeting(List<long> participantIds, int durationMinutes, DateTime earliestStart, DateTime latestEnd)
    {
        List<User> participants = new List<User>();
        foreach (long participantId in participantIds)
        {
            var user = await _userRepository.GetByIdAsync(participantId);
            if (user != null)
            {
                participants.Add(user);
            }
            else
            {
                throw new ArgumentException($"User with ID {participantId} does not exist.");
            }
        }

        DateTime possibleStartTime = earliestStart;
        while (possibleStartTime <= latestEnd)
        {
            //Check if all users are available at possible start time 
            if (participants.All(user => user.IsUserAvailableAtTime(possibleStartTime)))
            {
                //Check if all users are available for the entire meeting duration
                if (participants.All(user => user.IsUserAvailableAtTimeSpan(possibleStartTime, possibleStartTime.AddMinutes(durationMinutes))))
                {
                    //Schedule the meeting
                    var meeting = new Meeting
                    {
                        StartTime = possibleStartTime,
                        EndTime = possibleStartTime.AddMinutes(durationMinutes),
                        
                    };
                    await _meetingRepository.AddAsync(meeting);
                    return meeting;
                }
            }
            else
            {
                //If any user is not available, wait until the user will be and repeat first step
                possibleStartTime += participants.First(user => !user.IsUserAvailableAtTime(possibleStartTime)).TimeToEndOfMeeting(possibleStartTime);
            }
        }

        throw new InvalidOperationException("Unable to schedule meeting.");
    }
}
