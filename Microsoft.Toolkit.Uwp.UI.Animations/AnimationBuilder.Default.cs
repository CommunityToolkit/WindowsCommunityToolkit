// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

#nullable enable

using System;
using System.Numerics;
using Microsoft.Toolkit.Diagnostics;
using Windows.UI.Composition;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using static Microsoft.Toolkit.Uwp.UI.Animations.Extensions.AnimationExtensions;

namespace Microsoft.Toolkit.Uwp.UI.Animations
{
    /// <inheritdoc cref="AnimationBuilder"/>
    public sealed partial class AnimationBuilder
    {
        /// <summary>
        /// Adds a new opacity animation to the current schedule.
        /// </summary>
        /// <param name="from">The optional starting value for the animation.</param>
        /// <param name="to">The final value for the animation.</param>
        /// <param name="delay">The optional initial delay for the animation.</param>
        /// <param name="duration">The animation duration.</param>
        /// <param name="easingType">The optional easing function type for the animation.</param>
        /// <param name="easingMode">The optional easing function mode for the animation.</param>
        /// <param name="layer">The target framework layer to animate.</param>
        /// <returns>The current <see cref="AnimationBuilder"/> instance.</returns>
        public AnimationBuilder Opacity(
            double? from,
            double to,
            TimeSpan? delay,
            TimeSpan duration,
            EasingType easingType = DefaultEasingType,
            EasingMode easingMode = DefaultEasingMode,
            FrameworkLayer layer = FrameworkLayer.Composition)
        {
            if (layer == FrameworkLayer.Composition)
            {
                return AddCompositionScalarAnimationFactory(nameof(Visual.Opacity), (float?)from, (float)to, delay, duration, easingType, easingMode);
            }
            else
            {
                return AddXamlDoubleAnimationFactory(nameof(UIElement.Opacity), from, to, delay, duration, easingType, easingMode, false);
            }
        }

        /// <summary>
        /// Adds a new translation animation for a single axis to the current schedule.
        /// </summary>
        /// <param name="axis">The target translation axis to animate.</param>
        /// <param name="from">The optional starting value for the animation.</param>
        /// <param name="to">The final value for the animation.</param>
        /// <param name="delay">The optional initial delay for the animation.</param>
        /// <param name="duration">The animation duration.</param>
        /// <param name="easingType">The optional easing function type for the animation.</param>
        /// <param name="easingMode">The optional easing function mode for the animation.</param>
        /// <param name="layer">The target framework layer to animate.</param>
        /// <returns>The current <see cref="AnimationBuilder"/> instance.</returns>
        public AnimationBuilder Translation(
            Axis axis,
            double? from,
            double to,
            TimeSpan? delay,
            TimeSpan duration,
            EasingType easingType = DefaultEasingType,
            EasingMode easingMode = DefaultEasingMode,
            FrameworkLayer layer = FrameworkLayer.Composition)
        {
            if (layer == FrameworkLayer.Composition)
            {
                return AddCompositionScalarAnimationFactory($"Translation.{axis}", (float?)from, (float)to, delay, duration, easingType, easingMode);
            }
            else
            {
                return AddXamlDoubleAnimationFactory($"Translate{axis}", from, to, delay, duration, easingType, easingMode, false);
            }
        }

        /// <summary>
        /// Adds a new translation animation for the X and Y axes to the current schedule.
        /// </summary>
        /// <param name="from">The optional starting point for the animation.</param>
        /// <param name="to">The final point for the animation.</param>
        /// <param name="delay">The optional initial delay for the animation.</param>
        /// <param name="duration">The animation duration.</param>
        /// <param name="easingType">The optional easing function type for the animation.</param>
        /// <param name="easingMode">The optional easing function mode for the animation.</param>
        /// <param name="layer">The target framework layer to animate.</param>
        /// <returns>The current <see cref="AnimationBuilder"/> instance.</returns>
        public AnimationBuilder Translation(
            Vector2? from,
            Vector2 to,
            TimeSpan? delay,
            TimeSpan duration,
            EasingType easingType = DefaultEasingType,
            EasingMode easingMode = DefaultEasingMode,
            FrameworkLayer layer = FrameworkLayer.Composition)
        {
            if (layer == FrameworkLayer.Composition)
            {
                return AddCompositionVector2AnimationFactory("Translation", from, to, delay, duration, easingType, easingMode);
            }
            else
            {
                AddXamlTransformDoubleAnimationFactory(nameof(CompositeTransform.TranslateX), from?.X, to.X, delay, duration, easingType, easingMode);
                AddXamlTransformDoubleAnimationFactory(nameof(CompositeTransform.TranslateY), from?.Y, to.Y, delay, duration, easingType, easingMode);

                return this;
            }
        }

