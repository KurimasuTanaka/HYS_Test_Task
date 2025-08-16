using System;

namespace DataAccess.DTO;

public class MeetingSchedulingInfo
{
    public List<long> ParticipantIds { get; set; }
    public int DurationMinutes { get; set; }
    public DateTime EarliestStart { get; set; }
    public DateTime LatestEnd { get; set; }

    public MeetingSchedulingInfo(List<long> participantIds, int durationMinutes, DateTime earliestStart, DateTime latestEnd)
    {
        ParticipantIds = participantIds;
        DurationMinutes = durationMinutes;
        EarliestStart = earliestStart;
        LatestEnd = latestEnd;
    }
}
