using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Toolkit.Uwp.UI.Media.Geometry;
using Windows.UI.Xaml;

namespace Microsoft.Toolkit.Uwp.UI.Media.Brushes
{
    /// <summary>
    /// Represents a brush
    /// </summary>
    public sealed class GeometryMaskSurfaceBrush : RenderSurfaceBrushBase
    {
        /// <summary>
        /// Geometry Dependency Property
        /// </summary>
        public static readonly DependencyProperty GeometryProperty = DependencyProperty.Register(
            "Geometry",
            typeof(CanvasCoreGeometry),
            typeof(GeometryMaskSurfaceBrush),
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
        /// <param name="d"><see cref="GeometryMaskSurfaceBrush" /></param>
        /// <param name="e">DependencyProperty changed event arguments</param>
        private static void OnGeometryChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var maskSurfaceBrush = (GeometryMaskSurfaceBrush)d;
            maskSurfaceBrush.OnGeometryChanged();
        }

        /// <summary>
        /// Instance handler for the changes to the Geometry dependency property.
        /// </summary>
        private void OnGeometryChanged()
        {
        }

        /// <summary>
        /// Source Dependency Property
        /// </summary>
        public static readonly DependencyProperty SourceProperty = DependencyProperty.Register(
            "Source",
            typeof(RenderSurfaceBrushBase),
            typeof(GeometryMaskSurfaceBrush),
            new PropertyMetadata(null, OnSourceChanged));

        /// <summary>
        /// Gets or sets the <see cref="RenderSurfaceBrushBase"/> on which the mask needs to be applied.
        /// </summary>
        public RenderSurfaceBrushBase Source
        {
            get => (RenderSurfaceBrushBase)GetValue(SourceProperty);
            set => SetValue(SourceProperty, value);
        }

        /// <summary>
        /// Handles changes to the Source property.
        /// </summary>
        /// <param name="d"><see cref="GeometryMaskSurfaceBrush" /></param>
        /// <param name="e">DependencyProperty changed event arguments</param>
        private static void OnSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var maskSurfaceBrush = (GeometryMaskSurfaceBrush)d;
            maskSurfaceBrush.OnSourceChanged();
        }

        /// <summary>
        /// Instance handler for the changes to the Source dependency property.
        /// </summary>
        private void OnSourceChanged()
        {
        }

        /// <inheritdoc/>
        protected override void OnSurfaceBrushUpdated()
        {
            base.OnSurfaceBrushUpdated();

            if (Source != null && Geometry != null)
            {
                var maskBrush = Window.Current.Compositor.CreateMaskBrush();
                maskBrush.Source = Source.Brush;
            }
        }
    }
}
