using System;
using System.ComponentModel.DataAnnotations;

namespace Database;

public class UserModel
{
    [Key]
    public long Id { get; set; }
    [StringLength(100)]
    public string Name { get; set; } = string.Empty;
    public IList<MeetingModel> Meetings { get; set; } = new List<MeetingModel>();
}
