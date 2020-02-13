// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;

#nullable enable

namespace Microsoft.Toolkit.Diagnostics
{
    /// <summary>
    /// Helper methods to verify conditions when running code.
    /// </summary>
    [DebuggerStepThrough]
    public static class Guard
    {
        /// <summary>
        /// Asserts that the input value is not <see langword="null"/>.
        /// </summary>
        /// <param name="value">The input value to test.</param>
        /// <param name="name">The name of the input parameter being tested.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="value"/> is <see langword="null"/>.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void IsNotNull(object? value, string name)
        {
            if (value is null)
            {
                throw new ArgumentNullException(name, $"Parameter {name} must be not null");
            }
        }

        /// <summary>
        /// Asserts that the input value is not <see langword="null"/>.
        /// </summary>
        /// <typeparam name="T">The type of nullable value type being tested.</typeparam>
        /// <param name="value">The input value to test.</param>
        /// <param name="name">The name of the input parameter being tested.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="value"/> is <see langword="null"/>.</exception>
        /// <remarks>The method is generic to avoid boxing the parameters, if they are value types.</remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void IsNotNull<T>(T? value, string name)
            where T : struct
        {
            if (value is null)
            {
                throw new ArgumentNullException(name, $"Parameter {name} must be not null");
            }
        }

        /// <summary>
        /// Asserts that the input value is of a specific type.
        /// </summary>
        /// <typeparam name="T">The type of the input value.</typeparam>
        /// <param name="value">The input <see cref="object"/> to test.</param>
        /// <param name="name">The name of the input parameter being tested.</param>
        /// <exception cref="ArgumentException">Thrown if <paramref name="value"/> is not of type <typeparamref name="T"/>.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void IsOfType<T>(object value, string name)
            where T : class
        {
            if (value.GetType() != typeof(T))
            {
                throw new ArgumentException($"Parameter {name} must be of type {typeof(T)}, was {value.GetType()}", name);
            }
        }

        /// <summary>
        /// Asserts that the input value can be cast to a specified type.
        /// </summary>
        /// <typeparam name="T">The type to check the input value against.</typeparam>
        /// <param name="value">The input <see cref="object"/> to test.</param>
        /// <param name="name">The name of the input parameter being tested.</param>
        /// <exception cref="ArgumentException">Thrown if <paramref name="value"/> can't be cast to type <typeparamref name="T"/>.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void IsAssignableToType<T>(object value, string name)
        {
            if (!(value is T))
            {
                throw new ArgumentException($"Parameter {name} must be assignable to type {typeof(T)}, was {value.GetType()}", name);
            }
        }

        /// <summary>
        /// Asserts that the input value must be <see langword="true"/>.
        /// </summary>
        /// <param name="value">The input <see cref="bool"/> to test.</param>
        /// <param name="name">The name of the input parameter being tested.</param>
        /// <exception cref="ArgumentException">Thrown if <paramref name="value"/> is <see langword="false"/>.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void IsTrue(bool value, string name)
        {
            if (!value)
            {
                throw new ArgumentException($"Parameter {name} must be true, was false", name);
            }
        }

        /// <summary>
        /// Asserts that the input value must be <see langword="false"/>.
        /// </summary>
        /// <param name="value">The input <see cref="bool"/> to test.</param>
        /// <param name="name">The name of the input parameter being tested.</param>
        /// <exception cref="ArgumentException">Thrown if <paramref name="value"/> is <see langword="true"/>.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void IsFalse(bool value, string name)
        {
            if (value)
            {
                throw new ArgumentException($"Parameter {name} must be false, was true", name);
            }
        }

