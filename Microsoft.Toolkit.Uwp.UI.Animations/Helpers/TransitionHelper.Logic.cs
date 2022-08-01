// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading;
using System.Threading.Tasks;
using Windows.Foundation;
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

        private void RestoreState(bool isTargetState)
        {
            this.IsTargetState = isTargetState;
            if(this.Source is not null)
            {
                Canvas.SetZIndex(this.Source, _sourceZIndex);
                ToggleVisualState(this.Source, this.SourceToggleMethod, !isTargetState);
            }

            if(this.Target is not null)
            {
                Canvas.SetZIndex(this.Target, _targetZIndex);
                ToggleVisualState(this.Target, this.TargetToggleMethod, isTargetState);
            }

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

        private Task AnimateControls(TimeSpan duration, bool reversed, CancellationToken token)
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

            TimeSpan? startTime = null;
            if (_currentAnimationGroupController.LastStopProgress.HasValue)
            {
                var startProgress = reversed ? (1 - _currentAnimationGroupController.LastStopProgress.Value) : _currentAnimationGroupController.LastStopProgress.Value;
                startTime = startProgress * duration;
            }

            animationTasks.Add(reversed
                        ? _currentAnimationGroupController.ReverseAsync(token, this.InverseEasingFunctionWhenReversing, duration)
                        : _currentAnimationGroupController.StartAsync(token, duration));

            animationTasks.Add(
                this.AnimateIndependentElements(
                    this.sourceIndependentAnimatedElements.Concat(sourceUnpairedElements),
                    reversed,
                    token,
                    startTime,
                    IndependentElementEasingType,
                    IndependentElementEasingMode));
            animationTasks.Add(
                this.AnimateIndependentElements(
                    this.targetIndependentAnimatedElements.Concat(targetUnpairedElements),
                    !reversed,
                    token,
                    startTime,
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

            source.GetVisual().CenterPoint = new Vector3(sourceCenterPoint, 0);
            target.GetVisual().CenterPoint = new Vector3(targetCenterPoint, 0);
            var easingType = config.EasingType ?? this.DefaultEasingType;
            var easingMode = config.EasingMode ?? this.DefaultEasingMode;
            var (sourceTranslationAnimation, targetTranslationAnimation) = this.AnimateTranslation(
                source,
                target,
                sourceCenterPoint,
                targetCenterPoint,
                duration,
                easingType,
                easingMode);
            var (sourceOpacityAnimation, targetOpacityAnimation) = this.AnimateOpacity(duration, this.OpacityTransitionProgressKey);
            var (sourceScaleAnimation, targetScaleAnimation, targetScale) = config.ScaleMode switch
            {
                ScaleMode.None => (null, null, Vector2.One),
                ScaleMode.Scale => this.AnimateScale(
                    sourceActualSize,
                    targetActualSize,
                    duration,
                    easingType,
                    easingMode),
                ScaleMode.ScaleX => this.AnimateScaleX(
                    sourceActualSize,
                    targetActualSize,
                    duration,
                    easingType,
                    easingMode),
                ScaleMode.ScaleY => this.AnimateScaleY(
                    sourceActualSize,
                    targetActualSize,
                    duration,
                    easingType,
                    easingMode),
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
            if (config.EnableClipAnimation)
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
                    easingType,
                    easingMode,
                    axis);
                controller.AddAnimationGroupFor(source, sourceClipAnimationGroup);
                controller.AddAnimationGroupFor(target, targetClipAnimationGroup);
            }
        }

        private Task AnimateIndependentElements(
            IEnumerable<UIElement> independentElements,
            bool isShow,
            CancellationToken token,
            TimeSpan? startTime,
            EasingType easingType,
            EasingMode easingMode)
        {
            if (independentElements?.ToArray() is not { Length: > 0 } elements)
            {
                return Task.CompletedTask;
            }

            var controller = new KeyFrameAnimationGroupController();
            var duration = isShow ? this.IndependentElementShowDuration : this.IndependentElementHideDuration;
            var delay = isShow ? this.IndependentElementShowDelay : TimeSpan.Zero;
            var opacityFrom = isShow ? 0 : 1;
            var opacityTo = isShow ? 1 : 0;
            foreach (var item in elements)
            {
                if (startTime.HasValue && delay < startTime)
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
                    var independentTranslation = GetIndependentTranslation(item) ?? this.DefaultIndependentTranslation;
                    var translationFrom = isShow ? independentTranslation.ToVector2() : Vector2.Zero;
                    var translationTo = isShow ? Vector2.Zero : independentTranslation.ToVector2();
                    var useDelay = delay - (startTime ?? TimeSpan.Zero);
                    if (Math.Abs(independentTranslation.X) > AlmostZero ||
                        Math.Abs(independentTranslation.Y) > AlmostZero)
                    {
                        controller.AddAnimationFor(item, this.Translation(
                            translationTo,
                            startTime.HasValue ? null : translationFrom,
                            useDelay,
                            duration: duration,
                            easingType: easingType,
                            easingMode: easingMode));
                    }

                    controller.AddAnimationFor(item, this.Opacity(
                        opacityTo,
                        startTime.HasValue ? null : opacityFrom,
                        useDelay,
                        duration,
                        easingType: easingType,
                        easingMode: easingMode));
                }

                if (isShow)
                {
                    delay += this.IndependentElementShowInterval;
                }
            }

            return controller.StartAsync(token);
        }

        private (IKeyFrameCompositionAnimationFactory, IKeyFrameCompositionAnimationFactory) AnimateTranslation(
            UIElement source,
            UIElement target,
            Vector2 sourceCenterPoint,
            Vector2 targetCenterPoint,
            TimeSpan duration,
            EasingType easingType,
            EasingMode easingMode)
        {
            var diff = target.TransformToVisual(source).TransformPoint(default).ToVector2() - sourceCenterPoint + targetCenterPoint;
            return (this.Translation(diff, Vector2.Zero, duration: duration, easingType: easingType, easingMode: easingMode),
                this.Translation(Vector2.Zero, -diff, duration: duration, easingType: easingType, easingMode: easingMode));
        }

        private (IKeyFrameCompositionAnimationFactory, IKeyFrameCompositionAnimationFactory, Vector2) AnimateScale(
            Vector2 sourceActualSize,
            Vector2 targetActualSize,
            TimeSpan duration,
            EasingType easingType,
            EasingMode easingMode)
        {
            var scaleX = targetActualSize.X / sourceActualSize.X;
            var scaleY = targetActualSize.Y / sourceActualSize.Y;
            var scale = new Vector2(scaleX, scaleY);
            var (sourceFactory, targetFactory) = this.AnimateScaleImp(scale, duration, easingType, easingMode);
            return (sourceFactory, targetFactory, scale);
        }

        private (IKeyFrameCompositionAnimationFactory, IKeyFrameCompositionAnimationFactory, Vector2) AnimateScaleX(
            Vector2 sourceActualSize,
            Vector2 targetActualSize,
            TimeSpan duration,
            EasingType easingType,
            EasingMode easingMode)
        {
            var scaleX = targetActualSize.X / sourceActualSize.X;
            var scale = new Vector2(scaleX, scaleX);
            var (sourceFactory, targetFactory) = this.AnimateScaleImp(scale, duration, easingType, easingMode);
            return (sourceFactory, targetFactory, scale);
        }

        private (IKeyFrameCompositionAnimationFactory, IKeyFrameCompositionAnimationFactory, Vector2) AnimateScaleY(
            Vector2 sourceActualSize,
            Vector2 targetActualSize,
            TimeSpan duration,
            EasingType easingType,
            EasingMode easingMode)
        {
            var scaleY = targetActualSize.Y / sourceActualSize.Y;
            var scale = new Vector2(scaleY, scaleY);
            var (sourceFactory, targetFactory) = this.AnimateScaleImp(scale, duration, easingType, easingMode);
            return (sourceFactory, targetFactory, scale);
        }

        private (IKeyFrameCompositionAnimationFactory, IKeyFrameCompositionAnimationFactory) AnimateScaleImp(
            Vector2 targetScale,
            TimeSpan duration,
            EasingType easingType,
            EasingMode easingMode)
        {
            return (this.Scale(targetScale, Vector2.One, duration: duration, easingType: easingType, easingMode: easingMode),
                this.Scale(Vector2.One, GetInverseScale(targetScale), duration: duration, easingType: easingType, easingMode: easingMode));
        }

        private (IKeyFrameCompositionAnimationFactory, IKeyFrameCompositionAnimationFactory) AnimateOpacity(TimeSpan duration, Point opacityTransitionProgressKey)
        {
            var normalKey = (float)Math.Max(0, Math.Min(opacityTransitionProgressKey.X, 1));
            var reversedKey = (float)Math.Max(0, Math.Min(1 - opacityTransitionProgressKey.Y, 1));
            var sourceNormalizedKeyFrames = new Dictionary<float, (float, EasingType?, EasingMode?)>
            {
                [normalKey] = (0, EasingType.Cubic, EasingMode.EaseIn)
            };
            var reversedSourceNormalizedKeyFrames = new Dictionary<float, (float, EasingType?, EasingMode?)>
            {
                [reversedKey] = (1, null, null),
                [1] = (0, EasingType.Cubic, EasingMode.EaseOut)
            };
            var targetNormalizedKeyFrames = new Dictionary<float, (float, EasingType?, EasingMode?)>
            {
                [normalKey] = (1, EasingType.Cubic, EasingMode.EaseOut)
            };
            var reversedTargetNormalizedKeyFrames = new Dictionary<float, (float, EasingType?, EasingMode?)>
            {
                [reversedKey] = (0, null, null),
                [1] = (1, EasingType.Cubic, EasingMode.EaseIn)
            };
            return (this.Opacity(0, 1, duration: duration, normalizedKeyFrames: sourceNormalizedKeyFrames, reversedNormalizedKeyFrames: reversedSourceNormalizedKeyFrames),
                this.Opacity(1, 0, duration: duration, normalizedKeyFrames: targetNormalizedKeyFrames, reversedNormalizedKeyFrames: reversedTargetNormalizedKeyFrames));
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
