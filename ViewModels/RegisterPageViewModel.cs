using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            RegisterCommand = new Command(() =>
            {
                if (isProcessing) return;

                if (UserName.Trim() == "" || Password.Trim() == "" || ) return;

                isProcessing = true;
                Login().GetAwaiter().OnCompleted(() =>
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
                var request = new RegistrationRequest
                {
                    Username = UserName,
                    Password = Password,
                    Email = Email
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
        private string email;
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
        public string Email
        {
            get { return email; }
            set { email = value; OnPropertyChanged(); }
        }
        public bool IsProcessing
        {
            get { return isProcessing; }
            set { isProcessing = value; OnPropertyChanged(); }
        }
        public ICommand RegisterCommand { get; set; }

    }
}
