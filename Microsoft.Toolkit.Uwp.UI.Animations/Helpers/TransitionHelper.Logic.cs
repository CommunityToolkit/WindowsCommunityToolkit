// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media.Animation;

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

            UpdateAnimatedElements(this.Source, this.sourceConnectedAnimatedElements, this.sourceIndependentAnimatedElements);
        }

        private void UpdateTargetAnimatedElements()
        {
            if (this.Target is null)
            {
                return;
            }

            UpdateAnimatedElements(this.Target, this.targetConnectedAnimatedElements, this.targetIndependentAnimatedElements);
        }

        private void UpdateAnimatedElements(DependencyObject parent, IDictionary<string, UIElement> connectedAnimatedElements, ICollection<UIElement> independentAnimatedElements)
        {
            if (this.Source is null)
            {
                return;
            }

            connectedAnimatedElements.Clear();
            independentAnimatedElements.Clear();

            foreach (var item in GetAnimatedElements(parent))
            {
                if (GetId(item) is { } id)
                {
                    connectedAnimatedElements[id] = item;
                }
                else
                {
                    independentAnimatedElements.Add(item);
                }
            }
        }

        private Task AnimateControlsAsync(bool reversed, CancellationToken token)
        {
            var duration = this.Duration;
            if (this._isInterruptedAnimation)
            {
                duration *= InterruptedAnimationReverseDurationRatio;
            }

            var animationTasks = new List<Task>();
            var sourceUnpairedElements = this.sourceConnectedAnimatedElements
                .Where(item => !this.targetConnectedAnimatedElements.ContainsKey(item.Key))
                .Select(item => item.Value);
            var targetUnpairedElements = this.targetConnectedAnimatedElements
                .Where(item => !this.sourceConnectedAnimatedElements.ContainsKey(item.Key))
                .Select(item => item.Value);
            var pairedElementKeys = this.sourceConnectedAnimatedElements
                .Where(item => this.targetConnectedAnimatedElements.ContainsKey(item.Key))
                .Select(item => item.Key);
            foreach (var key in pairedElementKeys)
            {
                var source = this.sourceConnectedAnimatedElements[key];
                var target = this.targetConnectedAnimatedElements[key];
                var animationConfig = this.Configs.FirstOrDefault(config => config.Id == key) ??
                                      this.DefaultConfig;
                animationTasks.Add(
                    this.AnimateElementsAsync(
                        reversed ? target : source,
                        reversed ? source : target,
                        duration,
                        animationConfig,
                        token));
            }

            animationTasks.Add(
                this.AnimateIndependentElementsAsync(
                    this.sourceIndependentAnimatedElements.Concat(sourceUnpairedElements),
                    reversed,
                    token,
                    IndependentElementEasingType,
                    IndependentElementEasingMode));
            animationTasks.Add(
                this.AnimateIndependentElementsAsync(
                    this.targetIndependentAnimatedElements.Concat(targetUnpairedElements),
                    !reversed,
                    token,
                    IndependentElementEasingType,
                    IndependentElementEasingMode));

            return Task.WhenAll(animationTasks);
        }

        private async Task StartInterruptibleAnimationsAsync(bool reversed, CancellationToken token, bool forceUpdateAnimatedElements)
        {
            if (!this._isInterruptedAnimation && IsTargetState != reversed)
            {
                return;
            }

            if (token.IsCancellationRequested)
            {
                return;
            }

            TaskCompletionSource<bool> currentTaskSource;
            if (reversed)
            {
                currentTaskSource = this._reverseTaskSource = new();
            }
            else
            {
                currentTaskSource = this._animateTaskSource = new();
            }

            await this.InitControlsStateAsync(forceUpdateAnimatedElements);
            if (token.IsCancellationRequested)
            {
                _ = currentTaskSource.TrySetResult(false);
                return;
            }

            await this.AnimateControlsAsync(reversed, token);
            if (token.IsCancellationRequested)
            {
                _ = currentTaskSource.TrySetResult(false);
                return;
            }

            this.RestoreState(!reversed, false);
            _ = currentTaskSource.TrySetResult(true);

            this._isInterruptedAnimation = false;
        }

        private void RestoreState(bool isTargetState, bool restoreAllChildElements)
        {
            this.IsTargetState = isTargetState;
            ToggleVisualState(this.Source, this.SourceToggleMethod, !isTargetState);
            ToggleVisualState(this.Target, this.TargetToggleMethod, isTargetState);
            if (restoreAllChildElements)
            {
                RestoreUIElements(this.SourceAnimatedElements);
                RestoreUIElements(this.TargetAnimatedElements);
            }
            else
            {
                RestoreUIElements(isTargetState ? this.SourceAnimatedElements : this.TargetAnimatedElements);
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

            target.IsHitTestVisible = IsHitTestVisibleWhenAnimating;

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

            var targetVisual = target.GetVisual();
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

        private Task AnimateElementsAsync(UIElement source, UIElement target, TimeSpan duration, TransitionConfig config, CancellationToken token)
        {
            var sourceBuilder = AnimationBuilder.Create();
            var targetBuilder = AnimationBuilder.Create();

            source.GetVisual().CenterPoint =
                new Vector3(source.ActualSize * config.NormalizedCenterPoint.ToVector2(), 0);
            target.GetVisual().CenterPoint =
                new Vector3(target.ActualSize * config.NormalizedCenterPoint.ToVector2(), 0);
            this.AnimateUIElementsTranslation(
                sourceBuilder,
                targetBuilder,
                source,
                target,
                config.NormalizedCenterPoint.ToVector2(),
                duration,
                config.EasingType,
                config.EasingMode);
            this.AnimateUIElementsOpacity(
                sourceBuilder,
                targetBuilder,
                duration,
                config.TransitionMode);
            switch (config.ScaleMode)
            {
                case ScaleMode.Scale:
                    this.AnimateUIElementsScale(
                        sourceBuilder,
                        targetBuilder,
                        source,
                        target,
                        duration,
                        config.EasingType,
                        config.EasingMode);
                    break;
                case ScaleMode.ScaleX:
                    this.AnimateUIElementsScaleX(
                        sourceBuilder,
                        targetBuilder,
                        source,
                        target,
                        duration,
                        config.EasingType,
                        config.EasingMode);
                    break;
                case ScaleMode.ScaleY:
                    this.AnimateUIElementsScaleY(
                        sourceBuilder,
                        targetBuilder,
                        source,
                        target,
                        duration,
                        config.EasingType,
                        config.EasingMode);
                    break;
                case ScaleMode.None:
                default:
                    break;
            }

            return Task.WhenAll(sourceBuilder.StartAsync(source, token), targetBuilder.StartAsync(target, token));
        }

        private Task AnimateIndependentElementsAsync(
            IEnumerable<UIElement> independentElements,
            bool isShow,
            CancellationToken token,
            EasingType easingType,
            EasingMode easingMode)
        {
            var uiElements = independentElements as UIElement[] ?? independentElements.ToArray();
            if (!uiElements.Any())
            {
                return Task.CompletedTask;
            }

            var animationTasks = new List<Task>();
            var duration = isShow ? this.IndependentElementShowDuration : this.IndependentElementHideDuration;
            var delay = isShow ? this.IndependentElementShowDelay : TimeSpan.Zero;
            if (this._isInterruptedAnimation)
            {
                duration *= InterruptedAnimationReverseDurationRatio;
                delay *= InterruptedAnimationReverseDurationRatio;
            }

            foreach (var item in uiElements)
            {
                if (this.IndependentElementHideTranslation != default)
                {
                    animationTasks.Add(
                        AnimationBuilder
                            .Create()
                            .Translation(
                                from: this._isInterruptedAnimation
                                    ? null
                                    : (isShow ? this.IndependentElementHideTranslation.ToVector3() : Vector3.Zero),
                                to: isShow ? Vector3.Zero : this.IndependentElementHideTranslation.ToVector3(),
                                duration: duration,
                                easingType: easingType,
                                easingMode: easingMode,
                                delay: delay)
                            .StartAsync(item, token));
                }

                animationTasks.Add(
                    AnimationBuilder
                        .Create()
                        .Opacity(
                            from: this._isInterruptedAnimation ? null : (isShow ? 0 : 1),
                            to: isShow ? 1 : 0,
                            duration: duration,
                            easingType: easingType,
                            easingMode: easingMode,
                            delay: delay)
                        .StartAsync(item, token));

                if (isShow)
                {
                    delay += this.IndependentElementShowInterval;
                }
            }

            return Task.WhenAll(animationTasks);
        }

        private void AnimateUIElementsTranslation(
            AnimationBuilder sourceBuilder,
            AnimationBuilder targetBuilder,
            UIElement source,
            UIElement target,
            Vector2 normalizedCenterPoint,
            TimeSpan duration,
            EasingType easingType,
            EasingMode easingMode)
        {
            if (this._isInterruptedAnimation)
            {
                _ = sourceBuilder.Translation(Vector3.Zero, duration: almostZeroDuration);
                _ = targetBuilder.Translation(Vector3.Zero, duration: duration, easingType: easingType, easingMode: easingMode);
                return;
            }

            var sourceNormalizedCenterPoint = source.ActualSize * normalizedCenterPoint;
            var targetNormalizedCenterPoint = target.ActualSize * normalizedCenterPoint;
            var diff = target.TransformToVisual(source).TransformPoint(default).ToVector2() -
                sourceNormalizedCenterPoint + targetNormalizedCenterPoint;
            _ = sourceBuilder.Translation().TimedKeyFrames(
                b => b
                    .KeyFrame(duration - almostZeroDuration, new Vector3(diff, 0), easingType, easingMode)
                    .KeyFrame(duration, Vector3.Zero));
            _ = targetBuilder.Translation().TimedKeyFrames(
                b => b
                    .KeyFrame(TimeSpan.Zero, new Vector3(-diff, 0))
                    .KeyFrame(duration - almostZeroDuration, Vector3.Zero, easingType, easingMode)
                    .KeyFrame(duration, Vector3.Zero));
        }

        private void AnimateUIElementsScale(
            AnimationBuilder sourceBuilder,
            AnimationBuilder targetBuilder,
            UIElement source,
            UIElement target,
            TimeSpan duration,
            EasingType easingType,
            EasingMode easingMode)
        {
            var scaleX = target.ActualSize.X / source.ActualSize.X;
            var scaleY = target.ActualSize.Y / source.ActualSize.Y;
            var scale = new Vector3(scaleX, scaleY, 1);
            this.AnimateUIElementsScale(sourceBuilder, targetBuilder, scale, duration, easingType, easingMode);
        }

        private void AnimateUIElementsScaleX(
            AnimationBuilder sourceBuilder,
            AnimationBuilder targetBuilder,
            UIElement source,
            UIElement target,
            TimeSpan duration,
            EasingType easingType,
            EasingMode easingMode)
        {
            var scaleX = target.ActualSize.X / source.ActualSize.X;
            var scale = new Vector3(scaleX, scaleX, 1);
            this.AnimateUIElementsScale(sourceBuilder, targetBuilder, scale, duration, easingType, easingMode);
        }

        private void AnimateUIElementsScaleY(
            AnimationBuilder sourceBuilder,
            AnimationBuilder targetBuilder,
            UIElement source,
            UIElement target,
            TimeSpan duration,
            EasingType easingType,
            EasingMode easingMode)
        {
            var scaleY = target.ActualSize.Y / source.ActualSize.Y;
            var scale = new Vector3(scaleY, scaleY, 1);
            this.AnimateUIElementsScale(sourceBuilder, targetBuilder, scale, duration, easingType, easingMode);
        }

        private void AnimateUIElementsScale(
            AnimationBuilder sourceBuilder,
            AnimationBuilder targetBuilder,
            Vector3 targetScale,
            TimeSpan duration,
            EasingType easingType,
            EasingMode easingMode)
        {
            if (this._isInterruptedAnimation)
            {
                _ = sourceBuilder.Scale(Vector3.One, duration: almostZeroDuration);
                _ = targetBuilder.Scale(Vector3.One, duration: duration, easingType: easingType, easingMode: easingMode);
                return;
            }

            _ = sourceBuilder.Scale().TimedKeyFrames(
                b => b
                    .KeyFrame(duration - almostZeroDuration, targetScale, easingType, easingMode)
                    .KeyFrame(duration, Vector3.One));
            _ = targetBuilder.Scale().TimedKeyFrames(
                b => b
                    .KeyFrame(TimeSpan.Zero, new Vector3(1 / targetScale.X, 1 / targetScale.Y, 1))
                    .KeyFrame(duration - almostZeroDuration, Vector3.One, easingType, easingMode)
                    .KeyFrame(duration, Vector3.One));
        }

        private void AnimateUIElementsOpacity(
            AnimationBuilder sourceBuilder,
            AnimationBuilder targetBuilder,
            TimeSpan duration,
            TransitionMode transitionMode)
        {
            if (this._isInterruptedAnimation)
            {
                _ = sourceBuilder.Opacity(0, duration: almostZeroDuration);
                _ = targetBuilder.Opacity(1, duration: almostZeroDuration);
                return;
            }

            switch (transitionMode)
            {
                case TransitionMode.Normal:
                    _ = sourceBuilder.Opacity().TimedKeyFrames(
                        b => b
                            .KeyFrame(TimeSpan.Zero, 1)
                            .KeyFrame(duration / 3, 0, EasingType.Linear));
                    break;
                case TransitionMode.Image:
                    _ = sourceBuilder.Opacity().TimedKeyFrames(
                        b => b
                            .KeyFrame(TimeSpan.Zero, 1)
                            .KeyFrame(duration / 3, 1)
                            .KeyFrame(duration * 2 / 3, 0, EasingType.Linear));
                    break;
            }

            _ = targetBuilder.Opacity().TimedKeyFrames(
                b => b
                    .KeyFrame(TimeSpan.Zero, 0)
                    .KeyFrame(duration / 3, 1, EasingType.Linear));
        }
    }
}
