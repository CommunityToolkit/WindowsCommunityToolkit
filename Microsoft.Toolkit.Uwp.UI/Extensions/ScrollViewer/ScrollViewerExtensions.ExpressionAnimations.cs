// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.Toolkit.Uwp.UI.Enums;
using Windows.UI.Composition;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Hosting;

namespace Microsoft.Toolkit.Uwp.UI.Extensions
{
    /// <summary>
    /// Provides attached dependency properties for the <see cref="ListViewBase"/>
    /// </summary>
    public static partial class ScrollViewerExtensions
    {
        /// <summary>
        /// Creates and starts an animation on the target element that binds either the X or Y axis of the source <see cref="ScrollViewer"/>.
        /// </summary>
        /// <param name="scroller">The source <see cref="ScrollViewer"/> control to use.</param>
        /// <param name="target">The target <see cref="UIElement"/> that will be animated.</param>
        /// <param name="axis">The scrolling axis of the source <see cref="ScrollViewer"/>.</param>
        /// <returns>An <see cref="ExpressionAnimation"/> instance that represents an already running animation.</returns>
        public static ExpressionAnimation StartExpressionAnimation(
            this ScrollViewer scroller,
            UIElement target,
            Axis axis)
        {
            return scroller.StartExpressionAnimation(target, axis, axis);
        }

        /// <summary>
        /// Creates and starts an animation on the target element that binds either the X or Y axis of the source <see cref="ScrollViewer"/>
        /// </summary>
        /// <param name="scroller">The source <see cref="ScrollViewer"/> control to use</param>
        /// <param name="target">The target <see cref="UIElement"/> that will be animated</param>
        /// <param name="sourceAxis">The scrolling axis of the source <see cref="ScrollViewer"/></param>
        /// <param name="targetAxis">The optional scrolling axis of the target element, if <see langword="null"/> the source axis will be used</param>
        /// <returns>An <see cref="ExpressionAnimation"/> instance that represents an already running animation.</returns>
        public static ExpressionAnimation StartExpressionAnimation(
            this ScrollViewer scroller,
            UIElement target,
            Axis sourceAxis,
            Axis targetAxis)
        {
            CompositionPropertySet scrollSet = ElementCompositionPreview.GetScrollViewerManipulationPropertySet(scroller);

            ExpressionAnimation animation = scrollSet.Compositor.CreateExpressionAnimation($"{nameof(scroller)}.{nameof(UIElement.Translation)}.{sourceAxis}");

            animation.SetReferenceParameter(nameof(scroller), scrollSet);

            Visual visual = ElementCompositionPreview.GetElementVisual(target);

            visual.StartAnimation($"{nameof(Visual.Offset)}.{targetAxis}", animation);

            return animation;
        }
    }
}