using System;
using DataAccess;
using DataAccess.DTO;

namespace HYStest.Services.MeetingSchedulerService;

/// Service implementation for meeting scheduling operations
public class MeetingSchedulerService : IMeetingSchedulerService
{
    private readonly IMeetingRepository _meetingRepository;
    private readonly IUserRepository _userRepository;
    private readonly ILogger<MeetingSchedulerService> _logger;

    private readonly TimeOnly _workDayStart = new TimeOnly(9, 0);
    private readonly TimeOnly _workDayEnd = new TimeOnly(17, 0);

    public MeetingSchedulerService(IMeetingRepository meetingRepository, IUserRepository userRepository, ILogger<MeetingSchedulerService> logger)
    {
        _meetingRepository = meetingRepository;
        _userRepository = userRepository;
        _logger = logger;
    }

    /// Schedules a meeting by finding the earliest available time slot for all participants
    public async Task<ScheduledTimeInfo> ScheduleMeeting(MeetingSchedulingInfo schedulingInfo)
    {
        _logger.LogInformation($"Scheduling meeting: {schedulingInfo}");

        if (schedulingInfo.EarliestStart > schedulingInfo.LatestEnd)
        {
            _logger.LogError("Earliest start time cannot be after latest end time.");
            throw new ArgumentException("Earliest start time cannot be after latest end time.");
        }

        if (schedulingInfo.LatestEnd - schedulingInfo.EarliestStart < TimeSpan.FromMinutes(schedulingInfo.DurationMinutes))
        {
            _logger.LogError("Insufficient time available for meeting.");
            throw new ArgumentException("Insufficient time available for meeting.");
        }

        List<User> participants = new List<User>();
        foreach (long participantId in schedulingInfo.ParticipantIds)
        {
            var user = await _userRepository.GetByIdAsync(participantId);
            if (user != null)
            {
                participants.Add(user);
            }
            else
            {
                _logger.LogError($"User with ID {participantId} does not exist.");
                throw new ArgumentException($"User with ID {participantId} does not exist.");
            }
        }


        //Trim time range to work hours
        if (schedulingInfo.EarliestStart.TimeOfDay < _workDayStart.ToTimeSpan())
        {
            schedulingInfo.EarliestStart = new DateTime(schedulingInfo.EarliestStart.Year, schedulingInfo.EarliestStart.Month, schedulingInfo.EarliestStart.Day, _workDayStart.Hour, _workDayStart.Minute, _workDayStart.Second);
        }
        if (schedulingInfo.LatestEnd.TimeOfDay > _workDayEnd.ToTimeSpan())
        {
            schedulingInfo.LatestEnd = new DateTime(schedulingInfo.LatestEnd.Year, schedulingInfo.LatestEnd.Month, schedulingInfo.LatestEnd.Day, _workDayEnd.Hour, _workDayEnd.Minute, _workDayEnd.Second);
        }

        DateTime possibleStartTime = schedulingInfo.EarliestStart;
        while (possibleStartTime <= schedulingInfo.LatestEnd)
        {
            //Check if possibleStartTime is outside of work hours

            if (possibleStartTime.TimeOfDay < _workDayStart.ToTimeSpan() || possibleStartTime.TimeOfDay >= _workDayEnd.ToTimeSpan())
            {
                possibleStartTime = new DateTime(possibleStartTime.Year, possibleStartTime.Month, possibleStartTime.Day + 1, _workDayStart.Hour, _workDayStart.Minute, _workDayStart.Second);
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
        
        throw new InvalidOperationException("Unable to schedule meeting.");
    }
}
