namespace Voxerra.Services.FriendAdd;

public class UserPublicProfileResponse : BaseResponse
{
    public string UserName { get; set; } = null!;
    public string AvatarSourceName { get; set; } = null!;
    public string Bio { get; set; } = null!;
    public int FriendsCount { get; set; }
    public bool IsOnline { get; set; }
    public string CreationDate { get; set; }
}