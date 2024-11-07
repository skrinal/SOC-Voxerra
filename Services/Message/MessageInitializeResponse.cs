namespace Voxerra.Services.Message
{
    public  class MessageInitializeResponse : BaseResponse
    {
        public User? FriendInfo { get; set; }
        public IEnumerable<Models.Message>? Messages { get; set; }
    }
}
