using System;
using System.Collections;
using System.Collections.Specialized;
using Xamarin.Forms;

namespace GamesWithGravitas.XamarinForms.Layout
{
    public class Layout
    {
        public static readonly BindableProperty ItemsSourceProperty =
            BindableProperty.CreateAttached("ItemsSource", typeof(IEnumerable), typeof(Layout), null, propertyChanged: (bindable, oldValue, newValue) => UpdateChildren((Layout<View>)bindable));

        public static readonly BindableProperty ItemTemplateProperty =
            BindableProperty.CreateAttached("ItemTemplate", typeof(DataTemplate), typeof(Layout), null, propertyChanged: (bindable, oldValue, newValue) => UpdateChildren((Layout<View>)bindable));

        public static readonly BindableProperty SeparatorTemplateProperty =
            BindableProperty.CreateAttached("SeparatorTemplate", typeof(DataTemplate), typeof(Layout), null, propertyChanged: (bindable, oldValue, newValue) => UpdateChildren((Layout<View>)bindable));

        public static IEnumerable GetItemsSource(BindableObject bindable) => (IEnumerable)bindable.GetValue(ItemsSourceProperty);
        public static void SetItemsSource(BindableObject bindable, IEnumerable value) => bindable.SetValue(ItemsSourceProperty, value);

        public static DataTemplate GetItemTemplate(BindableObject bindable) => (DataTemplate)bindable.GetValue(ItemTemplateProperty);
        public static void SetItemTemplate(BindableObject bindable, DataTemplate value) => bindable.SetValue(ItemTemplateProperty, value);

        public static DataTemplate GetSeparatorTemplate(BindableObject bindable) => (DataTemplate)bindable.GetValue(SeparatorTemplateProperty);
        public static void SetSeparatorTemplate(BindableObject bindable, DataTemplate value) => bindable.SetValue(SeparatorTemplateProperty, value);

        private static readonly BindableProperty CollectionChangedListenerProperty =
            BindableProperty.CreateAttached("CollectionChangedListener", typeof(CollectionChangedListener), typeof(Layout), null, propertyChanging: OnCollectionChangedListenerChanging);

        private static CollectionChangedListener GetCollectionChangedListener(BindableObject bindable) => (CollectionChangedListener)bindable.GetValue(CollectionChangedListenerProperty);
        private static void SetCollectionChangedListener(BindableObject bindable, CollectionChangedListener value) => bindable.SetValue(CollectionChangedListenerProperty, value);

        private static void UpdateChildren(Layout<View> layout)
        {
            var items = GetItemsSource(layout);
            var template = GetItemTemplate(layout);
            if (items == null || template == null)
            {
                return;
            }

            var separatorTemplate = GetSeparatorTemplate(layout);
            var hasSeparators = separatorTemplate != null;

            layout.Children.Clear();

            View separator = null;

            foreach (var item in items)
            {
                DataTemplate itemTemplate;
                if (template is DataTemplateSelector selector)
                {
                    itemTemplate = selector.SelectTemplate(item, layout);
                }
                else
                {
                    itemTemplate = template;
                }
                var child = (View)itemTemplate.CreateContent();
                child.BindingContext = item;
                layout.Children.Add(child);
                if (hasSeparators)
                {
                    separator = (View)separatorTemplate.CreateContent();
                    layout.Children.Add(separator);
                }
            }

            if (hasSeparators && separator != null)
            {
                layout.Children.Remove(separator);
            }

            if (items is INotifyCollectionChanged collection)
            {
                SetCollectionChangedListener(layout, new CollectionChangedListener(layout, collection));
            }
        }

        private static void OnCollectionChangedListenerChanging(BindableObject bindable, object oldValue, object newValue)
        {
            (oldValue as CollectionChangedListener)?.Dispose();
        }

        private class CollectionChangedListener : IDisposable
        {
            private WeakReference<Layout<View>> _layoutReference;
            private WeakReference<INotifyCollectionChanged> _collectionReference;

            public CollectionChangedListener(Layout<View> layout, INotifyCollectionChanged collection)
            {
                _layoutReference = new WeakReference<Layout<View>>(layout);
                SetupCollectionChanged(collection);
            }

            private void SetupCollectionChanged(INotifyCollectionChanged collection)
            {
                INotifyCollectionChanged oldCollection = null;
                _collectionReference?.TryGetTarget(out oldCollection);
                if (oldCollection != null)
                {
                    oldCollection.CollectionChanged -= OnItemsSourceCollectionChanged;
                }
                if (collection != null)
                {
                    collection.CollectionChanged += OnItemsSourceCollectionChanged;
                }
                _collectionReference = new WeakReference<INotifyCollectionChanged>(collection);
            }

            private void OnItemsSourceCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
            {
                _layoutReference.TryGetTarget(out Layout<View> layout);

                if (layout == null)
                {
                    return;
                }

                var items = GetItemsSource(layout);
                var template = GetItemTemplate(layout);

                switch (e.Action)
                {
                    case NotifyCollectionChangedAction.Add:
                        for(int i=0; i<e.NewItems.Count; i++)
                        {
                            var view = (View)template.CreateContent();
                            view.BindingContext = e.NewItems[i];
                            layout.Children.Insert(e.NewStartingIndex + i, view);
                        }
                        break;
                    case NotifyCollectionChangedAction.Move:
                        break;
                    case NotifyCollectionChangedAction.Remove:
                        for (int i = 0; i < e.OldItems.Count; i++)
                        {
                            layout.Children.RemoveAt(e.OldStartingIndex);
                        }
                        break;
                    case NotifyCollectionChangedAction.Replace:
                        break;
                    case NotifyCollectionChangedAction.Reset:
                        break;
                }
            }

            public void Dispose()
            {
                INotifyCollectionChanged collection = null;
                _collectionReference?.TryGetTarget(out collection);
                if (collection != null)
                {
                    collection.CollectionChanged -= OnItemsSourceCollectionChanged;
                }
            }
        }
    }
}
