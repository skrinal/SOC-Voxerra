namespace Voxerra.Pages;

public partial class FriendRequestPage : ContentPage
{
	public FriendRequestPage(FriendRequestViewModel viewModel)
	{
		InitializeComponent();

		this.BindingContext = viewModel;
	}

    protected override void OnAppearing()
    {
        base.OnAppearing();
        (BindingContext as FriendRequestViewModel)?.Initialize();
    }
}