// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using Windows.Foundation;
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
        private class AnimatedElementComparer : IEqualityComparer<DependencyObject>
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

        private static IEnumerable<UIElement> GetAnimatedElements(DependencyObject targetElement)
        {
            return targetElement?.FindDescendantsOrSelf()
                .Where(element => GetId(element) is not null || GetCoordinatedTarget(element) is not null || GetIsIndependent(element))
                .Distinct(new AnimatedElementComparer())
                .OfType<UIElement>();
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

        private static void IsNotNullAndIsLoaded(FrameworkElement target, string name)
        {
            if (target is null)
            {
                throw new ArgumentNullException(name);
            }

            if (target.IsLoaded is false)
            {
                throw new ArgumentException($"The {name} element has not been loaded yet.", name);
            }
        }

        private static Task UpdateControlLayout(FrameworkElement target)
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

        private static Vector2 GetInverseScale(Vector2 scale) => new(1 / scale.X, 1 / scale.Y);

        private static Thickness GetFixedThickness(Thickness thickness, double defaultValue = -4 /* -4 is used to prevent shadows from being cropped.*/)
        {
            var left = thickness.Left < AlmostZero ? defaultValue : thickness.Left;
            var top = thickness.Top < AlmostZero ? defaultValue : thickness.Top;
            var right = thickness.Right < AlmostZero ? defaultValue : thickness.Right;
            var bottom = thickness.Bottom < AlmostZero ? defaultValue : thickness.Bottom;
            return new Thickness(left, top, right, bottom);
        }

        private static Thickness? GetCoordinatedElementClip(Vector2 scale, Point targetLocation, Size targetSize, Rect targetParentBounds)
        {
            var inverseScale = GetInverseScale(scale);
            var targetBounds = new Rect(targetLocation, targetSize);
            if (targetParentBounds.Contains(targetLocation) && targetParentBounds.Contains(new Point(targetBounds.Right, targetBounds.Bottom)))
            {
                return null;
            }

            return GetFixedThickness(
                new Thickness(
                    (targetParentBounds.X - targetBounds.X) * inverseScale.X,
                    (targetParentBounds.Y - targetBounds.Y) * inverseScale.Y,
                    (targetBounds.Right - targetParentBounds.Right) * inverseScale.X,
                    (targetBounds.Bottom - targetParentBounds.Bottom) * inverseScale.X));
        }

        private static Thickness? GetConnectedElementClip(Axis? axis, Vector2 scale, Vector2 actualSize, Vector2 centerPoint, Rect targetParentBounds)
        {
            var targetLocation = -centerPoint * scale;
            var targetSize = (actualSize * scale).ToSize();
            if (axis is Axis.X)
            {
                var minY = Math.Min(targetParentBounds.Y, targetLocation.Y);
                targetParentBounds.Height = Math.Max(targetParentBounds.Bottom, targetLocation.Y + targetSize.Height) - minY;
                targetParentBounds.Y = minY;
            }
            else if (axis is Axis.Y)
            {
                var minX = Math.Min(targetParentBounds.X, targetLocation.X);
                targetParentBounds.Width = Math.Max(targetParentBounds.Right, targetLocation.X + targetSize.Width) - minX;
                targetParentBounds.X = minX;
            }

            return GetCoordinatedElementClip(scale, targetLocation.ToPoint(), targetSize, targetParentBounds);
        }
    }
}
