using GamesWithGravitas.XamarinForms.Sample;
using Xamarin.Forms;

namespace XamarinForms.Sample
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            BindingContext = new MainViewModel(Navigation);
        }
    }
}
