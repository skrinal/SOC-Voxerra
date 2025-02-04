namespace Voxerra.ViewModels.Settings.Account;

public class ProfilePictureViewModel
{
    public ProfilePictureViewModel()
    {
        GoBackCommand = new Command(OnGoBack);
    }
    private async void OnGoBack()
    {
        await Shell.Current.GoToAsync($"AccountDetailsPage");
    }
    public ICommand GoBackCommand { get; set; }
}