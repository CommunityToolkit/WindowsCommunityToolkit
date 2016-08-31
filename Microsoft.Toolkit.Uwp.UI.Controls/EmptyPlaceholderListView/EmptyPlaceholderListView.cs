using System.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    /// <summary>
    /// Extension of ListView that allows an empty template when no items present
    /// </summary>
    [TemplatePart(Name = PartEmptyContainer, Type = typeof(ContentPresenter))]
    public class EmptyPlaceholderListView : ListView
    {
        private ContentPresenter emptyContainer;
        public static readonly DependencyProperty EmptyTemplateProperty = DependencyProperty.Register(
            "EmptyTemplate",
            typeof(DataTemplate),
            typeof(EmptyPlaceholderListView),
            new PropertyMetadata(null));

        private const string PartEmptyContainer = "EmptyContainer";

        public DataTemplate EmptyTemplate
        {
            get { return (DataTemplate)GetValue(EmptyTemplateProperty); }
            set { SetValue(EmptyTemplateProperty, value); }
        }

        public EmptyPlaceholderListView()
        {
            DefaultStyleKey = typeof(EmptyPlaceholderListView);
        }

        protected override void OnApplyTemplate()
        {
            emptyContainer = (ContentPresenter)GetTemplateChild("EmptyContainer");
            UpdateEmptyContainer();

            base.OnApplyTemplate();
        }

        protected override void OnItemsChanged(object e)
        {
            base.OnItemsChanged(e);
            UpdateEmptyContainer();
        }

        private void UpdateEmptyContainer()
        {
            int count = 0;
            if (emptyContainer != null && EmptyTemplate != null)
            {
                var collection = ItemsSource as ICollection;
                if (collection != null)
                {
                    count = collection.Count;
                }
                else if (Items != null)
                {
                    count = Items.Count;
                }

                emptyContainer.ContentTemplate = count == 0 ? EmptyTemplate : null;
            }
        }
    }
}
