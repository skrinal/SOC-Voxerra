using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Voxerra.Services.Registration;

namespace Voxerra.ViewModels
{
    public class RegisterPageViewModel
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
            LoginId = "SkrinalLoginx";
            //UserName = "Skirnal";
            //Password = "Richard123";
            //RePassword = "Richard123";

            RegisterCommand = new Command(() =>
            {
                if (isProcessing) return;

                if (!IsUserNameUnique || !IsEmailUnique
                    || string.IsNullOrWhiteSpace(Password) || string.IsNullOrWhiteSpace(RePassword)) return;

                isProcessing = true;
                Register().GetAwaiter().OnCompleted(() =>
                {
                    isProcessing = false;
                });
            });

            GoBackCommand = new Command(OnGoBack);

            _serviceProvider = serviceProvider;
        }

        private async void OnGoBack()
        {
            await Shell.Current.GoToAsync("LoginPage");
        }
        async Task Register()
        {
            try
            {
                var request = new RegistrationInitializeRequest
                {
                    LoginId = loginId,
                    Username = UserName,
                    Password = Password,
                    Email = Email
                };
                var response = await _serviceProvider.CallWebApi<RegistrationInitializeRequest, RegistrationInitializeResponse>
                ("/Registration/RegisterUser", HttpMethod.Post, request);


                if (response.StatusCode == 200)
                {
                    if (response.Successful == true)
                    {
                        await Shell.Current.GoToAsync("LoginPage");
                    }
                    else
                    {
                        // error vuejbat
                    }
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
            return !string.IsNullOrWhiteSpace(username) && username.Length >= 3;
        }

        private async Task IsEmailUniqueCall(string email)
        {
            try
            {
                var request = new IsEmailUniqueRequest
                {
                    Email = email
                };

                var response = await _serviceProvider.CallWebApi<IsEmailUniqueRequest, IsEmailUniqueResponse>(
                    "/Registration/IsEmailUnique", HttpMethod.Post, request);


                if (response.StatusCode == 200)
                {
                    IsEmailUnique = response.IsEmailUnique;
                }
                else 
                {
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

                var response = await _serviceProvider.CallWebApi<IsUserNameUniqueRequest, IsUserNameUniqueResponse>(
                    "/Registration/IsUserNameUnique", HttpMethod.Post, request);


                if (response.StatusCode == 200)
                {
                    IsUserNameUnique = response.IsUserNameUnique;
                }
                else
                {
                    //await AppShell.Current.DisplayAlert("Voxerra", "Email is already in use.", "OK");
                }
            }
            catch (Exception ex)
            {
                IsEmailUnique = false;
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
                await Task.Delay(1000, token); // Wait 500ms before proceeding
                await validationFunc(input);
            }
            catch (TaskCanceledException)
            {
                // Task was canceled, no need to handle it
            }
        }

        private string loginId;
        private string userName;
        private string email;
        private string password;
        private string repassword;
        private bool isProcessing;
        private bool isEmailUnique;
        private bool isUserNameUnique;
        public string LoginId
        {
            get { return loginId; }
            set { loginId = value; OnPropertyChanged(); }
        }
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
