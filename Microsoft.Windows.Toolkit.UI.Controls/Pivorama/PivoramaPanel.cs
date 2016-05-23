using System;

using Microsoft.Windows.Toolkit.UI.Controls.Extensions;

using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace Microsoft.Windows.Toolkit.UI.Controls
{
    /// <summary>
    /// Defines the panel control for the <see cref="Pivorama"/>.
    /// </summary>
    public partial class PivoramaPanel : Panel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PivoramaPanel"/>.       
        /// </summary>
        public PivoramaPanel()
        {
            BuildPanes();
            HorizontalAlignment = HorizontalAlignment.Left;
        }

        /// <summary>
        /// Measures the size required for child elements of the <see cref="PivoramaPanel"/>.
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
            double itemWidth = ItemWidth;
            double maxHeight = 0;

            if (count > 0)
            {
                for (int n = 0; n < MaxItems; n++)
                {
                    var pane = Children[(index + n).Mod(MaxItems)] as ContentControl;
                    if (x < availableSize.Width + itemWidth * 2 && n <= count)
                    {
                        int inx = (index + n - 1).Mod(count);
                        pane.ContentTemplate = ItemTemplate;
                        pane.Content = _items[inx];
                        pane.Tag = inx;

                        pane.Measure(new Size(itemWidth, availableSize.Height));
                        if (n > 0 && x < availableSize.Width + itemWidth)
                        {
                            maxHeight = Math.Max(maxHeight, pane.DesiredSize.Height);
                        }
                        x += itemWidth;
                    }
                    else
                    {
                        pane.ContentTemplate = null;
                        pane.Content = null;
                        pane.Tag = null;

                        pane.Measure(new Size(itemWidth, availableSize.Height));
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
            int index = Index;
            int count = _items.Count;

            double x = 0;
            double itemWidth = ItemWidth;

            if (count > 0)
            {
                for (int n = 0; n < MaxItems; n++)
                {
                    var pane = Children[(index + n).Mod(MaxItems)] as ContentControl;
                    if (x < finalSize.Width)
                    {
                        pane.Arrange(new Rect(index * ItemWidth + x - itemWidth, 0, itemWidth, finalSize.Height));
                    }
                    else
                    {
                        break;
                    }
                    x += itemWidth;
                }
            }

            return new Size(0, finalSize.Height);
        }


        private void BuildPanes()
        {
            for (int n = 0; n < MaxItems; n++)
            {
                var pane = new ContentControl
                {
                    HorizontalContentAlignment = HorizontalAlignment.Stretch,
                    VerticalContentAlignment = VerticalAlignment.Stretch
                };

                pane.Tapped += Pane_Tapped;
                Children.Add(pane);
            }
        }

        private void Pane_Tapped(object sender, TappedRoutedEventArgs args)
        {
            if (SelectedIndexChanged != null)
            {
                var contentControl = sender as ContentControl;
                if (contentControl.Tag != null)
                {
                    SelectedIndexChanged(this, (int)contentControl.Tag);
                }
            }
        }
    }
}