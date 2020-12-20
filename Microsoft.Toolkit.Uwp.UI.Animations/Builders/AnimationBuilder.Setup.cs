// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Numerics;
using Windows.UI.Xaml.Media.Animation;

namespace Microsoft.Toolkit.Uwp.UI.Animations
{
    /// <summary>
    /// A <see langword="class"/> that allows to build custom animations targeting both the XAML and composition layers.
    /// </summary>
    public sealed partial class AnimationBuilder
    {
        /// <summary>
        /// The list of <see cref="ICompositionAnimation"/> instances representing animations to run.
        /// </summary>
        private readonly List<ICompositionAnimation> compositionAnimations = new();

        /// <summary>
        /// The list of <see cref="ICompositionAnimationFactory"/> instances representing factories for composition animations to run.
        /// </summary>
        private readonly List<ICompositionAnimationFactory> compositionAnimationFactories = new();

        /// <summary>
        /// The list of <see cref="IXamlAnimationFactory"/> instances representing factories for XAML animations to run.
        /// </summary>
        private readonly List<IXamlAnimationFactory> xamlAnimationFactories = new();

        /// <summary>
        /// Adds a new composition scalar animation to the current schedule.
        /// </summary>
        /// <param name="property">The target property to animate.</param>
        /// <param name="from">The optional starting value for the animation.</param>
        /// <param name="to">The final value for the animation.</param>
        /// <param name="delay">The optional initial delay for the animation.</param>
        /// <param name="duration">The animation duration.</param>
        /// <param name="easingType">The easing function for the animation.</param>
        /// <param name="easingMode">The easing mode for the animation.</param>
        /// <returns>The current <see cref="AnimationBuilder"/> instance.</returns>
        private AnimationBuilder AddCompositionScalarAnimationFactory(
            string property,
            float? from,
            float to,
            TimeSpan? delay,
            TimeSpan duration,
            EasingType easingType,
            EasingMode easingMode)
        {
            CompositionScalarAnimationFactory animation = new(property, from, to, delay, duration, easingType, easingMode);

            this.compositionAnimationFactories.Add(animation);

            return this;
        }

        /// <summary>
        /// Adds a new composition <see cref="Vector3"/> animation to the current schedule.
        /// </summary>
        /// <param name="property">The target property to animate.</param>
        /// <param name="from">The optional starting value for the animation.</param>
        /// <param name="to">The final value for the animation.</param>
        /// <param name="delay">The optional initial delay for the animation.</param>
        /// <param name="duration">The animation duration.</param>
        /// <param name="easingType">The easing function for the animation.</param>
        /// <param name="easingMode">The easing mode for the animation.</param>
        /// <returns>The current <see cref="AnimationBuilder"/> instance.</returns>
        private AnimationBuilder AddCompositionVector3AnimationFactory(
            string property,
            Vector3? from,
            Vector3 to,
            TimeSpan? delay,
            TimeSpan duration,
            EasingType easingType,
            EasingMode easingMode)
        {
            CompositionVector3AnimationFactory animation = new(property, from, to, delay, duration, easingType, easingMode);

            this.compositionAnimationFactories.Add(animation);

            return this;
        }

        /// <summary>
        /// Adds a new XAML <see cref="double"/> animation to the current schedule.
        /// </summary>
        /// <param name="property">The target property to animate.</param>
        /// <param name="from">The optional starting value for the animation.</param>
        /// <param name="to">The final value for the animation.</param>
        /// <param name="delay">The optional initial delay for the animation.</param>
        /// <param name="duration">The animation duration.</param>
        /// <param name="easingType">The easing function for the animation.</param>
        /// <param name="easingMode">The easing mode for the animation.</param>
        /// <param name="enableDependentAnimation">Whether to set <see cref="DoubleAnimation.EnableDependentAnimation"/>.</param>
        /// <returns>The current <see cref="AnimationBuilder"/> instance.</returns>
        private AnimationBuilder AddXamlDoubleAnimationFactory(
            string property,
            double? from,
            double to,
            TimeSpan? delay,
            TimeSpan duration,
            EasingType easingType,
            EasingMode easingMode,
            bool enableDependentAnimation)
        {
            XamlDoubleAnimationFactory animation = new(property, from, to, delay, duration, easingType, easingMode, enableDependentAnimation);

            this.xamlAnimationFactories.Add(animation);

            return this;
        }

        /// <summary>
        /// Adds a new XAML transform <see cref="double"/> animation to the current schedule.
        /// </summary>
        /// <param name="property">The target property to animate.</param>
        /// <param name="from">The optional starting value for the animation.</param>
        /// <param name="to">The final value for the animation.</param>
        /// <param name="delay">The optional initial delay for the animation.</param>
        /// <param name="duration">The animation duration.</param>
        /// <param name="easingType">The easing function for the animation.</param>
        /// <param name="easingMode">The easing mode for the animation.</param>
        /// <returns>The current <see cref="AnimationBuilder"/> instance.</returns>
        private AnimationBuilder AddXamlTransformDoubleAnimationFactory(
            string property,
            double? from,
            double to,
            TimeSpan? delay,
            TimeSpan duration,
            EasingType easingType,
            EasingMode easingMode)
        {
            XamlTransformDoubleAnimationFactory animation = new(property, from, to, delay, duration, easingType, easingMode);

            this.xamlAnimationFactories.Add(animation);

            return this;
        }
    }
}
