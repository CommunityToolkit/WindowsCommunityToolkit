// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;

namespace Microsoft.Toolkit.HighPerformance.Extensions
{
    /// <summary>
    /// Helpers for working with the <see cref="ulong"/> type.
    /// </summary>
    public static class UInt64Extensions
    {
        /// <summary>
        /// Checks whether or not a given bit is set.
        /// </summary>
        /// <param name="value">The input <see cref="ulong"/> value.</param>
        /// <param name="n">The position of the bit to check.</param>
        /// <returns>Whether or not the n-th bit is set.</returns>
        /// <remarks>
        /// This method doesn't validate <paramref name="n"/> against the valid range.
        /// If the parameter is not valid, the result will just be inconsistent.
        /// Additionally, no conditional branches are used to retrieve the flag.
        /// </remarks>
        [Pure]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool HasFlag(this ulong value, int n)
        {
            // Same logic as the uint version, see that for more info
            byte flag = (byte)((value >> n) & 1);

            return Unsafe.As<byte, bool>(ref flag);
        }

        /// <summary>
        /// Sets a bit to a specified value.
        /// </summary>
        /// <param name="value">The input <see cref="ulong"/> value.</param>
        /// <param name="n">The position of the bit to set or clear.</param>
        /// <param name="flag">The value to assign to the target bit.</param>
        /// <returns>An <see cref="ulong"/> value equal to <paramref name="value"/> except for the <paramref name="n"/>-th bit.</returns>
        /// <remarks>Just like <see cref="HasFlag"/>, this method doesn't validate <paramref name="n"/>
        /// and does not contain branching instructions, so it's well suited for use in tight loops as well.</remarks>
        [Pure]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ulong SetFlag(this ulong value, int n, bool flag)
        {
            // As with the method above, reuse the same logic as the uint version
            ulong
                bit = 1ul << n,
                not = ~bit,
                and = value & not;
            bool localFlag = flag;
            ulong
                flag64 = Unsafe.As<bool, byte>(ref localFlag),
                shift = flag64 << n,
                or = and | shift;

            return or;
        }
    }
}
