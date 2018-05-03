using System;
using Xamarin.Forms;

namespace GamesWithGravitas.XamarinForms.Layout
{
    public class LayerLayout : Layout<View>
    {
        public static readonly BindableProperty IncludeInvisibleChildrenProperty =
            BindableProperty.Create(nameof(IncludeInvisibleChildren), typeof(bool), typeof(LayerLayout), true, propertyChanged: (bindable, oldValue, newValue) => ((LayerLayout)bindable).InvalidateLayout());

        public bool IncludeInvisibleChildren
        {
            get => (bool)GetValue(IncludeInvisibleChildrenProperty);
            set => SetValue(IncludeInvisibleChildrenProperty, value);
        }

        protected override SizeRequest OnMeasure(double widthConstraint, double heightConstraint)
        {
            var width = 0d;
            var height = 0d;
            var minWidth = 0d;
            var minHeight = 0d;

            foreach (var child in Children)
            {
                if (!IncludeInvisibleChildren && !child.IsVisible)
                {
                    continue;
                }
                var sizeRequest = child.Measure(widthConstraint, heightConstraint, MeasureFlags.IncludeMargins);
                width = Math.Max(width, sizeRequest.Request.Width);
                height = Math.Max(height, sizeRequest.Request.Height);
                minWidth = Math.Max(width, sizeRequest.Minimum.Width);
                minHeight = Math.Max(height, sizeRequest.Minimum.Height);
            }
            return new SizeRequest(new Size(width, height), new Size(minWidth, minHeight));
        }

        protected override void LayoutChildren(double x, double y, double width, double height)
        {
            var rect = new Rectangle(x, y, width, height);
            foreach(var child in Children)
            {
                if (!IncludeInvisibleChildren && !child.IsVisible)
                {
                    continue;
                }
                LayoutChildIntoBoundingRegion(child, rect);
            }
        }
    }
}
