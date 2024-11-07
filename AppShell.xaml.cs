using Voxerra.Pages;

namespace Voxerra
{
    public partial class AppShell : Shell
    {
        public AppShell(LoginPage loginPage)
        {
            InitializeComponent();

            Routing.RegisterRoute("MessageCenterPage", typeof(MessageCenterPage));
            Routing.RegisterRoute("ChatPage", typeof(ChatPage));

            this.CurrentItem = loginPage;
        }

        //public AppShell(ChatPage chatPage)
        //{
        //    InitializeComponent();

        //    Routing.RegisterRoute("MessageCenterPage", typeof(MessageCenterPage));

        //    this.CurrentItem = chatPage;
        //}
    }
}
