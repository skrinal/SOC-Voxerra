using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Voxerra.Pages;

public partial class GroupPage : ContentPage
{
    public GroupPage(GroupViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}