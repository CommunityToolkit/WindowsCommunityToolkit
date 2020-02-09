// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    /// <summary>
    /// Merges this panel's layout process with the contents of an inner <see cref="ItemsPresenter"/> control's items. The parent <see cref="ItemsControl.ItemsPanel"/> must be set to an <see cref="InsetPanel"/> and this panels <see cref="IsCoordinating"/> property must be set to true.
    /// </summary>
    public abstract class CoordinatablePanel : Panel, ICoordinatingPanel
    {
        internal InsetPanel ChildPanel { get; set; }

        /// <inheritdoc/>
        public bool IsCoordinating { get; set; }

        // TODO: Does it make sense to have multiple ItemsPresenters supported?
        private ItemsPresenter _presenter; // TODO: Need to listen to collection change and re-do layout as the presenter itself won't change size to trigger it.

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

                if (_presenter != null)
                {
                    _presenter.Arrange(new Rect(0, 0, finalSize.Width, finalSize.Height));
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
                if (child is ItemsPresenter)
                {
                    // TODO: Check that we only ever have one ItemsPresenter?
                    _presenter = child as ItemsPresenter;

                    if (ChildPanel != null)
                    {
                        foreach (var innerChild in ChildPanel.Children)
                        {
                            elements.Add(innerChild);
                        }
                    }
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
    }
}
