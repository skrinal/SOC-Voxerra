using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Voxerra.ViewModels.Settings.Security;

namespace Voxerra.Pages.Settings.Security;

public partial class LoginAlertsPage : ContentPage
{
    public LoginAlertsPage(LoginAlertsViewModel viewModel)
    {
        InitializeComponent();
        this.BindingContext = viewModel;
    }
}