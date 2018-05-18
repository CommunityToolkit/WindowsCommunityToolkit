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
using System.ComponentModel;
using System.Globalization;

namespace Microsoft.Toolkit.Win32.UI.Controls.WinForms
{
    /// <summary>
    /// Converts a <see cref="string"/> type to a <see cref="Uri"/> type for <see cref="WebView"/>, and vice versa.
    /// </summary>
    public class WebBrowserUriTypeConverter : UriTypeConverter
    {
        /// <summary>
        /// Converts the given <paramref name="value" /> to the type of this converter, using the specified <paramref name="context"/> and <paramref name="culture"/> information.
        /// </summary>
        /// <param name="context">An <seealso cref="ITypeDescriptorContext"/> that provides a format context.</param>
        /// <param name="culture">The <seealso cref="CultureInfo"/> to use as the current culture.</param>
        /// <param name="value">The <seealso cref="object"/> to convert.</param>
        /// <returns>An <seealso cref="object"/> that represents the converted value.</returns>
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            var uri = base.ConvertFrom(context, culture, value) as Uri;
            if (uri != null && !string.IsNullOrEmpty(uri.OriginalString) && !uri.IsAbsoluteUri)
            {
                try
                {
                    uri = new Uri("http://" + uri.OriginalString.Trim());
                }
                catch (UriFormatException)
                {
                }
            }

            return uri;
        }
    }
}