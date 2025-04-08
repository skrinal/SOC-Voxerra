namespace Voxerra.ViewModels;

public class TwoAuthViewModel : INotifyPropertyChanged, IQueryAttributable
{
    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query == null || query.Count == 0) return;

        UserId = int.Parse(HttpUtility.UrlDecode(query["userId"].ToString()));
    }

    private ServiceProvider _serviceProvider;

    public TwoAuthViewModel(ServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;


        SendCodeCommand = new Command(() =>
        {
            Task.Run(async () =>
            {
                IsProcessing = true;
                await SendCode();
            }).GetAwaiter().OnCompleted(() => { IsProcessing = false; });
        });
        GoBackCommand = new Command(OnGoBack);
    }

    private async Task SendCode()
    {
        var request = new TwoAuthRequest
        {
            UserId = UserId,
            Code = Code
        };
        
        var response = await _serviceProvider.CallWebApi<TwoAuthRequest, AuthenticateResponse>
            ("/Authenticate/TwoAuth", HttpMethod.Post, request);
           
        if (response.StatusCode == 200)
        {
            _serviceProvider._accesToken = response.Token;
            await Shell.Current.GoToAsync($"//MainPage?userId={response.Id}");
        }
        else
        {
            await AppShell.Current.DisplayAlert("Voxerra", response.StatusMessage, "OK");
        }
    }
    
    
    private async void OnGoBack()
    {
        await Shell.Current.GoToAsync("//LoginPage");
    }

    private int userId;
    private int code;
    private bool isProcessing;

    private string entry1;
    private string entry2;
    private string entry3;
    private string entry4;
    private string entry5;
    
    public string Entry1
    {
        get => entry1;
        set
        {
            entry1 = value;
            OnPropertyChanged();
        }
    }

    public string Entry2
    {
        get => entry2;
        set
        {
            entry2 = value;
            OnPropertyChanged();
        }
    }

    public string Entry3
    {
        get => entry3;
        set
        {
            entry3 = value;
            OnPropertyChanged();
        }
    }

    public string Entry4
    {
        get => entry4;
        set
        {
            entry4 = value;
            OnPropertyChanged();
        }
    }

    public string Entry5
    {
        get => entry5;
        set
        {
            entry5 = value;
            OnPropertyChanged();
        }
    }
    public int UserId
    {
        get { return userId; }
        set
        {
            userId = value;
            OnPropertyChanged();
        }
    }
    public int Code
    {
        get
        {
            int.TryParse($"{Entry1}{Entry2}{Entry3}{Entry4}{Entry5}", out int code);
            return code;
        }
        set
        {
            code = value;
            OnPropertyChanged();
        }
    }


    public bool IsProcessing
    {
        get { return isProcessing; }
        set
        {
            isProcessing = value;
            OnPropertyChanged();
        }
    }

    public ICommand SendCodeCommand { get; set; }
    public ICommand GoBackCommand { get; set; }
}