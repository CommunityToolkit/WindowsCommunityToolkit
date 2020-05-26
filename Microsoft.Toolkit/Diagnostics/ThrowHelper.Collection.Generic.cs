// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using Microsoft.Toolkit.Extensions;

#nullable enable

namespace Microsoft.Toolkit.Diagnostics
{
    /// <summary>
    /// Helper methods to throw exceptions
    /// </summary>
    internal static partial class ThrowHelper
    {
        /// <summary>
        /// Throws an <see cref="ArgumentException"/> when <see cref="Guard.IsNotEmpty{T}(Span{T},string)"/> fails.
        /// </summary>
        /// <typeparam name="T">The item of items in the input <see cref="Span{T}"/> instance.</typeparam>
        /// <remarks>This method is needed because <see cref="Span{T}"/> can't be used as a generic type parameter.</remarks>
        [MethodImpl(MethodImplOptions.NoInlining)]
        [DoesNotReturn]
        public static void ThrowArgumentExceptionForIsNotEmptyWithSpan<T>(string name)
        {
            ThrowArgumentException(name, $"Parameter {name.ToAssertString()} ({typeof(Span<T>).ToTypeString()}) must not be empty");
        }

        /// <summary>
        /// Throws an <see cref="ArgumentException"/> when <see cref="Guard.IsNotEmpty{T}(ReadOnlySpan{T},string)"/> fails.
        /// </summary>
        /// <typeparam name="T">The item of items in the input <see cref="ReadOnlySpan{T}"/> instance.</typeparam>
        /// <remarks>This method is needed because <see cref="ReadOnlySpan{T}"/> can't be used as a generic type parameter.</remarks>
        [MethodImpl(MethodImplOptions.NoInlining)]
        [DoesNotReturn]
        public static void ThrowArgumentExceptionForIsNotEmptyWithReadOnlySpan<T>(string name)
        {
            ThrowArgumentException(name, $"Parameter {name.ToAssertString()} ({typeof(ReadOnlySpan<T>).ToTypeString()}) must not be empty");
        }

        /// <summary>
        /// Throws an <see cref="ArgumentException"/> when <see cref="Guard.IsNotEmpty{T}(T[],string)"/> (or an overload) fails.
        /// </summary>
        /// <typeparam name="T">The item of items in the input collection.</typeparam>
        [MethodImpl(MethodImplOptions.NoInlining)]
        [DoesNotReturn]
        public static void ThrowArgumentExceptionForIsNotEmpty<T>(string name)
        {
            ThrowArgumentException(name, $"Parameter {name.ToAssertString()} ({typeof(T).ToTypeString()}) must not be empty");
        }
    }
}
