using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

//// TODO: Make this generic based off an interface that WrapPanel implements, then wrap that generic class for XAML usage.
namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    /// <summary>
    /// Merges this panel's layout process with the contents of an inner <see cref="ItemsPresenter"/> control's items. The parent <see cref="ItemsControl.ItemsPanel"/> must be set to <see cref="InsetPanel"/>.
    /// </summary>
    internal class CoordinatedWrapPanel : WrapPanel
    {
        internal InsetPanel ChildPanel { get; set; }

        // TODO: Does it make sense to have multiple ItemsPresenters supported?
        private ItemsPresenter _presenter; // TODO: Need to listen to collection change and re-do layout as the presenter itself won't change size to trigger it.

        protected override Size MeasureOverride(Size availableSize)
        {
            return MeasureOverrideInternal(availableSize, GetElements());
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            var elements = GetElements();

            if (_presenter != null)
            {
                _presenter.Arrange(new Rect(0, 0, finalSize.Width, finalSize.Height));
            }

            return ArrangeOverrideInternal(finalSize, elements);
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
    }
}