        /// <summary>
        /// Adds a new composition translation animation for all axes to the current schedule.
        /// </summary>
        /// <param name="from">The optional starting point for the animation.</param>
        /// <param name="to">The final point for the animation.</param>
        /// <param name="delay">The optional initial delay for the animation.</param>
        /// <param name="duration">The animation duration.</param>
        /// <param name="easingType">The optional easing function type for the animation.</param>
        /// <param name="easingMode">The optional easing function mode for the animation.</param>
        /// <returns>The current <see cref="AnimationBuilder"/> instance.</returns>
        /// <remarks>This animation is only available on the composition layer.</remarks>
        public AnimationBuilder Translation(
            Vector3? from,
            Vector3 to,
            TimeSpan? delay,
            TimeSpan duration,
            EasingType easingType = DefaultEasingType,
            EasingMode easingMode = DefaultEasingMode)
        {
            return AddCompositionVector3AnimationFactory("Translation", from, to, delay, duration, easingType, easingMode);
        }

        /// <summary>
        /// Adds a new composition offset animation for a single axis to the current schedule.
        /// </summary>
        /// <param name="axis">The target translation axis to animate.</param>
        /// <param name="from">The optional starting value for the animation.</param>
        /// <param name="to">The final value for the animation.</param>
        /// <param name="delay">The optional initial delay for the animation.</param>
        /// <param name="duration">The animation duration.</param>
        /// <param name="easingType">The optional easing function type for the animation.</param>
        /// <param name="easingMode">The optional easing function mode for the animation.</param>
        /// <returns>The current <see cref="AnimationBuilder"/> instance.</returns>
        /// <remarks>This animation is only available on the composition layer.</remarks>
        public AnimationBuilder Offset(
            Axis axis,
            double? from,
            double to,
            TimeSpan? delay,
            TimeSpan duration,
            EasingType easingType = DefaultEasingType,
            EasingMode easingMode = DefaultEasingMode)
        {
            return AddCompositionScalarAnimationFactory($"{nameof(Visual.Offset)}.{axis}", (float?)from, (float)to, delay, duration, easingType, easingMode);
        }

        /// <summary>
        /// Adds a new composition offset animation for the X and Y axes to the current schedule.
        /// </summary>
        /// <param name="from">The optional starting point for the animation.</param>
        /// <param name="to">The final point for the animation.</param>
        /// <param name="delay">The optional initial delay for the animation.</param>
        /// <param name="duration">The animation duration.</param>
        /// <param name="easingType">The optional easing function type for the animation.</param>
        /// <param name="easingMode">The optional easing function mode for the animation.</param>
        /// <returns>The current <see cref="AnimationBuilder"/> instance.</returns>
        /// <remarks>This animation is only available on the composition layer.</remarks>
        public AnimationBuilder Offset(
            Vector2? from,
            Vector2 to,
            TimeSpan? delay,
            TimeSpan duration,
            EasingType easingType = DefaultEasingType,
            EasingMode easingMode = DefaultEasingMode)
        {
            return AddCompositionVector2AnimationFactory(nameof(Visual.Offset), from, to, delay, duration, easingType, easingMode);
        }

        /// <summary>
        /// Adds a new composition offset translation animation for all axes to the current schedule.
        /// </summary>
        /// <param name="from">The optional starting point for the animation.</param>
        /// <param name="to">The final point for the animation.</param>
        /// <param name="delay">The optional initial delay for the animation.</param>
        /// <param name="duration">The animation duration.</param>
        /// <param name="easingType">The optional easing function type for the animation.</param>
        /// <param name="easingMode">The optional easing function mode for the animation.</param>
        /// <returns>The current <see cref="AnimationBuilder"/> instance.</returns>
        /// <remarks>This animation is only available on the composition layer.</remarks>
        public AnimationBuilder Offset(
            Vector3? from,
            Vector3 to,
            TimeSpan? delay,
            TimeSpan duration,
            EasingType easingType = DefaultEasingType,
            EasingMode easingMode = DefaultEasingMode)
        {
            return AddCompositionVector3AnimationFactory(nameof(Visual.Offset), from, to, delay, duration, easingType, easingMode);
        }

