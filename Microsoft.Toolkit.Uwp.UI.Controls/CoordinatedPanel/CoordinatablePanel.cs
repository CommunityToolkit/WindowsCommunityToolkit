// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using Microsoft.Toolkit.Uwp.UI.Extensions;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    /// <summary>
    /// Merges this panel's layout process with the contents of an inner <see cref="ItemsPresenter"/> control's items. The parent <see cref="ItemsControl.ItemsPanel"/> must be set to an <see cref="InsetPanel"/> and this panels <see cref="IsCoordinating"/> property must be set to true. This scenario meant for use within the <see cref="ControlTemplate"/> of an <see cref="ItemsControl"/>.
    /// </summary>
    public abstract class CoordinatablePanel : Panel, ICoordinatingPanel
    {
        // TODO: Does it make sense to have multiple ItemsPresenters supported?
        private InsetPanel _insetPanel;
        private ItemsPresenter _coordinatedItemsPresenter;
        private ItemsControl _itemsController;

        /// <inheritdoc/>
        public bool IsCoordinating { get; set; }

        /// <inheritdoc/>
        protected override Size MeasureOverride(Size availableSize)
        {
            return MeasureElements(availableSize, IsCoordinating ? GetElements() : Children);
        }

        /// <inheritdoc/>
        protected override Size ArrangeOverride(Size finalSize)
        {
            if (IsCoordinating)
            {
                var elements = GetElements();

                // During arrange we make sure to make our coordinated ItemsPresenter the same size as our control so we can layout its items within our same airspace.
                if (_coordinatedItemsPresenter != null)
                {
                    _coordinatedItemsPresenter.Arrange(new Rect(0, 0, finalSize.Width, finalSize.Height));
                }

                return ArrangeElements(finalSize, elements);
            }

            return ArrangeElements(finalSize, Children);
        }

        // TODO: Optimize for watching changes to Children of parent/child?
        private IEnumerable<UIElement> GetElements()
        {
            var elements = new List<UIElement>();

            foreach (var child in Children)
            {
                if (_insetPanel != null && child == _coordinatedItemsPresenter)
                {
                    // Inject our inset panel's children into our total collection of elements to measure/arrange in the spot where their ItemsPresenter is.
                    foreach (var innerChild in _insetPanel.Children)
                    {
                        elements.Add(innerChild);
                    }
                }
                else if (child is ItemsPresenter)
                {
                    // Skip
                    // TODO: Should figure out if we need a way to still include these
                    // otherwise if we try and include the one that has the InsetPanel we
                    // cause layout issues, we just haven't been notified yet in the initial
                    // loading process.
                }
                else
                {
                    elements.Add(child);
                }
            }

            return elements;
        }

        /// <inheritdoc/>
        public abstract Size MeasureElements(Size availableSize, IEnumerable<UIElement> elements);

        /// <inheritdoc/>
        public abstract Size ArrangeElements(Size finalSize, IEnumerable<UIElement> elements);

        /// <inheritdoc/>
        public void RegisterCoordinatedChild(InsetPanel insetPanel)
        {
            _insetPanel = insetPanel;

            _coordinatedItemsPresenter = _insetPanel.FindAscendant<ItemsPresenter>();

            if (_itemsController != null && _itemsController.Items != null)
            {
                _itemsController.Items.VectorChanged -= Items_VectorChanged;
            }

            _itemsController = _coordinatedItemsPresenter.FindAscendant<ItemsControl>();

            if (_itemsController != null && _itemsController.Items != null)
            {
                _itemsController.Items.VectorChanged += Items_VectorChanged;
            }
        }

        private void Items_VectorChanged(Windows.Foundation.Collections.IObservableVector<object> sender, Windows.Foundation.Collections.IVectorChangedEventArgs @event)
        {
            // Update Panel when any items change in our parent ItemsControl.
            InvalidateMeasure();
        }
    }
}
