﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

#nullable enable

using System;
using System.Numerics;
using Microsoft.Toolkit.Diagnostics;
using Windows.UI.Composition;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using static Microsoft.Toolkit.Uwp.UI.Animations.Extensions.AnimationExtensions;

namespace Microsoft.Toolkit.Uwp.UI.Animations
{
    /// <inheritdoc cref="AnimationBuilder"/>
    public sealed partial class AnimationBuilder
    {
        /// <summary>
        /// Adds a new anchor point animation for a single axis to the current schedule.
        /// </summary>
        /// <param name="axis">The target anchor point axis to animate.</param>
        /// <returns>An <see cref="IPropertyAnimationBuilder{T}"/> instance to configure the animation.</returns>
        /// <remarks>This animation is only available on the composition layer.</remarks>
        public IPropertyAnimationBuilder<double> AnchorPoint(Axis axis)
        {
            return new PropertyAnimationBuilder<double>(this, $"{nameof(Visual.AnchorPoint)}.{axis}", FrameworkLayer.Composition);
        }

        /// <summary>
        /// Adds a new anchor point animation for the X and Y axes to the current schedule.
        /// </summary>
        /// <returns>An <see cref="IPropertyAnimationBuilder{T}"/> instance to configure the animation.</returns>
        /// <remarks>This animation is only available on the composition layer.</remarks>
        public IPropertyAnimationBuilder<Vector2> AnchorPoint()
        {
            return new PropertyAnimationBuilder<Vector2>(this, nameof(Visual.AnchorPoint), FrameworkLayer.Composition);
        }

        /// <summary>
        /// Adds a new opacity animation to the current schedule.
        /// </summary>
        /// <param name="layer">The target framework layer to animate.</param>
        /// <returns>An <see cref="IPropertyAnimationBuilder{T}"/> instance to configure the animation.</returns>
        public IPropertyAnimationBuilder<double> Opacity(FrameworkLayer layer = FrameworkLayer.Composition)
        {
            return new PropertyAnimationBuilder<double>(this, nameof(Visual.Opacity), layer);
        }

        /// <summary>
        /// Adds a new translation animation for a single axis to the current schedule.
        /// </summary>
        /// <param name="axis">The target translation axis to animate.</param>
        /// <param name="layer">The target framework layer to animate.</param>
        /// <returns>An <see cref="IPropertyAnimationBuilder{T}"/> instance to configure the animation.</returns>
        public IPropertyAnimationBuilder<double> Translation(Axis axis, FrameworkLayer layer = FrameworkLayer.Composition)
        {
            if (layer == FrameworkLayer.Composition)
            {
                return new PropertyAnimationBuilder<double>(this, $"Translation.{axis}", layer);
            }

            return new XamlTransformPropertyAnimationBuilder(this, $"Translate{axis}");
        }

        /// <summary>
        /// Adds a new composition translation animation for all axes to the current schedule.
        /// </summary>
        /// <returns>An <see cref="IPropertyAnimationBuilder{T}"/> instance to configure the animation.</returns>
        /// <remarks>This animation is only available on the composition layer.</remarks>
        public IPropertyAnimationBuilder<Vector3> Translation()
        {
            return new PropertyAnimationBuilder<Vector3>(this, "Translation", FrameworkLayer.Composition);
        }

        /// <summary>
        /// Adds a new composition offset animation for a single axis to the current schedule.
        /// </summary>
        /// <param name="axis">The target translation axis to animate.</param>
        /// <returns>An <see cref="IPropertyAnimationBuilder{T}"/> instance to configure the animation.</returns>
        /// <remarks>This animation is only available on the composition layer.</remarks>
        public IPropertyAnimationBuilder<double> Offset(Axis axis)
        {
            return new PropertyAnimationBuilder<double>(this, $"{nameof(Visual.Offset)}.{axis}", FrameworkLayer.Composition);
        }

        /// <summary>
        /// Adds a new composition offset translation animation for all axes to the current schedule.
        /// </summary>
        /// <returns>An <see cref="IPropertyAnimationBuilder{T}"/> instance to configure the animation.</returns>
        /// <remarks>This animation is only available on the composition layer.</remarks>
        public IPropertyAnimationBuilder<Vector3> Offset()
        {
            return new PropertyAnimationBuilder<Vector3>(this, nameof(Visual.Offset), FrameworkLayer.Composition);
        }

