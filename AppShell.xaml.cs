namespace Voxerra
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute("MessageCenterPage", typeof(MessageCenterPage));
            Routing.RegisterRoute("ChatPage", typeof(ChatPage));
            Routing.RegisterRoute("LoginPage", typeof(LoginPage));
            Routing.RegisterRoute("RegisterPage", typeof(RegisterPage));
            Routing.RegisterRoute("RegisterConfirmationPage", typeof(RegisterConfirmationPage));
            Routing.RegisterRoute("ProfilePage", typeof(ProfilePage));
            Routing.RegisterRoute("ForgotPassword", typeof(ForgotPasswordPage));


            GoToLoginPage();
            //this.CurrentItem = loginPage;

            // _loginStateService = loginStateService;

            /* pozriet ci SecureStorage ma RefreshToken(dlhodoby)

                (rozhodnut ci spravit navyse ze TokenExpiracia bude zapisana pri nom
                alebo aj kvazi "Exipered" token skusit zavolat na authV2)


             if (true) - call authV2 controller ktory vrati AccesToken(kratkodoby)
             - normalne otvori messageCenterPage
                -- pokial nevrati AccesToken lebo RefreshToken je prepadnuty tak otvori LoginPage

             if (false) - normalne otvori loginPage

            */



        }

        private async void GoToLoginPage()
        {
            await GoToAsync("//LoginPage");
        }

    }
}
