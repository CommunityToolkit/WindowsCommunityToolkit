// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Numerics;
using System.Threading.Tasks;
using Microsoft.Toolkit.Uwp.UI.Media.Pipelines;
using Windows.Foundation;
using Windows.UI.Composition;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Markup;
using Windows.UI.Xaml.Media;

namespace Microsoft.Toolkit.Uwp.UI.Media
{
    /// <summary>
    /// Contains the properties for passthrue to the <see cref="CompositionSurfaceBrush"/>.
    /// </summary>
    public sealed partial class SurfaceBrushFactory
    {
        /// <summary>
        /// Gets or sets the <see cref="CompositionSurfaceBrush.AnchorPoint"/> property of the underlying <see cref="CompositionSurfaceBrush"/>.
        /// </summary>
        public Point AnchorPoint
        {
            get => _brush.AnchorPoint.ToPoint();
            set => _brush.AnchorPoint = value.ToVector2();
        }

        /// <summary>
        /// Gets or sets the <see cref="CompositionSurfaceBrush.BitmapInterpolationMode"/> property of the underlying <see cref="CompositionSurfaceBrush"/>.
        /// </summary>
        public CompositionBitmapInterpolationMode BitmapInterpolationMode
        {
            get => _brush.BitmapInterpolationMode;
            set => _brush.BitmapInterpolationMode = value;
        }

        /// <summary>
        /// Gets or sets the <see cref="CompositionSurfaceBrush.CenterPoint"/> property of the underlying <see cref="CompositionSurfaceBrush"/>.
        /// </summary>
        public Point CenterPoint
        {
            get => _brush.CenterPoint.ToPoint();
            set => _brush.CenterPoint = value.ToVector2();
        }

        /// <summary>
        /// Gets or sets the <see cref="CompositionSurfaceBrush.HorizontalAlignmentRatio"/> property of the underlying <see cref="CompositionSurfaceBrush"/>.
        /// </summary>
        public double HorizontalAlignmentRatio
        {
            get => _brush.HorizontalAlignmentRatio;
            set => _brush.HorizontalAlignmentRatio = (float)Math.Clamp(value, 0.0f, 1.0f);
        }

        /// <summary>
        /// Gets or sets the <see cref="CompositionSurfaceBrush.Offset"/> property of the underlying <see cref="CompositionSurfaceBrush"/>.
        /// </summary>
        public Point Offset
        {
            get => _brush.Offset.ToPoint();
            set => _brush.Offset = value.ToVector2();
        }

        /// <summary>
        /// Gets or sets the <see cref="CompositionSurfaceBrush.RotationAngle"/> property of the underlying <see cref="CompositionSurfaceBrush"/>.
        /// </summary>
        public double RotationAngle
        {
            get => _brush.RotationAngle;
            set => _brush.RotationAngle = (float)value;
        }

        /// <summary>
        /// Gets or sets the <see cref="CompositionSurfaceBrush.RotationAngleInDegrees"/> property of the underlying <see cref="CompositionSurfaceBrush"/>.
        /// </summary>
        public double RotationAngleInDegrees
        {
            get => _brush.RotationAngleInDegrees;
            set => _brush.RotationAngleInDegrees = (float)value;
        }

        /// <summary>
        /// Gets or sets the <see cref="CompositionSurfaceBrush.Scale"/> property of the underlying <see cref="CompositionSurfaceBrush"/>.
        /// </summary>
        public Point Scale
        {
            get => _brush.Scale.ToPoint();
            set => _brush.Scale = value.ToVector2();
        }

        /// <summary>
        /// Gets or sets a value indicating whether <see cref="CompositionSurfaceBrush.SnapToPixels"/> property of the underlying <see cref="CompositionSurfaceBrush"/> is set.
        /// </summary>
        public bool SnapToPixels
        {
            get => _brush.SnapToPixels;
            set => _brush.SnapToPixels = value;
        }

        /// <summary>
        /// Gets or sets the <see cref="CompositionSurfaceBrush.Stretch"/> property of the underlying <see cref="CompositionSurfaceBrush"/>.
        /// </summary>
        public CompositionStretch Stretch
        {
            get => _brush.Stretch;
            set => _brush.Stretch = value;
        }

        /// <summary>
        /// Gets or sets the <see cref="CompositionSurfaceBrush.TransformMatrix"/> property of the underlying <see cref="CompositionSurfaceBrush"/>.
        /// </summary>
        public Matrix3x2 TransformMatrix
        {
            get => _brush.TransformMatrix;
            set => _brush.TransformMatrix = value;
        }

        /// <summary>
        /// Gets or sets the <see cref="CompositionSurfaceBrush.VerticalAlignmentRatio"/> property of the underlying <see cref="CompositionSurfaceBrush"/>.
        /// </summary>
        public double VerticalAlignmentRatio
        {
            get => _brush.VerticalAlignmentRatio;
            set => _brush.VerticalAlignmentRatio = (float)Math.Clamp(value, 0.0f, 1.0f);
        }

    }
}
