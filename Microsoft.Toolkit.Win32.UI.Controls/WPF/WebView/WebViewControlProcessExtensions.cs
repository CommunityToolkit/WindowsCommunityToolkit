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
using System.Security;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Toolkit.Win32.UI.Controls.Interop.WinRT;

namespace Microsoft.Toolkit.Win32.UI.Controls.WPF
{
    internal static class WebViewControlProcessExtensions
    {
        /// <summary>
        /// Creates a <see cref="IWebView"/> within the context of <paramref name="process"/>.
        /// </summary>
        /// <param name="process">An instance of <see cref="WebViewControlProcess" />.</param>
        /// <param name="hostWindowHandle">The host window handle.</param>
        /// <param name="bounds">A <see cref="Rect" /> containing numerical values that represent the location and size of the control.</param>
        /// <returns>An <see cref="IWebView" /> instance.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="hostWindowHandle"/> is equal to <see cref="IntPtr.Zero"/>, or
        /// <paramref name="process"/> is <see langword="null" />.
        /// </exception>
        internal static IWebView CreateWebView(
            this WebViewControlProcess process,
            IntPtr hostWindowHandle,
            Rect bounds)
        {
            if (process is null)
            {
                throw new ArgumentNullException(nameof(process));
            }

            if (hostWindowHandle == IntPtr.Zero)
            {
                throw new ArgumentNullException(nameof(hostWindowHandle));
            }

            return new WebView(process.CreateWebViewControlHost(hostWindowHandle, bounds));
        }

        /// <summary>
        /// Creates a <see cref="IWebView"/> within the context of <paramref name="process"/>.
        /// </summary>
        /// <param name="process">An instance of <see cref="WebViewControlProcess" />.</param>
        /// <param name="hostWindowHandle">The host window handle.</param>
        /// <param name="bounds">A <see cref="Rect" /> containing numerical values that represent the location and size of the control.</param>
        /// <returns>An <see cref="IWebView" /> instance.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="hostWindowHandle"/> is equal to <see cref="IntPtr.Zero"/>, or
        /// <paramref name="process"/> is <see langword="null" />.
        /// </exception>
        internal static async Task<IWebView> CreateWebViewAsync(
            this WebViewControlProcess process,
            IntPtr hostWindowHandle,
            Rect bounds)
        {
            if (process is null)
            {
                throw new ArgumentNullException(nameof(process));
            }

            if (hostWindowHandle == IntPtr.Zero)
            {
                throw new ArgumentNullException(nameof(hostWindowHandle));
            }

            return new WebView(await process.CreateWebViewControlHostAsync(hostWindowHandle, bounds).ConfigureAwait(false));
        }

        [SecuritySafeCritical]
        internal static WebViewControlHost CreateWebViewControlHost(
            this WebViewControlProcess process,
            IntPtr hostWindowHandle,
            Rect bounds)
        {
            Verify.IsNotNull(process);
            Verify.IsFalse(hostWindowHandle == IntPtr.Zero);

            var rect = new Windows.Foundation.Rect(bounds.X, bounds.Y, bounds.Width, bounds.Height);
            return process
                    .CreateWebViewControlHostAsync(hostWindowHandle, rect)
                    .GetAwaiter()
                    .GetResult();
        }

        [SecuritySafeCritical]
        internal static Task<WebViewControlHost> CreateWebViewControlHostAsync(
            this WebViewControlProcess process,
            IntPtr hostWindowHandle,
            Rect bounds)
        {
            Verify.IsNotNull(process);
            Verify.IsFalse(hostWindowHandle == IntPtr.Zero);

            var rect = new Windows.Foundation.Rect(bounds.X, bounds.Y, bounds.Width, bounds.Height);
            return process.CreateWebViewControlHostAsync(hostWindowHandle, rect);
        }
    }
}