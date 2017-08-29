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
using Windows.UI.Xaml;

namespace Microsoft.Toolkit.Uwp.UI.Extensions
{
    /// <summary>
    /// Provides attached dependency properties for the <see cref="Windows.UI.Xaml.Controls.WebView"/> that allows attaching HTML string content/>.
    /// </summary>
    public partial class WebView
    {
        private static void OnContentChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Windows.UI.Xaml.Controls.WebView wv = d as Windows.UI.Xaml.Controls.WebView;

            var content = e.NewValue as string;

            if (string.IsNullOrEmpty(content))
            {
                return;
            }

            wv?.NavigateToString(content);
        }

        private static void OnContentUriChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Windows.UI.Xaml.Controls.WebView wv = d as Windows.UI.Xaml.Controls.WebView;

            var uri = e.NewValue as Uri;

            if (uri == null)
            {
                return;
            }

            wv?.Navigate(uri);
        }
    }
}
