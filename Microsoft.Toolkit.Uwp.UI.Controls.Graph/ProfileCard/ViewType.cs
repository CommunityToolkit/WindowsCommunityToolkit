// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Microsoft.Toolkit.Uwp.UI.Controls.Graph
{
    /// <summary>
    /// The visual layout of the <see cref="ProfileCard"/> control. Default is PictureOnly.
    /// </summary>
    public enum ViewType
    {
        /// <summary>
        /// Only the user photo is shown.
        /// </summary>
        PictureOnly = 0,

        /// <summary>
        /// Only the user email is shown.
        /// </summary>
        EmailOnly = 1,

        /// <summary>
        /// A basic user profile is shown, and the user photo is place on the left side.
        /// </summary>
        LargeProfilePhotoLeft = 2,

        /// <summary>
        /// A basic user profile is shown, and the user photo is place on the right side.
        /// </summary>
        LargeProfilePhotoRight = 3
    }
}
