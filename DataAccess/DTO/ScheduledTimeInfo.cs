using System;

namespace DataAccess.DTO;

public class ScheduledTimeInfo
{
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }

    public ScheduledTimeInfo(DateTime startTime, DateTime endTime)
    {
        StartTime = startTime;
        EndTime = endTime;
    }
}
