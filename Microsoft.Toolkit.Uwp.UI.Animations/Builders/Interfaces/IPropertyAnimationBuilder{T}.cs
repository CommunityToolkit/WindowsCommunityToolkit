// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;

namespace Microsoft.Toolkit.Uwp.UI.Animations
{
    /// <summary>
    /// An animation for an animation builder using keyframes, targeting a specific property.
    /// </summary>
    /// <typeparam name="T">The type of values being set by the animation being constructed.</typeparam>
    public interface IPropertyAnimationBuilder<in T>
    {
        /// <summary>
        /// Adds a custom animation based on normalized keyframes ot the current schedule.
        /// </summary>
        /// <param name="build">The callback to use to construct the custom animation.</param>
        /// <param name="delay">The optional initial delay for the animation.</param>
        /// <param name="duration">The animation duration.</param>
        /// <returns>The current <see cref="AnimationBuilder"/> instance.</returns>
        AnimationBuilder NormalizedKeyFrames(
            Action<INormalizedKeyFrameAnimationBuilder<T>> build,
            TimeSpan? delay = null,
            TimeSpan? duration = null);

        /// <summary>
        /// Adds a custom animation based on timed keyframes to the current schedule.
        /// </summary>
        /// <param name="build">The callback to use to construct the custom animation.</param>
        /// <param name="delay">The optional initial delay for the animation.</param>
        /// <returns>The current <see cref="AnimationBuilder"/> instance.</returns>
        AnimationBuilder TimedKeyFrames(
            Action<ITimedKeyFrameAnimationBuilder<T>> build,
            TimeSpan? delay = null);
    }
}
