// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

/* ========================
 * Auto generated file
 * ===================== */

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
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
        /// Throws an <see cref="ArgumentException"/> when <see cref="Guard.IsEmpty{T}(T[],string)"/> (or an overload) fails.
        /// </summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void ThrowArgumentExceptionForIsEmpty<T>(Span<T> span, string name)
        {
            ThrowArgumentException(name, $"Parameter \"{name}\" ({typeof(Span<T>).ToTypeString()}) must be empty, had a size of {span.Length}");
        }

        /// <summary>
        /// Throws an <see cref="ArgumentException"/> when <see cref="Guard.HasSizeEqualTo{T}(T[],int,string)"/> (or an overload) fails.
        /// </summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void ThrowArgumentExceptionForHasSizeEqualTo<T>(Span<T> span, int size, string name)
        {
            ThrowArgumentException(name, $"Parameter \"{name}\" ({typeof(Span<T>).ToTypeString()}) must be sized == {size}, had a size of {span.Length}");
        }

        /// <summary>
        /// Throws an <see cref="ArgumentException"/> when <see cref="Guard.HasSizeNotEqualTo{T}(T[],int,string)"/> (or an overload) fails.
        /// </summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void ThrowArgumentExceptionForHasSizeNotEqualTo<T>(Span<T> span, int size, string name)
        {
            ThrowArgumentException(name, $"Parameter \"{name}\" ({typeof(Span<T>).ToTypeString()}) must be sized != {size}, had a size of {span.Length}");
        }

        /// <summary>
        /// Throws an <see cref="ArgumentException"/> when <see cref="Guard.HasSizeOver{T}(T[],int,string)"/> (or an overload) fails.
        /// </summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void ThrowArgumentExceptionForHasSizeOver<T>(Span<T> span, int size, string name)
        {
            ThrowArgumentException(name, $"Parameter \"{name}\" ({typeof(Span<T>).ToTypeString()}) must be sized > {size}, had a size of {span.Length}");
        }

        /// <summary>
        /// Throws an <see cref="ArgumentException"/> when <see cref="Guard.HasSizeAtLeast{T}(T[],int,string)"/> (or an overload) fails.
        /// </summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void ThrowArgumentExceptionForHasSizeAtLeast<T>(Span<T> span, int size, string name)
        {
            ThrowArgumentException(name, $"Parameter \"{name}\" ({typeof(Span<T>).ToTypeString()}) must be sized >= {size}, had a size of {span.Length}");
        }

        /// <summary>
        /// Throws an <see cref="ArgumentException"/> when <see cref="Guard.HasSizeLessThan{T}(T[],int,string)"/> (or an overload) fails.
        /// </summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void ThrowArgumentExceptionForHasSizeLessThan<T>(Span<T> span, int size, string name)
        {
            ThrowArgumentException(name, $"Parameter \"{name}\" ({typeof(Span<T>).ToTypeString()}) must be sized < {size}, had a size of {span.Length}");
        }

        /// <summary>
        /// Throws an <see cref="ArgumentException"/> when <see cref="Guard.HasSizeLessThanOrEqualTo{T}(T[],int,string)"/> (or an overload) fails.
        /// </summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void ThrowArgumentExceptionForHasSizeLessThanOrEqualTo<T>(Span<T> span, int size, string name)
        {
            ThrowArgumentException(name, $"Parameter \"{name}\" ({typeof(Span<T>).ToTypeString()}) must be sized <= {size}, had a size of {span.Length}");
        }

        /// <summary>
        /// Throws an <see cref="ArgumentException"/> when <see cref="Guard.HasSizeEqualTo{T}(T[],T[],string)"/> (or an overload) fails.
        /// </summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void ThrowArgumentExceptionForHasSizeEqualTo<T>(Span<T> source, Span<T> destination, string name)
        {
            ThrowArgumentException(name, $"The source \"{name}\" ({typeof(Span<T>).ToTypeString()}) must be sized == {destination.Length}, had a size of {source.Length}");
        }

        /// <summary>
        /// Throws an <see cref="ArgumentException"/> when <see cref="Guard.HasSizeLessThanOrEqualTo{T}(T[],T[],string)"/> (or an overload) fails.
        /// </summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void ThrowArgumentExceptionForHasSizeLessThanOrEqualTo<T>(Span<T> source, Span<T> destination, string name)
        {
            ThrowArgumentException(name, $"The source \"{name}\" ({typeof(Span<T>).ToTypeString()}) must be sized <= {destination.Length}, had a size of {source.Length}");
        }

        /// <summary>
        /// Throws an <see cref="ArgumentOutOfRangeException"/> when <see cref="Guard.IsInRangeFor{T}(int,T[],string)"/> (or an overload) fails.
        /// </summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void ThrowArgumentOutOfRangeExceptionForIsInRangeFor<T>(int index, Span<T> span, string name)
        {
            ThrowArgumentOutOfRangeException(name, $"Parameter \"{name}\" (int) must be >= 0 and < {span.Length} to be a valid index for the target collection ({typeof(Span<T>).ToTypeString()}), was {index}");
        }

        /// <summary>
        /// Throws an <see cref="ArgumentException"/> when <see cref="Guard.IsEmpty{T}(T[],string)"/> (or an overload) fails.
        /// </summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void ThrowArgumentExceptionForIsEmpty<T>(ReadOnlySpan<T> span, string name)
        {
            ThrowArgumentException(name, $"Parameter \"{name}\" ({typeof(ReadOnlySpan<T>).ToTypeString()}) must be empty, had a size of {span.Length}");
        }

        /// <summary>
        /// Throws an <see cref="ArgumentException"/> when <see cref="Guard.HasSizeEqualTo{T}(T[],int,string)"/> (or an overload) fails.
        /// </summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void ThrowArgumentExceptionForHasSizeEqualTo<T>(ReadOnlySpan<T> span, int size, string name)
        {
            ThrowArgumentException(name, $"Parameter \"{name}\" ({typeof(ReadOnlySpan<T>).ToTypeString()}) must be sized == {size}, had a size of {span.Length}");
        }

        /// <summary>
        /// Throws an <see cref="ArgumentException"/> when <see cref="Guard.HasSizeNotEqualTo{T}(T[],int,string)"/> (or an overload) fails.
        /// </summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void ThrowArgumentExceptionForHasSizeNotEqualTo<T>(ReadOnlySpan<T> span, int size, string name)
        {
            ThrowArgumentException(name, $"Parameter \"{name}\" ({typeof(ReadOnlySpan<T>).ToTypeString()}) must be sized != {size}, had a size of {span.Length}");
        }

        /// <summary>
        /// Throws an <see cref="ArgumentException"/> when <see cref="Guard.HasSizeOver{T}(T[],int,string)"/> (or an overload) fails.
        /// </summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void ThrowArgumentExceptionForHasSizeOver<T>(ReadOnlySpan<T> span, int size, string name)
        {
            ThrowArgumentException(name, $"Parameter \"{name}\" ({typeof(ReadOnlySpan<T>).ToTypeString()}) must be sized > {size}, had a size of {span.Length}");
        }

        /// <summary>
        /// Throws an <see cref="ArgumentException"/> when <see cref="Guard.HasSizeAtLeast{T}(T[],int,string)"/> (or an overload) fails.
        /// </summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void ThrowArgumentExceptionForHasSizeAtLeast<T>(ReadOnlySpan<T> span, int size, string name)
        {
            ThrowArgumentException(name, $"Parameter \"{name}\" ({typeof(ReadOnlySpan<T>).ToTypeString()}) must be sized >= {size}, had a size of {span.Length}");
        }

        /// <summary>
        /// Throws an <see cref="ArgumentException"/> when <see cref="Guard.HasSizeLessThan{T}(T[],int,string)"/> (or an overload) fails.
        /// </summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void ThrowArgumentExceptionForHasSizeLessThan<T>(ReadOnlySpan<T> span, int size, string name)
        {
            ThrowArgumentException(name, $"Parameter \"{name}\" ({typeof(ReadOnlySpan<T>).ToTypeString()}) must be sized < {size}, had a size of {span.Length}");
        }

        /// <summary>
        /// Throws an <see cref="ArgumentException"/> when <see cref="Guard.HasSizeLessThanOrEqualTo{T}(T[],int,string)"/> (or an overload) fails.
        /// </summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void ThrowArgumentExceptionForHasSizeLessThanOrEqualTo<T>(ReadOnlySpan<T> span, int size, string name)
        {
            ThrowArgumentException(name, $"Parameter \"{name}\" ({typeof(ReadOnlySpan<T>).ToTypeString()}) must be sized <= {size}, had a size of {span.Length}");
        }

        /// <summary>
        /// Throws an <see cref="ArgumentException"/> when <see cref="Guard.HasSizeEqualTo{T}(T[],T[],string)"/> (or an overload) fails.
        /// </summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void ThrowArgumentExceptionForHasSizeEqualTo<T>(ReadOnlySpan<T> source, Span<T> destination, string name)
        {
            ThrowArgumentException(name, $"The source \"{name}\" ({typeof(ReadOnlySpan<T>).ToTypeString()}) must be sized == {destination.Length}, had a size of {source.Length}");
        }

        /// <summary>
        /// Throws an <see cref="ArgumentException"/> when <see cref="Guard.HasSizeLessThanOrEqualTo{T}(T[],T[],string)"/> (or an overload) fails.
        /// </summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void ThrowArgumentExceptionForHasSizeLessThanOrEqualTo<T>(ReadOnlySpan<T> source, Span<T> destination, string name)
        {
            ThrowArgumentException(name, $"The source \"{name}\" ({typeof(ReadOnlySpan<T>).ToTypeString()}) must be sized <= {destination.Length}, had a size of {source.Length}");
        }

        /// <summary>
        /// Throws an <see cref="ArgumentOutOfRangeException"/> when <see cref="Guard.IsInRangeFor{T}(int,T[],string)"/> (or an overload) fails.
        /// </summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void ThrowArgumentOutOfRangeExceptionForIsInRangeFor<T>(int index, ReadOnlySpan<T> span, string name)
        {
            ThrowArgumentOutOfRangeException(name, $"Parameter \"{name}\" (int) must be >= 0 and < {span.Length} to be a valid index for the target collection ({typeof(ReadOnlySpan<T>).ToTypeString()}), was {index}");
        }

        /// <summary>
        /// Throws an <see cref="ArgumentException"/> when <see cref="Guard.IsEmpty{T}(T[],string)"/> (or an overload) fails.
        /// </summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void ThrowArgumentExceptionForIsEmpty<T>(Memory<T> memory, string name)
        {
            ThrowArgumentException(name, $"Parameter \"{name}\" ({typeof(Memory<T>).ToTypeString()}) must be empty, had a size of {memory.Length}");
        }

        /// <summary>
        /// Throws an <see cref="ArgumentException"/> when <see cref="Guard.HasSizeEqualTo{T}(T[],int,string)"/> (or an overload) fails.
        /// </summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void ThrowArgumentExceptionForHasSizeEqualTo<T>(Memory<T> memory, int size, string name)
        {
            ThrowArgumentException(name, $"Parameter \"{name}\" ({typeof(Memory<T>).ToTypeString()}) must be sized == {size}, had a size of {memory.Length}");
        }

        /// <summary>
        /// Throws an <see cref="ArgumentException"/> when <see cref="Guard.HasSizeNotEqualTo{T}(T[],int,string)"/> (or an overload) fails.
        /// </summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void ThrowArgumentExceptionForHasSizeNotEqualTo<T>(Memory<T> memory, int size, string name)
        {
            ThrowArgumentException(name, $"Parameter \"{name}\" ({typeof(Memory<T>).ToTypeString()}) must be sized != {size}, had a size of {memory.Length}");
        }

        /// <summary>
        /// Throws an <see cref="ArgumentException"/> when <see cref="Guard.HasSizeOver{T}(T[],int,string)"/> (or an overload) fails.
        /// </summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void ThrowArgumentExceptionForHasSizeOver<T>(Memory<T> memory, int size, string name)
        {
            ThrowArgumentException(name, $"Parameter \"{name}\" ({typeof(Memory<T>).ToTypeString()}) must be sized > {size}, had a size of {memory.Length}");
        }

        /// <summary>
        /// Throws an <see cref="ArgumentException"/> when <see cref="Guard.HasSizeAtLeast{T}(T[],int,string)"/> (or an overload) fails.
        /// </summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void ThrowArgumentExceptionForHasSizeAtLeast<T>(Memory<T> memory, int size, string name)
        {
            ThrowArgumentException(name, $"Parameter \"{name}\" ({typeof(Memory<T>).ToTypeString()}) must be sized >= {size}, had a size of {memory.Length}");
        }

        /// <summary>
        /// Throws an <see cref="ArgumentException"/> when <see cref="Guard.HasSizeLessThan{T}(T[],int,string)"/> (or an overload) fails.
        /// </summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void ThrowArgumentExceptionForHasSizeLessThan<T>(Memory<T> memory, int size, string name)
        {
            ThrowArgumentException(name, $"Parameter \"{name}\" ({typeof(Memory<T>).ToTypeString()}) must be sized < {size}, had a size of {memory.Length}");
        }

        /// <summary>
        /// Throws an <see cref="ArgumentException"/> when <see cref="Guard.HasSizeLessThanOrEqualTo{T}(T[],int,string)"/> (or an overload) fails.
        /// </summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void ThrowArgumentExceptionForHasSizeLessThanOrEqualTo<T>(Memory<T> memory, int size, string name)
        {
            ThrowArgumentException(name, $"Parameter \"{name}\" ({typeof(Memory<T>).ToTypeString()}) must be sized <= {size}, had a size of {memory.Length}");
        }

        /// <summary>
        /// Throws an <see cref="ArgumentException"/> when <see cref="Guard.HasSizeEqualTo{T}(T[],T[],string)"/> (or an overload) fails.
        /// </summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void ThrowArgumentExceptionForHasSizeEqualTo<T>(Memory<T> source, Memory<T> destination, string name)
        {
            ThrowArgumentException(name, $"The source \"{name}\" ({typeof(Memory<T>).ToTypeString()}) must be sized == {destination.Length}, had a size of {source.Length}");
        }

        /// <summary>
        /// Throws an <see cref="ArgumentException"/> when <see cref="Guard.HasSizeLessThanOrEqualTo{T}(T[],T[],string)"/> (or an overload) fails.
        /// </summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void ThrowArgumentExceptionForHasSizeLessThanOrEqualTo<T>(Memory<T> source, Memory<T> destination, string name)
        {
            ThrowArgumentException(name, $"The source \"{name}\" ({typeof(Memory<T>).ToTypeString()}) must be sized <= {destination.Length}, had a size of {source.Length}");
        }

        /// <summary>
        /// Throws an <see cref="ArgumentOutOfRangeException"/> when <see cref="Guard.IsInRangeFor{T}(int,T[],string)"/> (or an overload) fails.
        /// </summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void ThrowArgumentOutOfRangeExceptionForIsInRangeFor<T>(int index, Memory<T> memory, string name)
        {
            ThrowArgumentOutOfRangeException(name, $"Parameter \"{name}\" (int) must be >= 0 and < {memory.Length} to be a valid index for the target collection ({typeof(Memory<T>).ToTypeString()}), was {index}");
        }

        /// <summary>
        /// Throws an <see cref="ArgumentException"/> when <see cref="Guard.IsEmpty{T}(T[],string)"/> (or an overload) fails.
        /// </summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void ThrowArgumentExceptionForIsEmpty<T>(ReadOnlyMemory<T> memory, string name)
        {
            ThrowArgumentException(name, $"Parameter \"{name}\" ({typeof(ReadOnlyMemory<T>).ToTypeString()}) must be empty, had a size of {memory.Length}");
        }

        /// <summary>
        /// Throws an <see cref="ArgumentException"/> when <see cref="Guard.HasSizeEqualTo{T}(T[],int,string)"/> (or an overload) fails.
        /// </summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void ThrowArgumentExceptionForHasSizeEqualTo<T>(ReadOnlyMemory<T> memory, int size, string name)
        {
            ThrowArgumentException(name, $"Parameter \"{name}\" ({typeof(ReadOnlyMemory<T>).ToTypeString()}) must be sized == {size}, had a size of {memory.Length}");
        }

        /// <summary>
        /// Throws an <see cref="ArgumentException"/> when <see cref="Guard.HasSizeNotEqualTo{T}(T[],int,string)"/> (or an overload) fails.
        /// </summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void ThrowArgumentExceptionForHasSizeNotEqualTo<T>(ReadOnlyMemory<T> memory, int size, string name)
        {
            ThrowArgumentException(name, $"Parameter \"{name}\" ({typeof(ReadOnlyMemory<T>).ToTypeString()}) must be sized != {size}, had a size of {memory.Length}");
        }

        /// <summary>
        /// Throws an <see cref="ArgumentException"/> when <see cref="Guard.HasSizeOver{T}(T[],int,string)"/> (or an overload) fails.
        /// </summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void ThrowArgumentExceptionForHasSizeOver<T>(ReadOnlyMemory<T> memory, int size, string name)
        {
            ThrowArgumentException(name, $"Parameter \"{name}\" ({typeof(ReadOnlyMemory<T>).ToTypeString()}) must be sized > {size}, had a size of {memory.Length}");
        }

        /// <summary>
        /// Throws an <see cref="ArgumentException"/> when <see cref="Guard.HasSizeAtLeast{T}(T[],int,string)"/> (or an overload) fails.
        /// </summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void ThrowArgumentExceptionForHasSizeAtLeast<T>(ReadOnlyMemory<T> memory, int size, string name)
        {
            ThrowArgumentException(name, $"Parameter \"{name}\" ({typeof(ReadOnlyMemory<T>).ToTypeString()}) must be sized >= {size}, had a size of {memory.Length}");
        }

        /// <summary>
        /// Throws an <see cref="ArgumentException"/> when <see cref="Guard.HasSizeLessThan{T}(T[],int,string)"/> (or an overload) fails.
        /// </summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void ThrowArgumentExceptionForHasSizeLessThan<T>(ReadOnlyMemory<T> memory, int size, string name)
        {
            ThrowArgumentException(name, $"Parameter \"{name}\" ({typeof(ReadOnlyMemory<T>).ToTypeString()}) must be sized < {size}, had a size of {memory.Length}");
        }

        /// <summary>
        /// Throws an <see cref="ArgumentException"/> when <see cref="Guard.HasSizeLessThanOrEqualTo{T}(T[],int,string)"/> (or an overload) fails.
        /// </summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void ThrowArgumentExceptionForHasSizeLessThanOrEqualTo<T>(ReadOnlyMemory<T> memory, int size, string name)
        {
            ThrowArgumentException(name, $"Parameter \"{name}\" ({typeof(ReadOnlyMemory<T>).ToTypeString()}) must be sized <= {size}, had a size of {memory.Length}");
        }

        /// <summary>
        /// Throws an <see cref="ArgumentException"/> when <see cref="Guard.HasSizeEqualTo{T}(T[],T[],string)"/> (or an overload) fails.
        /// </summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void ThrowArgumentExceptionForHasSizeEqualTo<T>(ReadOnlyMemory<T> source, Memory<T> destination, string name)
        {
            ThrowArgumentException(name, $"The source \"{name}\" ({typeof(ReadOnlyMemory<T>).ToTypeString()}) must be sized == {destination.Length}, had a size of {source.Length}");
        }

        /// <summary>
        /// Throws an <see cref="ArgumentException"/> when <see cref="Guard.HasSizeLessThanOrEqualTo{T}(T[],T[],string)"/> (or an overload) fails.
        /// </summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void ThrowArgumentExceptionForHasSizeLessThanOrEqualTo<T>(ReadOnlyMemory<T> source, Memory<T> destination, string name)
        {
            ThrowArgumentException(name, $"The source \"{name}\" ({typeof(ReadOnlyMemory<T>).ToTypeString()}) must be sized <= {destination.Length}, had a size of {source.Length}");
        }

        /// <summary>
        /// Throws an <see cref="ArgumentOutOfRangeException"/> when <see cref="Guard.IsInRangeFor{T}(int,T[],string)"/> (or an overload) fails.
        /// </summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void ThrowArgumentOutOfRangeExceptionForIsInRangeFor<T>(int index, ReadOnlyMemory<T> memory, string name)
        {
            ThrowArgumentOutOfRangeException(name, $"Parameter \"{name}\" (int) must be >= 0 and < {memory.Length} to be a valid index for the target collection ({typeof(ReadOnlyMemory<T>).ToTypeString()}), was {index}");
        }

        /// <summary>
        /// Throws an <see cref="ArgumentException"/> when <see cref="Guard.IsEmpty{T}(T[],string)"/> (or an overload) fails.
        /// </summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void ThrowArgumentExceptionForIsEmpty<T>(T[] array, string name)
        {
            ThrowArgumentException(name, $"Parameter \"{name}\" ({typeof(T[]).ToTypeString()}) must be empty, had a size of {array.Length}");
        }

        /// <summary>
        /// Throws an <see cref="ArgumentException"/> when <see cref="Guard.HasSizeEqualTo{T}(T[],int,string)"/> (or an overload) fails.
        /// </summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void ThrowArgumentExceptionForHasSizeEqualTo<T>(T[] array, int size, string name)
        {
            ThrowArgumentException(name, $"Parameter \"{name}\" ({typeof(T[]).ToTypeString()}) must be sized == {size}, had a size of {array.Length}");
        }

        /// <summary>
        /// Throws an <see cref="ArgumentException"/> when <see cref="Guard.HasSizeNotEqualTo{T}(T[],int,string)"/> (or an overload) fails.
        /// </summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void ThrowArgumentExceptionForHasSizeNotEqualTo<T>(T[] array, int size, string name)
        {
            ThrowArgumentException(name, $"Parameter \"{name}\" ({typeof(T[]).ToTypeString()}) must be sized != {size}, had a size of {array.Length}");
        }

        /// <summary>
        /// Throws an <see cref="ArgumentException"/> when <see cref="Guard.HasSizeOver{T}(T[],int,string)"/> (or an overload) fails.
        /// </summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void ThrowArgumentExceptionForHasSizeOver<T>(T[] array, int size, string name)
        {
            ThrowArgumentException(name, $"Parameter \"{name}\" ({typeof(T[]).ToTypeString()}) must be sized > {size}, had a size of {array.Length}");
        }

        /// <summary>
        /// Throws an <see cref="ArgumentException"/> when <see cref="Guard.HasSizeAtLeast{T}(T[],int,string)"/> (or an overload) fails.
        /// </summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void ThrowArgumentExceptionForHasSizeAtLeast<T>(T[] array, int size, string name)
        {
            ThrowArgumentException(name, $"Parameter \"{name}\" ({typeof(T[]).ToTypeString()}) must be sized >= {size}, had a size of {array.Length}");
        }

        /// <summary>
        /// Throws an <see cref="ArgumentException"/> when <see cref="Guard.HasSizeLessThan{T}(T[],int,string)"/> (or an overload) fails.
        /// </summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void ThrowArgumentExceptionForHasSizeLessThan<T>(T[] array, int size, string name)
        {
            ThrowArgumentException(name, $"Parameter \"{name}\" ({typeof(T[]).ToTypeString()}) must be sized < {size}, had a size of {array.Length}");
        }

        /// <summary>
        /// Throws an <see cref="ArgumentException"/> when <see cref="Guard.HasSizeLessThanOrEqualTo{T}(T[],int,string)"/> (or an overload) fails.
        /// </summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void ThrowArgumentExceptionForHasSizeLessThanOrEqualTo<T>(T[] array, int size, string name)
        {
            ThrowArgumentException(name, $"Parameter \"{name}\" ({typeof(T[]).ToTypeString()}) must be sized <= {size}, had a size of {array.Length}");
        }

        /// <summary>
        /// Throws an <see cref="ArgumentException"/> when <see cref="Guard.HasSizeEqualTo{T}(T[],T[],string)"/> (or an overload) fails.
        /// </summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void ThrowArgumentExceptionForHasSizeEqualTo<T>(T[] source, T[] destination, string name)
        {
            ThrowArgumentException(name, $"The source \"{name}\" ({typeof(T[]).ToTypeString()}) must be sized == {destination.Length}, had a size of {source.Length}");
        }

        /// <summary>
        /// Throws an <see cref="ArgumentException"/> when <see cref="Guard.HasSizeLessThanOrEqualTo{T}(T[],T[],string)"/> (or an overload) fails.
        /// </summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void ThrowArgumentExceptionForHasSizeLessThanOrEqualTo<T>(T[] source, T[] destination, string name)
        {
            ThrowArgumentException(name, $"The source \"{name}\" ({typeof(T[]).ToTypeString()}) must be sized <= {destination.Length}, had a size of {source.Length}");
        }

        /// <summary>
        /// Throws an <see cref="ArgumentOutOfRangeException"/> when <see cref="Guard.IsInRangeFor{T}(int,T[],string)"/> (or an overload) fails.
        /// </summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void ThrowArgumentOutOfRangeExceptionForIsInRangeFor<T>(int index, T[] array, string name)
        {
            ThrowArgumentOutOfRangeException(name, $"Parameter \"{name}\" (int) must be >= 0 and < {array.Length} to be a valid index for the target collection ({typeof(T[]).ToTypeString()}), was {index}");
        }

        /// <summary>
        /// Throws an <see cref="ArgumentException"/> when <see cref="Guard.IsEmpty{T}(T[],string)"/> (or an overload) fails.
        /// </summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void ThrowArgumentExceptionForIsEmpty<T>(List<T> list, string name)
        {
            ThrowArgumentException(name, $"Parameter \"{name}\" ({typeof(List<T>).ToTypeString()}) must be empty, had a size of {list.Count}");
        }

        /// <summary>
        /// Throws an <see cref="ArgumentException"/> when <see cref="Guard.HasSizeEqualTo{T}(T[],int,string)"/> (or an overload) fails.
        /// </summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void ThrowArgumentExceptionForHasSizeEqualTo<T>(List<T> list, int size, string name)
        {
            ThrowArgumentException(name, $"Parameter \"{name}\" ({typeof(List<T>).ToTypeString()}) must be sized == {size}, had a size of {list.Count}");
        }

        /// <summary>
        /// Throws an <see cref="ArgumentException"/> when <see cref="Guard.HasSizeNotEqualTo{T}(T[],int,string)"/> (or an overload) fails.
        /// </summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void ThrowArgumentExceptionForHasSizeNotEqualTo<T>(List<T> list, int size, string name)
        {
            ThrowArgumentException(name, $"Parameter \"{name}\" ({typeof(List<T>).ToTypeString()}) must be sized != {size}, had a size of {list.Count}");
        }

        /// <summary>
        /// Throws an <see cref="ArgumentException"/> when <see cref="Guard.HasSizeOver{T}(T[],int,string)"/> (or an overload) fails.
        /// </summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void ThrowArgumentExceptionForHasSizeOver<T>(List<T> list, int size, string name)
        {
            ThrowArgumentException(name, $"Parameter \"{name}\" ({typeof(List<T>).ToTypeString()}) must be sized > {size}, had a size of {list.Count}");
        }

        /// <summary>
        /// Throws an <see cref="ArgumentException"/> when <see cref="Guard.HasSizeAtLeast{T}(T[],int,string)"/> (or an overload) fails.
        /// </summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void ThrowArgumentExceptionForHasSizeAtLeast<T>(List<T> list, int size, string name)
        {
            ThrowArgumentException(name, $"Parameter \"{name}\" ({typeof(List<T>).ToTypeString()}) must be sized >= {size}, had a size of {list.Count}");
        }

        /// <summary>
        /// Throws an <see cref="ArgumentException"/> when <see cref="Guard.HasSizeLessThan{T}(T[],int,string)"/> (or an overload) fails.
        /// </summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void ThrowArgumentExceptionForHasSizeLessThan<T>(List<T> list, int size, string name)
        {
            ThrowArgumentException(name, $"Parameter \"{name}\" ({typeof(List<T>).ToTypeString()}) must be sized < {size}, had a size of {list.Count}");
        }

        /// <summary>
        /// Throws an <see cref="ArgumentException"/> when <see cref="Guard.HasSizeLessThanOrEqualTo{T}(T[],int,string)"/> (or an overload) fails.
        /// </summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void ThrowArgumentExceptionForHasSizeLessThanOrEqualTo<T>(List<T> list, int size, string name)
        {
            ThrowArgumentException(name, $"Parameter \"{name}\" ({typeof(List<T>).ToTypeString()}) must be sized <= {size}, had a size of {list.Count}");
        }

        /// <summary>
        /// Throws an <see cref="ArgumentException"/> when <see cref="Guard.HasSizeEqualTo{T}(T[],T[],string)"/> (or an overload) fails.
        /// </summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void ThrowArgumentExceptionForHasSizeEqualTo<T>(List<T> source, List<T> destination, string name)
        {
            ThrowArgumentException(name, $"The source \"{name}\" ({typeof(List<T>).ToTypeString()}) must be sized == {destination.Count}, had a size of {source.Count}");
        }

        /// <summary>
        /// Throws an <see cref="ArgumentException"/> when <see cref="Guard.HasSizeLessThanOrEqualTo{T}(T[],T[],string)"/> (or an overload) fails.
        /// </summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void ThrowArgumentExceptionForHasSizeLessThanOrEqualTo<T>(List<T> source, List<T> destination, string name)
        {
            ThrowArgumentException(name, $"The source \"{name}\" ({typeof(List<T>).ToTypeString()}) must be sized <= {destination.Count}, had a size of {source.Count}");
        }

        /// <summary>
        /// Throws an <see cref="ArgumentOutOfRangeException"/> when <see cref="Guard.IsInRangeFor{T}(int,T[],string)"/> (or an overload) fails.
        /// </summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void ThrowArgumentOutOfRangeExceptionForIsInRangeFor<T>(int index, List<T> list, string name)
        {
            ThrowArgumentOutOfRangeException(name, $"Parameter \"{name}\" (int) must be >= 0 and < {list.Count} to be a valid index for the target collection ({typeof(List<T>).ToTypeString()}), was {index}");
        }

        /// <summary>
        /// Throws an <see cref="ArgumentException"/> when <see cref="Guard.IsEmpty{T}(T[],string)"/> (or an overload) fails.
        /// </summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void ThrowArgumentExceptionForIsEmpty<T>(ICollection<T> collection, string name)
        {
            ThrowArgumentException(name, $"Parameter \"{name}\" ({typeof(ICollection<T>).ToTypeString()}) must be empty, had a size of {collection.Count}");
        }

        /// <summary>
        /// Throws an <see cref="ArgumentException"/> when <see cref="Guard.HasSizeEqualTo{T}(T[],int,string)"/> (or an overload) fails.
        /// </summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void ThrowArgumentExceptionForHasSizeEqualTo<T>(ICollection<T> collection, int size, string name)
        {
            ThrowArgumentException(name, $"Parameter \"{name}\" ({typeof(ICollection<T>).ToTypeString()}) must be sized == {size}, had a size of {collection.Count}");
        }

        /// <summary>
        /// Throws an <see cref="ArgumentException"/> when <see cref="Guard.HasSizeNotEqualTo{T}(T[],int,string)"/> (or an overload) fails.
        /// </summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void ThrowArgumentExceptionForHasSizeNotEqualTo<T>(ICollection<T> collection, int size, string name)
        {
            ThrowArgumentException(name, $"Parameter \"{name}\" ({typeof(ICollection<T>).ToTypeString()}) must be sized != {size}, had a size of {collection.Count}");
        }

        /// <summary>
        /// Throws an <see cref="ArgumentException"/> when <see cref="Guard.HasSizeOver{T}(T[],int,string)"/> (or an overload) fails.
        /// </summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void ThrowArgumentExceptionForHasSizeOver<T>(ICollection<T> collection, int size, string name)
        {
            ThrowArgumentException(name, $"Parameter \"{name}\" ({typeof(ICollection<T>).ToTypeString()}) must be sized > {size}, had a size of {collection.Count}");
        }

        /// <summary>
        /// Throws an <see cref="ArgumentException"/> when <see cref="Guard.HasSizeAtLeast{T}(T[],int,string)"/> (or an overload) fails.
        /// </summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void ThrowArgumentExceptionForHasSizeAtLeast<T>(ICollection<T> collection, int size, string name)
        {
            ThrowArgumentException(name, $"Parameter \"{name}\" ({typeof(ICollection<T>).ToTypeString()}) must be sized >= {size}, had a size of {collection.Count}");
        }

        /// <summary>
        /// Throws an <see cref="ArgumentException"/> when <see cref="Guard.HasSizeLessThan{T}(T[],int,string)"/> (or an overload) fails.
        /// </summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void ThrowArgumentExceptionForHasSizeLessThan<T>(ICollection<T> collection, int size, string name)
        {
            ThrowArgumentException(name, $"Parameter \"{name}\" ({typeof(ICollection<T>).ToTypeString()}) must be sized < {size}, had a size of {collection.Count}");
        }

        /// <summary>
        /// Throws an <see cref="ArgumentException"/> when <see cref="Guard.HasSizeLessThanOrEqualTo{T}(T[],int,string)"/> (or an overload) fails.
        /// </summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void ThrowArgumentExceptionForHasSizeLessThanOrEqualTo<T>(ICollection<T> collection, int size, string name)
        {
            ThrowArgumentException(name, $"Parameter \"{name}\" ({typeof(ICollection<T>).ToTypeString()}) must be sized <= {size}, had a size of {collection.Count}");
        }

        /// <summary>
        /// Throws an <see cref="ArgumentException"/> when <see cref="Guard.HasSizeEqualTo{T}(T[],T[],string)"/> (or an overload) fails.
        /// </summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void ThrowArgumentExceptionForHasSizeEqualTo<T>(ICollection<T> source, ICollection<T> destination, string name)
        {
            ThrowArgumentException(name, $"The source \"{name}\" ({typeof(ICollection<T>).ToTypeString()}) must be sized == {destination.Count}, had a size of {source.Count}");
        }

        /// <summary>
        /// Throws an <see cref="ArgumentException"/> when <see cref="Guard.HasSizeLessThanOrEqualTo{T}(T[],T[],string)"/> (or an overload) fails.
        /// </summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void ThrowArgumentExceptionForHasSizeLessThanOrEqualTo<T>(ICollection<T> source, ICollection<T> destination, string name)
        {
            ThrowArgumentException(name, $"The source \"{name}\" ({typeof(ICollection<T>).ToTypeString()}) must be sized <= {destination.Count}, had a size of {source.Count}");
        }

        /// <summary>
        /// Throws an <see cref="ArgumentOutOfRangeException"/> when <see cref="Guard.IsInRangeFor{T}(int,T[],string)"/> (or an overload) fails.
        /// </summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void ThrowArgumentOutOfRangeExceptionForIsInRangeFor<T>(int index, ICollection<T> collection, string name)
        {
            ThrowArgumentOutOfRangeException(name, $"Parameter \"{name}\" (int) must be >= 0 and < {collection.Count} to be a valid index for the target collection ({typeof(ICollection<T>).ToTypeString()}), was {index}");
        }

        /// <summary>
        /// Throws an <see cref="ArgumentException"/> when <see cref="Guard.IsEmpty{T}(T[],string)"/> (or an overload) fails.
        /// </summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void ThrowArgumentExceptionForIsEmpty<T>(IReadOnlyCollection<T> collection, string name)
        {
            ThrowArgumentException(name, $"Parameter \"{name}\" ({typeof(IReadOnlyCollection<T>).ToTypeString()}) must be empty, had a size of {collection.Count}");
        }

        /// <summary>
        /// Throws an <see cref="ArgumentException"/> when <see cref="Guard.HasSizeEqualTo{T}(T[],int,string)"/> (or an overload) fails.
        /// </summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void ThrowArgumentExceptionForHasSizeEqualTo<T>(IReadOnlyCollection<T> collection, int size, string name)
        {
            ThrowArgumentException(name, $"Parameter \"{name}\" ({typeof(IReadOnlyCollection<T>).ToTypeString()}) must be sized == {size}, had a size of {collection.Count}");
        }

        /// <summary>
        /// Throws an <see cref="ArgumentException"/> when <see cref="Guard.HasSizeNotEqualTo{T}(T[],int,string)"/> (or an overload) fails.
        /// </summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void ThrowArgumentExceptionForHasSizeNotEqualTo<T>(IReadOnlyCollection<T> collection, int size, string name)
        {
            ThrowArgumentException(name, $"Parameter \"{name}\" ({typeof(IReadOnlyCollection<T>).ToTypeString()}) must be sized != {size}, had a size of {collection.Count}");
        }

        /// <summary>
        /// Throws an <see cref="ArgumentException"/> when <see cref="Guard.HasSizeOver{T}(T[],int,string)"/> (or an overload) fails.
        /// </summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void ThrowArgumentExceptionForHasSizeOver<T>(IReadOnlyCollection<T> collection, int size, string name)
        {
            ThrowArgumentException(name, $"Parameter \"{name}\" ({typeof(IReadOnlyCollection<T>).ToTypeString()}) must be sized > {size}, had a size of {collection.Count}");
        }

        /// <summary>
        /// Throws an <see cref="ArgumentException"/> when <see cref="Guard.HasSizeAtLeast{T}(T[],int,string)"/> (or an overload) fails.
        /// </summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void ThrowArgumentExceptionForHasSizeAtLeast<T>(IReadOnlyCollection<T> collection, int size, string name)
        {
            ThrowArgumentException(name, $"Parameter \"{name}\" ({typeof(IReadOnlyCollection<T>).ToTypeString()}) must be sized >= {size}, had a size of {collection.Count}");
        }

        /// <summary>
        /// Throws an <see cref="ArgumentException"/> when <see cref="Guard.HasSizeLessThan{T}(T[],int,string)"/> (or an overload) fails.
        /// </summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void ThrowArgumentExceptionForHasSizeLessThan<T>(IReadOnlyCollection<T> collection, int size, string name)
        {
            ThrowArgumentException(name, $"Parameter \"{name}\" ({typeof(IReadOnlyCollection<T>).ToTypeString()}) must be sized < {size}, had a size of {collection.Count}");
        }

        /// <summary>
        /// Throws an <see cref="ArgumentException"/> when <see cref="Guard.HasSizeLessThanOrEqualTo{T}(T[],int,string)"/> (or an overload) fails.
        /// </summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void ThrowArgumentExceptionForHasSizeLessThanOrEqualTo<T>(IReadOnlyCollection<T> collection, int size, string name)
        {
            ThrowArgumentException(name, $"Parameter \"{name}\" ({typeof(IReadOnlyCollection<T>).ToTypeString()}) must be sized <= {size}, had a size of {collection.Count}");
        }

        /// <summary>
        /// Throws an <see cref="ArgumentException"/> when <see cref="Guard.HasSizeEqualTo{T}(T[],T[],string)"/> (or an overload) fails.
        /// </summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void ThrowArgumentExceptionForHasSizeEqualTo<T>(IReadOnlyCollection<T> source, ICollection<T> destination, string name)
        {
            ThrowArgumentException(name, $"The source \"{name}\" ({typeof(IReadOnlyCollection<T>).ToTypeString()}) must be sized == {destination.Count}, had a size of {source.Count}");
        }

        /// <summary>
        /// Throws an <see cref="ArgumentException"/> when <see cref="Guard.HasSizeLessThanOrEqualTo{T}(T[],T[],string)"/> (or an overload) fails.
        /// </summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void ThrowArgumentExceptionForHasSizeLessThanOrEqualTo<T>(IReadOnlyCollection<T> source, ICollection<T> destination, string name)
        {
            ThrowArgumentException(name, $"The source \"{name}\" ({typeof(IReadOnlyCollection<T>).ToTypeString()}) must be sized <= {destination.Count}, had a size of {source.Count}");
        }

        /// <summary>
        /// Throws an <see cref="ArgumentOutOfRangeException"/> when <see cref="Guard.IsInRangeFor{T}(int,T[],string)"/> (or an overload) fails.
        /// </summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void ThrowArgumentOutOfRangeExceptionForIsInRangeFor<T>(int index, IReadOnlyCollection<T> collection, string name)
        {
            ThrowArgumentOutOfRangeException(name, $"Parameter \"{name}\" (int) must be >= 0 and < {collection.Count} to be a valid index for the target collection ({typeof(IReadOnlyCollection<T>).ToTypeString()}), was {index}");
        }
    }
}
