// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Microsoft.Toolkit.HighPerformance.Helpers.Internals
{
    /// <summary>
    /// A helper class that with utility methods for dealing with references, and other low-level details.
    /// </summary>
    internal static class RuntimeHelpers
    {
        /// <summary>
        /// Converts a length of items from one size to another. This method exposes the logic from
        /// <see cref="MemoryMarshal.Cast{TFrom,TTo}(Span{TFrom})"/>, just for the length conversion.
        /// </summary>
        /// <typeparam name="TFrom">The source type of items.</typeparam>
        /// <typeparam name="TTo">The target type of items.</typeparam>
        /// <param name="length">The input length to convert.</param>
        /// <returns>The converted length for the specified argument and types.</returns>
        [Pure]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int ConvertLength<TFrom, TTo>(int length)
            where TFrom : unmanaged
            where TTo : unmanaged
        {
            uint fromSize = (uint)Unsafe.SizeOf<TFrom>();
            uint toSize = (uint)Unsafe.SizeOf<TTo>();
            uint fromLength = (uint)length;
            int toLength;

            if (fromSize == toSize)
            {
                toLength = (int)fromLength;
            }
            else if (fromSize == 1)
            {
                toLength = (int)(fromLength / toSize);
            }
            else
            {
                ulong toLengthUInt64 = (ulong)fromLength * fromSize / toSize;

                toLength = checked((int)toLengthUInt64);
            }

            return toLength;
        }
    }
}