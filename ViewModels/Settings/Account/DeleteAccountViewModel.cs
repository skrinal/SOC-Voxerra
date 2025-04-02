namespace Voxerra.ViewModels.Settings.Account;

public class DeleteAccountViewModel : INotifyPropertyChanged, IQueryAttributable {
    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query == null || query.Count == 0) return;

        userId = _dataCenterService.UserInfo.Id;
        UserName = HttpUtility.UrlDecode(query["UserName"].ToString());
        Bio = HttpUtility.UrlDecode(query["Bio"].ToString());
        AvatarSourceName = HttpUtility.UrlDecode(query["AvatarName"].ToString());
        
    }
    public event PropertyChangedEventHandler? PropertyChanged;
    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
    private ServiceProvider _serviceProvider;
    private DataCenterService _dataCenterService;
    public DeleteAccountViewModel(ServiceProvider serviceProvider, DataCenterService dataCenterService)
    {
        ButtonStatus = false;
        _serviceProvider = serviceProvider;
        _dataCenterService = dataCenterService;
        
        DeleteAccountCommand = new Command(() =>
        {
            if (IsRefreshing) return;
            
            if (!isChecked) return;
            
            IsRefreshing = true;
            DeleteAccount().GetAwaiter().OnCompleted(() =>
            {
                IsRefreshing = false;
            });
        });
        GoBackCommand = new Command(OnGoBack);
    }
    
    private async Task DeleteAccount()
    {
        try
        {
            var response = await _serviceProvider.CallWebApi<int, BaseResponse>(
                "/UserSettings/DeleteAccount", HttpMethod.Post, userId);
            if (response.StatusCode == 200)
            {
                _dataCenterService.Reset();
                await Shell.Current.GoToAsync($"//LoginPage");
            }
            else
            {
                AnswerText = "Account couldn't be deleted";
                AnswerColor = "Red";
            }
        }
        catch (Exception ex)
        {
            await AppShell.Current.DisplayAlert("Voxerra", ex.Message, "OK");
        }
    }
    
    private async void OnGoBack()
    {
        await Shell.Current.GoToAsync($"AccountDetailsPage");
    }
    
    private bool isRefreshing;
    private int userId;

    private string userName;
    private string bio;
    private string avatarSourceName;
    
    private string answerColor;
    private string answerText;
    
    private bool isChecked;

    private bool buttonStatus;
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
    public string Bio
    {
        get { return bio; }
        set { bio = value; OnPropertyChanged(); }
    }
    public string AvatarSourceName
    {
        get { return avatarSourceName; }
        set { avatarSourceName = value; OnPropertyChanged(); }
    }
    
    
    
    public string AnswerColor
    {
        get { return answerColor; }
        set { answerColor = value; OnPropertyChanged(); }
    }
    public string AnswerText
    {
        get { return answerText; }
        set { answerText = value; OnPropertyChanged(); }
    }
    
    public bool ButtonStatus
    {
        get { return buttonStatus; }
        set { buttonStatus = value; OnPropertyChanged(); }
    }
    
    
    public bool IsChecked
    {
        get { return isChecked; }
        set
        {
            isChecked = value;
            OnPropertyChanged();
            ButtonStatus = isChecked;
        }
    }
    
    public ICommand DeleteAccountCommand { get; set; }
    public ICommand ReallyWantToDeleteCommand { get; set; }
    public ICommand GoBackCommand { get; set; }
}