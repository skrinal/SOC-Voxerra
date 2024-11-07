namespace Voxerra.Pages;
public partial class ChatPage : ContentPage
{
	public ChatPage(ChatPageViewModel viewModel)
	{
		InitializeComponent();

		this.BindingContext = viewModel;
	}

}