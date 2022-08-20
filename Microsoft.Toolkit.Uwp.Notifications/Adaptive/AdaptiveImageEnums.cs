// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Microsoft.Toolkit.Uwp.Notifications
{
    /// <summary>
    /// Specifies the horizontal alignment for an image.
    /// </summary>
    public enum AdaptiveImageAlign
    {
        /// <summary>
        /// Default value, alignment behavior determined by renderer.
        /// </summary>
        Default,

        /// <summary>
        /// Image stretches to fill available width (and potentially available height too, depending on where the image is).
        /// </summary>
        Stretch,

        /// <summary>
        /// Align the image to the left, displaying the image at its native resolution.
        /// </summary>
        Left,

        /// <summary>
        /// Align the image in the center horizontally, displaying the image at its native resolution.
        /// </summary>
        Center,

        /// <summary>
        /// Align the image to the right, displaying the image at its native resolution.
        /// </summary>
        Right
    }

    /// <summary>
    /// Specify the desired cropping of the image.
    /// </summary>
    public enum AdaptiveImageCrop
    {
        /// <summary>
        /// Default value, cropping behavior determined by renderer.
        /// </summary>
        Default,

        /// <summary>
        /// Image is not cropped.
        /// </summary>
        None,

        /// <summary>
        /// Image is cropped to a circle shape.
        /// </summary>
        Circle
    }
}