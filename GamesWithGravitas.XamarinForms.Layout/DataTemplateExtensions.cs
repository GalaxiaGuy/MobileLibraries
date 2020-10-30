using Xamarin.Forms;

namespace GamesWithGravitas.XamarinForms.Layout
{
    internal static class DataTemplateExtensions
    {
        public static DataTemplate SelectDataTemplate(this DataTemplate self, object item, BindableObject container)
        {
            return !(self is DataTemplateSelector selector) ? self : selector.SelectTemplate(item, container);
        }

        public static object CreateContent(this DataTemplate self, object item, BindableObject container)
        {
            return self.SelectDataTemplate(item, container).CreateContent();
        }
    }
}