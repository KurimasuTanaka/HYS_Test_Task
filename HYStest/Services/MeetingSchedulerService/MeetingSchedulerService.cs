using System;
using DataAccess;
using DataAccess.DTO;

namespace HYStest.Services.MeetingSchedulerService;

public class MeetingSchedulerService : IMeetingSchedulerService
{
    private readonly IMeetingRepository _meetingRepository;
    private readonly IUserRepository _userRepository;

    private const int _workDayStart = 9;
    private const int _workDayEnd = 17;

    public MeetingSchedulerService(IMeetingRepository meetingRepository, IUserRepository userRepository)
    {
        _meetingRepository = meetingRepository;
        _userRepository = userRepository;
    }

    public async Task<ScheduledTimeInfo> ScheduleMeeting(MeetingSchedulingInfo schedulingInfo)
    {
        List<User> participants = new List<User>();

        List<User> allUsers = (await _userRepository.GetAllAsync()).ToList();

        foreach (long participantId in schedulingInfo.ParticipantIds)
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

        DateTime possibleStartTime = schedulingInfo.EarliestStart;
        while (possibleStartTime <= schedulingInfo.LatestEnd)
        {
            if(possibleStartTime.Hour < _workDayStart || possibleStartTime.Hour >= _workDayEnd)
            {
                possibleStartTime = new DateTime(possibleStartTime.Year, possibleStartTime.Month, possibleStartTime.Day + 1, _workDayStart, 0, 0);
            }

            //Check if all users are available at possible start time 
            if (participants.All(user => user.IsUserAvailableAtTime(possibleStartTime)))
            {
                //Check if all users are available for the entire meeting duration
                if (participants.All(user => user.IsUserAvailableAtTimeSpan(possibleStartTime, possibleStartTime.AddMinutes(schedulingInfo.DurationMinutes))))
                {
                    //Schedule the meeting
                    var meeting = new Meeting(participants, possibleStartTime, possibleStartTime.AddMinutes(schedulingInfo.DurationMinutes));
                    await _meetingRepository.AddAsync(meeting);

                    return new ScheduledTimeInfo(meeting.StartTime, meeting.EndTime);
                }
                else
                {
                    //If any user is not available, wait until the user will be and repeat first step
                    User user = participants.First(u => !u.IsUserAvailableAtTimeSpan(possibleStartTime, possibleStartTime.AddMinutes(schedulingInfo.DurationMinutes)));
                    possibleStartTime = user.TimeToEndOfFirstMeetingAtTimeSpan(possibleStartTime, possibleStartTime.AddMinutes(schedulingInfo.DurationMinutes));
                }
            }
            else
            {
                //If any user is not available, wait until the user will be and repeat first step
                User user = participants.First(u => !u.IsUserAvailableAtTime(possibleStartTime));

                possibleStartTime = user.DateTimeOfEndOfMeeting(possibleStartTime);
            }
        }
        throw new InvalidOperationException("Unable to schedule meeting.");    }
}
