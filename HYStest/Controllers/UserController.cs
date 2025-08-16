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
        private readonly ILogger<UserController> _logger;

        public UserController(IUserRepository userRepository, IMeetingRepository meetingRepository, ILogger<UserController> logger)
        {
            _userRepository = userRepository;
            _meetingRepository = meetingRepository;
            _logger = logger;
        }

        [HttpPost("/users")]
        public async Task<IActionResult> CreateUser([FromBody] string userName)
        {
            _logger.LogInformation($"Creating user with name: {userName}");

            User user = new User(userName);
            var createdUser = await _userRepository.AddAsync(user);
            if (createdUser == null)
            {
                _logger.LogError("User could not be created.");
                return BadRequest("User could not be created.");
            }

            _logger.LogInformation($"User created successfully: {createdUser.Id}");
            return Ok(createdUser);
        }

        [HttpGet("/users/{id}/meetings")]
        public async Task<IActionResult> GetUserMeetings(long id)
        {
            _logger.LogInformation($"Retrieving meetings for user: {id}");
            List<Meeting> meetings = (await _meetingRepository.GetByUserIdAsync(id)).ToList();
            if (meetings.Count == 0)
            {
                _logger.LogWarning($"No meetings found for user: {id}");
                return NotFound("User meetings was not found.");
            }
            return Ok(meetings);
        }
    }
}
