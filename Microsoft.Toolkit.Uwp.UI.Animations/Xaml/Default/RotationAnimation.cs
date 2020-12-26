// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Windows.UI.Composition;
using Windows.UI.Xaml.Media.Animation;
using static Microsoft.Toolkit.Uwp.UI.Animations.Extensions.AnimationExtensions;

namespace Microsoft.Toolkit.Uwp.UI.Animations
{
    /// <summary>
    /// A rotation animation working on the composition or XAML layer.
    /// </summary>
    public sealed class RotationAnimation : ImplicitAnimation<double?, double>
    {
        /// <inheritdoc/>
        protected override string ExplicitTarget => nameof(Visual.RotationAngle);

        /// <inheritdoc/>
        public override AnimationBuilder AppendToBuilder(AnimationBuilder builder, TimeSpan? delayHint, TimeSpan? durationHint, EasingType? easingTypeHint, EasingMode? easingModeHint)
        {
            if (KeyFrames.Count > 0)
            {
                return builder.Rotation().NormalizedKeyFrames(
                    delay: Delay ?? delayHint,
                    duration: Duration ?? durationHint,
                    build: b => KeyFrame<double?, double>.AppendToBuilder(b, KeyFrames));
            }

            return builder.Rotation(
                To!.Value,
                From,
                Delay ?? delayHint,
                Duration ?? durationHint,
                EasingType ?? easingTypeHint ?? DefaultEasingType,
                EasingMode ?? easingModeHint ?? DefaultEasingMode);
        }

        /// <inheritdoc/>
        protected override (double? To, double? From) GetParsedValues()
        {
            return (To, From);
        }
    }
}
