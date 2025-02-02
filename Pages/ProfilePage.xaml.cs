namespace Voxerra.Pages;

public partial class ProfilePage : ContentPage
{
	public ProfilePage(ProfilePageViewModel viewModel)
	{
		InitializeComponent();

        this.BindingContext = viewModel;
    }

	protected override void OnAppearing()
    {
        base.OnAppearing();

        (BindingContext as ProfilePageViewModel)?.Initialize();
    }
}