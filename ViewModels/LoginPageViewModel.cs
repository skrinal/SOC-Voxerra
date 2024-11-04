
namespace Voxerra.ViewModels
{
    public class LoginPageViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
       
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        
        private ServiceProvider _serviceProvider;

        public LoginPageViewModel(ServiceProvider serviceProvider)
        {
            
            UserName = "wanda";
            Password = "Abc12345";
            isProcessing = false;

            LoginCommand = new Command(() =>
            {
                if (isProcessing) return;

                if (UserName.Trim() == "" || Password.Trim() == "") return;

                isProcessing = true;
                Login().GetAwaiter().OnCompleted(() =>
                {
                    isProcessing = false;
                });
            });
            this._serviceProvider = serviceProvider;
        }
        
        async Task Login()
        {
            try
            {
                var request = new AuthenticateRequest
                {
                    LoginId = UserName,
                    Password = Password,
                };
                var response = await _serviceProvider.Authenticate(request);
                if (response.StatusCode == 200)
                {
                    //    await AppShell.Current.DisplayAlert("Voxerra",
                    //        "Login sucessful! \n" +
                    //        $"Username: {response.UserName} \n" +
                    //        $"Token: {response.Token}", "OK");
                    //

                    await Shell.Current.GoToAsync($"MessageCenterPage?userId={response.Id}");
                }
                else
                {
                    await AppShell.Current.DisplayAlert("Voxerra", response.StatusMessage, "OK");
                }
            }
            catch (Exception ex)
            {
                await AppShell.Current.DisplayAlert("Voxerra", ex.Message, "OK");
            }
        }

        private string userName;
        private string password;
        private bool isProcessing;

        public string UserName
        {
            get { return userName; }
            set { userName = value; OnPropertyChanged(); }
        }
        public string Password
        {
            get { return password; }
            set { password = value; OnPropertyChanged(); }
        }
        public bool IsProcessing
        {
            get { return isProcessing; }
            set{ isProcessing = value; OnPropertyChanged(); }
        }
        public ICommand LoginCommand { get; set; }
    }
}
