using Xamarin.Forms;

namespace GamesWithGravitas.XamarinForms.Layout
{
    public static class WrapElement
    {
        public static BindableProperty ColumnSpacingProperty = 
            BindableProperty.Create(nameof(IWrapElement.ColumnSpacing), typeof(double), typeof(WrapElement), Grid.ColumnSpacingProperty.DefaultValue, propertyChanged: (bindable, oldValue, newValue) => ((IWrapElement)bindable).InvalidateLayout());

        public static BindableProperty RowSpacingProperty = 
            BindableProperty.Create(nameof(IWrapElement.RowSpacing), typeof(double), typeof(WrapElement), Grid.RowSpacingProperty.DefaultValue, propertyChanged: (bindable, oldValue, newValue) => ((IWrapElement)bindable).InvalidateLayout());

        public static BindableProperty ChildMaximumWidthProperty =
            BindableProperty.CreateAttached("ChildMaximumWidth", typeof(double), typeof(WrapElement), double.PositiveInfinity);

        public static double GetChildMaximumWidth(BindableObject wrap) => (double)wrap.GetValue(ChildMaximumWidthProperty);
        public static void SetChildMaximumWidth(BindableObject wrap, double value) => wrap.SetValue(ChildMaximumWidthProperty, value);
    }

    public interface IWrapElement
    {
        double ColumnSpacing { get; set; }
        double RowSpacing { get; set; }

        void InvalidateLayout();
    }
}
