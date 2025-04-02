using Voxerra.Services.Password;

namespace Voxerra.ViewModels;

public class PasswordConfirmationViewModel : INotifyPropertyChanged, IQueryAttributable
{
    public event PropertyChangedEventHandler PropertyChanged;
    private ServiceProvider _serviceProvider;

    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query == null || query.Count == 0) return;

        Email = HttpUtility.UrlDecode(query["email"].ToString());
    }

    public PasswordConfirmationViewModel(ServiceProvider serviceProvider, RegisterPageViewModel viewModel)
    {
        IsProcessing = false;
        ConfirmnCommand = new Command(() =>
        {
            if (IsProcessing) return;

            IsProcessing = true;
            ConfirmCodeCommand().GetAwaiter().OnCompleted(() => { isProcessing = false; });
        });

        SendNewCodeCommand = new Command(() =>
        {
            if (IsProcessing) return;
            if (isTimerRunning) return;

            StartTimer();

            IsProcessing = true;

            SendNewCode().GetAwaiter().OnCompleted(() => { isProcessing = false; });
        });

        GoBackCommand = new Command(OnGoBack);

        _serviceProvider = serviceProvider;
    }

    private async void StartTimer()
    {
        RemainingTime = 60;
        IsTimerRunning = true;

        while (RemainingTime > 0)
        {
            await Task.Delay(1000); // Wait 1 second
            RemainingTime--;
        }

        IsProcessing = false;
        IsTimerRunning = false;
    }

    public async Task SendNewCode()
    {
        try
        {
            var response = await _serviceProvider.CallWebApi<string, BaseResponse>
                ("/Password/SendNewCode", HttpMethod.Post, Email);

            if (response.StatusCode == 200)
            {
            }
            else
            {
                await AppShell.Current.DisplayAlert("Voxerra", response.StatusMessage, "OK");
            }
        }
        catch (Exception ex)
        {
            await AppShell.Current.DisplayAlert("Voxerra", ex.Message, "OK");
        }
    }

    public async Task ConfirmCodeCommand()
    {
        try
        {
            var request = new PassRCRequest
            {
                Email = Email,
                Code = RegistrationCode
            };
            var response = await _serviceProvider.CallWebApi<PassRCRequest, BaseResponse>
                ("/Registration/RPCodeValidation", HttpMethod.Post, request);

            if (response.StatusCode == 200)
            {
                ResetEntry();
                await Shell.Current.GoToAsync($"//LoginPage");
            }
            else
            {
                await AppShell.Current.DisplayAlert("Voxerra", response.StatusMessage, "OK");
            }
        }
        catch (Exception ex)
        {
            await AppShell.Current.DisplayAlert("Voxerra", ex.Message, "OK");
        }
    }

    private async void OnGoBack()
    {
        ResetEntry();
        await Shell.Current.GoToAsync($"//LoginPage"); // zle
    }

    private void ResetEntry()
    {
        Entry1 = "";
        Entry2 = "";
        Entry3 = "";
        Entry4 = "";
        Entry5 = "";
    }

    private int registrationCode;
    private bool isProcessing;


    private string entry1;
    private string entry2;
    private string entry3;
    private string entry4;
    private string entry5;
    private string email;


    private int remainingTime;
    private bool isTimerRunning;

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

    public string Email
    {
        get { return email; }
        set
        {
            email = value;
            OnPropertyChanged();
        }
    }

    public int RegistrationCode
    {
        get
        {
            int.TryParse($"{Entry1}{Entry2}{Entry3}{Entry4}{Entry5}", out int code);
            return code;
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

    public bool IsTimerRunning
    {
        get { return isTimerRunning; }
        set
        {
            isTimerRunning = value;
            OnPropertyChanged();
        }
    }

    public int RemainingTime
    {
        get { return remainingTime; }
        set
        {
            remainingTime = value;
            OnPropertyChanged();
        }
    }

    public ICommand ConfirmnCommand { get; set; }
    public ICommand SendNewCodeCommand { get; set; }
    public ICommand GoBackCommand { get; set; }
}