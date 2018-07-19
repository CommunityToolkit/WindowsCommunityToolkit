using System;

namespace Microsoft.Toolkit.Win32.UI.Controls.WPF
{
    /// <summary>
    /// <see cref="global::Windows.UI.Core.CoreInputDeviceTypes"/>
    /// </summary>
    [Flags]
    public enum CoreInputDeviceTypes : uint
    {
        None = 0,
        Touch = 1,
        Pen = 2,
        Mouse = 4,
    }
}