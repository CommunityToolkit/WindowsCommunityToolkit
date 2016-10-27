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
using Windows.UI.Xaml.Controls;

namespace Microsoft.Toolkit.Uwp.UI
{
    /// <summary>
    /// Provides attached dependency properties for the <see cref="WebView"/> that allows attaching HTML string content/>.
    /// </summary>
    public static class WebViewExtensions
    {
        /// <summary>
        /// Using a DependencyProperty as the backing store for HTML.  This enables animation, styling, binding, etc.
        /// </summary>
        public static readonly DependencyProperty HTMLProperty = DependencyProperty.RegisterAttached(
            "HTML",
            typeof(string),
            typeof(WebViewExtensions),
            new PropertyMetadata(string.Empty, OnHTMLChanged));

        /// <summary>
        /// Gets HTML associated with the <see cref="WebView"/>
        /// </summary>
        /// <param name="obj">The <see cref="DependencyObject"/> from which to get the associated HTML from.</param>
        /// <returns>HTML content</returns>
        public static string GetHTML(DependencyObject obj)
        {
            return (string)obj.GetValue(HTMLProperty);
        }

        /// <summary>
        /// Sets HTML from the <see cref="WebView"/>
        /// </summary>
        /// <param name="obj">The <see cref="DependencyObject"/> to set the HTML content to.</param>
        /// <param name="value">HTML content</param>
        public static void SetHTML(DependencyObject obj, string value)
        {
            obj.SetValue(HTMLProperty, value);
        }

        private static void OnHTMLChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            WebView wv = d as WebView;

            wv?.NavigateToString((string)e.NewValue);
        }
    }
}
