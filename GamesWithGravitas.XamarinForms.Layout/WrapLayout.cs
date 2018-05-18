using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;

namespace GamesWithGravitas.XamarinForms.Layout
{
    public class WrapLayout : Layout<View>, IWrapElement
    {
        public static BindableProperty ColumnSpacingProperty = WrapElement.ColumnSpacingProperty;
        public static BindableProperty RowSpacingProperty = WrapElement.RowSpacingProperty;
        public static readonly BindableProperty OrientationProperty =
            BindableProperty.Create(nameof(Orientation), typeof(StackOrientation), typeof(WrapLayout), StackOrientation.Horizontal, propertyChanged: (bindable, oldValue, newValue) => ((IWrapElement)bindable).InvalidateLayout());

        public double ColumnSpacing
        {
            get => (double)GetValue(ColumnSpacingProperty);
            set => SetValue(ColumnSpacingProperty, value);
        }

        public double RowSpacing
        {
            get => (double)GetValue(RowSpacingProperty);
            set => SetValue(RowSpacingProperty, value);
        }

        public StackOrientation Orientation
        {
            get => (StackOrientation)GetValue(OrientationProperty);
            set => SetValue(OrientationProperty, value);
        }

        void IWrapElement.InvalidateLayout() => InvalidateLayout();

        protected override SizeRequest OnMeasure(double widthConstraint, double heightConstraint)
        {
            if (Children.Count == 0)
            {
                return new SizeRequest(new Size(0, 0));
            }
            double width = 0;
            double height = 0;
            double mainAxisSize = 0;
            double crossAxisSize = 0;

            foreach (var child in Children)
            {
                var sizeRequest = child.Measure(widthConstraint, heightConstraint, MeasureFlags.IncludeMargins);
                if (Orientation == StackOrientation.Horizontal)
                {
                    if (mainAxisSize + ColumnSpacing + sizeRequest.Request.Width > widthConstraint)
                    {
                        height += crossAxisSize + RowSpacing;
                        crossAxisSize = sizeRequest.Request.Height;
                        mainAxisSize = sizeRequest.Request.Width;
                    }
                    else
                    {
                        mainAxisSize += sizeRequest.Request.Width + ColumnSpacing;
                        crossAxisSize = Math.Max(crossAxisSize, sizeRequest.Request.Height);
                    }
                    width = Math.Max(mainAxisSize, width);
                }
                else
                {
                    if (mainAxisSize + RowSpacing + sizeRequest.Request.Height > heightConstraint)
                    {
                        width += crossAxisSize + ColumnSpacing;
                        crossAxisSize = sizeRequest.Request.Width;
                        mainAxisSize = sizeRequest.Request.Height;
                    }
                    else
                    {
                        mainAxisSize += sizeRequest.Request.Height + RowSpacing;
                        crossAxisSize = Math.Max(crossAxisSize, sizeRequest.Request.Width);
                    }
                    height = Math.Max(mainAxisSize, height);                    
                }
            }
            if (Orientation == StackOrientation.Horizontal)
            {
                height += crossAxisSize;
            }
            else
            {
                width += crossAxisSize;
            }

            var size = new Size(width, height);
            return new SizeRequest(size, size);
        }

        protected override void LayoutChildren(double x, double y, double width, double height)
        {
            List<(View, Size)> crossAxisChildren = new List<(View, Size)>();
            double mainAxisSize = 0;
            double crossAxisSize = 0;
            int mainAxisIndex = 0;
            foreach (var child in Children)
            {
                var sizeRequest = child.Measure(width, height, MeasureFlags.IncludeMargins);
                if (Orientation == StackOrientation.Horizontal)
                {
                    if (mainAxisSize + ColumnSpacing + sizeRequest.Request.Width > width)
                    {
                        LayoutRow(crossAxisChildren, x, y, width, crossAxisSize);
                        crossAxisChildren.Clear();
                        y += crossAxisSize + RowSpacing;
                        mainAxisSize = sizeRequest.Request.Width;
                        crossAxisSize = sizeRequest.Request.Height;
                        mainAxisIndex++;
                    }
                    else
                    {
                        mainAxisSize += sizeRequest.Request.Width + ColumnSpacing;
                        crossAxisSize = Math.Max(crossAxisSize, sizeRequest.Request.Height);
                    }
                    crossAxisChildren.Add((child, sizeRequest.Request));
                }
                else
                {
                    if (mainAxisSize + RowSpacing + sizeRequest.Request.Height > height)
                    {
                        LayoutColumn(crossAxisChildren, x, y, crossAxisSize, height);
                        crossAxisChildren.Clear();
                        x += crossAxisSize + ColumnSpacing;
                        mainAxisSize = sizeRequest.Request.Height;
                        crossAxisSize = sizeRequest.Request.Width;
                        mainAxisIndex++;
                    }
                    else
                    {
                        mainAxisSize += sizeRequest.Request.Height + RowSpacing;
                        crossAxisSize = Math.Max(crossAxisSize, sizeRequest.Request.Width);
                    }
                    crossAxisChildren.Add((child, sizeRequest.Request));
                }
            }
            if (Orientation == StackOrientation.Horizontal)
            {
                LayoutRow(crossAxisChildren, x, y, width, crossAxisSize);
            }
            else
            {
                LayoutColumn(crossAxisChildren, x, y, crossAxisSize, height);
            }
        }

        private void LayoutRow(List<(View child, Size size)> children, double x, double y, double width, double height)
        {
            var minimumWidth = children.Sum(c => c.size.Width);
            var extraAvailableWidth = width - minimumWidth;
            var expandingChildren = children.Where(c => c.child.HorizontalOptions.Expands);
            var expandingChlidCount = expandingChildren.Count();
            var expandingChildWidth = expandingChildren.Sum(c => c.size.Width);

            var spareWidth = width - minimumWidth - (children.Count - 1) * ColumnSpacing;
            foreach ((var child, var size) in children)
            {
                if (child.HorizontalOptions.Expands)
                {
                    var expandingChildWidthFraction = size.Width / expandingChildWidth;
                    var newWidth = size.Width + spareWidth * expandingChildWidthFraction;
                    LayoutChildIntoBoundingRegion(child, new Rectangle(x, y, newWidth, height));
                    x += newWidth + ColumnSpacing;
                }
                else
                {
                    LayoutChildIntoBoundingRegion(child, new Rectangle(x, y, size.Width, height));
                    x += size.Width + ColumnSpacing;
                }
            }
        }

        private void LayoutColumn(List<(View child, Size size)> children, double x, double y, double width, double height)
        {
            var minimumHeight = children.Sum(c => c.size.Height);
            var extraAvailableHeight = height - minimumHeight;
            var expandingChildren = children.Where(c => c.child.VerticalOptions.Expands);
            var expandingChlidCount = expandingChildren.Count();
            var expandingChildHeight = expandingChildren.Sum(c => c.size.Height);

            var spareHeight = width - minimumHeight - (children.Count - 1) * RowSpacing;
            foreach ((var child, var size) in children)
            {
                if (child.VerticalOptions.Expands)
                {
                    var expandingChildHeightFraction = size.Height / expandingChildHeight;
                    var newHeight = size.Height + spareHeight * expandingChildHeightFraction;
                    LayoutChildIntoBoundingRegion(child, new Rectangle(x, y, width, newHeight));
                    y += newHeight + RowSpacing;
                }
                else
                {
                    LayoutChildIntoBoundingRegion(child, new Rectangle(x, y, width, size.Height));
                    y += size.Height + RowSpacing;
                }
            }
        }
    }
}
