using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    /// <summary>
    /// The ItemContainerGenerator provides useful utilities for ItemsControls.
    /// </summary>
    /// <QualityBand>Preview</QualityBand>
    internal sealed partial class ItemsControlHelper
    {
        /// <summary>
        /// Prepares the specified container to display the specified item.
        /// </summary>
        /// <param name="element">
        /// Container element used to display the specified item.
        /// </param>
        /// <param name="parentItemContainerStyle">
        /// The ItemContainerStyle for the parent ItemsControl.
        /// </param>
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "Code is linked into multiple projects.")]
        internal static void PrepareContainerForItemOverride(DependencyObject element, Style parentItemContainerStyle)
        {
            // Apply the ItemContainerStyle to the item
            Control control = element as Control;
            if (parentItemContainerStyle != null && control != null && control.Style == null)
            {
                control.SetValue(FrameworkElement.StyleProperty, parentItemContainerStyle);
            }

            // Note: WPF also does preparation for ContentPresenter,
            // ContentControl, HeaderedContentControl, and ItemsControl.  Since
            // we don't have any other ItemsControls using this
            // ItemContainerGenerator, we've removed that code for now.  It
            // should be added back later when necessary.
        }

        /// <summary>
        /// Gets the ItemsControl being tracked by the
        /// ItemContainerGenerator.
        /// </summary>
        private ItemsControl ItemsControl { get; }

        /// <summary>
        /// A Panel that is used as the ItemsHost of the ItemsControl.  This
        /// property will only be valid when the ItemsControl is live in the
        /// tree and has generated containers for some of its items.
        /// </summary>
        private Panel _itemsHost;

        /// <summary>
        /// Gets a Panel that is used as the ItemsHost of the ItemsControl.
        /// This property will only be valid when the ItemsControl is live in
        /// the tree and has generated containers for some of its items.
        /// </summary>
        internal Panel ItemsHost
        {
            get
            {
                // Lookup the ItemsHost if we haven't already cached it.
                if (_itemsHost == null && ItemsControl?.ItemContainerGenerator != null)
                {
                    // Get any live container
                    DependencyObject container = ItemsControl.ContainerFromIndex(0);

                    if (container != null)
                    {
                        // Get the parent of the container
                        _itemsHost = VisualTreeHelper.GetParent(container) as Panel;
                    }
                }

                return _itemsHost;
            }
        }

        /// <summary>
        /// A ScrollViewer that is used to scroll the items in the ItemsHost.
        /// </summary>
        private ScrollViewer _scrollHost;

        /// <summary>
        /// Gets a ScrollViewer that is used to scroll the items in the
        /// ItemsHost.
        /// </summary>
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "Code is linked into multiple projects.")]
        internal ScrollViewer ScrollHost
        {
            get
            {
                if (_scrollHost == null)
                {
                    Panel itemsHost = ItemsHost;
                    if (itemsHost != null)
                    {
                        for (DependencyObject obj = itemsHost; obj != ItemsControl && obj != null; obj = VisualTreeHelper.GetParent(obj))
                        {
                            ScrollViewer viewer = obj as ScrollViewer;
                            if (viewer != null)
                            {
                                _scrollHost = viewer;
                                break;
                            }
                        }
                    }
                }

                return _scrollHost;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ItemsControlHelper"/> class.
        /// </summary>
        /// <param name="control">
        /// The ItemsControl being tracked by the ItemContainerGenerator.
        /// </param>
        internal ItemsControlHelper(ItemsControl control)
        {
            Debug.Assert(control != null, "control cannot be null!");
            ItemsControl = control;
        }

        /// <summary>
        /// Apply a control template to the ItemsControl.
        /// </summary>
        internal void OnApplyTemplate()
        {
            // Clear the cached ItemsHost, ScrollHost
            _itemsHost = null;
            _scrollHost = null;
        }

        /// <summary>
        /// Update the style of any generated items when the ItemContainerStyle
        /// has been changed.
        /// </summary>
        /// <param name="itemContainerStyle">The ItemContainerStyle.</param>
        /// <remarks>
        /// Silverlight does not support setting a Style multiple times, so we
        /// only attempt to set styles on elements whose style hasn't already
        /// been set.
        /// </remarks>
        internal void UpdateItemContainerStyle(Style itemContainerStyle)
        {
            if (itemContainerStyle == null)
            {
                return;
            }

            Panel itemsHost = ItemsHost;
            if (itemsHost?.Children == null)
            {
                return;
            }

            foreach (UIElement element in itemsHost.Children)
            {
                FrameworkElement obj = element as FrameworkElement;
                Debug.Assert(obj != null, "obj != null");
                if (obj.Style == null)
                {
                    obj.Style = itemContainerStyle;
                }
            }
        }

        /// <summary>
        /// Scroll the desired element into the ScrollHost's viewport.
        /// </summary>
        /// <param name="element">Element to scroll into view.</param>
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "File is linked across multiple projects and this method is used in some but not others.")]
        internal void ScrollIntoView(FrameworkElement element)
        {
            // Get the ScrollHost
            if (ScrollHost == null)
            {
                return;
            }

            // Get the position of the element relative to the ScrollHost
            GeneralTransform transform;

            try
            {
                transform = element.TransformToVisual(ScrollHost);
            }
            catch (ArgumentException)
            {
                // Ignore failures when not in the visual tree
                return;
            }

            Rect itemRect = new Rect(
                transform.TransformPoint(default(Point)),
                transform.TransformPoint(new Point(element.ActualWidth, element.ActualHeight)));

            // Scroll vertically
            double verticalOffset = ScrollHost.VerticalOffset;
            double verticalDelta = 0;
            double hostBottom = ScrollHost.ViewportHeight;
            double itemBottom = itemRect.Bottom;

            if (hostBottom < itemBottom)
            {
                verticalDelta = itemBottom - hostBottom;
                verticalOffset += verticalDelta;
            }

            double itemTop = itemRect.Top;

            if (itemTop - verticalDelta < 0)
            {
                verticalOffset -= verticalDelta - itemTop;
            }

            // Scroll horizontally
            double horizontalOffset = ScrollHost.HorizontalOffset;
            double horizontalDelta = 0;
            double hostRight = ScrollHost.ViewportWidth;
            double itemRight = itemRect.Right;

            if (hostRight < itemRight)
            {
                horizontalDelta = itemRight - hostRight;
                horizontalOffset += horizontalDelta;
            }

            double itemLeft = itemRect.Left;

            if (itemLeft - horizontalDelta < 0)
            {
                horizontalOffset -= horizontalDelta - itemLeft;
            }

            ScrollHost.ChangeView(horizontalOffset, verticalOffset, null);
        }
    }
}