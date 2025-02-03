namespace Voxerra.ViewModels.Settings
{
    public class NameViewModel : INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private DataCenterService _dataCenterService;
        public NameViewModel(DataCenterService dataCenterService)
        {
            _dataCenterService = dataCenterService;

            ChangeNameCommand = new Command(() =>
            {
                if (IsProcessing) return;

                if (!IsUserNameUnique) return;

                IsProcessing = true;
                XXX().GetAwaiter().OnCompleted(() =>
                {
                    IsProcessing = false;
                });
            });

            GoBackCommand = new Command(OnGoBack);
        }


        public void GetUserName()
        {
            userName = _dataCenterService.UserInfo.UserName;
        }

        public void Initialize()
        {
            Task.Run(async () =>
            {
                IsProcessing = true;
                GetUserName();
            }).GetAwaiter().OnCompleted(() =>
            {
                IsProcessing = false;
                OnPropertyChanged(nameof(UserName));

            });
        }




        private CancellationTokenSource _cts;
        public async void DebounceUserName(string query)
        {
            _cts?.Cancel();
            _cts = new CancellationTokenSource();
            try
            {
                await Task.Delay(800, _cts.Token);
                

                if (string.IsNullOrWhiteSpace(query)) return; //set mesage


                IsProcessing = true;
                await GetUsersList(query);
                IsProcessing = false;
            }
            catch (TaskCanceledException) { }
        }

        private async void OnGoBack()
        {
            await Shell.Current.GoToAsync("AccountDetailsPage");
        }

        private bool isProcessing;
        private string userName;


        public string EntryQueary
        {
            get => entryQueary;
            set
            {
                entryQueary = value;
                OnPropertyChanged();
                
                DebounceUserName(value);
            }
        }

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
    
        public ICommand ChangeNameCommand { get; set; }
        public ICommand GoBackCommand { get; set; }
    }   
}