using System;
using DataAccess;

namespace Tests;

public class UserTests
{
    [Fact]
    public void CheckIfUserIsAvailableAtTimeWhenScheduleIsEmpty()
    {
        //Arrange
        User user = new User("Alice");

        //Act
        bool isAvailable = user.IsUserAvailableAtTime(new DateTime(2023, 10, 1, 9, 0, 0));

        //Assert
        Assert.True(isAvailable);
    }
    [Fact]
    public void CheckIfUserIsAvailableAtTimeWhenTimeIsTaken()
    {
        //Arrange
        User user = new User("Alice");
        user.Meetings.Add(new Meeting(new List<User> { new User("Alice") }, new DateTime(2023, 10, 1, 9, 0, 0), new DateTime(2023, 10, 1, 17, 0, 0)));
        //Act
        bool isAvailable = user.IsUserAvailableAtTime(new DateTime(2023, 10, 1, 9, 0, 0));

        //Assert
        Assert.False(isAvailable);
    }
    [Fact]
    public void CheckIfUserIsAvailableAtTimeRangeWhenTimeIsTaken()
    {
        //Arrange
        User user = new User("Alice");
        user.Meetings.Add(new Meeting(new List<User> { new User("Alice") }, new DateTime(2023, 10, 1, 9, 0, 0), new DateTime(2023, 10, 1, 13, 0, 0)));
        //Act
        bool isAvailable = user.IsUserAvailableAtTimeSpan(new DateTime(2023, 10, 1, 12, 0, 0), new DateTime(2023, 10, 1, 14, 0, 0));

        //Assert
        Assert.False(isAvailable);
    }

    [Fact]
    public void CheckIfUserIsAvailableAtTimeRangeWhenTimeIsAvailable()
    {
        //Arrange
        User user = new User("Alice");
        user.Meetings.Add(new Meeting(new List<User> { new User("Alice") }, new DateTime(2023, 10, 1, 10, 0, 0), new DateTime(2023, 10, 1, 13, 0, 0)));
        //Act
        bool isAvailable = user.IsUserAvailableAtTimeSpan(new DateTime(2023, 10, 1, 9, 0, 0), new DateTime(2023, 10, 1, 10, 0, 0));

        //Assert
        Assert.True(isAvailable);
    }


    [Fact]
    public void FindTimeToEndOfScheduledMeeting()
    {
        //Arrange
        User user = new User("Alice");
        user.Meetings.Add(new Meeting(new List<User> { new User("Alice") }, new DateTime(2023, 10, 1, 9, 0, 0), new DateTime(2023, 10, 1, 12, 0, 0)));
        //Act
        DateTime timeLeft = user.DateTimeOfEndOfMeeting(new DateTime(2023, 10, 1, 11, 0, 0));

        //Assert
        Assert.True(timeLeft == new DateTime(2023, 10, 1, 12, 0, 0));
    }

    [Fact]
    public void FindTimeToEndOfScheduledMeetingAtTimeSpan()
    {
        //Arrange
        User user = new User("Alice");
        user.Meetings.Add(new Meeting(new List<User> { new User("Alice") }, new DateTime(2023, 10, 1, 11, 0, 0), new DateTime(2023, 10, 1, 11, 30, 0)));
        //Act
        DateTime timeOfEnd = user.TimeToEndOfFirstMeetingAtTimeSpan(new DateTime(2023, 10, 1, 10, 30, 0), new DateTime(2023, 10, 1, 11, 20, 0));

        //Assert
        Assert.True(timeOfEnd == new DateTime(2023, 10, 1, 11, 30, 0));
    }
}
