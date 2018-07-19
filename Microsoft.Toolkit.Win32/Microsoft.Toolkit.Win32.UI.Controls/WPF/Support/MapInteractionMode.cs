using System;

namespace Microsoft.Toolkit.Win32.UI.Controls.WPF
{
    /// <summary>
    /// <see cref="global::Windows.UI.Xaml.Controls.Maps.MapInteractionMode"/>
    /// </summary>
    public enum MapInteractionMode : int
    {
        Auto = 0,
        Disabled = 1,
        GestureOnly = 2,
        PointerAndKeyboard = 2,
        ControlOnly = 3,
        GestureAndControl = 4,
        PointerKeyboardAndControl = 4,
        PointerOnly = 5,
    }
}