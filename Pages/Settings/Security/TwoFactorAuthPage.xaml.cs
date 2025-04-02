using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Voxerra.Pages.Settings.Security;

public partial class TwoFactorAuthPage : ContentPage
{
    public TwoFactorAuthPage(TwoFactorAuthViewModel viewModel)
    {
        InitializeComponent();
        this.BindingContext = viewModel;
    }
}