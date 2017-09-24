using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using System.Runtime.CompilerServices;

namespace GamesWithGravitas.XamarinForms.SkiaSharp
{
    public abstract class SKCanvasChildView : ContentView
    {
        protected override void OnParentSet()
        {
            base.OnParentSet();
            FindParentCanvasContentView();
        }

        protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);
            if (CanvasContentView == null)
            {
                if (propertyName == WidthProperty.PropertyName ||
                    propertyName == HeightProperty.PropertyName)
                {
                    FindParentCanvasContentView();
                }
            }
            if (propertyName == XProperty.PropertyName ||
                propertyName == YProperty.PropertyName)
            {
                FindParentCanvasContentView();
            }
        }

        protected SKCanvasContentView CanvasContentView { get; private set; }

        public abstract void Paint(SKSurface surface, SKImageInfo info);

        private void FindParentCanvasContentView()
        {
            var view = (View)this;
            var x = 0d;
            var y = 0d;
            int z = 0;
            int scale = 1;
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
