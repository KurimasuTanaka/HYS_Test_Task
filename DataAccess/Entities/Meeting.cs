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

    public Meeting(List<User> participants, DateTime startTime, DateTime endTime)
    {
        Participants = participants.Select(p => new UserModel
        {
            Id = p.Id,
            Name = p.Name
        }).ToList();
        StartTime = startTime;
        EndTime = endTime;
    }

}