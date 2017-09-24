using Xamarin.Forms;

namespace GamesWithGravitas.XamarinForms.Layout
{
    public static class WrapElement
    {
        public static BindableProperty ColumnSpacingProperty = 
            BindableProperty.Create(nameof(IWrapElement.ColumnSpacing), typeof(double), typeof(WrapElement), Grid.ColumnSpacingProperty.DefaultValue, propertyChanged: (bindable, oldValue, newValue) => ((IWrapElement)bindable).InvalidateLayout());

        public static BindableProperty RowSpacingProperty = 
            BindableProperty.Create(nameof(IWrapElement.RowSpacing), typeof(double), typeof(WrapElement), Grid.RowSpacingProperty.DefaultValue, propertyChanged: (bindable, oldValue, newValue) => ((IWrapElement)bindable).InvalidateLayout());
    }

    public interface IWrapElement
    {
        double ColumnSpacing { get; set; }
        double RowSpacing { get; set; }

        void InvalidateLayout();
    }
}
