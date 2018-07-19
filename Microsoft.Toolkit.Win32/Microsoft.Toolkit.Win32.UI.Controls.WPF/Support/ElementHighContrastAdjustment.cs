using System;

namespace Microsoft.Toolkit.Win32.UI.Controls.WPF
{
    /// <summary>
    /// <see cref="global::Windows.UI.Xaml.ElementHighContrastAdjustment"/>
    /// </summary>
    [Flags]
    public enum ElementHighContrastAdjustment : uint
    {
        None = 0,
        Application = 2147483648,
        Auto = 4294967295,
    }
}