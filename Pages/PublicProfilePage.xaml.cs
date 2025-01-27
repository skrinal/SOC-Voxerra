

namespace Voxerra.Pages;

public partial class PublicProfilePage : ContentPage
{
    public PublicProfilePage(PublicProfileViewModel viewModel)
    {
        InitializeComponent();
        this.BindingContext = viewModel;
    }
}