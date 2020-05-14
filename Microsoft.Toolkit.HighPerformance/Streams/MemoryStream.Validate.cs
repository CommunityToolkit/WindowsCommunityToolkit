// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.IO;
using System.Runtime.CompilerServices;

namespace Microsoft.Toolkit.HighPerformance.Streams
{
    /// <summary>
    /// A <see cref="Stream"/> implementation wrapping a <see cref="System.Memory{T}"/> or <see cref="System.ReadOnlyMemory{T}"/> instance.
    /// </summary>
    internal partial class MemoryStream
    {
        /// <summary>
        /// Validates the <see cref="Stream.Position"/> argument.
        /// </summary>
        /// <param name="position">The new <see cref="Stream.Position"/> value being set.</param>
        /// <param name="length">The maximum length of the target <see cref="Stream"/>.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void ValidatePosition(long position, int length)
        {
            if ((ulong)position >= (ulong)length)
            {
                ThrowArgumentOutOfRangeExceptionForPosition();
            }
        }

        /// <summary>
        /// Validates the <see cref="Stream.Read(byte[],int,int)"/> or <see cref="Stream.Write(byte[],int,int)"/> arguments.
        /// </summary>
        /// <param name="buffer">The target array.</param>
        /// <param name="offset">The offset within <paramref name="buffer"/>.</param>
        /// <param name="count">The number of elements to process within <paramref name="buffer"/>.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void ValidateBuffer(byte[]? buffer, int offset, int count)
        {
            if (buffer is null)
            {
                ThrowArgumentNullExceptionForBuffer();
            }

            if (offset < 0)
            {
                ThrowArgumentOutOfRangeExceptionForOffset();
            }

            if (count < 0)
            {
                ThrowArgumentOutOfRangeExceptionForCount();
            }

            if (offset + count > buffer!.Length)
            {
                ThrowArgumentExceptionForLength();
            }
        }

        /// <summary>
        /// Validates the <see cref="CanWrite"/> property.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void ValidateCanWrite()
        {
            if (!CanWrite)
            {
                ThrowNotSupportedExceptionForCanWrite();
            }
        }

        /// <summary>
        /// Validates that the current instance hasn't been disposed.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void ValidateDisposed()
        {
            if (this.disposed)
            {
                ThrowObjectDisposedException();
            }
        }
    }
}
