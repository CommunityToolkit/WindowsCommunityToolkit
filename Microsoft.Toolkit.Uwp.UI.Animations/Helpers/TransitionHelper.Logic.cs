// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading;
using System.Threading.Tasks;
using Windows.UI.Composition;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Hosting;

namespace Microsoft.Toolkit.Uwp.UI.Animations
{
    /// <summary>
    /// A animation helper that morphs between two controls.
    /// </summary>
    public sealed partial class TransitionHelper
    {
        private void UpdateSourceAnimatedElements()
        {
            if (this.Source is null)
            {
                return;
            }

            this.sourceAnimatedElements.Clear();
            var filters = this.AnimationConfigs.Select(config => config.Id);

            foreach (var item in GetAnimatedElements(this.Source, filters))
            {
                this.sourceAnimatedElements[GetId(item)] = item;
            }
        }

        private void UpdateTargetAnimatedElements()
        {
            if (this.Target is null)
            {
                return;
            }

            this.targetAnimatedElements.Clear();
            var filters = this.AnimationConfigs.Select(config => config.Id);

            foreach (var item in GetAnimatedElements(this.Target, filters))
            {
                this.targetAnimatedElements[GetId(item)] = item;
            }
        }

        private void ToggleVisualState(UIElement target, VisualStateToggleMethod method, bool isVisible)
        {
            if (target is null)
            {
                return;
            }

            switch (method)
            {
                case VisualStateToggleMethod.ByVisibility:
                    target.Visibility = isVisible ? Visibility.Visible : Visibility.Collapsed;
                    break;
                case VisualStateToggleMethod.ByIsVisible:
                    var targetVisual = ElementCompositionPreview.GetElementVisual(target);
                    targetVisual.IsVisible = isVisible;
                    break;
                default:
                    break;
            }

            target.IsHitTestVisible = isVisible;
        }

        private (UIElement, UIElement) GetPairElements(AnimationConfig config)
        {
            return (this.sourceAnimatedElements.ContainsKey(config.Id) ? this.sourceAnimatedElements[config.Id] : null,
                this.targetAnimatedElements.ContainsKey(config.Id) ? this.targetAnimatedElements[config.Id] : null);
        }

        private Task AnimateControlsAsync(bool reversed, CancellationToken token)
        {
            var duration = this.AnimationDuration;
            if (this._isInterruptedAnimation)
            {
                duration *= this.interruptedAnimationReverseDurationRatio;
            }

            var animationTasks = new List<Task>();
            var sourceUnpairedElements = new List<UIElement>();
            var targetUnpairedElements = new List<UIElement>();
            foreach (var item in this.AnimationConfigs)
            {
                var (source, target) = this.GetPairElements(item);
                if (source is null || target is null)
                {
                    if (source is not null)
                    {
                        sourceUnpairedElements.Add(source);
                    }

                    if (target is not null)
                    {
                        targetUnpairedElements.Add(target);
                    }

                    continue;
                }

                animationTasks.Add(
                    this.AnimateElementsAsync(
                        reversed ? target : source,
                        reversed ? source : target,
                        duration,
                        item,
                        token));
            }

            animationTasks.Add(this.AnimateIgnoredOrUnpairedElementsAsync(this.Source, sourceUnpairedElements, reversed, token));
            animationTasks.Add(this.AnimateIgnoredOrUnpairedElementsAsync(this.Target, targetUnpairedElements, !reversed, token));

            return Task.WhenAll(animationTasks);
        }

        private async Task StartInterruptibleAnimationsAsync(bool reversed, CancellationToken token, bool forceUpdateAnimatedElements)
        {
            var reversedCancellationTokenSource =
                reversed ? this._animateCancellationTokenSource : this._reverseCancellationTokenSource;
            var reversedTaskSource =
                reversed ? this._animateTaskSource : this._reverseTaskSource;

            if (reversedCancellationTokenSource is not null)
            {
                if (this._isInterruptedAnimation)
                {
                    this._isInterruptedAnimation = false;
                    await reversedTaskSource.Task;
                }
                else
                {
                    reversedCancellationTokenSource?.Cancel();
                    this._isInterruptedAnimation = true;
                }
            }

            if (!this._isInterruptedAnimation && IsTargetState != reversed)
            {
                return;
            }

            if (token.IsCancellationRequested)
            {
                return;
            }

            TaskCompletionSource<object> currentTaskSource;
            if (reversed)
            {
                currentTaskSource = this._reverseTaskSource = new TaskCompletionSource<object>();
            }
            else
            {
                currentTaskSource = this._animateTaskSource = new TaskCompletionSource<object>();
            }

            await this.InitControlsStateAsync(forceUpdateAnimatedElements);
            await this.AnimateControlsAsync(reversed, token);
            if (token.IsCancellationRequested)
            {
                return;
            }

            this.RestoreState(!reversed);
            _ = currentTaskSource.TrySetResult(null);

            this._isInterruptedAnimation = false;
        }

