// ******************************************************************
// Copyright (c) Microsoft. All rights reserved.
// This code is licensed under the MIT License (MIT).
// THE CODE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
// TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH
// THE CODE OR THE USE OR OTHER DEALINGS IN THE CODE.
// ******************************************************************

using System;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;

namespace Microsoft.Toolkit.Uwp.UI.Extensions
{
    /// <summary>
    /// Provides attached dependency properties for the <see cref="Windows.UI.Xaml.Controls.ListViewBase"/>
    /// </summary>
    public static partial class ScrollViewerEx
    {
        private static void OnHorizontalScrollBarMarginPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            if (!(sender is FrameworkElement baseElement))
            {
                return;
            }

            // We need to wait for the component to be loaded before getting its ScrollViewer
            baseElement.Loaded -= ChangeHorizontalScrollBarMarginProperty;

            if (HorizontalScrollBarMarginProperty != null)
            {
                baseElement.Loaded += ChangeHorizontalScrollBarMarginProperty;
            }
        }

        private static void OnVerticalScrollBarMarginPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            if (!(sender is FrameworkElement baseElement))
            {
                return;
            }

            // We need to wait for the component to be loaded before getting its ScrollViewer
            baseElement.Loaded -= ChangeVerticalScrollBarMarginProperty;

            if (VerticalScrollBarMarginProperty != null)
            {
                baseElement.Loaded += ChangeVerticalScrollBarMarginProperty;
            }
        }

        private static void ChangeHorizontalScrollBarMarginProperty(object sender, RoutedEventArgs routedEventArgs)
        {
            var baseElement = sender as FrameworkElement;
            var scrollViewer = sender as ScrollViewer ?? baseElement.FindDescendant<ScrollViewer>();

            // Last scrollbar with "HorizontalScrollBar" as name is our target to set its margin and avoid it overlapping the header
            var scrollBar = scrollViewer?.FindDescendants<ScrollBar>().LastOrDefault(bar => bar.Name == "HorizontalScrollBar");

            if (scrollBar == null)
            {
                return;
            }

            var newMargin = GetVerticalScrollBarMargin(baseElement);

            scrollBar.Margin = newMargin;
        }

        private static void ChangeVerticalScrollBarMarginProperty(object sender, RoutedEventArgs routedEventArgs)
        {
            var baseElement = sender as FrameworkElement;
            var scrollViewer = sender as ScrollViewer ?? baseElement.FindDescendant<ScrollViewer>();

            // Last scrollbar with "VerticalScrollBar" as name is our target to set its margin and avoid it overlapping the header
            var scrollBar = scrollViewer?.FindDescendants<ScrollBar>().LastOrDefault(bar => bar.Name == "VerticalScrollBar");

            if (scrollBar == null)
            {
                return;
            }

            var newMargin = GetVerticalScrollBarMargin(baseElement);

            scrollBar.Margin = newMargin;
        }
    }
}
