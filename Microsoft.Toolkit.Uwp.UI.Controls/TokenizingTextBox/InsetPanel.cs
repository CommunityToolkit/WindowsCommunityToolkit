using Microsoft.Toolkit.Uwp.UI.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Xaml.Controls;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    /// <summary>
    /// Panel paired with an <see cref="CoordinatedWrapPanel"/>, use as ItemsPanel. Must have a parent which is a CoordinatedPanel to function.
    /// </summary>
    internal class InsetPanel : Panel
    {
        private CoordinatedWrapPanel _parentPanel; // TODO: Use generic interface/base class here so can be generalized.

        public InsetPanel()
        {
            Loaded += this.InsetPanel_Loaded;
        }

        private void InsetPanel_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            _parentPanel = this.FindAscendant<CoordinatedWrapPanel>();

            if (_parentPanel == null)
            {
                throw new InvalidOperationException("Must have parent CoordinatedPanel.");
            }

            _parentPanel.ChildPanel = this;
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            // Whenever the ItemsPresenter modifies it's item, we'll be called here.
            // We need to instead have our Parent Coordinated Panel, re-layout everything.
            _parentPanel?.InvalidateMeasure();

            return _parentPanel?.DesiredSize ?? availableSize;
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            return _parentPanel?.RenderSize ?? finalSize;
        }
    }
}
