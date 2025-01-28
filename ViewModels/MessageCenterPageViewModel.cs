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
            
            UserInfo = _dataCenterService.UserInfo;
            UserFriends = _dataCenterService.UserFriends;
            LastestMessages = _dataCenterService.LastestMessages;


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
                await Shell.Current.GoToAsync($"//ChatPage?fromUserId={UserInfo.Id}&toUserId={param}");
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
                await Shell.Current.GoToAsync("//AddFriendPage");
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
                UserInfo = response.User;
                UserFriends = new ObservableCollection<User>(response.UserFriends);
                LastestMessages =  new ObservableCollection<LastestMessage>(response.LastestMessages);
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

        public void Initialize()
        {
            Task.Run(async () =>
            {
                IsProcessing = true;
                await GetListFriends();
            }).GetAwaiter().OnCompleted(() =>
            {
                IsProcessing = false;
            });
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
                var lastestMessage = LastestMessages.FirstOrDefault(x => x.UserFiendInfo.Id == fromUserId);
                if (lastestMessage != null)
                {
                    //LastestMessages.Remove(lastestMessage);
                    lastestMessage.Content = message;
                    OnPropertyChanged("LastestMessages");
                }
                else
                {
                    var newLastestMessage = new LastestMessage
                    {
                        UserId = userInfo.Id,
                        Content = message,
                        UserFiendInfo = UserFriends.Where(x => x.Id == fromUserId).FirstOrDefault()
                    };

                    LastestMessages.Insert(0, newLastestMessage);
                    OnPropertyChanged(nameof(LastestMessages));
                }
            });
            
        }

        //void OnReceivedMessage(int fromUserId, string message)
        //{
        //    MainThread.BeginInvokeOnMainThread(() =>
        //    {
        //        var lastestMessage = LastestMessages.FirstOrDefault(x => x.UserId == fromUserId);

        //        if (lastestMessage != null)
        //        {
        //            lastestMessage.Content = message;
        //            OnPropertyChanged(nameof(LastestMessages));
        //        }
        //        else
        //        {
        //            var newLastestMessage = new LastestMessage
        //            {
        //                UserId = fromUserId,
        //                Content = message,
        //                UserFiendInfo = UserFriends.FirstOrDefault(x => x.Id == fromUserId)
        //            };

        //            LastestMessages.Insert(0, newLastestMessage);
        //            OnPropertyChanged(nameof(LastestMessages));
        //        }
        //    });
        //}


        private User userInfo;
        private ObservableCollection<User> userFriends;
        private ObservableCollection<LastestMessage> latestMessages;
        private bool isProcessing;

        /*public User UserInfo
        {
            get { return userInfo; }
            set { userInfo = value; OnPropertyChanged(); }
        }
        public ObservableCollection<User> UserFriends
        {
            get { return userFriends; }
            set { userFriends = value; OnPropertyChanged(); }
        }

        public ObservableCollection<LastestMessage> LastestMessages
        {
            get { return latestMessages; }
            set { latestMessages = value; OnPropertyChanged(); }
        }*/
        
        public User UserInfo
        {
            get => _dataCenterService.UserInfo;
            set
            {
                _dataCenterService.UserInfo = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<User> UserFriends
        {
            get => _dataCenterService.UserFriends;
            set
            {
                _dataCenterService.UserFriends = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<LastestMessage> LastestMessages
        {
            get => _dataCenterService.LastestMessages;
            set
            {
                _dataCenterService.LastestMessages = value;
                OnPropertyChanged();
            }
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
