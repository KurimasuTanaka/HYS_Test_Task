using DataAccess;
using DataAccess.DTO;
using Database;
using HYStest.Services.MeetingSchedulerService;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;

namespace Tests;

public class MeetingsServicesTests
{
    private readonly TestDbContextFactory _contextFactory;
    private readonly IUserRepository _userRepository;
    private readonly IMeetingRepository _meetingRepository;
    private readonly IMeetingSchedulerService _meetingSchedulerService;

    public MeetingsServicesTests()
    {
        _contextFactory = new TestDbContextFactory();
        _userRepository = new UserRepository(_contextFactory);
        _meetingRepository = new MeetingRepository(_contextFactory);
        _meetingSchedulerService = new MeetingSchedulerService(_meetingRepository, _userRepository);

        _contextFactory.DeleteTestDb();
    }

    [Fact]
    public async Task ScheduleMeetingOnFreeTimeInEmptySchedule()
    {
        //Arrange

        await _userRepository.AddAsync(new User("Alice"));
        await _userRepository.AddAsync(new User("Bob"));

        //Act
        MeetingSchedulingInfo schedulingInfo = new MeetingSchedulingInfo([1, 2], 50, new DateTime(2023, 10, 1, 9, 0, 0), new DateTime(2023, 10, 1, 17, 0, 0));

        ScheduledTimeInfo scheduledTimeInfo = await _meetingSchedulerService.ScheduleMeeting(schedulingInfo);

        //Assert
        Assert.NotNull(scheduledTimeInfo);
        Assert.Equal(new DateTime(2023, 10, 1, 9, 0, 0), scheduledTimeInfo.StartTime);
        Assert.Equal(new DateTime(2023, 10, 1, 9, 50, 0), scheduledTimeInfo.EndTime);
    }

    [Fact]
    public async Task ScheduleMeetingOnFreeTimeInOccupiedSchedule()
    {
        //Arrange

        //Schedule meeting for Alice from 9:00 to 10:30
        User alice = new User("Alice");
        alice.Meetings.Add(new Meeting(new List<User> {  }, new DateTime(2023, 10, 1, 9, 0, 0), new DateTime(2023, 10, 1, 10, 30, 0)));

        //Schedule meeting for Bob from 11:00 to 11:30
        User bob = new User("Bob");
        bob.Meetings.Add(new Meeting(new List<User> {  }, new DateTime(2023, 10, 1, 11, 00, 0), new DateTime(2023, 10, 1, 11, 30, 0)));

        await _userRepository.AddAsync(alice);
        await _userRepository.AddAsync(bob);


        //Act
        MeetingSchedulingInfo schedulingInfo = new MeetingSchedulingInfo([1, 2], 50, new DateTime(2023, 10, 1, 9, 0, 0), new DateTime(2023, 10, 1, 17, 0, 0));
        ScheduledTimeInfo scheduledTimeInfo = await _meetingSchedulerService.ScheduleMeeting(schedulingInfo);

        //Assert
        Assert.NotNull(scheduledTimeInfo);
        Assert.Equal(new DateTime(2023, 10, 1, 11, 30, 0), scheduledTimeInfo.StartTime);
        Assert.Equal(new DateTime(2023, 10, 1, 12, 20, 0), scheduledTimeInfo.EndTime);
    }
}
