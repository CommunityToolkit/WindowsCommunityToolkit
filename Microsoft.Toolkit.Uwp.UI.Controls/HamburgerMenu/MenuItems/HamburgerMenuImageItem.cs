// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    /// <summary>
    /// The HamburgerMenuImageItem provides an image based implementation for HamburgerMenu entries.
    /// </summary>
    [Obsolete("The HamburgerMenuImageItem will be removed alongside the HamburgerMenu in a future major release. Please use the NavigationView control available in the Fall Creators Update")]
    public class HamburgerMenuImageItem : HamburgerMenuItem
    {
        /// <summary>
        /// Identifies the <see cref="Thumbnail"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ThumbnailProperty = DependencyProperty.Register(nameof(Thumbnail), typeof(ImageSource), typeof(HamburgerMenuItem), new PropertyMetadata(null));

        /// <summary>
        /// Gets or sets a value that specifies a bitmap to display with an Image control.
        /// </summary>
        public ImageSource Thumbnail
        {
            get
            {
                return (ImageSource)GetValue(ThumbnailProperty);
            }

            set
            {
                SetValue(ThumbnailProperty, value);
            }
        }
    }
}
