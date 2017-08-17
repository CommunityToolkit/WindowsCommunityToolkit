// ******************************************************************
// Copyright (c) Microsoft. All rights reserved.
// This code is licensed under the MIT License (MIT).
// THE CODE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
// TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH
// THE CODE OR THE USE OR OTHER DEALINGS IN THE CODE.
// ******************************************************************

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Markup;
using Windows.UI.Xaml.Media;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    /// <summary>
    /// Control that implements support for transformations as if applied by LayoutTransform.
    /// </summary>
    [ContentProperty(Name = "Child")]
    public partial class LayoutTransformControl : Control
    {
        /// <summary>
        /// Value used to work around double arithmetic rounding issues.
        /// </summary>
        private const double AcceptableDelta = 0.0001;

        /// <summary>
        /// Value used to work around double arithmetic rounding issues.
        /// </summary>
        private const int DecimalsAfterRound = 4;

        /// <summary>
        /// List of property change event sources for events when properties of the Transform tree change
        /// </summary>
        private readonly Dictionary<Transform, List<PropertyChangeEventSource<double>>>
            _transformPropertyChangeEventSources = new Dictionary
                <Transform, List<PropertyChangeEventSource<double>>>();

        /// <summary>
        /// Host panel for Child element.
        /// </summary>
        private Panel _layoutRoot;

        /// <summary>
        /// RenderTransform/MatrixTransform applied to layout root.
        /// </summary>
        private MatrixTransform _matrixTransform;

        /// <summary>
        /// Transformation matrix corresponding to matrix transform.
        /// </summary>
        private Matrix _transformation;

        /// <summary>
        /// Actual DesiredSize of Child element.
        /// </summary>
        private Size _childActualSize = Size.Empty;

        /// <summary>
        /// Initializes a new instance of the <see cref="LayoutTransformControl"/> class.
        /// </summary>
        public LayoutTransformControl()
        {
            DefaultStyleKey = typeof(LayoutTransformControl);

            // Can't tab to LayoutTransformControl
            IsTabStop = false;

            // Disable layout rounding because its rounding of values confuses things.
            UseLayoutRounding = false;
        }

        /// <summary>
        /// Called whenever the control's template changes.
        /// </summary>
        protected override void OnApplyTemplate()
        {
            // Save existing content and remove it from the visual tree
            FrameworkElement savedContent = Child;
            Child = null;

            // Apply new template
            base.OnApplyTemplate();

            // Find template parts
            _layoutRoot = GetTemplateChild("LayoutRoot") as Panel;
            _matrixTransform = GetTemplateChild("MatrixTransform") as MatrixTransform;

            // RestoreAsync saved content
            Child = savedContent;

            // Apply the current transform
            TransformUpdated();
        }

        /// <summary>
        /// Notifies the LayoutTransformControl that some aspect of its Transform property has changed.
        /// </summary>
        /// <remarks>
        /// Call this to update the LayoutTransform in cases where
        /// LayoutTransformControl wouldn't otherwise know to do so.
        /// </remarks>
        public void TransformUpdated()
        {
            ProcessTransform();
        }

        /// <summary>
        /// Return true if Size a is smaller than Size b in either dimension.
        /// </summary>
        /// <param name="a">The left size.</param>
        /// <param name="b">The right size.</param>
        /// <returns>A value indicating whether the left size is smaller than
        /// the right.</returns>
        private static bool IsSizeSmaller(Size a, Size b)
        {
            // WPF equivalent of following code:
            // return ((a.Width < b.Width) || (a.Height < b.Height));
            return (a.Width + AcceptableDelta < b.Width) || (a.Height + AcceptableDelta < b.Height);
        }

        /// <summary>
        /// Rounds the non-offset elements of a matrix to avoid issues due to floating point imprecision.
        /// </summary>
        /// <param name="matrix">The matrix to round.</param>
        /// <param name="decimalsAfterRound">The number of decimals after the round.</param>
        /// <returns>The rounded matrix.</returns>
        private static Matrix RoundMatrix(Matrix matrix, int decimalsAfterRound)
        {
            return new Matrix(
                Math.Round(matrix.M11, decimalsAfterRound),
                Math.Round(matrix.M12, decimalsAfterRound),
                Math.Round(matrix.M21, decimalsAfterRound),
                Math.Round(matrix.M22, decimalsAfterRound),
                matrix.OffsetX,
                matrix.OffsetY);
        }

        /// <summary>
        /// Implement WPF's Rect.Transform.
        /// </summary>
        /// <param name="rectangle">The rectangle to transform.</param>
        /// <param name="matrix">The matrix to use to transform the rectangle.
        /// </param>
        /// <returns>The transformed rectangle.</returns>
        private static Rect RectTransform(Rect rectangle, Matrix matrix)
        {
            // WPF equivalent of following code:
            // var rectTransformed = Rect.Transform(rect, matrix);
            Point leftTop = matrix.Transform(new Point(rectangle.Left, rectangle.Top));
            Point rightTop = matrix.Transform(new Point(rectangle.Right, rectangle.Top));
            Point leftBottom = matrix.Transform(new Point(rectangle.Left, rectangle.Bottom));
            Point rightBottom = matrix.Transform(new Point(rectangle.Right, rectangle.Bottom));
            double left = Math.Min(Math.Min(leftTop.X, rightTop.X), Math.Min(leftBottom.X, rightBottom.X));
            double top = Math.Min(Math.Min(leftTop.Y, rightTop.Y), Math.Min(leftBottom.Y, rightBottom.Y));
            double right = Math.Max(Math.Max(leftTop.X, rightTop.X), Math.Max(leftBottom.X, rightBottom.X));
            double bottom = Math.Max(Math.Max(leftTop.Y, rightTop.Y), Math.Max(leftBottom.Y, rightBottom.Y));
            Rect rectTransformed = new Rect(left, top, right - left, bottom - top);
            return rectTransformed;
        }

        /// <summary>
        /// Implements WPF's Matrix.Multiply.
        /// </summary>
        /// <param name="matrix1">The left matrix.</param>
        /// <param name="matrix2">The right matrix.</param>
        /// <returns>The product of the two matrices.</returns>
        private static Matrix MatrixMultiply(Matrix matrix1, Matrix matrix2)
        {
            // WPF equivalent of following code:
            // return Matrix.Multiply(matrix1, matrix2);
            return new Matrix(
                (matrix1.M11 * matrix2.M11) + (matrix1.M12 * matrix2.M21),
                (matrix1.M11 * matrix2.M12) + (matrix1.M12 * matrix2.M22),
                (matrix1.M21 * matrix2.M11) + (matrix1.M22 * matrix2.M21),
                (matrix1.M21 * matrix2.M12) + (matrix1.M22 * matrix2.M22),
                ((matrix1.OffsetX * matrix2.M11) + (matrix1.OffsetY * matrix2.M21)) + matrix2.OffsetX,
                ((matrix1.OffsetX * matrix2.M12) + (matrix1.OffsetY * matrix2.M22)) + matrix2.OffsetY);
        }

        /// <summary>
        /// Implements WPF's Matrix.HasInverse.
        /// </summary>
        /// <param name="matrix">The matrix.</param>
        /// <returns>True if matrix has an inverse.</returns>
        private static bool MatrixHasInverse(Matrix matrix)
        {
            // WPF equivalent of following code:
            // return matrix.HasInverse;
            return ((matrix.M11 * matrix.M22) - (matrix.M12 * matrix.M21)) != 0;
        }

        /// <summary>
        /// Processes the current transform to determine the corresponding
        /// matrix.
        /// </summary>
        private void ProcessTransform()
        {
            // Get the transform matrix and apply it
            _transformation = RoundMatrix(GetTransformMatrix(Transform), DecimalsAfterRound);

            if (_matrixTransform != null)
            {
                _matrixTransform.Matrix = _transformation;
            }

            // New transform means re-layout is necessary
            InvalidateMeasure();
        }

        /// <summary>
        /// Walks the Transform and returns the corresponding matrix.
        /// </summary>
        /// <param name="transform">The transform to create a matrix for.
        /// </param>
        /// <returns>The matrix calculated from the transform.</returns>
        private Matrix GetTransformMatrix(Transform transform)
        {
            if (transform != null)
            {
                // WPF equivalent of this entire method (why oh why only WPF...):
                // return transform.Value;

                // Process the TransformGroup
                var transformGroup = transform as TransformGroup;

                if (transformGroup != null)
                {
                    var groupMatrix = Matrix.Identity;

                    foreach (var child in transformGroup.Children)
                    {
                        groupMatrix = MatrixMultiply(groupMatrix, GetTransformMatrix(child));
                    }

                    return groupMatrix;
                }

                // Process the RotateTransform
                var rotateTransform = transform as RotateTransform;

                if (rotateTransform != null)
                {
                    var angle = rotateTransform.Angle;
                    var angleRadians = (2 * Math.PI * angle) / 360;
                    var sine = Math.Sin(angleRadians);
                    var cosine = Math.Cos(angleRadians);

                    return new Matrix(cosine, sine, -sine, cosine, 0, 0);
                }

                // Process the ScaleTransform
                var scaleTransform = transform as ScaleTransform;

                if (scaleTransform != null)
                {
                    var scaleX = scaleTransform.ScaleX;
                    var scaleY = scaleTransform.ScaleY;

                    return new Matrix(scaleX, 0, 0, scaleY, 0, 0);
                }

                // Process the SkewTransform
                var skewTransform = transform as SkewTransform;

                if (skewTransform != null)
                {
                    var angleX = skewTransform.AngleX;
                    var angleY = skewTransform.AngleY;
                    var angleXRadians = (2 * Math.PI * angleX) / 360;
                    var angleYRadians = (2 * Math.PI * angleY) / 360;

                    return new Matrix(1, angleYRadians, angleXRadians, 1, 0, 0);
                }

                // Process the MatrixTransform
                var matrixTransform = transform as MatrixTransform;
                if (matrixTransform != null)
                {
                    return matrixTransform.Matrix;
                }

                if (transform is CompositeTransform)
                {
                    throw new NotSupportedException("CompositeTransforms are not supported (yet) by the LayoutTransformControl.");
                }

                // TranslateTransform has no effect in LayoutTransform
            }

            // Fall back to no-op transformation
            return Matrix.Identity;
        }

        /// <summary>
        /// Provides the behavior for the "Measure" pass of layout.
        /// </summary>
        /// <param name="availableSize">The available size that this element can
        /// give to child elements. Infinity can be specified as a value to
        /// indicate that the element will size to whatever content is available.</param>
        /// <returns>The size that this element determines it needs during
        /// layout, based on its calculations of child element sizes.</returns>
        protected override Size MeasureOverride(Size availableSize)
        {
            FrameworkElement child = Child;
            if (_layoutRoot == null || child == null)
            {
                // No content, no size
                return Size.Empty;
            }

            Size measureSize;
            if (_childActualSize == Size.Empty)
            {
                // Determine the largest size after the transformation
                measureSize = ComputeLargestTransformedSize(availableSize);
            }
            else
            {
                // Previous measure/arrange pass determined that Child.DesiredSize was larger than believed.
                measureSize = _childActualSize;
            }

            // Perform a mesaure on the _layoutRoot (containing Child)
            _layoutRoot.Measure(measureSize);

            // Transform DesiredSize to find its width/height
            Rect transformedDesiredRect = RectTransform(new Rect(0, 0, _layoutRoot.DesiredSize.Width, _layoutRoot.DesiredSize.Height), _transformation);
            Size transformedDesiredSize = new Size(transformedDesiredRect.Width, transformedDesiredRect.Height);

            // Return result to allocate enough space for the transformation
            return transformedDesiredSize;
        }

        /// <summary>
        /// Provides the behavior for the "Arrange" pass of layout.
        /// </summary>
        /// <param name="finalSize">The final area within the parent that this
        /// element should use to arrange itself and its children.</param>
        /// <returns>The actual size used.</returns>
        protected override Size ArrangeOverride(Size finalSize)
        {
            FrameworkElement child = Child;
            if (_layoutRoot == null || child == null)
            {
                // No child, use whatever was given
                return finalSize;
            }

            // Determine the largest available size after the transformation
            Size finalSizeTransformed = ComputeLargestTransformedSize(finalSize);
            if (IsSizeSmaller(finalSizeTransformed, _layoutRoot.DesiredSize))
            {
                // Some elements do not like being given less space than they asked for (ex: TextBlock)
                // Bump the working size up to do the right thing by them
                finalSizeTransformed = _layoutRoot.DesiredSize;
            }

            // Transform the working size to find its width/height
            Rect transformedRect = RectTransform(new Rect(0, 0, finalSizeTransformed.Width, finalSizeTransformed.Height), _transformation);

            // Create the Arrange rect to center the transformed content
            Rect finalRect = new Rect(
                -transformedRect.Left + ((finalSize.Width - transformedRect.Width) / 2),
                -transformedRect.Top + ((finalSize.Height - transformedRect.Height) / 2),
                finalSizeTransformed.Width,
                finalSizeTransformed.Height);

            // Perform an Arrange on _layoutRoot (containing Child)
            _layoutRoot.Arrange(finalRect);

            // This is the first opportunity to find out the Child's true DesiredSize
            if (IsSizeSmaller(finalSizeTransformed, child.RenderSize) && (Size.Empty == _childActualSize))
            {
                // Unfortunately, all the work so far is invalid because the wrong DesiredSize was used
                // Make a note of the actual DesiredSize
                _childActualSize = new Size(child.ActualWidth, child.ActualHeight);

                // Force a new measure/arrange pass
                InvalidateMeasure();
            }
            else
            {
                // Clear the "need to measure/arrange again" flag
                _childActualSize = Size.Empty;
            }

            // Return result to perform the transformation
            return finalSize;
        }

        /// <summary>
        /// Computes the largest usable size after applying the transformation to the specified bounds.
        /// </summary>
        /// <param name="arrangeBounds">The size to arrange within.</param>
        /// <returns>The size required.</returns>
        [SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity", Justification = "Closely corresponds to WPF's FrameworkElement.FindMaximalAreaLocalSpaceRect.")]
        private Size ComputeLargestTransformedSize(Size arrangeBounds)
        {
            // Computed largest transformed size
            Size computedSize = Size.Empty;

            // Detect infinite bounds and constrain the scenario
            bool infiniteWidth = double.IsInfinity(arrangeBounds.Width);

            if (infiniteWidth)
            {
                arrangeBounds.Width = arrangeBounds.Height;
            }

            bool infiniteHeight = double.IsInfinity(arrangeBounds.Height);

            if (infiniteHeight)
            {
                arrangeBounds.Height = arrangeBounds.Width;
            }

            // Capture the matrix parameters
            double a = _transformation.M11;
            double b = _transformation.M12;
            double c = _transformation.M21;
            double d = _transformation.M22;

            // Compute maximum possible transformed width/height based on starting width/height
            // These constraints define two lines in the positive x/y quadrant
            double maxWidthFromWidth = Math.Abs(arrangeBounds.Width / a);
            double maxHeightFromWidth = Math.Abs(arrangeBounds.Width / c);
            double maxWidthFromHeight = Math.Abs(arrangeBounds.Height / b);
            double maxHeightFromHeight = Math.Abs(arrangeBounds.Height / d);

            // The transformed width/height that maximize the area under each segment is its midpoint
            // At most one of the two midpoints will satisfy both constraints
            double idealWidthFromWidth = maxWidthFromWidth / 2;
            double idealHeightFromWidth = maxHeightFromWidth / 2;
            double idealWidthFromHeight = maxWidthFromHeight / 2;
            double idealHeightFromHeight = maxHeightFromHeight / 2;

            // Compute slope of both constraint lines
            double slopeFromWidth = -(maxHeightFromWidth / maxWidthFromWidth);
            double slopeFromHeight = -(maxHeightFromHeight / maxWidthFromHeight);

            if (arrangeBounds.Width == 0 || arrangeBounds.Height == 0)
            {
                // Check for empty bounds
                computedSize = new Size(0, 0);
            }
            else if (infiniteWidth && infiniteHeight)
            {
                // Check for completely unbound scenario
                computedSize = new Size(double.PositiveInfinity, double.PositiveInfinity);
            }
            else if (!MatrixHasInverse(_transformation))
            {
                // Check for singular matrix
                computedSize = new Size(0, 0);
            }
            else if (b == 0 || c == 0)
            {
                // Check for 0/180 degree special cases
                double maxHeight = infiniteHeight ? double.PositiveInfinity : maxHeightFromHeight;
                double maxWidth = infiniteWidth ? double.PositiveInfinity : maxWidthFromWidth;

                if (b == 0 && c == 0)
                {
                    // No constraints
                    computedSize = new Size(maxWidth, maxHeight);
                }
                else if (b == 0)
                {
                    // Constrained by width
                    double computedHeight = Math.Min(idealHeightFromWidth, maxHeight);
                    computedSize = new Size(
                        maxWidth - Math.Abs((c * computedHeight) / a),
                        computedHeight);
                }
                else if (c == 0)
                {
                    // Constrained by height
                    double computedWidth = Math.Min(idealWidthFromHeight, maxWidth);
                    computedSize = new Size(
                        computedWidth,
                        maxHeight - Math.Abs((b * computedWidth) / d));
                }
            }
            else if (a == 0 || d == 0)
            {
                // Check for 90/270 degree special cases
                double maxWidth = infiniteHeight ? double.PositiveInfinity : maxWidthFromHeight;
                double maxHeight = infiniteWidth ? double.PositiveInfinity : maxHeightFromWidth;

                if (a == 0 && d == 0)
                {
                    // No constraints
                    computedSize = new Size(maxWidth, maxHeight);
                }
                else if (a == 0)
                {
                    // Constrained by width
                    double computedHeight = Math.Min(idealHeightFromHeight, maxHeight);
                    computedSize = new Size(
                        maxWidth - Math.Abs((d * computedHeight) / b),
                        computedHeight);
                }
                else if (d == 0)
                {
                    // Constrained by height.
                    double computedWidth = Math.Min(idealWidthFromWidth, maxWidth);
                    computedSize = new Size(
                        computedWidth,
                        maxHeight - Math.Abs((a * computedWidth) / c));
                }
            }
            else if (idealHeightFromWidth <= ((slopeFromHeight * idealWidthFromWidth) + maxHeightFromHeight))
            {
                // Check the width midpoint for viability (by being below the height constraint line).
                computedSize = new Size(idealWidthFromWidth, idealHeightFromWidth);
            }
            else if (idealHeightFromHeight <= ((slopeFromWidth * idealWidthFromHeight) + maxHeightFromWidth))
            {
                // Check the height midpoint for viability (by being below the width constraint line).
                computedSize = new Size(idealWidthFromHeight, idealHeightFromHeight);
            }
            else
            {
                // Neither midpoint is viable; use the intersection of the two constraint lines instead.

                // Compute width by setting heights equal (m1*x+c1=m2*x+c2).
                double computedWidth = (maxHeightFromHeight - maxHeightFromWidth) / (slopeFromWidth - slopeFromHeight);

                // Compute height from width constraint line (y=m*x+c; using height would give same result).
                computedSize = new Size(
                    computedWidth,
                    (slopeFromWidth * computedWidth) + maxHeightFromWidth);
            }

            return computedSize;
        }
    }
}