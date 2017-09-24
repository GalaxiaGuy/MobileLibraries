using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GamesWithGravitas.XamarinForms.Sample
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class SkiaPage : ContentPage
	{
		public SkiaPage()
		{
			InitializeComponent();
            animatedView.AnimateAsync();
		}

        private void ButtonClicked(object sender, EventHandler e)
        {
            animatedView.AnimateAsync();
        }
	}
}