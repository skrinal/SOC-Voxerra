namespace Voxerra.Services.Settings;

public class UpdatePasswordChangeRequest
{
    public int UserId { get; set; }
    public string NewPassword { get; set; }
}