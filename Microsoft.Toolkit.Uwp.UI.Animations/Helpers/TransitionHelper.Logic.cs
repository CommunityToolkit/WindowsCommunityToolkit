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

        private async Task StartInterruptibleAnimationsAsync(bool reversed, CancellationToken token, float? startProgress, TimeSpan totalDuration)
        {
            this._lastAnimationDuration = totalDuration * (1 - startProgress);
            _lastAnimationStartTime = DateTime.Now;

            await this.AnimateControls(totalDuration, startProgress, reversed, token);
            if (token.IsCancellationRequested)
            {
                return;
            }

            this.RestoreState(!reversed);
            this._lastAnimationStartTime = null;
            this._lastAnimationDuration = null;
            this._lastStartProgress = null;
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

        private Task AnimateControls(TimeSpan duration, float? startProgress, bool reversed, CancellationToken token)
        {
            var animationTasks = new List<Task>(3);
            var sourceUnpairedElements = this.sourceConnectedAnimatedElements
                .Where(item => !this.targetConnectedAnimatedElements.ContainsKey(item.Key))
                .Select(item => item.Value);
            var targetUnpairedElements = this.targetConnectedAnimatedElements
                .Where(item => !this.sourceConnectedAnimatedElements.ContainsKey(item.Key))
                .Select(item => item.Value);
            var pairedElementKeys = this.sourceConnectedAnimatedElements
                .Where(item => this.targetConnectedAnimatedElements.ContainsKey(item.Key))
                .Select(item => item.Key);
            if (_currentAnimationGroupController is null)
            {
                _currentAnimationGroupController = new KeyFrameAnimationGroupController();
                foreach (var key in pairedElementKeys)
                {
                    var source = this.sourceConnectedAnimatedElements[key];
                    var target = this.targetConnectedAnimatedElements[key];
                    var animationConfig = this.Configs.FirstOrDefault(config => config.Id == key) ??
                                          this.DefaultConfig;
                    this.BuildConnectedAnimationController(
                        _currentAnimationGroupController,
                        source,
                        target,
                        duration,
                        animationConfig);
                }
            }

            animationTasks.Add(reversed
                        ? _currentAnimationGroupController.ReverseAsync(token, duration, startProgress)
                        : _currentAnimationGroupController.StartAsync(token, duration, startProgress));

            animationTasks.Add(
                this.AnimateIndependentElements(
                    this.sourceIndependentAnimatedElements.Concat(sourceUnpairedElements),
                    reversed,
                    token,
                    duration,
                    startProgress,
                    IndependentElementEasingType,
                    IndependentElementEasingMode));
            animationTasks.Add(
                this.AnimateIndependentElements(
                    this.targetIndependentAnimatedElements.Concat(targetUnpairedElements),
                    !reversed,
                    token,
                    duration,
                    startProgress,
                    IndependentElementEasingType,
                    IndependentElementEasingMode));

            return Task.WhenAll(animationTasks);
        }

        private void BuildConnectedAnimationController(KeyFrameAnimationGroupController controller, UIElement source, UIElement target, TimeSpan duration, TransitionConfig config)
        {
            var sourceActualSize = source is FrameworkElement sourceElement ? new Vector2((float)sourceElement.ActualWidth, (float)sourceElement.ActualHeight) : source.ActualSize;
            var targetActualSize = target is FrameworkElement targetElement ? new Vector2((float)targetElement.ActualWidth, (float)targetElement.ActualHeight) : target.ActualSize;
            var sourceCenterPoint = sourceActualSize * config.NormalizedCenterPoint.ToVector2();
            var targetCenterPoint = targetActualSize * config.NormalizedCenterPoint.ToVector2();

            var currentSourceScale = GetXY(source.GetVisual().Scale);

            source.GetVisual().CenterPoint = new Vector3(sourceCenterPoint, 0);
            target.GetVisual().CenterPoint = new Vector3(targetCenterPoint, 0);
            var (sourceTranslationAnimation, targetTranslationAnimation) = this.AnimateTranslation(
                source,
                target,
                sourceCenterPoint,
                targetCenterPoint,
                currentSourceScale,
                duration,
                config.EasingType,
                config.EasingMode);
            var (sourceOpacityAnimation, targetOpacityAnimation) = this.AnimateOpacity(duration);
            var (sourceScaleAnimation, targetScaleAnimation, targetScale) = config.ScaleMode switch
            {
                ScaleMode.None => (null, null, Vector2.One),
                ScaleMode.Scale => this.AnimateScale(
                    sourceActualSize,
                    targetActualSize,
                    currentSourceScale,
                    duration,
                    config.EasingType,
                    config.EasingMode),
                ScaleMode.ScaleX => this.AnimateScaleX(
                    sourceActualSize,
                    targetActualSize,
                    currentSourceScale,
                    duration,
                    config.EasingType,
                    config.EasingMode),
                ScaleMode.ScaleY => this.AnimateScaleY(
                    sourceActualSize,
                    targetActualSize,
                    currentSourceScale,
                    duration,
                    config.EasingType,
                    config.EasingMode),
                _ => (null, null, Vector2.One),
            };

            controller.AddAnimationGroupFor(
                source,
                new[]
                {
                    sourceTranslationAnimation,
                    sourceOpacityAnimation,
                    sourceScaleAnimation
                });

            controller.AddAnimationGroupFor(
                target,
                new[]
                {
                    targetTranslationAnimation,
                    targetOpacityAnimation,
                    targetScaleAnimation
                });

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
                var (sourceClipAnimationGroup, targetClipAnimationGroup) = this.AnimateClip(
                    sourceActualSize,
                    targetActualSize,
                    sourceCenterPoint,
                    targetCenterPoint,
                    targetScale,
                    duration,
                    config.EasingType,
                    config.EasingMode,
                    axis);
                controller.AddAnimationGroupFor(source, sourceClipAnimationGroup);
                controller.AddAnimationGroupFor(target, targetClipAnimationGroup);
            }
        }

        private Task AnimateIndependentElements(
            IEnumerable<UIElement> independentElements,
            bool isShow,
            CancellationToken token,
            TimeSpan totalDuration,
            float? startProgress,
            EasingType easingType,
            EasingMode easingMode)
        {
            if (independentElements?.ToArray() is not { Length: > 0 } elements)
            {
                return Task.CompletedTask;
            }

            var startTime = TimeSpan.Zero;
            if (startProgress.HasValue)
            {
                startTime = totalDuration * startProgress.Value;
            }

            var controller = new KeyFrameAnimationGroupController();

            var duration = isShow ? this.IndependentElementShowDuration : this.IndependentElementHideDuration;
            var delay = isShow ? this.IndependentElementShowDelay : TimeSpan.Zero;
            var translationFrom = isShow ? this.IndependentElementHideTranslation.ToVector2() : Vector2.Zero;
            var translationTo = isShow ? Vector2.Zero : this.IndependentElementHideTranslation.ToVector2();
            var opacityFrom = isShow ? 0 : 1;
            var opacityTo = isShow ? 1 : 0;
            if (this._isInterruptedAnimation)
            {
                duration *= InterruptedAnimationReverseDurationRatio;
                delay *= InterruptedAnimationReverseDurationRatio;
            }

            foreach (var item in elements)
            {
                if (delay < startTime)
                {
                    if (isShow)
                    {
                        RestoreElement(item);
                    }
                    else
                    {
                        item.GetVisual().Opacity = 0;
                    }
                }
                else
                {
                    var useDelay = delay - startTime;
                    if (Math.Abs(this.IndependentElementHideTranslation.X) > 0.01 ||
                    Math.Abs(this.IndependentElementHideTranslation.Y) > 0.01)
                    {
                        controller.AddAnimationFor(item, this.Translation(
                            translationTo,
                            this._isInterruptedAnimation ? null : translationFrom,
                            useDelay,
                            duration: duration,
                            easingType: easingType,
                            easingMode: easingMode));
                    }

                    controller.AddAnimationFor(item, this.Opacity(
                        opacityTo,
                        this._isInterruptedAnimation ? null : opacityFrom,
                        delay,
                        duration,
                        easingType: easingType,
                        easingMode: easingMode));
                }

                if (isShow)
                {
                    delay += this.IndependentElementShowInterval;
                }
            }

            return controller.StartAsync(token, null, null);
        }

        private (IKeyFrameCompositionAnimationFactory, IKeyFrameCompositionAnimationFactory) AnimateTranslation(
            UIElement source,
            UIElement target,
            Vector2 sourceCenterPoint,
            Vector2 targetCenterPoint,
            Vector2 initialScale,
            TimeSpan duration,
            EasingType easingType,
            EasingMode easingMode)
        {
            var diff = ((target.TransformToVisual(source).TransformPoint(default).ToVector2() - sourceCenterPoint) * initialScale) + targetCenterPoint;
            return (this.Translation(diff, Vector2.Zero, duration: duration, easingType: easingType, easingMode: easingMode),
                this.Translation(Vector2.Zero, -diff, duration: duration, easingType: easingType, easingMode: easingMode));
        }

        private (IKeyFrameCompositionAnimationFactory, IKeyFrameCompositionAnimationFactory, Vector2) AnimateScale(
            Vector2 sourceActualSize,
            Vector2 targetActualSize,
            Vector2 initialScale,
            TimeSpan duration,
            EasingType easingType,
            EasingMode easingMode)
        {
            var scaleX = targetActualSize.X / sourceActualSize.X;
            var scaleY = targetActualSize.Y / sourceActualSize.Y;
            var scale = new Vector2(scaleX, scaleY);
            var (sourceFactory, targetFactory) = this.AnimateScaleImp(scale, initialScale, duration, easingType, easingMode);
            return (sourceFactory, targetFactory, scale);
        }

        private (IKeyFrameCompositionAnimationFactory, IKeyFrameCompositionAnimationFactory, Vector2) AnimateScaleX(
            Vector2 sourceActualSize,
            Vector2 targetActualSize,
            Vector2 initialScale,
            TimeSpan duration,
            EasingType easingType,
            EasingMode easingMode)
        {
            var scaleX = targetActualSize.X / sourceActualSize.X;
            var scale = new Vector2(scaleX, scaleX);
            var (sourceFactory, targetFactory) = this.AnimateScaleImp(scale, initialScale, duration, easingType, easingMode);
            return (sourceFactory, targetFactory, scale);
        }

        private (IKeyFrameCompositionAnimationFactory, IKeyFrameCompositionAnimationFactory, Vector2) AnimateScaleY(
            Vector2 sourceActualSize,
            Vector2 targetActualSize,
            Vector2 initialScale,
            TimeSpan duration,
            EasingType easingType,
            EasingMode easingMode)
        {
            var scaleY = targetActualSize.Y / sourceActualSize.Y;
            var scale = new Vector2(scaleY, scaleY);
            var (sourceFactory, targetFactory) = this.AnimateScaleImp(scale, initialScale, duration, easingType, easingMode);
            return (sourceFactory, targetFactory, scale);
        }

        private (IKeyFrameCompositionAnimationFactory, IKeyFrameCompositionAnimationFactory) AnimateScaleImp(
            Vector2 targetScale,
            Vector2 initialScale,
            TimeSpan duration,
            EasingType easingType,
            EasingMode easingMode)
        {
            return (this.Scale(targetScale, initialScale, duration: duration, easingType: easingType, easingMode: easingMode),
                this.Scale(Vector2.One, GetInverseScale(targetScale / initialScale), duration: duration, easingType: easingType, easingMode: easingMode));
        }

        private (IKeyFrameCompositionAnimationFactory, IKeyFrameCompositionAnimationFactory) AnimateOpacity(TimeSpan duration)
        {
            var useDuration = TimeSpan.FromMilliseconds(Math.Min(200, duration.TotalMilliseconds / 3));
            var normalizedProgress = (float)(useDuration / duration);
            var sourceNormalizedKeyFrames = new Dictionary<float, (float, EasingType?, EasingMode?)>
            {
                [normalizedProgress] = (0, EasingType.Cubic, EasingMode.EaseIn)
            };
            var targetNormalizedKeyFrames = new Dictionary<float, (float, EasingType?, EasingMode?)>
            {
                [normalizedProgress] = (1, EasingType.Cubic, EasingMode.EaseOut)
            };
            return (this.Opacity(0, 1, duration: duration, normalizedKeyFrames: sourceNormalizedKeyFrames),
                this.Opacity(1, 0, duration: duration, normalizedKeyFrames: targetNormalizedKeyFrames));
        }

        private (IKeyFrameCompositionAnimationFactory[], IKeyFrameCompositionAnimationFactory[]) AnimateClip(
            Vector2 sourceActualSize,
            Vector2 targetActualSize,
            Vector2 sourceCenterPoint,
            Vector2 targetCenterPoint,
            Vector2 targetScale,
            TimeSpan duration,
            EasingType easingType,
            EasingMode easingMode,
            Axis? axis)
        {
            // -4 is used to prevent shadows from being cropped.
            var defaultValue = -4;
            var defaultThickness = new Thickness(defaultValue);
            var inverseScale = GetInverseScale(targetScale);

            var sourceEndViewportLeft = (axis is Axis.Y ? -sourceCenterPoint.X * targetScale.X : -targetCenterPoint.X) + (sourceCenterPoint.X * targetScale.X);
            var sourceEndViewportTop = (axis is Axis.X ? -sourceCenterPoint.Y * targetScale.Y : -targetCenterPoint.Y) + (sourceCenterPoint.Y * targetScale.Y);
            var sourceEndViewportRight = (axis is Axis.Y ? (sourceActualSize.X - sourceCenterPoint.X) * targetScale.X : targetActualSize.X - targetCenterPoint.X) + (sourceCenterPoint.X * targetScale.X);
            var sourceEndViewportBottom = (axis is Axis.X ? (sourceActualSize.Y - sourceCenterPoint.Y) * targetScale.Y : targetActualSize.Y - targetCenterPoint.Y) + (sourceCenterPoint.Y * targetScale.Y);

            var targetBeginViewportLeft = (axis is Axis.Y ? -targetCenterPoint.X * inverseScale.X : -sourceCenterPoint.X) + (targetCenterPoint.X * inverseScale.X);
            var targetBeginViewportTop = (axis is Axis.X ? -targetCenterPoint.Y * inverseScale.Y : -sourceCenterPoint.Y) + (targetCenterPoint.Y * inverseScale.Y);
            var targetBeginViewportRight = (axis is Axis.Y ? (targetActualSize.X - targetCenterPoint.X) * inverseScale.X : sourceActualSize.X - sourceCenterPoint.X) + (targetCenterPoint.X * inverseScale.X);
            var targetBeginViewportBottom = (axis is Axis.X ? (targetActualSize.Y - targetCenterPoint.Y) * inverseScale.Y : sourceActualSize.Y - sourceCenterPoint.Y) + (targetCenterPoint.Y * inverseScale.Y);

            var sourceLeftTop = new Vector2(sourceEndViewportLeft, sourceEndViewportTop) * inverseScale;
            var sourceRightBottom = new Vector2(sourceEndViewportRight, sourceEndViewportBottom) * inverseScale;
            var sourceRight = sourceActualSize.X - sourceRightBottom.X;
            var sourceBottom = sourceActualSize.Y - sourceRightBottom.Y;

            var targetLeftTop = new Vector2(targetBeginViewportLeft, targetBeginViewportTop) * targetScale;
            var targetRightBottom = new Vector2(targetBeginViewportRight, targetBeginViewportBottom) * targetScale;
            var targetRight = targetActualSize.X - targetRightBottom.X;
            var targetBottom = targetActualSize.Y - targetRightBottom.Y;

            return (
                this.Clip(
                    GetFixedThickness(new Thickness(sourceLeftTop.X, sourceLeftTop.Y, sourceRight, sourceBottom), defaultValue),
                    from: defaultThickness,
                    duration: duration,
                    easingType: easingType,
                    easingMode: easingMode),
                this.Clip(
                    defaultThickness,
                    GetFixedThickness(new Thickness(targetLeftTop.X, targetLeftTop.Y, targetRight, targetBottom), defaultValue),
                    duration: duration,
                    easingType: easingType,
                    easingMode: easingMode)
                );
        }
    }
}
