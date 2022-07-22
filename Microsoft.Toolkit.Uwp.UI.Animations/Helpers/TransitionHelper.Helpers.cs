// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Linq;
using System.Numerics;
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
                .Where(element => GetId(element) is not null || GetIsIndependent(element))
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
            visual.Opacity = 1;
            visual.Scale = Vector3.One;
            visual.Clip = null;
            visual.Properties.InsertVector3("Translation", Vector3.Zero);
        }

        private static Vector2 GetInverseScale(Vector2 scale) => new(1 / scale.X, 1 / scale.Y);

        private static Thickness GetFixedThickness(Thickness thickness, double defaultValue)
        {
            var left = thickness.Left < AlmostZero ? defaultValue : thickness.Left;
            var top = thickness.Top < AlmostZero ? defaultValue : thickness.Top;
            var right = thickness.Right < AlmostZero ? defaultValue : thickness.Right;
            var bottom = thickness.Bottom < AlmostZero ? defaultValue : thickness.Bottom;
            return new Thickness(left, top, right, bottom);
        }
    }
}
