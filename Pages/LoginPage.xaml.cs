using Microsoft.Maui.Controls;

namespace Voxerra.Pages;

public partial class LoginPage : ContentPage
{
	public LoginPage()
	{
		InitializeComponent();
    }

    private void Entry_Focused(object sender, FocusEventArgs e)
    {

        UserNameBorder.Stroke = Application.Current.Resources["TertiaryColor"];

    }

}