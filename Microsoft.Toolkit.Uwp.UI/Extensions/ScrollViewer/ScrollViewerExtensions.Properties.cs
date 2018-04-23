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

using Windows.UI.Xaml;

namespace Microsoft.Toolkit.Uwp.UI.Extensions
{
    /// <summary>
    /// Provides attached dependency properties for the <see cref="Windows.UI.Xaml.Controls.ScrollViewer"/>
    /// </summary>
    public partial class ScrollViewerExtensions
    {
#pragma warning disable CS0419 // Ambiguous reference in cref attribute
        /// <summary>
        /// Attached <see cref="DependencyProperty"/> for binding a <see cref="Windows.UI.Xaml.Thickness"/> for the horizontal <see cref="Windows.UI.Xaml.Controls.Primitives.ScrollBar"/> of a <see cref="Windows.UI.Xaml.Controls.ScrollViewer"/>
        /// </summary>
        public static readonly DependencyProperty HorizontalScrollBarMarginProperty = DependencyProperty.RegisterAttached("HorizontalScrollBarMargin", typeof(Thickness), typeof(ScrollViewerExtensions), new PropertyMetadata(null, OnHorizontalScrollBarMarginPropertyChanged));

        /// <summary>
        /// Attached <see cref="DependencyProperty"/> for binding a <see cref="Windows.UI.Xaml.Thickness"/> for the vertical <see cref="Windows.UI.Xaml.Controls.Primitives.ScrollBar"/> of a <see cref="Windows.UI.Xaml.Controls.ScrollViewer"/>
        /// </summary>
        public static readonly DependencyProperty VerticalScrollBarMarginProperty = DependencyProperty.RegisterAttached("VerticalScrollBarMargin", typeof(Thickness), typeof(ScrollViewerExtensions), new PropertyMetadata(null, OnVerticalScrollBarMarginPropertyChanged));

        /// <summary>
        /// Gets the <see cref="Windows.UI.Xaml.Thickness"/> associated with the specified vertical <see cref="Windows.UI.Xaml.Controls.Primitives.ScrollBar"/> of a <see cref="Windows.UI.Xaml.Controls.ScrollViewer"/>
        /// </summary>
        /// <param name="obj">The <see cref="FrameworkElement"/> to get the associated <see cref="Windows.UI.Xaml.Thickness"/> from</param>
        /// <returns>The <see cref="Windows.UI.Xaml.Thickness"/> associated with the <see cref="FrameworkElement"/></returns>
        public static Thickness GetVerticalScrollBarMargin(FrameworkElement obj)
        {
            return (Thickness)obj.GetValue(VerticalScrollBarMarginProperty);
        }

        /// <summary>
        /// Sets the <see cref="Windows.UI.Xaml.Thickness"/> associated with the specified vertical <see cref="Windows.UI.Xaml.Controls.Primitives.ScrollBar"/> of a <see cref="Windows.UI.Xaml.Controls.ScrollViewer"/>
        /// </summary>
        /// <param name="obj">The <see cref="FrameworkElement"/> to associate the <see cref="Windows.UI.Xaml.Thickness"/> with</param>
        /// <param name="value">The <see cref="Windows.UI.Xaml.Thickness"/> for binding to the <see cref="FrameworkElement"/></param>
        public static void SetVerticalScrollBarMargin(FrameworkElement obj, Thickness value)
        {
            obj.SetValue(VerticalScrollBarMarginProperty, value);
        }

        /// <summary>
        /// Gets the <see cref="Windows.UI.Xaml.Thickness"/> associated with the specified horizontal <see cref="Windows.UI.Xaml.Controls.Primitives.ScrollBar"/> of a <see cref="Windows.UI.Xaml.Controls.ScrollViewer"/>
        /// </summary>
        /// <param name="obj">The <see cref="FrameworkElement"/> to get the associated <see cref="Windows.UI.Xaml.Thickness"/> from</param>
        /// <returns>The <see cref="Windows.UI.Xaml.Thickness"/> associated with the <see cref="FrameworkElement"/></returns>
        public static Thickness GetHorizontalScrollBarMargin(FrameworkElement obj)
        {
            return (Thickness)obj.GetValue(HorizontalScrollBarMarginProperty);
        }

        /// <summary>
        /// Sets the <see cref="Windows.UI.Xaml.Thickness"/> associated with the specified horizontal <see cref="Windows.UI.Xaml.Controls.Primitives.ScrollBar"/> of a <see cref="Windows.UI.Xaml.Controls.ScrollViewer"/>
        /// </summary>
        /// <param name="obj">The <see cref="FrameworkElement"/> to associate the <see cref="Windows.UI.Xaml.Thickness"/> with</param>
        /// <param name="value">The <see cref="Windows.UI.Xaml.Thickness"/> for binding to the <see cref="FrameworkElement"/></param>
        public static void SetHorizontalScrollBarMargin(FrameworkElement obj, Thickness value)
        {
            obj.SetValue(HorizontalScrollBarMarginProperty, value);
        }
#pragma warning restore CS0419 // Ambiguous reference in cref attribute
    }
}