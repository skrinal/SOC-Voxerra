namespace Voxerra.ViewModels.Settings.Account;

public class BioViewModel
{
    public BioViewModel()
    {
        GoBackCommand = new Command(OnGoBack);
    }
    
    private async void OnGoBack()
    {
        await Shell.Current.GoToAsync($"AccountDetailsPage");
    }
    public ICommand GoBackCommand { get; set; }
    
}