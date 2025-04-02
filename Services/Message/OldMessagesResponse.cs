namespace Voxerra.Services.Message;

public class OldMessagesResponse : BaseResponse
{
    public IEnumerable<Models.Message> Messages { get; set; } = null!;
}