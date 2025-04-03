namespace Voxerra.ViewModels.Settings.Account;

public class EmailConfViewModel : INotifyPropertyChanged, IQueryAttributable
{
    // Make a XAML page for this

    public event PropertyChangedEventHandler? PropertyChanged;
    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query == null || query.Count == 0) return;

        Email = HttpUtility.UrlDecode(query["Email"].ToString());
        
    }
    private ServiceProvider _serviceProvider;

    public EmailConfViewModel(ServiceProvider serviceProvider)
    {
         _serviceProvider = serviceProvider;
        
        SendCodeCommand = new Command(() =>
        {
            if (IsRefreshing) return;

            IsRefreshing = true;

            SendCode().GetAwaiter().OnCompleted(() =>
            {
                IsRefreshing = false;
            });
        });

        SendNewCodeCommand = new Command(() =>
        {
            if (IsRefreshing) return;

            IsRefreshing = true;

            SendNewCode().GetAwaiter().OnCompleted(() =>
            {
                IsRefreshing = false;
            });
        });
    }

    private async Task SendCode()
    {
        try
        {
            var response = await _serviceProvider.CallWebApi<int, BaseResponse>(
                "/UserSettings/ChangeEmailConfirm", HttpMethod.Post, Code);

            if (response.StatusCode == 200)
            {
                await AppShell.Current.DisplayAlert("Voxerra", "Email changed successfully.", "OK");
                await Shell.Current.GoToAsync("MainSettingPage");
            }
            else
            {
                await AppShell.Current.DisplayAlert("Voxerra", "Something went wrong, try again later.", "OK");
                await Shell.Current.GoToAsync("MainSettingPage");
            }
        }
        catch (Exception ex)
        {
            await AppShell.Current.DisplayAlert("Voxerra", ex.Message, "OK");
        }
    }
    private async Task SendNewCode()
    {
        try
        {
            // Prazdny API call ???
            /*
            var response = await _serviceProvider.CallWebApi<int, BaseResponse>(
                "/UserSettings/ReturnEmail", HttpMethod.Post, UserId);

            if (response.StatusCode == 200)
            {
                email = response.Email;
                CurrentEmail = Email;
                OnPropertyChanged(nameof(Email));
            }
            */
        }
        catch (Exception ex)
        {
            await AppShell.Current.DisplayAlert("Voxerra", ex.Message, "OK");
        }
    }

    private bool isRefreshing;
    private string email;
    private int code;

    public bool IsRefreshing
    {
        get { return isRefreshing; }
        set { isRefreshing = value; OnPropertyChanged(); }
    }
    public string Email
    {
        get { return email; }
        set { email = value; OnPropertyChanged(); }
    }
    public int Code
    {
        get { return code; }
        set { code = value; OnPropertyChanged(); }
    }


    public ICommand SendCodeCommand { get; set;}
    public ICommand SendNewCodeCommand { get; set;}
}