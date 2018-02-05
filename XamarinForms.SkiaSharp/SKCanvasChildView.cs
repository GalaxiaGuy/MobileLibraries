using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using System.Runtime.CompilerServices;
using SkiaSharp.Views.Forms;

namespace GamesWithGravitas.XamarinForms.SkiaSharp
{
    public abstract class SKCanvasChildView : ContentView
    {
        protected SKCanvasChildView()
        {
            _ownCanvasView = new Lazy<SKCanvasView>(() =>
            {
                var canvas = new SKCanvasView();
                Content = canvas;
                return canvas;
            });
        }
        public static readonly BindableProperty ProvideOwnCanvasViewProperty =
            BindableProperty.Create(nameof(ProvideOwnCanvasView), typeof(bool), typeof(SKCanvasChildView), false, propertyChanged: ProvideOwnCanvasViewPropertyChanged);

        public bool ProvideOwnCanvasView
        {
            get => (bool)GetValue(ProvideOwnCanvasViewProperty);
            set => SetValue(ProvideOwnCanvasViewProperty, value);
        }

        private static void ProvideOwnCanvasViewPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            ((SKCanvasChildView)bindable).SetupOwnCanvas();
        }

        private void SetupOwnCanvas()
        {
            CanvasView.PaintSurface += OnPaintSurface;
        }

        private void OnPaintSurface(object sender, SKPaintSurfaceEventArgs e)
        {
            Paint(e.Surface, e.Info);
        }

        private Lazy<SKCanvasView> _ownCanvasView;

        protected SKCanvasView CanvasView => ProvideOwnCanvasView ? _ownCanvasView?.Value : CanvasContentView?.CanvasView;

        protected override void OnParentSet()
        {
            base.OnParentSet();
            if (!ProvideOwnCanvasView)
            {
                FindParentCanvasContentView();
            }
        }

        protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);
            if (ProvideOwnCanvasView)
            {
                return;
            }
            if (propertyName == WidthProperty.PropertyName ||
                propertyName == HeightProperty.PropertyName ||
                propertyName == XProperty.PropertyName ||
                propertyName == YProperty.PropertyName)
            {
                FindParentCanvasContentView();
            }
            CanvasView.InvalidateSurface();
        }

        public SKCanvasContentView CanvasContentView { get; private set; }

        public abstract void Paint(SKSurface surface, SKImageInfo info);

        private void FindParentCanvasContentView()
        {
            var view = (View)this;
            var x = 0d;
            var y = 0d;
            int z = 0;
            int scale = 1;
            if (view.X == -1 || view.Y == -1 || view.Width == -1 || view.Height == -1)
            {
                return;
            }
            while (true)
            {
                x += view.X;
                y += view.Y;
                if (view.Parent as View != null)
                {
                    if (view.Parent is Layout<View> layout)
                    {
                        z += (layout.Children.IndexOf(view) + 1) * scale;
                    }
                    if (view.Parent is SKCanvasContentView canvasContentView)
                    {
                        CanvasContentView = canvasContentView;
                        CanvasContentView.RegisterChild(this, x, y, z);
                        return;
                    }
                    view = view.Parent as View;
                    scale = scale << 1;
                }
                else
                {
                    return;
                }
            }
        }
    }
}
