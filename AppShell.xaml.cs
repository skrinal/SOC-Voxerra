namespace Voxerra
{
    public partial class AppShell : Shell
    {
        private readonly ILoginStateService _loginStateService;
        public AppShell(ILoginStateService loginStateService, LoginPage loginPage)
        {
            InitializeComponent();

            Routing.RegisterRoute("MessageCenterPage", typeof(MessageCenterPage));
            Routing.RegisterRoute("ChatPage", typeof(ChatPage));
            Routing.RegisterRoute("LoginPage", typeof(LoginPage));
            Routing.RegisterRoute("RegisterPage", typeof(RegisterPage));

            _loginStateService = loginStateService;

            /* pozriet ci SecureStorage ma RefreshToken(dlhodoby)

                (rozhodnut ci spravit navyse ze TokenExpiracia bude zapisana pri nom
                alebo aj kvazi "Exipered" token skusit zavolat na authV2)


             if (true) - call authV2 controller ktory vrati AccesToken(kratkodoby)
             - normalne otvori messageCenterPage
                -- pokial nevrati AccesToken lebo RefreshToken je prepadnuty tak otvori LoginPage

             if (false) - normalne otvori loginPage

            */
            this.CurrentItem = loginStateService.IsLoggedIn ? new MessageCenterPage() : loginPage;
            //this.CurrentItem = loginPage;
        }

    }
}
