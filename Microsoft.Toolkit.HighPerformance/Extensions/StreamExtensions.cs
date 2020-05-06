// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.IO;
#if !SPAN_RUNTIME_SUPPORT
using System;
using System.Buffers;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
#endif

namespace Microsoft.Toolkit.HighPerformance.Extensions
{
    /// <summary>
    /// Helpers for working with the <see cref="Stream"/> type.
    /// </summary>
    public static class StreamExtensions
    {
#if !SPAN_RUNTIME_SUPPORT
        /// <summary>
        /// Asynchronously reads a sequence of bytes from a given <see cref="Stream"/> instance.
        /// </summary>
        /// <param name="stream">The source <see cref="Stream"/> to read data from.</param>
        /// <param name="buffer">The destination <see cref="Memory{T}"/> to write data to.</param>
        /// <param name="cancellationToken">The optional <see cref="CancellationToken"/> for the operation.</param>
        /// <returns>A <see cref="ValueTask"/> representing the operation being performed.</returns>
        public static ValueTask<int> ReadAsync(this Stream stream, Memory<byte> buffer, CancellationToken cancellationToken = default)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return new ValueTask<int>(Task.FromCanceled<int>(cancellationToken));
            }

            // If the memory wraps an array, extract it and use it directly
            if (MemoryMarshal.TryGetArray(buffer, out ArraySegment<byte> segment))
            {
                return new ValueTask<int>(stream.ReadAsync(segment.Array!, segment.Offset, segment.Count, cancellationToken));
            }

            // Local function used as the fallback path. This happens when the input memory
            // doesn't wrap an array instance we can use. We use a local function as we need
            // the body to be asynchronous, in order to execute the finally block after the
            // write operation has been completed. By separating the logic, we can keep the
            // main method as a synchronous, value-task returning function. This fallback
            // path should hopefully be pretty rare, as memory instances are typically just
            // created around arrays, often being rented from a memory pool in particular.
            static async Task<int> ReadAsyncFallback(Stream stream, Memory<byte> buffer, CancellationToken cancellationToken)
            {
                byte[] rent = ArrayPool<byte>.Shared.Rent(buffer.Length);

                try
                {
                    int bytesRead = await stream.ReadAsync(rent, 0, buffer.Length, cancellationToken);

                    if (bytesRead > 0)
                    {
                        rent.AsSpan(0, bytesRead).CopyTo(buffer.Span);
                    }

                    return bytesRead;
                }
                finally
                {
                    ArrayPool<byte>.Shared.Return(rent);
                }
            }

            return new ValueTask<int>(ReadAsyncFallback(stream, buffer, cancellationToken));
        }

        /// <summary>
        /// Asynchronously writes a sequence of bytes to a given <see cref="Stream"/> instance.
        /// </summary>
        /// <param name="stream">The destination <see cref="Stream"/> to write data to.</param>
        /// <param name="buffer">The source <see cref="ReadOnlyMemory{T}"/> to read data from.</param>
        /// <param name="cancellationToken">The optional <see cref="CancellationToken"/> for the operation.</param>
        /// <returns>A <see cref="ValueTask"/> representing the operation being performed.</returns>
        public static ValueTask WriteAsync(this Stream stream, ReadOnlyMemory<byte> buffer, CancellationToken cancellationToken = default)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return new ValueTask(Task.FromCanceled(cancellationToken));
            }

            if (MemoryMarshal.TryGetArray(buffer, out ArraySegment<byte> segment))
            {
                return new ValueTask(stream.WriteAsync(segment.Array!, segment.Offset, segment.Count, cancellationToken));
            }

            // Local function, same idea as above
            static async Task WriteAsyncFallback(Stream stream, ReadOnlyMemory<byte> buffer, CancellationToken cancellationToken)
            {
                byte[] rent = ArrayPool<byte>.Shared.Rent(buffer.Length);

                try
                {
                    buffer.Span.CopyTo(rent);

                    await stream.WriteAsync(rent, 0, buffer.Length, cancellationToken);
                }
                finally
                {
                    ArrayPool<byte>.Shared.Return(rent);
                }
            }

            return new ValueTask(WriteAsyncFallback(stream, buffer, cancellationToken));
        }

        /// <summary>
        /// Reads a sequence of bytes from a given <see cref="Stream"/> instance.
        /// </summary>
        /// <param name="stream">The source <see cref="Stream"/> to read data from.</param>
        /// <param name="buffer">The target <see cref="Span{T}"/> to write data to.</param>
        /// <returns>The number of bytes that have been read.</returns>
        public static int Read(this Stream stream, Span<byte> buffer)
        {
            byte[] rent = ArrayPool<byte>.Shared.Rent(buffer.Length);

            try
            {
                int bytesRead = stream.Read(rent, 0, buffer.Length);

                if (bytesRead > 0)
                {
                    rent.AsSpan(0, bytesRead).CopyTo(buffer);
                }

                return bytesRead;
            }
            finally
            {
                ArrayPool<byte>.Shared.Return(rent);
            }
        }

        /// <summary>
        /// Writes a sequence of bytes to a given <see cref="Stream"/> instance.
        /// </summary>
        /// <param name="stream">The destination <see cref="Stream"/> to write data to.</param>
        /// <param name="buffer">The source <see cref="Span{T}"/> to read data from.</param>
        public static void Write(this Stream stream, ReadOnlySpan<byte> buffer)
        {
            byte[] rent = ArrayPool<byte>.Shared.Rent(buffer.Length);

            try
            {
                buffer.CopyTo(rent);

                stream.Write(rent, 0, buffer.Length);
            }
            finally
            {
                ArrayPool<byte>.Shared.Return(rent);
            }
        }
#endif
    }
}