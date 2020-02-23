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
        /// Throws an <see cref="ArgumentException"/> when <see cref="Guard.IsEmpty{T}(ReadOnlySpan{T},string)"/> fails.
        /// </summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void ThrowArgumentExceptionForIsEmpty<T>(ReadOnlySpan<T> span, string name)
        {
            ThrowArgumentException(name, $"Parameter {name} must be empty, had a size of {span.Length}");
        }

        /// <summary>
        /// Throws an <see cref="ArgumentException"/> when <see cref="Guard.IsEmpty{T}(Span{T},string)"/> fails.
        /// </summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void ThrowArgumentExceptionForIsEmpty<T>(Span<T> span, string name)
        {
            ThrowArgumentException(name, $"Parameter {name} must be empty, had a size of {span.Length}");
        }

        /// <summary>
        /// Throws an <see cref="ArgumentException"/> when <see cref="Guard.IsNotEmpty{T}(ReadOnlySpan{T},string)"/> fails.
        /// </summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void ThrowArgumentExceptionForIsNotEmpty<T>(ReadOnlySpan<T> span, string name)
        {
            ThrowArgumentException(name, $"Parameter {name} must not be empty");
        }

        /// <summary>
        /// Throws an <see cref="ArgumentException"/> when <see cref="Guard.IsNotEmpty{T}(Span{T},string)"/> fails.
        /// </summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void ThrowArgumentExceptionForIsNotEmpty<T>(Span<T> span, string name)
        {
            ThrowArgumentException(name, $"Parameter {name} must not be empty");
        }

        /// <summary>
        /// Throws an <see cref="ArgumentException"/> when <see cref="Guard.HasSizeEqualTo{T}(ReadOnlySpan{T},int,string)"/> fails.
        /// </summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void ThrowArgumentExceptionForHasSizeEqualTo<T>(ReadOnlySpan<T> span, int size, string name)
        {
            ThrowArgumentException(name, $"Parameter {name} must be sized == {size}, had a size of {span.Length}");
        }

        /// <summary>
        /// Throws an <see cref="ArgumentException"/> when <see cref="Guard.HasSizeEqualTo{T}(Span{T},int,string)"/> fails.
        /// </summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void ThrowArgumentExceptionForHasSizeEqualTo<T>(Span<T> span, int size, string name)
        {
            ThrowArgumentException(name, $"Parameter {name} must be sized == {size}, had a size of {span.Length}");
        }

        /// <summary>
        /// Throws an <see cref="ArgumentException"/> when <see cref="Guard.HasSizeNotEqualTo{T}(ReadOnlySpan{T},int,string)"/> fails.
        /// </summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void ThrowArgumentExceptionForHasSizeNotEqualTo<T>(ReadOnlySpan<T> span, int size, string name)
        {
            ThrowArgumentException(name, $"Parameter {name} must be sized != {size}, had a size of {span.Length}");
        }

        /// <summary>
        /// Throws an <see cref="ArgumentException"/> when <see cref="Guard.HasSizeNotEqualTo{T}(Span{T},int,string)"/> fails.
        /// </summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void ThrowArgumentExceptionForHasSizeNotEqualTo<T>(Span<T> span, int size, string name)
        {
            ThrowArgumentException(name, $"Parameter {name} must be sized != {size}, had a size of {span.Length}");
        }

        /// <summary>
        /// Throws an <see cref="ArgumentException"/> when <see cref="Guard.HasSizeOver{T}(ReadOnlySpan{T},int,string)"/> fails.
        /// </summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void ThrowArgumentExceptionForHasSizeOver<T>(ReadOnlySpan<T> span, int size, string name)
        {
            ThrowArgumentException(name, $"Parameter {name} must be sized >= {size}, had a size of {span.Length}");
        }

        /// <summary>
        /// Throws an <see cref="ArgumentException"/> when <see cref="Guard.HasSizeOver{T}(Span{T},int,string)"/> fails.
        /// </summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void ThrowArgumentExceptionForHasSizeOver<T>(Span<T> span, int size, string name)
        {
            ThrowArgumentException(name, $"Parameter {name} must be sized >= {size}, had a size of {span.Length}");
        }

        /// <summary>
        /// Throws an <see cref="ArgumentException"/> when <see cref="Guard.HasSizeAtLeast{T}(ReadOnlySpan{T},int,string)"/> fails.
        /// </summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void ThrowArgumentExceptionForHasSizeAtLeast<T>(ReadOnlySpan<T> span, int size, string name)
        {
            ThrowArgumentException(name, $"Parameter {name} must be sized >= {size}, had a size of {span.Length}");
        }

        /// <summary>
        /// Throws an <see cref="ArgumentException"/> when <see cref="Guard.HasSizeAtLeast{T}(Span{T},int,string)"/> fails.
        /// </summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void ThrowArgumentExceptionForHasSizeAtLeast<T>(Span<T> span, int size, string name)
        {
            ThrowArgumentException(name, $"Parameter {name} must be sized >= {size}, had a size of {span.Length}");
        }

        /// <summary>
        /// Throws an <see cref="ArgumentException"/> when <see cref="Guard.HasSizeLessThan{T}(ReadOnlySpan{T},int,string)"/> fails.
        /// </summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void ThrowArgumentExceptionForHasSizeLessThan<T>(ReadOnlySpan<T> span, int size, string name)
        {
            ThrowArgumentException(name, $"Parameter {name} must be sized < {size}, had a size of {span.Length}");
        }

        /// <summary>
        /// Throws an <see cref="ArgumentException"/> when <see cref="Guard.HasSizeLessThan{T}(Span{T},int,string)"/> fails.
        /// </summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void ThrowArgumentExceptionForHasSizeLessThan<T>(Span<T> span, int size, string name)
        {
            ThrowArgumentException(name, $"Parameter {name} must be sized < {size}, had a size of {span.Length}");
        }

        /// <summary>
        /// Throws an <see cref="ArgumentException"/> when <see cref="Guard.HasSizeLessThanOrEqualTo{T}(ReadOnlySpan{T},int,string)"/> fails.
        /// </summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void ThrowArgumentExceptionForHasSizeLessThanOrEqualTo<T>(ReadOnlySpan<T> span, int size, string name)
        {
            ThrowArgumentException(name, $"Parameter {name} must be sized <= {size}, had a size of {span.Length}");
        }

        /// <summary>
        /// Throws an <see cref="ArgumentException"/> when <see cref="Guard.HasSizeLessThanOrEqualTo{T}(Span{T},int,string)"/> fails.
        /// </summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void ThrowArgumentExceptionForHasSizeLessThanOrEqualTo<T>(Span<T> span, int size, string name)
        {
            ThrowArgumentException(name, $"Parameter {name} must be sized <= {size}, had a size of {span.Length}");
        }

        /// <summary>
        /// Throws an <see cref="ArgumentException"/> when <see cref="Guard.HasSizeEqualTo{T}(ReadOnlySpan{T},Span{T},string)"/> fails.
        /// </summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void ThrowArgumentExceptionForHasSizeEqualTo<T>(ReadOnlySpan<T> source, Span<T> destination, string name)
        {
            ThrowArgumentException(name, $"The source {name} must be sized == {destination.Length}, had a size of {source.Length}");
        }

        /// <summary>
        /// Throws an <see cref="ArgumentException"/> when <see cref="Guard.HasSizeEqualTo{T}(Span{T},Span{T},string)"/> fails.
        /// </summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void ThrowArgumentExceptionForHasSizeEqualTo<T>(Span<T> source, Span<T> destination, string name)
        {
            ThrowArgumentException(name, $"The source {name} must be sized == {destination.Length}, had a size of {source.Length}");
        }

        /// <summary>
        /// Throws an <see cref="ArgumentException"/> when <see cref="Guard.HasSizeLessThanOrEqualTo{T}(ReadOnlySpan{T},Span{T},string)"/> fails.
        /// </summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void ThrowArgumentExceptionForHasSizeLessThanOrEqualTo<T>(ReadOnlySpan<T> source, Span<T> destination, string name)
        {
            ThrowArgumentException(name, $"The source {name} must be sized <= {destination.Length}, had a size of {source.Length}");
        }

        /// <summary>
        /// Throws an <see cref="ArgumentException"/> when <see cref="Guard.HasSizeLessThanOrEqualTo{T}(Span{T},Span{T},string)"/> fails.
        /// </summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void ThrowArgumentExceptionForHasSizeLessThanOrEqualTo<T>(Span<T> source, Span<T> destination, string name)
        {
            ThrowArgumentException(name, $"The source {name} must be sized <= {destination.Length}, had a size of {source.Length}");
        }
    }
}
