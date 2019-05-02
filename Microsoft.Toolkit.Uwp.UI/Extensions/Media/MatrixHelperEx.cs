// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Windows.Foundation;
using Windows.UI.Xaml.Media;

namespace Microsoft.Toolkit.Uwp.UI.Extensions
{
    // TODO: Check if we can make these static extension methods of the MatrixHelper in C#8 or move to an operator extension.

    /// <summary>
    /// Static helper methods for <see cref="o:Windows.UI.Xaml.Media.Matrix"/>.
    /// </summary>
    public static class MatrixHelperEx
    {
        /// <summary>
        /// Implements WPF's Matrix.Multiply.
        /// </summary>
        /// <param name="matrix1">The left matrix.</param>
        /// <param name="matrix2">The right matrix.</param>
        /// <returns>The product of the two matrices.</returns>
        public static Matrix Multiply(Matrix matrix1, Matrix matrix2)
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
        /// Rounds the non-offset elements of a matrix to avoid issues due to floating point imprecision and returns the result.
        /// </summary>
        /// <param name="matrix">The matrix to round.</param>
        /// <param name="decimalsAfterRound">The number of decimals after the round.</param>
        /// <returns>The rounded matrix.</returns>
        public static Matrix Round(Matrix matrix, int decimalsAfterRound)
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
        public static Rect RectTransform(Rect rectangle, Matrix matrix)
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
    }
}
