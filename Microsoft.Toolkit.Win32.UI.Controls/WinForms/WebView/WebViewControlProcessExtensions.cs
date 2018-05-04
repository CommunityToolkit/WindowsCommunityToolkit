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
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Toolkit.Win32.UI.Controls.Interop.WinRT;
using Windows.Foundation;
using WebViewControlProcess = Microsoft.Toolkit.Win32.UI.Controls.Interop.WinRT.WebViewControlProcess;

namespace Microsoft.Toolkit.Win32.UI.Controls.WinForms
{
    /// <summary>
    /// Extends the funcionality of <see cref="WebViewControlProcess"/> for Windows Forms.
    /// </summary>
    public static class WebViewControlProcessExtensions
    {
        /// <summary>
        /// Creates a <see cref="IWebView" /> within the context of <paramref name="process" />.
        /// </summary>
        /// <param name="process">An instance of <see cref="WebViewControlProcess" />.</param>
        /// <param name="hostWindowHandle">The parent window handle hosting the control.</param>
        /// <param name="bounds">A <see cref="Rectangle" /> containing numerical values that represent the location and size of the control.</param>
        /// <returns>An <see cref="IWebView" /> instance.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="hostWindowHandle"/> is equal to <see cref="IntPtr.Zero"/>, or
        /// <paramref name="process"/> is <see langword="null" />.
        /// </exception>
        public static IWebView CreateWebView(
            this WebViewControlProcess process,
            IntPtr hostWindowHandle,
            Rectangle bounds)
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
        /// Creates a <see cref="IWebView" /> within the context of <paramref name="process" />.
        /// </summary>
        /// <param name="process">An instance of <see cref="WebViewControlProcess" />.</param>
        /// <param name="control">An instance of <see cref="Control"/> to parent the <see cref="WebView"/>.</param>
        /// <returns>An <see cref="IWebView"/> instance.</returns>
        /// <exception cref="ArgumentNullException">Occurs when <paramref name="control"/> is <see langword="null" />.</exception>
        public static IWebView CreateWebView(
            this WebViewControlProcess process,
            Control control)
        {
            if (control == null)
            {
                throw new ArgumentNullException(nameof(control));
            }

            return process.CreateWebView(control, control.Bounds);
        }

        /// <summary>
        /// Creates a <see cref="IWebView" /> within the context of <paramref name="process" />.
        /// </summary>
        /// <param name="process">An instance of <see cref="WebViewControlProcess" />.</param>
        /// <param name="control">An instance of <see cref="Control"/> to parent the <see cref="WebView"/>.</param>
        /// <param name="bounds">A <see cref="Rectangle" /> containing numerical values that represent the location and size of the control.</param>
        /// <returns>An <see cref="IWebView"/> instance.</returns>
        /// <exception cref="ArgumentNullException">Occurs when <paramref name="control"/> is <see langword="null" />.</exception>
        public static IWebView CreateWebView(
            this WebViewControlProcess process,
            Control control,
            Rectangle bounds)
        {
            if (control == null)
            {
                throw new ArgumentNullException(nameof(control));
            }

            return process.CreateWebView(control.Handle, bounds);
        }

        /// <summary>
        /// Creates a <see cref="IWebView" /> within the context of <paramref name="process" />.
        /// </summary>
        /// <param name="process">An instance of <see cref="WebViewControlProcess" />.</param>
        /// <param name="hostWindowHandle">The parent window handle hosting the control.</param>
        /// <param name="bounds">A <see cref="Rectangle" /> containing numerical values that represent the location and size of the control.</param>
        /// <returns>An <see cref="IWebView" /> instance.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="hostWindowHandle"/> is equal to <see cref="IntPtr.Zero"/>, or
        /// <paramref name="process"/> is <see langword="null" />.
        /// </exception>
        public static async Task<IWebView> CreateWebViewAsync(
            this WebViewControlProcess process,
            IntPtr hostWindowHandle,
            Rectangle bounds)
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
        /// Creates a <see cref="IWebView" /> within the context of <paramref name="process" />.
        /// </summary>
        /// <param name="process">An instance of <see cref="WebViewControlProcess" />.</param>
        /// <param name="control">An instance of <see cref="Control"/> to parent the <see cref="WebView"/>.</param>
        /// <returns>An <see cref="IWebView"/> instance.</returns>
        /// <exception cref="ArgumentNullException">Occurs when <paramref name="control"/> is <see langword="null" />.</exception>
        public static Task<IWebView> CreateWebViewAsync(
            this WebViewControlProcess process,
            Control control)
        {
            if (control == null)
            {
                throw new ArgumentNullException(nameof(control));
            }

            return process.CreateWebViewAsync(control, control.Bounds);
        }

