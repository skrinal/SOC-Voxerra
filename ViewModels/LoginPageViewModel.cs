
namespace Voxerra.ViewModels
{
    public class LoginPageViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private ServiceProvider _serviceProvider;
        //private readonly ILoginStateService _loginStateService;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        
        //private ServiceProvider _serviceProvider;

        public LoginPageViewModel(ServiceProvider serviceProvider/*, ILoginStateService loginStateService*/)
        {
            _serviceProvider = serviceProvider;
            //_loginStateService = loginStateService;

            //UserName = "wanda";
            //Password = "Abc12345";

            UserName = "skrinal";
            Password = "lucka123";

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

            RegisterCommand = new Command(() =>
            {
                if (isProcessing) return;

                isProcessing = true;
                RegisterPage().GetAwaiter().OnCompleted(() =>
                {
                    isProcessing = false;
                });
            });

            //LogoutCommand = new Command(Logout);
        }

        //void Logout()
        //{
        //    _loginStateService.Logout(); // Update login state
        //    Shell.Current.GoToAsync("//LoginPage");
        //}

        async Task Login()
        {
            try
            {
                var request = new AuthenticateRequest
                {
                    UserName = UserName,
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


                    //_loginStateService.IsLoggedIn = true;

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

        async Task RegisterPage()
        {
            try
            {
                await Shell.Current.GoToAsync($"RegisterPage");
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
        public ICommand RegisterCommand { get; set; }
        //public ICommand LogoutCommand { get; set; }
    }
}
