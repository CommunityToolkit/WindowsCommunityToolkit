using System.Collections.Generic;
using Microsoft.Toolkit.Win32.UI.Interop.WinForms;
using Windows.UI.Xaml;

namespace Microsoft.Toolkit.Win32.UI.Controls.WinForms
{
    /// <summary>
    /// WrapperLookup is a set of extension methods to extend <see cref="FrameworkElement"/> to make it relatively easy
    /// to find its associated WindowsXamlHostBaseExt.
    /// (WPF Interop uses an attached DependencyProperty for this).
    /// </summary>
    public static class WrapperLookup
    {
        private static readonly IDictionary<FrameworkElement, WindowsXamlHostBaseExt> _controlCollection = new Dictionary<FrameworkElement, WindowsXamlHostBaseExt>();

        public static WindowsXamlHostBaseExt GetWrapper(this FrameworkElement control)
        {
            _controlCollection.TryGetValue(control, out var result);
            return result;
        }

        public static void SetWrapper(this FrameworkElement control, WindowsXamlHostBaseExt wrapper)
        {
            _controlCollection.Add(control, wrapper);
        }

        public static void ClearWrapper(this FrameworkElement control)
        {
            _controlCollection.Remove(control);
        }
    }
}
