// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.ComponentModel;
using System.Numerics;
using Microsoft.Graphics.Canvas.Geometry;
using Microsoft.Toolkit.Uwp.UI.Converters;
using Microsoft.Toolkit.Uwp.UI.Media.Surface;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;

namespace Microsoft.Toolkit.Uwp.UI.Media.Geometry
{
    /// <summary>
    /// Represents a Geometry defined by the combination of two <see cref="CanvasCoreGeometry"/> objects.
    /// </summary>
    public class CanvasCombinedGeometry : CanvasCoreGeometry
    {
        /// <summary>
        /// Geometry1 Dependency Property
        /// </summary>
        public static readonly DependencyProperty Geometry1Property = DependencyProperty.Register(
            "Geometry1",
            typeof(CanvasCoreGeometry),
            typeof(CanvasCombinedGeometry),
            new PropertyMetadata(null, OnGeometry1Changed));

        /// <summary>
        /// Gets or sets the first <see cref="CanvasCoreGeometry"/> object to combine.
        /// </summary>
        public CanvasCoreGeometry Geometry1
        {
            get => (CanvasCoreGeometry)GetValue(Geometry1Property);
            set => SetValue(Geometry1Property, value);
        }

        /// <summary>
        /// Handles changes to the Geometry1 property.
        /// </summary>
        /// <param name="d"><see cref="CanvasCombinedGeometry"/></param>
        /// <param name="e">DependencyProperty changed event arguments</param>
        private static void OnGeometry1Changed(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var combinedGeometry = (CanvasCombinedGeometry)d;
            combinedGeometry.OnGeometry1Changed();
        }

        /// <summary>
        /// Instance handler for the changes to the Geometry1 dependency property.
        /// </summary>
        private void OnGeometry1Changed()
        {
            UpdateGeometry();
        }

        /// <summary>
        /// Geometry2 Dependency Property
        /// </summary>
        public static readonly DependencyProperty Geometry2Property = DependencyProperty.Register(
            "Geometry2",
            typeof(CanvasCoreGeometry),
            typeof(CanvasCombinedGeometry),
            new PropertyMetadata(null, OnGeometry2Changed));

        /// <summary>
        /// Gets or sets the second <see cref="CanvasCoreGeometry"/> to combine.
        /// </summary>
        public CanvasCoreGeometry Geometry2
        {
            get => (CanvasCoreGeometry)GetValue(Geometry2Property);
            set => SetValue(Geometry2Property, value);
        }

        /// <summary>
        /// Handles changes to the Geometry2 property.
        /// </summary>
        /// <param name="d"><see cref="CanvasCombinedGeometry" /></param>
        /// <param name="e">DependencyProperty changed event arguments</param>
        private static void OnGeometry2Changed(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var combinedGeometry = (CanvasCombinedGeometry)d;
            combinedGeometry.OnGeometry2Changed();
        }

        /// <summary>
        /// Instance handler for the changes to the Geometry2 dependency property.
        /// </summary>
        private void OnGeometry2Changed()
        {
            UpdateGeometry();
        }

        /// <summary>
        /// Transform Dependency Property
        /// </summary>
        public static readonly DependencyProperty TransformProperty = DependencyProperty.Register(
            "Transform",
            typeof(MatrixTransform),
            typeof(CanvasCombinedGeometry),
            new PropertyMetadata(Matrix3x2.Identity.ToMatrixTransform(), OnTransformChanged));

        /// <summary>
        /// Gets or sets the MatrixTransform to be applied to Geometry2 before combining with Geometry1.
        /// </summary>
        public MatrixTransform Transform
        {
            get => (MatrixTransform)GetValue(TransformProperty);
            set => SetValue(TransformProperty, value);
        }

        /// <summary>
        /// Handles changes to the Transform property.
        /// </summary>
        /// <param name="d"><see cref="CanvasCombinedGeometry" /></param>
        /// <param name="e">DependencyProperty changed event arguments</param>
        private static void OnTransformChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var combinedGeometry = (CanvasCombinedGeometry)d;
            combinedGeometry.OnTransformChanged();
        }

        /// <summary>
        /// Instance handler for the changes to the Transform dependency property.
        /// </summary>
        private void OnTransformChanged()
        {
            UpdateGeometry();
        }

        /// <summary>
        /// GeometryCombineMode Dependency Property
        /// </summary>
        public static readonly DependencyProperty GeometryCombineModeProperty = DependencyProperty.Register(
            "GeometryCombineMode",
            typeof(CanvasGeometryCombine),
            typeof(CanvasCombinedGeometry),
            new PropertyMetadata(CanvasGeometryCombine.Union, OnGeometryCombineModeChanged));

        /// <summary>
        /// Gets or sets the method by which the geometries specified by <see cref="Geometry1"/> and <see cref="Geometry2"/> are meant to be combined.
        /// </summary>
        public CanvasGeometryCombine GeometryCombineMode
        {
            get => (CanvasGeometryCombine)GetValue(GeometryCombineModeProperty);
            set => SetValue(GeometryCombineModeProperty, value);
        }

        /// <summary>
        /// Handles changes to the GeometryCombineMode property.
        /// </summary>
        /// <param name="d"><see cref="CanvasCombinedGeometry" /></param>
        /// <param name="e">DependencyProperty changed event arguments</param>
        private static void OnGeometryCombineModeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var combinedGeometry = (CanvasCombinedGeometry)d;
            combinedGeometry.OnGeometryCombineModeChanged();
        }

        /// <summary>
        /// Instance handler for the changes to the GeometryCombineMode dependency property.
        /// </summary>
        private void OnGeometryCombineModeChanged()
        {
            UpdateGeometry();
        }

        /// <inheritdoc/>
        protected override void UpdateGeometry()
        {
            if (Geometry1?.Geometry == null || Geometry2?.Geometry == null)
            {
                Geometry = null;
                return;
            }

            Geometry = Geometry1.Geometry.CombineWith(Geometry2.Geometry, Transform.ToMatrix3x2(), GeometryCombineMode);
        }
    }
}
