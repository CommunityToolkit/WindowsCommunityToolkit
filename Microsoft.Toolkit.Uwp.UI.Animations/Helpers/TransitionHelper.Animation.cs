// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Windows.UI.Composition;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Hosting;
using Windows.UI.Xaml.Media.Animation;

namespace Microsoft.Toolkit.Uwp.UI.Animations
{
    /// <summary>
    /// A animation helper that morphs between two controls.
    /// </summary>
    public sealed partial class TransitionHelper
    {
        private interface IKeyFrameCompositionAnimationFactory
        {
            KeyFrameAnimation GetAnimation(CompositionObject targetHint, out CompositionObject target);
        }

        private interface IKeyFrameAnimationGroupController
        {
            float? LastStopProgress { get; }

            Task StartAsync(CancellationToken token, TimeSpan? duration, float? startProgress);

            Task ReverseAsync(CancellationToken token, TimeSpan? duration, float? startProgress);

            void AddAnimationFor(UIElement target, IKeyFrameCompositionAnimationFactory factory);

            void AddAnimationGroupFor(UIElement target, IKeyFrameCompositionAnimationFactory[] factories);
        }

        private sealed record KeyFrameAnimationFactory<T>(
            string Property,
            T To,
            T? From,
            TimeSpan? Delay,
            TimeSpan? Duration,
            EasingType? EasingType,
            EasingMode? EasingMode,
            Dictionary<float, (T, EasingType?, EasingMode?)> NormalizedKeyFrames)
            : IKeyFrameCompositionAnimationFactory
            where T : unmanaged
        {
            public KeyFrameAnimation GetAnimation(CompositionObject targetHint, out CompositionObject target)
            {
                target = null;

                if (typeof(T) == typeof(float))
                {
                    var scalarAnimation = targetHint.Compositor.CreateScalarKeyFrameAnimation(
                        Property,
                        CastTo<float>(To),
                        CastToNullable<float>(From),
                        Delay,
                        Duration,
                        GetEasingFunction(targetHint.Compositor, EasingType, EasingMode),
                        iterationBehavior: AnimationIterationBehavior.Count,
                        iterationCount: 1);
                    if (NormalizedKeyFrames?.Count > 0)
                    {
                        foreach (var item in NormalizedKeyFrames)
                        {
                            var (value, easingType, easingMode) = item.Value;
                            scalarAnimation.InsertKeyFrame(item.Key, CastTo<float>(value), GetEasingFunction(targetHint.Compositor, easingType, easingMode));
                        }
                    }

                    return scalarAnimation;
                }

                if (typeof(T) == typeof(Vector2))
                {
                    var vector2Animation = targetHint.Compositor.CreateVector2KeyFrameAnimation(
                        Property,
                        CastTo<Vector2>(To),
                        CastToNullable<Vector2>(From),
                        Delay,
                        Duration,
                        GetEasingFunction(targetHint.Compositor, EasingType, EasingMode),
                        iterationBehavior: AnimationIterationBehavior.Count,
                        iterationCount: 1);
                    if (NormalizedKeyFrames?.Count > 0)
                    {
                        foreach (var item in NormalizedKeyFrames)
                        {
                            var (value, easingType, easingMode) = item.Value;
                            vector2Animation.InsertKeyFrame(item.Key, CastTo<Vector2>(value), GetEasingFunction(targetHint.Compositor, easingType, easingMode));
                        }
                    }

                    return vector2Animation;
                }

                throw new InvalidOperationException("Invalid animation type");
            }

            [Pure]
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            private TValue CastTo<TValue>(T value)
                where TValue : unmanaged
            {
                return (TValue)(object)value;
            }

            [Pure]
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            private TValue? CastToNullable<TValue>(T? value)
                where TValue : unmanaged
            {
                if (value is null)
                {
                    return null;
                }

                T validValue = value.GetValueOrDefault();

                return (TValue)(object)validValue;
            }
        }

