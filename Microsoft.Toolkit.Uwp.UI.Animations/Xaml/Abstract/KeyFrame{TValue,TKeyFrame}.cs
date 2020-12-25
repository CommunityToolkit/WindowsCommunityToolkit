// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

#nullable enable

using System.Collections.Generic;
using Windows.UI.Xaml.Media.Animation;
using static Microsoft.Toolkit.Uwp.UI.Animations.Extensions.AnimationExtensions;

namespace Microsoft.Toolkit.Uwp.UI.Animations
{
    /// <summary>
    /// A base model representing a typed keyframe that can be used in XAML.
    /// </summary>
    /// <typeparam name="TValue">
    /// The type to use for the public <see cref="Value"/> property.
    /// This can differ from <typeparamref name="TKeyFrame"/> to facilitate XAML parsing.
    /// </typeparam>
    /// <typeparam name="TKeyFrame">The actual type of keyframe values in use.</typeparam>
    public abstract class KeyFrame<TValue, TKeyFrame> : IKeyFrame<TKeyFrame>
    {
        /// <summary>
        /// Gets or sets the key time for the current keyframe. This is a normalized
        /// value in the [0, 1] range, relative to the total animation duration.
        /// </summary>
        public double Key { get; set; }

        /// <summary>
        /// Gets or sets the animation value for the current keyframe.
        /// </summary>
        public TValue? Value { get; set; }

        /// <summary>
        /// Gets or sets the optional easing function type for the keyframe.
        /// </summary>
        public EasingType? EasingType { get; set; }

        /// <summary>
        /// Gets or sets the optional easing function mode for the keyframe.
        /// </summary>
        public EasingMode? EasingMode { get; set; }

        /// <summary>
        /// Appends a sequence of <see cref="IKeyFrame{T}"/> instances to a target <see cref="INormalizedKeyFrameAnimationBuilder{T}"/> instance.
        /// </summary>
        /// <param name="builder">The target <see cref="INormalizedKeyFrameAnimationBuilder{T}"/> instance to add the keyframe to.</param>
        /// <param name="keyFrames">The keyframes to append.</param>
        /// <returns>The same <see cref="INormalizedKeyFrameAnimationBuilder{T}"/> instance as <paramref name="builder"/>.</returns>
        public static INormalizedKeyFrameAnimationBuilder<TKeyFrame> AppendToBuilder(INormalizedKeyFrameAnimationBuilder<TKeyFrame> builder, IEnumerable<IKeyFrame<TKeyFrame>> keyFrames)
        {
            foreach (var keyFrame in keyFrames)
            {
                builder = keyFrame.AppendToBuilder(builder);
            }

            return builder;
        }

        /// <inheritdoc/>
        public INormalizedKeyFrameAnimationBuilder<TKeyFrame> AppendToBuilder(INormalizedKeyFrameAnimationBuilder<TKeyFrame> builder)
        {
            return builder.KeyFrame(Key, GetParsedValue()!, EasingType ?? DefaultEasingType, EasingMode ?? DefaultEasingMode);
        }

        /// <summary>
        /// Gets the parsed <typeparamref name="TKeyFrame"/> values for <see cref="Value"/>.
        /// </summary>
        /// <returns>The parsed keyframe values a <typeparamref name="TKeyFrame"/>.</returns>
        protected abstract TKeyFrame? GetParsedValue();
    }
}
