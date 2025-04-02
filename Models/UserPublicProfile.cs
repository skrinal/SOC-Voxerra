namespace Voxerra.Models;

public class UserPublicProfile
{
    public string UserName { get; set; } = null!;
    public string AvatarSourceName { get; set; } = null!;
    public string Bio { get; set; } = null!;
    public int FriendsCount { get; set; }
    public string IsOnline { get; set; }
    public string CreationYear { get; set; }
}