        private void RestoreState(bool isTargetState)
        {
            this.IsTargetState = isTargetState;
            this.ToggleVisualState(this.Source, this.SourceToggleMethod, !isTargetState);
            this.ToggleVisualState(this.Target, this.TargetToggleMethod, isTargetState);
        }

        private void RestoreUIElements(IEnumerable<UIElement> animatedElements)
        {
            foreach (var animatedElement in animatedElements)
            {
                AnimationBuilder.Create()
                    .Translation(to: Vector3.Zero, duration: almostZeroDuration)
                    .Opacity(to: 1, duration: almostZeroDuration)
                    .Scale(to: Vector3.One, duration: almostZeroDuration)
                    .Start(animatedElement);
            }
        }

        private async Task InitControlsStateAsync(bool forceUpdateAnimatedElements = false)
        {
            await Task.WhenAll(
                this.InitControlStateAsync(this.Source, this._needUpdateSourceLayout),
                this.InitControlStateAsync(this.Target, this._needUpdateTargetLayout));

            this._needUpdateSourceLayout = false;
            this._needUpdateTargetLayout = false;

            if (forceUpdateAnimatedElements)
            {
                this.UpdateSourceAnimatedElements();
                this.UpdateTargetAnimatedElements();
            }
        }

        private Task InitControlStateAsync(FrameworkElement target, bool needUpdateLayout)
        {
            var updateLayoutTask = Task.CompletedTask;
            if (target is null)
            {
                return updateLayoutTask;
            }

            target.IsHitTestVisible = false;

            if (target.Visibility == Visibility.Collapsed)
            {
                target.Visibility = Visibility.Visible;
                if (needUpdateLayout)
                {
                    updateLayoutTask = this.UpdateControlLayoutAsync(target);
                }
            }

            if (target.Opacity < 0.01)
            {
                target.Opacity = 1;
            }

            var targetVisual = ElementCompositionPreview.GetElementVisual(target);
            if (!targetVisual.IsVisible)
            {
                targetVisual.IsVisible = true;
            }

            return updateLayoutTask;
        }

        private Task UpdateControlLayoutAsync(FrameworkElement target)
        {
            var updateTargetLayoutTaskSource = new TaskCompletionSource<object>();
            void OnTargetLayoutUpdated(object sender, object e)
            {
                target.LayoutUpdated -= OnTargetLayoutUpdated;
                _ = updateTargetLayoutTaskSource.TrySetResult(null);
            }

            target.LayoutUpdated += OnTargetLayoutUpdated;
            target.UpdateLayout();
            return updateTargetLayoutTaskSource.Task;
        }

        private Task AnimateElementsAsync(UIElement source, UIElement target, TimeSpan duration, AnimationConfig config, CancellationToken token)
        {
            var sourceBuilder = AnimationBuilder.Create();
            var targetBuilder = AnimationBuilder.Create();
            this.AnimateUIElementsTranslation(sourceBuilder, targetBuilder, source, target, duration);
            TimeSpan opacityAnimationDuration = config.OpacityMode switch
            {
                OpacityAnimationMode.Normal => duration,
                OpacityAnimationMode.Faster => duration * 1 / 3,
                _ => duration
            };
            this.AnimateUIElementsOpacity(sourceBuilder, targetBuilder, opacityAnimationDuration);
            switch (config.ScaleMode)
            {
                case ScaleMode.Scale:
                    this.AnimateUIElementsScale(sourceBuilder, targetBuilder, source, target, duration);
                    break;
                case ScaleMode.ScaleX:
                    this.AnimateUIElementsScaleX(sourceBuilder, targetBuilder, source, target, duration);
                    break;
                case ScaleMode.ScaleY:
                    this.AnimateUIElementsScaleY(sourceBuilder, targetBuilder, source, target, duration);
                    break;
                case ScaleMode.None:
                default:
                    break;
            }

            return Task.WhenAll(sourceBuilder.StartAsync(source, token), targetBuilder.StartAsync(target, token));
        }

        private Task AnimateIgnoredOrUnpairedElementsAsync(UIElement parent, IEnumerable<UIElement> unpairedElements, bool isShow, CancellationToken token)
        {
            if (parent is null)
            {
                return Task.CompletedTask;
            }

            var animationTasks = new List<Task>();
            var ignoredElements = GetIgnoredElements(parent);
            var duration = isShow ? this.IgnoredOrUnpairedElementShowDuration : this.IgnoredOrUnpairedElementHideDuration;
            var delay = isShow ? this.IgnoredOrUnpairedElementShowDelayDuration : TimeSpan.Zero;
            if (this._isInterruptedAnimation)
            {
                duration *= this.interruptedAnimationReverseDurationRatio;
                delay *= this.interruptedAnimationReverseDurationRatio;
            }

            foreach (var item in ignoredElements.Concat(unpairedElements))
            {
                if (this.IgnoredOrUnpairedElementHideTranslation != Vector3.Zero)
                {
                    animationTasks.Add(AnimationBuilder.Create().Translation(
                        from: this._isInterruptedAnimation ? null : (isShow ? this.IgnoredOrUnpairedElementHideTranslation : Vector3.Zero),
                        to: isShow ? Vector3.Zero : this.IgnoredOrUnpairedElementHideTranslation,
                        duration: duration,
                        delay: delay).StartAsync(item, token));
                }

                animationTasks.Add(AnimationBuilder.Create().Opacity(
                    from: this._isInterruptedAnimation ? null : (isShow ? 0 : 1),
                    to: isShow ? 1 : 0,
                    duration: duration,
                    delay: delay).StartAsync(item, token));

                if (isShow)
                {
                    delay += this.IgnoredOrUnpairedElementShowStepDuration;
                }
            }

            return Task.WhenAll(animationTasks);
        }

