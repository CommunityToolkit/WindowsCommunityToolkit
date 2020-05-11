// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.IO;
using System.Runtime.CompilerServices;

namespace Microsoft.Toolkit.HighPerformance.Streams
{
    /// <summary>
    /// A <see cref="Stream"/> implementation wrapping a <see cref="Memory{T}"/> or <see cref="ReadOnlyMemory{T}"/> instance.
    /// </summary>
    internal partial class MemoryStream
    {
        /// <summary>
        /// Throws an <see cref="ArgumentOutOfRangeException"/> when setting the <see cref="Stream.Position"/> property.
        /// </summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        private static void ThrowArgumentOutOfRangeExceptionForPosition()
        {
            throw new ArgumentOutOfRangeException(nameof(Position), "The value for the property was not in the valid range.");
        }

        /// <summary>
        /// Throws an <see cref="ArgumentNullException"/> when an input buffer is <see langword="null"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        private static void ThrowArgumentNullExceptionForBuffer()
        {
            throw new ArgumentNullException("buffer", "The buffer is null.");
        }

        /// <summary>
        /// Throws an <see cref="ArgumentOutOfRangeException"/> when the input count is negative.
        /// </summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        private static void ThrowArgumentOutOfRangeExceptionForOffset()
        {
            throw new ArgumentOutOfRangeException("offset", "Offset can't be negative.");
        }

        /// <summary>
        /// Throws an <see cref="ArgumentOutOfRangeException"/> when the input count is negative.
        /// </summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        private static void ThrowArgumentOutOfRangeExceptionForCount()
        {
            throw new ArgumentOutOfRangeException("count", "Count can't be negative.");
        }

        /// <summary>
        /// Throws an <see cref="ArgumentException"/> when the sum of offset and count exceeds the length of the target buffer.
        /// </summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        private static void ThrowArgumentExceptionForLength()
        {
            throw new ArgumentException("The sum of offset and count can't be larger than the buffer length.", "buffer");
        }

        /// <summary>
        /// Throws a <see cref="NotSupportedException"/> when trying to write on a readonly stream.
        /// </summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        private static void ThrowNotSupportedExceptionForCanWrite()
        {
            throw new NotSupportedException("The current stream doesn't support writing.");
        }

        /// <summary>
        /// Throws an <see cref="ArgumentException"/> when trying to write too many bytes to the target stream.
        /// </summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        private static void ThrowArgumentExceptionForEndOfStreamOnWrite()
        {
            throw new ArgumentException("The current stream can't contain the requested input data.");
        }

        /// <summary>
        /// Throws a <see cref="NotSupportedException"/> when trying to set the length of the stream.
        /// </summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        private static void ThrowNotSupportedExceptionForSetLength()
        {
            throw new NotSupportedException("Setting the length is not supported for this stream.");
        }

        /// <summary>
        /// Throws an <see cref="ArgumentException"/> when using an invalid seek mode.
        /// </summary>
        /// <returns>Nothing, as this method throws unconditionally.</returns>
        [MethodImpl(MethodImplOptions.NoInlining)]
        private static long ThrowArgumentExceptionForSeekOrigin()
        {
            throw new ArgumentException("The input seek mode is not valid.", "origin");
        }

        /// <summary>
        /// Throws an <see cref="ObjectDisposedException"/> when using a disposed <see cref="Stream"/> instance.
        /// </summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        private static void ThrowObjectDisposedException()
        {
            throw new ObjectDisposedException(nameof(memory), "The current stream has already been disposed");
        }
    }
}
