namespace Voxerra.ViewModels.Settings
{
    public class NotificationsViewModel
    {
        public NotificationsViewModel()
        {
            GoBackCommand = new Command(OnGoBack);
        }

        private async void OnGoBack()
        {
            await Shell.Current.GoToAsync("MainSettingPage");
        }
        public ICommand GoBackCommand { get; set; }
    
    }
}