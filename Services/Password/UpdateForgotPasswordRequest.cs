namespace Voxerra.Services.Password;

public class UpdateForgotPasswordRequest
{
    public string Email { get; set; } = null!;
    public string NewPassword { get; set; } = null!;
}