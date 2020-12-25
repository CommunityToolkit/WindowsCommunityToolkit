// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Numerics;
using Microsoft.Toolkit.Uwp.UI.Extensions;
using Windows.UI.Composition;
using Windows.UI.Xaml.Media.Animation;
using static Microsoft.Toolkit.Uwp.UI.Animations.Extensions.AnimationExtensions;

namespace Microsoft.Toolkit.Uwp.UI.Animations
{
    /// <summary>
    /// An orientation animation working on the composition layer.
    /// </summary>
    public class OrientationAnimation : ImplicitAnimation<string, Quaternion>
    {
        /// <inheritdoc/>
        protected override string Target => nameof(Visual.Orientation);

        /// <inheritdoc/>
        public override AnimationBuilder AppendToBuilder(AnimationBuilder builder, TimeSpan? delayHint, TimeSpan? durationHint, EasingType? easingTypeHint, EasingMode? easingModeHint)
        {
            if (KeyFrames.Count > 0)
            {
                return builder.Orientation().NormalizedKeyFrames(
                    delay: Delay ?? delayHint,
                    duration: Duration ?? durationHint,
                    build: b => KeyFrame<string, Quaternion>.AppendToBuilder(b, KeyFrames));
            }

            return builder.Orientation(
                To!.ToQuaternion(),
                From?.ToQuaternion(),
                Delay ?? delayHint,
                Duration ?? durationHint,
                EasingType ?? easingTypeHint ?? DefaultEasingType,
                EasingMode ?? easingModeHint ?? DefaultEasingMode);
        }

        /// <inheritdoc/>
        protected override (Quaternion? To, Quaternion? From) GetParsedValues()
        {
            return (To?.ToQuaternion(), From?.ToQuaternion());
        }
    }
}
