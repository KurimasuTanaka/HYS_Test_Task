using DataAccess;
using DataAccess.DTO;
using HYStest.Services.MeetingSchedulerService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace HYStest.Controllers
{
    [ApiController]
    public class MeetingController : ControllerBase
    {
        private readonly IMeetingSchedulerService _meetingSchedulerService;
        private readonly ILogger<MeetingController> _logger;

        public MeetingController(IMeetingSchedulerService meetingSchedulerService, ILogger<MeetingController> logger)
        {
            _meetingSchedulerService = meetingSchedulerService;
            _logger = logger;
        }

        [HttpPost("meetings")]
        public async Task<ActionResult<ScheduledTimeInfo>> ScheduleMeeting([FromBody] MeetingSchedulingInfo schedulingInfo)
        {
            ScheduledTimeInfo? scheduledTimeInfo = null;

            try
            {
                scheduledTimeInfo = await _meetingSchedulerService.ScheduleMeeting(schedulingInfo);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error scheduling meeting: {ex.Message}");
            }

            return scheduledTimeInfo != null ? Ok(scheduledTimeInfo) : BadRequest("Failed to schedule meeting.")   ;
        }


    }
}
