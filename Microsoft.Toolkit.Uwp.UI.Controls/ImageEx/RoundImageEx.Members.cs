// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Windows.UI.Composition;
using Windows.UI.Xaml;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    /// <summary>
    /// The RoundImageEx control extends the default ImageBrush platform control improving the performance and responsiveness of your Apps.
    /// Source images are downloaded asynchronously showing a load indicator while in progress.
    /// Once downloaded, the source image is stored in the App local cache to preserve resources and load time next time the image needs to be displayed.
    /// </summary>
    public partial class RoundImageEx
    {
        /// <summary>
        /// Identifies the <see cref="CornerRadius"/> dependency property.
        /// </summary>
        public static new readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.Register(nameof(CornerRadius), typeof(double), typeof(RoundImageEx), new PropertyMetadata(0));

        /// <summary>
        /// Gets or sets the corner radius of the image
        /// </summary>
        [Obsolete("Use ImageEx directly instead for 16299 and above.", false)]
        public new double CornerRadius
        {
            get { return (double)GetValue(CornerRadiusProperty); }
            set { SetValue(CornerRadiusProperty, value); }
        }

        /// <inheritdoc/>
        public override CompositionBrush GetAlphaMask()
        {
            return IsInitialized ? ImageRectangle.GetAlphaMask() : null;
        }
    }
}