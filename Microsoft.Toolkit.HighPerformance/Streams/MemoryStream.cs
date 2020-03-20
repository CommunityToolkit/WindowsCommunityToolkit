// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

#nullable enable

namespace Microsoft.Toolkit.HighPerformance.Streams
{
    /// <summary>
    /// A <see cref="Stream"/> implementation wrapping a <see cref="Memory{T}"/> or <see cref="ReadOnlyMemory{T}"/> instance.
    /// </summary>
    internal sealed partial class MemoryStream : Stream
    {
        /// <summary>
        /// The <see cref="Memory{T}"/> instance currently in use.
        /// </summary>
        private readonly Memory<byte> memory;

        /// <summary>
        /// The current position within <see cref="memory"/>.
        /// </summary>
        private int position;

        /// <summary>
        /// Initializes a new instance of the <see cref="MemoryStream"/> class.
        /// </summary>
        /// <param name="memory">The input <see cref="Memory{T}"/> instance to use.</param>
        public MemoryStream(Memory<byte> memory)
        {
            this.memory = memory;
            this.position = 0;
            this.CanWrite = true;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MemoryStream"/> class.
        /// </summary>
        /// <param name="memory">The input <see cref="ReadOnlyMemory{T}"/> instance to use.</param>
        public MemoryStream(ReadOnlyMemory<byte> memory)
        {
            this.memory = MemoryMarshal.AsMemory(memory);
            this.position = 0;
            this.CanWrite = false;
        }

        /// <inheritdoc/>
        public override bool CanRead => true;

        /// <inheritdoc/>
        public override bool CanSeek => true;

        /// <inheritdoc/>
        public override bool CanWrite
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get;
        }

        /// <inheritdoc/>
        public override long Length
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => this.memory.Length;
        }

        /// <inheritdoc/>
        public override long Position
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => this.position;

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set
            {
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
            ValidateBuffer(buffer, offset, count);

            Span<byte> source = this.memory.Span.Slice(this.position);

            int bytesCopied = Math.Min(source.Length, count);

            Span<byte> destination = buffer.AsSpan(offset, bytesCopied);

            source.CopyTo(destination);

            this.position += bytesCopied;

            return bytesCopied;
        }

#if NETSTANDARD2_1
        /// <inheritdoc/>
        public override int Read(Span<byte> buffer)
        {
            Span<byte> source = this.memory.Span.Slice(this.position);

            int bytesCopied = Math.Min(source.Length, buffer.Length);

            Span<byte> destination = buffer.Slice(0, bytesCopied);

            source.CopyTo(destination);

            this.position += bytesCopied;

            return bytesCopied;
        }
#endif

        /// <inheritdoc/>
        public override long Seek(long offset, SeekOrigin origin)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override void SetLength(long value)
        {
            ThrowNotSupportedExceptionForSetLength();
        }

        /// <inheritdoc/>
        public override void Write(byte[] buffer, int offset, int count)
        {
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

#if NETSTANDARD2_1
        /// <inheritdoc/>
        public override void Write(ReadOnlySpan<byte> buffer)
        {
            ValidateCanWrite();

            Span<byte> destination = this.memory.Span.Slice(this.position);

            if (!buffer.TryCopyTo(destination))
            {
                ThrowArgumentExceptionForEndOfStreamOnWrite();
            }

            this.position += buffer.Length;
        }
#endif
    }
}
