using Database;
using Google.Protobuf.WellKnownTypes;
using Org.BouncyCastle.Asn1.Cms;

namespace DataAccess;

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

    public bool IsUserAvailableAtTime(DateTime time)
    {
        return !Meetings.Any(m => m.StartTime <= time && m.EndTime > time);
    }

    public DateTime DateTimeOfEndOfMeeting(DateTime timeOnWhichMeetingIsScheduled)
    {
        var meeting = Meetings.FirstOrDefault(m => m.StartTime <= timeOnWhichMeetingIsScheduled && m.EndTime > timeOnWhichMeetingIsScheduled);

        if (meeting == null) return DateTime.MinValue;
        return meeting.EndTime;
    }

    public bool IsUserAvailableAtTimeSpan(DateTime startTime, DateTime endTime)
    {
        return !Meetings.Any(m => (m.EndTime > startTime && m.EndTime < endTime) || (m.StartTime > startTime && m.StartTime < endTime));
    }

    public DateTime TimeToEndOfFirstMeetingAtTimeSpan(DateTime startTime, DateTime endTime)
    {
        var meeting = Meetings.FirstOrDefault(m => (m.EndTime > startTime && m.EndTime < endTime) || (m.StartTime > startTime && m.StartTime < endTime));
        if (meeting == null) return DateTime.MinValue;
        return meeting.EndTime;
    }
} 