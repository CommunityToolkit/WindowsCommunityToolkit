// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Security;
using Microsoft.Windows.Interop;

namespace Microsoft.Toolkit.Win32.UI.Controls.WPF
{
    /// <summary>
    /// Provides data for events. This class cannot be inherited.
    /// </summary>
    /// <remarks>Copy from <see cref="global::Windows.UI.Xaml.Controls.InkToolbarIsStencilButtonCheckedChangedEventArgs"/> to avoid requirement to link Windows.winmd</remarks>
    /// <seealso cref="global::Windows.UI.Xaml.Controls.InkToolbarIsStencilButtonCheckedChangedEventArgs"/>
    public sealed class InkToolbarIsStencilButtonCheckedChangedEventArgs : EventArgs
    {
        [SecurityCritical]
        private readonly global::Windows.UI.Xaml.Controls.InkToolbarIsStencilButtonCheckedChangedEventArgs _args;

        [SecurityCritical]
        internal InkToolbarIsStencilButtonCheckedChangedEventArgs(global::Windows.UI.Xaml.Controls.InkToolbarIsStencilButtonCheckedChangedEventArgs args)
        {
            _args = args;
        }

        public Microsoft.Toolkit.Win32.UI.Controls.WPF.InkToolbarStencilButton StencilButton
        {
            [SecurityCritical]
            get => (Microsoft.Toolkit.Win32.UI.Controls.WPF.InkToolbarStencilButton)_args.StencilButton.GetWrapper();
        }

        public global::Windows.UI.Xaml.Controls.InkToolbarStencilKind StencilKind
        {
            [SecurityCritical]
            get => (global::Windows.UI.Xaml.Controls.InkToolbarStencilKind)_args.StencilKind;
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="global::Windows.UI.Xaml.Controls.InkToolbarIsStencilButtonCheckedChangedEventArgs"/> to <see cref="Microsoft.Toolkit.Win32.UI.Controls.WPF.InkToolbarIsStencilButtonCheckedChangedEventArgs"/>.
        /// </summary>
        /// <param name="args">The <see cref="global::Windows.UI.Xaml.Controls.InkToolbarIsStencilButtonCheckedChangedEventArgs"/> instance containing the event data.</param>
        /// <returns>The result of the conversion.</returns>
        [SecurityCritical]
        public static implicit operator InkToolbarIsStencilButtonCheckedChangedEventArgs(
            global::Windows.UI.Xaml.Controls.InkToolbarIsStencilButtonCheckedChangedEventArgs args)
        {
            return FromInkToolbarIsStencilButtonCheckedChangedEventArgs(args);
        }

        /// <summary>
        /// Creates a <see cref="InkToolbarIsStencilButtonCheckedChangedEventArgs"/> from <see cref="global::Windows.UI.Xaml.Controls.InkToolbarIsStencilButtonCheckedChangedEventArgs"/>.
        /// </summary>
        /// <param name="args">The <see cref="global::Windows.UI.Xaml.Controls.InkToolbarIsStencilButtonCheckedChangedEventArgs"/> instance containing the event data.</param>
        /// <returns><see cref="InkToolbarIsStencilButtonCheckedChangedEventArgs"/></returns>
        public static InkToolbarIsStencilButtonCheckedChangedEventArgs FromInkToolbarIsStencilButtonCheckedChangedEventArgs(global::Windows.UI.Xaml.Controls.InkToolbarIsStencilButtonCheckedChangedEventArgs args)
        {
            return new InkToolbarIsStencilButtonCheckedChangedEventArgs(args);
        }
    }
}