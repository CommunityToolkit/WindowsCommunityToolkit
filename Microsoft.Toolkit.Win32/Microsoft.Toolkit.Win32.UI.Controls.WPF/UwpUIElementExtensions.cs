// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Microsoft.Toolkit.Win32.UI.Controls.WPF
{
    /// <summary>
    /// Extensions for use with UWP UIElement objects wrapped by the WindowsXamlHostBaseExt
    /// </summary>
    public static class UwpUIElementExtensions
    {
        public static Windows.UI.Xaml.DependencyProperty WrapperProperty { get; } =
            Windows.UI.Xaml.DependencyProperty.RegisterAttached("Wrapper", typeof(System.Windows.UIElement), typeof(WindowsXamlHostBaseExt), new Windows.UI.Xaml.PropertyMetadata(null));

        public static WindowsXamlHostBaseExt GetWrapper(this Windows.UI.Xaml.UIElement element)
        {
            return (WindowsXamlHostBaseExt)element.GetValue(WrapperProperty);
        }

        public static void SetWrapper(this Windows.UI.Xaml.UIElement element, WindowsXamlHostBaseExt wrapper)
        {
            element.SetValue(WrapperProperty, wrapper);
        }
    }
}
