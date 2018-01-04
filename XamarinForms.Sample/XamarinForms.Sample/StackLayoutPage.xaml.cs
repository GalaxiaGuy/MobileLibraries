using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace GamesWithGravitas.XamarinForms.Sample
{
    public partial class StackLayoutPage : ContentPage
    {
        public StackLayoutPage()
        {
            InitializeComponent();
            BindingContext = new StackLayoutViewModel();
        }
    }
}
