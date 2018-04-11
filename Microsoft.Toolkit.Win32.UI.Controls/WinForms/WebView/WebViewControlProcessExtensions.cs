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
using Microsoft.Toolkit.Win32.UI.Controls.Interop.WinRT;
using Windows.Foundation;
using Windows.Web.UI.Interop;
using WebViewControlProcess = Microsoft.Toolkit.Win32.UI.Controls.Interop.WinRT.WebViewControlProcess;

namespace Microsoft.Toolkit.Win32.UI.Controls.WinForms
{
    public static class WebViewControlProcessExtensions
    {
        internal static WebViewControlHost CreateWebViewControlHost(
            this WebViewControlProcess process,
            IntPtr hostWindowHandle,
            Rectangle bounds)
        {
            Verify.IsNotNull(process);
            var f = process.CreateWebViewControlHostAsync(hostWindowHandle, bounds).ConfigureAwait(false);
            return f.GetAwaiter().GetResult();
        }

        internal static async Task<WebViewControlHost> CreateWebViewControlHostAsync(
            this WebViewControlProcess process,
            IntPtr hostWindowHandle,
            Rectangle bounds)
        {
            Verify.IsNotNull(process);
            Verify.IsFalse(hostWindowHandle == IntPtr.Zero);
            if (hostWindowHandle == IntPtr.Zero)
            {
                throw new ArgumentNullException(nameof(hostWindowHandle));
            }

            var wvc = await await Task.Run(() => process.CreateWebViewControlAsync(hostWindowHandle, bounds)).ConfigureAwait(false);

            return new WebViewControlHost(wvc);
        }

        internal static IAsyncOperation<WebViewControl> CreateWebViewControlAsync(
            this WebViewControlProcess process,
            IntPtr hostWindowHandle,
            Rectangle bounds)
        {
            Verify.IsNotNull(process);
            var rect = new Rect(bounds.X, bounds.Y, bounds.Width, bounds.Height);
            return process.CreateWebViewControlAsync(hostWindowHandle, rect);
        }
    }
}