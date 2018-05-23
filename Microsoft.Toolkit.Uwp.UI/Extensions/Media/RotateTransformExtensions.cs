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

using Windows.UI.Xaml.Media;

namespace Microsoft.Toolkit.Uwp.UI.Extensions.Media
{
    /// <summary>
    /// Extension methods for <see cref="RotateTransform"/>.
    /// </summary>
    public static class RotateTransformExtensions
    {
        /// <summary>
        /// Gets the matrix that represents this transform.
        /// Implements WPF's SkewTransform.Value.
        /// </summary>
        /// <param name="transform">Extended SkewTranform.</param>
        /// <returns>Matrix representing transform.</returns>
        public static Matrix GetMatrix(this RotateTransform transform)
        {
            return Matrix.Identity.RotateAt(transform.Angle, transform.CenterX, transform.CenterY);
        }
    }
}
