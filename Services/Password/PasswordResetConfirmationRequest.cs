namespace Voxerra.Services.Password
{
    public class PasswordResetConfirmationRequest
    {
        public string Email { get; set; } = null!;
        public int Code { get; set; }
    }
}