        /// <summary>
        /// Asserts that the input value must be less than a specified value.
        /// </summary>
        /// <typeparam name="T">The type of input values to compare.</typeparam>
        /// <param name="value">The input <typeparamref name="T"/> value to test.</param>
        /// <param name="max">The exclusive maximum <typeparamref name="T"/> value that is accepted.</param>
        /// <param name="name">The name of the input parameter being tested.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="value"/> is >= <paramref name="max"/>.</exception>
        /// <remarks>The method is generic to avoid boxing the parameters, if they are value types.</remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void IsLessThan<T>(T value, T max, string name)
            where T : notnull, IComparable<T>
        {
            if (value.CompareTo(max) >= 0)
            {
                throw new ArgumentOutOfRangeException(name, $"Parameter {name} must be < {max}, was {value}");
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
                throw new ArgumentOutOfRangeException(name, $"Parameter {name} must be <= {maximum}, was {value}");
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
                throw new ArgumentOutOfRangeException(name, $"Parameter {name} must be > {minimum}, was {value}");
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
                throw new ArgumentOutOfRangeException(name, $"Parameter {name} must be >= {minimum}, was {value}");
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
        /// <remarks>The method is generic to avoid boxing the parameters, if they are value types.</remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void IsBetween<T>(T value, T minimum, T maximum, string name)
            where T : notnull, IComparable<T>
        {
            if (value.CompareTo(minimum) <= 0 || value.CompareTo(maximum) >= 0)
            {
                throw new ArgumentOutOfRangeException(name, $"Parameter {name} must be > {minimum} and < {maximum}, was {value}");
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
        /// <remarks>The method is generic to avoid boxing the parameters, if they are value types.</remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void IsNotBetween<T>(T value, T minimum, T maximum, string name)
            where T : notnull, IComparable<T>
        {
            if (value.CompareTo(minimum) > 0 || value.CompareTo(maximum) < 0)
            {
                throw new ArgumentOutOfRangeException(name, $"Parameter {name} must be <= {minimum} and >= {maximum}, was {value}");
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
        /// <remarks>The method is generic to avoid boxing the parameters, if they are value types.</remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void IsBetweenOrEqualTo<T>(T value, T minimum, T maximum, string name)
            where T : notnull, IComparable<T>
        {
            if (value.CompareTo(minimum) < 0 || value.CompareTo(maximum) > 0)
            {
                throw new ArgumentOutOfRangeException(name, $"Parameter {name} must be >= {minimum} and <= {maximum}, was {value}");
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
        /// <remarks>The method is generic to avoid boxing the parameters, if they are value types.</remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void IsNotBetweenOrEqualTo<T>(T value, T minimum, T maximum, string name)
            where T : notnull, IComparable<T>
        {
            if (value.CompareTo(minimum) >= 0 || value.CompareTo(maximum) <= 0)
            {
                throw new ArgumentOutOfRangeException(name, $"Parameter {name} must be < {minimum} and > {maximum}, was {value}");
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
                throw new ArgumentException(name, $"Parameter {name} must be == {target}, was {value}");
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
                throw new ArgumentException(name, $"Parameter {name} must be != {target}, was {value}");
            }
        }

        /// <summary>
        /// Asserts that the input <see cref="IEnumerable{T}"/> instance must have a size of a specified value.
        /// </summary>
        /// <typeparam name="T">The type of items in the input <see cref="IEnumerable{T}"/> instance.</typeparam>
        /// <param name="enumerable">The input <see cref="IEnumerable{T}"/> instance to check the size for.</param>
        /// <param name="size">The target size to test.</param>
        /// <param name="name">The name of the input parameter being tested.</param>
        /// <exception cref="ArgumentException">Thrown if the size of <paramref name="enumerable"/> is != <paramref name="size"/>.</exception>
        /// <remarks>The method will skip enumerating <paramref name="enumerable"/> if possible (if it's an <see cref="ICollection{T}"/> or <see cref="IReadOnlyCollection{T}"/>).</remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void HasSizeEqualTo<T>(IEnumerable<T> enumerable, int size, string name)
        {
            int actualSize;

            if (((enumerable as ICollection<T>)?.Count is int collectionCount &&
                 (actualSize = collectionCount) != size) ||
                ((enumerable as IReadOnlyCollection<T>)?.Count is int readOnlyCollectionCount &&
                 (actualSize = readOnlyCollectionCount) != size) ||
                (actualSize = enumerable.Count()) != size)
            {
                throw new ArgumentException($"Parameter {name} must be sized == {size}, had a size of {actualSize}");
            }
        }

        /// <summary>
        /// Asserts that the input <see cref="IEnumerable{T}"/> instance must have a size of at least specified value.
        /// </summary>
        /// <typeparam name="T">The type of items in the input <see cref="IEnumerable{T}"/> instance.</typeparam>
        /// <param name="enumerable">The input <see cref="IEnumerable{T}"/> instance to check the size for.</param>
        /// <param name="size">The target size to test.</param>
        /// <param name="name">The name of the input parameter being tested.</param>
        /// <exception cref="ArgumentException">Thrown if the size of <paramref name="enumerable"/> is &lt;= <paramref name="size"/>.</exception>
        /// <remarks>The method will skip enumerating <paramref name="enumerable"/> if possible (if it's an <see cref="ICollection{T}"/> or <see cref="IReadOnlyCollection{T}"/>).</remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void HasSizeAtLeast<T>(IEnumerable<T> enumerable, int size, string name)
        {
            int actualSize;

            if (((enumerable as ICollection<T>)?.Count is int collectionCount &&
                 (actualSize = collectionCount) <= size) ||
                ((enumerable as IReadOnlyCollection<T>)?.Count is int readOnlyCollectionCount &&
                 (actualSize = readOnlyCollectionCount) <= size) ||
                (actualSize = enumerable.Count()) <= size)
            {
                throw new ArgumentException($"Parameter {name} must be sized > {size}, had a size of {actualSize}");
            }
        }

        /// <summary>
        /// Asserts that the input <see cref="IEnumerable{T}"/> instance must have a size of at least or equal to a specified value.
        /// </summary>
        /// <typeparam name="T">The type of items in the input <see cref="IEnumerable{T}"/> instance.</typeparam>
        /// <param name="enumerable">The input <see cref="IEnumerable{T}"/> instance to check the size for.</param>
        /// <param name="size">The target size to test.</param>
        /// <param name="name">The name of the input parameter being tested.</param>
        /// <exception cref="ArgumentException">Thrown if the size of <paramref name="enumerable"/> is &lt; <paramref name="size"/>.</exception>
        /// <remarks>The method will skip enumerating <paramref name="enumerable"/> if possible (if it's an <see cref="ICollection{T}"/> or <see cref="IReadOnlyCollection{T}"/>).</remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void HasSizeAtLeastOrEqualTo<T>(IEnumerable<T> enumerable, int size, string name)
        {
            int actualSize;

            if (((enumerable as ICollection<T>)?.Count is int collectionCount &&
                 (actualSize = collectionCount) < size) ||
                ((enumerable as IReadOnlyCollection<T>)?.Count is int readOnlyCollectionCount &&
                 (actualSize = readOnlyCollectionCount) < size) ||
                (actualSize = enumerable.Count()) < size)
            {
                throw new ArgumentException($"Parameter {name} must be sized >= {size}, had a size of {actualSize}");
            }
        }

        /// <summary>
        /// Asserts that the input <see cref="IEnumerable{T}"/> instance must have a size of less than a specified value.
        /// </summary>
        /// <typeparam name="T">The type of items in the input <see cref="IEnumerable{T}"/> instance.</typeparam>
        /// <param name="enumerable">The input <see cref="IEnumerable{T}"/> instance to check the size for.</param>
        /// <param name="size">The target size to test.</param>
        /// <param name="name">The name of the input parameter being tested.</param>
        /// <exception cref="ArgumentException">Thrown if the size of <paramref name="enumerable"/> is >= <paramref name="size"/>.</exception>
        /// <remarks>The method will skip enumerating <paramref name="enumerable"/> if possible (if it's an <see cref="ICollection{T}"/> or <see cref="IReadOnlyCollection{T}"/>).</remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void HasSizeLessThan<T>(IEnumerable<T> enumerable, int size, string name)
        {
            int actualSize;

            if (((enumerable as ICollection<T>)?.Count is int collectionCount &&
                 (actualSize = collectionCount) >= size) ||
                ((enumerable as IReadOnlyCollection<T>)?.Count is int readOnlyCollectionCount &&
                 (actualSize = readOnlyCollectionCount) >= size) ||
                (actualSize = enumerable.Count()) >= size)
            {
                throw new ArgumentException($"Parameter {name} must be sized < {size}, had a size of {actualSize}");
            }
        }

        /// <summary>
        /// Asserts that the input <see cref="IEnumerable{T}"/> instance must have a size of less than or equal to a specified value.
        /// </summary>
        /// <typeparam name="T">The type of items in the input <see cref="IEnumerable{T}"/> instance.</typeparam>
        /// <param name="enumerable">The input <see cref="IEnumerable{T}"/> instance to check the size for.</param>
        /// <param name="size">The target size to test.</param>
        /// <param name="name">The name of the input parameter being tested.</param>
        /// <exception cref="ArgumentException">Thrown if the size of <paramref name="enumerable"/> is > <paramref name="size"/>.</exception>
        /// <remarks>The method will skip enumerating <paramref name="enumerable"/> if possible (if it's an <see cref="ICollection{T}"/> or <see cref="IReadOnlyCollection{T}"/>).</remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void HasSizeLessThanOrEqualTo<T>(IEnumerable<T> enumerable, int size, string name)
        {
            int actualSize;

            if (((enumerable as ICollection<T>)?.Count is int collectionCount &&
                 (actualSize = collectionCount) > size) ||
                ((enumerable as IReadOnlyCollection<T>)?.Count is int readOnlyCollectionCount &&
                 (actualSize = readOnlyCollectionCount) > size) ||
                (actualSize = enumerable.Count()) > size)
            {
                throw new ArgumentException($"Parameter {name} must be sized <= {size}, had a size of {actualSize}");
            }
        }

        /// <summary>
        /// Asserts that the input <see cref="ReadOnlySpan{T}"/> instance must have a size of a specified value.
        /// </summary>
        /// <typeparam name="T">The type of items in the input <see cref="ReadOnlySpan{T}"/> instance.</typeparam>
        /// <param name="span">The input <see cref="ReadOnlySpan{T}"/> instance to check the size for.</param>
        /// <param name="size">The target size to test.</param>
        /// <param name="name">The name of the input parameter being tested.</param>
        /// <exception cref="ArgumentException">Thrown if the size of <paramref name="span"/> is != <paramref name="size"/>.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void HasSizeEqualTo<T>(ReadOnlySpan<T> span, int size, string name)
        {
            if (span.Length != size)
            {
                throw new ArgumentException($"Parameter {name} must be sized == {size}, had a size of {span.Length}");
            }
        }

        /// <summary>
        /// Asserts that the input <see cref="ReadOnlySpan{T}"/> instance must have a size of at least specified value.
        /// </summary>
        /// <typeparam name="T">The type of items in the input <see cref="ReadOnlySpan{T}"/> instance.</typeparam>
        /// <param name="span">The input <see cref="ReadOnlySpan{T}"/> instance to check the size for.</param>
        /// <param name="size">The target size to test.</param>
        /// <param name="name">The name of the input parameter being tested.</param>
        /// <exception cref="ArgumentException">Thrown if the size of <paramref name="span"/> is &lt;= <paramref name="size"/>.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void HasSizeAtLeast<T>(ReadOnlySpan<T> span, int size, string name)
        {
            if (span.Length <= size)
            {
                throw new ArgumentException($"Parameter {name} must be sized > {size}, had a size of {span.Length}");
            }
        }

        /// <summary>
        /// Asserts that the input <see cref="ReadOnlySpan{T}"/> instance must have a size of at least or equal to a specified value.
        /// </summary>
        /// <typeparam name="T">The type of items in the input <see cref="ReadOnlySpan{T}"/> instance.</typeparam>
        /// <param name="span">The input <see cref="ReadOnlySpan{T}"/> instance to check the size for.</param>
        /// <param name="size">The target size to test.</param>
        /// <param name="name">The name of the input parameter being tested.</param>
        /// <exception cref="ArgumentException">Thrown if the size of <paramref name="span"/> is &lt; <paramref name="size"/>.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void HasSizeAtLeastOrEqualTo<T>(ReadOnlySpan<T> span, int size, string name)
        {
            if (span.Length < size)
            {
                throw new ArgumentException($"Parameter {name} must be sized >= {size}, had a size of {span.Length}");
            }
        }

        /// <summary>
        /// Asserts that the input <see cref="ReadOnlySpan{T}"/> instance must have a size of less than a specified value.
        /// </summary>
        /// <typeparam name="T">The type of items in the input <see cref="ReadOnlySpan{T}"/> instance.</typeparam>
        /// <param name="span">The input <see cref="ReadOnlySpan{T}"/> instance to check the size for.</param>
        /// <param name="size">The target size to test.</param>
        /// <param name="name">The name of the input parameter being tested.</param>
        /// <exception cref="ArgumentException">Thrown if the size of <paramref name="span"/> is >= <paramref name="size"/>.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void HasSizeLessThan<T>(ReadOnlySpan<T> span, int size, string name)
        {
            if (span.Length >= size)
            {
                throw new ArgumentException($"Parameter {name} must be sized < {size}, had a size of {span.Length}");
            }
        }

        /// <summary>
        /// Asserts that the input <see cref="ReadOnlySpan{T}"/> instance must have a size of less than or equal to a specified value.
        /// </summary>
        /// <typeparam name="T">The type of items in the input <see cref="ReadOnlySpan{T}"/> instance.</typeparam>
        /// <param name="span">The input <see cref="ReadOnlySpan{T}"/> instance to check the size for.</param>
        /// <param name="size">The target size to test.</param>
        /// <param name="name">The name of the input parameter being tested.</param>
        /// <exception cref="ArgumentException">Thrown if the size of <paramref name="span"/> is > <paramref name="size"/>.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void HasSizeLessThanOrEqualTo<T>(ReadOnlySpan<T> span, int size, string name)
        {
            if (span.Length > size)
            {
                throw new ArgumentException($"Parameter {name} must be sized <= {size}, had a size of {span.Length}");
            }
        }

        /// <summary>
        /// Asserts that the input <see cref="Stream"/> instance must support reading.
        /// </summary>
        /// <param name="stream">The input <see cref="Stream"/> instance to test.</param>
        /// <param name="name">The name of the input parameter being tested.</param>
        /// <exception cref="ArgumentException">Thrown if <paramref name="stream"/> doesn't support reading.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CanRead(Stream stream, string name)
        {
            if (!stream.CanRead)
            {
                throw new ArgumentException($"Stream {name} doesn't support reading");
            }
        }

        /// <summary>
        /// Asserts that the input <see cref="Stream"/> instance must support writing.
        /// </summary>
        /// <param name="stream">The input <see cref="Stream"/> instance to test.</param>
        /// <param name="name">The name of the input parameter being tested.</param>
        /// <exception cref="ArgumentException">Thrown if <paramref name="stream"/> doesn't support writing.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CanWrite(Stream stream, string name)
        {
            if (!stream.CanWrite)
            {
                throw new ArgumentException($"Stream {name} doesn't support writing");
            }
        }

        /// <summary>
        /// Asserts that the input <see cref="Stream"/> instance must support seeking.
        /// </summary>
        /// <param name="stream">The input <see cref="Stream"/> instance to test.</param>
        /// <param name="name">The name of the input parameter being tested.</param>
        /// <exception cref="ArgumentException">Thrown if <paramref name="stream"/> doesn't support seeking.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CanSeek(Stream stream, string name)
        {
            if (!stream.CanSeek)
            {
                throw new ArgumentException($"Stream {name} doesn't support seeking");
            }
        }

        /// <summary>
        /// Asserts that the input <see cref="Stream"/> instance must be at the starting position.
        /// </summary>
        /// <param name="stream">The input <see cref="Stream"/> instance to test.</param>
        /// <param name="name">The name of the input parameter being tested.</param>
        /// <exception cref="ArgumentException">Thrown if <paramref name="stream"/> is not at the starting position.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void IsAtStartPosition(Stream stream, string name)
        {
            if (stream.Position != 0)
            {
                throw new ArgumentException($"Stream {name} must be at start position, was at {stream.Position}");
            }
        }
    }
}
