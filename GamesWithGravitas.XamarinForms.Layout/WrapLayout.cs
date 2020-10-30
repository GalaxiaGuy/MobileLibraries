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

        public static readonly BindableProperty DistributeProperty =
            BindableProperty.Create(nameof(Distribute), typeof(bool), typeof(WrapLayout), false, propertyChanged: (bindable, oldValue, newValue) => ((IWrapElement)bindable).InvalidateLayout());

        public static readonly BindableProperty IncludeInvisibleChildrenProperty =
            BindableProperty.Create(nameof(IncludeInvisibleChildren), typeof(bool), typeof(WrapLayout), true, propertyChanged: (bindable, oldValue, newValue) => ((WrapLayout)bindable).InvalidateLayout());

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

        public bool Distribute
        {
            get => (bool)GetValue(DistributeProperty);
            set => SetValue(DistributeProperty, value);
        }

        public bool IncludeInvisibleChildren
        {
            get => (bool)GetValue(IncludeInvisibleChildrenProperty);
            set => SetValue(IncludeInvisibleChildrenProperty, value);
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
                if (!IncludeInvisibleChildren && !child.IsVisible)
                {
                    continue;
                }

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
            List<CrossAxisItem> crossAxisItems = new List<CrossAxisItem>();
            List<SizedView> crossAxisChildren = new List<SizedView>();
            double mainAxisLength = 0;
            double crossAxisLength = 0;
            int mainAxisIndex = 0;
            foreach (var child in Children)
            {
                if (!IncludeInvisibleChildren && !child.IsVisible)
                {
                    continue;
                }

                var sizeRequest = child.Measure(width, height, MeasureFlags.IncludeMargins);
                if (Orientation == StackOrientation.Horizontal)
                {
                    if (mainAxisLength + ColumnSpacing + sizeRequest.Request.Width > width)
                    {
                        crossAxisItems.Add(new CrossAxisItem(Orientation, ColumnSpacing, crossAxisChildren, x, y, crossAxisLength, width));
                        crossAxisChildren = new List<SizedView>();
                        y += crossAxisLength + RowSpacing;
                        mainAxisLength = sizeRequest.Request.Width;
                        crossAxisLength = sizeRequest.Request.Height;
                        mainAxisIndex++;
                    }
                    else
                    {
                        mainAxisLength += sizeRequest.Request.Width + ColumnSpacing;
                        crossAxisLength = Math.Max(crossAxisLength, sizeRequest.Request.Height);
                    }
                    crossAxisChildren.Add(new SizedView(child, sizeRequest.Request));
                }
                else
                {
                    if (mainAxisLength + RowSpacing + sizeRequest.Request.Height > height)
                    {
                        crossAxisItems.Add(new CrossAxisItem(Orientation, ColumnSpacing, crossAxisChildren, x, y, crossAxisLength, height));
                        crossAxisChildren = new List<SizedView>();
                        x += crossAxisLength + ColumnSpacing;
                        mainAxisLength = sizeRequest.Request.Height;
                        crossAxisLength = sizeRequest.Request.Width;
                        mainAxisIndex++;
                    }
                    else
                    {
                        mainAxisLength += sizeRequest.Request.Height + RowSpacing;
                        crossAxisLength = Math.Max(crossAxisLength, sizeRequest.Request.Width);
                    }
                    crossAxisChildren.Add(new SizedView(child, sizeRequest.Request));
                }
            }
            if (Orientation == StackOrientation.Horizontal)
            {
                crossAxisItems.Add(new CrossAxisItem(Orientation, ColumnSpacing, crossAxisChildren, x, y, crossAxisLength, width));
                if (Distribute)
                {
                    crossAxisItems.Distribute();
                }
                foreach (var item in crossAxisItems)
                {
                    LayoutRow(item);
                }
            }
            else
            {
                crossAxisItems.Add(new CrossAxisItem(Orientation, RowSpacing, crossAxisChildren, x, y, crossAxisLength, height));
                if (Distribute)
                {
                    crossAxisItems.Distribute();
                }
                foreach (var item in crossAxisItems)
                {
                    LayoutColumn(item);
                }
            }
        }

        private void LayoutRow(CrossAxisItem item)
        {
            var (children, x, y, _, height, width) = item;

            var minimumWidth = children.Sum(c => c.Size.Width);
            var extraAvailableWidth = width - minimumWidth;
            var expandingChildren = children.Where(c => c.View.HorizontalOptions.Expands);
            var expandingChlidCount = expandingChildren.Count();
            var expandingChildWidth = expandingChildren.Sum(c => c.Size.Width);

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

        private void LayoutColumn(CrossAxisItem item)
        {
            var (children, x, y, _, width, height) = item;

            var minimumHeight = children.Sum(c => c.Size.Height);
            var extraAvailableHeight = height - minimumHeight;
            var expandingChildren = children.Where(c => c.View.VerticalOptions.Expands);
            var expandingChlidCount = expandingChildren.Count();
            var expandingChildHeight = expandingChildren.Sum(c => c.Size.Height);

            var spareHeight = height - minimumHeight - (children.Count - 1) * RowSpacing;
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

    /// <summary>
    /// A row when vertical, a column when horizontal
    /// </summary>
    internal class CrossAxisItem
    {
        public StackOrientation Orientation { get; }
        public double Spacing { get; }
        public List<SizedView> Children { get; }
        public double X { get; }
        public double Y { get; }
        public double MaximumMainAxisLength { get; }
        public double CrossAxisLength { get; }
        public double TotalSpacing => Children?.Count > 0 ? (Children.Count - 1) * Spacing : 0;
        public double MainAxisLength => Children?.Count > 0 ? Children.Sum(x => x.MainAxisLength(Orientation)) + TotalSpacing : 0;

        public CrossAxisItem(StackOrientation orientation, double spacing, List<SizedView> chlidren, double x, double y, double crossAxisLength, double maximumMainAxisLength)
        {
            Orientation = orientation;
            Spacing = spacing;
            Children = chlidren;
            X = x;
            Y = y;
            CrossAxisLength = crossAxisLength;
            MaximumMainAxisLength = maximumMainAxisLength;
        }

        public void Deconstruct(out List<SizedView> children, out double x, out double y, out double mainAxisLength, out double crossAxisLength, out double maximumMainAxisLength)
        {
            children = Children;
            x = X;
            y = Y;
            crossAxisLength = CrossAxisLength;
            mainAxisLength = MainAxisLength;
            maximumMainAxisLength = MaximumMainAxisLength;
        }
    }

    internal class SizedView
    {
        public View View;
        public Size Size;

        public SizedView(View view, Size size)
        {
            View = view;
            Size = size;
        }

        public void Deconstruct(out View view, out Size size)
        {
            view = View;
            size = Size;
        }

        public double MainAxisLength(StackOrientation orientation) => orientation == StackOrientation.Horizontal ? Size.Width : Size.Height;
    }

    internal static class CrossAxisItemExtensions
    {
        public static void Distribute(this IList<CrossAxisItem> items)
        {
            if (items?.Any() != true)
            {
                return;
            }
            var orientation = items[0].Orientation;
            var spacing = items[0].Spacing;
            for (int i = 0; i < items.Count - 1; i++)
            {
                var thisItem = items[i];
                var nextItem = items[i + 1];

                while (true)
                {
                    if (thisItem.Children == null)
                    {
                        break;
                    }
                    var candidate = thisItem.Children.LastOrDefault();
                    if (candidate == null)
                    {
                        break;
                    }

                    var thisItemCurrentLength = thisItem.MainAxisLength;
                    var nextItemCurrentLength = nextItem.MainAxisLength;
                    var currentDiff = Math.Abs(thisItemCurrentLength - nextItemCurrentLength);

                    var thisItemCandidateLength = thisItemCurrentLength - candidate.MainAxisLength(orientation) - spacing;
                    var nextItemCandidateLength = nextItemCurrentLength + candidate.MainAxisLength(orientation) - spacing;
                    var candidateDiff = Math.Abs(thisItemCandidateLength - nextItemCandidateLength);

                    if (candidateDiff < currentDiff)
                    {
                        thisItem.Children.Remove(candidate);
                        nextItem.Children.Insert(0, candidate);
                    }
                    else
                    {
                        break;
                    }
                }
            }
        }
    }
}
