// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using Windows.UI.Xaml.Media.Animation;
using static Microsoft.Toolkit.Uwp.UI.Animations.Extensions.AnimationExtensions;

namespace Microsoft.Toolkit.Uwp.UI.Animations
{
    /// <summary>
    /// A generic keyframe animation builder.
    /// </summary>
    /// <typeparam name="T">The type of values being set by the animation being constructed.</typeparam>
    internal abstract partial class NormalizedKeyFrameAnimationBuilder<T> : INormalizedKeyFrameAnimationBuilder<T>
        where T : unmanaged
    {
        /// <summary>
        /// The target property to animate.
        /// </summary>
        private readonly string property;

        /// <summary>
        /// The target delay for the animation, if any.
        /// </summary>
        private readonly TimeSpan? delay;

        /// <summary>
        /// The target duration for the animation.
        /// </summary>
        private readonly TimeSpan? duration;

        /// <summary>
        /// The list of keyframes to use.
        /// </summary>
        private readonly List<KeyFrameInfo> keyFrames = new();

        /// <summary>
        /// Initializes a new instance of the <see cref="NormalizedKeyFrameAnimationBuilder{T}"/> class.
        /// </summary>
        /// <param name="property">The target property to animate.</param>
        /// <param name="delay">The target delay for the animation, if any.</param>
        /// <param name="duration">The target duration for the animation.</param>
        protected NormalizedKeyFrameAnimationBuilder(string property, TimeSpan? delay, TimeSpan? duration)
        {
            this.property = property;
            this.delay = delay;
            this.duration = duration;
        }

        /// <inheritdoc/>
        public INormalizedKeyFrameAnimationBuilder<T> KeyFrame(
            double progress,
            T value,
            EasingType easingType = DefaultEasingType,
            EasingMode easingMode = DefaultEasingMode)
        {
            this.keyFrames.Add(new(progress, value, easingType, easingMode));

            return this;
        }

        /// <summary>
        /// The abstracted info for a normalized animation keyframe.
        /// </summary>
        protected readonly struct KeyFrameInfo
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="KeyFrameInfo"/> struct.
            /// </summary>
            /// <param name="progress">The normalized progress for the keyframe.</param>
            /// <param name="value">The value for the new keyframe to add.</param>
            /// <param name="easingType">The easing type to use to reach the new keyframe.</param>
            /// <param name="easingMode">The easing mode to use to reach the new keyframe.</param>
            public KeyFrameInfo(
                double progress,
                T value,
                EasingType easingType,
                EasingMode easingMode)
            {
                Progress = progress;
                Value = value;
                EasingType = easingType;
                EasingMode = easingMode;
            }

            /// <summary>
            /// The normalized progress for the keyframe.
            /// </summary>
            public readonly double Progress;

            /// <summary>
            /// The value for the new keyframe to add.
            /// </summary>
            public readonly T Value;

            /// <summary>
            /// The easing type to use to reach the new keyframe.
            /// </summary>
            public readonly EasingType EasingType;

            /// <summary>
            /// The easing mode to use to reach the new keyframe.
            /// </summary>
            public readonly EasingMode EasingMode;

            /// <summary>
            /// Gets a <see cref="KeyTime"/> for the current instance, relative to a <see cref="TimeSpan"/> value.
            /// </summary>
            /// <param name="duration">The target <see cref="TimeSpan"/> value to use.</param>
            /// <returns>A normalized <see cref="KeyTime"/> value relative to <paramref name="duration"/>.</returns>
            public KeyTime GetKeyTime(TimeSpan duration)
            {
                long ticks = duration.Ticks;
                TimeSpan step = TimeSpan.FromTicks((long)(ticks * Progress));

                return KeyTime.FromTimeSpan(step);
            }
        }
    }
}
