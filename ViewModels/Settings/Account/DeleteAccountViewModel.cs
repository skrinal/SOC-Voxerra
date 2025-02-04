namespace Voxerra.ViewModels.Settings.Account;

public class DeleteAccountViewModel
{
    public DeleteAccountViewModel()
    {
        GoBackCommand = new Command(OnGoBack);
    }
    
    private async void OnGoBack()
    {
        await Shell.Current.GoToAsync($"AccountDetailsPage");
    }
    public ICommand GoBackCommand { get; set; }
}