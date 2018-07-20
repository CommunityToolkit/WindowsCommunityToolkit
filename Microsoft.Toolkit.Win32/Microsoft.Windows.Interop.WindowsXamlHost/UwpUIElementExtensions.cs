// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Microsoft.Toolkit.Win32.UI.Interop
{

    public static class UwpUIElementExtensions
    {
        public static WindowsXamlHost GetWrapper(this global::Windows.UI.Xaml.UIElement element)
        {
            return (WindowsXamlHost)element.GetValue(WindowsXamlHost.WrapperProperty);
        }

        public static void SetWrapper(this global::Windows.UI.Xaml.UIElement element, WindowsXamlHost wrapper)
        {
            element.SetValue(WindowsXamlHost.WrapperProperty, wrapper);
        }
    }
}
