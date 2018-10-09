// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Microsoft.Toolkit.Win32.UI.Controls.Interop.WinRT
{
    /// <summary>
    /// <see cref="Windows.UI.Xaml.Media.ImageSource"/>
    /// </summary>
    public class ImageSource
    {
        private Windows.UI.Xaml.Media.ImageSource UwpInstance { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ImageSource"/> class, a
        /// Wpf-enabled wrapper for <see cref="Windows.UI.Xaml.Media.ImageSource"/>
        /// </summary>
        public ImageSource(Windows.UI.Xaml.Media.ImageSource instance)
        {
            this.UwpInstance = instance;
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="Windows.UI.Xaml.Media.ImageSource"/> to <see cref="ImageSource"/>.
        /// </summary>
        /// <param name="args">The <see cref="Windows.UI.Xaml.Media.ImageSource"/> instance containing the event data.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator ImageSource(
            Windows.UI.Xaml.Media.ImageSource args)
        {
            return FromImageSource(args);
        }

        /// <summary>
        /// Creates a <see cref="ImageSource"/> from <see cref="Windows.UI.Xaml.Media.ImageSource"/>.
        /// </summary>
        /// <param name="args">The <see cref="Windows.UI.Xaml.Media.ImageSource"/> instance containing the event data.</param>
        /// <returns><see cref="ImageSource"/></returns>
        public static ImageSource FromImageSource(Windows.UI.Xaml.Media.ImageSource args)
        {
            return new ImageSource(args);
        }
    }
}