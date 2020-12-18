// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Microsoft.Toolkit.Uwp.UI.Extensions;

namespace Microsoft.Toolkit.Uwp.UI.Animations.Xaml
{
    /// <summary>
    /// A translation animation working on the composition or XAML layer.
    /// </summary>
    public class TranslationAnimation : TypedAnimation<string>, ITimeline
    {
        /// <summary>
        /// Gets or sets the target framework layer to animate.
        /// </summary>
        public FrameworkLayer Layer { get; set; }

        /// <inheritdoc/>
        AnimationBuilder ITimeline.AppendToBuilder(AnimationBuilder builder, TimeSpan? delayHint, TimeSpan? durationHint)
        {
            if (Layer == FrameworkLayer.Composition)
            {
                return builder.Translation(From?.ToVector3(), To!.ToVector3(), Delay ?? delayHint, Duration ?? durationHint.GetValueOrDefault(), EasingType, EasingMode);
            }
            else
            {
                return builder.Translation(From?.ToVector2(), To!.ToVector2(), Delay ?? delayHint, Duration ?? durationHint.GetValueOrDefault(), EasingType, EasingMode, FrameworkLayer.Xaml);
            }
        }
    }
}
