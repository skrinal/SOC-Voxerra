using System.Collections.ObjectModel;

namespace Voxerra.ViewModels;

public class PublicProfileViewModel : INotifyPropertyChanged, IQueryAttributable
{
    public event PropertyChangedEventHandler? PropertyChanged;
    private readonly DataCenterService _dataCenterService;
    private ServiceProvider _serviceProvider;
    
    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public async void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query == null || query.Count == 0) return;

        PublicUserId = int.Parse(HttpUtility.UrlDecode(query["publicUserId"].ToString()));

        var user = _dataCenterService.UserSearch.FirstOrDefault(x => x.Id == PublicUserId);
        
        var userProfile = await _serviceProvider.CallWebApi<int, UserPublicProfileResponse>
            ("/FriendAdd/PublicProfile", HttpMethod.Post, PublicUserId);
        
        if (user != null)
        {
            userName = user.UserName;
            avatarSourceName = user.AvatarSourceName;
            bio = string.IsNullOrWhiteSpace(userProfile.Bio) ? "No bio" : userProfile.Bio;
            friendsCount = userProfile.FriendsCount;
            isOnline = userProfile.IsOnline ? "Online" : "Offline";
            creationYear = $"Member since {userProfile.CreationDate}";
            if (userProfile.IsFriend)
            {
                AddFButtonT = "Friends";
                AddButtonS = false;
            }
            else if (userProfile.IsFriendRequest)
            {
                AddFButtonT = "Request sent";
                AddButtonS = false;
            }
            else
            {
                AddFButtonT = "Add Friend";
                AddButtonS = true;
            }
            
            OnPropertyChanged(nameof(UserName));
            OnPropertyChanged(nameof(AvatarSourceName));
            OnPropertyChanged(nameof(Bio));
            OnPropertyChanged(nameof(FriendsCount));
            OnPropertyChanged(nameof(IsOnline));
            OnPropertyChanged(nameof(CreationYear));
        }
        else
        {
            // Handle a case where the user with PublicUserId is not found
            UserProfilePublic = new ObservableCollection<UserPublicProfile>();
        }
    }
    
    public PublicProfileViewModel(DataCenterService dataCenterService, ServiceProvider serviceProvider)
    {
        _dataCenterService = dataCenterService;
        _serviceProvider = serviceProvider;
        AddFButtonT = "Add Friend";
        AddButtonS = true;
        
        SendFriendRequestCommand = new Command(() =>
        {
            if (IsProcessing) return;

            isProcessing = true;
            SendFriendRequest().GetAwaiter().OnCompleted(() =>
            {
                isProcessing = false;
            });

        });
        
        GoBackCommand = new Command(OnGoBack);
    }

    private async Task SendFriendRequest()
    {
        try
        {
            var response = await _serviceProvider.CallWebApi<int, BaseResponse>
                ("/FriendAdd/FriendRequest", HttpMethod.Post, PublicUserId);

            if (response.StatusCode == 200)
            {
                AddFButtonT = "Request sent";
                AddButtonS = false;
                
                // TODO: Verify other user about request
                
            }
        }
        catch (Exception e)
        {
            await AppShell.Current.DisplayAlert("Voxerra", e.Message, "OK");
        }
    }
    
    private async void OnGoBack()
    {
        await Shell.Current.GoToAsync("..");
    }
    

    
    private ObservableCollection<UserPublicProfile> userProfilePublic;
    private int publicUserId;
    private bool isProcessing;
    private string userName;
    private string avatarSourceName;
    private string bio;
    private int friendsCount;
    private string isOnline;
    private string creationYear;
    private string addFButtonT;
    private bool addButtonS;
    
    public ObservableCollection<UserPublicProfile> UserProfilePublic
    {
        get { return userProfilePublic; }
        set { userProfilePublic = value; OnPropertyChanged();  }
    }
    
    public bool IsProcessing
    {
        get { return isProcessing; }
        set { isProcessing = value; OnPropertyChanged(); }
    }
    
    public int PublicUserId
    {
        get { return publicUserId; }
        set { publicUserId = value; OnPropertyChanged(); }
    }
    public string UserName
    {
        get { return userName;}
        set { userName = value; OnPropertyChanged(); }
    }
    public string AvatarSourceName
    {
        get { return avatarSourceName;}
        set { avatarSourceName = value; OnPropertyChanged(); }
    }
    public string Bio
    {
        get { return bio;}
        set { bio = value; OnPropertyChanged(); }
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
    public string CreationYear
    {
        get { return creationYear; }
        set { creationYear = value; OnPropertyChanged(); }
    }
    public string AddFButtonT
    {
        get { return addFButtonT; }
        set { addFButtonT = value; OnPropertyChanged(); }
    }
    public bool AddButtonS
    {
        get { return addButtonS; }
        set { addButtonS = value; OnPropertyChanged(); }
    }
    
    
    public ICommand SendFriendRequestCommand { get; set; }
    public ICommand GoBackCommand { get; set; }
}