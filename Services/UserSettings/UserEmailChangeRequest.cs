namespace Voxerra.Services.Settings;

public class UserEmailChangeRequest
{
    public int UserId { get; set; }
    public string NewEmail { get; set; } = null!;
}