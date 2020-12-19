// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Numerics;
using Microsoft.Toolkit.Uwp.UI.Animations.Extensions;
using Windows.UI.Composition;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;

namespace Microsoft.Toolkit.Uwp.UI.Animations
{
    /// <inheritdoc cref="AnimationBuilder"/>
    public sealed partial class AnimationBuilder
    {
        /// <summary>
        /// A model representing a specified composition double animation for a target <see cref="CompositionObject"/>.
        /// </summary>
        private sealed record CompositionDoubleAnimation(
            CompositionObject Target,
            string Property,
            float? From,
            float To,
            TimeSpan? Delay,
            TimeSpan Duration,
            EasingType EasingType,
            EasingMode EasingMode)
            : ICompositionAnimation
        {
            /// <inheritdoc/>
            public void StartAnimation()
            {
                CompositionEasingFunction easingFunction = Target.Compositor.CreateCubicBezierEasingFunction(EasingType, EasingMode);
                ScalarKeyFrameAnimation animation = Target.Compositor.CreateScalarKeyFrameAnimation(Property, From, To, Duration, Delay, easingFunction);

                Target.StartAnimation(Property, animation);
            }
        }

        /// <summary>
        /// A model representing a specified composition scalar animation factory.
        /// </summary>
        private sealed record CompositionScalarAnimationFactory(
            string Property,
            float? From,
            float To,
            TimeSpan? Delay,
            TimeSpan Duration,
            EasingType EasingType,
            EasingMode EasingMode)
            : ICompositionAnimationFactory
        {
            /// <inheritdoc/>
            public CompositionAnimation GetAnimation(Visual visual)
            {
                CompositionEasingFunction easingFunction = visual.Compositor.CreateCubicBezierEasingFunction(EasingType, EasingMode);
                ScalarKeyFrameAnimation animation = visual.Compositor.CreateScalarKeyFrameAnimation(Property, From, To, Duration, Delay, easingFunction);

                return animation;
            }
        }

        /// <summary>
        /// A model representing a specified composition <see cref="Vector3"/> animation factory.
        /// </summary>
        private sealed record CompositionVector3AnimationFactory(
            string Property,
            Vector3? From,
            Vector3 To,
            TimeSpan? Delay,
            TimeSpan Duration,
            EasingType EasingType,
            EasingMode EasingMode)
            : ICompositionAnimationFactory
        {
            /// <inheritdoc/>
            public CompositionAnimation GetAnimation(Visual visual)
            {
                CompositionEasingFunction easingFunction = visual.Compositor.CreateCubicBezierEasingFunction(EasingType, EasingMode);
                Vector3KeyFrameAnimation animation = visual.Compositor.CreateVector3KeyFrameAnimation(Property, From, To, Duration, Delay, easingFunction);

                return animation;
            }
        }

        /// <summary>
        /// A model representing a specified composition scalar animation factory targeting a clip.
        /// </summary>
        private sealed record CompositionClipScalarAnimation(
            string Property,
            float? From,
            float To,
            TimeSpan? Delay,
            TimeSpan Duration,
            EasingType EasingType,
            EasingMode EasingMode)
            : ICompositionAnimationFactory
        {
            /// <inheritdoc/>
            public CompositionAnimation GetAnimation(Visual visual)
            {
                InsetClip clip = visual.Clip as InsetClip ?? (InsetClip)(visual.Clip = visual.Compositor.CreateInsetClip());
                CompositionEasingFunction easingFunction = clip.Compositor.CreateCubicBezierEasingFunction(EasingType, EasingMode);
                ScalarKeyFrameAnimation animation = visual.Compositor.CreateScalarKeyFrameAnimation(Property, From, To, Duration, Delay, easingFunction);

                return animation;
            }
        }

        /// <summary>
        /// A model representing a specified XAML <see cref="double"/> animation factory.
        /// </summary>
        private sealed record XamlDoubleAnimationFactory(
            string Property,
            double? From,
            double To,
            TimeSpan? Delay,
            TimeSpan Duration,
            EasingType EasingType,
            EasingMode EasingMode,
            bool EnableDependentAnimation)
            : IXamlAnimationFactory
        {
            /// <inheritdoc/>
            public Timeline GetAnimation(UIElement element)
            {
                return element.CreateDoubleAnimation(Property, From, To, Delay, Duration, EasingType.ToEasingFunction(EasingMode), EnableDependentAnimation);
            }
        }

        /// <summary>
        /// A model representing a specified XAML <see cref="double"/> animation factory targeting a transform.
        /// </summary>
        private sealed record XamlTransformDoubleAnimationFactory(
            string Property,
            double? From,
            double To,
            TimeSpan? Delay,
            TimeSpan Duration,
            EasingType EasingType,
            EasingMode EasingMode)
            : IXamlAnimationFactory
        {
            /// <inheritdoc/>
            public Timeline GetAnimation(UIElement element)
            {
                CompositeTransform transform = element.GetTransform<CompositeTransform>();

                return transform.CreateDoubleAnimation(Property, From, To, Delay, Duration, EasingType.ToEasingFunction(EasingMode));
            }
        }

        /// <summary>
        /// An interface for factories of XAML animations.
        /// </summary>
        internal interface IXamlAnimationFactory
        {
            /// <summary>
            /// Gets a <see cref="Timeline"/> instance representing the animation to start.
            /// </summary>
            /// <param name="element">The target <see cref="UIElement"/> instance to animate.</param>
            /// <returns>A <see cref="Timeline"/> instance with the specified animation.</returns>
            Timeline GetAnimation(UIElement element);
        }

        /// <summary>
        /// An interface for factories of composition animations.
        /// </summary>
        internal interface ICompositionAnimationFactory
        {
            /// <summary>
            /// Gets a <see cref="CompositionAnimation"/> instance representing the animation to start.
            /// </summary>
            /// <param name="visual">The target <see cref="Visual"/> instance to animate.</param>
            /// <returns>A <see cref="CompositionAnimation"/> instance with the specified animation.</returns>
            CompositionAnimation GetAnimation(Visual visual);
        }

        /// <summary>
        /// An interface for custom external composition animations.
        /// </summary>
        internal interface ICompositionAnimation
        {
            /// <summary>
            /// Starts a <see cref="CompositionAnimation"/> with some embedded parameters.
            /// </summary>
            void StartAnimation();
        }
    }
}
