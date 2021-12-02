// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.UI.Input;
using Windows.System;
using Windows.UI.Core;

namespace CommunityToolkit.WinUI.Utilities
{
    internal static class KeyboardHelper
    {
        public static void GetMetaKeyState(out bool ctrl, out bool shift)
        {
            ctrl = InputKeyboardSource.GetKeyStateForCurrentThread(VirtualKey.Control).HasFlag(CoreVirtualKeyStates.Down);
            shift = InputKeyboardSource.GetKeyStateForCurrentThread(VirtualKey.Shift).HasFlag(CoreVirtualKeyStates.Down);
        }

        public static void GetMetaKeyState(out bool ctrl, out bool shift, out bool alt)
        {
            GetMetaKeyState(out ctrl, out shift);
            alt = InputKeyboardSource.GetKeyStateForCurrentThread(VirtualKey.Menu).HasFlag(CoreVirtualKeyStates.Down);
        }
    }
}