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

using Windows.System;
using Windows.UI.Xaml.Input;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    /// <summary>
    /// Arguments relating to a CTRL Key Shortcut being activated.
    /// </summary>
    public class ShortcutKeyRequestArgs
    {
        public ShortcutKeyRequestArgs(VirtualKey key, bool shiftKeyHeld, KeyRoutedEventArgs args)
        {
            Key = key;
            ShiftKeyHeld = shiftKeyHeld;
            OriginalArgs = args;
        }

        /// <summary>
        /// Gets key pressed with CTRL
        /// </summary>
        public VirtualKey Key { get; private set; }

        /// <summary>
        /// Gets the Original KeyDown arguments
        /// </summary>
        public KeyRoutedEventArgs OriginalArgs { get; private set; }

        /// <summary>
        /// Gets a value indicating whether Shift was held down too
        /// </summary>
        public bool ShiftKeyHeld { get; private set; }

        /// <summary>
        /// Gets or sets a value indicating whether the Key been Handled
        /// </summary>
        public bool Handled { get; set; } = false;
    }
}