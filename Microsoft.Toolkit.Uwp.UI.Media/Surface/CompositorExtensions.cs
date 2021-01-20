// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Numerics;
using Windows.UI.Composition;

namespace Microsoft.Toolkit.Uwp.UI.Media.Surface
{
    /// <summary>
    /// Extension methods for Windows.UI.Composition.Compositor
    /// The easing function values have been obtained from http://easings.net/
    /// </summary>
    public static class CompositorExtensions
    {
        /// <summary>
        /// Creates a CompositionGenerator object
        /// </summary>
        /// <param name="compositor">Compositor</param>
        /// <returns>ICompositionGenerator</returns>
        public static ICompositionGenerator CreateCompositionGenerator(this Compositor compositor)
        {
            return new CompositionGenerator(compositor);
        }

        /// <summary>
        /// Creates a CompositionGenerator object
        /// </summary>
        /// <param name="compositor">Compositor</param>
        /// <param name="useSharedCanvasDevice">Whether to use a shared CanvasDevice or to create a new one.</param>
        /// <param name="useSoftwareRenderer">Whether to use Software Renderer when creating a new CanvasDevice.</param>
        /// <returns>ICompositionGenerator</returns>
        public static ICompositionGenerator CreateCompositionGenerator(this Compositor compositor, bool useSharedCanvasDevice, bool useSoftwareRenderer)
        {
            return new CompositionGenerator(compositor, useSharedCanvasDevice, useSoftwareRenderer);
        }

        /// <summary>
        /// This extension method creates a scoped batch and handles the completed event
        /// the subscribing and unsubscribing process internally.
        ///
        /// Example usage:
        /// _compositor.CreateScopedBatch(CompositionBatchTypes.Animation,
        ///        () => // Action
        ///        {
        ///            transitionVisual.StartAnimation("Scale.XY", _scaleUpAnimation);
        ///        },
        ///        () => // Post Action
        ///        {
        ///            BackBtn.IsEnabled = true;
        ///        });
        ///
        /// </summary>
        /// <param name="compositor">Compositor</param>
        /// <param name="batchType">Composition Batch Type</param>
        /// <param name="action">Action to perform within the scoped batch</param>
        /// <param name="postAction">Action to perform once the batch completes</param>
        public static void CreateScopedBatch(this Compositor compositor, CompositionBatchTypes batchType, Action action, Action postAction = null)
        {
            if (action == null)
            {
                throw new ArgumentException("Cannot create a scoped batch on an action with null value!", nameof(action));
            }

            // Create ScopedBatch
            var batch = compositor.CreateScopedBatch(batchType);

            // Handler for the Completed Event
            void BatchCompletedHandler(object s, CompositionBatchCompletedEventArgs ea)
            {
                var scopedBatch = s as CompositionScopedBatch;

                // Unsubscribe the handler from the Completed event
                if (scopedBatch != null)
                {
                    scopedBatch.Completed -= BatchCompletedHandler;
                }

                try
                {
                    // Invoke the post action
                    postAction?.Invoke();
                }
                finally
                {
                    scopedBatch?.Dispose();
                }
            }

            // Subscribe to the Completed event
            batch.Completed += BatchCompletedHandler;

            // Invoke the action
            action();

            // End Batch
            batch.End();
        }

        /// <summary>
        /// This extension method creates a scoped batch and handles the completed event
        /// the subscribing and unsubscribing process internally.
        ///
        /// Example usage:
        /// _compositor.CreateScopedBatch(CompositionBatchTypes.Animation,
        ///        (batch) => // Action
        ///        {
        ///            transitionVisual.StartAnimation("Scale.XY", _scaleUpAnimation);
        ///        },
        ///        (batch) => // Post Action
        ///        {
        ///            BackBtn.IsEnabled = true;
        ///        });
        ///
        /// </summary>
        /// <param name="compositor">Compositor</param>
        /// <param name="batchType">Composition Batch Type</param>
        /// <param name="action">Action to perform within the scoped batch</param>
        /// <param name="postAction">Action to perform once the batch completes</param>
        public static void CreateScopedBatch(this Compositor compositor, CompositionBatchTypes batchType, Action<CompositionScopedBatch> action, Action<CompositionScopedBatch> postAction = null)
        {
            if (action == null)
            {
                throw new ArgumentException("Cannot create a scoped batch on an action with null value!", nameof(action));
            }

            // Create ScopedBatch
            var batch = compositor.CreateScopedBatch(batchType);

            // Handler for the Completed Event
            void BatchCompletedHandler(object s, CompositionBatchCompletedEventArgs ea)
            {
                var scopedBatch = s as CompositionScopedBatch;

                // Unsubscribe the handler from the Completed event
                if (scopedBatch != null)
                {
                    scopedBatch.Completed -= BatchCompletedHandler;
                }

                try
                {
                    // Invoke the post action
                    postAction?.Invoke(scopedBatch);
                }
                finally
                {
                    scopedBatch?.Dispose();
                }
            }

            // Subscribe to the Completed event
            batch.Completed += BatchCompletedHandler;

            // Invoke the action
            action(batch);

            // End Batch
            batch.End();
        }

