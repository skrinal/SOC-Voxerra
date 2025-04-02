namespace Voxerra.Pages;

public partial class MessageCenterPage : ContentPage
{
	public MessageCenterPage(MessageCenterPageViewModel viewModel)
	{
		InitializeComponent();

        this.BindingContext = viewModel;
    }


    /*protected override void OnAppearing()
    {
        base.OnAppearing();
        
        (BindingContext as MessageCenterPageViewModel)?.Initialize();
    }*/

}