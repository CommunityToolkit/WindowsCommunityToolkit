// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Microsoft.Toolkit.Win32.UI.Controls.WPF
{
    /// <summary>
    /// <see cref="Windows.UI.Xaml.Input.ProcessKeyboardAcceleratorEventArgs"/>
    /// </summary>
    public class ProcessKeyboardAcceleratorEventArgs
    {
        internal Windows.UI.Xaml.Input.ProcessKeyboardAcceleratorEventArgs UwpInstance { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ProcessKeyboardAcceleratorEventArgs"/> class, a
        /// Wpf-enabled wrapper for <see cref="Windows.UI.Xaml.Input.ProcessKeyboardAcceleratorEventArgs"/>
        /// </summary>
        public ProcessKeyboardAcceleratorEventArgs(Windows.UI.Xaml.Input.ProcessKeyboardAcceleratorEventArgs instance)
        {
            this.UwpInstance = instance;
        }

        /// <summary>
        /// Gets or sets a value indicating whether <see cref="Windows.UI.Xaml.Input.ProcessKeyboardAcceleratorEventArgs.Handled"/>
        /// </summary>
        public bool Handled
        {
            get => UwpInstance.Handled;
            set => UwpInstance.Handled = value;
        }

        /// <summary>
        /// Gets <see cref="Windows.UI.Xaml.Input.ProcessKeyboardAcceleratorEventArgs.Key"/>
        /// </summary>
        public Windows.System.VirtualKey Key
        {
            get => UwpInstance.Key;
        }

        /// <summary>
        /// Gets <see cref="Windows.UI.Xaml.Input.ProcessKeyboardAcceleratorEventArgs.Modifiers"/>
        /// </summary>
        public Windows.System.VirtualKeyModifiers Modifiers
        {
            get => UwpInstance.Modifiers;
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="Windows.UI.Xaml.Input.ProcessKeyboardAcceleratorEventArgs"/> to <see cref="Microsoft.Toolkit.Win32.UI.Controls.WPF.ProcessKeyboardAcceleratorEventArgs"/>.
        /// </summary>
        /// <param name="args">The <see cref="Windows.UI.Xaml.Input.ProcessKeyboardAcceleratorEventArgs"/> instance containing the event data.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator ProcessKeyboardAcceleratorEventArgs(
            Windows.UI.Xaml.Input.ProcessKeyboardAcceleratorEventArgs args)
        {
            return FromProcessKeyboardAcceleratorEventArgs(args);
        }

        /// <summary>
        /// Creates a <see cref="ProcessKeyboardAcceleratorEventArgs"/> from <see cref="Windows.UI.Xaml.Input.ProcessKeyboardAcceleratorEventArgs"/>.
        /// </summary>
        /// <param name="args">The <see cref="Windows.UI.Xaml.Input.ProcessKeyboardAcceleratorEventArgs"/> instance containing the event data.</param>
        /// <returns><see cref="ProcessKeyboardAcceleratorEventArgs"/></returns>
        public static ProcessKeyboardAcceleratorEventArgs FromProcessKeyboardAcceleratorEventArgs(Windows.UI.Xaml.Input.ProcessKeyboardAcceleratorEventArgs args)
        {
            return new ProcessKeyboardAcceleratorEventArgs(args);
        }
    }
}