        /// <summary>
        /// Adds a new uniform scale animation on the X and Y axes to the current schedule.
        /// </summary>
        /// <param name="from">The optional starting value for the animation.</param>
        /// <param name="to">The final value for the animation.</param>
        /// <param name="delay">The optional initial delay for the animation.</param>
        /// <param name="duration">The animation duration.</param>
        /// <param name="easingType">The optional easing function type for the animation.</param>
        /// <param name="easingMode">The optional easing function mode for the animation.</param>
        /// <param name="layer">The target framework layer to animate.</param>
        /// <returns>The current <see cref="AnimationBuilder"/> instance.</returns>
        public AnimationBuilder Scale(
            double? from,
            double to,
            TimeSpan? delay,
            TimeSpan duration,
            EasingType easingType = DefaultEasingType,
            EasingMode easingMode = DefaultEasingMode,
            FrameworkLayer layer = FrameworkLayer.Composition)
        {
            if (layer == FrameworkLayer.Composition)
            {
                Vector2? from2 = from is null ? null : new((float)(double)from);
                Vector2 to2 = new((float)to);

                return AddCompositionVector2AnimationFactory(nameof(Visual.Scale), from2, to2, delay, duration, easingType, easingMode);
            }
            else
            {
                AddXamlTransformDoubleAnimationFactory(nameof(CompositeTransform.ScaleX), from, to, delay, duration, easingType, easingMode);
                AddXamlTransformDoubleAnimationFactory(nameof(CompositeTransform.ScaleY), from, to, delay, duration, easingType, easingMode);

                return this;
            }
        }

        /// <summary>
        /// Adds a new scale animation on a specified axis to the current schedule.
        /// </summary>
        /// <param name="axis">The target scale axis to animate.</param>
        /// <param name="from">The optional starting value for the animation.</param>
        /// <param name="to">The final value for the animation.</param>
        /// <param name="delay">The optional initial delay for the animation.</param>
        /// <param name="duration">The animation duration.</param>
        /// <param name="easingType">The optional easing function type for the animation.</param>
        /// <param name="easingMode">The optional easing function mode for the animation.</param>
        /// <returns>The current <see cref="AnimationBuilder"/> instance.</returns>
        /// <remarks>This animation is only available on the XAML layer.</remarks>
        public AnimationBuilder Scale(
            Axis axis,
            double? from,
            double to,
            TimeSpan? delay,
            TimeSpan duration,
            EasingType easingType = DefaultEasingType,
            EasingMode easingMode = DefaultEasingMode)
        {
            return AddXamlTransformDoubleAnimationFactory($"Scale{axis}", from, to, delay, duration, easingType, easingMode);
        }

        /// <summary>
        /// Adds a new scale animation for the X and Y axes to the current schedule.
        /// </summary>
        /// <param name="from">The optional starting point for the animation.</param>
        /// <param name="to">The final point for the animation.</param>
        /// <param name="delay">The optional initial delay for the animation.</param>
        /// <param name="duration">The animation duration.</param>
        /// <param name="easingType">The optional easing function type for the animation.</param>
        /// <param name="easingMode">The optional easing function mode for the animation.</param>
        /// <param name="layer">The target framework layer to animate.</param>
        /// <returns>The current <see cref="AnimationBuilder"/> instance.</returns>
        public AnimationBuilder Scale(
            Vector2? from,
            Vector2 to,
            TimeSpan? delay,
            TimeSpan duration,
            EasingType easingType = DefaultEasingType,
            EasingMode easingMode = DefaultEasingMode,
            FrameworkLayer layer = FrameworkLayer.Composition)
        {
            if (layer == FrameworkLayer.Composition)
            {
                return AddCompositionVector2AnimationFactory(nameof(Visual.Scale), from, to, delay, duration, easingType, easingMode);
            }
            else
            {
                AddXamlTransformDoubleAnimationFactory(nameof(CompositeTransform.ScaleX), from?.X, to.X, delay, duration, easingType, easingMode);
                AddXamlTransformDoubleAnimationFactory(nameof(CompositeTransform.ScaleY), from?.Y, to.Y, delay, duration, easingType, easingMode);

                return this;
            }
        }

