namespace Voxerra.Services.FriendAdd;

public class FriendAddInitializeResponse : BaseResponse
{
    public IEnumerable<UserSearch> Users { get; set; }

}