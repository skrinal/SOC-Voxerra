namespace Voxerra.ViewModels.Settings.Security;

public class PasswordViewModel : INotifyPropertyChanged, IQueryAttributable
{
    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query == null || query.Count == 0) return;

        UserId = int.Parse(HttpUtility.UrlDecode(query["UserId"].ToString()));
        
        Initialize();
    }
    public event PropertyChangedEventHandler? PropertyChanged;
    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    private ServiceProvider _serviceProvider;
    private DataCenterService _dataCenterService;
    

    public PasswordViewModel(ServiceProvider serviceProvider, DataCenterService dataCenterService)
    {
        _serviceProvider = serviceProvider;
        _dataCenterService = dataCenterService;


    }

    private async Task UpdatePassword(string passwordChange)
    {
        try
        {
            var request = new UpdatePasswordChangeRequest
            {
                UserId = UserId,
                newPassword = passwordChange
            };
            var response = await _serviceProvider.CallWebApi<UpdatePasswordChangeRequest, BaseResponse>(
                "/UserSettings/ChangePassword", HttpMethod.Post, request);
            if (response.StatusCode == 200)
            {
                LabelIcon = "check";
                LabelColor = "Green";
                
                AnswerText = "Password changed successfully";
                AnswerColor = "Green";
                
                isEmailUnique = false;
                ButtonStatus = false;
                CurrentEmail = email;
            }
            else
            {
                LabelIcon = "close";
                LabelColor = "Red";
                
                AnswerText = "Password failed to change";
                AnswerColor = "Red";
                
                isEmailUnique = false;
                ButtonStatus = false;
            }
        }
        catch (Exception ex)
        {
            await AppShell.Current.DisplayAlert("Voxerra", ex.Message, "OK");
        }
    }


    private bool IsPasswordValid(string Password)
    {
        bool[] ruleBreak = new bool[4];
        // 0 - same password
        // 1 - min 5 max 18
        // 2 - uppercase / lower
        // 3 -   no space
        // 4 - password dont match

        AnswerColor = "Transparent";    
        AnswerText = "";
        RuleColor1 = "White";
        RuleColor2 = "White";
        RuleColor3 = "White";
        RuleColor4 = "White";

        if (Password == CurrentPassword && string.IsNulOrEmpty(RePassword)) return false; 
        
        if (Password == CurrentPassword) ruleBreak[0] = true;

        if (Password.Length < 5 || Password.Length > 18) ruleBreak[1] = true;

        if (!Password.Any(char.IsUpper) || !Password.Any(char.IsLower)) ruleBreak[2] = true;

        if (string.IsNullOrWhiteSpace(Password) || Password.Contains(" ")) ruleBreak[3] = true;

        if (Password != RePassword) ruleBreak[4] = true;

        if (ruleBreak.Any(x => x))
        {
            if (ruleBreak[0])
            {
                AnswerColor = "Red";
                AnswerText = "New password is same as your current."
            } 
            if (ruleBreak[1]) RuleColor1 = "Red";
            if (ruleBreak[2]) RuleColor2 = "Red";
            if (ruleBreak[3]) RuleColor3 = "Red";
            if (ruleBreak[4]) RuleColor4 = "Red";
            
            isPasswordValid = false;
            ButtonStatus = false;
            return false;
        }


        isPasswordValid = true;
        ButtonStatus = true;
        return true;
    }


    private int UserId;
    private bool isPasswordValid;
    private string CurrentEmail;
    
    private bool isRefreshing;
    private bool buttonStatus;
    private string email;


    private string labelIcon;
    private string labelColor;
    
    private string RuleColor0;
    private string ruleColor1;
    private string ruleColor2;
    private string ruleColor3;
    private string ruleColor4;

    private string answerColor;
    private string answerText;
    
    public bool IsRefreshing
    {
        get { return isRefreshing; }
        set { isRefreshing = value; OnPropertyChanged(); }
    }
    public string Email
    {
        get { return email; }
        set
        {
            email = value;
            OnPropertyChanged();
            if (IsPasswordValid(value))
            {
                UpdatePassword(value);
            }
            else
            {
                isPasswordValid = false;
            }
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


    public string RuleColor0
    {
        get { return ruleColor0; }
        set { ruleColor0 = value; OnPropertyChanged(); }
    }
    public string RuleColor1
    {
        get { return ruleColor1; }
        set { ruleColor1 = value; OnPropertyChanged(); }
    }
    public string RuleColor2
    {
        get { return ruleColor2; }
        set { ruleColor2 = value; OnPropertyChanged(); }
    }
    public string RuleColor3
    {
        get { return ruleColor3; }
        set { ruleColor3 = value; OnPropertyChanged(); }
    }
    public string RuleColor4
    {
        get { return ruleColor4; }
        set { ruleColor4 = value; OnPropertyChanged(); }
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

    
    public ICommand UpdatePasswordCommand { get; set; }
    public ICommand GoBackCommand { get; set; }
}