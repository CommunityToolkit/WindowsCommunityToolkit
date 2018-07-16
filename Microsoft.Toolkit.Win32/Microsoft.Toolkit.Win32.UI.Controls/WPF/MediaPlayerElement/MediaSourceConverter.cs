// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Globalization;
using System.Windows.Data;
using Windows.Media.Core;

namespace Microsoft.Toolkit.Win32.UI.Controls.WPF
{
    /// <summary>
    /// Dual interface IValueConverter, converts a uri string to UWP MediaSource and back on behalf of both WPF and UWP bindings.
    /// </summary>
    internal class MediaSourceConverter : IValueConverter, global::Windows.UI.Xaml.Data.IValueConverter
    {
        object IValueConverter.Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return null;
            }

            return ((MediaSource)value).Uri.ToString();
        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        object global::Windows.UI.Xaml.Data.IValueConverter.Convert(object value, Type targetType, object parameter, string language)
        {
            if (value == null)
            {
                return null;
            }

            return MediaSource.CreateFromUri(new Uri(value as string));
        }

        object global::Windows.UI.Xaml.Data.IValueConverter.ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}