// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Microsoft.Graphics.Canvas.Brushes;
using Microsoft.Graphics.Canvas.Geometry;
using Microsoft.Toolkit.Uwp.Helpers;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Xaml;

namespace Microsoft.Toolkit.Uwp.UI.Media
{
    /// <summary>
    /// Brush which renders the provided <see cref="CanvasCoreGeometry"/>.
    /// </summary>
    public sealed class GeometrySurfaceBrush : RenderSurfaceBrushBase
    {
        private WeakEventListener<CanvasCoreGeometry, object, EventArgs> _geometryUpdateListener;
        private WeakEventListener<RenderCanvasBrushBase, object, EventArgs> _strokeUpdateListener;
        private WeakEventListener<RenderCanvasBrushBase, object, EventArgs> _fillBrushUpdateListener;
        private WeakEventListener<RenderCanvasBrushBase, object, EventArgs> _bgBrushUpdateListener;
        private WeakEventListener<StrokeStyle, object, EventArgs> _strokeStyleUpdateListener;

        /// <summary>
        /// Geometry Dependency Property
        /// </summary>
        public static readonly DependencyProperty GeometryProperty = DependencyProperty.Register(
            "Geometry",
            typeof(CanvasCoreGeometry),
            typeof(GeometrySurfaceBrush),
            new PropertyMetadata(null, OnGeometryChanged));

        /// <summary>
        /// Gets or sets the <see cref="CanvasCoreGeometry"/> that is used to create the mask.
        /// </summary>
        public CanvasCoreGeometry Geometry
        {
            get => (CanvasCoreGeometry)GetValue(GeometryProperty);
            set => SetValue(GeometryProperty, value);
        }

        /// <summary>
        /// Handles changes to the Geometry property.
        /// </summary>
        /// <param name="d"><see cref="GeometrySurfaceBrush" />.</param>
        /// <param name="e">DependencyProperty changed event arguments.</param>
        private static void OnGeometryChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var maskSurfaceBrush = (GeometrySurfaceBrush)d;
            maskSurfaceBrush.OnGeometryChanged();
        }

