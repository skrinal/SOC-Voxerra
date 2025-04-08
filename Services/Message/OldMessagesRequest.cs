namespace Voxerra.Services.Message;

public class OldMessagesRequest
{
    public int ToUserId { get; set; }
    public int LastMessageId { get; set; }
}