        /// <summary>
        /// Adds a new scale animation on a specified axis to the current schedule.
        /// </summary>
        /// <param name="axis">The target scale axis to animate.</param>
        /// <param name="layer">The target framework layer to animate.</param>
        /// <returns>An <see cref="IPropertyAnimationBuilder{T}"/> instance to configure the animation.</returns>
        public IPropertyAnimationBuilder<double> Scale(Axis axis, FrameworkLayer layer = FrameworkLayer.Composition)
        {
            if (layer == FrameworkLayer.Composition)
            {
                return new PropertyAnimationBuilder<double>(this, $"{nameof(Visual.Scale)}.{axis}", layer);
            }

            return new XamlTransformPropertyAnimationBuilder(this, $"Scale{axis}");
        }

        /// <summary>
        /// Adds a new scale animation for all axes to the current schedule.
        /// </summary>
        /// <returns>An <see cref="IPropertyAnimationBuilder{T}"/> instance to configure the animation.</returns>
        /// <remarks>This animation is only available on the composition layer.</remarks>
        public IPropertyAnimationBuilder<Vector3> Scale()
        {
            return new PropertyAnimationBuilder<Vector3>(this, nameof(Visual.Scale), FrameworkLayer.Composition);
        }

        /// <summary>
        /// Adds a new center point animation on a specified axis to the current schedule.
        /// </summary>
        /// <param name="axis">The target scale axis to animate.</param>
        /// <param name="layer">The target framework layer to animate.</param>
        /// <returns>An <see cref="IPropertyAnimationBuilder{T}"/> instance to configure the animation.</returns>
        public IPropertyAnimationBuilder<double> CenterPoint(Axis axis, FrameworkLayer layer = FrameworkLayer.Composition)
        {
            if (layer == FrameworkLayer.Composition)
            {
                return new PropertyAnimationBuilder<double>(this, $"{nameof(Visual.CenterPoint)}.{axis}", layer);
            }

            return new XamlTransformPropertyAnimationBuilder(this, $"Center{axis}");
        }

        /// <summary>
        /// Adds a new center point animation for all axes to the current schedule.
        /// </summary>
        /// <returns>An <see cref="IPropertyAnimationBuilder{T}"/> instance to configure the animation.</returns>
        /// <remarks>This animation is only available on the composition layer.</remarks>
        public IPropertyAnimationBuilder<Vector3> CenterPoint()
        {
            return new PropertyAnimationBuilder<Vector3>(this, nameof(Visual.CenterPoint), FrameworkLayer.Composition);
        }

        /// <summary>
        /// Adds a new rotation animation to the current schedule.
        /// </summary>
        /// <returns>An <see cref="IPropertyAnimationBuilder{T}"/> instance to configure the animation.</returns>
        /// <remarks>This animation is only available on the composition layer.</remarks>
        public IPropertyAnimationBuilder<double> Rotation()
        {
            return new PropertyAnimationBuilder<double>(this, nameof(Visual.RotationAngle), FrameworkLayer.Composition);
        }

        /// <summary>
        /// Adds a new rotation animation in degrees to the current schedule.
        /// </summary>
        /// <param name="layer">The target framework layer to animate.</param>
        /// <returns>An <see cref="IPropertyAnimationBuilder{T}"/> instance to configure the animation.</returns>
        public IPropertyAnimationBuilder<double> RotationInDegrees(FrameworkLayer layer = FrameworkLayer.Composition)
        {
            if (layer == FrameworkLayer.Composition)
            {
                return new PropertyAnimationBuilder<double>(this, nameof(Visual.RotationAngleInDegrees), layer);
            }

            return new XamlTransformPropertyAnimationBuilder(this, nameof(CompositeTransform.Rotation));
        }

        /// <summary>
        /// Adds a new rotation axis animation to the current schedule.
        /// </summary>
        /// <returns>An <see cref="IPropertyAnimationBuilder{T}"/> instance to configure the animation.</returns>
        /// <remarks>This animation is only available on the composition layer.</remarks>
        public IPropertyAnimationBuilder<Vector3> RotationAxis()
        {
            return new PropertyAnimationBuilder<Vector3>(this, nameof(Visual.RotationAxis), FrameworkLayer.Composition);
        }

