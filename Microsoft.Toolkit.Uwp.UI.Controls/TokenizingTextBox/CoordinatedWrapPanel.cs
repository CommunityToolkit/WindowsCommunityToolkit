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
    /// Controls the layout for a set of controls along with an <see cref="InsetPanel"/>.
    /// </summary>
    internal class CoordinatedWrapPanel : WrapPanel
    {
        internal InsetPanel ChildPanel { get; set; }

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

        private IEnumerable<UIElement> GetElements()
        {
            var elements = new List<UIElement>();
            if (ChildPanel != null)
            {
                foreach (var child in ChildPanel.Children)
                {
                    elements.Add(child);
                }
            }

            foreach (var child in Children)
            {
                if (!(child is ItemsPresenter))
                {
                    elements.Add(child);
                }
                else
                {
                    _presenter = child as ItemsPresenter;
                }
            }

            return elements;
        }
    }
}
