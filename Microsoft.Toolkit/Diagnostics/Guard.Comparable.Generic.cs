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
        /// Asserts that the input value is <see langword="default"/>.
        /// </summary>
        /// <typeparam name="T">The type of <see langword="struct"/> value type being tested.</typeparam>
        /// <param name="value">The input value to test.</param>
        /// <param name="name">The name of the input parameter being tested.</param>
        /// <exception cref="ArgumentException">Thrown if <paramref name="value"/> is not <see langword="default"/>.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void IsDefault<T>(T value, string name)
            where T : struct, IEquatable<T>
        {
            if (!value.Equals(default))
            {
                ThrowHelper.ThrowArgumentExceptionForIsDefault(value, name);
            }
        }

        /// <summary>
        /// Asserts that the input value is not <see langword="default"/>.
        /// </summary>
        /// <typeparam name="T">The type of <see langword="struct"/> value type being tested.</typeparam>
        /// <param name="value">The input value to test.</param>
        /// <param name="name">The name of the input parameter being tested.</param>
        /// <exception cref="ArgumentException">Thrown if <paramref name="value"/> is <see langword="default"/>.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void IsNotDefault<T>(T value, string name)
            where T : struct, IEquatable<T>
        {
            if (value.Equals(default))
            {
                ThrowHelper.ThrowArgumentExceptionForIsNotDefault<T>(name);
            }
        }

        /// <summary>
        /// Asserts that the input value must be equal to a specified value.
        /// </summary>
        /// <typeparam name="T">The type of input values to compare.</typeparam>
        /// <param name="value">The input <typeparamref name="T"/> value to test.</param>
        /// <param name="target">The target <typeparamref name="T"/> value to test for.</param>
        /// <param name="name">The name of the input parameter being tested.</param>
        /// <exception cref="ArgumentException">Thrown if <paramref name="value"/> is != <paramref name="target"/>.</exception>
        /// <remarks>The method is generic to avoid boxing the parameters, if they are value types.</remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void IsEqualTo<T>(T value, T target, string name)
            where T : notnull, IEquatable<T>
        {
            if (!value.Equals(target))
            {
                ThrowHelper.ThrowArgumentExceptionForIsEqualTo(value, target, name);
            }
        }

        /// <summary>
        /// Asserts that the input value must be not equal to a specified value.
        /// </summary>
        /// <typeparam name="T">The type of input values to compare.</typeparam>
        /// <param name="value">The input <typeparamref name="T"/> value to test.</param>
        /// <param name="target">The target <typeparamref name="T"/> value to test for.</param>
        /// <param name="name">The name of the input parameter being tested.</param>
        /// <exception cref="ArgumentException">Thrown if <paramref name="value"/> is == <paramref name="target"/>.</exception>
        /// <remarks>The method is generic to avoid boxing the parameters, if they are value types.</remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void IsNotEqualTo<T>(T value, T target, string name)
            where T : notnull, IEquatable<T>
        {
            if (value.Equals(target))
            {
                ThrowHelper.ThrowArgumentExceptionForIsNotEqualTo(value, target, name);
            }
        }

        /// <summary>
        /// Asserts that the input value must be a bitwise match with a specified value.
        /// </summary>
        /// <typeparam name="T">The type of input values to compare.</typeparam>
        /// <param name="value">The input <typeparamref name="T"/> value to test.</param>
        /// <param name="target">The target <typeparamref name="T"/> value to test for.</param>
        /// <param name="name">The name of the input parameter being tested.</param>
        /// <exception cref="ArgumentException">Thrown if <paramref name="value"/> is not a bitwise match for <paramref name="target"/>.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void IsBitwiseEqualTo<T>(T value, T target, string name)
            where T : unmanaged
        {
            /* Include some fast paths if the input type is of size 1, 2, 4 or 8.
             * In those cases, just reinterpret the bytes as values of an integer type,
             * and compare them directly, which is much faster than having a loop over each byte.
             * The conditional branches below are known at compile time by the JIT compiler,
             * so that only the right one will actually be translated into native code. */
            if (typeof(T) == typeof(byte) ||
                typeof(T) == typeof(sbyte) ||
                typeof(T) == typeof(bool))
            {
                byte valueByte = Unsafe.As<T, byte>(ref value);
                byte targetByte = Unsafe.As<T, byte>(ref target);

                if (valueByte != targetByte)
                {
                    ThrowHelper.ThrowArgumentExceptionForsBitwiseEqualTo(value, target, name);
                }
            }
            else if (typeof(T) == typeof(ushort) ||
                     typeof(T) == typeof(short) ||
                     typeof(T) == typeof(char))
            {
                ushort valueUShort = Unsafe.As<T, ushort>(ref value);
                ushort targetUShort = Unsafe.As<T, ushort>(ref target);

                if (valueUShort != targetUShort)
                {
                    ThrowHelper.ThrowArgumentExceptionForsBitwiseEqualTo(value, target, name);
                }
            }
            else if (typeof(T) == typeof(uint) ||
                     typeof(T) == typeof(int) ||
                     typeof(T) == typeof(float))
            {
                uint valueUInt = Unsafe.As<T, uint>(ref value);
                uint targetUInt = Unsafe.As<T, uint>(ref target);

                if (valueUInt != targetUInt)
                {
                    ThrowHelper.ThrowArgumentExceptionForsBitwiseEqualTo(value, target, name);
                }
            }
            else if (typeof(T) == typeof(ulong) ||
                     typeof(T) == typeof(long) ||
                     typeof(T) == typeof(double))
            {
                ulong valueULong = Unsafe.As<T, ulong>(ref value);
                ulong targetULong = Unsafe.As<T, ulong>(ref target);

                if (valueULong != targetULong)
                {
                    ThrowHelper.ThrowArgumentExceptionForsBitwiseEqualTo(value, target, name);
                }
            }
            else
            {
                ref byte valueRef = ref Unsafe.As<T, byte>(ref value);
                ref byte targetRef = ref Unsafe.As<T, byte>(ref target);
                int bytesCount = Unsafe.SizeOf<T>();

                for (int i = 0; i < bytesCount; i++)
                {
                    byte valueByte = Unsafe.Add(ref valueRef, i);
                    byte targetByte = Unsafe.Add(ref targetRef, i);

                    if (valueByte != targetByte)
                    {
                        ThrowHelper.ThrowArgumentExceptionForsBitwiseEqualTo(value, target, name);
                    }
                }
            }
        }

        /// <summary>
        /// Asserts that the input value must be less than a specified value.
        /// </summary>
        /// <typeparam name="T">The type of input values to compare.</typeparam>
        /// <param name="value">The input <typeparamref name="T"/> value to test.</param>
        /// <param name="maximum">The exclusive maximum <typeparamref name="T"/> value that is accepted.</param>
        /// <param name="name">The name of the input parameter being tested.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="value"/> is >= <paramref name="maximum"/>.</exception>
        /// <remarks>The method is generic to avoid boxing the parameters, if they are value types.</remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void IsLessThan<T>(T value, T maximum, string name)
            where T : notnull, IComparable<T>
        {
            if (value.CompareTo(maximum) >= 0)
            {
                ThrowHelper.ThrowArgumentOutOfRangeExceptionForIsLessThan(value, maximum, name);
            }
        }

        /// <summary>
        /// Asserts that the input value must be less than or equal to a specified value.
        /// </summary>
        /// <typeparam name="T">The type of input values to compare.</typeparam>
        /// <param name="value">The input <typeparamref name="T"/> value to test.</param>
        /// <param name="maximum">The inclusive maximum <typeparamref name="T"/> value that is accepted.</param>
        /// <param name="name">The name of the input parameter being tested.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="value"/> is > <paramref name="maximum"/>.</exception>
        /// <remarks>The method is generic to avoid boxing the parameters, if they are value types.</remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void IsLessThanOrEqualTo<T>(T value, T maximum, string name)
            where T : notnull, IComparable<T>
        {
            if (value.CompareTo(maximum) > 0)
            {
                ThrowHelper.ThrowArgumentOutOfRangeExceptionForIsLessThanOrEqualTo(value, maximum, name);
            }
        }

        /// <summary>
        /// Asserts that the input value must be greater than a specified value.
        /// </summary>
        /// <typeparam name="T">The type of input values to compare.</typeparam>
        /// <param name="value">The input <typeparamref name="T"/> value to test.</param>
        /// <param name="minimum">The exclusive minimum <typeparamref name="T"/> value that is accepted.</param>
        /// <param name="name">The name of the input parameter being tested.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="value"/> is &lt;= <paramref name="minimum"/>.</exception>
        /// <remarks>The method is generic to avoid boxing the parameters, if they are value types.</remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void IsGreaterThan<T>(T value, T minimum, string name)
            where T : notnull, IComparable<T>
        {
            if (value.CompareTo(minimum) <= 0)
            {
                ThrowHelper.ThrowArgumentOutOfRangeExceptionForIsGreaterThan(value, minimum, name);
            }
        }

        /// <summary>
        /// Asserts that the input value must be greater than or equal to a specified value.
        /// </summary>
        /// <typeparam name="T">The type of input values to compare.</typeparam>
        /// <param name="value">The input <typeparamref name="T"/> value to test.</param>
        /// <param name="minimum">The inclusive minimum <typeparamref name="T"/> value that is accepted.</param>
        /// <param name="name">The name of the input parameter being tested.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="value"/> is &lt; <paramref name="minimum"/>.</exception>
        /// <remarks>The method is generic to avoid boxing the parameters, if they are value types.</remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void IsGreaterThanOrEqualTo<T>(T value, T minimum, string name)
            where T : notnull, IComparable<T>
        {
            if (value.CompareTo(minimum) < 0)
            {
                ThrowHelper.ThrowArgumentOutOfRangeExceptionForIsGreaterThanOrEqualTo(value, minimum, name);
            }
        }

        /// <summary>
        /// Asserts that the input value must be in a given range.
        /// </summary>
        /// <typeparam name="T">The type of input values to compare.</typeparam>
        /// <param name="value">The input <typeparamref name="T"/> value to test.</param>
        /// <param name="minimum">The inclusive minimum <typeparamref name="T"/> value that is accepted.</param>
        /// <param name="maximum">The exclusive maximum <typeparamref name="T"/> value that is accepted.</param>
        /// <param name="name">The name of the input parameter being tested.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="value"/> is &lt; <paramref name="minimum"/> or >= <paramref name="maximum"/>.</exception>
        /// <remarks>
        /// This API asserts the equivalent of "<paramref name="value"/> in [<paramref name="minimum"/>, <paramref name="maximum"/>)", using arithmetic notation.
        /// The method is generic to avoid boxing the parameters, if they are value types.
        /// </remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void IsInRange<T>(T value, T minimum, T maximum, string name)
            where T : notnull, IComparable<T>
        {
            if (value.CompareTo(minimum) < 0 || value.CompareTo(maximum) >= 0)
            {
                ThrowHelper.ThrowArgumentOutOfRangeExceptionForIsInRange(value, minimum, maximum, name);
            }
        }

        /// <summary>
        /// Asserts that the input value must not be in a given range.
        /// </summary>
        /// <typeparam name="T">The type of input values to compare.</typeparam>
        /// <param name="value">The input <typeparamref name="T"/> value to test.</param>
        /// <param name="minimum">The inclusive minimum <typeparamref name="T"/> value that is accepted.</param>
        /// <param name="maximum">The exclusive maximum <typeparamref name="T"/> value that is accepted.</param>
        /// <param name="name">The name of the input parameter being tested.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="value"/> is >= <paramref name="minimum"/> or &lt; <paramref name="maximum"/>.</exception>
        /// <remarks>
        /// This API asserts the equivalent of "<paramref name="value"/> not in [<paramref name="minimum"/>, <paramref name="maximum"/>)", using arithmetic notation.
        /// The method is generic to avoid boxing the parameters, if they are value types.
        /// </remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void IsNotInRange<T>(T value, T minimum, T maximum, string name)
            where T : notnull, IComparable<T>
        {
            if (value.CompareTo(minimum) >= 0 && value.CompareTo(maximum) < 0)
            {
                ThrowHelper.ThrowArgumentOutOfRangeExceptionForIsNotInRange(value, minimum, maximum, name);
            }
        }

        /// <summary>
        /// Asserts that the input value must be in a given interval.
        /// </summary>
        /// <typeparam name="T">The type of input values to compare.</typeparam>
        /// <param name="value">The input <typeparamref name="T"/> value to test.</param>
        /// <param name="minimum">The exclusive minimum <typeparamref name="T"/> value that is accepted.</param>
        /// <param name="maximum">The exclusive maximum <typeparamref name="T"/> value that is accepted.</param>
        /// <param name="name">The name of the input parameter being tested.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="value"/> is &lt;= <paramref name="minimum"/> or >= <paramref name="maximum"/>.</exception>
        /// <remarks>
        /// This API asserts the equivalent of "<paramref name="value"/> in (<paramref name="minimum"/>, <paramref name="maximum"/>)", using arithmetic notation.
        /// The method is generic to avoid boxing the parameters, if they are value types.
        /// </remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void IsBetween<T>(T value, T minimum, T maximum, string name)
            where T : notnull, IComparable<T>
        {
            if (value.CompareTo(minimum) <= 0 || value.CompareTo(maximum) >= 0)
            {
                ThrowHelper.ThrowArgumentOutOfRangeExceptionForIsBetween(value, minimum, maximum, name);
            }
        }

        /// <summary>
        /// Asserts that the input value must not be in a given interval.
        /// </summary>
        /// <typeparam name="T">The type of input values to compare.</typeparam>
        /// <param name="value">The input <typeparamref name="T"/> value to test.</param>
        /// <param name="minimum">The exclusive minimum <typeparamref name="T"/> value that is accepted.</param>
        /// <param name="maximum">The exclusive maximum <typeparamref name="T"/> value that is accepted.</param>
        /// <param name="name">The name of the input parameter being tested.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="value"/> is > <paramref name="minimum"/> or &lt; <paramref name="maximum"/>.</exception>
        /// <remarks>
        /// This API asserts the equivalent of "<paramref name="value"/> not in (<paramref name="minimum"/>, <paramref name="maximum"/>)", using arithmetic notation.
        /// The method is generic to avoid boxing the parameters, if they are value types.
        /// </remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void IsNotBetween<T>(T value, T minimum, T maximum, string name)
            where T : notnull, IComparable<T>
        {
            if (value.CompareTo(minimum) > 0 && value.CompareTo(maximum) < 0)
            {
                ThrowHelper.ThrowArgumentOutOfRangeExceptionForIsNotBetween(value, minimum, maximum, name);
            }
        }

        /// <summary>
        /// Asserts that the input value must be in a given interval.
        /// </summary>
        /// <typeparam name="T">The type of input values to compare.</typeparam>
        /// <param name="value">The input <typeparamref name="T"/> value to test.</param>
        /// <param name="minimum">The inclusive minimum <typeparamref name="T"/> value that is accepted.</param>
        /// <param name="maximum">The inclusive maximum <typeparamref name="T"/> value that is accepted.</param>
        /// <param name="name">The name of the input parameter being tested.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="value"/> is &lt; <paramref name="minimum"/> or > <paramref name="maximum"/>.</exception>
        /// <remarks>
        /// This API asserts the equivalent of "<paramref name="value"/> in [<paramref name="minimum"/>, <paramref name="maximum"/>]", using arithmetic notation.
        /// The method is generic to avoid boxing the parameters, if they are value types.
        /// </remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void IsBetweenOrEqualTo<T>(T value, T minimum, T maximum, string name)
            where T : notnull, IComparable<T>
        {
            if (value.CompareTo(minimum) < 0 || value.CompareTo(maximum) > 0)
            {
                ThrowHelper.ThrowArgumentOutOfRangeExceptionForIsBetweenOrEqualTo(value, minimum, maximum, name);
            }
        }

        /// <summary>
        /// Asserts that the input value must not be in a given interval.
        /// </summary>
        /// <typeparam name="T">The type of input values to compare.</typeparam>
        /// <param name="value">The input <typeparamref name="T"/> value to test.</param>
        /// <param name="minimum">The inclusive minimum <typeparamref name="T"/> value that is accepted.</param>
        /// <param name="maximum">The inclusive maximum <typeparamref name="T"/> value that is accepted.</param>
        /// <param name="name">The name of the input parameter being tested.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="value"/> is >= <paramref name="minimum"/> or &lt;= <paramref name="maximum"/>.</exception>
        /// <remarks>
        /// This API asserts the equivalent of "<paramref name="value"/> not in [<paramref name="minimum"/>, <paramref name="maximum"/>]", using arithmetic notation.
        /// The method is generic to avoid boxing the parameters, if they are value types.
        /// </remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void IsNotBetweenOrEqualTo<T>(T value, T minimum, T maximum, string name)
            where T : notnull, IComparable<T>
        {
            if (value.CompareTo(minimum) >= 0 && value.CompareTo(maximum) <= 0)
            {
                ThrowHelper.ThrowArgumentOutOfRangeExceptionForIsNotBetweenOrEqualTo(value, minimum, maximum, name);
            }
        }
    }
}
