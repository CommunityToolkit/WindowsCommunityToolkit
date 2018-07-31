// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Microsoft.Toolkit.Win32.UI.Controls.WPF
{
    /// <summary>
    /// <see cref="Windows.UI.Xaml.Controls.InkToolbarCustomPen"/>
    /// </summary>
    public class InkToolbarCustomPen
    {
        internal Windows.UI.Xaml.Controls.InkToolbarCustomPen UwpInstance { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="InkToolbarCustomPen"/> class, a
        /// Wpf-enabled wrapper for <see cref="Windows.UI.Xaml.Controls.InkToolbarCustomPen"/>
        /// </summary>
        public InkToolbarCustomPen(Windows.UI.Xaml.Controls.InkToolbarCustomPen instance)
        {
            this.UwpInstance = instance;
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="Windows.UI.Xaml.Controls.InkToolbarCustomPen"/> to <see cref="Microsoft.Toolkit.Win32.UI.Controls.WPF.InkToolbarCustomPen"/>.
        /// </summary>
        /// <param name="args">The <see cref="Windows.UI.Xaml.Controls.InkToolbarCustomPen"/> instance containing the event data.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator InkToolbarCustomPen(
            Windows.UI.Xaml.Controls.InkToolbarCustomPen args)
        {
            return FromInkToolbarCustomPen(args);
        }

        /// <summary>
        /// Creates a <see cref="InkToolbarCustomPen"/> from <see cref="Windows.UI.Xaml.Controls.InkToolbarCustomPen"/>.
        /// </summary>
        /// <param name="args">The <see cref="Windows.UI.Xaml.Controls.InkToolbarCustomPen"/> instance containing the event data.</param>
        /// <returns><see cref="InkToolbarCustomPen"/></returns>
        public static InkToolbarCustomPen FromInkToolbarCustomPen(Windows.UI.Xaml.Controls.InkToolbarCustomPen args)
        {
            return new InkToolbarCustomPen(args);
        }
    }
}