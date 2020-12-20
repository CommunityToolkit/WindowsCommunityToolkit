// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Windows.UI.Composition;

namespace Microsoft.Toolkit.Uwp.UI.Animations
{
    /// <inheritdoc cref="TimedKeyFrameAnimationBuilder{T}"/>
    internal abstract partial class TimedKeyFrameAnimationBuilder<T> : ITimedKeyFrameAnimationBuilder<T>
        where T : unmanaged
    {
        /// <summary>
        /// A custom <see cref="TimedKeyFrameAnimationBuilder{T}"/> class targeting the composition layer.
        /// </summary>
        public sealed class Composition : TimedKeyFrameAnimationBuilder<T>, AnimationBuilder.ICompositionAnimationFactory
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="Composition"/> class.
            /// </summary>
            /// <inheritdoc cref="TimedKeyFrameAnimationBuilder{T}"/>
            public Composition(string property, TimeSpan? delay)
                : base(property, delay)
            {
            }

            /// <inheritdoc/>
            public CompositionAnimation GetAnimation(Visual visual)
            {
                // We can retrieve the total duration from the last timed keyframe, and then set
                // this as the target duration and use it to normalize the keyframe progresses.
                TimeSpan duration = this.keyFrames[this.keyFrames.Count - 1].GetTimedProgress(default);

                return NormalizedKeyFrameAnimationBuilder<T>.GetAnimation(
                    visual,
                    this.property,
                    this.delay,
                    duration,
                    this.keyFrames);
            }
        }
    }
}
