// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

#nullable enable

using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Threading;
using System.Threading.Tasks;
using Windows.UI.Composition;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Hosting;
using Windows.UI.Xaml.Media.Animation;

namespace Microsoft.Toolkit.Uwp.UI.Animations
{
    /// <summary>
    /// A <see langword="class"/> that allows to build custom animations targeting both the XAML and composition layers.
    /// </summary>
    public sealed partial class AnimationBuilder
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AnimationBuilder"/> class.
        /// </summary>
        /// <remarks>This is private as the public entry point is the <see cref="Create"/> method.</remarks>
        private AnimationBuilder()
        {
        }

        /// <summary>
        /// <para>
        /// Creates a new <see cref="AnimationBuilder"/> instance to setup an animation schedule.
        /// This can be used as the entry point to construct a custom animation sequence.
        /// </para>
        /// For instance:
        /// <code>
        /// AnimationBuilder.Create()<br/>
        ///     .Opacity(from: 0, to: 1)<br/>
        ///     .Translation(Axis.X, from: -40, to: 0)<br/>
        ///     .Start(MyButton);
        /// </code>
        /// <para>
        /// Configured <see cref="AnimationBuilder"/> instances are also reusable, meaning that the same
        /// one can be used to start an animation sequence on multiple elements as well.
        /// </para>
        /// For instance:
        /// <code>
        /// var animation = AnimationBuilder.Create().Opacity(0, 1).Size(1.2, 1);<br/>
        /// <br/>
        /// animation.Start(MyButton);<br/>
        /// animation.Start(MyGrid);
        /// </code>
        /// Alternatively, the <see cref="AnimationSet"/> type can be used to configure animations directly from XAML.
        /// The same <see cref="AnimationBuilder"/> APIs will still be used behind the scenes to handle animations.
        /// </summary>
        /// <returns>An empty <see cref="AnimationBuilder"/> instance to use to construct an animation sequence.</returns>
        [Pure]
        public static AnimationBuilder Create() => new();

