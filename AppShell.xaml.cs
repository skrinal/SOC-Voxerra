namespace Voxerra
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute("LoginPage", typeof(LoginPage));
            Routing.RegisterRoute("RegisterPage", typeof(RegisterPage));
            Routing.RegisterRoute("RegisterConfirmationPage", typeof(RegisterConfirmationPage));
            Routing.RegisterRoute("ForgotPasswordPage", typeof(ForgotPasswordPage));
            Routing.RegisterRoute("PasswordConfirmationPage", typeof(PasswordConfirmationPage));
            Routing.RegisterRoute("TwoAuthPage", typeof(TwoAuthPage));

            
            Routing.RegisterRoute("MessageCenterPage", typeof(MessageCenterPage));
            Routing.RegisterRoute("ChatPage", typeof(ChatPage));
            Routing.RegisterRoute("AddFriendPage", typeof(AddFriendPage));  
            Routing.RegisterRoute("PublicProfilePage", typeof(PublicProfilePage));

            
            Routing.RegisterRoute("GroupCenterPage", typeof(GroupCenterPage));  
            Routing.RegisterRoute("GroupCenterViewModel", typeof(GroupCenterViewModel));
            Routing.RegisterRoute("CreateGroupPage", typeof(CreateGroupPage));  
            Routing.RegisterRoute("CreateGroupViewModel", typeof(CreateGroupViewModel));
            
            
            Routing.RegisterRoute("ProfilePage", typeof(ProfilePage));
            Routing.RegisterRoute("FriendRequestPage", typeof(FriendRequestPage));
            Routing.RegisterRoute("MainSettingPage", typeof(MainSettingPage));
            

            Routing.RegisterRoute("AccountDetailsPage", typeof(AccountDetailsPage));
            Routing.RegisterRoute("NamePage", typeof(NamePage));
            Routing.RegisterRoute("EmailPage", typeof(EmailPage));
            Routing.RegisterRoute("ProfilePicturePage", typeof(ProfilePicturePage));
            Routing.RegisterRoute("BioPage", typeof(BioPage));
            Routing.RegisterRoute("DeleteAccountPage", typeof(DeleteAccountPage));
            
            Routing.RegisterRoute("NotificationsPage", typeof(NotificationsPage));
            
            Routing.RegisterRoute("SecurityPage", typeof(SecurityPage));
            Routing.RegisterRoute("PasswordPage", typeof(PasswordPage));
            Routing.RegisterRoute("TwoFactorAuthPage", typeof(TwoFactorAuthPage));
            Routing.RegisterRoute("SavedLoginPage", typeof(SavedLoginPage));
            Routing.RegisterRoute("WhereIsUserLoggedPage", typeof(WhereIsUserLoggedPage));
            Routing.RegisterRoute("LoginPage", typeof(LoginPage));

            /* pozriet ci SecureStorage ma RefreshToken(dlhodoby)

                (rozhodnut ci spravit navyse ze TokenExpiracia bude zapisana pri nom
                alebo aj kvazi "Exipered" token skusit zavolat na authV2)


             if (true) - call authV2 controller ktory vrati AccesToken(kratkodoby)
             - normalne otvori messageCenterPage
                -- pokial nevrati AccesToken lebo RefreshToken je prepadnuty tak otvori LoginPage

             if (false) - normalne otvori loginPage

            */



        }

        protected override void OnNavigating(ShellNavigatingEventArgs args)
        {
            base.OnNavigating(args);

            // Hide navigation bar for Login, Register, and ForgotPassword pages
            if (args.Target.Location.OriginalString.Contains("LoginPage") ||
                args.Target.Location.OriginalString.Contains("RegisterPage") ||
                args.Target.Location.OriginalString.Contains("ForgotPasswordPage")||
                args.Target.Location.OriginalString.Contains("TwoAuthPage"))
            {
                Shell.SetNavBarIsVisible(this, false);
            }
            else
            {
                // Show navigation bar for all other pages
                Shell.SetNavBarIsVisible(this, true);
            }
        }
        

    }
}
