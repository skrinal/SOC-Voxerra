namespace Voxerra.Services.UserSettings
{
    public class UserPasswordChangeRequest
    {
        public int UserId { get; set; }
        public string newPasword { get; set; }
    }
}