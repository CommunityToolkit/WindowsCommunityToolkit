// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Globalization;
using System.Windows.Data;
using Microsoft.Toolkit.Wpf.UI.XamlHost;
using uwpXaml = Windows.UI.Xaml;

namespace Microsoft.Toolkit.Wpf.UI.Controls
{
    /// <summary>
    /// Dual interface IValueConverter, assumes that the conversion is between a WindowsXamlHostBaseExt and its wrapped UIElement (XamlRoot) and attempts to return the correct instance of each.
    /// </summary>
    internal class WindowsXamlHostWrapperConverter : IValueConverter, Windows.UI.Xaml.Data.IValueConverter
    {
        object IValueConverter.Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (value as uwpXaml.UIElement)?.GetWrapper();
        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        object Windows.UI.Xaml.Data.IValueConverter.Convert(object value, Type targetType, object parameter, string language)
        {
            return (value as WindowsXamlHostBase)?.GetUwpInternalObject();
        }

        object Windows.UI.Xaml.Data.IValueConverter.ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}