        /// <summary>
        /// Adds a new scale animation for all axes to the current schedule.
        /// </summary>
        /// <param name="from">The optional starting point for the animation.</param>
        /// <param name="to">The final point for the animation.</param>
        /// <param name="delay">The optional initial delay for the animation.</param>
        /// <param name="duration">The animation duration.</param>
        /// <param name="easingType">The optional easing function type for the animation.</param>
        /// <param name="easingMode">The optional easing function mode for the animation.</param>
        /// <returns>The current <see cref="AnimationBuilder"/> instance.</returns>
        /// <remarks>This animation is only available on the composition layer.</remarks>
        public AnimationBuilder Scale(
            Vector3? from,
            Vector3 to,
            TimeSpan? delay,
            TimeSpan duration,
            EasingType easingType = DefaultEasingType,
            EasingMode easingMode = DefaultEasingMode)
        {
            return AddCompositionVector3AnimationFactory(nameof(Visual.Scale), from, to, delay, duration, easingType, easingMode);
        }

        /// <summary>
        /// Adds a new rotation animation to the current schedule.
        /// </summary>
        /// <param name="from">The optional starting value for the animation.</param>
        /// <param name="to">The final value for the animation.</param>
        /// <param name="delay">The optional initial delay for the animation.</param>
        /// <param name="duration">The animation duration.</param>
        /// <param name="easingType">The optional easing function type for the animation.</param>
        /// <param name="easingMode">The optional easing function mode for the animation.</param>
        /// <param name="layer">The target framework layer to animate.</param>
        /// <returns>The current <see cref="AnimationBuilder"/> instance.</returns>
        public AnimationBuilder Rotate(
            double? from,
            double to,
            TimeSpan? delay,
            TimeSpan duration,
            EasingType easingType = DefaultEasingType,
            EasingMode easingMode = DefaultEasingMode,
            FrameworkLayer layer = FrameworkLayer.Composition)
        {
            if (layer == FrameworkLayer.Composition)
            {
                return AddCompositionScalarAnimationFactory(nameof(Visual.RotationAngle), (float?)from, (float)to, delay, duration, easingType, easingMode);
            }
            else
            {
                double? fromDegrees = from * Math.PI / 180;
                double toDegrees = to * Math.PI / 180;

                AddXamlTransformDoubleAnimationFactory(nameof(CompositeTransform.Rotation), fromDegrees, toDegrees, delay, duration, easingType, easingMode);

                return this;
            }
        }

        /// <summary>
        /// Adds a new rotation animation in degrees to the current schedule.
        /// </summary>
        /// <param name="from">The optional starting value for the animation.</param>
        /// <param name="to">The final value for the animation.</param>
        /// <param name="delay">The optional initial delay for the animation.</param>
        /// <param name="duration">The animation duration.</param>
        /// <param name="easingType">The optional easing function type for the animation.</param>
        /// <param name="easingMode">The optional easing function mode for the animation.</param>
        /// <param name="layer">The target framework layer to animate.</param>
        /// <returns>The current <see cref="AnimationBuilder"/> instance.</returns>
        public AnimationBuilder RotateInDegrees(
            double? from,
            double to,
            TimeSpan? delay,
            TimeSpan duration,
            EasingType easingType = DefaultEasingType,
            EasingMode easingMode = DefaultEasingMode,
            FrameworkLayer layer = FrameworkLayer.Composition)
        {
            if (layer == FrameworkLayer.Composition)
            {
                return AddCompositionScalarAnimationFactory(nameof(Visual.RotationAngleInDegrees), (float?)from, (float)to, delay, duration, easingType, easingMode);
            }
            else
            {
                AddXamlTransformDoubleAnimationFactory(nameof(CompositeTransform.Rotation), from, to, delay, duration, easingType, easingMode);

                return this;
            }
        }

        /// <summary>
        /// Adds a new clip animation to the current schedule.
        /// </summary>
        /// <param name="side">The clip size to animate.</param>
        /// <param name="from">The optional starting value for the animation.</param>
        /// <param name="to">The final value for the animation.</param>
        /// <param name="delay">The optional initial delay for the animation.</param>
        /// <param name="duration">The animation duration.</param>
        /// <param name="easingType">The optional easing function type for the animation.</param>
        /// <param name="easingMode">The optional easing function mode for the animation.</param>
        /// <returns>The current <see cref="AnimationBuilder"/> instance.</returns>
        /// <remarks>This animation is only available on the composition layer.</remarks>
        public AnimationBuilder Clip(
            Side side,
            double? from,
            double to,
            TimeSpan? delay,
            TimeSpan duration,
            EasingType easingType = DefaultEasingType,
            EasingMode easingMode = DefaultEasingMode)
        {
            string property = side switch
            {
                Side.Top => nameof(InsetClip.TopInset),
                Side.Bottom => nameof(InsetClip.BottomInset),
                Side.Right => nameof(InsetClip.RightInset),
                Side.Left => nameof(InsetClip.LeftInset),
                _ => ThrowHelper.ThrowArgumentException<string>("Invalid clip size")
            };

            CompositionClipScalarAnimation animation = new(
                property,
                (float?)from,
                (float)to,
                delay,
                duration,
                easingType,
                easingMode);

            this.compositionAnimationFactories.Add(animation);

            return this;
        }

