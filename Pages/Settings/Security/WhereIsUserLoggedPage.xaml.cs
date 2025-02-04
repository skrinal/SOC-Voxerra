using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Voxerra.Pages.Settings.Security;

public partial class WhereIsUserLoggedPage : ContentPage
{
    public WhereIsUserLoggedPage(WhereIsUserLoggedViewModel viewModel)
    {
        InitializeComponent();
        this.BindingContext = viewModel;
    }
}