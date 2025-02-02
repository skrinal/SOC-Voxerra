using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Voxerra.ViewModels
{
    public class ProfilePageViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private readonly DataCenterService _dataCenterService;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public ProfilePageViewModel(DataCenterService dataCenterService)
        {
            _dataCenterService = dataCenterService;

            FriendRequestPageCommand = new Command(() =>
            {
                if (isProcessing) return;

                isProcessing = true;
                FriendRequestGTA().GetAwaiter().OnCompleted(() =>
                {
                    isProcessing = false;
                });

            });

            EditProfileCommand = new Command(() =>
            {
                if (isProcessing) return;

                isProcessing = true;
                EditProfileGTA().GetAwaiter().OnCompleted(() =>
                {
                    isProcessing = false;
                });

            });

        }

        public void Initialize()
        {
            userName = _dataCenterService.UserInfo.UserName;
            friendsCount = _dataCenterService.UserFriends.Count;
            isOnline = _dataCenterService.UserInfo.IsOnline ? "Online" : "Offline";
            bio = string.IsNullOrWhiteSpace(_dataCenterService.UserInfo.Bio) ? "No bio" : _dataCenterService.UserInfo.Bio;
            avatarSourceName = _dataCenterService.UserInfo.AvatarSourceName;
            creationYear = $"Member since {_dataCenterService.UserInfo.CreationYear}";
            
            OnPropertyChanged(nameof(UserName));
            OnPropertyChanged(nameof(FriendsCount));
            OnPropertyChanged(nameof(IsOnline));
            OnPropertyChanged(nameof(Bio));
            OnPropertyChanged(nameof(AvatarSourceName));
            OnPropertyChanged(nameof(CreationYear));
        }
        
        async Task FriendRequestGTA()
        {
            try
            {
                await Shell.Current.GoToAsync("FriendRequestPage"); 
            }
            catch (Exception ex)
            {
                await AppShell.Current.DisplayAlert("Voxerra", ex.Message, "OK");
            }
        }

        async Task EditProfileGTA()
        {
            try
            {
                await Shell.Current.GoToAsync("MainSettingPage"); //edit profile page
            }
            catch (Exception ex)
            {
                await AppShell.Current.DisplayAlert("Voxerra", ex.Message, "OK");
            }
        }


        private bool isProcessing;
        private string userName;
        private int friendsCount;
        private string isOnline;
        private string bio;
        private string avatarSourceName;
        private string creationYear;
        
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
        public int FriendsCount
        {
            get { return friendsCount; }
            set { friendsCount = value; OnPropertyChanged(); }
        }
        public string IsOnline
        {
            get { return isOnline; }
            set { isOnline = value; OnPropertyChanged(); }
        }
        public string Bio
        {
            get { return bio; }
            set { bio = value; OnPropertyChanged(); }
        }
        public string AvatarSourceName
        {
            get { return avatarSourceName; }
            set { avatarSourceName = value; OnPropertyChanged();}
        }
        public string CreationYear
        {
            get { return creationYear; }
            set { creationYear = value; OnPropertyChanged(); }
        }

        public ICommand EditProfileCommand { get; set; }
        public ICommand FriendRequestPageCommand { get; set; }
        public ICommand GoBackCommand { get; set; }

        

    }
}
