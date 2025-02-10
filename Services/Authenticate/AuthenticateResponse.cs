namespace Voxerra.Services.Authenticate
{
    public class AuthenticateResponse:BaseResponse
    {
        public int Id { get; set; }
        public string UserName { get; set; } = null!;
        public string Token { get; set; } = null!;
        public bool RequiresTwoFactorAuth { get; set; }
    }
}
