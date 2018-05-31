// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Security;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using Microsoft.Toolkit.Win32.UI.Controls.Interop.WinRT;

namespace Microsoft.Toolkit.Win32.UI.Controls.WPF
{
    /// <summary>
    /// Extends the funcionality of <see cref="WebViewControlProcess"/> for WPF.
    /// </summary>
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
        /// Creates a <see cref="IWebView"/> within the context of <paramref name="process"/> using the <paramref name="visual"/> to create a HWND and within the specified <paramref name="bounds"/>.
        /// </summary>
        /// <param name="process">An instance of <see cref="WebViewControlProcess" />.</param>
        /// <param name="visual">A <see cref="Visual"/> instance in which to create a HWND.</param>
        /// <param name="bounds">A <see cref="Rect" /> containing numerical values that represent the location and size of the control.</param>
        /// <returns>An <see cref="IWebView"/> instance.</returns>
        internal static IWebView CreateWebView(this WebViewControlProcess process, Visual visual, Rect bounds)
        {
            return visual.Dispatcher.Invoke(() => process.CreateWebViewAsync(visual, bounds).GetAwaiter().GetResult());
        }

        /// <summary>
        /// Creates a <see cref="IWebView"/> within the context of <paramref name="process"/> using the <paramref name="visual"/> to create a HWND.
        /// </summary>
        /// <param name="process">An instance of <see cref="WebViewControlProcess" />.</param>
        /// <param name="visual">A <see cref="Visual"/> instance in which to create a HWND.</param>
        /// <returns>An <see cref="IWebView"/> instance.</returns>
        /// <remarks>
        /// The bounds to draw the <see cref="WebView"/> are determined by the height and width of the <paramref name="visual"/>.
        /// </remarks>
        /// <seealso cref="CreateWebViewAsync(WebViewControlProcess,IntPtr,Rect)"/>
        internal static IWebView CreateWebView(this WebViewControlProcess process, Visual visual)
        {
            return visual.Dispatcher.Invoke(() => process.CreateWebViewAsync(visual).GetAwaiter().GetResult());
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

        /// <summary>
        /// Creates a <see cref="IWebView"/> within the context of <paramref name="process"/> using the <paramref name="visual"/> to create a HWND and within the specified <paramref name="bounds"/>.
        /// </summary>
        /// <param name="process">An instance of <see cref="WebViewControlProcess" />.</param>
        /// <param name="visual">A <see cref="Visual"/> instance in which to create a HWND.</param>
        /// <param name="bounds">A <see cref="Rect" /> containing numerical values that represent the location and size of the control.</param>
        /// <returns>An asynchronous operation that completes with a <see cref="IWebView"/>.</returns>
        internal static async Task<IWebView> CreateWebViewAsync(this WebViewControlProcess process, Visual visual, Rect bounds)
        {
            HwndSource sourceHwnd;
            if (!visual.Dispatcher.CheckAccess())
            {
                sourceHwnd = visual.Dispatcher.Invoke(() => (HwndSource)PresentationSource.FromVisual(visual));
            }
            else
            {
                sourceHwnd = (HwndSource)PresentationSource.FromVisual(visual);
            }

            Verify.IsNotNull(sourceHwnd);

            var webViewControlHost = await process.CreateWebViewControlHostAsync(sourceHwnd?.Handle ?? IntPtr.Zero, bounds);

            return !visual.Dispatcher.CheckAccess()
                ? visual.Dispatcher.Invoke(() => new WebView(webViewControlHost))
                : new WebView(webViewControlHost);
        }

        /// <summary>
        /// Creates a <see cref="IWebView"/> within the context of <paramref name="process"/> using the <paramref name="visual"/> to create a HWND.
        /// </summary>
        /// <param name="process">An instance of <see cref="WebViewControlProcess" />.</param>
        /// <param name="visual">A <see cref="Visual"/> instance in which to create a HWND.</param>
        /// <returns>An asynchronous operation that completes with a <see cref="IWebView"/>.</returns>
        /// <remarks>
        /// The bounds to draw the <see cref="WebView"/> are determined by the height and width of the <paramref name="visual"/>.
        /// </remarks>
        /// <seealso cref="CreateWebViewAsync(WebViewControlProcess,IntPtr,Rect)"/>
        internal static Task<IWebView> CreateWebViewAsync(this WebViewControlProcess process, Visual visual)
        {
            double width;
            double height;

            if (!visual.Dispatcher.CheckAccess())
            {
                width = visual.Dispatcher.Invoke(() => (double)visual.GetValue(FrameworkElement.ActualWidthProperty));
                height = visual.Dispatcher.Invoke(() => (double)visual.GetValue(FrameworkElement.ActualHeightProperty));
            }
            else
            {
                width = (double)visual.GetValue(FrameworkElement.ActualWidthProperty);
                height = (double)visual.GetValue(FrameworkElement.ActualHeightProperty);
            }

            return process.CreateWebViewAsync(visual, new Rect(new Size(height, width)));
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