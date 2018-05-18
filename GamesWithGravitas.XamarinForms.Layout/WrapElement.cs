using Xamarin.Forms;

namespace GamesWithGravitas.XamarinForms.Layout
{
    public static class WrapElement
    {
        public static readonly BindableProperty ColumnSpacingProperty = 
            BindableProperty.Create(nameof(IWrapElement.RowSpacing), typeof(double), typeof(WrapElement), Grid.ColumnSpacingProperty.DefaultValue, propertyChanged: (bindable, oldValue, newValue) => ((IWrapElement)bindable).InvalidateLayout());

        public static readonly BindableProperty RowSpacingProperty = 
            BindableProperty.Create(nameof(IWrapElement.RowSpacing), typeof(double), typeof(WrapElement), Grid.RowSpacingProperty.DefaultValue, propertyChanged: (bindable, oldValue, newValue) => ((IWrapElement)bindable).InvalidateLayout());
    }

    public interface IWrapElement
    {
        double RowSpacing { get; set; }
        double ColumnSpacing { get; set; }

        void InvalidateLayout();
    }
}
