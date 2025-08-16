using DataAccess;
using DataAccess.DTO;
using HYStest.Services.MeetingSchedulerService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HYStest.Controllers
{
    [ApiController]
    public class MeetingController : ControllerBase
    {
        private readonly IMeetingSchedulerService _meetingSchedulerService;

        public MeetingController(IMeetingSchedulerService meetingSchedulerService)
        {
            _meetingSchedulerService = meetingSchedulerService;
        }

        [HttpPost("meetings")]
        public async Task<ScheduledTimeInfo> ScheduleMeeting([FromBody] MeetingSchedulingInfo schedulingInfo)
        {
            var scheduledTime = await _meetingSchedulerService.ScheduleMeeting(schedulingInfo);
            // if (scheduledTime.StartTime != DateTime.MinValue && scheduledTime.EndTime != DateTime.MinValue)
            // {
            // }
                return scheduledTime;
        }


    }
}
