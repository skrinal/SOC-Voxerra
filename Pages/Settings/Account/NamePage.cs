namespace Voxerra.Pages.Settings;

public partial class NamePage : ContentPage
{
    public NamePage(NameViewModel viewModel)
    {
        InitializeComponent();

        this.BindingContext = viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        (BindingContext as NameViewModel)?.Initialize();
    }
}