        /// <summary>
        /// Back Easing Function - Ease In
        /// </summary>
        /// <param name="compositor">Compositor</param>
        /// <returns>CubicBezierEasingFunction</returns>
        public static CubicBezierEasingFunction CreateEaseInBackEasingFunction(this Compositor compositor)
        {
            return compositor.CreateCubicBezierEasingFunction(new Vector2(0.600f, -0.280f), new Vector2(0.735f, 0.045f));
        }

        /// <summary>
        /// Circle Easing Function - Ease In
        /// </summary>
        /// <param name="compositor">Compositor</param>
        /// <returns>CubicBezierEasingFunction</returns>
        public static CubicBezierEasingFunction CreateEaseInCircleEasingFunction(this Compositor compositor)
        {
            return compositor.CreateCubicBezierEasingFunction(new Vector2(0.600f, 0.040f), new Vector2(0.980f, 0.335f));
        }

        /// <summary>
        /// Cubic Easing Function - Ease In
        /// </summary>
        /// <param name="compositor">Compositor</param>
        /// <returns>CubicBezierEasingFunction</returns>
        public static CubicBezierEasingFunction CreateEaseInCubicEasingFunction(this Compositor compositor)
        {
            return compositor.CreateCubicBezierEasingFunction(new Vector2(0.550f, 0.055f), new Vector2(0.675f, 0.190f));
        }

        /// <summary>
        /// Exponential Easing Function - Ease In
        /// </summary>
        /// <param name="compositor">Compositor</param>
        /// <returns>CubicBezierEasingFunction</returns>
        public static CubicBezierEasingFunction CreateEaseInExponentialEasingFunction(this Compositor compositor)
        {
            return compositor.CreateCubicBezierEasingFunction(new Vector2(0.950f, 0.050f), new Vector2(0.795f, 0.035f));
        }

        /// <summary>
        /// Quadratic Easing Function - Ease In
        /// </summary>
        /// <param name="compositor">Compositor</param>
        /// <returns>CubicBezierEasingFunction</returns>
        public static CubicBezierEasingFunction CreateEaseInQuadraticEasingFunction(this Compositor compositor)
        {
            return compositor.CreateCubicBezierEasingFunction(new Vector2(0.550f, 0.085f), new Vector2(0.680f, 0.530f));
        }

        /// <summary>
        /// Quartic Easing Function - Ease In
        /// </summary>
        /// <param name="compositor">Compositor</param>
        /// <returns>CubicBezierEasingFunction</returns>
        public static CubicBezierEasingFunction CreateEaseInQuarticEasingFunction(this Compositor compositor)
        {
            return compositor.CreateCubicBezierEasingFunction(new Vector2(0.895f, 0.030f), new Vector2(0.685f, 0.220f));
        }

        /// <summary>
        /// Quintic Easing Function - Ease In
        /// </summary>
        /// <param name="compositor">Compositor</param>
        /// <returns>CubicBezierEasingFunction</returns>
        public static CubicBezierEasingFunction CreateEaseInQuinticEasingFunction(this Compositor compositor)
        {
            return compositor.CreateCubicBezierEasingFunction(new Vector2(0.755f, 0.050f), new Vector2(0.855f, 0.060f));
        }

