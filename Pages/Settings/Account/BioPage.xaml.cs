namespace Voxerra.Pages.Settings.Account;

public partial class BioPage : ContentPage
{
    public BioPage(BioViewModel viewModel)
    {
        InitializeComponent();
        this.BindingContext = viewModel;
    }
}