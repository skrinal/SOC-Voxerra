namespace Voxerra.ViewModels.Settings.Account;

public class BioViewModel : INotifyPropertyChanged, IQueryAttributable {
    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query == null || query.Count == 0) return;

        userId = int.Parse(HttpUtility.UrlDecode(query["UserId"].ToString()));
        Bio = HttpUtility.UrlDecode(query["Bio"].ToString());
        

    }
    public event PropertyChangedEventHandler? PropertyChanged;
    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    private DataCenterService _dataCenterService;
    private ServiceProvider _serviceProvider;
    public BioViewModel(ServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;

        UpdateBioCommand = new Command(() =>
        {
            if (IsRefreshing) return;

            if (!isValidBio) return;

            IsRefreshing = true;

            UpdateBio().GetAwaiter().OnCompleted(() =>
            {
                IsRefreshing = false;
            });
        });

        GoBackCommand = new Command(OnGoBack);
    }
    
    private async Task UpdateBio()
    {
        try
        {
            var request = new UserBioChangeRequest
            {
                UserId = userId,
                NewBio = Bio
            };
            var response = await _serviceProvider.CallWebApi<UserBioChangeRequest, BaseResponse>(
                "/UserSettings/ChangeBio", HttpMethod.Post, request);
            if (response.StatusCode == 200)
            {
                LabelIcon = "check";
                LabelColor = "Green";
                
                AnswerText = "Bio changed successfully";
                AnswerColor = "Green";
                
                isValidBio = false;
                ButtonStatus = false;
                CurrentBio = Bio;
            }
            else
            {
                LabelIcon = "close";
                LabelColor = "Red";
                
                AnswerText = "Bio failed to change";
                AnswerColor = "Red";
                
                isValidBio = false;
                ButtonStatus = false;
            }
        }
        catch (Exception ex)
        {
            await AppShell.Current.DisplayAlert("Voxerra", ex.Message, "OK");
        }
    }
    
    private async void OnGoBack()
    {
        LabelIcon = "";
        LabelColor = "Transparent";
        AnswerText = "";
        AnswerColor = "Transparent";
        isValidBio = false;
        ButtonStatus = false;

        await Shell.Current.GoToAsync($"AccountDetailsPage");
    }
    

    private bool IsValidBio(string Bio)
    {
        if (Bio == CurrentBio)
        {
            AnswerColor = "Transparent";
            AnswerText = "";
            CharacterCount = $"Character Count: 255 / {Bio.Length}";
            RuleColor1 = "White";
            ButtonStatus = false;
            isValidBio = false;
            return false;
        } 
            
        if (string.IsNullOrWhiteSpace(Bio))
        {
            Bio = "";
            CharacterCount = $"Character Count: 255 / {Bio.Length}";
            RuleColor1 = "White";
            ButtonStatus = true;
            isValidBio = true;
            return true;
        }
        if (Bio.Length > 255)
        {
            RuleColor1 = "Red";
            CharacterCount = $"Character Count: 255 / {Bio.Length}";
            ButtonStatus = false;
            isValidBio = false;
            return false;
        }
        
        CharacterCount = $"Character Count: 255 / {Bio.Length}";
        isValidBio = true;
        ButtonStatus = true;
        return true;
    }

    private bool isRefreshing;
    private int userId;
    private string bio;
    private string CurrentBio;
    private bool isValidBio;
    
    private bool buttonStatus;

    private string labelIcon;
    private string labelColor;
    
    private string ruleColor1;
    private string characterCount;
    
    private string answerColor;
    private string answerText;

    public bool IsRefreshing
    {
        get { return isRefreshing; }
        set { isRefreshing = value; OnPropertyChanged(); }
    }
    public string Bio
    {
        get { return bio; }
        set
        {
            bio = value;
            OnPropertyChanged();
            IsValidBio(value);
        }
    }

    public bool ButtonStatus
    {
        get { return buttonStatus; }
        set { buttonStatus = value; OnPropertyChanged(); }
    }
    public string LabelIcon
    {
        get { return labelIcon; }
        set { labelIcon = value; OnPropertyChanged(); }
    }
    public string LabelColor
    {
        get { return labelColor; }
        set { labelColor = value; OnPropertyChanged(); }
    }

    
    public string RuleColor1
    {
        get { return ruleColor1; }
        set { ruleColor1 = value; OnPropertyChanged(); }
    }
    public string CharacterCount
    {
        get { return characterCount; }
        set { characterCount = value; OnPropertyChanged(); }
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

    public ICommand GoBackCommand { get; set; }
    public ICommand UpdateBioCommand { get; set; }
    
    
}