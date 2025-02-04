namespace Voxerra.ViewModels.Settings.Account;

public class EmailViewModel
{
    public EmailViewModel()
    {
        GoBackCommand = new Command(OnGoBack);
    }
    private async void OnGoBack()
    {
        await Shell.Current.GoToAsync($"AccountDetailsPage");
    }
    public ICommand GoBackCommand { get; set; }
}