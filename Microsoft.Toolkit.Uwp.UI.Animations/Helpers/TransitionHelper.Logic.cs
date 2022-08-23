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
            UpdateAnimatedElements(this.Source, this.sourceConnectedElements, this.sourceCoordinatedElements, this.sourceIndependentElements);
        }

        private void UpdateTargetAnimatedElements()
        {
            UpdateAnimatedElements(this.Target, this.targetConnectedElements, this.targetCoordinatedElements, this.targetIndependentElements);
        }

        private void UpdateAnimatedElements(DependencyObject parent, IDictionary<string, UIElement> connectedElements, IDictionary<string, List<UIElement>> coordinatedElements, ICollection<UIElement> independentElements)
        {
            connectedElements.Clear();
            coordinatedElements.Clear();
            independentElements.Clear();

            if (parent is null)
            {
                return;
            }

            foreach (var item in GetAnimatedElements(parent))
            {
                if (GetId(item) is { } id)
                {
                    connectedElements[id] = item;
                }
                else if (GetCoordinatedTarget(item) is { } targetId)
                {
                    if (coordinatedElements.ContainsKey(targetId) is false)
                    {
                        coordinatedElements[targetId] = new List<UIElement> { item };
                    }
                    else
                    {
                        coordinatedElements[targetId].Add(item);
                    }
                }
                else
                {
                    independentElements.Add(item);
                }
            }
        }

        private void RestoreState(bool isTargetState)
        {
            this.IsTargetState = isTargetState;
            if (this.Source is not null)
            {
                Canvas.SetZIndex(this.Source, _sourceZIndex);
                ToggleVisualState(this.Source, this.SourceToggleMethod, !isTargetState);
            }

            if (this.Target is not null)
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
                    updateLayoutTask = UpdateControlLayout(target);
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

        private Task AnimateControls(TimeSpan duration, bool reversed, CancellationToken token)
        {
            var animationTasks = new List<Task>(3);
            var sourceUnpairedElements = this.sourceConnectedElements
                .Where(item => !this.targetConnectedElements.ContainsKey(item.Key))
                .SelectMany(item =>
                {
                    var result = new List<UIElement>(1) { item.Value };
                    if (this.sourceCoordinatedElements.TryGetValue(item.Key, out var coordinatedElements))
                    {
                        return result.Concat(coordinatedElements);
                    }

                    return result;
                });
            var targetUnpairedElements = this.targetConnectedElements
                .Where(item => !this.sourceConnectedElements.ContainsKey(item.Key))
                .SelectMany(item =>
                {
                    var result = new List<UIElement>(1) { item.Value };
                    if (this.targetCoordinatedElements.TryGetValue(item.Key, out var coordinatedElements))
                    {
                        return result.Concat(coordinatedElements);
                    }

                    return result;
                });
            var pairedElementKeys = this.sourceConnectedElements
                .Where(item => this.targetConnectedElements.ContainsKey(item.Key))
                .Select(item => item.Key);
            if (_currentAnimationGroupController is null)
            {
                _currentAnimationGroupController = new KeyFrameAnimationGroupController();
                foreach (var key in pairedElementKeys)
                {
                    var source = this.sourceConnectedElements[key];
                    var target = this.targetConnectedElements[key];
                    var animationConfig = this.Configs.FirstOrDefault(config => config.Id == key) ??
                                          this.DefaultConfig;
                    this.sourceCoordinatedElements.TryGetValue(key, out var sourceAttachedElements);
                    this.targetCoordinatedElements.TryGetValue(key, out var targetAttachedElements);
                    this.BuildConnectedAnimationController(
                        _currentAnimationGroupController,
                        source,
                        target,
                        sourceAttachedElements,
                        targetAttachedElements,
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
                    this.sourceIndependentElements.Concat(sourceUnpairedElements),
                    reversed,
                    token,
                    startTime,
                    IndependentElementEasingType,
                    IndependentElementEasingMode));
            animationTasks.Add(
                this.AnimateIndependentElements(
                    this.targetIndependentElements.Concat(targetUnpairedElements),
                    !reversed,
                    token,
                    startTime,
                    IndependentElementEasingType,
                    IndependentElementEasingMode));

            return Task.WhenAll(animationTasks);
        }

        private void BuildConnectedAnimationController(
            IKeyFrameAnimationGroupController controller,
            UIElement source,
            UIElement target,
            List<UIElement> sourceAttachedElements,
            List<UIElement> targetAttachedElements,
            TimeSpan duration,
            TransitionConfig config)
        {
            var sourceActualSize = source is FrameworkElement sourceElement ? new Vector2((float)sourceElement.ActualWidth, (float)sourceElement.ActualHeight) : source.ActualSize;
            var targetActualSize = target is FrameworkElement targetElement ? new Vector2((float)targetElement.ActualWidth, (float)targetElement.ActualHeight) : target.ActualSize;
            var sourceCenterPoint = sourceActualSize * config.NormalizedCenterPoint.ToVector2();
            var targetCenterPoint = targetActualSize * config.NormalizedCenterPoint.ToVector2();

            source.GetVisual().CenterPoint = new Vector3(sourceCenterPoint, 0);
            target.GetVisual().CenterPoint = new Vector3(targetCenterPoint, 0);
            var easingType = config.EasingType ?? this.DefaultEasingType;
            var easingMode = config.EasingMode ?? this.DefaultEasingMode;
            var (sourceTranslationAnimation, targetTranslationAnimation, sourceTargetTranslation) = this.AnimateTranslation(
                source,
                target,
                sourceCenterPoint,
                targetCenterPoint,
                duration,
                easingType,
                easingMode);
            var (sourceOpacityAnimation, targetOpacityAnimation) = this.AnimateOpacity(duration, this.OpacityTransitionProgressKey);
            var (sourceScaleAnimation, targetScaleAnimation, sourceTargetScale) = config.ScaleMode switch
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
            if (sourceAttachedElements?.Count > 0)
            {
                var targetControlBounds = new Rect(0, 0, this.Target.ActualWidth, this.Target.ActualHeight);
                var targetTransformedBounds = this.Target.TransformToVisual(this.Source).TransformBounds(targetControlBounds);
                var targetScale = sourceTargetScale;
                foreach (var coordinatedElement in sourceAttachedElements)
                {
                    var coordinatedElementActualSize = coordinatedElement is FrameworkElement coordinatedFrameworkElement ? new Vector2((float)coordinatedFrameworkElement.ActualWidth, (float)coordinatedFrameworkElement.ActualHeight) : coordinatedElement.ActualSize;
                    coordinatedElement.GetVisual().CenterPoint = new Vector3(coordinatedElementActualSize * config.NormalizedCenterPoint.ToVector2(), 0);
                    controller.AddAnimationGroupFor(
                        coordinatedElement,
                        new[]
                        {
                            sourceTranslationAnimation,
                            sourceOpacityAnimation,
                            sourceScaleAnimation
                        });
                    var location = coordinatedElement.TransformToVisual(this.Source).TransformPoint(sourceTargetTranslation.ToPoint());
                    var targetClip = GetCoordinatedElementClip(targetScale, location, (coordinatedElementActualSize * targetScale).ToSize(), targetTransformedBounds);
                    if (targetClip.HasValue)
                    {
                        controller.AddAnimationGroupFor(
                            coordinatedElement,
                            this.Clip(
                                targetClip.Value,
                                duration: duration,
                                easingType: easingType,
                                easingMode: easingMode));
                    }
                }
            }

            if (targetAttachedElements?.Count > 0)
            {
                var sourceControlBounds = new Rect(0, 0, this.Source.ActualWidth, this.Source.ActualHeight);
                var sourceTransformedBounds = this.Source.TransformToVisual(this.Target).TransformBounds(sourceControlBounds);
                var targetScale = GetInverseScale(sourceTargetScale);
                foreach (var coordinatedElement in targetAttachedElements)
                {
                    var coordinatedElementActualSize = coordinatedElement is FrameworkElement coordinatedFrameworkElement ? new Vector2((float)coordinatedFrameworkElement.ActualWidth, (float)coordinatedFrameworkElement.ActualHeight) : coordinatedElement.ActualSize;
                    coordinatedElement.GetVisual().CenterPoint = new Vector3(coordinatedElementActualSize * config.NormalizedCenterPoint.ToVector2(), 0);
                    controller.AddAnimationGroupFor(
                        coordinatedElement,
                        new[]
                        {
                            targetTranslationAnimation,
                            targetOpacityAnimation,
                            targetScaleAnimation
                        });
                    var location = coordinatedElement.TransformToVisual(this.Target).TransformPoint((-sourceTargetTranslation).ToPoint());
                    var targetClip = GetCoordinatedElementClip(targetScale, location, (coordinatedElementActualSize * targetScale).ToSize(), sourceTransformedBounds);
                    if (targetClip.HasValue)
                    {
                        controller.AddAnimationGroupFor(
                            coordinatedElement,
                            this.Clip(
                                default,
                                from: targetClip.Value,
                                duration: duration,
                                easingType: easingType,
                                easingMode: easingMode));
                    }
                }
            }

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
                    sourceTargetScale,
                    duration,
                    easingType,
                    easingMode,
                    axis);
                if (sourceClipAnimationGroup is not null)
                {
                    controller.AddAnimationGroupFor(source, sourceClipAnimationGroup);
                }

                if (targetClipAnimationGroup is not null)
                {
                    controller.AddAnimationGroupFor(target, targetClipAnimationGroup);
                }
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

            return controller.StartAsync(token, null);
        }

        private (IKeyFrameCompositionAnimationFactory, IKeyFrameCompositionAnimationFactory, Vector2) AnimateTranslation(
            UIElement source,
            UIElement target,
            Vector2 sourceCenterPoint,
            Vector2 targetCenterPoint,
            TimeSpan duration,
            EasingType easingType,
            EasingMode easingMode)
        {
            var translation = target.TransformToVisual(source).TransformPoint(default).ToVector2() - sourceCenterPoint + targetCenterPoint;
            return (this.Translation(translation, Vector2.Zero, duration: duration, easingType: easingType, easingMode: easingMode),
                this.Translation(Vector2.Zero, -translation, duration: duration, easingType: easingType, easingMode: easingMode),
                translation);
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
            Vector2 sourceTargetScale,
            TimeSpan duration,
            EasingType easingType,
            EasingMode easingMode,
            Axis? axis)
        {
            var sourceToClip = GetConnectedElementClip(axis, sourceTargetScale, sourceActualSize, sourceCenterPoint, new Rect((-targetCenterPoint).ToPoint(), targetActualSize.ToSize()));
            var targetFromClip = GetConnectedElementClip(axis, GetInverseScale(sourceTargetScale), targetActualSize, targetCenterPoint, new Rect((-sourceCenterPoint).ToPoint(), sourceActualSize.ToSize()));
            return (
                sourceToClip.HasValue
                    ? this.Clip(
                        sourceToClip.Value,
                        GetFixedThickness(default),
                        duration: duration,
                        easingType: easingType,
                        easingMode: easingMode)
                    : null,
                targetFromClip.HasValue
                    ? this.Clip(
                        GetFixedThickness(default),
                        targetFromClip.Value,
                        duration: duration,
                        easingType: easingType,
                        easingMode: easingMode)
                    : null
                );
        }
    }
}
