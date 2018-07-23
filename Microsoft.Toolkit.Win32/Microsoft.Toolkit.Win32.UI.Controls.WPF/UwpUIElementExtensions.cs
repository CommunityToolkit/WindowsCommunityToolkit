// <copyright file="WindowsXamlHostBaseExt.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// <author>Microsoft</author>

namespace Microsoft.Toolkit.Win32.UI.Controls.WPF
{
    /// <summary>
    /// Extensions for use with UWP UIElement objects wrapped by the WindowsXamlHostBaseExt
    /// </summary>
    public static class UwpUIElementExtensions
    {
        public static global::Windows.UI.Xaml.DependencyProperty WrapperProperty { get; } =
            global::Windows.UI.Xaml.DependencyProperty.RegisterAttached("Wrapper", typeof(System.Windows.UIElement), typeof(WindowsXamlHostBaseExt), new global::Windows.UI.Xaml.PropertyMetadata(null));

        public static WindowsXamlHostBaseExt GetWrapper(this global::Windows.UI.Xaml.UIElement element)
        {
            return (WindowsXamlHostBaseExt)element.GetValue(WrapperProperty);
        }

        public static void SetWrapper(this global::Windows.UI.Xaml.UIElement element, WindowsXamlHostBaseExt wrapper)
        {
            element.SetValue(WrapperProperty, wrapper);
        }
    }
}
