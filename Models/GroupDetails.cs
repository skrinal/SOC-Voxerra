namespace Voxerra.Models;

public class GroupDetails
{
    public int GroupId { get; set; }
    public string GroupName { get; set; }
    public string AvatarSourceName  { get; set; }
    public int CreatedBy { get; set; }
    public string LastMessage { get; set; }
    public DateTime CreatedAt { get; set; }
    public int MembersCount { get; set; } // Number of members in the group
}