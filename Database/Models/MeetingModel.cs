using System;
using System.ComponentModel.DataAnnotations;

namespace Database;

public class MeetingModel
{
    [Key]
    public long Id { get; set; }
    public List<UserModel> Participants { get; set; } = new List<UserModel>();
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }   
}
