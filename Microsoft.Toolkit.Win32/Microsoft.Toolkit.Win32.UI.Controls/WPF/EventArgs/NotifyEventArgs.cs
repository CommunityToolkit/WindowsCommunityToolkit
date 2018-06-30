using System;
using System.Security;
using Microsoft.Windows.Interop;

namespace Microsoft.Toolkit.Win32.UI.Controls.WPF
{
    /// <summary>
    /// Provides data for events. This class cannot be inherited.
    /// </summary>
    /// <remarks>Copy from <see cref="global::Windows.UI.Xaml.Controls.NotifyEventArgs"/> to avoid requirement to link Windows.winmd</remarks>
    /// <seealso cref="global::Windows.UI.Xaml.Controls.NotifyEventArgs"/>
    public sealed class NotifyEventArgs : EventArgs
    {
        [SecurityCritical]
        private readonly global::Windows.UI.Xaml.Controls.NotifyEventArgs _args;

        [SecurityCritical]
        internal NotifyEventArgs(global::Windows.UI.Xaml.Controls.NotifyEventArgs args)
        {
            _args = args;
        }

        public string Value
        {
            [SecurityCritical]
            get => (string)_args.Value;
        }

        public System.Uri CallingUri
        {
            [SecurityCritical]
            get => (System.Uri)_args.CallingUri;
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="global::Windows.UI.Xaml.Controls.NotifyEventArgs"/> to <see cref="Microsoft.Toolkit.Win32.UI.Controls.WPF.NotifyEventArgs"/>.
        /// </summary>
        /// <param name="args">The <see cref="global::Windows.UI.Xaml.Controls.NotifyEventArgs"/> instance containing the event data.</param>
        /// <returns>The result of the conversion.</returns>
        [SecurityCritical]
        public static implicit operator NotifyEventArgs(
            global::Windows.UI.Xaml.Controls.NotifyEventArgs args)
        {
            return FromNotifyEventArgs(args);
        }

        /// <summary>
        /// Creates a <see cref="NotifyEventArgs"/> from <see cref="global::Windows.UI.Xaml.Controls.NotifyEventArgs"/>.
        /// </summary>
        /// <param name="args">The <see cref="global::Windows.UI.Xaml.Controls.NotifyEventArgs"/> instance containing the event data.</param>
        /// <returns><see cref="NotifyEventArgs"/></returns>
        public static NotifyEventArgs FromNotifyEventArgs(global::Windows.UI.Xaml.Controls.NotifyEventArgs args)
        {
            return new NotifyEventArgs(args);
        }
    }
}