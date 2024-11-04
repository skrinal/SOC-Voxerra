using System.Collections.ObjectModel;
using System.Web;
using Voxerra.Models;
using Voxerra.Services.MessageCenter;

namespace Voxerra.ViewModels
{
    public class MessageCenterPageViewModel : INotifyPropertyChanged, IQueryAttributable
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private User userInfo;
        private ObservableCollection<User> userFriends;
        private ObservableCollection<LastestMessage> lastestMessage;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public MessageCenterPageViewModel() 
        {
            UserInfo = new User();
            UserFriends = new ObservableCollection<User>();
            LastestMessages = new ObservableCollection<LastestMessage>();

        }

        async Task GetListFriends()
        {
            var response = await ServiceProvider.GetInstance().CallWebApi<int, MessageCenterInitializeResponse>
                ("/MessageCenter/Initialize", HttpMethod.Post, UserInfo.Id);
           
            if (response.StatusCode == 200)
            {
                UserInfo = response.User;
                UserFriends = new ObservableCollection<User>(response.UserFriends);
                LastestMessages = new ObservableCollection<LastestMessage>(response.LastestMessages);
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
            Task.Run(async () =>
            {
                await GetListFriends();
            });

        }

        //public async void ApplyQueryAttributes(IDictionary<string, object> query)
        //{
        //    try
        //    {
        //        // Check if query or userId is missing or empty
        //        if (query == null || query.Count == 0 || !query.ContainsKey("userId")) return;


        //        // Check for null or empty value for userId
        //        var userIdValue = query["userId"]?.ToString();
        //        if (string.IsNullOrWhiteSpace(userIdValue)) return;

        //        // Parse and assign the userId
        //        UserInfo.Id = int.Parse(HttpUtility.UrlDecode(userIdValue));
        //    }
        //    catch (Exception ex)
        //    {
        //        // Display the error message
        //        await AppShell.Current.DisplayAlert("Voxerra", ex.Message, "OK");
        //    }
        //}


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

        public ObservableCollection<LastestMessage> LastestMessages
        {
            get { return lastestMessage; }
            set { lastestMessage = value; OnPropertyChanged(); }
        }

    }
}
