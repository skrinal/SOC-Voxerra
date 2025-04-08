namespace Voxerra.Services.GroupChat;

public class GroupMessagesResponse : BaseResponse
{
    public IEnumerable<User>? FriendsInfo { get; set; }
    public IEnumerable<GroupMessages>? Messages { get; set; }
}