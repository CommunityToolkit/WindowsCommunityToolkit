// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Windows.Media.Casting;
using Windows.UI.Composition;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    /// <summary>
    /// The ImageEx control extends the default Image platform control improving the performance and responsiveness of your Apps.
    /// Source images are downloaded asynchronously showing a load indicator while in progress.
    /// Once downloaded, the source image is stored in the App local cache to preserve resources and load time next time the image needs to be displayed.
    /// </summary>
    public partial class ImageEx
    {
        /// <summary>
        /// Identifies the <see cref="NineGrid"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty NineGridProperty = DependencyProperty.Register(nameof(NineGrid), typeof(Thickness), typeof(ImageEx), new PropertyMetadata(default(Thickness)));

        /// <summary>
        /// Gets or sets the nine-grid used by the image.
        /// </summary>
        public Thickness NineGrid
        {
            get { return (Thickness)GetValue(NineGridProperty); }
            set { SetValue(NineGridProperty, value); }
        }

        /// <inheritdoc/>
        public override CompositionBrush GetAlphaMask()
        {
            return IsInitialized ? (Image as Image).GetAlphaMask() : null;
        }

        /// <summary>
        /// Returns the image as a <see cref="CastingSource"/>.
        /// </summary>
        /// <returns>The image as a <see cref="CastingSource"/>.</returns>
        public CastingSource GetAsCastingSource()
        {
            return IsInitialized ? (Image as Image).GetAsCastingSource() : null;
        }
    }
}