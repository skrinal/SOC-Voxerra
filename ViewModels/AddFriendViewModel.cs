using System.Collections.ObjectModel;

namespace Voxerra.ViewModels;

public class AddFriendViewModel : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler PropertyChanged;
    private ServiceProvider _serviceProvider;
    
    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
    
    public AddFriendViewModel(ServiceProvider serviceProvide)
    {
        _serviceProvider = serviceProvide;
        
        UserSearchList = new ObservableCollection<UserSearch>();
        
        UserProfilePageCommand = new Command<int>(async (param) =>
        {
            await Shell.Current.GoToAsync($"//PublicProfilePage?publicUserId={param}");
        });

        GoBackCommand = new Command(OnGoBack);
    }

    private async Task GetUsersList(string query)
    {
        var request = new FriendSearchRequest
        {
            IdOfUser = 1, //////// ZLE ZLE  ZLE ZLE  ZLE ZLE  ZLE ZLE  ZLE ZLE  ZLE ZLE  ZLE ZLE 
            Search = query,
        };
        
        var response = await _serviceProvider.CallWebApi<FriendSearchRequest, FriendAddInitializeResponse>
            ("/FriendAdd/Search", HttpMethod.Post, request);
        
        if (response.StatusCode == 200)
        {
            UserSearchList = new ObservableCollection<UserSearch>(response.Users);
            
            OnPropertyChanged(nameof(UserSearchList));
        }
        else
        {
            await AppShell.Current.DisplayAlert("Voxerra", response.StatusMessage, "OK");
        }
    }
    private async void OnGoBack()
    {
        await Shell.Current.GoToAsync($"//MainPage");
        UserSearchList.Clear();
        EntryQueary = "";
    }
    
    private CancellationTokenSource _cts;
    
    /*private async void DebounceUserSearch(string input)
    {
        _cts?.Cancel();
        _cts = new CancellationTokenSource();

        try
        {
            await Task.Delay(500, _cts.Token);
            GetUsersList(input);
        }
        catch (TaskCanceledException) { }
    }*/
    
    public async void DebounceUserSearch(string query)
    {
        _cts?.Cancel();
        _cts = new CancellationTokenSource();
        try
        {
            await Task.Delay(800, _cts.Token);
            

            if (string.IsNullOrWhiteSpace(query))
            {
                UserSearchList.Clear(); // Clear the list
                
                return; // Exit the method if the condition is true
            }


            IsProcessing = true;
            await GetUsersList(query);
            IsProcessing = false;
        }
        catch (TaskCanceledException) { }
    }
    
    private ObservableCollection<UserSearch> userSearchList;
    private string entryQueary;
    private bool isProcessing;
    
    public ObservableCollection<UserSearch> UserSearchList
    {
        get { return userSearchList; }
        set { userSearchList = value; OnPropertyChanged();  }
    }
    
    public string EntryQueary
    {
        get => entryQueary;
        set
        {
            entryQueary = value;
            OnPropertyChanged();
            
            DebounceUserSearch(value);
        }
    }

    public bool IsProcessing
    {
        get { return isProcessing; }
        set { isProcessing = value; OnPropertyChanged(); }
    }
    
    public ICommand UserProfilePageCommand { get; set; }
    public ICommand GoBackCommand { get; set; }
}