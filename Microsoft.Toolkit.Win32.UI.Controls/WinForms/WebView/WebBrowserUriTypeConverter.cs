// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

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