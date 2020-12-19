// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

#nullable enable

using System;
using static Microsoft.Toolkit.Uwp.UI.Animations.Extensions.AnimationExtensions;

namespace Microsoft.Toolkit.Uwp.UI.Animations
{
    /// <inheritdoc cref="AnimationBuilder"/>
    public sealed partial class AnimationBuilder
    {
        /// <summary>
        /// Adds a custom animation based on normalized keyframes ot the current schedule.
        /// </summary>
        /// <typeparam name="T">The type of values to animate.</typeparam>
        /// <param name="property">The target property to animate.</param>
        /// <param name="build">The callback to use to construct the custom animation.</param>
        /// <param name="delay">The optional initial delay for the animation.</param>
        /// <param name="duration">The animation duration.</param>
        /// <param name="layer">The target framework layer to animate.</param>
        /// <returns>The current <see cref="AnimationBuilder"/> instance.</returns>
        public AnimationBuilder NormalizedKeyFrames<T>(
            string property,
            Action<INormalizedKeyFrameAnimationBuilder<T>> build,
            TimeSpan? delay = null,
            TimeSpan? duration = null,
            FrameworkLayer layer = FrameworkLayer.Composition)
            where T : unmanaged
        {
            if (layer == FrameworkLayer.Composition)
            {
                NormalizedKeyFrameAnimationBuilder<T>.Composition builder = new(property, delay, duration ?? DefaultDuration);

                build(builder);

                this.compositionAnimationFactories.Add(builder);
            }
            else
            {
                NormalizedKeyFrameAnimationBuilder<T>.Xaml builder = new(property, delay, duration ?? DefaultDuration);

                build(builder);

                this.xamlAnimationFactories.Add(builder);
            }

            return this;
        }
    }
}
