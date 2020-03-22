// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

#nullable enable

namespace Microsoft.Toolkit.HighPerformance.Streams
{
    /// <summary>
    /// A <see cref="Stream"/> implementation wrapping a <see cref="Memory{T}"/> or <see cref="ReadOnlyMemory{T}"/> instance.
    /// </summary>
    internal sealed partial class MemoryStream : Stream
    {
        /// <summary>
        /// Indicates whether <see cref="memory"/> was actually a <see cref="ReadOnlyMemory{T}"/> instance.
        /// </summary>
        private readonly bool isReadOnly;

        /// <summary>
        /// The <see cref="Memory{T}"/> instance currently in use.
        /// </summary>
        private Memory<byte> memory;

        /// <summary>
        /// The current position within <see cref="memory"/>.
        /// </summary>
        private int position;

        /// <summary>
        /// Indicates whether or not the current instance has been disposed
        /// </summary>
        private bool disposed;

        /// <summary>
        /// Initializes a new instance of the <see cref="MemoryStream"/> class.
        /// </summary>
        /// <param name="memory">The input <see cref="Memory{T}"/> instance to use.</param>
        public MemoryStream(Memory<byte> memory)
        {
            this.memory = memory;
            this.position = 0;
            this.isReadOnly = false;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MemoryStream"/> class.
        /// </summary>
        /// <param name="memory">The input <see cref="ReadOnlyMemory{T}"/> instance to use.</param>
        public MemoryStream(ReadOnlyMemory<byte> memory)
        {
            this.memory = MemoryMarshal.AsMemory(memory);
            this.position = 0;
            this.isReadOnly = true;
        }

        /// <inheritdoc/>
        public override bool CanRead
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => !this.disposed;
        }

        /// <inheritdoc/>
        public override bool CanSeek
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => !this.disposed;
        }

        /// <inheritdoc/>
        public override bool CanWrite
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => !this.isReadOnly && !this.disposed;
        }

        /// <inheritdoc/>
        public override long Length
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                ValidateDisposed();

                return this.memory.Length;
            }
        }

        /// <inheritdoc/>
        public override long Position
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                ValidateDisposed();

                return this.position;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set
            {
                ValidateDisposed();
                ValidatePosition(value, this.memory.Length);

                this.position = unchecked((int)value);
            }
        }

        /// <inheritdoc/>
        public override void Flush()
        {
        }

        /// <inheritdoc/>
        public override int Read(byte[]? buffer, int offset, int count)
        {
            ValidateDisposed();
            ValidateBuffer(buffer, offset, count);

            Span<byte> source = this.memory.Span.Slice(this.position);

            int bytesCopied = Math.Min(source.Length, count);

            Span<byte> destination = buffer.AsSpan(offset, bytesCopied);

            source.CopyTo(destination);

            this.position += bytesCopied;

            return bytesCopied;
        }

        /// <inheritdoc/>
        public override Task<int> ReadAsync(byte[]? buffer, int offset, int count, CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return Task.FromCanceled<int>(cancellationToken);
            }

            try
            {
                int result = Read(buffer, offset, count);

                return Task.FromResult(result);
            }
            catch (OperationCanceledException e)
            {
                return Task.FromCanceled<int>(e.CancellationToken);
            }
            catch (Exception e)
            {
                return Task.FromException<int>(e);
            }
        }

#if NETSTANDARD2_1
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
#endif

        /// <inheritdoc/>
        public override int ReadByte()
        {
            ValidateDisposed();

            if (this.position == this.memory.Length)
            {
                return -1;
            }

            return this.memory.Span[this.position++];
        }

        /// <inheritdoc/>
        public override long Seek(long offset, SeekOrigin origin)
        {
            ValidateDisposed();

            long index = origin switch
            {
                SeekOrigin.Begin => offset,
                SeekOrigin.Current => this.position + offset,
                SeekOrigin.End => this.memory.Length + offset,
                _ => ThrowArgumentExceptionForSeekOrigin()
            };

            ValidatePosition(index, this.memory.Length);

            this.position = unchecked((int)index);

            return index;
        }

        /// <inheritdoc/>
        public override void SetLength(long value)
        {
            ThrowNotSupportedExceptionForSetLength();
        }

        /// <inheritdoc/>
        public override void Write(byte[]? buffer, int offset, int count)
        {
            ValidateDisposed();
            ValidateCanWrite();
            ValidateBuffer(buffer, offset, count);

            Span<byte>
                source = buffer.AsSpan(offset, count),
                destination = this.memory.Span.Slice(this.position);

            if (!source.TryCopyTo(destination))
            {
                ThrowArgumentExceptionForEndOfStreamOnWrite();
            }

            this.position += source.Length;
        }

        /// <inheritdoc/>
        public override Task WriteAsync(byte[]? buffer, int offset, int count, CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return Task.FromCanceled(cancellationToken);
            }

            try
            {
                Write(buffer, offset, count);

                return Task.CompletedTask;
            }
            catch (OperationCanceledException e)
            {
                return Task.FromCanceled(e.CancellationToken);
            }
            catch (Exception e)
            {
                return Task.FromException(e);
            }
        }

#if NETSTANDARD2_1
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

        public override ValueTask WriteAsync(ReadOnlyMemory<byte> buffer, CancellationToken cancellationToken = default)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return new ValueTask(Task.FromCanceled(cancellationToken));
            }

            try
            {
                this.Write(buffer.Span);

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
#endif

        /// <inheritdoc/>
        public override void WriteByte(byte value)
        {
            ValidateDisposed();
            ValidateCanWrite();

            if (this.position == this.memory.Length)
            {
                ThrowArgumentExceptionForEndOfStreamOnWrite();
            }

            this.memory.Span[this.position++] = value;
        }

        /// <inheritdoc/>
        protected override void Dispose(bool disposing)
        {
            if (this.disposed)
            {
                return;
            }

            this.disposed = true;
            this.memory = default;
        }
    }
}
