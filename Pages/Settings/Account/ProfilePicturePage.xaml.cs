using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Voxerra.Pages.Settings.Account;

public partial class ProfilePicturePage : ContentPage
{
    public ProfilePicturePage(ProfilePictureViewModel viewModel)
    {
        InitializeComponent();
        this.BindingContext = viewModel;
    }
}