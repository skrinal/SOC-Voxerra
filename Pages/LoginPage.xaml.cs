namespace Voxerra.Pages
{
    public partial class LoginPage : ContentPage
    {
        public LoginPage(LoginPageViewModel viewModel)
        {
            InitializeComponent();

            this.BindingContext = viewModel;
        }

    }
}

