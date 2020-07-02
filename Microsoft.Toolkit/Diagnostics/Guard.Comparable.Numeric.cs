// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Runtime.CompilerServices;

#nullable enable

namespace Microsoft.Toolkit.Diagnostics
{
    /// <summary>
    /// Helper methods to verify conditions when running code.
    /// </summary>
    public static partial class Guard
    {
        /// <summary>
        /// Asserts that the input value must be within a given distance from a specified value.
        /// </summary>
        /// <param name="value">The input <see cref="int"/> value to test.</param>
        /// <param name="target">The target <see cref="int"/> value to test for.</param>
        /// <param name="delta">The maximum distance to allow between <paramref name="value"/> and <paramref name="target"/>.</param>
        /// <param name="name">The name of the input parameter being tested.</param>
        /// <exception cref="ArgumentException">Thrown if (<paramref name="value"/> - <paramref name="target"/>) > <paramref name="delta"/>.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void IsCloseTo(int value, int target, uint delta, string name)
        {
            // Cast to long before calculating the difference to avoid overflows
            // when the values are at the two extremes of the supported range.
            // Then cast to double to calculate the absolute value: this allows
            // the JIT compiler to use AVX instructions on X64 CPUs instead of
            // conditional jumps, which results in more efficient assembly code.
            // The IEEE 754 specs guarantees that a 32 bit integer value can
            // be stored within a double precision floating point value with
            // no loss of precision, so the result will always be correct here.
            // The difference is then cast to uint as that's the maximum possible
            // value it can have, and comparing two 32 bit integer values
            // results in shorter and slightly faster code than using doubles.
            if ((uint)Math.Abs((double)((long)value - target)) <= delta)
            {
                return;
            }

            ThrowHelper.ThrowArgumentExceptionForIsCloseTo(value, target, delta, name);
        }

        /// <summary>
        /// Asserts that the input value must not be within a given distance from a specified value.
        /// </summary>
        /// <param name="value">The input <see cref="int"/> value to test.</param>
        /// <param name="target">The target <see cref="int"/> value to test for.</param>
        /// <param name="delta">The maximum distance to allow between <paramref name="value"/> and <paramref name="target"/>.</param>
        /// <param name="name">The name of the input parameter being tested.</param>
        /// <exception cref="ArgumentException">Thrown if (<paramref name="value"/> - <paramref name="target"/>) &lt;= <paramref name="delta"/>.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void IsNotCloseTo(int value, int target, uint delta, string name)
        {
            if ((uint)Math.Abs((double)((long)value - target)) > delta)
            {
                return;
            }

            ThrowHelper.ThrowArgumentExceptionForIsNotCloseTo(value, target, delta, name);
        }

        /// <summary>
        /// Asserts that the input value must be within a given distance from a specified value.
        /// </summary>
        /// <param name="value">The input <see cref="long"/> value to test.</param>
        /// <param name="target">The target <see cref="long"/> value to test for.</param>
        /// <param name="delta">The maximum distance to allow between <paramref name="value"/> and <paramref name="target"/>.</param>
        /// <param name="name">The name of the input parameter being tested.</param>
        /// <exception cref="ArgumentException">Thrown if (<paramref name="value"/> - <paramref name="target"/>) > <paramref name="delta"/>.</exception>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void IsCloseTo(long value, long target, ulong delta, string name)
        {
            // This method and the one below are not inlined because
            // using the decimal type results in quite a bit of code.
            if ((ulong)Math.Abs((decimal)value - target) <= delta)
            {
                return;
            }

            ThrowHelper.ThrowArgumentExceptionForIsCloseTo(value, target, delta, name);
        }

