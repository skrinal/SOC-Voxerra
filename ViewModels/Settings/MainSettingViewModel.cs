namespace Voxerra.ViewModels.Settings;

public class MainSettingViewModel : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;
    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    private DataCenterService _dataCenterService;

    public MainSettingViewModel(DataCenterService dataCenterService)
    {
        _dataCenterService = dataCenterService;

        AccountPageCommand = new Command(() =>
        {
            if (isProcessing) return;

            isProcessing = true;
            AccountDetailsPageGTA().GetAwaiter().OnCompleted(() =>
            {
                isProcessing = false;
            });
        });

        NotificationsPageCommand = new Command(() =>
        {
            if (isProcessing) return;


            isProcessing = true;
            NotificationsPageGTA().GetAwaiter().OnCompleted(() =>
            {
                isProcessing = false;
            });
        });

        SecurityPageCommand = new Command(() =>
        {
            if (IsProcessing) return;


            IsProcessing = true;
            SecurityPageGTA().GetAwaiter().OnCompleted(() =>
            {
                IsProcessing = false;
            });
        });

        GoBackCommand = new Command(OnGoBack);
    }



    async Task AccountDetailsPageGTA()
    {
        try
        {
            await Shell.Current.GoToAsync("AccountDetailsPage");
        }
        catch (Exception ex)
        {
            await AppShell.Current.DisplayAlert("Voxerra", ex.Message, "OK");
        }
    }
    async Task NotificationsPageGTA()
    {
        try
        {
            await Shell.Current.GoToAsync("NotificationsPage");
        }
        catch (Exception ex)
        {
            await AppShell.Current.DisplayAlert("Voxerra", ex.Message, "OK");
        }
    }
    async Task SecurityPageGTA()
    {
        try
        {
            await Shell.Current.GoToAsync("SecurityPage");
        }
        catch (Exception ex)
        {
            await AppShell.Current.DisplayAlert("Voxerra", ex.Message, "OK");
        }
    }
    /*async Task AddFriendPageGTA()
    {
        try
        {
            await Shell.Current.GoToAsync("AddFriendPage");
        }
        catch (Exception ex)
        {
            await AppShell.Current.DisplayAlert("Voxerra", ex.Message, "OK");
        }
    }*/

    public void Initialize()
    {
        userName = _dataCenterService.UserInfo.UserName;
        avatarSourceName = _dataCenterService.UserInfo.AvatarSourceName;

        OnPropertyChanged(nameof(UserName));
        OnPropertyChanged(nameof(AvatarSourceName));
    }

    private async void OnGoBack()
    {
        await Shell.Current.GoToAsync("//ProfilePage");
    }

    private bool isProcessing;
    private string userName;
    private string avatarSourceName;

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
    public string AvatarSourceName
    {
        get { return avatarSourceName; }
        set { avatarSourceName = value; OnPropertyChanged(); }
    }
    public ICommand GoBackCommand { get; set; }
    public ICommand AccountPageCommand { get; set; }
    public ICommand NotificationsPageCommand { get; set; }
    public ICommand SecurityPageCommand { get; set; }

}