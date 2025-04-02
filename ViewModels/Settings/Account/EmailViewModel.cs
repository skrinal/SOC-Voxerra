using System.Text.RegularExpressions;
using Voxerra.Services.Settings;

namespace Voxerra.ViewModels.Settings.Account;

public class EmailViewModel : INotifyPropertyChanged, IQueryAttributable
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

    private DataCenterService _dataCenterService;
    private ServiceProvider _serviceProvider;

    public EmailViewModel(DataCenterService dataCenterService, ServiceProvider serviceProvider)
    {
        _dataCenterService = dataCenterService;
        _serviceProvider = serviceProvider;
        
        UpdateEmailCommand = new Command(() =>
        {
            if (IsRefreshing) return;

            if (!isEmailUnique) return;

            IsRefreshing = true;

            UpdateEmail().GetAwaiter().OnCompleted(() =>
            {
                IsRefreshing = false;
            });
        });
        
        GoBackCommand = new Command(OnGoBack);
    }
    private async void OnGoBack()
    {
        LabelIcon = "";
        LabelColor = "Transparent";
        AnswerText = "";
        AnswerColor = "Transparent";
        isEmailUnique = false;
        ButtonStatus = false;
        await Shell.Current.GoToAsync("MainSettingPage");
    }
    
    private async void Initialize()
    {
        IsRefreshing = true;
        await GetEmail(); 
        IsRefreshing = false;
    }



    private async Task GetEmail()
    {
        try
        {
            var response = await _serviceProvider.CallWebApi<int, EmailResponse>(
                "/UserSettings/ReturnEmail", HttpMethod.Post, UserId);

            if (response.StatusCode == 200)
            {
                email = response.Email;
                CurrentEmail = Email;
                OnPropertyChanged(nameof(Email));
            }
        }
        catch (Exception ex)
        {
            await AppShell.Current.DisplayAlert("Voxerra", ex.Message, "OK");
        }
    }
    
    private async Task UpdateEmail()
    {
        try
        {
            var request = new UserEmailChangeRequest
            {
                UserId = UserId,
                NewEmail = email
            };
            var response = await _serviceProvider.CallWebApi<UserEmailChangeRequest, BaseResponse>(
                "/UserSettings/ChangeEmail", HttpMethod.Post, request);
            if (response.StatusCode == 200)
            {
                LabelIcon = "check";
                LabelColor = "Green";
                
                AnswerText = "Email changed successfully";
                AnswerColor = "Green";
                
                isEmailUnique = false;
                ButtonStatus = false;
                CurrentEmail = email;
            }
            else
            {
                LabelIcon = "close";
                LabelColor = "Red";
                
                AnswerText = "Email failed to change";
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
    
    private async Task IsEmailUniqueCall(string emailQueary)
    {
        try
        {
            var request = new IsEmailUniqueRequest
            {
                Email = emailQueary
            };

            var response = await _serviceProvider.CallWebApi<IsEmailUniqueRequest, BaseResponse>(
                "/Registration/IsEmailUnique", HttpMethod.Post, request);


            if (response.StatusCode == 200)
            {
                LabelIcon = "check";
                LabelColor = "Green";
                
                AnswerText = "Email not in use";
                AnswerColor = "Green";
                
                isEmailUnique = true;
                ButtonStatus = true;
            }
            else
            {
                LabelIcon = "close";
                LabelColor = "Red";
                
                AnswerText = "Email already in use";
                AnswerColor = "Red";
                
                isEmailUnique = false;
                ButtonStatus = false;
            }
        }
        catch (Exception ex)
        {
            isEmailUnique = false;
            await AppShell.Current.DisplayAlert("Voxerra", ex.Message, "OK");
        }
    }
    private bool IsValidEmail(string email)
    {
        if (email == CurrentEmail) return false;
        
        if (string.IsNullOrWhiteSpace(email)) return false;
        return !Regex.IsMatch(email,
            @"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.IgnoreCase);
    }
    
    private CancellationTokenSource _cts;
    public async void DebounceEmail(string query)
    {
        _cts?.Cancel();
        _cts = new CancellationTokenSource();
        try
        {
            await Task.Delay(800, _cts.Token);
                

            if (string.IsNullOrWhiteSpace(query)) return; //set mesage


            IsRefreshing = true;
            await IsEmailUniqueCall(query);
            IsRefreshing = false;
        }
        catch (TaskCanceledException) { }
    }

    private int UserId;
    private bool isEmailUnique;
    private string CurrentEmail;
    
    private bool isRefreshing;
    private bool buttonStatus;
    private string email;


    private string labelIcon;
    private string labelColor;
    
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
            if (IsValidEmail(value))
            {
                DebounceEmail(value);
            }
            else
            {
                isEmailUnique = false;
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

    
    public ICommand UpdateEmailCommand { get; set; }
    public ICommand GoBackCommand { get; set; }
}