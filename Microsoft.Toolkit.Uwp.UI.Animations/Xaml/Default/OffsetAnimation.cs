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
    /// An offset animation working on the composition layer.
    /// </summary>
    public class OffsetAnimation : ImplicitAnimation<string, Vector3>
    {
        /// <inheritdoc/>
        protected override string Target => nameof(Visual.Offset);

        /// <inheritdoc/>
        public override AnimationBuilder AppendToBuilder(AnimationBuilder builder, TimeSpan? delayHint, TimeSpan? durationHint, EasingType? easingTypeHint, EasingMode? easingModeHint)
        {
            if (KeyFrames.Count > 0)
            {
                return builder.Offset().NormalizedKeyFrames(
                    delay: Delay ?? delayHint,
                    duration: Duration ?? durationHint,
                    build: b => KeyFrame<string, Vector3>.AppendToBuilder(b, KeyFrames));
            }

            return builder.Offset(
                To!.ToVector3(),
                From?.ToVector3(),
                Delay ?? delayHint,
                Duration ?? durationHint,
                EasingType ?? easingTypeHint ?? DefaultEasingType,
                EasingMode ?? easingModeHint ?? DefaultEasingMode);
        }

        /// <inheritdoc/>
        protected override (Vector3? To, Vector3? From) GetParsedValues()
        {
            return (To?.ToVector3(), From?.ToVector3());
        }
    }
}
