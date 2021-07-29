// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Windows.UI.Composition;

namespace Microsoft.Toolkit.Uwp.UI.Animations
{
    /// <summary>
    /// Vector3Animation that animates the <see cref="Visual.Scale"/> property
    /// </summary>
    public class ScaleAnimation : Vector3Animation
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ScaleAnimation"/> class.
        /// </summary>
        public ScaleAnimation()
        {
            Target = nameof(Visual.Scale);
        }
    }
}
