using GamesWithGravitas.XamarinForms.Layout;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using System.Collections.Generic;
using Xamarin.Forms;

namespace GamesWithGravitas.XamarinForms.SkiaSharp
{
    [ContentProperty(nameof(CanvasContent))]
    public class SKCanvasContentView : ContentView
    {
        public static BindableProperty CanvasContentProperty =
            BindableProperty.Create(nameof(CanvasContent), typeof(View), typeof(SKCanvasContentView), null);

        public View CanvasContent
        {
            get => (View)GetValue(CanvasContentProperty);
            set => SetValue(CanvasContentProperty, value);
        }

        public SKCanvasView CanvasView { get; private set; }

        public SKCanvasContentView()
        {
            CanvasView = new SKCanvasView();
            CanvasView.PaintSurface += OnPaintSurface;
            var layout = new LayerLayout();
            var contentView = new ContentView();
            contentView.SetBinding(ContentProperty, new Binding(nameof(CanvasContent), source: this));
            layout.Children.Add(CanvasView);
            layout.Children.Add(contentView);
            Content = layout;
        }

        private void OnPaintSurface(object sender, SKPaintSurfaceEventArgs e)
        {
            e.Surface.Canvas.Clear(SKColor.Empty);
            var canvasView = (SKCanvasView)sender;
            var pixelScale = canvasView.CanvasSize.Width / (float)canvasView.Width;
            foreach (var child in _canvasChildren)
            {
                e.Surface.Canvas.Save();
                e.Surface.Canvas.Translate(child.X*pixelScale, child.Y* pixelScale);

                var region = new SKRegion();
                var x = child.X * pixelScale;
                var y = child.Y * pixelScale;
                var width = (child.Child.Width + child.X) * pixelScale;
                var height = (child.Child.Height + child.Y) * pixelScale;
                region.SetRect(new SKRectI((int)x, (int)y, (int)width, (int)height));
                e.Surface.Canvas.ClipRegion(region);

                child.Child.Paint(e.Surface, e.Info);

                e.Surface.Canvas.Restore();
            }
        }

        private SortedSet<SKCanvasChild> _canvasChildren = new SortedSet<SKCanvasChild>(SKCanvasChild.DefaultComparer);

        public void RegisterChild(SKCanvasChildView child, double x, double y, int z)
        {
            _canvasChildren.RemoveWhere(c => c.Child == child);
            _canvasChildren.Add(new SKCanvasChild { X = (float)x, Y = (float)y, Z = z, Child = child });
            CanvasView.InvalidateSurface();
        }

        private class SKCanvasChild
        {
            public float X;
            public float Y;
            public int Z;
            public SKCanvasChildView Child;

            public static SKCanvasChildComparer DefaultComparer = new SKCanvasChildComparer();

            public class SKCanvasChildComparer : IComparer<SKCanvasChild>
            {
                public int Compare(SKCanvasChild x, SKCanvasChild y)
                {
                    return x.Z.CompareTo(y.Z);
                }
            }
        }
    }
}
