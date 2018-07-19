using System;

namespace Microsoft.Toolkit.Win32.UI.Controls.WPF
{
    /// <summary>
    /// <see cref="global::Windows.UI.Xaml.Input.ManipulationModes"/>
    /// </summary>
    [Flags]
    public enum ManipulationModes : uint
    {
        None = 0,
        TranslateX = 1,
        TranslateY = 2,
        TranslateRailsX = 4,
        TranslateRailsY = 8,
        Rotate = 16,
        Scale = 32,
        TranslateInertia = 64,
        RotateInertia = 128,
        ScaleInertia = 256,
        All = 65535,
        System = 65536,
    }
}