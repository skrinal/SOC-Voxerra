namespace Voxerra.Services.Authenticate;

public class TwoAuthRequest
{
    public int UserId { get; set; }
    public int Code { get; set; }
}