        /// <summary>
        /// Creates a <see cref="IWebView" /> within the context of <paramref name="process" />.
        /// </summary>
        /// <param name="process">An instance of <see cref="WebViewControlProcess" />.</param>
        /// <param name="control">An instance of <see cref="Control"/> to parent the <see cref="WebView"/>.</param>
        /// <param name="bounds">A <see cref="Rectangle" /> containing numerical values that represent the location and size of the control.</param>
        /// <returns>An <see cref="IWebView"/> instance.</returns>
        /// <exception cref="ArgumentNullException">Occurs when <paramref name="control"/> is <see langword="null" />.</exception>
        public static async Task<IWebView> CreateWebViewAsync(
            this WebViewControlProcess process,
            Control control,
            Rectangle bounds)
        {
            if (control == null)
            {
                throw new ArgumentNullException(nameof(control));
            }

            var webViewControl = await process.CreateWebViewAsync(control.Handle, bounds).ConfigureAwait(false);
            control.Controls.Add((Control)webViewControl);
            return webViewControl;
        }

        /// <summary>
        /// Creates a <see cref="WebViewControlHost"/> within the context of <paramref name="process"/>.
        /// </summary>
        /// <param name="process">An instance of <see cref="WebViewControlProcess"/>.</param>
        /// <param name="hostWindowHandle">The parent window handle hosting the control.</param>
        /// <param name="bounds">A <see cref="Rectangle"/> containing numerical values that represent the location and size of the control.</param>
        /// <returns>A <see cref="WebViewControlHost"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="hostWindowHandle"/> is equal to <see cref="IntPtr.Zero"/></exception>
        /// <seealso cref="CreateWebViewControlHostAsync"/>
        internal static WebViewControlHost CreateWebViewControlHost(
            this WebViewControlProcess process,
            IntPtr hostWindowHandle,
            Rectangle bounds)
        {
            Verify.IsNotNull(process);
            Verify.IsFalse(hostWindowHandle == IntPtr.Zero);
            var f = process.CreateWebViewControlHostAsync(hostWindowHandle, bounds).ConfigureAwait(false);
            return f.GetAwaiter().GetResult();
        }

        /// <summary>
        /// Asynchronously creates a <see cref="WebViewControlHost"/> within the context of <paramref name="process"/>.
        /// </summary>
        /// <param name="process">An instance of <see cref="WebViewControlProcess"/>.</param>
        /// <param name="hostWindowHandle">The parent window handle hosting the control.</param>
        /// <param name="bounds">A <see cref="Rectangle"/> containing numerical values that represent the location and size of the control.</param>
        /// <returns>An asynchronous operation that completes with a <see cref="WebViewControlHost"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="hostWindowHandle"/> is equal to <see cref="IntPtr.Zero"/></exception>
        internal static Task<WebViewControlHost> CreateWebViewControlHostAsync(
            this WebViewControlProcess process,
            IntPtr hostWindowHandle,
            Rectangle bounds)
        {
            Verify.IsNotNull(process);
            Verify.IsFalse(hostWindowHandle == IntPtr.Zero);

            var rect = new Rect(bounds.X, bounds.Y, bounds.Width, bounds.Height);
            return process.CreateWebViewControlHostAsync(hostWindowHandle, rect);
        }
    }
}