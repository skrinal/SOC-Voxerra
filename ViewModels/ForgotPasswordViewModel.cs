
using Voxerra.Services.Password;

namespace Voxerra.ViewModels
{
    public class ForgotPasswordViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private ServiceProvider _serviceProvider;
        public ForgotPasswordViewModel(ServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;

            ResetPasswordCommand = new Command(() =>
            {
                if (IsProcessing) return;

                if (string.IsNullOrWhiteSpace(Email) ) return;

                IsProcessing = true;
                ResetPassword().GetAwaiter().OnCompleted(() =>
                {
                    IsProcessing = false;
                });
            });

            RegisterPageCommand = new Command(RegisterPage);
            GoBackCommand = new Command(OnGoBack);

        }

        public async Task ResetPassword() 
        {
            try
            {

                var response = await _serviceProvider.CallWebApi<string, BaseResponse>
                ("/Password/ResetPassword", HttpMethod.Post, Email);


                if (response.StatusCode == 200)
                {
                    await Shell.Current.GoToAsync($"PasswordConfirmationPage?email={Email}");

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
        private async void OnGoBack()
        {
            await Shell.Current.GoToAsync($"..");
        }
        
        private async void RegisterPage()
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


        bool isProcessing;
        string email;
        public bool IsProcessing
        {
            get { return isProcessing; }
            set { isProcessing = value; OnPropertyChanged(); }
        }
        public string Email
        {
            get { return email; }
            set { email = value; OnPropertyChanged(); }
        }
        
        public ICommand ResetPasswordCommand { get; set; }
        public ICommand RegisterPageCommand { get; set; }
        public ICommand GoBackCommand { get; set; }
    }
}
