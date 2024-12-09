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

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private ServiceProvider _serviceProvider;

        public RegisterPageViewModel(ServiceProvider serviceProvider)
        {
            LoginId = "SkrinalLogin";
            UserName = "Skirnal";
            Password = "Richard123";
            RePassword = "Richard123";

            RegisterCommand = new Command(() =>
            {
                if (isProcessing) return;

                if (string.IsNullOrWhiteSpace(UserName) || string.IsNullOrWhiteSpace(Email) 
                 || string.IsNullOrWhiteSpace(Password) || string.IsNullOrWhiteSpace(RePassword)) return;

                isProcessing = true;
                Register().GetAwaiter().OnCompleted(() =>
                {
                    isProcessing = false;
                });
            });
            _serviceProvider = serviceProvider;
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

                //IsEmailUnique = response.StatusCode == 200;

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

        private string loginId;
        private string userName;
        private string email;
        private string password;
        private string repassword;
        private bool isProcessing;
        private bool isEmailUnique;

        public string LoginId
        {
            get { return loginId; }
            set { loginId = value; OnPropertyChanged(); }
        }
        public string UserName
        {
            get { return userName; }
            set { userName = value; OnPropertyChanged(); }
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
                    Task task = IsEmailUniqueCall(Email);
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
        public ICommand RegisterCommand { get; set; }

    }
}
