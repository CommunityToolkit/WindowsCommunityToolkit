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
using Windows.UI.Xaml.Controls;
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

        private Task AnimateControls(bool reversed, CancellationToken token)
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
                    this.AnimateElements(
                        reversed ? target : source,
                        reversed ? source : target,
                        duration,
                        animationConfig,
                        token));
            }

            animationTasks.Add(
                this.AnimateIndependentElements(
                    this.sourceIndependentAnimatedElements.Concat(sourceUnpairedElements),
                    reversed,
                    token,
                    IndependentElementEasingType,
                    IndependentElementEasingMode));
            animationTasks.Add(
                this.AnimateIndependentElements(
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

            await this.AnimateControls(reversed, token);
            if (token.IsCancellationRequested)
            {
                _ = currentTaskSource.TrySetResult(false);
                return;
            }

            this.RestoreState(!reversed);
            _ = currentTaskSource.TrySetResult(true);

            this._isInterruptedAnimation = false;
        }

        private void RestoreState(bool isTargetState)
        {
            this.IsTargetState = isTargetState;
            Canvas.SetZIndex(this.Source, _sourceZIndex);
            Canvas.SetZIndex(this.Target, _targetZIndex);
            ToggleVisualState(this.Source, this.SourceToggleMethod, !isTargetState);
            ToggleVisualState(this.Target, this.TargetToggleMethod, isTargetState);
            RestoreElements(this.SourceAnimatedElements.Concat(this.TargetAnimatedElements));
        }

        private async Task InitControlsStateAsync(bool forceUpdateAnimatedElements = false)
        {
            var maxZIndex = Math.Max(_sourceZIndex, _targetZIndex) + 1;
            Canvas.SetZIndex(this.IsTargetState ? this.Source : this.Target, maxZIndex);

            await Task.WhenAll(
                this.InitControlState(this.Source, this._needUpdateSourceLayout),
                this.InitControlState(this.Target, this._needUpdateTargetLayout));

            this._needUpdateSourceLayout = false;
            this._needUpdateTargetLayout = false;

            if (forceUpdateAnimatedElements)
            {
                this.UpdateSourceAnimatedElements();
                this.UpdateTargetAnimatedElements();
            }
        }

        private Task InitControlState(FrameworkElement target, bool needUpdateLayout)
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
                    updateLayoutTask = this.UpdateControlLayout(target);
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

        private Task UpdateControlLayout(FrameworkElement target)
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

        private Task AnimateElements(UIElement source, UIElement target, TimeSpan duration, TransitionConfig config, CancellationToken token)
        {
            var sourceBuilder = AnimationBuilder.Create();
            var targetBuilder = AnimationBuilder.Create();

            var sourceActualSize = source is FrameworkElement sourceElement ? new Vector2((float)sourceElement.ActualWidth, (float)sourceElement.ActualHeight) : source.ActualSize;
            var targetActualSize = target is FrameworkElement targetElement ? new Vector2((float)targetElement.ActualWidth, (float)targetElement.ActualHeight) : target.ActualSize;
            var sourceCenterPoint = sourceActualSize * config.NormalizedCenterPoint.ToVector2();
            var targetCenterPoint = targetActualSize * config.NormalizedCenterPoint.ToVector2();

            source.GetVisual().CenterPoint = new Vector3(sourceCenterPoint, 0);
            target.GetVisual().CenterPoint = new Vector3(targetCenterPoint, 0);
            this.AnimateTranslation(
                sourceBuilder,
                targetBuilder,
                source,
                target,
                sourceCenterPoint,
                targetCenterPoint,
                duration,
                config.EasingType,
                config.EasingMode);
            this.AnimateOpacity(
                sourceBuilder,
                targetBuilder,
                duration);
            if (config is { EnableClipAnimation: true, ScaleMode: not ScaleMode.Scale })
            {
                Axis? axis = config.ScaleMode switch
                {
                    ScaleMode.None => null,
                    ScaleMode.Scale => null,
                    ScaleMode.ScaleX => Axis.Y,
                    ScaleMode.ScaleY => Axis.X,
                    _ => null,
                };
                this.AnimateClip(
                    sourceBuilder,
                    targetBuilder,
                    sourceActualSize,
                    targetActualSize,
                    sourceCenterPoint,
                    targetCenterPoint,
                    duration,
                    config.EasingType,
                    config.EasingMode,
                    axis);
            }

            switch (config.ScaleMode)
            {
                case ScaleMode.Scale:
                    this.AnimateScale(
                        sourceBuilder,
                        targetBuilder,
                        sourceActualSize,
                        targetActualSize,
                        duration,
                        config.EasingType,
                        config.EasingMode);
                    break;
                case ScaleMode.ScaleX:
                    this.AnimateScaleX(
                        sourceBuilder,
                        targetBuilder,
                        sourceActualSize,
                        targetActualSize,
                        duration,
                        config.EasingType,
                        config.EasingMode);
                    break;
                case ScaleMode.ScaleY:
                    this.AnimateScaleY(
                        sourceBuilder,
                        targetBuilder,
                        sourceActualSize,
                        targetActualSize,
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

        private Task AnimateIndependentElements(
            IEnumerable<UIElement> independentElements,
            bool isShow,
            CancellationToken token,
            EasingType easingType,
            EasingMode easingMode)
        {
            if (independentElements?.ToArray() is not { Length: > 0 } elements)
            {
                return Task.CompletedTask;
            }

            var animationTasks = new List<Task>();
            var duration = isShow ? this.IndependentElementShowDuration : this.IndependentElementHideDuration;
            var delay = isShow ? this.IndependentElementShowDelay : TimeSpan.Zero;
            var translationFrom = isShow ? this.IndependentElementHideTranslation.ToVector3() : Vector3.Zero;
            var translationTo = isShow ? Vector3.Zero : this.IndependentElementHideTranslation.ToVector3();
            var opacityFrom = isShow ? 0 : 1;
            var opacityTo = isShow ? 1 : 0;
            if (this._isInterruptedAnimation)
            {
                duration *= InterruptedAnimationReverseDurationRatio;
                delay *= InterruptedAnimationReverseDurationRatio;
            }

            foreach (var item in elements)
            {
                var animationBuilder = AnimationBuilder.Create();
                if (Math.Abs(this.IndependentElementHideTranslation.X) > 0.01 ||
                    Math.Abs(this.IndependentElementHideTranslation.Y) > 0.01)
                {
                    animationBuilder.Translation(
                        translationTo,
                        this._isInterruptedAnimation ? null : translationFrom,
                        delay,
                        duration: duration,
                        easingType: easingType,
                        easingMode: easingMode);
                }

                animationBuilder.Opacity(
                    opacityTo,
                    this._isInterruptedAnimation ? null : opacityFrom,
                    delay,
                    duration,
                    easingType: easingType,
                    easingMode: easingMode);

                animationTasks.Add(animationBuilder.StartAsync(item, token));
                if (isShow)
                {
                    delay += this.IndependentElementShowInterval;
                }
            }

            return Task.WhenAll(animationTasks);
        }

        private void AnimateTranslation(
            AnimationBuilder sourceBuilder,
            AnimationBuilder targetBuilder,
            UIElement source,
            UIElement target,
            Vector2 sourceCenterPoint,
            Vector2 targetCenterPoint,
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

            var diff = target.TransformToVisual(source).TransformPoint(default).ToVector2() - sourceCenterPoint + targetCenterPoint;
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

        private void AnimateScale(
            AnimationBuilder sourceBuilder,
            AnimationBuilder targetBuilder,
            Vector2 sourceActualSize,
            Vector2 targetActualSize,
            TimeSpan duration,
            EasingType easingType,
            EasingMode easingMode)
        {
            var scaleX = targetActualSize.X / sourceActualSize.X;
            var scaleY = targetActualSize.Y / sourceActualSize.Y;
            var scale = new Vector3(scaleX, scaleY, 1);
            this.AnimateScale(sourceBuilder, targetBuilder, scale, duration, easingType, easingMode);
        }

        private void AnimateScaleX(
            AnimationBuilder sourceBuilder,
            AnimationBuilder targetBuilder,
            Vector2 sourceActualSize,
            Vector2 targetActualSize,
            TimeSpan duration,
            EasingType easingType,
            EasingMode easingMode)
        {
            var scaleX = targetActualSize.X / sourceActualSize.X;
            var scale = new Vector3(scaleX, scaleX, 1);
            this.AnimateScale(sourceBuilder, targetBuilder, scale, duration, easingType, easingMode);
        }

        private void AnimateScaleY(
            AnimationBuilder sourceBuilder,
            AnimationBuilder targetBuilder,
            Vector2 sourceActualSize,
            Vector2 targetActualSize,
            TimeSpan duration,
            EasingType easingType,
            EasingMode easingMode)
        {
            var scaleY = targetActualSize.Y / sourceActualSize.Y;
            var scale = new Vector3(scaleY, scaleY, 1);
            this.AnimateScale(sourceBuilder, targetBuilder, scale, duration, easingType, easingMode);
        }

        private void AnimateScale(
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

        private void AnimateOpacity(
            AnimationBuilder sourceBuilder,
            AnimationBuilder targetBuilder,
            TimeSpan duration)
        {
            if (this._isInterruptedAnimation)
            {
                _ = sourceBuilder.Opacity(0, duration: almostZeroDuration);
                _ = targetBuilder.Opacity(1, duration: almostZeroDuration);
                return;
            }

            var useDuration = TimeSpan.FromMilliseconds(Math.Min(200, duration.TotalMilliseconds / 3));
            _ = sourceBuilder.Opacity().TimedKeyFrames(
                b => b
                    .KeyFrame(TimeSpan.Zero, 1)
                    .KeyFrame(useDuration, 0, EasingType.Cubic, EasingMode.EaseIn));
            _ = targetBuilder.Opacity().TimedKeyFrames(
                b => b
                    .KeyFrame(TimeSpan.Zero, 0)
                    .KeyFrame(useDuration, 1, EasingType.Cubic, EasingMode.EaseOut));
        }

        private void AnimateClip(
            AnimationBuilder sourceBuilder,
            AnimationBuilder targetBuilder,
            Vector2 sourceActualSize,
            Vector2 targetActualSize,
            Vector2 sourceCenterPoint,
            Vector2 targetCenterPoint,
            TimeSpan duration,
            EasingType easingType,
            EasingMode easingMode,
            Axis? axis)
        {
            // -4 is used to prevent shadows from being cropped.
            var defaultValue = -4;
            var defaultThickness = new Thickness(defaultValue);
            if (this._isInterruptedAnimation)
            {
                _ = sourceBuilder.Clip(defaultThickness, duration: almostZeroDuration);
                _ = targetBuilder.Clip(defaultThickness, duration: duration, easingType: easingType, easingMode: easingMode);
                return;
            }

            var left = axis == Axis.X
                ? Math.Max(defaultValue, targetCenterPoint.X - sourceCenterPoint.X)
                : defaultValue;
            var top = axis == Axis.Y
                ? Math.Max(defaultValue, targetCenterPoint.Y - sourceCenterPoint.Y)
                : defaultValue;
            var right = axis == Axis.X
                ? Math.Max(defaultValue, targetActualSize.X - sourceActualSize.X - left)
                : defaultValue;
            var bottom = axis == Axis.Y
                ? Math.Max(defaultValue, targetActualSize.Y - sourceActualSize.Y - top)
                : defaultValue;

            _ = sourceBuilder.Clip(
                new Thickness(left, top, right, bottom),
                duration: duration,
                easingType: easingType,
                easingMode: easingMode);
            _ = targetBuilder.Clip(
                defaultThickness,
                new Thickness(left, top, right, bottom),
                duration: duration,
                easingType: easingType,
                easingMode: easingMode);
        }
    }
}