        /// <summary>
        /// Asserts that the input value must not be within a given distance from a specified value.
        /// </summary>
        /// <param name="value">The input <see cref="long"/> value to test.</param>
        /// <param name="target">The target <see cref="long"/> value to test for.</param>
        /// <param name="delta">The maximum distance to allow between <paramref name="value"/> and <paramref name="target"/>.</param>
        /// <param name="name">The name of the input parameter being tested.</param>
        /// <exception cref="ArgumentException">Thrown if (<paramref name="value"/> - <paramref name="target"/>) &lt;= <paramref name="delta"/>.</exception>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void IsNotCloseTo(long value, long target, ulong delta, string name)
        {
            if ((ulong)Math.Abs((decimal)value - target) > delta)
            {
                return;
            }

            ThrowHelper.ThrowArgumentExceptionForIsNotCloseTo(value, target, delta, name);
        }

        /// <summary>
        /// Asserts that the input value must be within a given distance from a specified value.
        /// </summary>
        /// <param name="value">The input <see cref="float"/> value to test.</param>
        /// <param name="target">The target <see cref="float"/> value to test for.</param>
        /// <param name="delta">The maximum distance to allow between <paramref name="value"/> and <paramref name="target"/>.</param>
        /// <param name="name">The name of the input parameter being tested.</param>
        /// <exception cref="ArgumentException">Thrown if (<paramref name="value"/> - <paramref name="target"/>) > <paramref name="delta"/>.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void IsCloseTo(float value, float target, float delta, string name)
        {
            if (Math.Abs(value - target) <= delta)
            {
                return;
            }

            ThrowHelper.ThrowArgumentExceptionForIsCloseTo(value, target, delta, name);
        }

        /// <summary>
        /// Asserts that the input value must not be within a given distance from a specified value.
        /// </summary>
        /// <param name="value">The input <see cref="float"/> value to test.</param>
        /// <param name="target">The target <see cref="float"/> value to test for.</param>
        /// <param name="delta">The maximum distance to allow between <paramref name="value"/> and <paramref name="target"/>.</param>
        /// <param name="name">The name of the input parameter being tested.</param>
        /// <exception cref="ArgumentException">Thrown if (<paramref name="value"/> - <paramref name="target"/>) &lt;= <paramref name="delta"/>.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void IsNotCloseTo(float value, float target, float delta, string name)
        {
            if (Math.Abs(value - target) > delta)
            {
                return;
            }

            ThrowHelper.ThrowArgumentExceptionForIsNotCloseTo(value, target, delta, name);
        }

        /// <summary>
        /// Asserts that the input value must be within a given distance from a specified value.
        /// </summary>
        /// <param name="value">The input <see cref="double"/> value to test.</param>
        /// <param name="target">The target <see cref="double"/> value to test for.</param>
        /// <param name="delta">The maximum distance to allow between <paramref name="value"/> and <paramref name="target"/>.</param>
        /// <param name="name">The name of the input parameter being tested.</param>
        /// <exception cref="ArgumentException">Thrown if (<paramref name="value"/> - <paramref name="target"/>) > <paramref name="delta"/>.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void IsCloseTo(double value, double target, double delta, string name)
        {
            if (Math.Abs(value - target) <= delta)
            {
                return;
            }

            ThrowHelper.ThrowArgumentExceptionForIsCloseTo(value, target, delta, name);
        }

        /// <summary>
        /// Asserts that the input value must not be within a given distance from a specified value.
        /// </summary>
        /// <param name="value">The input <see cref="double"/> value to test.</param>
        /// <param name="target">The target <see cref="double"/> value to test for.</param>
        /// <param name="delta">The maximum distance to allow between <paramref name="value"/> and <paramref name="target"/>.</param>
        /// <param name="name">The name of the input parameter being tested.</param>
        /// <exception cref="ArgumentException">Thrown if (<paramref name="value"/> - <paramref name="target"/>) &lt;= <paramref name="delta"/>.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void IsNotCloseTo(double value, double target, double delta, string name)
        {
            if (Math.Abs(value - target) > delta)
            {
                return;
            }

            ThrowHelper.ThrowArgumentExceptionForIsNotCloseTo(value, target, delta, name);
        }
    }
}
