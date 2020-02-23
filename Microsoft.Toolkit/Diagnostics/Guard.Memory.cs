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
        /// Asserts that the input <see cref="ReadOnlySpan{T}"/> instance must be empty.
        /// </summary>
        /// <typeparam name="T">The type of items in the input <see cref="ReadOnlySpan{T}"/> instance.</typeparam>
        /// <param name="span">The input <see cref="ReadOnlySpan{T}"/> instance to check the size for.</param>
        /// <param name="name">The name of the input parameter being tested.</param>
        /// <exception cref="ArgumentException">Thrown if the size of <paramref name="span"/> is != 0.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void IsEmpty<T>(ReadOnlySpan<T> span, string name)
        {
            if (span.Length != 0)
            {
                ThrowHelper.ThrowArgumentExceptionForIsEmpty(span, name);
            }
        }

        /// <summary>
        /// Asserts that the input <see cref="ReadOnlySpan{T}"/> instance must not be empty.
        /// </summary>
        /// <typeparam name="T">The type of items in the input <see cref="ReadOnlySpan{T}"/> instance.</typeparam>
        /// <param name="span">The input <see cref="ReadOnlySpan{T}"/> instance to check the size for.</param>
        /// <param name="name">The name of the input parameter being tested.</param>
        /// <exception cref="ArgumentException">Thrown if the size of <paramref name="span"/> is == 0.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void IsNotEmpty<T>(ReadOnlySpan<T> span, string name)
        {
            if (span.Length != 0)
            {
                ThrowHelper.ThrowArgumentExceptionForIsNotEmpty(span, name);
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
                ThrowHelper.ThrowArgumentExceptionForHasSizeEqualTo(span, size, name);
            }
        }

        /// <summary>
        /// Asserts that the input <see cref="ReadOnlySpan{T}"/> instance must have a size not equal to a specified value.
        /// </summary>
        /// <typeparam name="T">The type of items in the input <see cref="ReadOnlySpan{T}"/> instance.</typeparam>
        /// <param name="span">The input <see cref="ReadOnlySpan{T}"/> instance to check the size for.</param>
        /// <param name="size">The target size to test.</param>
        /// <param name="name">The name of the input parameter being tested.</param>
        /// <exception cref="ArgumentException">Thrown if the size of <paramref name="span"/> is == <paramref name="size"/>.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void HasSizeNotEqualTo<T>(ReadOnlySpan<T> span, int size, string name)
        {
            if (span.Length == size)
            {
                ThrowHelper.ThrowArgumentExceptionForHasSizeNotEqualTo(span, size, name);
            }
        }

        /// <summary>
        /// Asserts that the input <see cref="ReadOnlySpan{T}"/> instance must have a size over a specified value.
        /// </summary>
        /// <typeparam name="T">The type of items in the input <see cref="ReadOnlySpan{T}"/> instance.</typeparam>
        /// <param name="span">The input <see cref="ReadOnlySpan{T}"/> instance to check the size for.</param>
        /// <param name="size">The target size to test.</param>
        /// <param name="name">The name of the input parameter being tested.</param>
        /// <exception cref="ArgumentException">Thrown if the size of <paramref name="span"/> is &lt;= <paramref name="size"/>.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void HasSizeOver<T>(ReadOnlySpan<T> span, int size, string name)
        {
            if (span.Length <= size)
            {
                ThrowHelper.ThrowArgumentExceptionForHasSizeOver(span, size, name);
            }
        }

        /// <summary>
        /// Asserts that the input <see cref="ReadOnlySpan{T}"/> instance must have a size of at least specified value.
        /// </summary>
        /// <typeparam name="T">The type of items in the input <see cref="ReadOnlySpan{T}"/> instance.</typeparam>
        /// <param name="span">The input <see cref="ReadOnlySpan{T}"/> instance to check the size for.</param>
        /// <param name="size">The target size to test.</param>
        /// <param name="name">The name of the input parameter being tested.</param>
        /// <exception cref="ArgumentException">Thrown if the size of <paramref name="span"/> is &lt; <paramref name="size"/>.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void HasSizeAtLeast<T>(ReadOnlySpan<T> span, int size, string name)
        {
            if (span.Length < size)
            {
                ThrowHelper.ThrowArgumentExceptionForHasSizeAtLeast(span, size, name);
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
                ThrowHelper.ThrowArgumentExceptionForHasSizeLessThan(span, size, name);
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
                ThrowHelper.ThrowArgumentExceptionForHasSizeLessThanOrEqualTo(span, size, name);
            }
        }

        /// <summary>
        /// Asserts that the source <see cref="ReadOnlySpan{T}"/> instance must have the same size of a destination <see cref="Span{T}"/> instance.
        /// </summary>
        /// <typeparam name="T">The type of items in the input <see cref="ReadOnlySpan{T}"/> instance.</typeparam>
        /// <param name="source">The source <see cref="ReadOnlySpan{T}"/> instance to check the size for.</param>
        /// <param name="destination">The destination <see cref="Span{T}"/> instance to check the size for.</param>
        /// <param name="name">The name of the input parameter being tested.</param>
        /// <exception cref="ArgumentException">Thrown if the size of <paramref name="source"/> is != the one of <paramref name="destination"/>.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void HasSizeEqualTo<T>(ReadOnlySpan<T> source, Span<T> destination, string name)
        {
            if (source.Length != destination.Length)
            {
                ThrowHelper.ThrowArgumentExceptionForHasSizeEqualTo(source, destination, name);
            }
        }

        /// <summary>
        /// Asserts that the source <see cref="ReadOnlySpan{T}"/> instance must have a size of less than or equal to that of a destination <see cref="Span{T}"/> instance.
        /// </summary>
        /// <typeparam name="T">The type of items in the input <see cref="ReadOnlySpan{T}"/> instance.</typeparam>
        /// <param name="source">The source <see cref="ReadOnlySpan{T}"/> instance to check the size for.</param>
        /// <param name="destination">The destination <see cref="Span{T}"/> instance to check the size for.</param>
        /// <param name="name">The name of the input parameter being tested.</param>
        /// <exception cref="ArgumentException">Thrown if the size of <paramref name="source"/> is > the one of <paramref name="destination"/>.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void HasSizeLessThanOrEqualTo<T>(ReadOnlySpan<T> source, Span<T> destination, string name)
        {
            if (source.Length > destination.Length)
            {
                ThrowHelper.ThrowArgumentExceptionForHasSizeLessThanOrEqualTo(source, destination, name);
            }
        }

        /// <summary>
        /// Asserts that the input <see cref="ReadOnlyMemory{T}"/> instance must be empty.
        /// </summary>
        /// <typeparam name="T">The type of items in the input <see cref="ReadOnlyMemory{T}"/> instance.</typeparam>
        /// <param name="memory">The input <see cref="ReadOnlyMemory{T}"/> instance to check the size for.</param>
        /// <param name="name">The name of the input parameter being tested.</param>
        /// <exception cref="ArgumentException">Thrown if the size of <paramref name="memory"/> is != 0.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void IsEmpty<T>(ReadOnlyMemory<T> memory, string name)
        {
            if (memory.Length != 0)
            {
                ThrowArgumentException(name, $"Parameter {name} must be empty, had a size of {memory.Length}");
            }
        }

        /// <summary>
        /// Asserts that the input <see cref="ReadOnlyMemory{T}"/> instance must not be empty.
        /// </summary>
        /// <typeparam name="T">The type of items in the input <see cref="ReadOnlyMemory{T}"/> instance.</typeparam>
        /// <param name="memory">The input <see cref="ReadOnlyMemory{T}"/> instance to check the size for.</param>
        /// <param name="name">The name of the input parameter being tested.</param>
        /// <exception cref="ArgumentException">Thrown if the size of <paramref name="memory"/> is == 0.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void IsNotEmpty<T>(ReadOnlyMemory<T> memory, string name)
        {
            if (memory.Length != 0)
            {
                ThrowArgumentException(name, $"Parameter {name} must not be empty");
            }
        }

        /// <summary>
        /// Asserts that the input <see cref="ReadOnlyMemory{T}"/> instance must have a size of a specified value.
        /// </summary>
        /// <typeparam name="T">The type of items in the input <see cref="ReadOnlyMemory{T}"/> instance.</typeparam>
        /// <param name="memory">The input <see cref="ReadOnlyMemory{T}"/> instance to check the size for.</param>
        /// <param name="size">The target size to test.</param>
        /// <param name="name">The name of the input parameter being tested.</param>
        /// <exception cref="ArgumentException">Thrown if the size of <paramref name="memory"/> is != <paramref name="size"/>.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void HasSizeEqualTo<T>(ReadOnlyMemory<T> memory, int size, string name)
        {
            if (memory.Length != size)
            {
                ThrowArgumentException(name, $"Parameter {name} must be sized == {size}, had a size of {memory.Length}");
            }
        }

        /// <summary>
        /// Asserts that the input <see cref="ReadOnlyMemory{T}"/> instance must have a size not equal to a specified value.
        /// </summary>
        /// <typeparam name="T">The type of items in the input <see cref="ReadOnlyMemory{T}"/> instance.</typeparam>
        /// <param name="memory">The input <see cref="ReadOnlyMemory{T}"/> instance to check the size for.</param>
        /// <param name="size">The target size to test.</param>
        /// <param name="name">The name of the input parameter being tested.</param>
        /// <exception cref="ArgumentException">Thrown if the size of <paramref name="memory"/> is == <paramref name="size"/>.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void HasSizeNotEqualTo<T>(ReadOnlyMemory<T> memory, int size, string name)
        {
            if (memory.Length == size)
            {
                ThrowArgumentException(name, $"Parameter {name} must be sized != {size}, had a size of {memory.Length}");
            }
        }

        /// <summary>
        /// Asserts that the input <see cref="ReadOnlyMemory{T}"/> instance must have a size over a specified value.
        /// </summary>
        /// <typeparam name="T">The type of items in the input <see cref="ReadOnlyMemory{T}"/> instance.</typeparam>
        /// <param name="memory">The input <see cref="ReadOnlyMemory{T}"/> instance to check the size for.</param>
        /// <param name="size">The target size to test.</param>
        /// <param name="name">The name of the input parameter being tested.</param>
        /// <exception cref="ArgumentException">Thrown if the size of <paramref name="memory"/> is &lt;= <paramref name="size"/>.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void HasSizeOver<T>(ReadOnlyMemory<T> memory, int size, string name)
        {
            if (memory.Length <= size)
            {
                ThrowArgumentException(name, $"Parameter {name} must be sized > {size}, had a size of {memory.Length}");
            }
        }

        /// <summary>
        /// Asserts that the input <see cref="ReadOnlyMemory{T}"/> instance must have a size of at least specified value.
        /// </summary>
        /// <typeparam name="T">The type of items in the input <see cref="ReadOnlyMemory{T}"/> instance.</typeparam>
        /// <param name="memory">The input <see cref="ReadOnlyMemory{T}"/> instance to check the size for.</param>
        /// <param name="size">The target size to test.</param>
        /// <param name="name">The name of the input parameter being tested.</param>
        /// <exception cref="ArgumentException">Thrown if the size of <paramref name="memory"/> is &lt; <paramref name="size"/>.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void HasSizeAtLeast<T>(ReadOnlyMemory<T> memory, int size, string name)
        {
            if (memory.Length < size)
            {
                ThrowArgumentException(name, $"Parameter {name} must be sized >= {size}, had a size of {memory.Length}");
            }
        }

        /// <summary>
        /// Asserts that the input <see cref="ReadOnlyMemory{T}"/> instance must have a size of less than a specified value.
        /// </summary>
        /// <typeparam name="T">The type of items in the input <see cref="ReadOnlyMemory{T}"/> instance.</typeparam>
        /// <param name="memory">The input <see cref="ReadOnlyMemory{T}"/> instance to check the size for.</param>
        /// <param name="size">The target size to test.</param>
        /// <param name="name">The name of the input parameter being tested.</param>
        /// <exception cref="ArgumentException">Thrown if the size of <paramref name="memory"/> is >= <paramref name="size"/>.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void HasSizeLessThan<T>(ReadOnlyMemory<T> memory, int size, string name)
        {
            if (memory.Length >= size)
            {
                ThrowArgumentException(name, $"Parameter {name} must be sized < {size}, had a size of {memory.Length}");
            }
        }

        /// <summary>
        /// Asserts that the input <see cref="ReadOnlyMemory{T}"/> instance must have a size of less than or equal to a specified value.
        /// </summary>
        /// <typeparam name="T">The type of items in the input <see cref="ReadOnlyMemory{T}"/> instance.</typeparam>
        /// <param name="memory">The input <see cref="ReadOnlyMemory{T}"/> instance to check the size for.</param>
        /// <param name="size">The target size to test.</param>
        /// <param name="name">The name of the input parameter being tested.</param>
        /// <exception cref="ArgumentException">Thrown if the size of <paramref name="memory"/> is > <paramref name="size"/>.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void HasSizeLessThanOrEqualTo<T>(ReadOnlyMemory<T> memory, int size, string name)
        {
            if (memory.Length > size)
            {
                ThrowArgumentException(name, $"Parameter {name} must be sized <= {size}, had a size of {memory.Length}");
            }
        }

        /// <summary>
        /// Asserts that the source <see cref="ReadOnlyMemory{T}"/> instance must have the same size of a destination <see cref="Memory{T}"/> instance.
        /// </summary>
        /// <typeparam name="T">The type of items in the input <see cref="ReadOnlyMemory{T}"/> instance.</typeparam>
        /// <param name="source">The source <see cref="ReadOnlyMemory{T}"/> instance to check the size for.</param>
        /// <param name="destination">The destination <see cref="Memory{T}"/> instance to check the size for.</param>
        /// <param name="name">The name of the input parameter being tested.</param>
        /// <exception cref="ArgumentException">Thrown if the size of <paramref name="source"/> is != the one of <paramref name="destination"/>.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void HasSizeEqualTo<T>(ReadOnlyMemory<T> source, Memory<T> destination, string name)
        {
            if (source.Length != destination.Length)
            {
                ThrowArgumentException(name, $"The source {name} must be sized == {destination.Length}, had a size of {source.Length}");
            }
        }

        /// <summary>
        /// Asserts that the source <see cref="ReadOnlyMemory{T}"/> instance must have a size of less than or equal to that of a destination <see cref="Memory{T}"/> instance.
        /// </summary>
        /// <typeparam name="T">The type of items in the input <see cref="ReadOnlyMemory{T}"/> instance.</typeparam>
        /// <param name="source">The source <see cref="ReadOnlyMemory{T}"/> instance to check the size for.</param>
        /// <param name="destination">The destination <see cref="Memory{T}"/> instance to check the size for.</param>
        /// <param name="name">The name of the input parameter being tested.</param>
        /// <exception cref="ArgumentException">Thrown if the size of <paramref name="source"/> is > the one of <paramref name="destination"/>.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void HasSizeLessThanOrEqualTo<T>(ReadOnlyMemory<T> source, Memory<T> destination, string name)
        {
            if (source.Length > destination.Length)
            {
                ThrowArgumentException(name, $"The source {name} must be sized <= {destination.Length}, had a size of {source.Length}");
            }
        }
    }
}
