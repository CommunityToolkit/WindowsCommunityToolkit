// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Microsoft.Toolkit.Win32.UI.Controls.WPF
{
    /// <summary>
    /// <see cref="Windows.UI.Xaml.Media.GeneralTransform"/>
    /// </summary>
    public class GeneralTransform
    {
        internal Windows.UI.Xaml.Media.GeneralTransform UwpInstance { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="GeneralTransform"/> class, a
        /// Wpf-enabled wrapper for <see cref="Windows.UI.Xaml.Media.GeneralTransform"/>
        /// </summary>
        public GeneralTransform(Windows.UI.Xaml.Media.GeneralTransform instance)
        {
            this.UwpInstance = instance;
        }

        /// <summary>
        /// Gets <see cref="Windows.UI.Xaml.Media.GeneralTransform.Inverse"/>
        /// </summary>
        public Microsoft.Toolkit.Win32.UI.Controls.WPF.GeneralTransform Inverse
        {
            get => UwpInstance.Inverse;
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="Windows.UI.Xaml.Media.GeneralTransform"/> to <see cref="Microsoft.Toolkit.Win32.UI.Controls.WPF.GeneralTransform"/>.
        /// </summary>
        /// <param name="args">The <see cref="Windows.UI.Xaml.Media.GeneralTransform"/> instance containing the event data.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator GeneralTransform(
            Windows.UI.Xaml.Media.GeneralTransform args)
        {
            return FromGeneralTransform(args);
        }

        /// <summary>
        /// Creates a <see cref="GeneralTransform"/> from <see cref="Windows.UI.Xaml.Media.GeneralTransform"/>.
        /// </summary>
        /// <param name="args">The <see cref="Windows.UI.Xaml.Media.GeneralTransform"/> instance containing the event data.</param>
        /// <returns><see cref="GeneralTransform"/></returns>
        public static GeneralTransform FromGeneralTransform(Windows.UI.Xaml.Media.GeneralTransform args)
        {
            return new GeneralTransform(args);
        }
    }
}