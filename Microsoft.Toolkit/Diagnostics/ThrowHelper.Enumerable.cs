// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.CompilerServices;
using Microsoft.Toolkit.Extensions;

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
        /// Throws an <see cref="ArgumentException"/> when <see cref="Guard.IsEmpty{T}(ICollection{T},string)"/> fails.
        /// </summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void ThrowArgumentExceptionForIsEmpty<T>(ICollection<T> collection, string name)
        {
            ThrowArgumentException(name, $"Parameter \"{name}\" ({collection.GetType().ToTypeString()}) must be empty, had a size of {collection.Count}");
        }

        /// <summary>
        /// Throws an <see cref="ArgumentException"/> when <see cref="Guard.IsEmpty{T}(IReadOnlyCollection{T},string)"/> fails.
        /// </summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void ThrowArgumentExceptionForIsEmpty<T>(IReadOnlyCollection<T> collection, string name)
        {
            ThrowArgumentException(name, $"Parameter \"{name}\" ({collection.GetType().ToTypeString()}) must be empty, had a size of {collection.Count}");
        }

        /// <summary>
        /// Throws an <see cref="ArgumentException"/> when <see cref="Guard.IsEmpty{T}(IEnumerable{T},string)"/> fails.
        /// </summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void ThrowArgumentExceptionForIsEmpty<T>(IEnumerable<T> enumerable, string name)
        {
            ThrowArgumentException(name, $"Parameter \"{name}\" ({enumerable.GetType().ToTypeString()}) must be empty, had a size of {enumerable.Count()}");
        }

        /// <summary>
        /// Throws an <see cref="ArgumentException"/> when <see cref="Guard.IsNotEmpty{T}(ICollection{T},string)"/> fails.
        /// </summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void ThrowArgumentExceptionForIsNotEmpty<T>(ICollection<T> collection, string name)
        {
            ThrowArgumentException(name, $"Parameter \"{name}\" ({collection.GetType().ToTypeString()}) must not be empty");
        }

        /// <summary>
        /// Throws an <see cref="ArgumentException"/> when <see cref="Guard.IsNotEmpty{T}(IReadOnlyCollection{T},string)"/> fails.
        /// </summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void ThrowArgumentExceptionForIsNotEmpty<T>(IReadOnlyCollection<T> collection, string name)
        {
            ThrowArgumentException(name, $"Parameter \"{name}\" ({collection.GetType().ToTypeString()}) must not be empty");
        }

        /// <summary>
        /// Throws an <see cref="ArgumentException"/> when <see cref="Guard.IsNotEmpty{T}(IEnumerable{T},string)"/> fails.
        /// </summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void ThrowArgumentExceptionForIsNotEmpty<T>(IEnumerable<T> enumerable, string name)
        {
            ThrowArgumentException(name, $"Parameter \"{name}\" ({enumerable.GetType().ToTypeString()}) must not be empty");
        }

        /// <summary>
        /// Throws an <see cref="ArgumentException"/> when <see cref="Guard.HasSizeEqualTo{T}(ICollection{T},int,string)"/> fails.
        /// </summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void ThrowArgumentExceptionForHasSizeEqualTo<T>(ICollection<T> collection, int size, string name)
        {
            ThrowArgumentException(name, $"Parameter \"{name}\" ({collection.GetType().ToTypeString()}) must be sized == {size}, had a size of {collection.Count}");
        }

        /// <summary>
        /// Throws an <see cref="ArgumentException"/> when <see cref="Guard.HasSizeEqualTo{T}(IReadOnlyCollection{T},int,string)"/> fails.
        /// </summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void ThrowArgumentExceptionForHasSizeEqualTo<T>(IReadOnlyCollection<T> collection, int size, string name)
        {
            ThrowArgumentException(name, $"Parameter \"{name}\" ({collection.GetType().ToTypeString()}) must be sized == {size}, had a size of {collection.Count}");
        }

        /// <summary>
        /// Throws an <see cref="ArgumentException"/> when <see cref="Guard.HasSizeEqualTo{T}(IEnumerable{T},int,string)"/> fails.
        /// </summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void ThrowArgumentExceptionForHasSizeEqualTo<T>(IEnumerable<T> enumerable, int size, string name)
        {
            ThrowArgumentException(name, $"Parameter \"{name}\" ({enumerable.GetType().ToTypeString()}) must be sized == {size}, had a size of {enumerable.Count()}");
        }

        /// <summary>
        /// Throws an <see cref="ArgumentException"/> when <see cref="Guard.HasSizeNotEqualTo{T}(ICollection{T},int,string)"/> fails.
        /// </summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void ThrowArgumentExceptionForHasSizeNotEqualTo<T>(ICollection<T> collection, int size, string name)
        {
            ThrowArgumentException(name, $"Parameter \"{name}\" ({collection.GetType().ToTypeString()}) must be sized != {size}");
        }

        /// <summary>
        /// Throws an <see cref="ArgumentException"/> when <see cref="Guard.HasSizeNotEqualTo{T}(IReadOnlyCollection{T},int,string)"/> fails.
        /// </summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void ThrowArgumentExceptionForHasSizeNotEqualTo<T>(IReadOnlyCollection<T> collection, int size, string name)
        {
            ThrowArgumentException(name, $"Parameter \"{name}\" ({collection.GetType().ToTypeString()}) must be sized != {size}");
        }

        /// <summary>
        /// Throws an <see cref="ArgumentException"/> when <see cref="Guard.HasSizeNotEqualTo{T}(IEnumerable{T},int,string)"/> fails.
        /// </summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void ThrowArgumentExceptionForHasSizeNotEqualTo<T>(IEnumerable<T> enumerable, int size, string name)
        {
            ThrowArgumentException(name, $"Parameter \"{name}\" ({enumerable.GetType().ToTypeString()}) must be sized != {size}");
        }

        /// <summary>
        /// Throws an <see cref="ArgumentException"/> when <see cref="Guard.HasSizeOver{T}(ICollection{T},int,string)"/> fails.
        /// </summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void ThrowArgumentExceptionForHasSizeOver<T>(ICollection<T> collection, int size, string name)
        {
            ThrowArgumentException(name, $"Parameter \"{name}\" ({collection.GetType().ToTypeString()}) must be sized > {size}, had a size of {collection.Count}");
        }

        /// <summary>
        /// Throws an <see cref="ArgumentException"/> when <see cref="Guard.HasSizeOver{T}(IReadOnlyCollection{T},int,string)"/> fails.
        /// </summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void ThrowArgumentExceptionForHasSizeOver<T>(IReadOnlyCollection<T> collection, int size, string name)
        {
            ThrowArgumentException(name, $"Parameter \"{name}\" ({collection.GetType().ToTypeString()}) must be sized > {size}, had a size of {collection.Count}");
        }

        /// <summary>
        /// Throws an <see cref="ArgumentException"/> when <see cref="Guard.HasSizeOver{T}(IEnumerable{T},int,string)"/> fails.
        /// </summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void ThrowArgumentExceptionForHasSizeOver<T>(IEnumerable<T> enumerable, int size, string name)
        {
            ThrowArgumentException(name, $"Parameter \"{name}\" ({enumerable.GetType().ToTypeString()}) must be sized > {size}, had a size of {enumerable.Count()}");
        }

        /// <summary>
        /// Throws an <see cref="ArgumentException"/> when <see cref="Guard.HasSizeAtLeast{T}(ICollection{T},int,string)"/> fails.
        /// </summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void ThrowArgumentExceptionForHasSizeAtLeast<T>(ICollection<T> collection, int size, string name)
        {
            ThrowArgumentException(name, $"Parameter \"{name}\" ({collection.GetType().ToTypeString()}) must be sized >= {size}, had a size of {collection.Count}");
        }

        /// <summary>
        /// Throws an <see cref="ArgumentException"/> when <see cref="Guard.HasSizeAtLeast{T}(IReadOnlyCollection{T},int,string)"/> fails.
        /// </summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void ThrowArgumentExceptionForHasSizeAtLeast<T>(IReadOnlyCollection<T> collection, int size, string name)
        {
            ThrowArgumentException(name, $"Parameter \"{name}\" ({collection.GetType().ToTypeString()}) must be sized >= {size}, had a size of {collection.Count}");
        }

        /// <summary>
        /// Throws an <see cref="ArgumentException"/> when <see cref="Guard.HasSizeAtLeast{T}(IEnumerable{T},int,string)"/> fails.
        /// </summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void ThrowArgumentExceptionForHasSizeAtLeast<T>(IEnumerable<T> enumerable, int size, string name)
        {
            ThrowArgumentException(name, $"Parameter \"{name}\" ({enumerable.GetType().ToTypeString()}) must be sized >= {size}, had a size of {enumerable.Count()}");
        }

        /// <summary>
        /// Throws an <see cref="ArgumentException"/> when <see cref="Guard.HasSizeLessThan{T}(ICollection{T},int,string)"/> fails.
        /// </summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void ThrowArgumentExceptionForHasSizeLessThan<T>(ICollection<T> collection, int size, string name)
        {
            ThrowArgumentException(name, $"Parameter \"{name}\" ({collection.GetType().ToTypeString()}) must be sized < {size}, had a size of {collection.Count}");
        }

        /// <summary>
        /// Throws an <see cref="ArgumentException"/> when <see cref="Guard.HasSizeLessThan{T}(IReadOnlyCollection{T},int,string)"/> fails.
        /// </summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void ThrowArgumentExceptionForHasSizeLessThan<T>(IReadOnlyCollection<T> collection, int size, string name)
        {
            ThrowArgumentException(name, $"Parameter \"{name}\" ({collection.GetType().ToTypeString()}) must be sized < {size}, had a size of {collection.Count}");
        }

        /// <summary>
        /// Throws an <see cref="ArgumentException"/> when <see cref="Guard.HasSizeLessThan{T}(IEnumerable{T},int,string)"/> fails.
        /// </summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void ThrowArgumentExceptionForHasSizeLessThan<T>(IEnumerable<T> enumerable, int size, string name)
        {
            ThrowArgumentException(name, $"Parameter \"{name}\" ({enumerable.GetType().ToTypeString()}) must be sized < {size}, had a size of {enumerable.Count()}");
        }

        /// <summary>
        /// Throws an <see cref="ArgumentException"/> when <see cref="Guard.HasSizeLessThanOrEqualTo{T}(ICollection{T},int,string)"/> fails.
        /// </summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void ThrowArgumentExceptionForHasSizeLessThanOrEqualTo<T>(ICollection<T> collection, int size, string name)
        {
            ThrowArgumentException(name, $"Parameter \"{name}\" ({collection.GetType().ToTypeString()}) must be sized <= {size}, had a size of {collection.Count}");
        }

        /// <summary>
        /// Throws an <see cref="ArgumentException"/> when <see cref="Guard.HasSizeLessThanOrEqualTo{T}(IReadOnlyCollection{T},int,string)"/> fails.
        /// </summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void ThrowArgumentExceptionForHasSizeLessThanOrEqualTo<T>(IReadOnlyCollection<T> collection, int size, string name)
        {
            ThrowArgumentException(name, $"Parameter \"{name}\" ({collection.GetType().ToTypeString()}) must be sized <= {size}, had a size of {collection.Count}");
        }

        /// <summary>
        /// Throws an <see cref="ArgumentException"/> when <see cref="Guard.HasSizeLessThanOrEqualTo{T}(IEnumerable{T},int,string)"/> fails.
        /// </summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void ThrowArgumentExceptionForHasSizeLessThanOrEqualTo<T>(IEnumerable<T> enumerable, int size, string name)
        {
            ThrowArgumentException(name, $"Parameter \"{name}\" ({enumerable.GetType().ToTypeString()}) must be sized <= {size}, had a size of {enumerable.Count()}");
        }
    }
}
