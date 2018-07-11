using System;
using System.Linq;
using System.Security;

namespace Microsoft.Toolkit.Win32.UI.Controls.WPF
{
    /// <summary>
    /// Provides data for events. This class cannot be inherited.
    /// </summary>
    /// <remarks>Copy from <see cref="global::Windows.UI.Input.Inking.InkStrokesErasedEventArgs"/> to avoid requirement to link Windows.winmd</remarks>
    /// <seealso cref="global::Windows.UI.Input.Inking.InkStrokesErasedEventArgs"/>
    public sealed class InkStrokesErasedEventArgs : EventArgs
    {
        [SecurityCritical]
        private readonly global::Windows.UI.Input.Inking.InkStrokesErasedEventArgs _args;

        [SecurityCritical]
        internal InkStrokesErasedEventArgs(global::Windows.UI.Input.Inking.InkStrokesErasedEventArgs args)
        {
            _args = args;
        }

        public System.Collections.Generic.IReadOnlyList<InkStroke> Strokes
        {
            [SecurityCritical]
            get => _args.Strokes.Cast<InkStroke>().ToList();
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="global::Windows.UI.Input.Inking.InkStrokesErasedEventArgs"/> to <see cref="Microsoft.Toolkit.Win32.UI.Controls.WPF.InkStrokesErasedEventArgs"/>.
        /// </summary>
        /// <param name="args">The <see cref="global::Windows.UI.Input.Inking.InkStrokesErasedEventArgs"/> instance containing the event data.</param>
        /// <returns>The result of the conversion.</returns>
        [SecurityCritical]
        public static implicit operator InkStrokesErasedEventArgs(
            global::Windows.UI.Input.Inking.InkStrokesErasedEventArgs args)
        {
            return FromInkStrokesErasedEventArgs(args);
        }

        /// <summary>
        /// Creates a <see cref="InkStrokesErasedEventArgs"/> from <see cref="global::Windows.UI.Input.Inking.InkStrokesErasedEventArgs"/>.
        /// </summary>
        /// <param name="args">The <see cref="global::Windows.UI.Input.Inking.InkStrokesErasedEventArgs"/> instance containing the event data.</param>
        /// <returns><see cref="InkStrokesErasedEventArgs"/></returns>
        public static InkStrokesErasedEventArgs FromInkStrokesErasedEventArgs(global::Windows.UI.Input.Inking.InkStrokesErasedEventArgs args)
        {
            return new InkStrokesErasedEventArgs(args);
        }
    }
}