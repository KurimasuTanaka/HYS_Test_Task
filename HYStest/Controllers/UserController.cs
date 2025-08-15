using DataAccess;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HYStest.Controllers
{
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IMeetingRepository _meetingRepository;

        public UserController(IUserRepository userRepository, IMeetingRepository meetingRepository)
        {
            _userRepository = userRepository;
            _meetingRepository = meetingRepository;
        }

        [HttpPost("/users")]
        public async Task<IActionResult> CreateUser([FromBody] string userName)
        {
            User user = new User(userName);
            var createdUser = await _userRepository.AddAsync(user);
            if (createdUser == null) return BadRequest("User could not be created.");
            return Ok(createdUser);
        }

        [HttpGet("/users/{id}/meetings")]
        public async Task<IActionResult> GetUserMeetings(long id)
        {
            List<Meeting> meetings = (await _meetingRepository.GetByUserIdAsync(id)).ToList();
            if (meetings.Count == 0) return NotFound("User meetings was not found.");
            return Ok(meetings);
        }
    }
}
