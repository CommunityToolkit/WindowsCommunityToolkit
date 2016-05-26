using System;

using Windows.Foundation;
using Windows.UI.Xaml.Controls;

namespace Microsoft.Windows.Toolkit.UI.Controls
{
    /// <summary>
    /// Defines a tab panel control for the <see cref="Pivorama"/>.
    /// </summary>
    public partial class PivoramaTabs : PivoramaPanel
    {
        /// <summary>
        /// Measures the size required for child elements of the <see cref="PivoramaTabs"/>.
        /// </summary>
        /// <param name="availableSize">
        /// The available size that can be provided for child elements.
        /// </param>
        /// <returns>
        /// Returns the calculated size this control needs based on child elements.
        /// </returns>
        protected override Size MeasureOverride(Size availableSize)
        {
            int index = Index;
            int count = _items.Count;

            double x = 0;
            double maxHeight = 0;

            if (count > 0)
            {
                for (int n = 0; n < MaxItems; n++)
                {
                    var pane = Children[(index + n).Mod(MaxItems)] as ContentControl;
                    if (n <= count)
                    {
                        int inx = (index + n - 1).Mod(count);
                        pane.ContentTemplate = n == 1 ? ItemTemplate : SelectedItemTemplate;
                        pane.Content = _items[inx];
                        pane.Tag = inx;

                        pane.Measure(availableSize);
                        maxHeight = Math.Max(maxHeight, pane.DesiredSize.Height);
                        x += pane.DesiredSize.Width;
                    }
                    else
                    {
                        pane.ContentTemplate = null;
                        pane.Content = null;
                        pane.Tag = null;
                        pane.Measure(availableSize);
                    }

                    if (n == 0)
                    {
                        PrevTabWidth = pane.DesiredSize.Width;
                    }
                    if (n == 1)
                    {
                        SelectedTabWidth = pane.DesiredSize.Width;
                    }
                }
            }

            return new Size(x, maxHeight);
        }

        /// <summary>
        /// Arranges the panel and contained child elements.
        /// </summary>
        /// <param name="finalSize">
        /// The final size contained within the panel that should be used to arrange itself and child elements.
        /// </param>
        /// <returns>
        /// Returns the size that was used.
        /// </returns>
        protected override Size ArrangeOverride(Size finalSize)
        {
            int inx = Index;
            int count = _items.Count;

            double x = 0;

            if (count > 0)
            {
                for (int n = 0; n < MaxItems; n++)
                {
                    var pane = Children[(inx + n).Mod(MaxItems)] as ContentControl;
                    if (n == 0)
                    {
                        x = -pane.DesiredSize.Width;
                    }

                    pane.Arrange(new Rect(x, 0, pane.DesiredSize.Width, finalSize.Height));
                    x += pane.DesiredSize.Width;
                }
            }

            return new Size(0, finalSize.Height);
        }
    }
}