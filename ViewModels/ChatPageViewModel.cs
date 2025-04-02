using System.Collections.ObjectModel;
using Voxerra.Services.Message;

namespace Voxerra.ViewModels
{
    public class ChatPageViewModel : INotifyPropertyChanged, IQueryAttributable
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (query == null || query.Count == 0) return;

            FromUserId = int.Parse(HttpUtility.UrlDecode(query["fromUserId"].ToString()));
            ToUserId = int.Parse(HttpUtility.UrlDecode(query["toUserId"].ToString()));

            Initialize();
        }

        private ServiceProvider _serviceProvider;
        private ChatHub _chatHub;
        private bool isLoadingOlderMessages = false;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public ChatPageViewModel(ServiceProvider serviceProvider, ChatHub chatHub)
        {
            Messages = new ObservableCollection<Message>();
            _serviceProvider = serviceProvider;
            _chatHub = chatHub;
            _chatHub.AddReceivedMessageHandler(OnReceiveMessage);
            _chatHub.Connect();

            
            LoadOlderMessagesCommand = new Command(async () => await GetOlderMessages());
            
            BackToHome = new Command(async () => { await Shell.Current.GoToAsync($"//MainPage"); });

            SendMessageCommand = new Command(async () =>
            {
                try
                {
                    if (Message.Trim() != "")
                    {
                        await _chatHub.SendMessageToUser(FromUserId, ToUserId, Message);

                        Messages.Add(new Message
                        {
                            Content = Message,
                            FromUserId = fromUserId,
                            ToUserId = toUserId,
                            SendDateTime = DateTime.Now,
                        });

                        Message = "";
                    }
                }
                catch (Exception ex)
                {
                    await AppShell.Current.DisplayAlert("Voxerra", ex.Message, "OK");
                }
            });
        }


        public async Task GetMessages()
        {
            var request = new MessageInitializeRequest
            {
                FromUserId = FromUserId,
                ToUserId = toUserId,
            };
            var response = await _serviceProvider.CallWebApi<MessageInitializeRequest, MessageInitializeResponse>
                ("/Message/Initialize", HttpMethod.Post, request);

            if (response.StatusCode == 200)
            {
                FriendInfo = response.FriendInfo;
                Messages = new ObservableCollection<Message>(response.Messages);
                //OnPropertyChanged(nameof(Messages));

                // MainThread.BeginInvokeOnMainThread(() =>
                // {
                //     MessagesCollectionView?.ScrollTo(Messages, position: ScrollToPosition.End, animate: false);
                // });
                
            }
            else
            {
                await AppShell.Current.DisplayAlert("Voxerra", response.StatusMessage, "OK");
            }
        }

        public async Task GetOlderMessages()
        {
            if (Messages.Count == 0) return;
            
            isLoadingOlderMessages = true;
            IsRefreshing = true;
            
            var olderMessages = Messages.FirstOrDefault();
            if (olderMessages == null) return;

            var request = new OldMessagesRequest
            {
                FromUserId = FromUserId,
                ToUserId = toUserId,
                LastMessageId = olderMessages.Id,
            };
            
            var response = await _serviceProvider.CallWebApi<OldMessagesRequest, OldMessagesResponse>
                ("/Message/OldMessages", HttpMethod.Post, request);

            if (response.StatusCode == 200 && response.Messages.Any())
            {
                var firstVisibleMessage = Messages.First();
                
                foreach (var msg in response.Messages)
                {
                    messages.Insert(0, msg);
                }
                
                OnPropertyChanged(nameof(Messages));
                
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    MessagesCollectionView?.ScrollTo(firstVisibleMessage, position: ScrollToPosition.Start, animate: false);
                });
                
                
            }
            isLoadingOlderMessages = false;
            IsRefreshing = false;
        }
        
        public void Initialize()
        {
            Task.Run(async () =>
            {
                IsRefreshing = true;
                await GetMessages();
            }).GetAwaiter().OnCompleted(() =>
            {
                IsRefreshing = false; 
            });
        }
        

        private void OnReceiveMessage(int fromUserId, string message)
        {
            Messages.Add(new Models.Message
            {
                Content = message,
                FromUserId = ToUserId,
                ToUserId = FromUserId,
                SendDateTime = DateTime.Now,
            });
        }

        private int fromUserId;
        private int toUserId;
        private User friendInfo;
        private ObservableCollection<Message> messages;
        private bool isRefreshing;
        private string message;
        
        private CollectionView messagesCollectionView;
        public CollectionView MessagesCollectionView
        {
            get => messagesCollectionView;
            set
            {
                messagesCollectionView = value;
                OnPropertyChanged();
            }
        }

        
        public int FromUserId
        {
            get { return fromUserId; }
            set
            {
                fromUserId = value;
                OnPropertyChanged();
            }
        }

        public int ToUserId
        {
            get { return toUserId; }
            set
            {
                toUserId = value;
                OnPropertyChanged();
            }
        }

        public User FriendInfo
        {
            get { return friendInfo; }
            set
            {
                friendInfo = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<Message> Messages
        {
            get { return messages; }
            set
            {
                messages = value;
                OnPropertyChanged();
            }
        }

        public bool IsRefreshing
        {
            get { return isRefreshing; }
            set
            {
                isRefreshing = value;
                OnPropertyChanged();
            }
        }

        public string Message
        {
            get { return message; }
            set
            {
                message = value;
                OnPropertyChanged();
            }
        }

        public ICommand SendMessageCommand { get; set; }
        public ICommand LoadOlderMessagesCommand { get; set; }
        public ICommand BackToHome { get; set; }
    }
}