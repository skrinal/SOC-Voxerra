using System.Text.RegularExpressions;

namespace Voxerra.ViewModels
{
    public class RegisterPageViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private CancellationTokenSource _debounceTokenSource;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private ServiceProvider _serviceProvider;

        public RegisterPageViewModel(ServiceProvider serviceProvider)
        {

            RegisterCommand = new Command(() =>
            {
                if (IsProcessing) return;

                if (!IsUserNameUnique || !IsEmailUnique
                    || string.IsNullOrWhiteSpace(Password) || string.IsNullOrWhiteSpace(RePassword)) return;

                IsProcessing = true;
                Register().GetAwaiter().OnCompleted(() =>
                {
                    IsProcessing = false;
                });
            });

            GoBackCommand = new Command(OnGoBack);

            _serviceProvider = serviceProvider;
        }

        public void ClearEnteries()
        {
            userName = "";
            email = "";
            password = "";
            repassword = "";
            
            OnPropertyChanged(nameof(UserName));
            OnPropertyChanged(nameof(Email));
            OnPropertyChanged(nameof(Password));
            OnPropertyChanged(nameof(RePassword));
        }
        
        private async void OnGoBack()
        {
            await Shell.Current.GoToAsync("//LoginPage");
        }
        private async Task Register()
        {
            try
            {
                var request = new RegistrationInitializeRequest
                {
                    Username = UserName,
                    Password = Password,
                    Email = Email
                };
                var response = await _serviceProvider.CallWebApi<RegistrationInitializeRequest, BaseResponse>
                ("/Registration/RegisterUser", HttpMethod.Post, request);


                if (response.StatusCode == 200)
                {
                    await Shell.Current.GoToAsync($"RegisterConfirmationPage?email={Email}");

                    //OnGoBack();
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
        private bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email)) return false;
            return Regex.IsMatch(email,
                @"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.IgnoreCase);
        }
        private bool IsValidUsername(string username)
        {
            if (string.IsNullOrWhiteSpace(username) || username.Length < 3)
            {
                return false;
            }
            // (a-z or A-Z)
            // cislo moze byt po pismenku
            // bez medzier
            string pattern = @"^[A-Za-z][A-Za-z0-9]{2,}$";

            return Regex.IsMatch(username, pattern);
        }
        private async Task IsEmailUniqueCall(string email)
        {
            try
            {
                Email = email;

                var response = await _serviceProvider.CallWebApi<string, BaseResponse>(
                    "/Registration/IsEmailUnique", HttpMethod.Post, Email);


                if (response.StatusCode == 200)
                {
                    IsEmailUnique = true;
                }
                else 
                {
                    IsEmailUnique = false;
                    //await AppShell.Current.DisplayAlert("Voxerra", "Email is already in use.", "OK");
                }
            }
            catch (Exception ex)
            {
                IsEmailUnique = false;
                await AppShell.Current.DisplayAlert("Voxerra", ex.Message, "OK");
            }
        }
        private async Task IsUserNameUniqueCall(string userName)
        {
            try
            {
                var request = new IsUserNameUniqueRequest
                {
                    UserName = userName
                };

                var response = await _serviceProvider.CallWebApi<IsUserNameUniqueRequest, BaseResponse>(
                    "/Registration/IsUserNameUnique", HttpMethod.Post, request);


                if (response.StatusCode == 200)
                {
                    IsUserNameUnique = true;
                }
                else
                {
                    IsUserNameUnique = false;
                    //await AppShell.Current.DisplayAlert("Voxerra", "Email is already in use.", "OK");
                }
            }
            catch (Exception ex)
            {
                IsUserNameUnique = false;
                await AppShell.Current.DisplayAlert("Voxerra", ex.Message, "OK");
            }
        }
        private async void DebounceValidation(string input, Func<string, Task> validationFunc)
        {
            _debounceTokenSource?.Cancel();
            _debounceTokenSource = new CancellationTokenSource();
            var token = _debounceTokenSource.Token;

            try
            {
                await Task.Delay(1000, token);
                await validationFunc(input);
            }
            catch (TaskCanceledException)
            {
                // netreba nic lebo task bol cancelnuty
            }
        }

        private string userName;
        private string email;
        private string password;
        private string repassword;
        private bool isProcessing;
        private bool isEmailUnique;
        private bool isUserNameUnique;

        public string UserName
        {
            get { return userName; }
            set 
            { 
                userName = value; 
                OnPropertyChanged();
                if (IsValidUsername(value))
                {
                    DebounceValidation(value, IsUserNameUniqueCall);
                }
                else
                {
                    IsUserNameUnique = false;
                }
            }
        }
        public string Email
        {
            get { return email; }
            set 
            { 
                email = value; 
                OnPropertyChanged(); 

                if (IsValidEmail(value))
                {
                    DebounceValidation(value, IsEmailUniqueCall);
                }
                else
                {
                    IsEmailUnique = false;
                }
            }
        }
        public string Password
        {
            get { return password; }
            set { password = value; OnPropertyChanged(); }
        }
        public string RePassword
        {
            get { return repassword; }
            set { repassword = value; OnPropertyChanged(); }
        }
        public bool IsProcessing
        {
            get { return isProcessing; }
            set { isProcessing = value; OnPropertyChanged(); }
        }
        public bool IsEmailUnique
        {
            get { return isEmailUnique; }
            set { isEmailUnique = value; OnPropertyChanged(); }
        }
        public bool IsUserNameUnique
        {
            get { return isUserNameUnique; }
            set { isUserNameUnique = value; OnPropertyChanged(); }
        }
        public ICommand RegisterCommand { get; set; }
        public ICommand GoBackCommand { get; set; }

    }
}
