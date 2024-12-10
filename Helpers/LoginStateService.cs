using Microsoft.Maui.Storage; // Required for Preferences

namespace Voxerra.Helpers
{
    public class LoginStateService : ILoginStateService
    {
        private const string LoginStatusKey = "IsLoggedIn";
        public bool IsLoggedIn
        {
            get
            {
                Preferences.Clear();
                return Preferences.Get(LoginStatusKey, false);
            }
            set
            {
                // Save the login state to Preferences
                Preferences.Set(LoginStatusKey, value);
            }
        }

        public void Logout()
        {
            IsLoggedIn = false;
        }
    }
}
