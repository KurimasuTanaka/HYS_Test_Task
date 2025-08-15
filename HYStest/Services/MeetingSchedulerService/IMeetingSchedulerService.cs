using System;
using DataAccess;

namespace HYStest.Services.MeetingSchedulerService;

public interface IMeetingSchedulerService
{
    Task<Meeting> ScheduleMeeting(
        List<long> participantIds,
        int durationMinutes,
        DateTime earliestStart,
        DateTime latestEnd
    );
}
