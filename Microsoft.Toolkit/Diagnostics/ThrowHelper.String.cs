// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

#nullable enable

namespace Microsoft.Toolkit.Diagnostics
{
    /// <summary>
    /// Helper methods to throw exceptions
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1618", Justification = "Internal helper methods")]
    internal static partial class ThrowHelper
    {
        /// <summary>
        /// Throws an <see cref="ArgumentException"/> when <see cref="Guard.IsNullOrEmpty"/> fails.
        /// </summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void ThrowArgumentExceptionForIsNullOrEmpty(string? text, string name)
        {
            ThrowArgumentException(name, $"Parameter {name.ToAssertString()} (string) must be null or empty, was {text.ToAssertString()}");
        }

        /// <summary>
        /// Throws an <see cref="ArgumentException"/> when <see cref="Guard.IsNotNullOrEmpty"/> fails.
        /// </summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void ThrowArgumentExceptionForIsNotNullOrEmpty(string? text, string name)
        {
            ThrowArgumentException(name, $"Parameter {name.ToAssertString()} (string) must not be null or empty, was {(text is null ? "null" : "empty")}");
        }

        /// <summary>
        /// Throws an <see cref="ArgumentException"/> when <see cref="Guard.IsNullOrWhitespace"/> fails.
        /// </summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void ThrowArgumentExceptionForIsNullOrWhitespace(string? text, string name)
        {
            ThrowArgumentException(name, $"Parameter {name.ToAssertString()} (string) must be null or whitespace, was {text.ToAssertString()}");
        }

        /// <summary>
        /// Throws an <see cref="ArgumentException"/> when <see cref="Guard.IsNotNullOrWhitespace"/> fails.
        /// </summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void ThrowArgumentExceptionForIsNotNullOrWhitespace(string? text, string name)
        {
            ThrowArgumentException(name, $"Parameter {name.ToAssertString()} (string) must not be null or whitespace, was {(text is null ? "null" : "whitespace")}");
        }

        /// <summary>
        /// Throws an <see cref="ArgumentException"/> when <see cref="Guard.IsEmpty"/> fails.
        /// </summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void ThrowArgumentExceptionForIsEmpty(string text, string name)
        {
            ThrowArgumentException(name, $"Parameter {name.ToAssertString()} (string) must be empty, was {text.ToAssertString()}");
        }

        /// <summary>
        /// Throws an <see cref="ArgumentException"/> when <see cref="Guard.IsNotEmpty"/> fails.
        /// </summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void ThrowArgumentExceptionForIsNotEmpty(string text, string name)
        {
            ThrowArgumentException(name, $"Parameter {name.ToAssertString()} (string) must not be empty");
        }

        /// <summary>
        /// Throws an <see cref="ArgumentException"/> when <see cref="Guard.IsWhitespace"/> fails.
        /// </summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void ThrowArgumentExceptionForIsWhitespace(string text, string name)
        {
            ThrowArgumentException(name, $"Parameter {name.ToAssertString()} (string) must be whitespace, was {text.ToAssertString()}");
        }

        /// <summary>
        /// Throws an <see cref="ArgumentException"/> when <see cref="Guard.IsNotWhitespace"/> fails.
        /// </summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void ThrowArgumentExceptionForIsNotWhitespace(string text, string name)
        {
            ThrowArgumentException(name, $"Parameter {name.ToAssertString()} (string) must not be whitespace, was {text.ToAssertString()}");
        }

        /// <summary>
        /// Throws an <see cref="ArgumentException"/> when <see cref="Guard.HasSizeEqualTo"/> fails.
        /// </summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void ThrowArgumentExceptionForHasSizeEqualTo(string text, int size, string name)
        {
            ThrowArgumentException(name, $"Parameter {name.ToAssertString()} (string) must have a size equal to {size}, had a size of {text.Length} and was {text.ToAssertString()}");
        }

        /// <summary>
        /// Throws an <see cref="ArgumentException"/> when <see cref="Guard.HasSizeNotEqualTo"/> fails.
        /// </summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void ThrowArgumentExceptionForHasSizeNotEqualTo(string text, int size, string name)
        {
            ThrowArgumentException(name, $"Parameter {name.ToAssertString()} (string) must not have a size equal to {size}, was {text.ToAssertString()}");
        }

        /// <summary>
        /// Throws an <see cref="ArgumentException"/> when <see cref="Guard.HasSizeOver"/> fails.
        /// </summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void ThrowArgumentExceptionForHasSizeOver(string text, int size, string name)
        {
            ThrowArgumentException(name, $"Parameter {name.ToAssertString()} (string) must have a size over {size}, had a size of {text.Length} and was {text.ToAssertString()}");
        }

        /// <summary>
        /// Throws an <see cref="ArgumentException"/> when <see cref="Guard.HasSizeAtLeast"/> fails.
        /// </summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void ThrowArgumentExceptionForHasSizeAtLeast(string text, int size, string name)
        {
            ThrowArgumentException(name, $"Parameter {name.ToAssertString()} (string) must have a size of at least {size}, had a size of {text.Length} and was {text.ToAssertString()}");
        }

        /// <summary>
        /// Throws an <see cref="ArgumentException"/> when <see cref="Guard.HasSizeLessThan"/> fails.
        /// </summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void ThrowArgumentExceptionForHasSizeLessThan(string text, int size, string name)
        {
            ThrowArgumentException(name, $"Parameter {name.ToAssertString()} (string) must have a size less than {size}, had a size of {text.Length} and was {text.ToAssertString()}");
        }

        /// <summary>
        /// Throws an <see cref="ArgumentException"/> when <see cref="Guard.HasSizeLessThanOrEqualTo"/> fails.
        /// </summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void ThrowArgumentExceptionForHasSizeLessThanOrEqualTo(string text, int size, string name)
        {
            ThrowArgumentException(name, $"Parameter {name.ToAssertString()} (string) must have a size less than or equal to {size}, had a size of {text.Length} and was {text.ToAssertString()}");
        }
    }
}
