namespace Voxerra.ViewModels
{
    public class AccountDetailsViewModel : INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private DataCenterService _dataCenterService;
        public AccountDetailsViewModel(DataCenterService dataCenterService)
        {
            _dataCenterService = dataCenterService;

            GoBackCommand = new Command(OnGoBack);
        }


        public void GetUserData()
        {
            userName = _dataCenterService.UserInfo.UserName;
            avatarSourceName = _dataCenterService.UserInfo.AvatarSourceName;
            bio = string.IsNullOrWhiteSpace(_dataCenterService.UserInfo.Bio) ? "No bio" : _dataCenterService.UserInfo.Bio;
        }

        public void Initialize()
        {
            Task.Run(async () =>
            {
                IsProcessing = true;
                GetUserData();
            }).GetAwaiter().OnCompleted(() =>
            {
                IsProcessing = false;
                OnPropertyChanged(nameof(UserName));
                OnPropertyChanged(nameof(AvatarSourceName));
                OnPropertyChanged(nameof(Bio));
            });
        }

        private async void OnGoBack()
        {
            await Shell.Current.GoToAsync("MainSettingPage");
        }

        private bool isProcessing;
        private string userName;
        private string avatarSourceName;
        private string bio;
        public bool IsProcessing
        {
            get { return isProcessing; }
            set { isProcessing = value; OnPropertyChanged(); }
        }
        public string UserName
        {
            get { return userName; }
            set { userName = value; OnPropertyChanged(); }
        }
        public string AvatarSourceName
        {
            get { return avatarSourceName; }
            set { avatarSourceName = value; OnPropertyChanged(); }
        }
        public string Bio
        {
            get { return bio; }
            set { bio = value; OnPropertyChanged(); }
        }

        public ICommand GoBackCommand { get; set; }
    }
}