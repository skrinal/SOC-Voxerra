namespace Voxerra.Pages;

public partial class MessageCenterPage : ContentPage
{
	public MessageCenterPage(MessageCenterPageViewModel viewModel)
	{
		InitializeComponent();

        this.BindingContext = viewModel;
    }


    protected override void OnAppearing()
    {
        base.OnAppearing();
        
        (BindingContext as MessageCenterPageViewModel)?.Initialize();
    }

    //protected override void OnAppearing()
    //{
    //    base.OnAppearing();

    //    // Call your view model method to refresh or update the message list (if necessary)
    //    var viewModel = (MessageCenterPageViewModel)BindingContext;
    //    viewModel.UpdateLatestMessages();
    //}


    //private void ContentPage_NavigatedTo(object sender, NavigatedToEventArgs e)
    //{
    //    (BindingContext as MessageCenterPageViewModel).Initialize();
    //}
}