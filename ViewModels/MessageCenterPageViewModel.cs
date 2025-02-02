using System.Collections.ObjectModel;
namespace Voxerra.ViewModels
{
    public class MessageCenterPageViewModel : INotifyPropertyChanged, IQueryAttributable
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private ServiceProvider _serviceProvider;
        private ChatHub _chatHub;
        private DataCenterService _dataCenterService;

        public MessageCenterPageViewModel(
            ServiceProvider serviceProvider, 
            ChatHub chatHub,
            DataCenterService stateService) 
        {

            _dataCenterService = stateService;
            
            UserInfo = new User();
            UserFriends = new ObservableCollection<User>();
            LatestMessages = new ObservableCollection<LastestMessage>();


            RefreshCommand = new Command(() =>
            {
                Task.Run(async () =>
                {
                    IsProcessing = true;
                    await GetListFriends();
                }).GetAwaiter().OnCompleted(() =>
                {
                    IsProcessing = false;
                });
            });

            OpenChatPageCommand = new Command<int>(async (param) =>
            {
                await Shell.Current.GoToAsync($"ChatPage?fromUserId={UserInfo.Id}&toUserId={param}");
            });

            AddFriendPageCommand = new Command(() =>
            {
                if (IsProcessing) return;

                IsProcessing = true;
                AddFriendPageGTA().GetAwaiter().OnCompleted(() =>
                {
                    IsProcessing = false;
                });
            });
            
            _serviceProvider = serviceProvider;
            _chatHub = chatHub;
            _chatHub.Connect();
            _chatHub.AddReceivedMessageHandler(OnReceivedMessage);
           
        }

        async Task AddFriendPageGTA()
        {
            try
            {
                await Shell.Current.GoToAsync("AddFriendPage");
            }
            catch (Exception ex)
            {
                await AppShell.Current.DisplayAlert("Voxerra", ex.Message, "OK");
            }
        }
        async Task GetListFriends()
        {
            var response = await _serviceProvider.CallWebApi<int, MessageCenterInitializeResponse>
                ("/MessageCenter/Initialize", HttpMethod.Post, UserInfo.Id);
           
            if (response.StatusCode == 200)
            {
                userInfo = response.User;
                userFriends = new ObservableCollection<User>(response.UserFriends);
                latestMessages =  new ObservableCollection<LastestMessage>(response.LastestMessages);
                

                OnPropertyChanged(nameof(UserInfo));
                OnPropertyChanged(nameof(UserFriends));
                OnPropertyChanged(nameof(LatestMessages));
            }
            else
            {
                await AppShell.Current.DisplayAlert("Voxerra", response.StatusMessage, "OK");
         
            }
        }
        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (query == null || query.Count == 0) return;

            UserInfo.Id = int.Parse(HttpUtility.UrlDecode(query["userId"].ToString()));

            Initialize();
        }
        

        public async Task SynchronizeWithDataCenterAsync()
        {
            // Ensure data exists before synchronization
            if (UserInfo == null || UserFriends == null || LatestMessages == null)
            {
                await AppShell.Current.DisplayAlert("Error", "Data is not initialized for synchronization.", "OK");
                return;
            }

            // Perform synchronization on the main thread to avoid threading conflicts
            MainThread.BeginInvokeOnMainThread(() =>
            {
                _dataCenterService.UserInfo = UserInfo;
                _dataCenterService.UserFriends = new ObservableCollection<User>(UserFriends);
                _dataCenterService.LastestMessages = new ObservableCollection<LastestMessage>(LatestMessages);
            });

        }
        
        public void Initialize()
        {
            Task.Run(async () =>
            {
                IsProcessing = true;
                await GetListFriends();
                await SynchronizeWithDataCenterAsync();
            }).GetAwaiter().OnCompleted(() =>
            {
                IsProcessing = false;
                OnPropertyChanged(nameof(UserInfo));
                OnPropertyChanged(nameof(UserFriends));
                OnPropertyChanged(nameof(LatestMessages));
            });
 
            IsProcessing = false;
        }

        //void OnReceivedMessage(int fromUserId, string message)
        //{
        //    var lastestMessage = LastestMessages.Where(x => x.Id == fromUserId).FirstOrDefault();
        //    if (lastestMessage != null)
        //        LastestMessages.Remove(lastestMessage);

        //    var newLastestMessage = new LastestMessage
        //    {
        //        UserId = userInfo.Id,
        //        Content = message,
        //        UserFiendInfo = UserFriends.Where(x => x.Id == fromUserId).FirstOrDefault()
        //    };

        //    LastestMessages.Insert(0, newLastestMessage);
        //    OnPropertyChanged("LastestMessages");
        //}

        void OnReceivedMessage(int fromUserId, string message)
        {

            MainThread.BeginInvokeOnMainThread(() =>
            {
                var lastestMessage = LatestMessages.FirstOrDefault(x => x.UserFiendInfo.Id == fromUserId);
                if (lastestMessage != null)
                {
                    lastestMessage.Content = message;
                }
                else
                {
                    var userFriendInfo = UserFriends.FirstOrDefault(x => x.Id == fromUserId);
                    if (userFriendInfo != null)
                    {
                        var newLastestMessage = new LastestMessage
                        {
                            UserId = fromUserId,
                            Content = message,
                            UserFiendInfo = userFriendInfo
                        };
                        LatestMessages.Insert(0, newLastestMessage);
                    }
                }
            });
        }

        private User userInfo;
        private ObservableCollection<User> userFriends;
        private ObservableCollection<LastestMessage> latestMessages;
        private bool isProcessing;

        public User UserInfo
        {
            get { return userInfo; }
            set { userInfo = value; OnPropertyChanged(); }
        }
        public ObservableCollection<User> UserFriends
        {
            get { return userFriends; }
            set { userFriends = value; OnPropertyChanged(); }
        }

        public ObservableCollection<LastestMessage> LatestMessages
        {
            get { return latestMessages; }
            set { latestMessages = value; OnPropertyChanged(); }
        }



        public bool IsProcessing
        {
            get { return isProcessing; }
            set { isProcessing = value; OnPropertyChanged(); }
        }

        public ICommand RefreshCommand { get; set; }
        public ICommand OpenChatPageCommand { get; set; }
        public ICommand AddFriendPageCommand { get; set; }
    }
}
