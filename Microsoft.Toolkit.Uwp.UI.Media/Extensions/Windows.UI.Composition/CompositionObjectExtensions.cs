// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Threading.Tasks;
using Windows.UI.Composition;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Hosting;

namespace Microsoft.Toolkit.Uwp.UI.Media.Extensions
{
    /// <summary>
    /// An extension <see langword="class"/> for the <see cref="CompositionObject"/> type
    /// </summary>
    internal static class CompositionObjectExtensions
    {
        /// <summary>
        /// Starts an <see cref="ExpressionAnimation"/> to keep the size of the source <see cref="CompositionObject"/> in sync with the target <see cref="UIElement"/>
        /// </summary>
        /// <param name="source">The <see cref="CompositionObject"/> to start the animation on</param>
        /// <param name="target">The target <see cref="UIElement"/> to read the size updates from</param>
        public static void BindSize(this CompositionObject source, UIElement target)
        {
            var visual = ElementCompositionPreview.GetElementVisual(target);
            var bindSizeAnimation = source.Compositor.CreateExpressionAnimation($"{nameof(visual)}.Size");

            bindSizeAnimation.SetReferenceParameter(nameof(visual), visual);

            // Start the animation
            source.StartAnimation("Size", bindSizeAnimation);
        }

        /// <summary>
        /// Tries to retrieve the <see cref="CoreDispatcher"/> instance of the input <see cref="CompositionObject"/>
        /// </summary>
        /// <param name="source">The source <see cref="CompositionObject"/> instance</param>
        /// <param name="dispatcher">The resulting <see cref="CoreDispatcher"/>, if existing</param>
        /// <returns><see langword="true"/> if the <see cref="CoreDispatcher"/> has been retrieved, <see langword="false"/> otherwise</returns>
        public static bool TryGetDispatcher(this CompositionObject source, out CoreDispatcher dispatcher)
        {
            try
            {
                dispatcher = source.Dispatcher;

                return true;
            }
            catch (ObjectDisposedException)
            {
                dispatcher = null;

                return false;
            }
        }

        /// <summary>
        /// Starts an animation on the given property of a <see cref="CompositionObject"/>
        /// </summary>
        /// <param name="target">The target <see cref="CompositionObject"/></param>
        /// <param name="property">The name of the property to animate</param>
        /// <param name="value">The final value of the property</param>
        /// <param name="duration">The animation duration</param>
        /// <returns>A <see cref="Task"/> that completes when the created animation completes</returns>
        public static Task StartAnimationAsync(this CompositionObject target, string property, float value, TimeSpan duration)
        {
            // Stop previous animations
            target.StopAnimation(property);

            // Setup the animation
            var animation = target.Compositor.CreateScalarKeyFrameAnimation();
            animation.InsertKeyFrame(1f, value);
            animation.Duration = duration;

            // Get the batch and start the animations
            var batch = target.Compositor.CreateScopedBatch(CompositionBatchTypes.Animation);

            var tcs = new TaskCompletionSource<object>();

            batch.Completed += (s, e) => tcs.SetResult(null);

            target.StartAnimation(property, animation);

            batch.End();

            return tcs.Task;
        }
    }
}
