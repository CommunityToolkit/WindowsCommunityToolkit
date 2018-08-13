// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Microsoft.Toolkit.Win32.UI.Controls.Interop.WinRT
{
    /// <summary>Specifies the set of possible accelerator key events that can invoke a callback.</summary>
    /// <remarks>Copy from <see cref="Windows.UI.Core.CoreAcceleratorKeyEventType"/> to avoid requirement to link Windows.winmd</remarks>
    /// <seealso cref="Windows.UI.Core.CoreAcceleratorKeyEventType"/>
#pragma warning disable 1591
    public enum CoreAcceleratorKeyEventType
    {
        KeyDown,
        KeyUp,
        Character,
        DeadCharacter,
        SystemKeyDown,
        SystemKeyUp,
        SystemCharacter,
        SystemDeadCharacter,
        UnicodeCharacter,
    }
#pragma warning restore 1591
}