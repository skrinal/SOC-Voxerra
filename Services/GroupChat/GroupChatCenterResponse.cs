namespace Voxerra.Services.GroupChat;

public class GroupChatCenterResponse:BaseResponse
{
    public IEnumerable<GroupDetails> DetailsOfGroups { get; set; } = new List<GroupDetails>();
}