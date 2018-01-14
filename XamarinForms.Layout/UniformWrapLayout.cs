using System;
using Xamarin.Forms;

namespace GamesWithGravitas.XamarinForms.Layout
{
    public class UniformWrapLayout : Layout<View>, IWrapElement
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

        private double _columnWidth;
        private double _rowHeight;
        private int _columnCount;
        private int _rowCount;

        protected override SizeRequest OnMeasure(double widthConstraint, double heightConstraint)
        {
            if (Children.Count == 0)
            {
                return new SizeRequest(new Size(0, 0));
            }
            double columnWidth = 0;
            double rowHeight = 0;

            var childMaximumWidth = WrapElement.GetChildMaximumWidth(this);

            foreach (var child in Children)
            {
                var sizeRequest = child.Measure(widthConstraint, heightConstraint, MeasureFlags.IncludeMargins);
                var childWidth = Math.Min(childMaximumWidth, sizeRequest.Request.Width);    
                columnWidth = Math.Max(columnWidth, childWidth);
                rowHeight = Math.Max(rowHeight, sizeRequest.Request.Height);
            }

            int columnCount = 1;
            double cumulativeWidth = columnWidth;
            while (columnCount < Children.Count)
            {
                if (cumulativeWidth + columnWidth + ColumnSpacing > widthConstraint)
                {
                    break;
                }
                columnCount++;
                cumulativeWidth += columnWidth + ColumnSpacing;
            }
            _columnCount = columnCount;
            var rowCount = Children.Count / _columnCount;
            if ((Children.Count % _columnCount > 0))
            {
                rowCount++;
            }

            _columnWidth = columnWidth;
            _rowHeight = rowHeight;
            _rowCount = rowCount;

            var size = new Size(_columnWidth * _columnCount + (_columnCount - 1) * ColumnSpacing, _rowHeight * rowCount + (rowCount - 1) * RowSpacing);
            return new SizeRequest(size, size);
        }

        protected override void LayoutChildren(double x, double y, double width, double height)
        {
            int row = 0;
            int column = 0;
            var totalColumnSpacing = (_columnCount - 1) * ColumnSpacing;
            var totalRowSpacing = (_rowCount - 1) * RowSpacing;
            var columnWidth = (width - totalColumnSpacing) / _columnCount;
            var rowHeight = (height - totalRowSpacing) / _rowCount;
            foreach (var child in Children)
            {
                var childX = column * columnWidth + column  * ColumnSpacing;
                var childY = row * _rowHeight + row * RowSpacing;
                LayoutChildIntoBoundingRegion(child, new Rectangle(x+childX, y+childY, columnWidth, rowHeight));
                column++;
                if (column >= _columnCount)
                {
                    column = 0;
                    row++;
                }
            }
        }
    }
}
