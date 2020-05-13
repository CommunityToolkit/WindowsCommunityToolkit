// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;

namespace Microsoft.Toolkit.HighPerformance.Helpers
{
    /// <summary>
    /// Helpers to perform bit operations on numeric types.
    /// </summary>
    public static class BitHelper
    {
        /// <summary>
        /// Checks whether or not a given bit is set.
        /// </summary>
        /// <param name="value">The input <see cref="uint"/> value.</param>
        /// <param name="n">The position of the bit to check.</param>
        /// <returns>Whether or not the n-th bit is set.</returns>
        /// <remarks>
        /// This method doesn't validate <paramref name="n"/> against the valid range.
        /// If the parameter is not valid, the result will just be inconsistent.
        /// Additionally, no conditional branches are used to retrieve the flag.
        /// </remarks>
        [Pure]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool HasFlag(uint value, int n)
        {
            // Read the n-th bit, downcast to byte
            byte flag = (byte)((value >> n) & 1);

            /* Reinterpret the byte to avoid the test, setnz and
             * movzx instructions (asm x64). This is because the JIT
             * compiler is able to optimize this reinterpret-cast as
             * a single "and eax, 0x1" instruction, whereas if we had
             * compared the previous computed flag against 0, the assembly
             * would have had to perform the test, set the non-zero
             * flag and then extend the (byte) result to eax. */
            return Unsafe.As<byte, bool>(ref flag);
        }

        /// <summary>
        /// Sets a bit to a specified value.
        /// </summary>
        /// <param name="value">The target <see cref="uint"/> value.</param>
        /// <param name="n">The position of the bit to set or clear.</param>
        /// <param name="flag">The value to assign to the target bit.</param>
        /// <remarks>
        /// Just like <see cref="HasFlag(uint,int)"/>, this method doesn't validate <paramref name="n"/>
        /// and does not contain branching instructions, so it's well suited for use in tight loops as well.
        /// </remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SetFlag(ref uint value, int n, bool flag)
        {
            value = SetFlag(value, n, flag);
        }

        /// <summary>
        /// Sets a bit to a specified value.
        /// </summary>
        /// <param name="value">The input <see cref="uint"/> value.</param>
        /// <param name="n">The position of the bit to set or clear.</param>
        /// <param name="flag">The value to assign to the target bit.</param>
        /// <returns>An <see cref="uint"/> value equal to <paramref name="value"/> except for the <paramref name="n"/>-th bit.</returns>
        /// <remarks>
        /// Just like <see cref="HasFlag(uint,int)"/>, this method doesn't validate <paramref name="n"/>
        /// and does not contain branching instructions, so it's well suited for use in tight loops as well.
        /// </remarks>
        [Pure]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint SetFlag(uint value, int n, bool flag)
        {
            /* Shift a bit left to the n-th position, negate the
             * resulting value and perform an AND with the input value.
             * This effectively clears the n-th bit of our input. */
            uint
                bit = 1u << n,
                not = ~bit,
                and = value & not;

            /* Reinterpret the flag as 1 or 0, and cast to uint.
             * The flag is first copied to a local variable as taking
             * the address of an argument is slower than doing the same
             * for a local variable. This is because when referencing
             * the argument the JIT compiler will emit code to temporarily
             * move the argument to the stack, and then copy it back.
             * With a temporary variable instead, the JIT will be able to
             * optimize the method to only use CPU registers. */
            bool localFlag = flag;
            uint
                flag32 = Unsafe.As<bool, byte>(ref localFlag),

                /* Finally, we left shift the uint flag to the right position
                 * and perform an OR with the resulting value of the previous
                 * operation. This will always guaranteed to work, thanks to the
                 * initial code clearing that bit before setting it again. */
                shift = flag32 << n,
                or = and | shift;

            return or;
        }

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
        public static bool HasFlag(ulong value, int n)
        {
            // Same logic as the uint version, see that for more info
            byte flag = (byte)((value >> n) & 1);

            return Unsafe.As<byte, bool>(ref flag);
        }

        /// <summary>
        /// Sets a bit to a specified value.
        /// </summary>
        /// <param name="value">The target <see cref="ulong"/> value.</param>
        /// <param name="n">The position of the bit to set or clear.</param>
        /// <param name="flag">The value to assign to the target bit.</param>
        /// <remarks>
        /// Just like <see cref="HasFlag(ulong,int)"/>, this method doesn't validate <paramref name="n"/>
        /// and does not contain branching instructions, so it's well suited for use in tight loops as well.
        /// </remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SetFlag(ref ulong value, int n, bool flag)
        {
            value = SetFlag(value, n, flag);
        }

        /// <summary>
        /// Sets a bit to a specified value.
        /// </summary>
        /// <param name="value">The input <see cref="ulong"/> value.</param>
        /// <param name="n">The position of the bit to set or clear.</param>
        /// <param name="flag">The value to assign to the target bit.</param>
        /// <returns>An <see cref="ulong"/> value equal to <paramref name="value"/> except for the <paramref name="n"/>-th bit.</returns>
        /// <remarks>
        /// Just like <see cref="HasFlag(ulong,int)"/>, this method doesn't validate <paramref name="n"/>
        /// and does not contain branching instructions, so it's well suited for use in tight loops as well.
        /// </remarks>
        [Pure]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ulong SetFlag(ulong value, int n, bool flag)
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