        /// <summary>
        /// Sine Easing Function - Ease In
        /// </summary>
        /// <param name="compositor">Compositor</param>
        /// <returns>CubicBezierEasingFunction</returns>
        public static CubicBezierEasingFunction CreateEaseInSineEasingFunction(this Compositor compositor)
        {
            return compositor.CreateCubicBezierEasingFunction(new Vector2(0.470f, 0.000f), new Vector2(0.745f, 0.715f));
        }

        /// <summary>
        /// Back Easing Function - Ease Out
        /// </summary>
        /// <param name="compositor">Compositor</param>
        /// <returns>CubicBezierEasingFunction</returns>
        public static CubicBezierEasingFunction CreateEaseOutBackEasingFunction(this Compositor compositor)
        {
            return compositor.CreateCubicBezierEasingFunction(new Vector2(0.175f, 0.885f), new Vector2(0.320f, 1.275f));
        }

        /// <summary>
        /// Circle Easing Function - Ease Out
        /// </summary>
        /// <param name="compositor">Compositor</param>
        /// <returns>CubicBezierEasingFunction</returns>
        public static CubicBezierEasingFunction CreateEaseOutCircleEasingFunction(this Compositor compositor)
        {
            return compositor.CreateCubicBezierEasingFunction(new Vector2(0.075f, 0.820f), new Vector2(0.165f, 1.000f));
        }

        /// <summary>
        /// Cubic Easing Function - Ease Out
        /// </summary>
        /// <param name="compositor">Compositor</param>
        /// <returns>CubicBezierEasingFunction</returns>
        public static CubicBezierEasingFunction CreateEaseOutCubicEasingFunction(this Compositor compositor)
        {
            return compositor.CreateCubicBezierEasingFunction(new Vector2(0.215f, 0.610f), new Vector2(0.355f, 1.000f));
        }

        /// <summary>
        /// Exponential Easing Function - Ease Out
        /// </summary>
        /// <param name="compositor">Compositor</param>
        /// <returns>CubicBezierEasingFunction</returns>
        public static CubicBezierEasingFunction CreateEaseOutExponentialEasingFunction(this Compositor compositor)
        {
            return compositor.CreateCubicBezierEasingFunction(new Vector2(0.190f, 1.000f), new Vector2(0.220f, 1.000f));
        }

        /// <summary>
        /// Quadratic Easing Function - Ease Out
        /// </summary>
        /// <param name="compositor">Compositor</param>
        /// <returns>CubicBezierEasingFunction</returns>
        public static CubicBezierEasingFunction CreateEaseOutQuadraticEasingFunction(this Compositor compositor)
        {
            return compositor.CreateCubicBezierEasingFunction(new Vector2(0.250f, 0.460f), new Vector2(0.450f, 0.940f));
        }

        /// <summary>
        /// Quartic Easing Function - Ease Out
        /// </summary>
        /// <param name="compositor">Compositor</param>
        /// <returns>CubicBezierEasingFunction</returns>
        public static CubicBezierEasingFunction CreateEaseOutQuarticEasingFunction(this Compositor compositor)
        {
            return compositor.CreateCubicBezierEasingFunction(new Vector2(0.165f, 0.840f), new Vector2(0.440f, 1.000f));
        }

        /// <summary>
        /// Quintic Easing Function - Ease Out
        /// </summary>
        /// <param name="compositor">Compositor</param>
        /// <returns>CubicBezierEasingFunction</returns>
        public static CubicBezierEasingFunction CreateEaseOutQuinticEasingFunction(this Compositor compositor)
        {
            return compositor.CreateCubicBezierEasingFunction(new Vector2(0.230f, 1.000f), new Vector2(0.320f, 1.000f));
        }

        /// <summary>
        /// Sine Easing Function - Ease Out
        /// </summary>
        /// <param name="compositor">Compositor</param>
        /// <returns>CubicBezierEasingFunction</returns>
        public static CubicBezierEasingFunction CreateEaseOutSineEasingFunction(this Compositor compositor)
        {
            return compositor.CreateCubicBezierEasingFunction(new Vector2(0.390f, 0.575f), new Vector2(0.565f, 1.000f));
        }

