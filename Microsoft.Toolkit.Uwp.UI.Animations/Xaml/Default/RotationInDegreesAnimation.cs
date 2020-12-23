// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Windows.UI.Xaml.Media.Animation;
using static Microsoft.Toolkit.Uwp.UI.Animations.Extensions.AnimationExtensions;

namespace Microsoft.Toolkit.Uwp.UI.Animations.Xaml
{
    /// <summary>
    /// A rotation in degrees animation working on the composition or XAML layer.
    /// </summary>
    public class RotationInDegreesAnimation : Animation<double?>
    {
        /// <summary>
        /// Gets or sets the target framework layer to animate.
        /// </summary>
        public FrameworkLayer Layer { get; set; }

        /// <inheritdoc/>
        public override AnimationBuilder AppendToBuilder(AnimationBuilder builder, TimeSpan? delayHint, TimeSpan? durationHint, EasingType? easingTypeHint, EasingMode? easingModeHint)
        {
            return builder.RotationInDegrees(
                To!.Value,
                From,
                Delay ?? delayHint,
                Duration ?? durationHint,
                EasingType ?? easingTypeHint ?? DefaultEasingType,
                EasingMode ?? easingModeHint ?? DefaultEasingMode,
                Layer);
        }
    }
}
