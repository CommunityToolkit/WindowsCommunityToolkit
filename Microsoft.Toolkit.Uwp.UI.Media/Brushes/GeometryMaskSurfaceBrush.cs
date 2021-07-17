// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Numerics;
using Microsoft.Toolkit.Uwp.Helpers;
using Windows.Foundation;
using Windows.UI.Composition;
using Windows.UI.Core;
using Windows.UI.Xaml;

namespace Microsoft.Toolkit.Uwp.UI.Media
{
    /// <summary>
    /// Represents a brush which defines a <see cref="CanvasCoreGeometry"/> as a mask to be applied on a <see cref="RenderSurfaceBrushBase"/> derivative.
    /// </summary>
    public sealed class GeometryMaskSurfaceBrush : RenderSurfaceBrushBase
    {
        private CompositionMaskBrush _maskBrush;

        private WeakEventListener<CanvasCoreGeometry, object, EventArgs> _maskUpdateListener;
        private WeakEventListener<RenderSurfaceBrushBase, object, EventArgs> _targetUpdateListener;

        /// <summary>
        /// Target Dependency Property
        /// </summary>
        public static readonly DependencyProperty TargetProperty = DependencyProperty.Register(
            "Target",
            typeof(RenderSurfaceBrushBase),
            typeof(GeometryMaskSurfaceBrush),
            new PropertyMetadata(null, OnTargetChanged));

        /// <summary>
        /// Gets or sets the target <see cref="RenderSurfaceBrushBase"/> on which the Mask is applied.
        /// </summary>
        public RenderSurfaceBrushBase Target
        {
            get => (RenderSurfaceBrushBase)GetValue(TargetProperty);
            set => SetValue(TargetProperty, value);
        }

        /// <summary>
        /// Handles changes to the Target property.
        /// </summary>
        /// <param name="d"><see cref="GeometryMaskSurfaceBrush" />.</param>
        /// <param name="e">DependencyProperty changed event arguments.</param>
        private static void OnTargetChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var geometryMaskSurfaceBrush = (GeometryMaskSurfaceBrush)d;
            geometryMaskSurfaceBrush.OnTargetChanged();
        }

