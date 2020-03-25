// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

#if NETSTANDARD2_1

using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

#nullable enable

namespace Microsoft.Toolkit.HighPerformance.Streams
{
    /// <summary>
    /// A <see cref="Stream"/> implementation wrapping a <see cref="Memory{T}"/> or <see cref="ReadOnlyMemory{T}"/> instance.
    /// </summary>
    internal partial class MemoryStream : Stream
    {
        /// <inheritdoc/>
        public override void CopyTo(Stream destination, int bufferSize)
        {
            ValidateDisposed();

            Span<byte> source = this.memory.Span.Slice(this.position);

            this.position += source.Length;

            destination.Write(source);
        }

        /// <inheritdoc/>
        public override ValueTask<int> ReadAsync(Memory<byte> buffer, CancellationToken cancellationToken = default)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return new ValueTask<int>(Task.FromCanceled<int>(cancellationToken));
            }

            try
            {
                int result = Read(buffer.Span);

                return new ValueTask<int>(result);
            }
            catch (OperationCanceledException e)
            {
                return new ValueTask<int>(Task.FromCanceled<int>(e.CancellationToken));
            }
            catch (Exception e)
            {
                return new ValueTask<int>(Task.FromException<int>(e));
            }
        }

        /// <inheritdoc/>
        public override ValueTask WriteAsync(ReadOnlyMemory<byte> buffer, CancellationToken cancellationToken = default)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return new ValueTask(Task.FromCanceled(cancellationToken));
            }

            try
            {
                Write(buffer.Span);

                return default;
            }
            catch (OperationCanceledException e)
            {
                return new ValueTask(Task.FromCanceled(e.CancellationToken));
            }
            catch (Exception e)
            {
                return new ValueTask(Task.FromException(e));
            }
        }

        /// <inheritdoc/>
        public override int Read(Span<byte> buffer)
        {
            ValidateDisposed();

            Span<byte> source = this.memory.Span.Slice(this.position);

            int bytesCopied = Math.Min(source.Length, buffer.Length);

            Span<byte> destination = buffer.Slice(0, bytesCopied);

            source.CopyTo(destination);

            this.position += bytesCopied;

            return bytesCopied;
        }

        /// <inheritdoc/>
        public override void Write(ReadOnlySpan<byte> buffer)
        {
            ValidateDisposed();
            ValidateCanWrite();

            Span<byte> destination = this.memory.Span.Slice(this.position);

            if (!buffer.TryCopyTo(destination))
            {
                ThrowArgumentExceptionForEndOfStreamOnWrite();
            }

            this.position += buffer.Length;
        }
    }
}

#endif
