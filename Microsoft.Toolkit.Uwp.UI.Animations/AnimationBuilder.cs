// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

#nullable enable

using System;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;
using Windows.UI.Composition;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Hosting;
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
        /// <para>
        /// A static constructor for the <see cref="AnimationBuilder"/> type.
        /// This can be used as the entry point to construct a custom animation sequence.
        /// </para>
        /// For instance:
        /// <code>
        /// AnimationBuilder.New()<br/>
        ///     .Opacity(from: 0, to: 1, duration: 400)<br/>
        ///     .Translation(Axis.X, from: -40, to: 0, duration: 400)<br/>
        ///     .Start(MyButton);
        /// </code>
        /// <para>
        /// Configured <see cref="AnimationBuilder"/> instances are also reusable, meaning that the same
        /// one can be used to start an animation sequence on multiple elements as well.
        /// </para>
        /// For instance:
        /// <code>
        /// var animation = AnimationBuilder.New().Opacity(0, 1, duration: 400).Size(1.2, 1, duration: 400);<br/>
        /// <br/>
        /// animation.Start(MyButton);<br/>
        /// animation.Start(MyGrid);
        /// </code>
        /// Alternatively, the <see cref="Xaml.AnimationCollection2"/> type can be used to configure animations directly
        /// from XAML. The same <see cref="AnimationBuilder"/> APIs will still be used behind the scenes to handle animations.
        /// </summary>
        /// <returns>An empty <see cref="AnimationBuilder"/> instance to use to construct an animation sequence.</returns>
        [Pure]
        public static AnimationBuilder New() => new();

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