        /// <summary>
        /// Instance handler for the changes to the Target dependency property.
        /// </summary>
        private void OnTargetChanged()
        {
            _targetUpdateListener?.Detach();
            _targetUpdateListener = null;

            if (Target != null)
            {
                _targetUpdateListener = new WeakEventListener<RenderSurfaceBrushBase, object, EventArgs>(Target)
                {
                    OnEventAction = async (instance, source, args) =>
                    {
                        await Window.Current.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                        {
                            OnSurfaceBrushUpdated();
                        });
                    }
                };

                Target.Updated += _targetUpdateListener.OnEvent;

                OnSurfaceBrushUpdated();
            }
        }

        /// <summary>
        /// Mask Dependency Property
        /// </summary>
        public static readonly DependencyProperty MaskProperty = DependencyProperty.Register(
            "Mask",
            typeof(CanvasCoreGeometry),
            typeof(GeometryMaskSurfaceBrush),
            new PropertyMetadata(null, OnMaskChanged));

        /// <summary>
        /// Gets or sets the Mask Geometry that is applied on the Target Geometry.
        /// </summary>
        public CanvasCoreGeometry Mask
        {
            get => (CanvasCoreGeometry)GetValue(MaskProperty);
            set => SetValue(MaskProperty, value);
        }

        /// <summary>
        /// Handles changes to the Mask property.
        /// </summary>
        /// <param name="d"><see cref="GeometryMaskSurfaceBrush" />.</param>
        /// <param name="e">DependencyProperty changed event arguments.</param>
        private static void OnMaskChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var geometryMaskSurfaceBrush = (GeometryMaskSurfaceBrush)d;
            geometryMaskSurfaceBrush.OnMaskChanged();
        }

        /// <summary>
        /// Instance handler for the changes to the Mask dependency property.
        /// </summary>
        private void OnMaskChanged()
        {
            _maskUpdateListener?.Detach();
            _maskUpdateListener = null;

            if (Mask != null)
            {
                _maskUpdateListener = new WeakEventListener<CanvasCoreGeometry, object, EventArgs>(Mask)
                {
                    OnEventAction = async (instance, source, args) =>
                    {
                        await Window.Current.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                        {
                            OnSurfaceBrushUpdated();
                        });
                    }
                };

                Mask.Updated += _maskUpdateListener.OnEvent;

                OnSurfaceBrushUpdated();
            }
        }

        /// <summary>
        /// OffsetX Dependency Property
        /// </summary>
        public static readonly DependencyProperty OffsetXProperty = DependencyProperty.Register(
            "OffsetX",
            typeof(double),
            typeof(GeometryMaskSurfaceBrush),
            new PropertyMetadata(0d, OnPropertyChanged));

        /// <summary>
        /// Gets or sets the offset on the x-axis from the top left corner of the Brush Surface where the Geometry is rendered.
        /// </summary>
        public double OffsetX
        {
            get => (double)GetValue(OffsetXProperty);
            set => SetValue(OffsetXProperty, value);
        }

        /// <summary>
        /// OffsetY Dependency Property
        /// </summary>
        public static readonly DependencyProperty OffsetYProperty = DependencyProperty.Register(
            "OffsetY",
            typeof(double),
            typeof(GeometryMaskSurfaceBrush),
            new PropertyMetadata(0d, OnPropertyChanged));

        /// <summary>
        /// Gets or sets the offset on the y-axis from the top left corner of the Brush Surface where the Geometry is rendered.
        /// </summary>
        public double OffsetY
        {
            get => (double)GetValue(OffsetYProperty);
            set => SetValue(OffsetYProperty, value);
        }

        /// <summary>
        /// BlurRadius Dependency Property
        /// </summary>
        public static readonly DependencyProperty BlurRadiusProperty = DependencyProperty.Register(
            "BlurRadius",
            typeof(double),
            typeof(GeometryMaskSurfaceBrush),
            new PropertyMetadata(0d, OnPropertyChanged));

        /// <summary>
        /// Gets or sets the radius of Gaussian Blur to be applied on the brush.
        /// </summary>
        public double BlurRadius
        {
            get => (double)GetValue(BlurRadiusProperty);
            set => SetValue(BlurRadiusProperty, value);
        }

        /// <summary>
        /// Method that is called whenever the dependency properties of the Brush changes.
        /// </summary>
        /// <param name="d">The object whose property has changed.</param>
        /// <param name="e">Event arguments.</param>
        private static void OnPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var brush = (GeometryMaskSurfaceBrush)d;

            // Recreate the canvas brush on any property change.
            brush.OnSurfaceBrushUpdated();
        }

        /// <inheritdoc/>
        protected override void OnSurfaceBrushUpdated(bool createSurface = false)
        {
            if (Generator == null)
            {
                GetGeneratorInstance();
            }

            if (Target == null || Target.Brush == null || Mask == null || Generator == null)
            {
                return;
            }

            var offset = new Vector2((float)OffsetX, (float)OffsetY);

            _maskBrush = Window.Current.Compositor.CreateMaskBrush();
            _maskBrush.Source = Target.Brush;

            if (createSurface || (RenderSurface == null))
            {
                CompositionBrush?.Dispose();
                RenderSurface = Generator.CreateGaussianMaskSurface(new Size(SurfaceWidth, SurfaceHeight), Mask.Geometry, offset, (float)BlurRadius);
                _maskBrush.Mask = Window.Current.Compositor.CreateSurfaceBrush(RenderSurface.Surface);
                CompositionBrush = _maskBrush;
            }
            else
            {
                ((IGaussianMaskSurface)RenderSurface).Redraw(new Size(SurfaceWidth, SurfaceHeight), Mask.Geometry, offset, (float)BlurRadius);
            }

            base.OnSurfaceBrushUpdated(createSurface);
        }
    }
}
