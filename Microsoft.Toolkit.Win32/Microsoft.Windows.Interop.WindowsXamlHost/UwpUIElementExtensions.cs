

namespace Microsoft.Toolkit.Win32.UI.Interop
{
    public static class UwpUiElementExtensions
    {
        public static WindowsXamlHost GetWrapper(this Windows.UI.Xaml.UIElement element)
        {
            return (WindowsXamlHost)element.GetValue(WindowsXamlHost.WrapperProperty);
        }

        public static void SetWrapper(this Windows.UI.Xaml.UIElement element, WindowsXamlHost wrapper)
        {
            element.SetValue(WindowsXamlHost.WrapperProperty, wrapper);
        }
    }
}
