using SkiaSharp;
using SkiaSharp.Views.Forms;
using System;
using System.Collections.Generic;
using System.Text;
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

        private SKCanvasView _canvasView;

        public SKCanvasContentView()
        {
            _canvasView = new SKCanvasView();
            _canvasView.PaintSurface += OnPaintSurface;
            var layout = new LayerLayout();
            var contentView = new ContentView();
            contentView.SetBinding(ContentProperty, new Binding(nameof(CanvasContent), source: this));
            layout.Children.Add(contentView);
            layout.Children.Add(_canvasView);
            Content = layout;
        }

        private void OnPaintSurface(object sender, SKPaintSurfaceEventArgs e)
        {
            foreach (var child in _canvasChildren)
            {
                e.Surface.Canvas.Save();
                e.Surface.Canvas.Translate(child.X, child.Y);

                var region = new SKRegion();
                region.SetRect(new SKRectI((int)child.X, (int)child.Y, (int)child.Child.Width + (int)child.X, (int)child.Child.Height + (int)child.Y));
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
            _canvasView.InvalidateSurface();
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
