// <copyright file="WindowsXamlHost.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// <author>Microsoft</author>

namespace Microsoft.Windows.Interop
{

    public static class UwpUIElementExtensions
    {

        public static global::Windows.UI.Xaml.DependencyProperty WrapperProperty { get; } =
            global::Windows.UI.Xaml.DependencyProperty.RegisterAttached("Wrapper", typeof(System.Windows.UIElement), typeof(WindowsXamlHostBase), new global::Windows.UI.Xaml.PropertyMetadata(null));

        public static WindowsXamlHostBase GetWrapper(this global::Windows.UI.Xaml.UIElement element)
        {
            return (WindowsXamlHostBase)element.GetValue(WrapperProperty);
        }

        public static void SetWrapper(this global::Windows.UI.Xaml.UIElement element, WindowsXamlHostBase wrapper)
        {
            element.SetValue(WrapperProperty, wrapper);
        }
    }
}
