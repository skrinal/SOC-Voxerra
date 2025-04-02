namespace Voxerra.Pages;

public partial class ForgotPasswordPage : ContentPage
{
	public ForgotPasswordPage(ForgotPasswordViewModel viewModel)
	{
		InitializeComponent();

        this.BindingContext = viewModel;
    }
}