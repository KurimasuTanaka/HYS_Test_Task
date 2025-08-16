using DataAccess;
using DataAccess.DTO;

namespace Tests;

[CollectionDefinition(DisableParallelization = true)]
public class MeetingsServicesTests : TestBase
{

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
        alice.Meetings.Add(new Meeting(new List<User> { }, new DateTime(2023, 10, 1, 9, 0, 0), new DateTime(2023, 10, 1, 10, 30, 0)));

        //Schedule meeting for Bob from 11:00 to 11:30
        User bob = new User("Bob");
        bob.Meetings.Add(new Meeting(new List<User> { }, new DateTime(2023, 10, 1, 11, 00, 0), new DateTime(2023, 10, 1, 11, 30, 0)));

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

    [Fact]
    public async Task ScheduleMeetingOnFreeTimeWhemTheFirstDayIsBusy()
    {
        //Arrange

        //Schedule meeting for Alice from 9:00 to 15:30
        User alice = new User("Alice");
        alice.Meetings.Add(new Meeting(new List<User> { }, new DateTime(2023, 10, 1, 9, 0, 0), new DateTime(2023, 10, 1, 15, 30, 0)));

        //Schedule meeting for Bob from 14:00 to 17:30
        User bob = new User("Bob");
        bob.Meetings.Add(new Meeting(new List<User> { }, new DateTime(2023, 10, 1, 14, 00, 0), new DateTime(2023, 10, 1, 17, 30, 0)));

        await _userRepository.AddAsync(alice);
        await _userRepository.AddAsync(bob);


        //Act
        MeetingSchedulingInfo schedulingInfo = new MeetingSchedulingInfo([1, 2], 120, new DateTime(2023, 10, 1, 9, 0, 0), new DateTime(2023, 10, 2, 17, 0, 0));
        ScheduledTimeInfo scheduledTimeInfo = await _meetingSchedulerService.ScheduleMeeting(schedulingInfo);

        //Assert
        Assert.NotNull(scheduledTimeInfo);
        Assert.Equal(new DateTime(2023, 10, 2, 9, 0, 0), scheduledTimeInfo.StartTime);
        Assert.Equal(new DateTime(2023, 10, 2, 11, 0, 0), scheduledTimeInfo.EndTime);
    }

    [Fact]
    public async Task ScheduleBackToBackMeetings()
    {
        //Arrange

        //Schedule meeting for Alice from 9:00 to 10:30
        User alice = new User("Alice");
        alice.Meetings.Add(new Meeting(new List<User> { }, new DateTime(2023, 10, 1, 9, 0, 0), new DateTime(2023, 10, 1, 10, 30, 0)));

        //Schedule meeting for Bob from 11:00 to 11:30
        User bob = new User("Bob");
        bob.Meetings.Add(new Meeting(new List<User> { }, new DateTime(2023, 10, 1, 11, 00, 0), new DateTime(2023, 10, 1, 11, 30, 0)));

        await _userRepository.AddAsync(alice);
        await _userRepository.AddAsync(bob);

        //Act
        MeetingSchedulingInfo schedulingInfo1 = new MeetingSchedulingInfo([1, 2], 120, new DateTime(2023, 10, 1, 9, 0, 0), new DateTime(2023, 10, 2, 17, 0, 0));
        ScheduledTimeInfo scheduledTimeInfo1 = await _meetingSchedulerService.ScheduleMeeting(schedulingInfo1);

        MeetingSchedulingInfo schedulingInfo2 = new MeetingSchedulingInfo([1], 60, new DateTime(2023, 10, 1, 9, 0, 0), new DateTime(2023, 10, 2, 17, 0, 0));
        ScheduledTimeInfo scheduledTimeInfo2 = await _meetingSchedulerService.ScheduleMeeting(schedulingInfo2);

        MeetingSchedulingInfo schedulingInfo3 = new MeetingSchedulingInfo([1, 2], 30, new DateTime(2023, 10, 1, 9, 0, 0), new DateTime(2023, 10, 2, 17, 0, 0));
        ScheduledTimeInfo scheduledTimeInfo3 = await _meetingSchedulerService.ScheduleMeeting(schedulingInfo3);


        //Assert
        Assert.NotNull(scheduledTimeInfo1);
        Assert.Equal(new DateTime(2023, 10, 1, 11, 30, 0), scheduledTimeInfo1.StartTime);
        Assert.Equal(new DateTime(2023, 10, 1, 13, 30, 0), scheduledTimeInfo1.EndTime);

        Assert.NotNull(scheduledTimeInfo2);
        Assert.Equal(new DateTime(2023, 10, 1, 10, 30, 0), scheduledTimeInfo2.StartTime);
        Assert.Equal(new DateTime(2023, 10, 1, 11, 30, 0), scheduledTimeInfo2.EndTime);

        Assert.NotNull(scheduledTimeInfo3);
        Assert.Equal(new DateTime(2023, 10, 1, 13, 30, 0), scheduledTimeInfo3.StartTime);
        Assert.Equal(new DateTime(2023, 10, 1, 14, 0, 0), scheduledTimeInfo3.EndTime);

    }


}
