using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;

namespace Microsoft.Toolkit.Win32.UI.Controls.WPF
{
    /// <summary>
    /// Provides data for events. This class cannot be inherited.
    /// </summary>
    /// <remarks>Copy from <see cref="global::Windows.UI.Core.PointerEventArgs"/> to avoid requirement to link Windows.winmd</remarks>
    /// <seealso cref="global::Windows.UI.Core.PointerEventArgs"/>
    public sealed class PointerEventArgs : EventArgs
    {
        [SecurityCritical]
        private readonly global::Windows.UI.Core.PointerEventArgs _args;

        [SecurityCritical]
        internal PointerEventArgs(global::Windows.UI.Core.PointerEventArgs args)
        {
            _args = args;
        }

        public IList<PointerPoint> GetIntermediatePoints() => _args.GetIntermediatePoints().Cast<PointerPoint>().ToList();

        public bool Handled { get => _args.Handled; set => _args.Handled = value; }

        public PointerPoint CurrentPoint { get => _args.CurrentPoint; }

        public VirtualKeyModifiers KeyModifiers { get => (VirtualKeyModifiers)(uint)_args.KeyModifiers; }

        /// <summary>
        /// Performs an implicit conversion from <see cref="global::Windows.UI.Core.PointerEventArgs"/> to <see cref="Microsoft.Toolkit.Win32.UI.Controls.WPF.PointerEventArgs"/>.
        /// </summary>
        /// <param name="args">The <see cref="global::Windows.UI.Core.PointerEventArgs"/> instance containing the event data.</param>
        /// <returns>The result of the conversion.</returns>
        [SecurityCritical]
        public static implicit operator PointerEventArgs(
            global::Windows.UI.Core.PointerEventArgs args)
        {
            return FromPointerEventArgs(args);
        }

        /// <summary>
        /// Creates a <see cref="PointerEventArgs"/> from <see cref="global::Windows.UI.Core.PointerEventArgs"/>.
        /// </summary>
        /// <param name="args">The <see cref="global::Windows.UI.Core.PointerEventArgs"/> instance containing the event data.</param>
        /// <returns><see cref="PointerEventArgs"/></returns>
        public static PointerEventArgs FromPointerEventArgs(global::Windows.UI.Core.PointerEventArgs args)
        {
            return new PointerEventArgs(args);
        }
    }
}