        private void AnimateUIElementsTranslation(AnimationBuilder sourceBuilder, AnimationBuilder targetBuilder, UIElement source, UIElement target, TimeSpan duration)
        {
            if (this._isInterruptedAnimation)
            {
                _ = sourceBuilder.Translation(to: Vector3.Zero, duration: almostZeroDuration);
                _ = targetBuilder.Translation(to: Vector3.Zero, duration: duration);
                return;
            }

            var diff = target.TransformToVisual(source).TransformPoint(default);
            _ = sourceBuilder.Translation().TimedKeyFrames(
                build: b => b
                    .KeyFrame(duration - almostZeroDuration, new Vector3((float)diff.X, (float)diff.Y, 0))
                    .KeyFrame(duration, Vector3.Zero));
            _ = targetBuilder.Translation().TimedKeyFrames(
                delayBehavior: AnimationDelayBehavior.SetInitialValueBeforeDelay,
                build: b => b
                    .KeyFrame(TimeSpan.Zero, new Vector3((float)-diff.X, (float)-diff.Y, 0))
                    .KeyFrame(duration - almostZeroDuration, Vector3.Zero)
                    .KeyFrame(duration, Vector3.Zero));
        }

        private void AnimateUIElementsScale(AnimationBuilder sourceBuilder, AnimationBuilder targetBuilder, UIElement source, UIElement target, TimeSpan duration)
        {
            var scaleX = target.ActualSize.X / source.ActualSize.X;
            var scaleY = target.ActualSize.Y / source.ActualSize.Y;
            var scale = new Vector3((float)scaleX, (float)scaleY, 1);
            this.AnimateUIElementsScale(sourceBuilder, targetBuilder, scale, duration);
        }

        private void AnimateUIElementsScaleX(AnimationBuilder sourceBuilder, AnimationBuilder targetBuilder, UIElement source, UIElement target, TimeSpan duration)
        {
            var scaleX = target.ActualSize.X / source.ActualSize.X;
            var scale = new Vector3((float)scaleX, (float)scaleX, 1);
            this.AnimateUIElementsScale(sourceBuilder, targetBuilder, scale, duration);
        }

        private void AnimateUIElementsScaleY(AnimationBuilder sourceBuilder, AnimationBuilder targetBuilder, UIElement source, UIElement target, TimeSpan duration)
        {
            var scaleY = target.ActualSize.Y / source.ActualSize.Y;
            var scale = new Vector3((float)scaleY, (float)scaleY, 1);
            this.AnimateUIElementsScale(sourceBuilder, targetBuilder, scale, duration);
        }

        private void AnimateUIElementsScale(AnimationBuilder sourceBuilder, AnimationBuilder targetBuilder, Vector3 targetScale, TimeSpan duration)
        {
            if (this._isInterruptedAnimation)
            {
                _ = sourceBuilder.Scale(to: Vector3.One, duration: almostZeroDuration);
                _ = targetBuilder.Scale(to: Vector3.One, duration: duration);
                return;
            }

            _ = sourceBuilder.Scale().TimedKeyFrames(
                build: b => b
                    .KeyFrame(duration - almostZeroDuration, targetScale)
                    .KeyFrame(duration, Vector3.One));
            _ = targetBuilder.Scale().TimedKeyFrames(
                delayBehavior: AnimationDelayBehavior.SetInitialValueBeforeDelay,
                build: b => b
                    .KeyFrame(TimeSpan.Zero, new Vector3(1 / targetScale.X, 1 / targetScale.Y, 1))
                    .KeyFrame(duration - almostZeroDuration, Vector3.One)
                    .KeyFrame(duration, Vector3.One));
        }

        private void AnimateUIElementsOpacity(AnimationBuilder sourceBuilder, AnimationBuilder targetBuilder, TimeSpan duration)
        {
            if (this._isInterruptedAnimation)
            {
                _ = sourceBuilder.Opacity(to: 0, duration: almostZeroDuration);
                _ = targetBuilder.Opacity(to: 1, duration: almostZeroDuration);
                return;
            }

            _ = sourceBuilder.Opacity(to: 0, duration: duration);
            _ = targetBuilder.Opacity(to: 1, duration: duration);
        }
    }
}
