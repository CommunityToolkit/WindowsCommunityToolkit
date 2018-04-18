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
using System.Windows;
using Microsoft.Toolkit.Win32.UI.Controls.Interop.WinRT;

namespace Microsoft.Toolkit.Win32.UI.Controls.WPF
{
    internal static class WebViewControlProcessExtensions
    {
        /// <summary>
        /// Creates a <see cref="WebViewControlHost"/> within the context of <paramref name="process"/>.
        /// </summary>
        /// <param name="process">An instance of <see cref="WebViewControlProcess"/>.</param>
        /// <param name="hostWindowHandle">The parent window handle hosting the control.</param>
        /// <param name="desiredSize">A <see cref="Size"/> containing numerical values that represent the size of the control.</param>
        /// <returns>A <see cref="WebViewControlHost"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="hostWindowHandle"/> is equal to <see cref="IntPtr.Zero"/></exception>
        /// <remarks>Since <paramref name="desiredSize"/> contains only size, the location is presumed 0, 0.</remarks>
        [SecuritySafeCritical]
        internal static WebViewControlHost CreateWebViewControlHost(
            this WebViewControlProcess process,
            IntPtr hostWindowHandle,
            Size desiredSize)
        {
            Verify.IsNotNull(process);
            if (hostWindowHandle == IntPtr.Zero)
            {
                throw new ArgumentNullException(nameof(hostWindowHandle));
            }

            var rect = new Windows.Foundation.Rect(0, 0, (int)desiredSize.Width, (int)desiredSize.Height);

            return process
                    .CreateWebViewControlHostAsync(hostWindowHandle, rect)
                    .GetAwaiter()
                    .GetResult();
        }
    }
}
