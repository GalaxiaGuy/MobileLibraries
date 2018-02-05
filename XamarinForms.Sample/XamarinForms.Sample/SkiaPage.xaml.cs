using System;
using Xamarin.Forms;

namespace GamesWithGravitas.XamarinForms.Sample
{
	public partial class SkiaPage : ContentPage
	{
		public SkiaPage()
		{
			InitializeComponent();
            animatedView.AnimateAsync();
		}

        private void ButtonClicked(object sender, EventArgs e)
        {
            animatedView.AnimateAsync();
        }
	}
}