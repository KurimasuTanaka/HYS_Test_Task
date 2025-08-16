using System;
using DataAccess;
using DataAccess.DTO;

namespace HYStest.Services.MeetingSchedulerService;

public interface IMeetingSchedulerService
{
    Task<ScheduledTimeInfo> ScheduleMeeting(MeetingSchedulingInfo schedulingInfo
    );
}
