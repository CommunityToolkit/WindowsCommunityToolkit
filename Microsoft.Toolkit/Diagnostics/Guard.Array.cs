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
        /// Asserts that the input <typeparamref name="T"/> array instance must be empty.
        /// </summary>
        /// <typeparam name="T">The type of items in the input <typeparamref name="T"/> array instance.</typeparam>
        /// <param name="array">The input <typeparamref name="T"/> array instance to check the size for.</param>
        /// <param name="name">The name of the input parameter being tested.</param>
        /// <exception cref="ArgumentException">Thrown if the size of <paramref name="array"/> is != 0.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void IsEmpty<T>(T[] array, string name)
        {
            if (array.Length != 0)
            {
                ThrowArgumentException(name, $"Parameter {name} must be empty, had a size of {array.Length}");
            }
        }

        /// <summary>
        /// Asserts that the input <typeparamref name="T"/> array instance must not be empty.
        /// </summary>
        /// <typeparam name="T">The type of items in the input <typeparamref name="T"/> array instance.</typeparam>
        /// <param name="array">The input <typeparamref name="T"/> array instance to check the size for.</param>
        /// <param name="name">The name of the input parameter being tested.</param>
        /// <exception cref="ArgumentException">Thrown if the size of <paramref name="array"/> is == 0.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void IsNotEmpty<T>(T[] array, string name)
        {
            if (array.Length != 0)
            {
                ThrowArgumentException(name, $"Parameter {name} must not be empty");
            }
        }

        /// <summary>
        /// Asserts that the input <typeparamref name="T"/> array instance must have a size of a specified value.
        /// </summary>
        /// <typeparam name="T">The type of items in the input <typeparamref name="T"/> array instance.</typeparam>
        /// <param name="array">The input <typeparamref name="T"/> array instance to check the size for.</param>
        /// <param name="size">The target size to test.</param>
        /// <param name="name">The name of the input parameter being tested.</param>
        /// <exception cref="ArgumentException">Thrown if the size of <paramref name="array"/> is != <paramref name="size"/>.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void HasSizeEqualTo<T>(T[] array, int size, string name)
        {
            if (array.Length != size)
            {
                ThrowArgumentException(name, $"Parameter {name} must be sized == {size}, had a size of {array.Length}");
            }
        }

        /// <summary>
        /// Asserts that the input <typeparamref name="T"/> array instance must have a size not equal to a specified value.
        /// </summary>
        /// <typeparam name="T">The type of items in the input <typeparamref name="T"/> array instance.</typeparam>
        /// <param name="array">The input <typeparamref name="T"/> array instance to check the size for.</param>
        /// <param name="size">The target size to test.</param>
        /// <param name="name">The name of the input parameter being tested.</param>
        /// <exception cref="ArgumentException">Thrown if the size of <paramref name="array"/> is == <paramref name="size"/>.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void HasSizeNotEqualTo<T>(T[] array, int size, string name)
        {
            if (array.Length == size)
            {
                ThrowArgumentException(name, $"Parameter {name} must be sized != {size}, had a size of {array.Length}");
            }
        }

        /// <summary>
        /// Asserts that the input <typeparamref name="T"/> array instance must have a size of at least specified value.
        /// </summary>
        /// <typeparam name="T">The type of items in the input <typeparamref name="T"/> array instance.</typeparam>
        /// <param name="array">The input <typeparamref name="T"/> array instance to check the size for.</param>
        /// <param name="size">The target size to test.</param>
        /// <param name="name">The name of the input parameter being tested.</param>
        /// <exception cref="ArgumentException">Thrown if the size of <paramref name="array"/> is &lt;= <paramref name="size"/>.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void HasSizeAtLeast<T>(T[] array, int size, string name)
        {
            if (array.Length <= size)
            {
                ThrowArgumentException(name, $"Parameter {name} must be sized > {size}, had a size of {array.Length}");
            }
        }

        /// <summary>
        /// Asserts that the input <typeparamref name="T"/> array instance must have a size of at least or equal to a specified value.
        /// </summary>
        /// <typeparam name="T">The type of items in the input <typeparamref name="T"/> array instance.</typeparam>
        /// <param name="array">The input <typeparamref name="T"/> array instance to check the size for.</param>
        /// <param name="size">The target size to test.</param>
        /// <param name="name">The name of the input parameter being tested.</param>
        /// <exception cref="ArgumentException">Thrown if the size of <paramref name="array"/> is &lt; <paramref name="size"/>.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void HasSizeAtLeastOrEqualTo<T>(T[] array, int size, string name)
        {
            if (array.Length < size)
            {
                ThrowArgumentException(name, $"Parameter {name} must be sized >= {size}, had a size of {array.Length}");
            }
        }

        /// <summary>
        /// Asserts that the input <typeparamref name="T"/> array instance must have a size of less than a specified value.
        /// </summary>
        /// <typeparam name="T">The type of items in the input <typeparamref name="T"/> array instance.</typeparam>
        /// <param name="array">The input <typeparamref name="T"/> array instance to check the size for.</param>
        /// <param name="size">The target size to test.</param>
        /// <param name="name">The name of the input parameter being tested.</param>
        /// <exception cref="ArgumentException">Thrown if the size of <paramref name="array"/> is >= <paramref name="size"/>.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void HasSizeLessThan<T>(T[] array, int size, string name)
        {
            if (array.Length >= size)
            {
                ThrowArgumentException(name, $"Parameter {name} must be sized < {size}, had a size of {array.Length}");
            }
        }

        /// <summary>
        /// Asserts that the input <typeparamref name="T"/> array instance must have a size of less than or equal to a specified value.
        /// </summary>
        /// <typeparam name="T">The type of items in the input <typeparamref name="T"/> array instance.</typeparam>
        /// <param name="array">The input <typeparamref name="T"/> array instance to check the size for.</param>
        /// <param name="size">The target size to test.</param>
        /// <param name="name">The name of the input parameter being tested.</param>
        /// <exception cref="ArgumentException">Thrown if the size of <paramref name="array"/> is > <paramref name="size"/>.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void HasSizeLessThanOrEqualTo<T>(T[] array, int size, string name)
        {
            if (array.Length > size)
            {
                ThrowArgumentException(name, $"Parameter {name} must be sized <= {size}, had a size of {array.Length}");
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
        public static void HasSizeEqualTo<T>(T[] source, T[] destination, string name)
        {
            if (source.Length != destination.Length)
            {
                ThrowArgumentException(name, $"The source {name} must be sized == {destination.Length}, had a size of {source.Length}");
            }
        }

        /// <summary>
        /// Asserts that the source <see cref="ReadOnlyMemory{T}"/> instance must have a size of less than or equal to that of a destination <see cref="Span{T}"/> instance.
        /// </summary>
        /// <typeparam name="T">The type of items in the input <see cref="ReadOnlyMemory{T}"/> instance.</typeparam>
        /// <param name="source">The source <see cref="ReadOnlyMemory{T}"/> instance to check the size for.</param>
        /// <param name="destination">The destination <see cref="Memory{T}"/> instance to check the size for.</param>
        /// <param name="name">The name of the input parameter being tested.</param>
        /// <exception cref="ArgumentException">Thrown if the size of <paramref name="source"/> is > the one of <paramref name="destination"/>.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void HasSizeLessThanOrEqualTo<T>(T[] source, T[] destination, string name)
        {
            if (source.Length > destination.Length)
            {
                ThrowArgumentException(name, $"The source {name} must be sized <= {destination.Length}, had a size of {source.Length}");
            }
        }
    }
}
