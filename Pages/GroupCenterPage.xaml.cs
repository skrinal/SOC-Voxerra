using CommunityToolkit.Mvvm.Messaging;

namespace Voxerra.Pages;

public partial class GroupCenterPage : ContentPage
{
    public GroupCenterPage(GroupCenterViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        // Execute the command when the page appears
        if (BindingContext is GroupCenterViewModel viewModel)
        {
            // Ensure the command is executed
            viewModel.LoadGroupsCommand.Execute(null);
        }
    }
}