        /// <summary>
        /// Adds a new orientation animation to the current schedule.
        /// </summary>
        /// <returns>An <see cref="IPropertyAnimationBuilder{T}"/> instance to configure the animation.</returns>
        /// <remarks>This animation is only available on the composition layer.</remarks>
        public IPropertyAnimationBuilder<Quaternion> Orientation()
        {
            return new PropertyAnimationBuilder<Quaternion>(this, nameof(Visual.Orientation), FrameworkLayer.Composition);
        }

        /// <summary>
        /// Adds a new clip animation to the current schedule.
        /// </summary>
        /// <param name="side">The clip size to animate.</param>
        /// <returns>An <see cref="IPropertyAnimationBuilder{T}"/> instance to configure the animation.</returns>
        /// <remarks>This animation is only available on the composition layer.</remarks>
        public IPropertyAnimationBuilder<double> Clip(Side side)
        {
            string property = side switch
            {
                Side.Top => nameof(InsetClip.TopInset),
                Side.Bottom => nameof(InsetClip.BottomInset),
                Side.Right => nameof(InsetClip.RightInset),
                Side.Left => nameof(InsetClip.LeftInset),
                _ => ThrowHelper.ThrowArgumentException<string>("Invalid clip size")
            };

            return new CompositionClipAnimationBuilder(this, property);
        }

        /// <summary>
        /// Adds a new size animation for a single axis to the current schedule.
        /// </summary>
        /// <param name="axis">The target size axis to animate.</param>
        /// <param name="layer">The target framework layer to animate.</param>
        /// <returns>An <see cref="IPropertyAnimationBuilder{T}"/> instance to configure the animation.</returns>
        public IPropertyAnimationBuilder<double> Size(Axis axis, FrameworkLayer layer = FrameworkLayer.Composition)
        {
            if (layer == FrameworkLayer.Composition)
            {
                return new PropertyAnimationBuilder<double>(this, $"{nameof(Visual.Size)}.{axis}", layer);
            }

            string property = axis switch
            {
                Axis.X => nameof(FrameworkElement.Width),
                Axis.Y => nameof(FrameworkElement.Height),
                _ => ThrowHelper.ThrowArgumentException<string>("Invalid size axis")
            };

            return new PropertyAnimationBuilder<double>(this, property, layer);
        }

        /// <summary>
        /// Adds a new composition size translation animation for all axes to the current schedule.
        /// </summary>
        /// <returns>An <see cref="IPropertyAnimationBuilder{T}"/> instance to configure the animation.</returns>
        /// <remarks>This animation is only available on the composition layer.</remarks>
        public IPropertyAnimationBuilder<Vector3> Size()
        {
            return new PropertyAnimationBuilder<Vector3>(this, nameof(Visual.Size), FrameworkLayer.Composition);
        }

        /// <summary>
        /// Adds a custom animation based on normalized keyframes to the current schedule.
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

        /// <summary>
        /// Adds a custom animation based on timed keyframes to the current schedule.
        /// </summary>
        /// <typeparam name="T">The type of values to animate.</typeparam>
        /// <param name="property">The target property to animate.</param>
        /// <param name="build">The callback to use to construct the custom animation.</param>
        /// <param name="delay">The optional initial delay for the animation.</param>
        /// <param name="layer">The target framework layer to animate.</param>
        /// <returns>The current <see cref="AnimationBuilder"/> instance.</returns>
        public AnimationBuilder TimedKeyFrames<T>(
            string property,
            Action<ITimedKeyFrameAnimationBuilder<T>> build,
            TimeSpan? delay = null,
            FrameworkLayer layer = FrameworkLayer.Composition)
            where T : unmanaged
        {
            if (layer == FrameworkLayer.Composition)
            {
                TimedKeyFrameAnimationBuilder<T>.Composition builder = new(property, delay);

                build(builder);

                this.compositionAnimationFactories.Add(builder);
            }
            else
            {
                TimedKeyFrameAnimationBuilder<T>.Xaml builder = new(property, delay);

                build(builder);

                this.xamlAnimationFactories.Add(builder);
            }

            return this;
        }
    }
}