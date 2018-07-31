// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Microsoft.Toolkit.Win32.UI.Controls.WPF
{
    /// <summary>
    /// <see cref="Windows.UI.Xaml.Media.Transform"/>
    /// </summary>
    public class Transform
    {
        internal Windows.UI.Xaml.Media.Transform UwpInstance { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Transform"/> class, a
        /// Wpf-enabled wrapper for <see cref="Windows.UI.Xaml.Media.Transform"/>
        /// </summary>
        public Transform(Windows.UI.Xaml.Media.Transform instance)
        {
            this.UwpInstance = instance;
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="Windows.UI.Xaml.Media.Transform"/> to <see cref="Microsoft.Toolkit.Win32.UI.Controls.WPF.Transform"/>.
        /// </summary>
        /// <param name="args">The <see cref="Windows.UI.Xaml.Media.Transform"/> instance containing the event data.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator Transform(
            Windows.UI.Xaml.Media.Transform args)
        {
            return FromTransform(args);
        }

        /// <summary>
        /// Creates a <see cref="Transform"/> from <see cref="Windows.UI.Xaml.Media.Transform"/>.
        /// </summary>
        /// <param name="args">The <see cref="Windows.UI.Xaml.Media.Transform"/> instance containing the event data.</param>
        /// <returns><see cref="Transform"/></returns>
        public static Transform FromTransform(Windows.UI.Xaml.Media.Transform args)
        {
            return new Transform(args);
        }
    }
}