        private sealed record ClipScalarAnimationFactory(
            string Property,
            float To,
            float? From,
            TimeSpan? Delay,
            TimeSpan? Duration,
            EasingType? EasingType,
            EasingMode? EasingMode)
            : IKeyFrameCompositionAnimationFactory
        {
            public KeyFrameAnimation GetAnimation(CompositionObject targetHint, out CompositionObject target)
            {
                Visual visual = (Visual)targetHint;
                InsetClip clip = visual.Clip as InsetClip ?? (InsetClip)(visual.Clip = visual.Compositor.CreateInsetClip());
                CompositionEasingFunction easingFunction = GetEasingFunction(clip.Compositor, EasingType, EasingMode);
                ScalarKeyFrameAnimation animation = clip.Compositor.CreateScalarKeyFrameAnimation(
                    Property,
                    To,
                    From,
                    Delay,
                    Duration,
                    easingFunction,
                    iterationBehavior: AnimationIterationBehavior.Count,
                    iterationCount: 1);

                target = clip;

                return animation;
            }
        }

        private static CompositionEasingFunction GetEasingFunction(Compositor compositor, EasingType? easingType, EasingMode? easingMode)
        {
            CompositionEasingFunction easingFunction = null;
            if (easingType.HasValue && easingMode.HasValue)
            {
                easingFunction = compositor.TryCreateEasingFunction(easingType.Value, easingMode.Value);
            }

            return easingFunction;
        }

        private IKeyFrameCompositionAnimationFactory[] Clip(
            Thickness to,
            Thickness? from = null,
            TimeSpan? delay = null,
            TimeSpan? duration = null,
            EasingType easingType = EasingType.Default,
            EasingMode easingMode = EasingMode.EaseInOut)
        {
            return new[]
            {
                new ClipScalarAnimationFactory(
                    nameof(InsetClip.LeftInset),
                    (float)to.Left,
                    (float?)from?.Left,
                    delay,
                    duration,
                    easingType,
                    easingMode),
                new ClipScalarAnimationFactory(
                    nameof(InsetClip.TopInset),
                    (float)to.Top,
                    (float?)from?.Top,
                    delay,
                    duration,
                    easingType,
                    easingMode),
                new ClipScalarAnimationFactory(
                    nameof(InsetClip.RightInset),
                    (float)to.Right,
                    (float?)from?.Right,
                    delay,
                    duration,
                    easingType,
                    easingMode),
                new ClipScalarAnimationFactory(
                    nameof(InsetClip.BottomInset),
                    (float)to.Bottom,
                    (float?)from?.Bottom,
                    delay,
                    duration,
                    easingType,
                    easingMode)
            };
        }

        private IKeyFrameCompositionAnimationFactory Translation(
            Vector2 to,
            Vector2? from = null,
            TimeSpan? delay = null,
            TimeSpan? duration = null,
            EasingType? easingType = null,
            EasingMode? easingMode = null,
            Dictionary<float, (Vector2, EasingType?, EasingMode?)> normalizedKeyFrames = null)
        {
            return new KeyFrameAnimationFactory<Vector2>("Translation.XY", to, from, delay, duration, easingType, easingMode, normalizedKeyFrames);
        }

        private IKeyFrameCompositionAnimationFactory Opacity(
            double to,
            double? from = null,
            TimeSpan? delay = null,
            TimeSpan? duration = null,
            EasingType? easingType = null,
            EasingMode? easingMode = null,
            Dictionary<float, (float, EasingType?, EasingMode?)> normalizedKeyFrames = null)
        {
            return new KeyFrameAnimationFactory<float>(nameof(Visual.Opacity), (float)to, (float?)from, delay, duration, easingType, easingMode, normalizedKeyFrames);
        }

        private IKeyFrameCompositionAnimationFactory Scale(
            Vector2 to,
            Vector2? from = null,
            TimeSpan? delay = null,
            TimeSpan? duration = null,
            EasingType? easingType = null,
            EasingMode? easingMode = null,
            Dictionary<float, (Vector2, EasingType?, EasingMode?)> normalizedKeyFrames = null)
        {
            return new KeyFrameAnimationFactory<Vector2>("Scale.XY", to, from, delay, duration, easingType, easingMode, normalizedKeyFrames);
        }

        private sealed class KeyFrameAnimationGroupController : IKeyFrameAnimationGroupController
        {
            private readonly Dictionary<UIElement, List<IKeyFrameCompositionAnimationFactory>> animationFactories = new();

            public float? LastStopProgress { get; private set; } = null;

