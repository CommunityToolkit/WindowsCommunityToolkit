using System;

namespace Microsoft.Toolkit.Win32.UI.Controls.WPF
{
    [Flags]
    public enum CoreInputDeviceTypes : uint
    {
        None = 0,
        Touch = 1,
        Pen = 2,
        Mouse = 4
    }
}