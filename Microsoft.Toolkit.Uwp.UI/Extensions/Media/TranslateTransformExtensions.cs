// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.UI.Xaml.Media;

namespace Microsoft.Toolkit.Uwp.UI.Extensions
{
    /// <summary>
    /// Extension methods for <see cref="TranslateTransform"/>.
    /// </summary>
    public static class TranslateTransformExtensions
    {
        /// <summary>
        /// Gets the matrix that represents this transform.
        /// Implements WPF's TranslateTransform.Value.
        /// </summary>
        /// <param name="transform">Extended TranslateTranform.</param>
        /// <returns>Matrix representing transform.</returns>
        public static Matrix GetMatrix(this TranslateTransform transform)
        {
#if WINDOWS_UWP
            return MatrixHelper.Identity
#else
            return Matrix.Identity
#endif
                .Translate(transform.X, transform.Y);
        }
    }
}
