namespace Voxerra.Pages;

public partial class RegisterConfirmationPage : ContentPage
{
	public RegisterConfirmationPage(RegisterConfirmationViewModel viewModel)
	{
		InitializeComponent();

        this.BindingContext = viewModel;
    }
}