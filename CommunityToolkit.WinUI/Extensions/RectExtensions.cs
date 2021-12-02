// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;
using Windows.Foundation;
using Rect = Windows.Foundation.Rect;
using Size = Windows.Foundation.Size;

namespace CommunityToolkit.WinUI
{
    /// <summary>
    /// Extensions for the <see cref="Rect"/> type.
    /// </summary>
    public static class RectExtensions
    {
        /// <summary>
        /// Determines if a rectangle intersects with another rectangle.
        /// </summary>
        /// <param name="rect1">The first rectangle to test.</param>
        /// <param name="rect2">The second rectangle to test.</param>
        /// <returns>This method returns <see langword="true"/> if there is any intersection, otherwise <see langword="false"/>.</returns>
        [Pure]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IntersectsWith(this Rect rect1, Rect rect2)
        {
            if (rect1.IsEmpty || rect2.IsEmpty)
            {
                return false;
            }

            return (rect1.Left <= rect2.Right) &&
                   (rect1.Right >= rect2.Left) &&
                   (rect1.Top <= rect2.Bottom) &&
                   (rect1.Bottom >= rect2.Top);
        }

        /// <summary>
        /// Creates a new <see cref="Size"/> of the specified <see cref="Rect"/>'s width and height.
        /// </summary>
        /// <param name="rect">Rectangle to size.</param>
        /// <returns>Size of rectangle.</returns>
        [Pure]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Size ToSize(this Rect rect)
        {
            return new Size(rect.Width, rect.Height);
        }
    }
}