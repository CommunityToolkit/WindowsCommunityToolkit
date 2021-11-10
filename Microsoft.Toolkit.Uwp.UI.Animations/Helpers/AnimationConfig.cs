// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Microsoft.Toolkit.Uwp.UI.Animations
{
    /// <summary>
    /// Configuration used for UI element animation.
    /// </summary>
    public class AnimationConfig
    {
        /// <summary>
        /// Gets or sets id of UI element.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the scale strategy of UI element.
        /// </summary>
        public ScaleMode ScaleMode { get; set; } = ScaleMode.None;
    }
}
