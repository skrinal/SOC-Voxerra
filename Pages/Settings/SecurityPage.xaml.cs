namespace Voxerra.Pages.Settings;

public partial class SecurityPage : ContentPage
{
    public SecurityPage(SecurityViewModel viewModel)
    {
        InitializeComponent();

        this.BindingContext = viewModel;
    }
}