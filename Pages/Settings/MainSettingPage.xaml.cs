using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Voxerra.Pages.Settings;

public partial class MainSettingPage : ContentPage
{
    public MainSettingPage(MainSettingViewModel viewModel)
    {
        InitializeComponent();
        this.BindingContext = viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        (BindingContext as MainSettingViewModel)?.Initialize();
    }
}