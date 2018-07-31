// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;

namespace Microsoft.Toolkit.Win32.UI.Controls.WPF
{
    /// <summary>
    /// Provides data for events. This class cannot be inherited.
    /// </summary>
    /// <remarks>Copy from <see cref="Windows.UI.Xaml.Controls.InkToolbarIsStencilButtonCheckedChangedEventArgs"/> to avoid requirement to link Windows.winmd</remarks>
    /// <seealso cref="Windows.UI.Xaml.Controls.InkToolbarIsStencilButtonCheckedChangedEventArgs"/>
    public sealed class InkToolbarIsStencilButtonCheckedChangedEventArgs : EventArgs
    {
        private readonly Windows.UI.Xaml.Controls.InkToolbarIsStencilButtonCheckedChangedEventArgs _args;

        internal InkToolbarIsStencilButtonCheckedChangedEventArgs(Windows.UI.Xaml.Controls.InkToolbarIsStencilButtonCheckedChangedEventArgs args)
        {
            _args = args;
        }

        public InkToolbarStencilButton StencilButton => (InkToolbarStencilButton)_args.StencilButton.GetWrapper();

        public Interop.WinRT.InkToolbarStencilKind StencilKind => (Interop.WinRT.InkToolbarStencilKind)_args.StencilKind;

        /// <summary>
        /// Performs an implicit conversion from <see cref="Windows.UI.Xaml.Controls.InkToolbarIsStencilButtonCheckedChangedEventArgs"/> to <see cref="Microsoft.Toolkit.Win32.UI.Controls.WPF.InkToolbarIsStencilButtonCheckedChangedEventArgs"/>.
        /// </summary>
        /// <param name="args">The <see cref="Windows.UI.Xaml.Controls.InkToolbarIsStencilButtonCheckedChangedEventArgs"/> instance containing the event data.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator InkToolbarIsStencilButtonCheckedChangedEventArgs(
            Windows.UI.Xaml.Controls.InkToolbarIsStencilButtonCheckedChangedEventArgs args)
        {
            return FromInkToolbarIsStencilButtonCheckedChangedEventArgs(args);
        }

        /// <summary>
        /// Creates a <see cref="InkToolbarIsStencilButtonCheckedChangedEventArgs"/> from <see cref="Windows.UI.Xaml.Controls.InkToolbarIsStencilButtonCheckedChangedEventArgs"/>.
        /// </summary>
        /// <param name="args">The <see cref="Windows.UI.Xaml.Controls.InkToolbarIsStencilButtonCheckedChangedEventArgs"/> instance containing the event data.</param>
        /// <returns><see cref="InkToolbarIsStencilButtonCheckedChangedEventArgs"/></returns>
        public static InkToolbarIsStencilButtonCheckedChangedEventArgs FromInkToolbarIsStencilButtonCheckedChangedEventArgs(Windows.UI.Xaml.Controls.InkToolbarIsStencilButtonCheckedChangedEventArgs args)
        {
            return new InkToolbarIsStencilButtonCheckedChangedEventArgs(args);
        }
    }
}