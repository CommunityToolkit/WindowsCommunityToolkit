// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Microsoft.Toolkit.Win32.UI.Controls.WPF
{
    /// <summary>
    /// <see cref="Windows.UI.Xaml.Media.Shadow"/>
    /// </summary>
    public class Shadow
    {
        internal Windows.UI.Xaml.Media.Shadow UwpInstance { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Shadow"/> class, a
        /// Wpf-enabled wrapper for <see cref="Windows.UI.Xaml.Media.Shadow"/>
        /// </summary>
        public Shadow(Windows.UI.Xaml.Media.Shadow instance)
        {
            this.UwpInstance = instance;
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="Windows.UI.Xaml.Media.Shadow"/> to <see cref="Microsoft.Toolkit.Win32.UI.Controls.WPF.Shadow"/>.
        /// </summary>
        /// <param name="args">The <see cref="Windows.UI.Xaml.Media.Shadow"/> instance containing the event data.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator Shadow(
            Windows.UI.Xaml.Media.Shadow args)
        {
            return FromShadow(args);
        }

        /// <summary>
        /// Creates a <see cref="Shadow"/> from <see cref="Windows.UI.Xaml.Media.Shadow"/>.
        /// </summary>
        /// <param name="args">The <see cref="Windows.UI.Xaml.Media.Shadow"/> instance containing the event data.</param>
        /// <returns><see cref="Shadow"/></returns>
        public static Shadow FromShadow(Windows.UI.Xaml.Media.Shadow args)
        {
            return new Shadow(args);
        }
    }
}