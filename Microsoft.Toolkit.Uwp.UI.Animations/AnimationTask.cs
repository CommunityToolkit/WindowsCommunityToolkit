// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Threading.Tasks;

namespace Microsoft.Toolkit.Uwp.UI.Animations
{
    /// <summary>
    /// Defines <see cref="AnimationTask"/> which is used by
    /// <see cref="AnimationSet"/> to run animations that require
    /// asynchronous initialization
    /// </summary>
    internal class AnimationTask
    {
        /// <summary>
        /// Gets or sets <see cref="Task"/> that will run before any animation
        /// and it will add the animation to the AnimationSet once complete
        /// </summary>
        public Task Task { get; set; }

        /// <summary>
        /// Gets or sets <see cref="AnimationSet"/> that will run the animation
        /// </summary>
        public AnimationSet AnimationSet { get; set; }

        /// <summary>
        /// Gets or sets Duration to be applied to the animation once the task is completed
        /// Used when Duration is changed before Task completes
        /// </summary>
        public TimeSpan? Duration { get; set; }

        /// <summary>
        /// Gets or sets Delay to be applied to the animation once the task is completed
        /// Used when Duration is changed before Task completes
        /// </summary>
        public TimeSpan? Delay { get; set; }
    }
}
