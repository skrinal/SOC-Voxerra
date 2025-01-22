namespace Voxerra.Services.Password
{
    public class PasswordResetConfirmationRequest
    {
        public string Email { get; set; } = null!;
        public int Token { get; set; }
    }
}
