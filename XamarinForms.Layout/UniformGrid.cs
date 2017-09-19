using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Xamarin.Forms;

namespace GamesWithGravitas.XamarinForms
{
    public class UniformGrid : Layout<View>
    {
        public static readonly BindableProperty RowProperty = Grid.RowProperty;

        public static readonly BindableProperty RowSpanProperty = Grid.RowSpanProperty;

        public static readonly BindableProperty ColumnProperty = Grid.ColumnProperty;

        public static readonly BindableProperty ColumnSpanProperty = Grid.ColumnSpanProperty;

        public static readonly BindableProperty SpacingProperty = BindableProperty.Create(nameof(Spacing), typeof(double), typeof(UniformGrid), 6d,
            propertyChanged: (bindable, oldValue, newValue) => ((UniformGrid)bindable).InvalidateLayout());

        public static readonly BindableProperty ColumnCountProperty = BindableProperty.Create(nameof(ColumnCount), typeof(int), typeof(UniformGrid), 1,
            propertyChanged: (bindable, oldValue, newValue) => ((UniformGrid)bindable).InvalidateLayout());

        public static readonly BindableProperty RowCountProperty = BindableProperty.Create(nameof(RowCount), typeof(int), typeof(UniformGrid), 1,
            propertyChanged: (bindable, oldValue, newValue) => ((UniformGrid)bindable).InvalidateLayout());

        public double Spacing
        {
            get { return (double)GetValue(SpacingProperty); }
            set { SetValue(SpacingProperty, value); }
        }
        public static int GetColumn(BindableObject bindable)
        {
            return (int)bindable.GetValue(ColumnProperty);
        }

        public static int GetColumnSpan(BindableObject bindable)
        {
            return (int)bindable.GetValue(ColumnSpanProperty);
        }

        public static int GetRow(BindableObject bindable)
        {
            return (int)bindable.GetValue(RowProperty);
        }

        public static int GetRowSpan(BindableObject bindable)
        {
            return (int)bindable.GetValue(RowSpanProperty);
        }

        public static void SetColumn(BindableObject bindable, int value)
        {
            bindable.SetValue(ColumnProperty, value);
        }

        public static void SetColumnSpan(BindableObject bindable, int value)
        {
            bindable.SetValue(ColumnSpanProperty, value);
        }

        public static void SetRow(BindableObject bindable, int value)
        {
            bindable.SetValue(RowProperty, value);
        }

        public static void SetRowSpan(BindableObject bindable, int value)
        {
            bindable.SetValue(RowSpanProperty, value);
        }

        public int RowCount
        {
            get => (int)GetValue(RowCountProperty);
            set => SetValue(RowCountProperty, value);
        }

        public int ColumnCount
        {
            get => (int)GetValue(ColumnCountProperty);
            set => SetValue(ColumnCountProperty, value);
        }

        protected override void OnChildAdded(Element child)
        {
            base.OnChildAdded(child);
            child.PropertyChanged += ChildPropertyChanged;
        }

        protected override void OnChildRemoved(Element child)
        {
            child.PropertyChanged -= ChildPropertyChanged;
            base.OnChildRemoved(child);
        }

        private void ChildPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == RowProperty.PropertyName ||
                e.PropertyName == RowSpanProperty.PropertyName ||
                e.PropertyName == ColumnProperty.PropertyName ||
                e.PropertyName == ColumnSpanProperty.PropertyName)
            {
                LayoutChild((View)sender);
            }
        }

        protected override SizeRequest OnMeasure(double widthConstraint, double heightConstraint)
        {
            if (double.IsInfinity(heightConstraint) && double.IsInfinity(heightConstraint))
            {
                if (Children.Count == 0)
                {
                    return new SizeRequest(Size.Zero);
                }
                var sizeRequest = Children[0].Measure(widthConstraint, heightConstraint, MeasureFlags.IncludeMargins);
                var cellSize = Math.Max(sizeRequest.Request.Width, sizeRequest.Request.Height);
                var width = ColumnCount * cellSize + Padding.Left + Padding.Right + (ColumnCount - 1) * cellSize;
                var height = RowCount * cellSize + Padding.Top + Padding.Bottom + (RowCount - 1) * cellSize;
                return new SizeRequest(new Size(width, height));
            }

            if (double.IsInfinity(widthConstraint))
            {
                return new SizeRequest(new Size(heightConstraint * ColumnCount / RowCount, heightConstraint));
            }
            if (double.IsInfinity(heightConstraint))
            {
                return new SizeRequest(new Size(widthConstraint, widthConstraint * RowCount / ColumnCount));
            }

            if (double.IsInfinity(widthConstraint))
            {
                return new SizeRequest(new Size(heightConstraint * ColumnCount / RowCount, heightConstraint));
            }

            var heightBasedOnWidth = widthConstraint * RowCount / ColumnCount;
            var widthBasedOnHeight = heightConstraint * ColumnCount / RowCount;

            if (heightBasedOnWidth < heightConstraint)
            {
                return new SizeRequest(new Size(widthConstraint, heightBasedOnWidth));
            }
            return new SizeRequest(new Size(widthBasedOnHeight, heightConstraint));
        }

        private double _layoutCellSize;
        private double _layoutX;
        private double _layoutY;

        protected override void LayoutChildren(double x, double y, double width, double height)
        {
            var availableHeight = height - Padding.Top - Padding.Bottom - (RowCount - 1) * Spacing;
            var availableWidth = width - Padding.Left - Padding.Right - (ColumnCount - 1) * Spacing;
            var cellWidth = availableWidth / ColumnCount;
            var cellHeight = availableHeight / RowCount;

            _layoutCellSize = Math.Min(cellWidth, cellHeight);

            _layoutX = x + Padding.Left;
            _layoutY = y + Padding.Right;

            foreach (var child in Children)
            {
                if (!child.IsVisible)
                {
                    continue;
                }
                LayoutChild(child);
            }
        }

        private void LayoutChild(View child)
        {
            var rect = new Rectangle(
                _layoutX + GetColumn(child) * (_layoutCellSize + Spacing),
                _layoutY + GetRow(child) * ( _layoutCellSize + Spacing),
                _layoutCellSize * GetColumnSpan(child) + Spacing * (GetColumnSpan(child) - 1),
                _layoutCellSize * GetRowSpan(child) + Spacing * (GetRowSpan(child) - 1));
            LayoutChildIntoBoundingRegion(child, rect);
        }
    }
}
