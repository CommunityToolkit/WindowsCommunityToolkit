// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

#nullable enable

using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Composition;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Hosting;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;

namespace Microsoft.Toolkit.Uwp.UI.Animations
{
    /// <summary>
    /// A animation helper that morphs between two controls.
    /// </summary>
    public sealed partial class TransitionHelper
    {
        private sealed class AnimatedElementComparer : IEqualityComparer<DependencyObject>
        {
            public bool Equals(DependencyObject x, DependencyObject y)
            {
                if (GetIsIndependent(x) || GetIsIndependent(y))
                {
                    return false;
                }

                return GetId(x) is { } xId && GetId(y) is { } yId && xId.Equals(yId);
            }

            public int GetHashCode(DependencyObject obj)
            {
                return 0;
            }
        }

        private static AnimatedElements<UIElement> GetAnimatedElements(DependencyObject parent)
        {
            var animatedElements = new AnimatedElements<UIElement>(
                new Dictionary<string, UIElement>(),
                new Dictionary<string, IList<UIElement>>(),
                new List<UIElement>());
            if (parent is null)
            {
                return animatedElements;
            }

            var allAnimatedElements = FindDescendantsWithBFSAndPruneAndPredicate(parent, IsNotVisible, IsAnimatedElement)
                .Distinct(new AnimatedElementComparer())
                .OfType<UIElement>();
            foreach (var item in allAnimatedElements)
            {
                if (GetId(item) is { } id)
                {
                    animatedElements.ConnectedElements[id] = item;
                }
                else if (GetCoordinatedTarget(item) is { } targetId)
                {
                    if (animatedElements.CoordinatedElements.ContainsKey(targetId) is false)
                    {
                        animatedElements.CoordinatedElements[targetId] = new List<UIElement> { item };
                    }
                    else
                    {
                        animatedElements.CoordinatedElements[targetId].Add(item);
                    }
                }
                else
                {
                    animatedElements.IndependentElements.Add(item);
                }
            }

            return animatedElements;
        }

        private static bool IsNotVisible(DependencyObject element)
        {
            if (element is not UIElement target
                || target.Visibility == Visibility.Collapsed
                || target.Opacity < AlmostZero)
            {
                return true;
            }

            return false;
        }

        private static bool IsAnimatedElement(DependencyObject element)
        {
            return GetId(element) is not null || GetCoordinatedTarget(element) is not null || GetIsIndependent(element);
        }

        private static IEnumerable<DependencyObject> FindDescendantsWithBFSAndPruneAndPredicate(DependencyObject element, Func<DependencyObject, bool> prune, Func<DependencyObject, bool> predicate)
        {
            if (predicate(element))
            {
                yield return element;
                yield break;
            }

            var searchQueue = new Queue<DependencyObject>();
            var childrenCount = VisualTreeHelper.GetChildrenCount(element);
            for (var i = 0; i < childrenCount; i++)
            {
                var child = VisualTreeHelper.GetChild(element, i);
                if (predicate(child))
                {
                    yield return child;
                }
                else if (prune(child) is false)
                {
                    searchQueue.Enqueue(child);
                }
            }

            while (searchQueue.Count > 0)
            {
                var parent = searchQueue.Dequeue();
                childrenCount = VisualTreeHelper.GetChildrenCount(parent);
                for (var j = 0; j < childrenCount; j++)
                {
                    var child = VisualTreeHelper.GetChild(parent, j);
                    if (predicate(child))
                    {
                        yield return child;
                    }
                    else if (prune(child) is false)
                    {
                        searchQueue.Enqueue(child);
                    }
                }
            }
        }

        private static void ToggleVisualState(UIElement target, VisualStateToggleMethod method, bool isVisible)
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
                    target.GetVisual().IsVisible = isVisible;
                    break;
            }

