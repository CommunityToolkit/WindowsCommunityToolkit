// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

#nullable enable

using System;
using System.Numerics;
using System.Threading.Tasks;
using Windows.UI.Composition;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Hosting;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using static Microsoft.Toolkit.Uwp.UI.Animations.Extensions.AnimationExtensions;

namespace Microsoft.Toolkit.Uwp.UI.Animations
{
    /// <summary>
    /// A <see langword="class"/> that allows to build custom animations targeting both the XAML and composition layers.
    /// </summary>
    public sealed partial class AnimationBuilder
    {
        /// <summary>
        /// Adds a new custom double animation targeting an arbitrary composition object.
        /// </summary>
        /// <param name="target">The target <see cref="CompositionObject"/> to animate.</param>
        /// <param name="property">The target property to animate.</param>
        /// <param name="from">The optional starting value for the animation.</param>
        /// <param name="to">The final value for the animation.</param>
        /// <param name="delay">The optional initial delay for the animation.</param>
        /// <param name="duration">The animation duration.</param>
        /// <param name="easingType">The optional easing function type for the animation.</param>
        /// <param name="easingMode">The optional easing function mode for the animation.</param>
        /// <returns>The current <see cref="AnimationBuilder"/> instance.</returns>
        public AnimationBuilder DoubleAnimation(
            CompositionObject target,
            string property,
            double? from,
            double to,
            TimeSpan? delay,
            TimeSpan duration,
            EasingType easingType = DefaultEasingType,
            EasingMode easingMode = DefaultEasingMode)
        {
            CompositionDoubleAnimation animation = new(
                target,
                property,
                (float?)from,
                (float)to,
                delay,
                duration,
                easingType,
                easingMode);

            this.compositionAnimations.Add(animation);

            return this;
        }

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
        /// <param name="layer">The target framework layer to animate.</param>
        /// <returns>The current <see cref="AnimationBuilder"/> instance.</returns>
        public AnimationBuilder Scale(
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
        /// Starts the animations present in the current <see cref="AnimationBuilder"/> instance.
        /// </summary>
        /// <param name="element">The target <see cref="UIElement"/> to animate.</param>
        public void Start(UIElement element)
        {
            if (this.compositionAnimations.Count > 0)
            {
                foreach (var animation in this.compositionAnimations)
                {
                    animation.StartAnimation();
                }
            }

            if (this.compositionAnimationFactories.Count > 0)
            {
                ElementCompositionPreview.SetIsTranslationEnabled(element, true);

                Visual visual = ElementCompositionPreview.GetElementVisual(element);
                CompositionAnimationGroup group = visual.Compositor.CreateAnimationGroup();

                foreach (var factory in this.compositionAnimationFactories)
                {
                    group.Add(factory.GetAnimation(visual));
                }

                visual.StartAnimationGroup(group);
            }

            if (this.xamlAnimationFactories.Count > 0)
            {
                Storyboard storyboard = new();

                foreach (var factory in this.xamlAnimationFactories)
                {
                    storyboard.Children.Add(factory.GetAnimation(element));
                }

                storyboard.Begin();
            }
        }

        /// <summary>
        /// Starts the animations present in the current <see cref="AnimationBuilder"/> instance.
        /// </summary>
        /// <param name="element">The target <see cref="UIElement"/> to animate.</param>
        /// <returns>A <see cref="Task"/> that completes when all animations have completed.</returns>
        public Task StartAsync(UIElement element)
        {
            Task
                compositionTask = Task.CompletedTask,
                xamlTask = Task.CompletedTask;

            if (this.compositionAnimationFactories.Count > 0 ||
                this.compositionAnimations.Count > 0)
            {
                Visual visual = ElementCompositionPreview.GetElementVisual(element);
                CompositionScopedBatch batch = visual.Compositor.CreateScopedBatch(CompositionBatchTypes.Animation);
                TaskCompletionSource<object?> taskCompletionSource = new();

                batch.Completed += (_, _) => taskCompletionSource.SetResult(null);

                foreach (var animation in this.compositionAnimations)
                {
                    animation.StartAnimation();
                }

                ElementCompositionPreview.SetIsTranslationEnabled(element, true);

                CompositionAnimationGroup group = visual.Compositor.CreateAnimationGroup();

                foreach (var factory in this.compositionAnimationFactories)
                {
                    group.Add(factory.GetAnimation(visual));
                }

                visual.StartAnimationGroup(group);

                batch.End();

                compositionTask = taskCompletionSource.Task;
            }

            if (this.xamlAnimationFactories.Count > 0)
            {
                Storyboard storyboard = new();
                TaskCompletionSource<object?> taskCompletionSource = new();

                foreach (var factory in this.xamlAnimationFactories)
                {
                    storyboard.Children.Add(factory.GetAnimation(element));
                }

                storyboard.Completed += (_, _) => taskCompletionSource.SetResult(null);
                storyboard.Begin();

                xamlTask = taskCompletionSource.Task;
            }

            return Task.WhenAll(compositionTask, xamlTask);
        }
    }
}
