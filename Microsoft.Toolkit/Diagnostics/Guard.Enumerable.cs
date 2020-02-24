// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
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
        /// Asserts that the input <see cref="ICollection{T}"/> instance must be empty.
        /// </summary>
        /// <typeparam name="T">The type of items in the input <see cref="ICollection{T}"/> instance.</typeparam>
        /// <param name="collection">The input <see cref="ICollection{T}"/> instance to check the size for.</param>
        /// <param name="name">The name of the input parameter being tested.</param>
        /// <exception cref="ArgumentException">Thrown if the size of <paramref name="collection"/> is != 0.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void IsEmpty<T>(ICollection<T> collection, string name)
        {
            if (collection.Count != 0)
            {
                ThrowHelper.ThrowArgumentExceptionForIsEmpty(collection, name);
            }
        }

        /// <summary>
        /// Asserts that the input <see cref="IReadOnlyCollection{T}"/> instance must be empty.
        /// </summary>
        /// <typeparam name="T">The type of items in the input <see cref="IReadOnlyCollection{T}"/> instance.</typeparam>
        /// <param name="collection">The input <see cref="IReadOnlyCollection{T}"/> instance to check the size for.</param>
        /// <param name="name">The name of the input parameter being tested.</param>
        /// <exception cref="ArgumentException">Thrown if the size of <paramref name="collection"/> is != 0.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void IsEmpty<T>(IReadOnlyCollection<T> collection, string name)
        {
            if (collection.Count != 0)
            {
                ThrowHelper.ThrowArgumentExceptionForIsEmpty(collection, name);
            }
        }

        /// <summary>
        /// Asserts that the input <see cref="IEnumerable{T}"/> instance must be empty.
        /// </summary>
        /// <typeparam name="T">The type of items in the input <see cref="IEnumerable{T}"/> instance.</typeparam>
        /// <param name="enumerable">The input <see cref="IEnumerable{T}"/> instance to check the size for.</param>
        /// <param name="name">The name of the input parameter being tested.</param>
        /// <exception cref="ArgumentException">Thrown if the size of <paramref name="enumerable"/> is != 0.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void IsEmpty<T>(IEnumerable<T> enumerable, string name)
        {
            if (!enumerable.Any())
            {
                ThrowHelper.ThrowArgumentExceptionForIsEmpty(enumerable, name);
            }
        }

        /// <summary>
        /// Asserts that the input <see cref="ICollection{T}"/> instance must not be empty.
        /// </summary>
        /// <typeparam name="T">The type of items in the input <see cref="ICollection{T}"/> instance.</typeparam>
        /// <param name="collection">The input <see cref="ICollection{T}"/> instance to check the size for.</param>
        /// <param name="name">The name of the input parameter being tested.</param>
        /// <exception cref="ArgumentException">Thrown if the size of <paramref name="collection"/> is == 0.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void IsNotEmpty<T>(ICollection<T> collection, string name)
        {
            if (collection.Count == 0)
            {
                ThrowHelper.ThrowArgumentExceptionForIsNotEmpty(collection, name);
            }
        }

        /// <summary>
        /// Asserts that the input <see cref="IReadOnlyCollection{T}"/> instance must not be empty.
        /// </summary>
        /// <typeparam name="T">The type of items in the input <see cref="IReadOnlyCollection{T}"/> instance.</typeparam>
        /// <param name="collection">The input <see cref="IReadOnlyCollection{T}"/> instance to check the size for.</param>
        /// <param name="name">The name of the input parameter being tested.</param>
        /// <exception cref="ArgumentException">Thrown if the size of <paramref name="collection"/> is == 0.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void IsNotEmpty<T>(IReadOnlyCollection<T> collection, string name)
        {
            if (collection.Count == 0)
            {
                ThrowHelper.ThrowArgumentExceptionForIsNotEmpty(collection, name);
            }
        }

        /// <summary>
        /// Asserts that the input <see cref="IEnumerable{T}"/> instance must not be empty.
        /// </summary>
        /// <typeparam name="T">The type of items in the input <see cref="IEnumerable{T}"/> instance.</typeparam>
        /// <param name="enumerable">The input <see cref="IEnumerable{T}"/> instance to check the size for.</param>
        /// <param name="name">The name of the input parameter being tested.</param>
        /// <exception cref="ArgumentException">Thrown if the size of <paramref name="enumerable"/> is == 0.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void IsNotEmpty<T>(IEnumerable<T> enumerable, string name)
        {
            if (enumerable.Any())
            {
                ThrowHelper.ThrowArgumentExceptionForIsNotEmpty(enumerable, name);
            }
        }

        /// <summary>
        /// Asserts that the input <see cref="ICollection{T}"/> instance must have a size of a specified value.
        /// </summary>
        /// <typeparam name="T">The type of items in the input <see cref="ICollection{T}"/> instance.</typeparam>
        /// <param name="collection">The input <see cref="ICollection{T}"/> instance to check the size for.</param>
        /// <param name="size">The target size to test.</param>
        /// <param name="name">The name of the input parameter being tested.</param>
        /// <exception cref="ArgumentException">Thrown if the size of <paramref name="collection"/> is != <paramref name="size"/>.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void HasSizeEqualTo<T>(ICollection<T> collection, int size, string name)
        {
            if (collection.Count != size)
            {
                ThrowHelper.ThrowArgumentExceptionForHasSizeEqualTo(collection, size, name);
            }
        }

        /// <summary>
        /// Asserts that the input <see cref="IReadOnlyCollection{T}"/> instance must have a size of a specified value.
        /// </summary>
        /// <typeparam name="T">The type of items in the input <see cref="IReadOnlyCollection{T}"/> instance.</typeparam>
        /// <param name="collection">The input <see cref="IReadOnlyCollection{T}"/> instance to check the size for.</param>
        /// <param name="size">The target size to test.</param>
        /// <param name="name">The name of the input parameter being tested.</param>
        /// <exception cref="ArgumentException">Thrown if the size of <paramref name="collection"/> is != <paramref name="size"/>.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void HasSizeEqualTo<T>(IReadOnlyCollection<T> collection, int size, string name)
        {
            if (collection.Count != size)
            {
                ThrowHelper.ThrowArgumentExceptionForHasSizeEqualTo(collection, size, name);
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
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void HasSizeEqualTo<T>(IEnumerable<T> enumerable, int size, string name)
        {
            if (enumerable.Count() != size)
            {
                ThrowHelper.ThrowArgumentExceptionForHasSizeEqualTo(enumerable, size, name);
            }
        }

        /// <summary>
        /// Asserts that the input <see cref="ICollection{T}"/> instance must have a size not equal to a specified value.
        /// </summary>
        /// <typeparam name="T">The type of items in the input <see cref="ICollection{T}"/> instance.</typeparam>
        /// <param name="collection">The input <see cref="ICollection{T}"/> instance to check the size for.</param>
        /// <param name="size">The target size to test.</param>
        /// <param name="name">The name of the input parameter being tested.</param>
        /// <exception cref="ArgumentException">Thrown if the size of <paramref name="collection"/> is == <paramref name="size"/>.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void HasSizeNotEqualTo<T>(ICollection<T> collection, int size, string name)
        {
            if (collection.Count == size)
            {
                ThrowHelper.ThrowArgumentExceptionForHasSizeNotEqualTo(collection, size, name);
            }
        }

        /// <summary>
        /// Asserts that the input <see cref="IReadOnlyCollection{T}"/> instance must have a size not equal to a specified value.
        /// </summary>
        /// <typeparam name="T">The type of items in the input <see cref="IReadOnlyCollection{T}"/> instance.</typeparam>
        /// <param name="collection">The input <see cref="IReadOnlyCollection{T}"/> instance to check the size for.</param>
        /// <param name="size">The target size to test.</param>
        /// <param name="name">The name of the input parameter being tested.</param>
        /// <exception cref="ArgumentException">Thrown if the size of <paramref name="collection"/> is == <paramref name="size"/>.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void HasSizeNotEqualTo<T>(IReadOnlyCollection<T> collection, int size, string name)
        {
            if (collection.Count == size)
            {
                ThrowHelper.ThrowArgumentExceptionForHasSizeNotEqualTo(collection, size, name);
            }
        }

        /// <summary>
        /// Asserts that the input <see cref="IEnumerable{T}"/> instance must have a size not equal to a specified value.
        /// </summary>
        /// <typeparam name="T">The type of items in the input <see cref="IEnumerable{T}"/> instance.</typeparam>
        /// <param name="enumerable">The input <see cref="IEnumerable{T}"/> instance to check the size for.</param>
        /// <param name="size">The target size to test.</param>
        /// <param name="name">The name of the input parameter being tested.</param>
        /// <exception cref="ArgumentException">Thrown if the size of <paramref name="enumerable"/> is == <paramref name="size"/>.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void HasSizeNotEqualTo<T>(IEnumerable<T> enumerable, int size, string name)
        {
            if (enumerable.Count() == size)
            {
                ThrowHelper.ThrowArgumentExceptionForHasSizeNotEqualTo(enumerable, size, name);
            }
        }

        /// <summary>
        /// Asserts that the input <see cref="ICollection{T}"/> instance must have a size over a specified value.
        /// </summary>
        /// <typeparam name="T">The type of items in the input <see cref="ICollection{T}"/> instance.</typeparam>
        /// <param name="collection">The input <see cref="ICollection{T}"/> instance to check the size for.</param>
        /// <param name="size">The target size to test.</param>
        /// <param name="name">The name of the input parameter being tested.</param>
        /// <exception cref="ArgumentException">Thrown if the size of <paramref name="collection"/> is &lt;= <paramref name="size"/>.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void HasSizeOver<T>(ICollection<T> collection, int size, string name)
        {
            if (collection.Count <= size)
            {
                ThrowHelper.ThrowArgumentExceptionForHasSizeOver(collection, size, name);
            }
        }

        /// <summary>
        /// Asserts that the input <see cref="IReadOnlyCollection{T}"/> instance must have a size over a specified value.
        /// </summary>
        /// <typeparam name="T">The type of items in the input <see cref="IReadOnlyCollection{T}"/> instance.</typeparam>
        /// <param name="collection">The input <see cref="IReadOnlyCollection{T}"/> instance to check the size for.</param>
        /// <param name="size">The target size to test.</param>
        /// <param name="name">The name of the input parameter being tested.</param>
        /// <exception cref="ArgumentException">Thrown if the size of <paramref name="collection"/> is &lt;= <paramref name="size"/>.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void HasSizeOver<T>(IReadOnlyCollection<T> collection, int size, string name)
        {
            if (collection.Count <= size)
            {
                ThrowHelper.ThrowArgumentExceptionForHasSizeOver(collection, size, name);
            }
        }

        /// <summary>
        /// Asserts that the input <see cref="IEnumerable{T}"/> instance must have a size over a specified value.
        /// </summary>
        /// <typeparam name="T">The type of items in the input <see cref="IEnumerable{T}"/> instance.</typeparam>
        /// <param name="enumerable">The input <see cref="IEnumerable{T}"/> instance to check the size for.</param>
        /// <param name="size">The target size to test.</param>
        /// <param name="name">The name of the input parameter being tested.</param>
        /// <exception cref="ArgumentException">Thrown if the size of <paramref name="enumerable"/> is &lt;= <paramref name="size"/>.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void HasSizeOver<T>(IEnumerable<T> enumerable, int size, string name)
        {
            if (enumerable.Count() <= size)
            {
                ThrowHelper.ThrowArgumentExceptionForHasSizeOver(enumerable, size, name);
            }
        }

        /// <summary>
        /// Asserts that the input <see cref="ICollection{T}"/> instance must have a size of at least specified value.
        /// </summary>
        /// <typeparam name="T">The type of items in the input <see cref="ICollection{T}"/> instance.</typeparam>
        /// <param name="collection">The input <see cref="ICollection{T}"/> instance to check the size for.</param>
        /// <param name="size">The target size to test.</param>
        /// <param name="name">The name of the input parameter being tested.</param>
        /// <exception cref="ArgumentException">Thrown if the size of <paramref name="collection"/> is &lt; <paramref name="size"/>.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void HasSizeAtLeast<T>(ICollection<T> collection, int size, string name)
        {
            if (collection.Count < size)
            {
                ThrowHelper.ThrowArgumentExceptionForHasSizeAtLeast(collection, size, name);
            }
        }

        /// <summary>
        /// Asserts that the input <see cref="IReadOnlyCollection{T}"/> instance must have a size of at least specified value.
        /// </summary>
        /// <typeparam name="T">The type of items in the input <see cref="IReadOnlyCollection{T}"/> instance.</typeparam>
        /// <param name="collection">The input <see cref="IReadOnlyCollection{T}"/> instance to check the size for.</param>
        /// <param name="size">The target size to test.</param>
        /// <param name="name">The name of the input parameter being tested.</param>
        /// <exception cref="ArgumentException">Thrown if the size of <paramref name="collection"/> is &lt; <paramref name="size"/>.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void HasSizeAtLeast<T>(IReadOnlyCollection<T> collection, int size, string name)
        {
            if (collection.Count < size)
            {
                ThrowHelper.ThrowArgumentExceptionForHasSizeAtLeast(collection, size, name);
            }
        }

        /// <summary>
        /// Asserts that the input <see cref="IEnumerable{T}"/> instance must have a size of at least specified value.
        /// </summary>
        /// <typeparam name="T">The type of items in the input <see cref="IEnumerable{T}"/> instance.</typeparam>
        /// <param name="enumerable">The input <see cref="IEnumerable{T}"/> instance to check the size for.</param>
        /// <param name="size">The target size to test.</param>
        /// <param name="name">The name of the input parameter being tested.</param>
        /// <exception cref="ArgumentException">Thrown if the size of <paramref name="enumerable"/> is &lt; <paramref name="size"/>.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void HasSizeAtLeast<T>(IEnumerable<T> enumerable, int size, string name)
        {
            if (enumerable.Count() < size)
            {
                ThrowHelper.ThrowArgumentExceptionForHasSizeAtLeast(enumerable, size, name);
            }
        }

        /// <summary>
        /// Asserts that the input <see cref="ICollection{T}"/> instance must have a size of less than a specified value.
        /// </summary>
        /// <typeparam name="T">The type of items in the input <see cref="ICollection{T}"/> instance.</typeparam>
        /// <param name="collection">The input <see cref="ICollection{T}"/> instance to check the size for.</param>
        /// <param name="size">The target size to test.</param>
        /// <param name="name">The name of the input parameter being tested.</param>
        /// <exception cref="ArgumentException">Thrown if the size of <paramref name="collection"/> is >= <paramref name="size"/>.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void HasSizeLessThan<T>(ICollection<T> collection, int size, string name)
        {
            if (collection.Count >= size)
            {
                ThrowHelper.ThrowArgumentExceptionForHasSizeLessThan(collection, size, name);
            }
        }

        /// <summary>
        /// Asserts that the input <see cref="IReadOnlyCollection{T}"/> instance must have a size of less than a specified value.
        /// </summary>
        /// <typeparam name="T">The type of items in the input <see cref="IReadOnlyCollection{T}"/> instance.</typeparam>
        /// <param name="collection">The input <see cref="IReadOnlyCollection{T}"/> instance to check the size for.</param>
        /// <param name="size">The target size to test.</param>
        /// <param name="name">The name of the input parameter being tested.</param>
        /// <exception cref="ArgumentException">Thrown if the size of <paramref name="collection"/> is >= <paramref name="size"/>.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void HasSizeLessThan<T>(IReadOnlyCollection<T> collection, int size, string name)
        {
            if (collection.Count >= size)
            {
                ThrowHelper.ThrowArgumentExceptionForHasSizeLessThan(collection, size, name);
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
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void HasSizeLessThan<T>(IEnumerable<T> enumerable, int size, string name)
        {
            if (enumerable.Count() >= size)
            {
                ThrowHelper.ThrowArgumentExceptionForHasSizeLessThan(enumerable, size, name);
            }
        }

        /// <summary>
        /// Asserts that the input <see cref="ICollection{T}"/> instance must have a size of less than or equal to a specified value.
        /// </summary>
        /// <typeparam name="T">The type of items in the input <see cref="ICollection{T}"/> instance.</typeparam>
        /// <param name="collection">The input <see cref="ICollection{T}"/> instance to check the size for.</param>
        /// <param name="size">The target size to test.</param>
        /// <param name="name">The name of the input parameter being tested.</param>
        /// <exception cref="ArgumentException">Thrown if the size of <paramref name="collection"/> is > <paramref name="size"/>.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void HasSizeLessThanOrEqualTo<T>(ICollection<T> collection, int size, string name)
        {
            if (collection.Count > size)
            {
                ThrowHelper.ThrowArgumentExceptionForHasSizeLessThanOrEqualTo(collection, size, name);
            }
        }

        /// <summary>
        /// Asserts that the input <see cref="IReadOnlyCollection{T}"/> instance must have a size of less than or equal to a specified value.
        /// </summary>
        /// <typeparam name="T">The type of items in the input <see cref="IReadOnlyCollection{T}"/> instance.</typeparam>
        /// <param name="collection">The input <see cref="IReadOnlyCollection{T}"/> instance to check the size for.</param>
        /// <param name="size">The target size to test.</param>
        /// <param name="name">The name of the input parameter being tested.</param>
        /// <exception cref="ArgumentException">Thrown if the size of <paramref name="collection"/> is > <paramref name="size"/>.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void HasSizeLessThanOrEqualTo<T>(IReadOnlyCollection<T> collection, int size, string name)
        {
            if (collection.Count > size)
            {
                ThrowHelper.ThrowArgumentExceptionForHasSizeLessThanOrEqualTo(collection, size, name);
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
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void HasSizeLessThanOrEqualTo<T>(IEnumerable<T> enumerable, int size, string name)
        {
            if (enumerable.Count() > size)
            {
                ThrowHelper.ThrowArgumentExceptionForHasSizeLessThanOrEqualTo(enumerable, size, name);
            }
        }
    }
}
