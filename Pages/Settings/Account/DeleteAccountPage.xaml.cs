using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Voxerra.Pages.Settings.Account;

public partial class DeleteAccountPage : ContentPage
{
    public DeleteAccountPage(DeleteAccountViewModel viewModel)
    {
        InitializeComponent();
        this.BindingContext = viewModel;
    }
}