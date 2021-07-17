// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Effects;
using Microsoft.Toolkit.Uwp.UI.Media.Pipelines;
using Windows.UI;
using Windows.UI.Composition;
using Windows.UI.Xaml.Media;

namespace Microsoft.Toolkit.Uwp.UI.Media
{
    /// <summary>
    /// Extension methods for Windows.UI.Composition.Compositor
    /// </summary>
    public static class CompositorExtensions
    {
        /// <summary>
        /// Creates the CompositionSurfaceBrush from the specified render surface.
        /// </summary>
        /// <param name="compositor">Compositor</param>
        /// <param name="renderSurface">An object deriving from IGeometryMaskSurface, IGaussianMaskSurface, IGeometrySurface or IImageSurface</param>
        /// <returns>CompositionSurfaceBrush</returns>
        public static CompositionSurfaceBrush CreateSurfaceBrush(this Compositor compositor, IRenderSurface renderSurface)
        {
            return compositor.CreateSurfaceBrush(renderSurface.Surface);
        }

        /// <summary>
        /// Creates a custom shaped Effect Brush using BackdropBrush and an IGeometryMaskSurface.
        /// </summary>
        /// <param name="compositor">Compositor</param>
        /// <param name="mask">IGeometryMaskSurface</param>
        /// <param name="blendColor">Color to blend in the BackdropBrush</param>
        /// <param name="blurAmount">Blur Amount of the Backdrop Brush</param>
        /// <param name="backdropBrush">Backdrop Brush (optional). If not provided, then compositor creates it.</param>
        /// <returns>CompositionEffectBrush</returns>
        public static CompositionEffectBrush CreateMaskedBackdropBrush(this Compositor compositor, IGeometryMaskSurface mask, Color blendColor, float blurAmount, CompositionBackdropBrush backdropBrush = null)
        {
            return CreateBackdropBrush(compositor, mask, blendColor, blurAmount, backdropBrush);
        }

        /// <summary>
        /// Creates a custom shaped Effect Brush using BackdropBrush and an IGaussianMaskSurface.
        /// </summary>
        /// <param name="compositor">Compositor</param>
        /// <param name="mask">IGeometryMaskSurface</param>
        /// <param name="blendColor">Color to blend in the BackdropBrush</param>
        /// <param name="blurRadius">Blur Amount of the Backdrop Brush</param>
        /// <param name="backdropBrush">Backdrop Brush (optional). If not provided, then compositor creates it.</param>
        /// <returns>CompositionEffectBrush</returns>
        public static CompositionEffectBrush CreateGaussianMaskedBackdropBrush(this Compositor compositor, IGaussianMaskSurface mask, Color blendColor, float blurRadius, CompositionBackdropBrush backdropBrush = null)
        {
            return CreateBackdropBrush(compositor, mask, blendColor, blurRadius, backdropBrush);
        }

        /// <summary>
        /// Creates a custom shaped Effect Brush using BackdropBrush and an IGeometryMaskSurface or an IGaussianMaskSurface.
        /// </summary>
        /// <param name="compositor">Compositor</param>
        /// <param name="mask">IGeometryMaskSurface or IGaussianMaskSurface</param>
        /// <param name="blendColor">Color to blend in the BackdropBrush</param>
        /// <param name="blurAmount">Blur Amount of the Backdrop Brush</param>
        /// <param name="backdropBrush">Backdrop Brush (optional). If not provided, then compositor creates it.</param>
        /// <returns>CompositionEffectBrush</returns>
        internal static CompositionEffectBrush CreateBackdropBrush(Compositor compositor, IRenderSurface mask, Color blendColor, float blurAmount, CompositionBackdropBrush backdropBrush = null)
        {
            // Blur Effect
            var blurEffect = new GaussianBlurEffect()
            {
                Name = "Blur",
                BlurAmount = blurAmount,
                BorderMode = EffectBorderMode.Hard,
                Optimization = EffectOptimization.Balanced,
                Source = new CompositionEffectSourceParameter("backdrop"),
            };

            // Blend Effect
            var blendEffect = new Graphics.Canvas.Effects.BlendEffect
            {
                Foreground = new ColorSourceEffect
                {
                    Name = "Color",
                    Color = blendColor
                },
                Background = blurEffect,
                Mode = BlendEffectMode.Multiply
            };

            // Composite Effect
            var effect = new CompositeEffect
            {
                Mode = CanvasComposite.DestinationIn,
                Sources =
                {
                    blendEffect,
                    new CompositionEffectSourceParameter("mask")
                }
            };

            // Create Effect Factory
            var factory = compositor.CreateEffectFactory(effect, new[] { "Blur.BlurAmount", "Color.Color" });

            // Create Effect Brush
            var brush = factory.CreateBrush();

            // Set the BackDropBrush
            // If no backdrop brush is provided, create one
            brush.SetSourceParameter("backdrop", backdropBrush ?? compositor.CreateBackdropBrush());

            // Set the Mask
            // Create SurfaceBrush from IGeometryMaskSurface
            var maskBrush = compositor.CreateSurfaceBrush(mask.Surface);
            brush.SetSourceParameter("mask", maskBrush);

            return brush;
        }

