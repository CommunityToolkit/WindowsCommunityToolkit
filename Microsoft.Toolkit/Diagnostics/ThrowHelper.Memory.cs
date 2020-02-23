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
        /// Throws an <see cref="ArgumentException"/> when <see cref="Guard.IsEmpty{T}(ReadOnlyMemory{T},string)"/> fails.
        /// </summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void ThrowArgumentExceptionForIsEmpty<T>(ReadOnlyMemory<T> memory, string name)
        {
            if (memory.Length != 0)
            {
                ThrowArgumentException(name, $"Parameter {name} must be empty, had a size of {memory.Length}");
            }
        }

        /// <summary>
        /// Throws an <see cref="ArgumentException"/> when <see cref="Guard.IsEmpty{T}(Memory{T},string)"/> fails.
        /// </summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void ThrowArgumentExceptionForIsEmpty<T>(Memory<T> memory, string name)
        {
            if (memory.Length != 0)
            {
                ThrowArgumentException(name, $"Parameter {name} must be empty, had a size of {memory.Length}");
            }
        }

        /// <summary>
        /// Throws an <see cref="ArgumentException"/> when <see cref="Guard.IsNotEmpty{T}(ReadOnlyMemory{T},string)"/> fails.
        /// </summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void ThrowArgumentExceptionForIsNotEmpty<T>(ReadOnlyMemory<T> memory, string name)
        {
            if (memory.Length != 0)
            {
                ThrowArgumentException(name, $"Parameter {name} must not be empty");
            }
        }

        /// <summary>
        /// Throws an <see cref="ArgumentException"/> when <see cref="Guard.IsNotEmpty{T}(Memory{T},string)"/> fails.
        /// </summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void ThrowArgumentExceptionForIsNotEmpty<T>(Memory<T> memory, string name)
        {
            if (memory.Length != 0)
            {
                ThrowArgumentException(name, $"Parameter {name} must not be empty");
            }
        }

        /// <summary>
        /// Throws an <see cref="ArgumentException"/> when <see cref="Guard.HasSizeEqualTo{T}(ReadOnlyMemory{T},int,string)"/> fails.
        /// </summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void ThrowArgumentExceptionForHasSizeEqualTo<T>(ReadOnlyMemory<T> memory, int size, string name)
        {
            if (memory.Length != size)
            {
                ThrowArgumentException(name, $"Parameter {name} must be sized == {size}, had a size of {memory.Length}");
            }
        }

        /// <summary>
        /// Throws an <see cref="ArgumentException"/> when <see cref="Guard.HasSizeEqualTo{T}(Memory{T},int,string)"/> fails.
        /// </summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void ThrowArgumentExceptionForHasSizeEqualTo<T>(Memory<T> memory, int size, string name)
        {
            if (memory.Length != size)
            {
                ThrowArgumentException(name, $"Parameter {name} must be sized == {size}, had a size of {memory.Length}");
            }
        }

        /// <summary>
        /// Throws an <see cref="ArgumentException"/> when <see cref="Guard.HasSizeNotEqualTo{T}(ReadOnlyMemory{T},int,string)"/> fails.
        /// </summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void ThrowArgumentExceptionForHasSizeNotEqualTo<T>(ReadOnlyMemory<T> memory, int size, string name)
        {
            if (memory.Length == size)
            {
                ThrowArgumentException(name, $"Parameter {name} must be sized != {size}, had a size of {memory.Length}");
            }
        }

        /// <summary>
        /// Throws an <see cref="ArgumentException"/> when <see cref="Guard.HasSizeNotEqualTo{T}(Memory{T},int,string)"/> fails.
        /// </summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void ThrowArgumentExceptionForHasSizeNotEqualTo<T>(Memory<T> memory, int size, string name)
        {
            if (memory.Length == size)
            {
                ThrowArgumentException(name, $"Parameter {name} must be sized != {size}, had a size of {memory.Length}");
            }
        }

        /// <summary>
        /// Throws an <see cref="ArgumentException"/> when <see cref="Guard.HasSizeOver{T}(ReadOnlyMemory{T},int,string)"/> fails.
        /// </summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void ThrowArgumentExceptionForHasSizeOver<T>(ReadOnlyMemory<T> memory, int size, string name)
        {
            if (memory.Length <= size)
            {
                ThrowArgumentException(name, $"Parameter {name} must be sized > {size}, had a size of {memory.Length}");
            }
        }

        /// <summary>
        /// Throws an <see cref="ArgumentException"/> when <see cref="Guard.HasSizeOver{T}(Memory{T},int,string)"/> fails.
        /// </summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void ThrowArgumentExceptionForHasSizeOver<T>(Memory<T> memory, int size, string name)
        {
            if (memory.Length <= size)
            {
                ThrowArgumentException(name, $"Parameter {name} must be sized > {size}, had a size of {memory.Length}");
            }
        }

        /// <summary>
        /// Throws an <see cref="ArgumentException"/> when <see cref="Guard.HasSizeAtLeast{T}(ReadOnlyMemory{T},int,string)"/> fails.
        /// </summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void ThrowArgumentExceptionForHasSizeAtLeast<T>(ReadOnlyMemory<T> memory, int size, string name)
        {
            if (memory.Length < size)
            {
                ThrowArgumentException(name, $"Parameter {name} must be sized >= {size}, had a size of {memory.Length}");
            }
        }

        /// <summary>
        /// Throws an <see cref="ArgumentException"/> when <see cref="Guard.HasSizeAtLeast{T}(Memory{T},int,string)"/> fails.
        /// </summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void ThrowArgumentExceptionForHasSizeAtLeast<T>(Memory<T> memory, int size, string name)
        {
            if (memory.Length < size)
            {
                ThrowArgumentException(name, $"Parameter {name} must be sized >= {size}, had a size of {memory.Length}");
            }
        }

        /// <summary>
        /// Throws an <see cref="ArgumentException"/> when <see cref="Guard.HasSizeLessThan{T}(ReadOnlyMemory{T},int,string)"/> fails.
        /// </summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void ThrowArgumentExceptionForHasSizeLessThan<T>(ReadOnlyMemory<T> memory, int size, string name)
        {
            if (memory.Length >= size)
            {
                ThrowArgumentException(name, $"Parameter {name} must be sized < {size}, had a size of {memory.Length}");
            }
        }

        /// <summary>
        /// Throws an <see cref="ArgumentException"/> when <see cref="Guard.HasSizeLessThan{T}(Memory{T},int,string)"/> fails.
        /// </summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void ThrowArgumentExceptionForHasSizeLessThan<T>(Memory<T> memory, int size, string name)
        {
            if (memory.Length >= size)
            {
                ThrowArgumentException(name, $"Parameter {name} must be sized < {size}, had a size of {memory.Length}");
            }
        }

        /// <summary>
        /// Throws an <see cref="ArgumentException"/> when <see cref="Guard.HasSizeLessThanOrEqualTo{T}(ReadOnlyMemory{T},int,string)"/> fails.
        /// </summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void ThrowArgumentExceptionForHasSizeLessThanOrEqualTo<T>(ReadOnlyMemory<T> memory, int size, string name)
        {
            if (memory.Length > size)
            {
                ThrowArgumentException(name, $"Parameter {name} must be sized <= {size}, had a size of {memory.Length}");
            }
        }

        /// <summary>
        /// Throws an <see cref="ArgumentException"/> when <see cref="Guard.HasSizeLessThanOrEqualTo{T}(Memory{T},int,string)"/> fails.
        /// </summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void ThrowArgumentExceptionForHasSizeLessThanOrEqualTo<T>(Memory<T> memory, int size, string name)
        {
            if (memory.Length > size)
            {
                ThrowArgumentException(name, $"Parameter {name} must be sized <= {size}, had a size of {memory.Length}");
            }
        }

        /// <summary>
        /// Throws an <see cref="ArgumentException"/> when <see cref="Guard.HasSizeEqualTo{T}(ReadOnlyMemory{T},Memory{T},string)"/> fails.
        /// </summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void ThrowArgumentExceptionForHasSizeEqualTo<T>(ReadOnlyMemory<T> source, Memory<T> destination, string name)
        {
            if (source.Length != destination.Length)
            {
                ThrowArgumentException(name, $"The source {name} must be sized == {destination.Length}, had a size of {source.Length}");
            }
        }

        /// <summary>
        /// Throws an <see cref="ArgumentException"/> when <see cref="Guard.HasSizeEqualTo{T}(Memory{T},Memory{T},string)"/> fails.
        /// </summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void ThrowArgumentExceptionForHasSizeEqualTo<T>(Memory<T> source, Memory<T> destination, string name)
        {
            if (source.Length != destination.Length)
            {
                ThrowArgumentException(name, $"The source {name} must be sized == {destination.Length}, had a size of {source.Length}");
            }
        }

        /// <summary>
        /// Throws an <see cref="ArgumentException"/> when <see cref="Guard.HasSizeLessThanOrEqualTo{T}(ReadOnlyMemory{T},Memory{T},string)"/> fails.
        /// </summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void ThrowArgumentExceptionForHasSizeLessThanOrEqualTo<T>(ReadOnlyMemory<T> source, Memory<T> destination, string name)
        {
            if (source.Length > destination.Length)
            {
                ThrowArgumentException(name, $"The source {name} must be sized <= {destination.Length}, had a size of {source.Length}");
            }
        }

        /// <summary>
        /// Throws an <see cref="ArgumentException"/> when <see cref="Guard.HasSizeLessThanOrEqualTo{T}(Memory{T},Memory{T},string)"/> fails.
        /// </summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void ThrowArgumentExceptionForHasSizeLessThanOrEqualTo<T>(Memory<T> source, Memory<T> destination, string name)
        {
            if (source.Length > destination.Length)
            {
                ThrowArgumentException(name, $"The source {name} must be sized <= {destination.Length}, had a size of {source.Length}");
            }
        }
    }
}
