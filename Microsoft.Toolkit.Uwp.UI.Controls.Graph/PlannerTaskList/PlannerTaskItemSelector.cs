using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Microsoft.Toolkit.Uwp.UI.Controls.Graph
{
    internal class PlannerTaskItemSelector : DataTemplateSelector
    {
        public DataTemplate NormalTemplate { get; set; }

        public DataTemplate ActiveTemplate { get; set; }

        protected override DataTemplate SelectTemplateCore(object item, DependencyObject container)
        {
            ListViewItem itemContainer = container as ListViewItem;
            DataTemplate dataTemplate = NormalTemplate;
            if (item is bool && (bool)item)
            {
                dataTemplate = ActiveTemplate;
            }

            if (itemContainer != null)
            {
                itemContainer.ContentTemplate = dataTemplate;
            }

            return dataTemplate;
        }
    }
}
