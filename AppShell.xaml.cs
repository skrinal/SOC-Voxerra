using Voxerra.Pages;

namespace Voxerra
{
    public partial class AppShell : Shell
    {
        public AppShell(LoginPage loginPage, RegisterPage registerPage)
        {
            InitializeComponent();

            Routing.RegisterRoute("MessageCenterPage", typeof(MessageCenterPage));
            Routing.RegisterRoute("ChatPage", typeof(ChatPage));
            Routing.RegisterRoute("LoginPage", typeof(LoginPage));

            this.CurrentItem = loginPage;
            //this.CurrentItem = registerPage;
        }

        //public AppShell(ChatPage chatPage)
        //{
        //    InitializeComponent();

        //    Routing.RegisterRoute("MessageCenterPage", typeof(MessageCenterPage));

        //    this.CurrentItem = chatPage;
        //}
    }
}
