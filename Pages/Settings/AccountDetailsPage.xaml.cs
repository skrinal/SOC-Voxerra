namespace Voxerra.Pages.Settings;

public partial class AccountDetailsPage : ContentPage
{
    public AccountDetailsPage(AccountDetailsViewModel viewModel)
    {
        InitializeComponent();

        this.BindingContext = viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        (BindingContext as AccountDetailsViewModel)?.Initialize();
    }
}