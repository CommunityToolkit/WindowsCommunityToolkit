// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Microsoft.Toolkit.Uwp.UI.Extensions;
using Windows.Foundation;
using Windows.UI.Xaml.Controls;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    /// <summary>
    /// Panel paired with a <see cref="CoordinatablePanel"/>, must use InsetPanel as an <see cref="ItemsControl.ItemsPanel"/>. Also, there must be a parent which is a <see cref="ICoordinatingPanel"/> with <see cref="ICoordinatingPanel.IsCoordinating"/> set to true to function properly.
    /// </summary>
    public class InsetPanel : Panel
    {
        private CoordinatablePanel _parentPanel;

        /// <summary>
        /// Initializes a new instance of the <see cref="InsetPanel"/> class.
        /// </summary>
        public InsetPanel()
        {
            Loaded += this.InsetPanel_Loaded;
        }

        private void InsetPanel_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            // We need to find out parent panel to coordinate with and let them know we're here.
            _parentPanel = this.FindAscendant<CoordinatablePanel>();

            if (_parentPanel == null)
            {
                throw new InvalidOperationException("Must have parent CoordinatedPanel.");
            }

            _parentPanel.RegisterCoordinatedChild(this);
        }

        /// <inheritdoc>
        protected override Size MeasureOverride(Size availableSize)
        {
            // Whenever the ItemsPresenter modifies it's item, we'll be called here.
            // We need to instead have our Parent Coordinated Panel, re-layout everything.
            _parentPanel?.InvalidateMeasure();

            return _parentPanel?.DesiredSize ?? availableSize;
        }

        /// <inheritdoc>
        protected override Size ArrangeOverride(Size finalSize)
        {
            return _parentPanel?.RenderSize ?? finalSize;
        }
    }
}
