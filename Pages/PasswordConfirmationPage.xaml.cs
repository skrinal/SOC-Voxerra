using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Voxerra.Pages;

public partial class PasswordConfirmationPage : ContentPage
{
    public PasswordConfirmationPage(PasswordConfirmationViewModel viewModel)
    {
        InitializeComponent();
        this.BindingContext = viewModel;
    }
}