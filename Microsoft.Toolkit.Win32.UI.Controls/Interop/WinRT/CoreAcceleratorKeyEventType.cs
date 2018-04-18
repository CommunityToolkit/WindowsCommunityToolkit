// ******************************************************************
// Copyright (c) Microsoft. All rights reserved.
// This code is licensed under the MIT License (MIT).
// THE CODE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
// TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH
// THE CODE OR THE USE OR OTHER DEALINGS IN THE CODE.
// ******************************************************************

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