        /// <summary>
        /// Creates a custom shaped Frosted Glass Effect Brush using BackdropBrush and a Mask
        /// </summary>
        /// <param name="compositor">Compositor</param>
        /// <param name="mask">IGeometryMaskSurface</param>
        /// <param name="blendColor">Color to blend in the BackdropBrush</param>
        /// <param name="blurAmount">Blur Amount of the Backdrop Brush</param>
        /// <param name="backdropBrush">Backdrop Brush (optional). If not provided, then compositor creates it.</param>
        /// <param name="multiplyAmount">MultiplyAmount of the ArithmeticCompositeEffect</param>
        /// <param name="colorAmount">Source1Amount of the ArithmeticCompositeEffect</param>
        /// <param name="backdropAmount">Source2Amount of the ArithmeticCompositeEffect</param>
        /// <returns>CompositionEffectBrush</returns>
        public static CompositionEffectBrush CreateFrostedGlassBrush(
            this Compositor compositor,
            IGeometryMaskSurface mask,
            Color blendColor,
            float blurAmount,
            CompositionBackdropBrush backdropBrush = null,
            float multiplyAmount = 0,
            float colorAmount = 0.5f,
            float backdropAmount = 0.5f)
        {
            // Create a frosty glass effect
            var frostEffect = new GaussianBlurEffect
            {
                Name = "Blur",
                BlurAmount = blurAmount,
                BorderMode = EffectBorderMode.Hard,
                Source = new ArithmeticCompositeEffect
                {
                    Name = "Source",
                    MultiplyAmount = multiplyAmount,
                    Source1Amount = backdropAmount,
                    Source2Amount = colorAmount,
                    Source1 = new CompositionEffectSourceParameter("backdrop"),
                    Source2 = new ColorSourceEffect
                    {
                        Name = "BlendColor",
                        Color = blendColor
                    }
                }
            };

            // Composite Effect
            var effect = new CompositeEffect
            {
                Mode = CanvasComposite.DestinationIn,
                Sources =
                {
                    frostEffect,
                    new CompositionEffectSourceParameter("mask")
                }
            };

            // Create Effect Factory
            var factory = compositor.CreateEffectFactory(effect, new[] { "Blur.BlurAmount", "BlendColor.Color" });

            // Create Effect Brush
            var brush = factory.CreateBrush();

            // Set the BackDropBrush
            // If no backdrop brush is provided, create one
            brush.SetSourceParameter("backdrop", backdropBrush ?? compositor.CreateBackdropBrush());

            // Set the Mask
            // Create SurfaceBrush from CompositionMask
            var maskBrush = compositor.CreateSurfaceBrush(mask.Surface);
            brush.SetSourceParameter("mask", maskBrush);

            return brush;
        }

        /// <summary>
        /// Updates the CompositionSurfaceBrush's Stretch and Alignment options
        /// </summary>
        /// <param name="surfaceBrush">CompositionSurfaceBrush</param>
        /// <param name="stretch">Stretch mode</param>
        /// <param name="alignX">Horizontal Alignment</param>
        /// <param name="alignY">Vertical Alignment</param>
        /// <param name="alignXAnimation">The animation to use to update the horizontal alignment of the surface brush</param>
        /// <param name="alignYAnimation">The animation to use to update the vertical alignment of the surface brush</param>
        public static void UpdateSurfaceBrushOptions(
            this CompositionSurfaceBrush surfaceBrush,
            Stretch stretch,
            AlignmentX alignX,
            AlignmentY alignY,
            ScalarKeyFrameAnimation alignXAnimation = null,
            ScalarKeyFrameAnimation alignYAnimation = null)
        {
            // Stretch Mode
            surfaceBrush.Stretch = stretch switch
            {
                Stretch.None => CompositionStretch.None,
                Stretch.Fill => CompositionStretch.Fill,
                Stretch.Uniform => CompositionStretch.Uniform,
                Stretch.UniformToFill => CompositionStretch.UniformToFill,
                _ => throw new ArgumentException("Invalid stretch value")

            };

            // Horizontal Alignment
            var finalAlignX = alignX switch
            {
                AlignmentX.Left => 0,
                AlignmentX.Center => 0.5f,
                AlignmentX.Right => 1f,
                _ => surfaceBrush.HorizontalAlignmentRatio
            };

            // If animation is available, animate to the new value
            // otherwise set it explicitly
            if (alignXAnimation == null)
            {
                surfaceBrush.HorizontalAlignmentRatio = finalAlignX;
            }
            else
            {
                alignXAnimation.InsertKeyFrame(1f, finalAlignX);
                surfaceBrush.StartAnimation("HorizontalAlignmentRatio", alignXAnimation);
            }

            // Vertical Alignment
            var finalAlignY = alignY switch
            {
                AlignmentY.Top => 0,
                AlignmentY.Center => 0.5f,
                AlignmentY.Bottom => 1f,
                _ => surfaceBrush.VerticalAlignmentRatio
            };

            // If animation is available, animate to the new value
            // otherwise set it explicitly
            if (alignYAnimation == null)
            {
                surfaceBrush.VerticalAlignmentRatio = finalAlignY;
            }
            else
            {
                alignYAnimation.InsertKeyFrame(1f, finalAlignY);
                surfaceBrush.StartAnimation("VerticalAlignmentRatio", alignYAnimation);
            }
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
    }
}
