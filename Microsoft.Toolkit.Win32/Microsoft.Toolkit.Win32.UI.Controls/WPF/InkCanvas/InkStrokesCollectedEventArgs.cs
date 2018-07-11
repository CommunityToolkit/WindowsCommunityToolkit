using System;
using System.Security;
using Microsoft.Windows.Interop;

namespace Microsoft.Toolkit.Win32.UI.Controls.WPF
{
    /// <summary>
    /// Provides data for events. This class cannot be inherited.
    /// </summary>
    /// <remarks>Copy from <see cref="global::Windows.UI.Input.Inking.InkStrokesCollectedEventArgs"/> to avoid requirement to link Windows.winmd</remarks>
    /// <seealso cref="global::Windows.UI.Input.Inking.InkStrokesCollectedEventArgs"/>
    public sealed class InkStrokesCollectedEventArgs : EventArgs
    {
        [SecurityCritical]
        private readonly global::Windows.UI.Input.Inking.InkStrokesCollectedEventArgs _args;

        [SecurityCritical]
        internal InkStrokesCollectedEventArgs(global::Windows.UI.Input.Inking.InkStrokesCollectedEventArgs args)
        {
            _args = args;
        }

        public System.Collections.Generic.IReadOnlyList<global::Windows.UI.Input.Inking.InkStroke> Strokes
        {
            [SecurityCritical]
            get => (System.Collections.Generic.IReadOnlyList<global::Windows.UI.Input.Inking.InkStroke>)_args.Strokes;
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="global::Windows.UI.Input.Inking.InkStrokesCollectedEventArgs"/> to <see cref="Microsoft.Toolkit.Win32.UI.Controls.WPF.InkStrokesCollectedEventArgs"/>.
        /// </summary>
        /// <param name="args">The <see cref="global::Windows.UI.Input.Inking.InkStrokesCollectedEventArgs"/> instance containing the event data.</param>
        /// <returns>The result of the conversion.</returns>
        [SecurityCritical]
        public static implicit operator InkStrokesCollectedEventArgs(
            global::Windows.UI.Input.Inking.InkStrokesCollectedEventArgs args)
        {
            return FromInkStrokesCollectedEventArgs(args);
        }

        /// <summary>
        /// Creates a <see cref="InkStrokesCollectedEventArgs"/> from <see cref="global::Windows.UI.Input.Inking.InkStrokesCollectedEventArgs"/>.
        /// </summary>
        /// <param name="args">The <see cref="global::Windows.UI.Input.Inking.InkStrokesCollectedEventArgs"/> instance containing the event data.</param>
        /// <returns><see cref="InkStrokesCollectedEventArgs"/></returns>
        public static InkStrokesCollectedEventArgs FromInkStrokesCollectedEventArgs(global::Windows.UI.Input.Inking.InkStrokesCollectedEventArgs args)
        {
            return new InkStrokesCollectedEventArgs(args);
        }
    }
}