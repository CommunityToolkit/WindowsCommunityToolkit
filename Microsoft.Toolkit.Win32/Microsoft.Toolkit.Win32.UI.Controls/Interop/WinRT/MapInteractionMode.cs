// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Microsoft.Toolkit.Win32.UI.Controls.Interop.WinRT
{
    /// <summary>
    /// <see cref="Windows.UI.Xaml.Controls.Maps.MapInteractionMode"/>
    /// </summary>
    public enum MapInteractionMode
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