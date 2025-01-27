namespace Voxerra.ViewModels;

public class PublicProfileViewModel : INotifyPropertyChanged, IQueryAttributable
{
    public event PropertyChangedEventHandler PropertyChanged;
    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public PublicProfileViewModel()
    {
        GoBackCommand = new Command(OnGoBack);
    }
    
    private async void OnGoBack()
    {
        await Shell.Current.GoToAsync("//AddFriendPage");
        //await Shell.Current.GoToAsync("//LoginPage");
    }
    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query == null || query.Count == 0) return;

        PublicUserId = HttpUtility.UrlDecode(query["publicUserId"].ToString());

        //Initialize();
    }

    public string publicUserId;
    private bool isProcessing;
    
    public string PublicUserId
    {
        get { return publicUserId; }
        set { publicUserId = value; OnPropertyChanged(); }
    }
    public bool IsProcessing
    {
        get { return isProcessing; }
        set { isProcessing = value; OnPropertyChanged(); }
    }
    public ICommand OpenChatPageCommand { get; set; }
    public ICommand AddFriendPageCommand { get; set; }
    public ICommand GoBackCommand { get; set; }
}