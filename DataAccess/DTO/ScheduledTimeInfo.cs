using System;

namespace DataAccess.DTO;

/// Data transfer object for scheduled meeting time information
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
