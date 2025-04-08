using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Voxerra.Pages;

public partial class TwoAuthPage : ContentPage
{
    public TwoAuthPage(TwoAuthViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}