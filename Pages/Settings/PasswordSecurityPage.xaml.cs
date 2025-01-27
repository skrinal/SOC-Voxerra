namespace Voxerra.Pages.Settings;

public partial class PasswordSecurityPage : ContentPage
{
    public PasswordSecurityPage(PasswordSecurityViewModel viewModel)
    {
        InitializeComponent();

        this.BindingContext = viewModel;
    }
}