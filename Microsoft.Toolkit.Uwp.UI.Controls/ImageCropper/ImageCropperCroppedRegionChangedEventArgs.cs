// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Windows.Foundation;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    /// <summary>
    ///为< 请参阅cref = “ ImageCropper.CroppedRegionChanged ” />事件提供事件数据。 
    /// </summary>
    public sealed class ImageCropperCroppedRegionChangedEventArgs
    {
        /// <summary>
        /// Gets the region that is currently cropped in the control.
        /// </summary>
        public Rect NewRegion { get; internal set; }

        /// <summary>
        /// Gets the region that was previously cropped in the control.
        /// </summary>
        public Rect OldRegion { get; internal set; }
    }
}
