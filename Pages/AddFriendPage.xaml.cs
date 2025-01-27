
namespace Voxerra.Pages
{
    public partial class AddFriendPage : ContentPage
    {
        public AddFriendPage(AddFriendViewModel viewModel)
        {
            InitializeComponent();
            this.BindingContext = viewModel;
        }
    }
}

