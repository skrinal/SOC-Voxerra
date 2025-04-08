using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Voxerra.ViewModels
{
    public class FriendRequestViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public ServiceProvider _serviceProvider;
        public DataCenterService _dataCenterService;
        public FriendRequestViewModel(ServiceProvider serviceProvider, DataCenterService dataCenterService) 
        {
            _serviceProvider = serviceProvider;
            _dataCenterService = dataCenterService;

            FriendRequestList = new ObservableCollection<UserSearch>();

            AcceptRequestCommand = new Command<int>((param) =>
            {
                if (IsProcessing) return;

                IsProcessing = true;
                ResponseToRequest(true, param).GetAwaiter().OnCompleted(() =>
                {
                    IsProcessing = false;
                });
            });

            DeclineRequestCommand = new Command<int>((param) =>
            {
                if (IsProcessing) return;

                IsProcessing = true;
                ResponseToRequest(false, param).GetAwaiter().OnCompleted(() =>
                {
                    IsProcessing = false;
                });
            });
            
            GoBackCommand = new Command(OnGoBack);
        }

        private async Task ResponseToRequest(bool decision, int UserRequestFromId)
        {
            var request = new FriendDecisionRequest
            {
                UserRequestFromId = UserRequestFromId,
                Decision = decision
            };
            try
            {
                var response = await _serviceProvider.CallWebApi<FriendDecisionRequest, BaseResponse>
                    ("/FriendAdd/FriendRequestDecision", HttpMethod.Post, request);
                
                if (response.StatusCode == 200 && decision)
                    await AppShell.Current.DisplayAlert("Voxerra", "Friend has been added", "OK");
    
                else if (response.StatusCode == 200 && decision)
                    await AppShell.Current.DisplayAlert("Voxerra", "Friend request denied", "OK");
                
                else
                    await AppShell.Current.DisplayAlert("Voxerra", "Failed to process", "OK");

                
                var itemToRemove = FriendRequestList.FirstOrDefault(x => x.Id == UserRequestFromId);
                
                if (itemToRemove != null) FriendRequestList.Remove(itemToRemove);
            }
            catch (Exception e)
            {
                await AppShell.Current.DisplayAlert("Request Failed", e.Message, "OK");
                throw;
            }
        }
        
        public async void Initialize()
        {
            IsProcessing = true;

            await GetFriendsRequestList();
            OnPropertyChanged(nameof(FriendRequestList));

            IsProcessing = false;
        }

        private async Task GetFriendsRequestList()
        {
            try
            {
                var userId = _dataCenterService.UserInfo.Id;

                var response = await _serviceProvider.CallWebApi<int, FriendSearchResponse>
                    ("/FriendAdd/FriendRequestList", HttpMethod.Post, userId);

                if (response.StatusCode == 200)
                {
                    friendRequestList = new ObservableCollection<UserSearch>(response.Users);

                    OnPropertyChanged(nameof(FriendRequestList));
                }
                else
                {
                    await AppShell.Current.DisplayAlert("Friend Request error", response.StatusMessage, "OK");
                }

            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
                await AppShell.Current.DisplayAlert("Exception Friend Request", e.Message, "OK");
            }
        }
        
        public async void OnGoBack()
        {
            await Shell.Current.GoToAsync($"//ProfilePage");
        }
        
        private bool isProcessing;
        private ObservableCollection<UserSearch> friendRequestList;

        public bool IsProcessing
        {
            get { return isProcessing; }
            set { isProcessing = value; OnPropertyChanged(); }
        }
        public ObservableCollection<UserSearch> FriendRequestList
        {
            get { return friendRequestList; }
            set { friendRequestList = value; OnPropertyChanged(); }
        }

        //public ICommand UserProfilePageCommand { get; set; }
        public ICommand AcceptRequestCommand { get; set; }
        public ICommand DeclineRequestCommand { get; set; }
        
        public ICommand GoBackCommand { get; set; }
    }
}
