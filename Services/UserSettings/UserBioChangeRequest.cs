namespace Voxerra.Services.Settings
{
    public class UserBioChangeRequest
    {
        public int UserId { get; set; }
        public string NewBio { get; set; } = null!;
    }
}