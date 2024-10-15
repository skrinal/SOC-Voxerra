namespace Voxerra.Pages;

public partial class MessageCenterPage : ContentPage
{
	public MessageCenterPage()
	{
		InitializeComponent();
	}

    private async void OnMenuButtonClicked(object sender, EventArgs e)
    {
        ProfileView.IsVisible = true;
        ProfileView.TranslationX = Application.Current.MainPage.Width; // Start from off-screen
        await ProfileView.TranslateTo(Application.Current.MainPage.Width / 2, 0, 250, Easing.SinIn); // Slide in
    }

    private async void OnCloseProfileClicked(object sender, EventArgs e)
    {
        await ProfileView.TranslateTo(Application.Current.MainPage.Width, 0, 250, Easing.SinOut); // Slide out
        ProfileView.IsVisible = false;
    }


}