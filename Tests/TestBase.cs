using System;
using DataAccess;
using HYStest.Services.MeetingSchedulerService;
using Microsoft.Extensions.Logging;
using Moq;

namespace Tests;

public class TestBase : IDisposable
{
    protected readonly TestDbContextFactory _contextFactory;
    protected readonly IUserRepository _userRepository;
    protected readonly IMeetingRepository _meetingRepository;
    protected readonly IMeetingSchedulerService _meetingSchedulerService;

    private readonly Mock<ILogger<MeetingSchedulerService>> _mockLogger = new Mock<ILogger<MeetingSchedulerService>>();
    public TestBase()
    {

        _contextFactory = new TestDbContextFactory();
        _userRepository = new UserRepository(_contextFactory);
        _meetingRepository = new MeetingRepository(_contextFactory);
        _meetingSchedulerService = new MeetingSchedulerService(_meetingRepository, _userRepository, _mockLogger.Object);

    }

    public void Dispose()
    {
        _contextFactory.DeleteTestDb();
    }
}
