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
using System.Drawing;
using Microsoft.Toolkit.Win32.UI.Controls.Interop.WinRT;

namespace Microsoft.Toolkit.Win32.UI.Controls.WinForms
{
    internal static class WebViewControlHostExtensions
    {
        /// <summary>
        /// Updates the location and size of <see cref="WebView"/>.
        /// </summary>
        /// <param name="host">A <see cref="WebViewControlHost"/> instance</param>
        /// <param name="bounds">A <see cref="Rectangle"/> containing numerical values that represent the location and size of the control.</param>
        /// <seealso cref="WebViewControlHost.UpdateBounds"/>
        /// <exception cref="ArgumentNullException"><paramref name="host"/> is <see langword="null"/>.</exception>
        /// <remarks><paramref name="bounds" /> is translated into a <seealso cref="Windows.Foundation.Rect(double, double, double, double)"/>.</remarks>
        internal static void UpdateBounds(this WebViewControlHost host, Rectangle bounds)
        {
            Windows.Foundation.Rect CreateBounds()
            {
                return new Windows.Foundation.Rect(
                    bounds.X,
                    bounds.Y,
                    bounds.Width,
                    bounds.Height);
            }

            if (host is null)
            {
                throw new ArgumentNullException(nameof(host));
            }

            host.UpdateBounds(CreateBounds());
        }
    }
}