        /// <summary>
        /// Starts the animations present in the current <see cref="AnimationBuilder"/> instance.
        /// </summary>
        /// <param name="element">The target <see cref="UIElement"/> to animate.</param>
        public void Start(UIElement element)
        {
            if (this.compositionAnimationFactories.Count > 0)
            {
                ElementCompositionPreview.SetIsTranslationEnabled(element, true);

                Visual visual = ElementCompositionPreview.GetElementVisual(element);

                foreach (var factory in this.compositionAnimationFactories)
                {
                    var animation = factory.GetAnimation(visual, out var target);

                    if (target is null)
                    {
                        visual.StartAnimation(animation.Target, animation);
                    }
                    else
                    {
                        target.StartAnimation(animation.Target, animation);
                    }
                }
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
        /// Starts the animations present in the current <see cref="AnimationBuilder"/> instance, and
        /// registers a given cancellation token to stop running animations before they complete.
        /// </summary>
        /// <param name="element">The target <see cref="UIElement"/> to animate.</param>
        /// <param name="token">The cancellation token to stop animations while they're running.</param>
        public void Start(UIElement element, CancellationToken token)
        {
            List<(CompositionObject Target, string Path)>? compositionAnimations = null;

            if (this.compositionAnimationFactories.Count > 0)
            {
                compositionAnimations = new List<(CompositionObject Target, string Path)>(this.compositionAnimationFactories.Count);

                ElementCompositionPreview.SetIsTranslationEnabled(element, true);

                Visual visual = ElementCompositionPreview.GetElementVisual(element);

                foreach (var factory in this.compositionAnimationFactories)
                {
                    var animation = factory.GetAnimation(visual, out var target);

                    if (target is null)
                    {
                        visual.StartAnimation(animation.Target, animation);
                    }
                    else
                    {
                        target.StartAnimation(animation.Target, animation);
                    }

                    compositionAnimations.Add((target ?? visual, animation.Target));
                }
            }

            Storyboard? storyboard = null;

            if (this.xamlAnimationFactories.Count > 0)
            {
                storyboard = new Storyboard();

                foreach (var factory in this.xamlAnimationFactories)
                {
                    storyboard.Children.Add(factory.GetAnimation(element));
                }

                storyboard.Begin();
            }

            static void Stop(object state)
            {
                (List<(CompositionObject Target, string Path)>? animations, Storyboard? storyboard) = ((List<(CompositionObject, string)>?, Storyboard?))state;

                if (animations is not null)
                {
                    foreach (var (target, path) in animations)
                    {
                        target.StopAnimation(path);
                    }
                }

                storyboard?.Stop();
            }

            token.Register(static obj => Stop(obj), (compositionAnimations, storyboard));
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

            if (this.compositionAnimationFactories.Count > 0)
            {
                ElementCompositionPreview.SetIsTranslationEnabled(element, true);

                Visual visual = ElementCompositionPreview.GetElementVisual(element);
                CompositionScopedBatch batch = visual.Compositor.CreateScopedBatch(CompositionBatchTypes.Animation);
                TaskCompletionSource<object?> taskCompletionSource = new();

                batch.Completed += (_, _) => taskCompletionSource.SetResult(null);

                foreach (var factory in this.compositionAnimationFactories)
                {
                    var animation = factory.GetAnimation(visual, out var target);

                    if (target is null)
                    {
                        visual.StartAnimation(animation.Target, animation);
                    }
                    else
                    {
                        target.StartAnimation(animation.Target, animation);
                    }
                }

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

        /// <summary>
        /// Starts the animations present in the current <see cref="AnimationBuilder"/> instance, and
        /// registers a given cancellation token to stop running animations before they complete.
        /// </summary>
        /// <param name="element">The target <see cref="UIElement"/> to animate.</param>
        /// <param name="token">The cancellation token to stop animations while they're running.</param>
        /// <returns>A <see cref="Task"/> that completes when all animations have completed.</returns>
        public Task StartAsync(UIElement element, CancellationToken token)
        {
            Task
                compositionTask = Task.CompletedTask,
                xamlTask = Task.CompletedTask;
            List<(CompositionObject Target, string Path)>? compositionAnimations = null;

            if (this.compositionAnimationFactories.Count > 0)
            {
                compositionAnimations = new List<(CompositionObject Target, string Path)>(this.compositionAnimationFactories.Count);

                ElementCompositionPreview.SetIsTranslationEnabled(element, true);

                Visual visual = ElementCompositionPreview.GetElementVisual(element);
                CompositionScopedBatch batch = visual.Compositor.CreateScopedBatch(CompositionBatchTypes.Animation);
                TaskCompletionSource<object?> taskCompletionSource = new();

                batch.Completed += (_, _) => taskCompletionSource.SetResult(null);

                foreach (var factory in this.compositionAnimationFactories)
                {
                    var animation = factory.GetAnimation(visual, out var target);

                    if (target is null)
                    {
                        visual.StartAnimation(animation.Target, animation);
                    }
                    else
                    {
                        target.StartAnimation(animation.Target, animation);
                    }

                    compositionAnimations.Add((target ?? visual, animation.Target));
                }

                batch.End();

                compositionTask = taskCompletionSource.Task;
            }

            Storyboard? storyboard = null;

            if (this.xamlAnimationFactories.Count > 0)
            {
                storyboard = new Storyboard();

                TaskCompletionSource<object?> taskCompletionSource = new();

                foreach (var factory in this.xamlAnimationFactories)
                {
                    storyboard.Children.Add(factory.GetAnimation(element));
                }

                storyboard.Completed += (_, _) => taskCompletionSource.SetResult(null);
                storyboard.Begin();

                xamlTask = taskCompletionSource.Task;
            }

            static void Stop(object state)
            {
                (List<(CompositionObject Target, string Path)>? animations, Storyboard? storyboard) = ((List<(CompositionObject, string)>?, Storyboard?))state;

                if (animations is not null)
                {
                    foreach (var (target, path) in animations)
                    {
                        target.StopAnimation(path);
                    }
                }

                storyboard?.Stop();
            }

            token.Register(static obj => Stop(obj), (compositionAnimations, storyboard));

            return Task.WhenAll(compositionTask, xamlTask);
        }
    }
}
