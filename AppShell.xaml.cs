namespace Voxerra
{
    public partial class AppShell : Shell
    {
        private readonly ILoginStateService _loginStateService;
        public AppShell(ILoginStateService loginStateService, LoginPage loginPage/*, MessageCenterPage messageCenterPage*/)
        {
            InitializeComponent();

            Routing.RegisterRoute("MessageCenterPage", typeof(MessageCenterPage));
            Routing.RegisterRoute("ChatPage", typeof(ChatPage));
            Routing.RegisterRoute("LoginPage", typeof(LoginPage));
            Routing.RegisterRoute("RegisterPage", typeof(RegisterPage));

            _loginStateService = loginStateService;


            //this.CurrentItem = loginStateService.IsLoggedIn ? messageCenterPage : loginPage;

            this.CurrentItem = loginPage;
            //this.CurrentItem = registerPage;
        }

    }
}
