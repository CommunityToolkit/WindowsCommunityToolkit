// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Globalization;
using System.Windows.Data;
using Microsoft.Toolkit.Win32.UI.Interop;
using uwpXaml = Windows.UI.Xaml;

namespace Microsoft.Toolkit.Win32.UI.Controls.WPF
{
    /// <summary>
    /// Dual interface IValueConverter, assumes that the conversion is between a WindowsXamlHostBaseExt and its wrapped UIElement (XamlRoot) and attempts to return the correct instance of each.
    /// </summary>
    internal class WindowsXamlHostWrapperConverter : IValueConverter, global::Windows.UI.Xaml.Data.IValueConverter
    {
        object IValueConverter.Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (value as uwpXaml.UIElement)?.GetWrapper();
        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        object global::Windows.UI.Xaml.Data.IValueConverter.Convert(object value, Type targetType, object parameter, string language)
        {
            return (value as WindowsXamlHostBaseExt)?.XamlRootInternal;
        }

        object global::Windows.UI.Xaml.Data.IValueConverter.ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}