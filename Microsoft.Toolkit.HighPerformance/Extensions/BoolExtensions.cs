// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;

namespace Microsoft.Toolkit.HighPerformance.Extensions
{
    /// <summary>
    /// Helpers for working with the <see cref="bool"/> type.
    /// </summary>
    public static class BoolExtensions
    {
        /// <summary>
        /// Converts the given <see cref="bool"/> value into an <see cref="int"/>.
        /// </summary>
        /// <param name="flag">The input value to convert.</param>
        /// <returns>1 if <paramref name="flag"/> is <see langword="true"/>, 0 otherwise.</returns>
        /// <remarks>This method does not contain branching instructions.</remarks>
        [Pure]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int ToInt(this bool flag)
        {
            return Unsafe.As<bool, byte>(ref flag);
        }
    }
}