            public void AddAnimationFor(UIElement target, IKeyFrameCompositionAnimationFactory factory)
            {
                if (factory is null)
                {
                    return;
                }

                if (animationFactories.ContainsKey(target))
                {
                    animationFactories[target].Add(factory);
                }
                else
                {
                    animationFactories.Add(target, new List<IKeyFrameCompositionAnimationFactory>() { factory });
                }
            }

            public void AddAnimationGroupFor(UIElement target, IKeyFrameCompositionAnimationFactory[] factories)
            {
                var validFactories = factories.Where(factory => factory is not null);
                if (validFactories.Any() is false)
                {
                    return;
                }

                if (animationFactories.ContainsKey(target))
                {
                    animationFactories[target].AddRange(validFactories);
                }
                else
                {
                    animationFactories.Add(target, new List<IKeyFrameCompositionAnimationFactory>(validFactories));
                }
            }

            public Task StartAsync(CancellationToken token, TimeSpan? duration = null, float? startProgress = null)
            {
                return AnimateAsync(false, token, duration, startProgress ?? this.LastStopProgress);
            }

            public Task ReverseAsync(CancellationToken token, TimeSpan? duration = null, float? startProgress = null)
            {
                return AnimateAsync(true, token, duration, startProgress ?? (1 - this.LastStopProgress));
            }

            private Task AnimateAsync(bool reversed, CancellationToken token, TimeSpan? duration, float? startProgress)
            {
                List<Task> tasks = null;
                List<(CompositionObject Target, string Path)> compositionAnimations = null;
                DateTime? animationStartTime = null;
                this.LastStopProgress = null;
                if (this.animationFactories.Count > 0)
                {
                    if (duration.HasValue)
                    {
                        var elapsedDuration = duration.Value * (startProgress ?? 0d);
                        animationStartTime = DateTime.Now - elapsedDuration;
                    }

                    tasks = new List<Task>(this.animationFactories.Count);
                    compositionAnimations = new List<(CompositionObject Target, string Path)>();
                    foreach (var item in this.animationFactories)
                    {
                        tasks.Add(StartForAsync(item.Key, reversed, duration, startProgress, compositionAnimations));
                    }
                }

                static void Stop(object state)
                {
                    var (controller, reversed, duration, animationStartTime, animations) = ((KeyFrameAnimationGroupController, bool, TimeSpan?, DateTime?, List<(CompositionObject Target, string Path)>))state;
                    foreach (var (target, path) in animations)
                    {
                        target.StopAnimation(path);
                    }

                    if (duration.HasValue is false || animationStartTime.HasValue is false)
                    {
                        return;
                    }

                    var stopProgress = Math.Max(0, Math.Min((DateTime.Now - animationStartTime.Value) / duration.Value, 1));
                    controller.LastStopProgress = (float)(reversed ? 1 - stopProgress : stopProgress);
                }

                if (compositionAnimations is not null)
                {
                    token.Register(static obj => Stop(obj), (this, reversed, duration, animationStartTime, compositionAnimations));
                }

                return tasks is null ? Task.CompletedTask : Task.WhenAll(tasks);
            }

            private Task StartForAsync(UIElement element, bool reversed, TimeSpan? duration, float? startProgress, List<(CompositionObject Target, string Path)> animations)
            {
                if (!this.animationFactories.TryGetValue(element, out var factories) || factories.Count > 0 is false)
                {
                    return Task.CompletedTask;
                }

                ElementCompositionPreview.SetIsTranslationEnabled(element, true);
                var visual = element.GetVisual();
                var batch = visual.Compositor.CreateScopedBatch(CompositionBatchTypes.Animation);
                var taskCompletionSource = new TaskCompletionSource<object>();
                batch.Completed += (_, _) => taskCompletionSource.SetResult(null);
                foreach (var factory in factories)
                {
                    var animation = factory.GetAnimation(visual, out var target);
                    if (duration.HasValue)
                    {
                        animation.Duration = duration.Value;
                    }

                    if (reversed)
                    {
                        animation.Direction = AnimationDirection.Reverse;
                    }

                    (target ?? visual).StartAnimation(animation.Target, animation);
                    if (startProgress.HasValue)
                    {
                        var controller = (target ?? visual).TryGetAnimationController(animation.Target);
                        if (controller is not null)
                        {
                            controller.Progress = startProgress.Value;
                        }
                    }

                    animations.Add((target ?? visual, animation.Target));
                }

                batch.End();
                return taskCompletionSource.Task;
            }
        }
    }
}
