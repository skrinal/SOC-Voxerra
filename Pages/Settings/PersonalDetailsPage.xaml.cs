namespace Voxerra.Pages.Settings;

public partial class PersonalDetailsPage : ContentPage
{
    public PersonalDetailsPage(PersonalDetailsViewModel viewModel)
    {
        InitializeComponent();

        this.BindingContext = viewModel;
    }
}