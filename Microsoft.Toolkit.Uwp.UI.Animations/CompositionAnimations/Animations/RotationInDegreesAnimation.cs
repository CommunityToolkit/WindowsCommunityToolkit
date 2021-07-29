// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Windows.UI.Composition;

namespace Microsoft.Toolkit.Uwp.UI.Animations
{
    /// <summary>
    /// ScalarAnimation that animates the <see cref="Visual.RotationAngleInDegrees"/> property
    /// </summary>
    public class RotationInDegreesAnimation : ScalarAnimation
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RotationInDegreesAnimation"/> class.
        /// </summary>
        public RotationInDegreesAnimation()
        {
            Target = nameof(Visual.RotationAngleInDegrees);
        }
    }
}