        /// <summary>
        /// Adds a new size animation for a single axis to the current schedule.
        /// </summary>
        /// <param name="axis">The target size axis to animate.</param>
        /// <param name="from">The optional starting value for the animation.</param>
        /// <param name="to">The final value for the animation.</param>
        /// <param name="delay">The optional initial delay for the animation.</param>
        /// <param name="duration">The animation duration.</param>
        /// <param name="easingType">The optional easing function type for the animation.</param>
        /// <param name="easingMode">The optional easing function mode for the animation.</param>
        /// <param name="layer">The target framework layer to animate.</param>
        /// <returns>The current <see cref="AnimationBuilder"/> instance.</returns>
        public AnimationBuilder Size(
            Axis axis,
            double? from,
            double to,
            TimeSpan? delay,
            TimeSpan duration,
            EasingType easingType = DefaultEasingType,
            EasingMode easingMode = DefaultEasingMode,
            FrameworkLayer layer = FrameworkLayer.Composition)
        {
            if (layer == FrameworkLayer.Composition)
            {
                return AddCompositionScalarAnimationFactory($"{nameof(Visual.Size)}.{axis}", (float?)from, (float)to, delay, duration, easingType, easingMode);
            }
            else
            {
                string property = axis switch
                {
                    Axis.X => nameof(FrameworkElement.Width),
                    Axis.Y => nameof(FrameworkElement.Height),
                    _ => ThrowHelper.ThrowArgumentException<string>("Invalid size axis")
                };

                return AddXamlDoubleAnimationFactory(property, from, to, delay, duration, easingType, easingMode, true);
            }
        }

        /// <summary>
        /// Adds a new size animation for the X and Y axes to the current schedule.
        /// </summary>
        /// <param name="from">The optional starting point for the animation.</param>
        /// <param name="to">The final point for the animation.</param>
        /// <param name="delay">The optional initial delay for the animation.</param>
        /// <param name="duration">The animation duration.</param>
        /// <param name="easingType">The optional easing function type for the animation.</param>
        /// <param name="easingMode">The optional easing function mode for the animation.</param>
        /// <param name="layer">The target framework layer to animate.</param>
        /// <returns>The current <see cref="AnimationBuilder"/> instance.</returns>
        public AnimationBuilder Size(
            Vector2? from,
            Vector2 to,
            TimeSpan? delay,
            TimeSpan duration,
            EasingType easingType = DefaultEasingType,
            EasingMode easingMode = DefaultEasingMode,
            FrameworkLayer layer = FrameworkLayer.Composition)
        {
            if (layer == FrameworkLayer.Composition)
            {
                return AddCompositionVector2AnimationFactory(nameof(Visual.Size), from, to, delay, duration, easingType, easingMode);
            }
            else
            {
                AddXamlDoubleAnimationFactory(nameof(FrameworkElement.Width), from?.X, to.X, delay, duration, easingType, easingMode, true);
                AddXamlDoubleAnimationFactory(nameof(FrameworkElement.Height), from?.Y, to.Y, delay, duration, easingType, easingMode, true);

                return this;
            }
        }

        /// <summary>
        /// Adds a new composition size translation animation for all axes to the current schedule.
        /// </summary>
        /// <param name="from">The optional starting point for the animation.</param>
        /// <param name="to">The final point for the animation.</param>
        /// <param name="delay">The optional initial delay for the animation.</param>
        /// <param name="duration">The animation duration.</param>
        /// <param name="easingType">The optional easing function type for the animation.</param>
        /// <param name="easingMode">The optional easing function mode for the animation.</param>
        /// <returns>The current <see cref="AnimationBuilder"/> instance.</returns>
        /// <remarks>This animation is only available on the composition layer.</remarks>
        public AnimationBuilder Size(
            Vector3? from,
            Vector3 to,
            TimeSpan? delay,
            TimeSpan duration,
            EasingType easingType = DefaultEasingType,
            EasingMode easingMode = DefaultEasingMode)
        {
            return AddCompositionVector3AnimationFactory(nameof(Visual.Size), from, to, delay, duration, easingType, easingMode);
        }
    }
}
