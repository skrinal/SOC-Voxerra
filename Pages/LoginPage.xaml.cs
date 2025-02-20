namespace Voxerra.Pages
{
    public partial class LoginPage : ContentPage
    {
        public LoginPage(LoginPageViewModel viewModel)
        {
            InitializeComponent();

            this.BindingContext = viewModel;
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
#if WINDOWS
        this.Window.Width = 450;
        this.Window.Height = 800;

#endif
        }
    }
}

