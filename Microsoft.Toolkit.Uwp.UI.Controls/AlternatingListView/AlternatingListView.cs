using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    /// <summary>
    /// Control derived from ListView that will set a different background alternating.
    /// </summary>
    public partial class AlternatingListView : ListView
    {
        /// <summary>
        /// Change the background color of the ListView item being added to the ListView container.
        /// </summary>
        protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
        {
            base.PrepareContainerForItemOverride(element, item);
            ListViewItem listViewItem = element as ListViewItem;

            if (listViewItem == null)
            {
                return;
            }

            int index = IndexFromContainer(element);
            listViewItem.Background = (index + 1) % 2 == 1 ? OddRowBackground : EvenRowBackground;
        }
    }
}
