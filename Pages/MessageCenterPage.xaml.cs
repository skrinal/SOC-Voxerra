namespace Voxerra.Pages;

public partial class MessageCenterPage : ContentPage
{
	public MessageCenterPage(MessageCenterPageViewModel viewModel)
	{
		InitializeComponent();

        this.BindingContext = viewModel;
    }

    private void ContentPage_NavigatedTo(object sender, NavigatedToEventArgs e)
    {
        (this.BindingContext as MessageCenterPageViewModel).Initialize();
    }
}