            target.IsHitTestVisible = isVisible;
        }

        private static void RestoreElements(IEnumerable<UIElement> animatedElements)
        {
            foreach (var animatedElement in animatedElements)
            {
                RestoreElement(animatedElement);
            }
        }

        private static void RestoreElement(UIElement animatedElement)
        {
            ElementCompositionPreview.SetIsTranslationEnabled(animatedElement, true);
            var visual = animatedElement.GetVisual();
            visual.StopAnimation(nameof(Visual.Opacity));
            visual.StopAnimation(TranslationXYPropertyName);
            visual.StopAnimation(ScaleXYPropertyName);
            if (visual.Clip is InsetClip clip)
            {
                clip.StopAnimation(nameof(InsetClip.LeftInset));
                clip.StopAnimation(nameof(InsetClip.TopInset));
                clip.StopAnimation(nameof(InsetClip.RightInset));
                clip.StopAnimation(nameof(InsetClip.BottomInset));
            }

            visual.Opacity = 1;
            visual.Scale = Vector3.One;
            visual.Clip = null;
            visual.Properties.InsertVector3(TranslationPropertyName, Vector3.Zero);
        }

        private static void IsNotNullAndIsInVisualTree(FrameworkElement target, string name)
        {
            if (target is null)
            {
                throw new ArgumentNullException(name);
            }

            if (VisualTreeHelper.GetParent(target) is null)
            {
                throw new ArgumentException($"The {name} element is not in the visual tree.", name);
            }
        }

        private static Task UpdateControlLayout(FrameworkElement target)
        {
            var updateTargetLayoutTaskSource = new TaskCompletionSource<object?>();
            void OnTargetLayoutUpdated(object sender, object e)
            {
                target.LayoutUpdated -= OnTargetLayoutUpdated;
                _ = updateTargetLayoutTaskSource.TrySetResult(null);
            }

            target.LayoutUpdated += OnTargetLayoutUpdated;
            target.UpdateLayout();
            return updateTargetLayoutTaskSource.Task;
        }

        private static Vector2 GetInverseScale(Vector2 scale) => new(1 / scale.X, 1 / scale.Y);

        private static Thickness GetFixedThickness(double left, double top, double right, double bottom, double defaultValue = 0)
        {
            var fixedLeft = left < AlmostZero ? defaultValue : left;
            var fixedTop = top < AlmostZero ? defaultValue : top;
            var fixedRight = right < AlmostZero ? defaultValue : right;
            var fixedBottom = bottom < AlmostZero ? defaultValue : bottom;
            return new Thickness(fixedLeft, fixedTop, fixedRight, fixedBottom);
        }

        private static Rect GetTransformedBounds(Vector2 initialLocation, Vector2 initialSize, Vector2 centerPoint, Vector2 targetScale)
        {
            var targetMatrix3x2 = Matrix3x2.CreateScale(targetScale, centerPoint);
            return new Rect((initialLocation + Vector2.Transform(default, targetMatrix3x2)).ToPoint(), (initialSize * targetScale).ToSize());
        }

        private static Thickness? GetTargetClip(Vector2 initialLocation, Vector2 initialSize, Vector2 centerPoint, Vector2 targetScale, Vector2 translation, Rect targetParentBounds)
        {
            var transformedBounds = GetTransformedBounds(initialLocation + translation, initialSize, centerPoint, targetScale);
            var inverseScale = GetInverseScale(targetScale);
            if (targetParentBounds.Contains(new Point(transformedBounds.Left, transformedBounds.Top)) && targetParentBounds.Contains(new Point(transformedBounds.Right, transformedBounds.Bottom)))
            {
                return null;
            }

            return GetFixedThickness(
                    (targetParentBounds.X - transformedBounds.X) * inverseScale.X,
                    (targetParentBounds.Y - transformedBounds.Y) * inverseScale.Y,
                    (transformedBounds.Right - targetParentBounds.Right) * inverseScale.X,
                    (transformedBounds.Bottom - targetParentBounds.Bottom) * inverseScale.X);
        }

        private static readonly Dictionary<(EasingType, EasingMode, bool), IEasingFunctionFactory> EasingFunctionFactoryCache = new();

        private static IEasingFunctionFactory GetEasingFunctionFactory(EasingType type = EasingType.Default, EasingMode mode = EasingMode.EaseInOut, bool inverse = false)
        {
            if (EasingFunctionFactoryCache.TryGetValue((type, mode, inverse), out var easingFunctionFactory))
            {
                return easingFunctionFactory;
            }

            var factory = new EasingFunctionFactory(type, mode, inverse);
            EasingFunctionFactoryCache[(type, mode, inverse)] = factory;
            return factory;
        }
    }
}
