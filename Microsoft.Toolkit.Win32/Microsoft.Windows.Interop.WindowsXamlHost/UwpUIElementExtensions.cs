// <copyright file="WindowsXamlHost.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// <author>Microsoft</author>

namespace Microsoft.Windows.Interop
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
