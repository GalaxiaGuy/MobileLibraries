using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace GamesWithGravitas.XamarinForms.Layout
{
    public class RescaleLayout : LayerLayout
    {
        public static readonly BindableProperty CoefficientProperty =
            BindableProperty.Create(nameof(Coefficient), typeof(double), typeof(RescaleLayout), 1d, propertyChanged: OnRescalePropertyChanged);

        public static readonly BindableProperty MaximumWidthProperty =
            BindableProperty.Create(nameof(MaximumWidth), typeof(double), typeof(RescaleLayout), double.PositiveInfinity, propertyChanged: OnRescalePropertyChanged);

        public static readonly BindableProperty MaximumHeightProperty =
            BindableProperty.Create(nameof(MaximumHeight), typeof(double), typeof(RescaleLayout), double.PositiveInfinity, propertyChanged: OnRescalePropertyChanged);

        public double Coefficient
        {
            get => (double)GetValue(CoefficientProperty);
            set => SetValue(CoefficientProperty, value);
        }

        public double MaximumWidth
        {
            get => (double)GetValue(MaximumWidthProperty);
            set => SetValue(MaximumWidthProperty, value);
        }

        public double MaximumHeight
        {
            get => (double)GetValue(MaximumHeightProperty);
            set => SetValue(MaximumHeightProperty, value);
        }

        private static void OnRescalePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            ((RescaleLayout)bindable).InvalidateMeasure();
        }

        protected override void LayoutChildren(double x, double y, double width, double height)
        {
            var scale = Coefficient;
            var rect = new Rectangle(x, y, width / scale, height / scale);
            if (rect.Width > MaximumWidth)
            {
                scale = width / MaximumWidth;
                rect = new Rectangle(x, y, width / scale, height / scale);
            }
            if (rect.Height > MaximumHeight)
            {
                scale = height / MaximumHeight;
                rect = new Rectangle(x, y, width / scale, height / scale);
            }
            var childScale = scale;
            foreach (var child in Children)
            {
                child.Scale = childScale;
                child.AnchorX = 0;
                child.AnchorY = 0;
                LayoutChildIntoBoundingRegion(child, rect);
            }
        }
    }
}
