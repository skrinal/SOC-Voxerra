namespace Voxerra.Services.Password
{
    public class PassRCRequest
    {
        public string NewPassword { get; set; } = null!;
        public string Email { get; set; } = null!;
        public int Code { get; set; }
    }
}
