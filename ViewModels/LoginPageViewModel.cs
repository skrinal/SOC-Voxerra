
namespace Voxerra.ViewModels
{
    public class LoginPageViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        //private readonly ILoginStateService _loginStateService;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        
        private ServiceProvider _serviceProvider;

        public LoginPageViewModel(ServiceProvider serviceProvider)
        {

            UserName = "skrinal";
            Password = "risko";

            IsProcessing = false;

            LoginCommand = new Command(() =>
            {
                if (IsProcessing) return;

                if (UserName.Trim() == "" || Password.Trim() == "") return;

                IsProcessing = true;
                Login().GetAwaiter().OnCompleted(() =>
                {
                    IsProcessing = false;
                });
            });
            this._serviceProvider = serviceProvider;

            RegisterCommand = new Command(() =>
            {
                if (IsProcessing) return;

                IsProcessing = true;
                RegisterPage().GetAwaiter().OnCompleted(() =>
                {
                    IsProcessing = false;
                });
            });

            ForgotPassword = new Command(() =>
            {
                if (IsProcessing) return;

                IsProcessing = true;
                PasswordPage().GetAwaiter().OnCompleted(() =>
                {
                    IsProcessing = false;
                });
            });

        }


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
                if (response.StatusCode == 222)
                {
                    await Shell.Current.GoToAsync($"//MainPage?userId={response.Id}");
                }
                if (response.StatusCode == 200)
                {
                    //await Shell.Current.GoToAsync($"MessageCenterPage?userId={response.Id}");
                    await Shell.Current.GoToAsync($"//MainPage?userId={response.Id}");

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
                //await Shell.Current.GoToAsync($"RegisterConfirmationPage");
            }
            catch (Exception ex)
            {
                await AppShell.Current.DisplayAlert("Voxerra", ex.Message, "OK");
            }
        }
        async Task PasswordPage()
        {
            try
            {
                await Shell.Current.GoToAsync($"ForgotPasswordPage");
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
        public ICommand ForgotPassword { get; set; }
    }
}
