using DataAccess;
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

        public async Task<IActionResult> ScheduleMeeting(
            [FromBody] List<long> participantIds,
            [FromBody] int durationMinutes,
            [FromBody] DateTime earliestStart,
            [FromBody] DateTime latestEnd)
        {
            Meeting meeting = await _meetingSchedulerService.ScheduleMeeting(participantIds, durationMinutes, earliestStart, latestEnd);
            if (meeting != null)
            {
                return Ok("Meeting created successfully.");
            }
            return BadRequest("Failed to create meeting.");
        }


    }
}
