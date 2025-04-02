namespace Voxerra.Pages.Settings.Account;

public partial class EmailPage : ContentPage
{
    public EmailPage(EmailViewModel viewModel)
    {
        InitializeComponent();
        this.BindingContext = viewModel;
    }
}