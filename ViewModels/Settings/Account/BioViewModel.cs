namespace Voxerra.ViewModels.Settings.Account;

public class BioViewModel : INotifyPropertyChanged, IQueryAttributable {
    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query == null || query.Count == 0) return;

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
                UserId = UserId,
                NewBio = bio
            };
            var response = await _serviceProvider.CallWebApi<UserBioChangeRequest, BaseResponse>(
                "/UserSettings/ChangeBio", HttpMethod.Post, request);
            if (response.StatusCode == 200)
            {
                LabelIcon = "check";
                LabelColor = "Green";
                
                AnswerText = "Bio changed successfully";
                AnswerColor = "Green";
                
                isEmailUnique = false;
                ButtonStatus = false;
                CurrentBio = email;
            }
            else
            {
                LabelIcon = "close";
                LabelColor = "Red";
                
                AnswerText = "Bio failed to change";
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
    
    private CancellationTokenSource _cts;
    private bool IsValidBio(string Bio)
    {
        cts?.Cancel();
        _cts = new CancellationTokenSource();
        try
        {
            if(Bio == CurrentBio)
            {
                AnswerColor = "Transparent";
                AnswerText = ""
                ButtonStatus = false;
                isValidBio = false;
                return;
            } 
            await Task.Delay(800, _cts.Token);

            

            if (string.IsNullOrWhiteSpace(query))
            {
                Bio = "";
                AnswerColor = "Green";
                AnswerText = "Your Bio will be deleted"
                ButtonStatus = true;
                isValidBio = true;
                return true; //set mesage
            }
            if(Bio.Length >= 255)
            {
                AnswerColor = "Red";
                AnswerText = "Your Bio is way too long"
                ButtonStatus = false;
                isValidBio = false;
                return false;
            }

            isValidBio = true;
            ButtonStatus = true;
            return true;
        }
        catch (TaskCanceledException) { }
    }

    private bool isProcessing;
    private string bio;
    private bool isValidBio;

    private bool buttonStatus;

    private string labelIcon;
    private string labelColor;
    
    private string answerColor;
    private string answerText;

    public bool IsProcessing
        {
            get { return isProcessing; }
            set { isProcessing = value; OnPropertyChanged(); }
        }
    public string Bio
    {
        get { return bio; }
        set
        {
            bio = value;
            OnPropertyChanged();
            IsValidBio(value)
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