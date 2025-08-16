using Database;
using Google.Protobuf.WellKnownTypes;
using Org.BouncyCastle.Asn1.Cms;

namespace DataAccess;

/// User entity with availability checking capabilities
public class User : UserModel
{
    public User(UserModel model)
    {
        Id = model.Id;
        Name = model.Name;
        Meetings = model.Meetings;
    }

    public User(string name)
    {
        Name = name;
    }

    /// Checks if the user is available at a specific time
    public bool IsUserAvailableAtTime(DateTime time)
    {
        return !Meetings.Any(m => m.StartTime <= time && m.EndTime > time);
    }

    /// Gets the end time of the meeting that conflicts with the specified time
    public DateTime DateTimeOfEndOfMeeting(DateTime timeOnWhichMeetingIsScheduled)
    {
        var meeting = Meetings.FirstOrDefault(m => m.StartTime <= timeOnWhichMeetingIsScheduled && m.EndTime > timeOnWhichMeetingIsScheduled);

        if (meeting == null) return DateTime.MinValue;
        return meeting.EndTime;
    }

    /// Checks if the user is available for an entire time span
    public bool IsUserAvailableAtTimeSpan(DateTime startTime, DateTime endTime)
    {
        return !Meetings.Any(m => (m.EndTime > startTime && m.EndTime < endTime) || (m.StartTime > startTime && m.StartTime < endTime));
    }

    /// Gets the end time of the first meeting that conflicts with the specified time span
    public DateTime TimeToEndOfFirstMeetingAtTimeSpan(DateTime startTime, DateTime endTime)
    {
        var meeting = Meetings.FirstOrDefault(m => (m.EndTime > startTime && m.EndTime < endTime) || (m.StartTime > startTime && m.StartTime < endTime));
        if (meeting == null) return DateTime.MinValue;
        return meeting.EndTime;
    }
} 