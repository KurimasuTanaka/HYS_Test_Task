using Database;

namespace DataAccess;

public class Meeting : MeetingModel
{
    public Meeting(MeetingModel model)
    {
        Id = model.Id;
        Participants = model.Participants;
        StartTime = model.StartTime;
        EndTime = model.EndTime;
    }
} 