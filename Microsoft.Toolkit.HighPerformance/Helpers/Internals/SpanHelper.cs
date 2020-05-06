// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;

namespace Microsoft.Toolkit.HighPerformance.Helpers.Internals
{
    /// <summary>
    /// Helpers to process sequences of values by reference.
    /// </summary>
    internal static partial class SpanHelper
    {
        /// <summary>
        /// Calculates the djb2 hash for the target sequence of items of a given type.
        /// </summary>
        /// <typeparam name="T">The type of items to hash.</typeparam>
        /// <param name="r0">The reference to the target memory area to hash.</param>
        /// <param name="length">The number of items to hash.</param>
        /// <returns>The Djb2 value for the input sequence of items.</returns>
        [Pure]
        public static unsafe int GetDjb2HashCode<T>(ref T r0, IntPtr length)
            where T : notnull
        {
            int hash = 5381;

            IntPtr offset = default;

            while ((byte*)length >= (byte*)8)
            {
                length -= 8;

                hash = unchecked((hash << 5) + hash ^ Unsafe.Add(ref r0, offset + 0).GetHashCode());
                hash = unchecked((hash << 5) + hash ^ Unsafe.Add(ref r0, offset + 1).GetHashCode());
                hash = unchecked((hash << 5) + hash ^ Unsafe.Add(ref r0, offset + 2).GetHashCode());
                hash = unchecked((hash << 5) + hash ^ Unsafe.Add(ref r0, offset + 3).GetHashCode());
                hash = unchecked((hash << 5) + hash ^ Unsafe.Add(ref r0, offset + 4).GetHashCode());
                hash = unchecked((hash << 5) + hash ^ Unsafe.Add(ref r0, offset + 5).GetHashCode());
                hash = unchecked((hash << 5) + hash ^ Unsafe.Add(ref r0, offset + 6).GetHashCode());
                hash = unchecked((hash << 5) + hash ^ Unsafe.Add(ref r0, offset + 7).GetHashCode());

                offset += 8;
            }

            if ((byte*)length >= (byte*)4)
            {
                length -= 4;

                hash = unchecked((hash << 5) + hash ^ Unsafe.Add(ref r0, offset + 0).GetHashCode());
                hash = unchecked((hash << 5) + hash ^ Unsafe.Add(ref r0, offset + 1).GetHashCode());
                hash = unchecked((hash << 5) + hash ^ Unsafe.Add(ref r0, offset + 2).GetHashCode());
                hash = unchecked((hash << 5) + hash ^ Unsafe.Add(ref r0, offset + 3).GetHashCode());

                offset += 4;
            }

            while ((byte*)length > (byte*)0)
            {
                length -= 1;

                hash = unchecked((hash << 5) + hash ^ Unsafe.Add(ref r0, offset).GetHashCode());

                offset += 1;
            }

            return hash;
        }
    }
}
