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
            Task.Run(async () =>
            {
                await GetMessages();
            }).GetAwaiter().OnCompleted(() =>
            {
            });
        }

        private ServiceProvider _serviceProvider;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public ChatPageViewModel(ServiceProvider serviceProvider)
        {
            Messages = new ObservableCollection<Message>();
            _serviceProvider = serviceProvider;
        }

        async Task GetMessages()
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
            }
            else
            {
                await AppShell.Current.DisplayAlert("Voxerra", response.StatusMessage, "OK");
            }
        }
        private int fromUserId;
        private int toUserId;
        private User friendInfo;
        private ObservableCollection<Message> messages;

        public int FromUserId
        {
            get { return fromUserId; }
            set { fromUserId = value; OnPropertyChanged(); }
        }

        public int ToUserId
        {
            get { return toUserId; }
            set { toUserId = value; OnPropertyChanged(); }
        }

        public User FriendInfo
        {
            get { return friendInfo; }
            set { friendInfo = value; OnPropertyChanged(); }
        }
        public ObservableCollection<Message> Messages
        {
            get { return messages; }
            set { messages = value; OnPropertyChanged(); }
        }
    }
    
}