        /// <summary>
        /// Instance handler for the changes to the Geometry dependency property.
        /// </summary>
        private void OnGeometryChanged()
        {
            _geometryUpdateListener?.Detach();
            _geometryUpdateListener = null;

            if (Geometry != null)
            {
                _geometryUpdateListener = new WeakEventListener<CanvasCoreGeometry, object, EventArgs>(Geometry)
                {
                    OnEventAction = async (instance, source, args) =>
                    {
                        await Window.Current.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                        {
                            OnSurfaceBrushUpdated();
                        });
                    }
                };

                Geometry.Updated += _geometryUpdateListener.OnEvent;

                OnSurfaceBrushUpdated();
            }
        }

        /// <summary>
        /// Stroke Dependency Property
        /// </summary>
        public static readonly DependencyProperty StrokeProperty = DependencyProperty.Register(
            "Stroke",
            typeof(RenderCanvasBrushBase),
            typeof(GeometrySurfaceBrush),
            new PropertyMetadata(null, OnStrokeChanged));

        /// <summary>
        /// Gets or sets the brush for rendering the outline stroke of the geometry.
        /// </summary>
        public RenderCanvasBrushBase Stroke
        {
            get => (RenderCanvasBrushBase)GetValue(StrokeProperty);
            set => SetValue(StrokeProperty, value);
        }

        /// <summary>
        /// Handles changes to the Stroke property.
        /// </summary>
        /// <param name="d"><see cref="GeometrySurfaceBrush" />.</param>
        /// <param name="e">DependencyProperty changed event arguments.</param>
        private static void OnStrokeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var geometrySurfaceBrush = (GeometrySurfaceBrush)d;
            geometrySurfaceBrush.OnStrokeChanged();
        }

        /// <summary>
        /// Instance handler for the changes to the Stroke dependency property.
        /// </summary>
        private void OnStrokeChanged()
        {
            _strokeUpdateListener?.Detach();
            _strokeUpdateListener = null;

            if (Stroke != null)
            {
                _strokeUpdateListener = new WeakEventListener<RenderCanvasBrushBase, object, EventArgs>(Stroke)
                {
                    OnEventAction = async (instance, source, args) =>
                    {
                        await Window.Current.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                        {
                            OnSurfaceBrushUpdated();
                        });
                    }
                };

                Stroke.Updated += _strokeUpdateListener.OnEvent;

                OnSurfaceBrushUpdated();
            }
        }

        /// <summary>
        /// StrokeThickness Dependency Property
        /// </summary>
        public static readonly DependencyProperty StrokeThicknessProperty = DependencyProperty.Register(
            "StrokeThickness",
            typeof(double),
            typeof(GeometrySurfaceBrush),
            new PropertyMetadata(0d, OnStrokeThicknessChanged));

        /// <summary>
        /// Gets or sets the thickness of the outline of the geometry.
        /// </summary>
        public double StrokeThickness
        {
            get => (double)GetValue(StrokeThicknessProperty);
            set => SetValue(StrokeThicknessProperty, value);
        }

        /// <summary>
        /// Handles changes to the StrokeThickness property.
        /// </summary>
        /// <param name="d"><see cref="GeometrySurfaceBrush" />.</param>
        /// <param name="e">DependencyProperty changed event arguments.</param>
        private static void OnStrokeThicknessChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var geometrySurfaceBrush = (GeometrySurfaceBrush)d;
            geometrySurfaceBrush.OnStrokeThicknessChanged();
        }

        /// <summary>
        /// Instance handler for the changes to the StrokeThickness dependency property.
        /// </summary>
        private void OnStrokeThicknessChanged()
        {
            if (StrokeThickness < 0d)
            {
                throw new ArgumentException("StrokeThickness must be a non-negative number.");
            }

            OnSurfaceBrushUpdated();
        }

        /// <summary>
        /// RenderStrokeStyle Dependency Property
        /// </summary>
        public static readonly DependencyProperty RenderStrokeStyleProperty = DependencyProperty.Register(
            "RenderStrokeStyle",
            typeof(StrokeStyle),
            typeof(GeometrySurfaceBrush),
            new PropertyMetadata(null, OnRenderStrokeStyleChanged));

        /// <summary>
        /// Gets or sets the style of the outline for the Geometry.
        /// </summary>
        public StrokeStyle RenderStrokeStyle
        {
            get => (StrokeStyle)GetValue(RenderStrokeStyleProperty);
            set => SetValue(RenderStrokeStyleProperty, value);
        }

        /// <summary>
        /// Handles changes to the RenderStrokeStyle property.
        /// </summary>
        /// <param name="d"><see cref="GeometrySurfaceBrush" />.</param>
        /// <param name="e">DependencyProperty changed event arguments.</param>
        private static void OnRenderStrokeStyleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var geometrySurfaceBrush = (GeometrySurfaceBrush)d;
            geometrySurfaceBrush.OnRenderStrokeStyleChanged();
        }

        /// <summary>
        /// Instance handler for the changes to the RenderStrokeStyle dependency property.
        /// </summary>
        private void OnRenderStrokeStyleChanged()
        {
            _strokeStyleUpdateListener?.Detach();
            _strokeStyleUpdateListener = null;

            if (RenderStrokeStyle != null)
            {
                _strokeStyleUpdateListener = new WeakEventListener<StrokeStyle, object, EventArgs>(RenderStrokeStyle)
                {
                    OnEventAction = async (instance, source, args) =>
                    {
                        await Window.Current.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                        {
                            OnSurfaceBrushUpdated();
                        });
                    }
                };

                RenderStrokeStyle.Updated += _strokeStyleUpdateListener.OnEvent;

                OnSurfaceBrushUpdated();
            }
        }

        /// <summary>
        /// FillBrush Dependency Property
        /// </summary>
        public static readonly DependencyProperty FillBrushProperty = DependencyProperty.Register(
            "FillBrush",
            typeof(RenderCanvasBrushBase),
            typeof(GeometrySurfaceBrush),
            new PropertyMetadata(null, OnFillBrushChanged));

        /// <summary>
        /// Gets or sets the brush which will be used to fill the geometry.
        /// </summary>
        public RenderCanvasBrushBase FillBrush
        {
            get => (RenderCanvasBrushBase)GetValue(FillBrushProperty);
            set => SetValue(FillBrushProperty, value);
        }

        /// <summary>
        /// Handles changes to the FillBrush property.
        /// </summary>
        /// <param name="d"><see cref="GeometrySurfaceBrush" />.</param>
        /// <param name="e">DependencyProperty changed event arguments.</param>
        private static void OnFillBrushChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var geometrySurfaceBrush = (GeometrySurfaceBrush)d;
            geometrySurfaceBrush.OnFillBrushChanged();
        }

        /// <summary>
        /// Instance handler for the changes to the FillBrush dependency property.
        /// </summary>
        private void OnFillBrushChanged()
        {
            _fillBrushUpdateListener?.Detach();
            _fillBrushUpdateListener = null;

            if (FillBrush != null)
            {
                _fillBrushUpdateListener = new WeakEventListener<RenderCanvasBrushBase, object, EventArgs>(FillBrush)
                {
                    OnEventAction = async (instance, source, args) =>
                    {
                        await Window.Current.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                        {
                            OnSurfaceBrushUpdated();
                        });
                    }
                };

                FillBrush.Updated += _fillBrushUpdateListener.OnEvent;

                OnSurfaceBrushUpdated();
            }
        }

        /// <summary>
        /// BackgroundBrush Dependency Property
        /// </summary>
        public static readonly DependencyProperty BackgroundBrushProperty = DependencyProperty.Register(
            "BackgroundBrush",
            typeof(RenderCanvasBrushBase),
            typeof(GeometrySurfaceBrush),
            new PropertyMetadata(null, OnBackgroundBrushChanged));

        /// <summary>
        /// Gets or sets the brush with which the background of the Geometry surface will be rendered.
        /// </summary>
        public RenderCanvasBrushBase BackgroundBrush
        {
            get => (RenderCanvasBrushBase)GetValue(BackgroundBrushProperty);
            set => SetValue(BackgroundBrushProperty, value);
        }

        /// <summary>
        /// Handles changes to the BackgroundBrush property.
        /// </summary>
        /// <param name="d"><see cref="GeometrySurfaceBrush" />.</param>
        /// <param name="e">DependencyProperty changed event arguments.</param>
        private static void OnBackgroundBrushChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var geometrySurfaceBrush = (GeometrySurfaceBrush)d;
            geometrySurfaceBrush.OnBackgroundBrushChanged();
        }

        /// <summary>
        /// Instance handler for the changes to the BackgroundBrush dependency property.
        /// </summary>
        private void OnBackgroundBrushChanged()
        {
            _bgBrushUpdateListener?.Detach();
            _bgBrushUpdateListener = null;

            if (BackgroundBrush != null)
            {
                _bgBrushUpdateListener = new WeakEventListener<RenderCanvasBrushBase, object, EventArgs>(BackgroundBrush)
                {
                    OnEventAction = async (instance, source, args) =>
                    {
                        await Window.Current.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                        {
                            OnSurfaceBrushUpdated();
                        });
                    }
                };

                BackgroundBrush.Updated += _bgBrushUpdateListener.OnEvent;

                OnSurfaceBrushUpdated();
            }
        }

        /// <inheritdoc/>
        protected override void OnSurfaceBrushUpdated(bool createSurface = false)
        {
            if (Generator == null)
            {
                GetGeneratorInstance();
            }

            if (Geometry == null || Generator == null)
            {
                return;
            }

            var transparentBrush = new CanvasSolidColorBrush(Generator.Device, Colors.Transparent);
            var strokeBrush = (Stroke == null) ? transparentBrush : Stroke.CanvasBrush;
            var fillBrush = (FillBrush == null) ? transparentBrush : FillBrush.CanvasBrush;
            var bgBrush = (BackgroundBrush == null) ? transparentBrush : BackgroundBrush.CanvasBrush;
            var strokeStyle = (RenderStrokeStyle == null) ? new CanvasStrokeStyle() : RenderStrokeStyle.GetCanvasStrokeStyle();
            var canvasstroke = new CanvasStroke(strokeBrush, (float)StrokeThickness, strokeStyle);

            if (createSurface || (RenderSurface == null))
            {
                CompositionBrush?.Dispose();
                RenderSurface = Generator.CreateGeometrySurface(new Size(SurfaceWidth, SurfaceHeight), Geometry.Geometry, canvasstroke, fillBrush, bgBrush);
                CompositionBrush = Window.Current.Compositor.CreateSurfaceBrush(RenderSurface.Surface);
            }
            else
            {
                ((IGeometrySurface)RenderSurface).Redraw(new Size(SurfaceWidth, SurfaceHeight), Geometry.Geometry, canvasstroke, fillBrush, bgBrush);
            }

            base.OnSurfaceBrushUpdated(createSurface);
        }
    }
}
