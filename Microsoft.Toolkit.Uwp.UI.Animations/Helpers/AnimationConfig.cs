// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Linq;

namespace Microsoft.Toolkit.Uwp.UI.Animations
{
    /// <summary>
    /// Configuration used for UI element animation.
    /// </summary>
    public class AnimationConfig
    {
        private readonly AnimationTarget[] defaultAnimations = new AnimationTarget[] { AnimationTarget.Translation, AnimationTarget.Opacity };

        /// <summary>
        /// Gets or sets id of UI element.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets additional animations applied to UI elements.
        /// </summary>
        public AnimationTarget[] AdditionalAnimations { get; set; }

        /// <summary>
        /// Gets all animations applied to UI elements.
        /// </summary>
        public IEnumerable<AnimationTarget> Animations
        {
            get
            {
                return new HashSet<AnimationTarget>(this.AdditionalAnimations?.Concat(this.defaultAnimations) ?? this.defaultAnimations);
            }
        }
    }
}
