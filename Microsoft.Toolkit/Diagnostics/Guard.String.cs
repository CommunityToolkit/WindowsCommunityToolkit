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
        /// Asserts that the input <see cref="string"/> instance must be <see langword="null"/> or empty.
        /// </summary>
        /// <param name="text">The input <see cref="string"/> instance to test.</param>
        /// <param name="name">The name of the input parameter being tested.</param>
        /// <exception cref="ArgumentException">Thrown if <paramref name="text"/> is neither <see langword="null"/> nor empty.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void IsNullOrEmpty(string? text, string name)
        {
            if (!string.IsNullOrEmpty(text))
            {
                ThrowArgumentException(name, $"Parameter {name} must be null or empty, was \"{text}\"");
            }
        }

        /// <summary>
        /// Asserts that the input <see cref="string"/> instance must not be <see langword="null"/> or empty.
        /// </summary>
        /// <param name="text">The input <see cref="string"/> instance to test.</param>
        /// <param name="name">The name of the input parameter being tested.</param>
        /// <exception cref="ArgumentException">Thrown if <paramref name="text"/> is <see langword="null"/> or empty.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void IsNotNullOrEmpty(string? text, string name)
        {
            if (string.IsNullOrEmpty(text))
            {
                ThrowArgumentException(name, $"Parameter {name} must not be null or empty, was {(text is null ? "null" : "empty")}");
            }
        }

        /// <summary>
        /// Asserts that the input <see cref="string"/> instance must be <see langword="null"/> or whitespace.
        /// </summary>
        /// <param name="text">The input <see cref="string"/> instance to test.</param>
        /// <param name="name">The name of the input parameter being tested.</param>
        /// <exception cref="ArgumentException">Thrown if <paramref name="text"/> is neither <see langword="null"/> nor whitespace.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void IsNullOrWhitespace(string? text, string name)
        {
            if (!string.IsNullOrWhiteSpace(text))
            {
                ThrowArgumentException(name, $"Parameter {name} must be null or whitespace, was \"{text}\"");
            }
        }

        /// <summary>
        /// Asserts that the input <see cref="string"/> instance must not be <see langword="null"/> or whitespace.
        /// </summary>
        /// <param name="text">The input <see cref="string"/> instance to test.</param>
        /// <param name="name">The name of the input parameter being tested.</param>
        /// <exception cref="ArgumentException">Thrown if <paramref name="text"/> is <see langword="null"/> or whitespace.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void IsNotNullOrWhitespace(string? text, string name)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                ThrowArgumentException(name, $"Parameter {name} must not be null or whitespace, was {(text is null ? "null" : "whitespace")}");
            }
        }

        /// <summary>
        /// Asserts that the input <see cref="string"/> instance must be empty.
        /// </summary>
        /// <param name="text">The input <see cref="string"/> instance to test.</param>
        /// <param name="name">The name of the input parameter being tested.</param>
        /// <exception cref="ArgumentException">Thrown if <paramref name="text"/> is empty.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void IsEmpty(string text, string name)
        {
            if (text.Length != 0)
            {
                ThrowArgumentException(name, $"Parameter {name} must be empty, was \"{text}\"");
            }
        }

        /// <summary>
        /// Asserts that the input <see cref="string"/> instance must not be empty.
        /// </summary>
        /// <param name="text">The input <see cref="string"/> instance to test.</param>
        /// <param name="name">The name of the input parameter being tested.</param>
        /// <exception cref="ArgumentException">Thrown if <paramref name="text"/> is empty.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void IsNotEmpty(string text, string name)
        {
            if (text.Length == 0)
            {
                ThrowArgumentException(name, $"Parameter {name} must not be empty, was empty");
            }
        }

        /// <summary>
        /// Asserts that the input <see cref="string"/> instance must be whitespace.
        /// </summary>
        /// <param name="text">The input <see cref="string"/> instance to test.</param>
        /// <param name="name">The name of the input parameter being tested.</param>
        /// <exception cref="ArgumentException">Thrown if <paramref name="text"/> is neither <see langword="null"/> nor whitespace.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void IsWhitespace(string text, string name)
        {
            if (!string.IsNullOrWhiteSpace(text))
            {
                ThrowArgumentException(name, $"Parameter {name} must be null or whitespace, was \"{text}\"");
            }
        }

        /// <summary>
        /// Asserts that the input <see cref="string"/> instance must not be <see langword="null"/> or whitespace.
        /// </summary>
        /// <param name="text">The input <see cref="string"/> instance to test.</param>
        /// <param name="name">The name of the input parameter being tested.</param>
        /// <exception cref="ArgumentException">Thrown if <paramref name="text"/> is <see langword="null"/> or whitespace.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void IsNotWhitespace(string text, string name)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                ThrowArgumentException(name, $"Parameter {name} must not be null or whitespace, was {(text is null ? "null" : "whitespace")}");
            }
        }

        /// <summary>
        /// Asserts that the input <see cref="string"/> instance must have a size of a specified value.
        /// </summary>
        /// <param name="text">The input <see cref="string"/> instance to check the size for.</param>
        /// <param name="size">The target size to test.</param>
        /// <param name="name">The name of the input parameter being tested.</param>
        /// <exception cref="ArgumentException">Thrown if the size of <paramref name="text"/> is != <paramref name="size"/>.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void HasSizeEqualTo(string text, int size, string name)
        {
            HasSizeEqualTo(text.AsSpan(), size, name);
        }

        /// <summary>
        /// Asserts that the input <see cref="string"/> instance must have a size not equal to a specified value.
        /// </summary>
        /// <param name="text">The input <see cref="string"/> instance to check the size for.</param>
        /// <param name="size">The target size to test.</param>
        /// <param name="name">The name of the input parameter being tested.</param>
        /// <exception cref="ArgumentException">Thrown if the size of <paramref name="text"/> is == <paramref name="size"/>.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void HasSizeNotEqualTo(string text, int size, string name)
        {
            HasSizeNotEqualTo(text.AsSpan(), size, name);
        }

        /// <summary>
        /// Asserts that the input <see cref="string"/> instance must have a size of at least specified value.
        /// </summary>
        /// <param name="text">The input <see cref="string"/> instance to check the size for.</param>
        /// <param name="size">The target size to test.</param>
        /// <param name="name">The name of the input parameter being tested.</param>
        /// <exception cref="ArgumentException">Thrown if the size of <paramref name="text"/> is &lt;= <paramref name="size"/>.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void HasSizeAtLeast(string text, int size, string name)
        {
            HasSizeAtLeast(text.AsSpan(), size, name);
        }

        /// <summary>
        /// Asserts that the input <see cref="string"/> instance must have a size of at least or equal to a specified value.
        /// </summary>
        /// <param name="text">The input <see cref="string"/> instance to check the size for.</param>
        /// <param name="size">The target size to test.</param>
        /// <param name="name">The name of the input parameter being tested.</param>
        /// <exception cref="ArgumentException">Thrown if the size of <paramref name="text"/> is &lt; <paramref name="size"/>.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void HasSizeAtLeastOrEqualTo(string text, int size, string name)
        {
            HasSizeAtLeastOrEqualTo(text.AsSpan(), size, name);
        }

        /// <summary>
        /// Asserts that the input <see cref="string"/> instance must have a size of less than a specified value.
        /// </summary>
        /// <param name="text">The input <see cref="string"/> instance to check the size for.</param>
        /// <param name="size">The target size to test.</param>
        /// <param name="name">The name of the input parameter being tested.</param>
        /// <exception cref="ArgumentException">Thrown if the size of <paramref name="text"/> is >= <paramref name="size"/>.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void HasSizeLessThan(string text, int size, string name)
        {
            HasSizeLessThan(text.AsSpan(), size, name);
        }

        /// <summary>
        /// Asserts that the input <see cref="string"/> instance must have a size of less than or equal to a specified value.
        /// </summary>
        /// <param name="text">The input <see cref="string"/> instance to check the size for.</param>
        /// <param name="size">The target size to test.</param>
        /// <param name="name">The name of the input parameter being tested.</param>
        /// <exception cref="ArgumentException">Thrown if the size of <paramref name="text"/> is > <paramref name="size"/>.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void HasSizeLessThanOrEqualTo(string text, int size, string name)
        {
            HasSizeLessThanOrEqualTo(text.AsSpan(), size, name);
        }
    }
}