        /// <summary>
        /// Back Easing Function - Ease In Out
        /// </summary>
        /// <param name="compositor">Compositor</param>
        /// <returns>CubicBezierEasingFunction</returns>
        public static CubicBezierEasingFunction CreateEaseInOutBackEasingFunction(this Compositor compositor)
        {
            return compositor.CreateCubicBezierEasingFunction(new Vector2(0.680f, -0.550f), new Vector2(0.265f, 1.550f));
        }

        /// <summary>
        /// Circle Easing Function - Ease In Out
        /// </summary>
        /// <param name="compositor">Compositor</param>
        /// <returns>CubicBezierEasingFunction</returns>
        public static CubicBezierEasingFunction CreateEaseInOutCircleEasingFunction(this Compositor compositor)
        {
            return compositor.CreateCubicBezierEasingFunction(new Vector2(0.785f, 0.135f), new Vector2(0.150f, 0.860f));
        }

        /// <summary>
        /// Cubic Easing Function - Ease In Out
        /// </summary>
        /// <param name="compositor">Compositor</param>
        /// <returns>CubicBezierEasingFunction</returns>
        public static CubicBezierEasingFunction CreateEaseInOutCubicEasingFunction(this Compositor compositor)
        {
            return compositor.CreateCubicBezierEasingFunction(new Vector2(0.645f, 0.045f), new Vector2(0.355f, 1.000f));
        }

        /// <summary>
        /// Exponential Easing Function - Ease In Out
        /// </summary>
        /// <param name="compositor">Compositor</param>
        /// <returns>CubicBezierEasingFunction</returns>
        public static CubicBezierEasingFunction CreateEaseInOutExponentialEasingFunction(this Compositor compositor)
        {
            return compositor.CreateCubicBezierEasingFunction(new Vector2(1.000f, 0.000f), new Vector2(0.000f, 1.000f));
        }

        /// <summary>
        /// Quadratic Easing Function - Ease In Out
        /// </summary>
        /// <param name="compositor">Compositor</param>
        /// <returns>CubicBezierEasingFunction</returns>
        public static CubicBezierEasingFunction CreateEaseInOutQuadraticEasingFunction(this Compositor compositor)
        {
            return compositor.CreateCubicBezierEasingFunction(new Vector2(0.455f, 0.030f), new Vector2(0.515f, 0.955f));
        }

        /// <summary>
        /// Quartic Easing Function - Ease In Out
        /// </summary>
        /// <param name="compositor">Compositor</param>
        /// <returns>CubicBezierEasingFunction</returns>
        public static CubicBezierEasingFunction CreateEaseInOutQuarticEasingFunction(this Compositor compositor)
        {
            return compositor.CreateCubicBezierEasingFunction(new Vector2(0.770f, 0.000f), new Vector2(0.175f, 1.000f));
        }

        /// <summary>
        /// Quintic Easing Function - Ease In Out
        /// </summary>
        /// <param name="compositor">Compositor</param>
        /// <returns>CubicBezierEasingFunction</returns>
        public static CubicBezierEasingFunction CreateEaseInOutQuinticEasingFunction(this Compositor compositor)
        {
            return compositor.CreateCubicBezierEasingFunction(new Vector2(0.860f, 0.000f), new Vector2(0.070f, 1.000f));
        }

        /// <summary>
        /// Sine Easing Function - Ease In Out
        /// </summary>
        /// <param name="compositor">Compositor</param>
        /// <returns>CubicBezierEasingFunction</returns>
        public static CubicBezierEasingFunction CreateEaseInOutSineEasingFunction(this Compositor compositor)
        {
            return compositor.CreateCubicBezierEasingFunction(new Vector2(0.445f, 0.050f), new Vector2(0.550f, 0.950f));
        }

        /// <summary>
        /// Creates the CompositionSurfaceBrush from the specified render surface.
        /// </summary>
        /// <param name="compositor">Compositor</param>
        /// <param name="renderSurface">An object deriving from IMaskSurface, IGaussianMaskSurface, IGeometrySurface or IImageSurface</param>
        /// <returns>CompositionSurfaceBrush</returns>
        public static CompositionSurfaceBrush CreateSurfaceBrush(this Compositor compositor, IRenderSurface renderSurface)
        {
            return compositor.CreateSurfaceBrush(renderSurface.Surface);
        }
    }
}
