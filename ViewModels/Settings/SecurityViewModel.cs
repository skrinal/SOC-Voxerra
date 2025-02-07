namespace Voxerra.ViewModels.Settings
{
    public class SecurityViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private DataCenterService _dataCenterService;

        public SecurityViewModel(DataCenterService dataCenterService)
        {
            _dataCenterService = dataCenterService;
            GoToDecisionCommand = new Command<string>(async (decision) =>
            {
                IsProcessing = true;
                switch (decision)
                {
                    case "Password":
                        await Shell.Current.GoToAsync($"PasswordPage?UserId={_dataCenterService.UserInfo.Id}");
                        break;
                    case "TwoFactor":
                        await Shell.Current.GoToAsync($"TwoFactorAuthPage?UserId={_dataCenterService.UserInfo.Id}"); // treba apicall na email
                        break;
                    case "WhereLogged":
                        //await Shell.Current.GoToAsync($"WhereIsUserLoggedPage?UserId={UserId}&Bio={bio}");
                        break;
                    case "LoginAlerts":
                        //await Shell.Current.GoToAsync($"LoginAlertsPage?Id={UserId}");
                        break;
                    default:
                        break;
                }
            });
            GoBackCommand = new Command(OnGoBack);
        }

        private async void OnGoBack()
        {
            await Shell.Current.GoToAsync($"MainSettingPage");
        }

        private bool isProcessing;

        public bool IsProcessing
        {
            get { return isProcessing; }
            set
            {
                isProcessing = value;
                OnPropertyChanged();
            }
        }


        public ICommand GoBackCommand { get; set; }
        public ICommand GoToDecisionCommand { get; set; }
    }
}