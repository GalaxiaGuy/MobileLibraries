using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace GamesWithGravitas.XamarinForms.Sample
{
    public class MainViewModel
    {
        public List<DemoItem> Items { get; }

        public MainViewModel(INavigation navigation)
        {
            Items = new List<DemoItem>
            {
                new DemoItem("SKCanvasContentView and SKCanvasChildView", async () => await navigation.PushAsync(new SkiaPage())),
                new DemoItem("Layout.ItemsSource", async () => await navigation.PushAsync(new StackLayoutPage()))
            };
        }

    }

    public class DemoItem
    {
        public string Title { get; }
        public Command Command { get; }

        public DemoItem(string title, Action commandDelegate)
        {
            Title = title;
            Command = new Command(_ => commandDelegate());
        }
    }
}
