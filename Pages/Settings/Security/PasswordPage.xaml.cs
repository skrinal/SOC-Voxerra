
namespace Voxerra.Pages.Settings.Security;

public partial class PasswordPage : ContentPage
{
    public PasswordPage(PasswordViewModel viewModel)
    {
        InitializeComponent();
        this.BindingContext = viewModel;
    }
}