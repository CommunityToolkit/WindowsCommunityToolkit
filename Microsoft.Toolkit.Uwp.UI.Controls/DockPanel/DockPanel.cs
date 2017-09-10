using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    /// <summary>
    /// Defines an area where you can arrange child elements either horizontally or vertically, relative to each other.
    /// </summary>
    public partial class DockPanel:Panel
    {
        private static void DockChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            // get parent and update.
        }

        private static void LastChildFillChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            // update dock panel view.
        }
    }
}
