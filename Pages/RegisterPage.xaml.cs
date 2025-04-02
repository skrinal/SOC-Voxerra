namespace Voxerra.Pages
{
	public partial class RegisterPage : ContentPage
	{
		public RegisterPage(RegisterPageViewModel viewModel)
		{
			InitializeComponent();

			this.BindingContext = viewModel;
		}
	}
}