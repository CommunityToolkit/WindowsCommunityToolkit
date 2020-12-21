// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media.Animation;

namespace Microsoft.Toolkit.Uwp.UI.Animations
{
    /// <inheritdoc cref="NormalizedKeyFrameAnimationBuilder{T}"/>
    internal abstract partial class NormalizedKeyFrameAnimationBuilder<T> : INormalizedKeyFrameAnimationBuilder<T>
        where T : unmanaged
    {
        /// <summary>
        /// A custom <see cref="NormalizedKeyFrameAnimationBuilder{T}"/> class targeting the XAML layer.
        /// </summary>
        public sealed class Xaml : NormalizedKeyFrameAnimationBuilder<T>, AnimationBuilder.IXamlAnimationFactory
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="Xaml"/> class.
            /// </summary>
            /// <inheritdoc cref="NormalizedKeyFrameAnimationBuilder{T}"/>
            public Xaml(string property, TimeSpan? delay, TimeSpan duration)
                : base(property, delay, duration)
            {
            }

            /// <inheritdoc/>
            public Timeline GetAnimation(DependencyObject targetHint)
            {
                return TimedKeyFrameAnimationBuilder<T>.GetAnimation(
                    targetHint,
                    this.property,
                    this.delay,
                    this.duration,
                    this.keyFrames);
            }
        }
    }
}
