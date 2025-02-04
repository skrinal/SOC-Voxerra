using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Voxerra.Pages.Settings.Security;

public partial class SavedLoginPage : ContentPage
{
    public SavedLoginPage(SavedLoginViewModel viewModel)
    {
        InitializeComponent();
        this.BindingContext = viewModel;   
    }
}