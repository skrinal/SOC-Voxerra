namespace Voxerra.Services.FriendAdd;

public class FriendSearchResponse : BaseResponse
{
    public IEnumerable<UserSearch> Users { get; set; }

}