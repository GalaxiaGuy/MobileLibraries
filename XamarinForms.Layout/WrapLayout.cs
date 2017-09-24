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

        void IWrapElement.InvalidateLayout() => InvalidateLayout();

        protected override SizeRequest OnMeasure(double widthConstraint, double heightConstraint)
        {
            if (Children.Count == 0)
            {
                return new SizeRequest(new Size(0, 0));
            }
            double width = 0;
            double height = 0;
            double columnWidth = 0;
            double rowHeight = 0;

            foreach (var child in Children)
            {
                var sizeRequest = child.Measure(widthConstraint, heightConstraint, MeasureFlags.IncludeMargins);
                if (columnWidth + ColumnSpacing + sizeRequest.Request.Width > widthConstraint)
                {
                    height += rowHeight + RowSpacing;
                    rowHeight = sizeRequest.Request.Height;
                    columnWidth = sizeRequest.Request.Width;
                }
                else
                {
                    columnWidth += sizeRequest.Request.Width + ColumnSpacing;
                    rowHeight = Math.Max(rowHeight, sizeRequest.Request.Height);
                }
                width = Math.Max(columnWidth, width);
            }
            height += rowHeight;

            var size = new Size(width, height);
            return new SizeRequest(size, size);
        }

        protected override void LayoutChildren(double x, double y, double width, double height)
        {
            List<(View, Size)> rowChildren = new List<(View, Size)>();
            double rowWidth = 0;
            double rowHeight = 0;
            int row = 0;
            foreach (var child in Children)
            {
                var sizeRequest = child.Measure(width, height, MeasureFlags.IncludeMargins);
                if (rowWidth + ColumnSpacing + sizeRequest.Request.Width > width)
                {
                    LayoutRow(rowChildren, x, y, width, rowHeight);
                    rowChildren.Clear();
                    y += rowHeight + RowSpacing;
                    rowWidth = sizeRequest.Request.Width;
                    rowHeight = sizeRequest.Request.Height;
                    row++;
                }
                rowWidth += sizeRequest.Request.Width + ColumnSpacing;
                rowHeight = Math.Max(rowHeight, sizeRequest.Request.Height);
                rowChildren.Add((child, sizeRequest.Request));
            }
            LayoutRow(rowChildren, x, y, width, rowHeight);
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
    }
}
