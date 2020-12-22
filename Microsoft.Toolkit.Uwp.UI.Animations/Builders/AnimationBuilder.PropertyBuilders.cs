// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Windows.UI.Composition;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using static Microsoft.Toolkit.Uwp.UI.Animations.Extensions.AnimationExtensions;

#nullable enable

namespace Microsoft.Toolkit.Uwp.UI.Animations
{
    /// <inheritdoc cref="AnimationBuilder"/>
    public sealed partial class AnimationBuilder
    {
        /// <summary>
        /// An animation for an animation builder using keyframes, targeting a specific property.
        /// </summary>
        /// <typeparam name="T">The type of values being set by the animation being constructed.</typeparam>
        public interface IPropertyAnimationBuilder<in T>
            where T : unmanaged
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

        /// <summary>
        /// A custom <see cref="IPropertyAnimationBuilder{T}"/> for a shared animation.
        /// </summary>
        /// <typeparam name="T">The type of property to animate.</typeparam>
        private sealed record PropertyAnimationBuilder<T>(
            AnimationBuilder Builder,
            string Property,
            FrameworkLayer Layer)
            : IPropertyAnimationBuilder<T>
            where T : unmanaged
        {
            /// <inheritdoc/>
            public AnimationBuilder NormalizedKeyFrames(
                Action<INormalizedKeyFrameAnimationBuilder<T>> build,
                TimeSpan? delay = null,
                TimeSpan? duration = null)
            {
                return Builder.NormalizedKeyFrames(Property, build, delay, duration, Layer);
            }

            /// <inheritdoc/>
            public AnimationBuilder TimedKeyFrames(
                Action<ITimedKeyFrameAnimationBuilder<T>> build,
                TimeSpan? delay = null)
            {
                return Builder.TimedKeyFrames(Property, build, delay, Layer);
            }
        }

        /// <summary>
        /// A custom <see cref="IPropertyAnimationBuilder{T}"/> for a composition clip animation.
        /// </summary>
        private sealed record CompositionClipAnimationBuilder(
            AnimationBuilder Builder,
            string Property)
            : IPropertyAnimationBuilder<double>
        {
            /// <inheritdoc/>
            public AnimationBuilder NormalizedKeyFrames(
                Action<INormalizedKeyFrameAnimationBuilder<double>> build,
                TimeSpan? delay = null,
                TimeSpan? duration = null)
            {
                NormalizedKeyFrameAnimationBuilder<double>.Composition builder = new(Property, delay, duration ?? DefaultDuration);

                build(builder);

                Builder.compositionAnimationFactories.Add(new Factory(builder));

                return Builder;
            }

            /// <inheritdoc/>
            public AnimationBuilder TimedKeyFrames(
                Action<ITimedKeyFrameAnimationBuilder<double>> build,
                TimeSpan? delay = null)
            {
                TimedKeyFrameAnimationBuilder<double>.Composition builder = new(Property, delay);

                build(builder);

                Builder.compositionAnimationFactories.Add(new Factory(builder));

                return Builder;
            }

            /// <summary>
            /// A private factory implementing <see cref="ICompositionAnimationFactory"/>.
            /// </summary>
            private sealed record Factory(ICompositionAnimationFactory Builder) : ICompositionAnimationFactory
            {
                /// <inheritdoc/>
                public CompositionAnimation GetAnimation(CompositionObject targetHint, out CompositionObject? target)
                {
                    Visual visual = (Visual)targetHint;
                    InsetClip clip = visual.Clip as InsetClip ?? (InsetClip)(visual.Clip = visual.Compositor.CreateInsetClip());
                    CompositionAnimation animation = Builder.GetAnimation(clip, out _);

                    target = clip;

                    return animation;
                }
            }
        }

        /// <summary>
        /// A custom <see cref="IPropertyAnimationBuilder{T}"/> for a XAML transform animation.
        /// </summary>
        private sealed record XamlTransformPropertyAnimationBuilder(
            AnimationBuilder Builder,
            string Property)
            : IPropertyAnimationBuilder<double>
        {
            /// <inheritdoc/>
            public AnimationBuilder NormalizedKeyFrames(
                Action<INormalizedKeyFrameAnimationBuilder<double>> build,
                TimeSpan? delay = null,
                TimeSpan? duration = null)
            {
                NormalizedKeyFrameAnimationBuilder<double>.Xaml builder = new(Property, delay, duration ?? DefaultDuration);

                build(builder);

                Builder.xamlAnimationFactories.Add(new Factory(builder));

                return Builder;
            }

            /// <inheritdoc/>
            public AnimationBuilder TimedKeyFrames(
                Action<ITimedKeyFrameAnimationBuilder<double>> build,
                TimeSpan? delay = null)
            {
                TimedKeyFrameAnimationBuilder<double>.Xaml builder = new(Property, delay);

                build(builder);

                Builder.xamlAnimationFactories.Add(new Factory(builder));

                return Builder;
            }

            /// <summary>
            /// A private factory implementing <see cref="IXamlAnimationFactory"/>.
            /// </summary>
            private sealed record Factory(IXamlAnimationFactory Builder) : IXamlAnimationFactory
            {
                /// <inheritdoc/>
                public Timeline GetAnimation(DependencyObject targetHint)
                {
                    UIElement element = (UIElement)targetHint;

                    if (element.RenderTransform is not CompositeTransform transform)
                    {
                        element.RenderTransform = transform = new CompositeTransform();
                    }

                    return Builder.GetAnimation(transform);
                }
            }
        }
    }
}
