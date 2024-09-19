namespace Voxerra
{
    public partial class MainPage : ContentPage
    {

        /*
        https://www.youtube.com/watch?v=r-Rm9YJzE24
        */
        private readonly HubConnection _connection;
        public MainPage()
        {
            InitializeComponent();

            _connection = new HubConnectionBuilder()
                .WithUrl("http://0.0.0.0:5296/chat")    
                .Build();
            
            _connection.On<string>("MessageReceived", (message) =>
            {
                chatMessage.Text += $"{Enviroment.NewLine}{message}";
            });
            
            Task.Run(() =>
            {
                Dispatcher.Dispatch(async () =>
                await _connection.StartAsync());
            });
        }

        private async void OnCounterClicked(object sender, EventArgs e)
        {
            await _connection.InvokeCoreAsync("SendMessage", arg: []
                { myChatMessage.Text });
            myChatMessage.Text = String.Empty;
        }
    }

}
