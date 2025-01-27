namespace Voxerra.Pages.Settings;

public partial class NotificationsPage : ContentPage
{
    public NotificationsPage(NotificationsViewModel viewModel)
    {
        InitializeComponent();

        this.BindingContext = viewModel;
    }
}