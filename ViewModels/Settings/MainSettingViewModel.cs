namespace Voxerra.ViewModels.Settings;

public class MainSettingViewModel : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;
    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    private DataCenterService _dataCenterService;
    private int UserId;
    public MainSettingViewModel(DataCenterService dataCenterService)
    {
        _dataCenterService = dataCenterService;
        
        AccountGoToCommand = new Command<string>(async (decision) =>
            {
                IsRefreshing = true;
                switch (decision)
                {
                    case "Name":
                        await Shell.Current.GoToAsync($"NamePage?UserName={userName}");
                        break;
                    case "Email":
                        await Shell.Current.GoToAsync($"EmailPage?UserId={UserId}"); 
                        break;
                    case "ProfilePicture":
                        await Shell.Current.GoToAsync($"ProfilePicturePage"); // NEFUNGUJE
                        break;
                    case "Bio":
                        await Shell.Current.GoToAsync($"BioPage?UserId={UserId}&Bio={bio}");
                        break;
                    case "DeleteAccount":
                        await Shell.Current.GoToAsync($"DeleteAccountPage?UserName={userName}&Bio={bio}&AvatarName={avatarSourceName}");
                        break;
                    default:
                        break;
                }
            });
            
            SecurityGoToCommand = new Command<string>(async (decision) =>
            {
                IsRefreshing = true;
                switch (decision)
                {
                    case "Password":
                        await Shell.Current.GoToAsync($"PasswordPage?UserId={_dataCenterService.UserInfo.Id}");
                        break;
                    case "TwoFactor":
                        await Shell.Current.GoToAsync($"TwoFactorAuthPage?UserId={_dataCenterService.UserInfo.Id}"); // treba apicall na email
                        break;
                    case "WhereLogged":
                        //await Shell.Current.GoToAsync($"WhereIsUserLoggedPage?UserId={UserId}&Bio={bio}");
                        break;
                    case "LoginAlerts":
                        //await Shell.Current.GoToAsync($"LoginAlertsPage?Id={UserId}");
                        break;
                    default:
                        break;
                }
            });
        
            SignOutCommand = new Command(() =>
            {
                if (IsRefreshing) return;
                
                IsRefreshing = true;

                /*UpdateBio().GetAwaiter().OnCompleted(() =>
                {
                    IsRefreshing = false;
                });*/
            });

            
        GoBackCommand = new Command(OnGoBack);
    }

 
    public void Initialize()
    {
        userName = _dataCenterService.UserInfo.UserName;
        avatarSourceName = _dataCenterService.UserInfo.AvatarSourceName;
        bio = string.IsNullOrWhiteSpace(_dataCenterService.UserInfo.Bio) ? "No bio" : _dataCenterService.UserInfo.Bio;
        
        OnPropertyChanged(nameof(UserName));
        OnPropertyChanged(nameof(AvatarSourceName));
        OnPropertyChanged(nameof(Bio));
    }

    private async void OnGoBack()
    {
        await Shell.Current.GoToAsync("//ProfilePage");
    }

    private bool isRefreshing;
    private string userName;
    private string avatarSourceName;
    private string bio;
    
    public bool IsRefreshing
    {
        get { return isRefreshing; }
        set { isRefreshing = value; OnPropertyChanged(); }
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
    public ICommand AccountGoToCommand  { get; set; }
    public ICommand SecurityGoToCommand  { get; set; }
    public ICommand SignOutCommand  { get; set; }
}