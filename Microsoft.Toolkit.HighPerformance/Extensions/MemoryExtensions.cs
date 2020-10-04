// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Buffers;
using System.Diagnostics.Contracts;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using MemoryStream = Microsoft.Toolkit.HighPerformance.Streams.MemoryStream;

namespace Microsoft.Toolkit.HighPerformance.Extensions
{
    /// <summary>
    /// Helpers for working with the <see cref="Memory{T}"/> type.
    /// </summary>
    public static class MemoryExtensions
    {
        /// <summary>
        /// Casts a <see cref="Span{T}"/> of one primitive type <typeparamref name="TFrom"/> to another primitive type <typeparamref name="TTo"/>.
        /// These types may not contain pointers or references. This is checked at runtime in order to preserve type safety.
        /// </summary>
        /// <typeparam name="TFrom">The type of items in the source <see cref="Span{T}"/>.</typeparam>
        /// <typeparam name="TTo">The type of items in the destination <see cref="Span{T}"/>.</typeparam>
        /// <param name="memory">The source slice, of type <typeparamref name="TFrom"/>.</param>
        /// <returns>A <see cref="Span{T}"/> of type <typeparamref name="TTo"/></returns>
        /// <remarks>
        /// Supported only for platforms that support misaligned memory access or when the memory block is aligned by other means.
        /// </remarks>
        [Pure]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Memory<TTo> Cast<TFrom, TTo>(this Memory<TFrom> memory)
            where TFrom : unmanaged
            where TTo : unmanaged
        {
            if (memory.IsEmpty)
            {
                return default;
            }

            if (MemoryMarshal.TryGetArray(memory, out ArraySegment<TFrom> segment))
            {
                return new ArrayMemoryManager<TFrom, TTo>(segment.Array!, segment.Offset, segment.Count).Memory;
            }

            if (MemoryMarshal.TryGetMemoryManager<TFrom, MemoryManager<TFrom>>(memory, out var memoryManager, out int start, out int length))
            {
                // If the memory manager is the one resulting from a previous cast, we can use it directly to retrieve
                // a new manager for the target type that wraps the original data store, instead of creating one that
                // wraps the current manager. This ensures that doing repeated casts always results in only up to one
                // indirection level in the chain of memory managers needed to access the target data buffer to use.
                if (memoryManager is IMemoryManager wrappingManager)
                {
                    return wrappingManager.Cast<TTo>(start, length).Memory;
                }

                return new ProxyMemoryManager<TFrom, TTo>(memoryManager, start, length).Memory;
            }

            // Throws when the memory instance has an unsupported backing store (eg. a string)
            static Memory<TTo> ThrowArgumentExceptionForUnsupportedMemory()
            {
                throw new ArgumentException("The input instance doesn't have a supported underlying data store.");
            }

            return ThrowArgumentExceptionForUnsupportedMemory();
        }

        /// <summary>
        /// Returns a <see cref="Stream"/> wrapping the contents of the given <see cref="Memory{T}"/> of <see cref="byte"/> instance.
        /// </summary>
        /// <param name="memory">The input <see cref="Memory{T}"/> of <see cref="byte"/> instance.</param>
        /// <returns>A <see cref="Stream"/> wrapping the data within <paramref name="memory"/>.</returns>
        /// <remarks>
        /// Since this method only receives a <see cref="Memory{T}"/> instance, which does not track
        /// the lifetime of its underlying buffer, it is responsibility of the caller to manage that.
        /// In particular, the caller must ensure that the target buffer is not disposed as long
        /// as the returned <see cref="Stream"/> is in use, to avoid unexpected issues.
        /// </remarks>
        [Pure]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Stream AsStream(this Memory<byte> memory)
        {
            return new MemoryStream(memory);
        }

        /// <summary>
        /// An interface for a <see cref="MemoryManager{T}"/> instance that can reinterpret its underlying data.
        /// </summary>
        internal interface IMemoryManager
        {
            /// <summary>
            /// Creates a new <see cref="MemoryManager{T}"/> that reinterprets the underlying data for the current instance.
            /// </summary>
            /// <typeparam name="T">The target type to cast the items to.</typeparam>
            /// <param name="offset">The starting offset within the data store.</param>
            /// <param name="length">The original used length for the data store.</param>
            /// <returns>A new <see cref="MemoryManager{T}"/> instance of the specified type, reinterpreting the current items.</returns>
            MemoryManager<T> Cast<T>(int offset, int length)
                where T : unmanaged;
        }

        /// <summary>
        /// A custom <see cref="MemoryManager{T}"/> that casts data from a <typeparamref name="TFrom"/> array, to <typeparamref name="TTo"/> values.
        /// </summary>
        /// <typeparam name="TFrom">The source type of items to read.</typeparam>
        /// <typeparam name="TTo">The target type to cast the source items to.</typeparam>
        internal sealed class ArrayMemoryManager<TFrom, TTo> : MemoryManager<TTo>, IMemoryManager
            where TFrom : unmanaged
            where TTo : unmanaged
        {
            /// <summary>
            /// The source <typeparamref name="TFrom"/> array to read data from.
            /// </summary>
            private readonly TFrom[] array;

            /// <summary>
            /// The starting offset within <see name="array"/>.
            /// </summary>
            private readonly int offset;

            /// <summary>
            /// The original used length for <see name="array"/>.
            /// </summary>
            private readonly int length;

            /// <summary>
            /// Initializes a new instance of the <see cref="ArrayMemoryManager{TFrom, TTo}"/> class.
            /// </summary>
            /// <param name="array">The source <typeparamref name="TFrom"/> array to read data from.</param>
            /// <param name="offset">The starting offset within <paramref name="array"/>.</param>
            /// <param name="length">The original used length for <paramref name="array"/>.</param>
            public ArrayMemoryManager(TFrom[] array, int offset, int length)
            {
                this.array = array;
                this.offset = offset;
                this.length = length;
            }

            /// <inheritdoc/>
            public override Span<TTo> GetSpan()
            {
#if SPAN_RUNTIME_SUPPORT
                ref TFrom r0 = ref this.array.DangerousGetReferenceAt(this.offset);

                Span<TFrom> span = MemoryMarshal.CreateSpan(ref r0, this.length);
#else
                Span<TFrom> span = this.array.AsSpan(this.offset, this.length);
#endif

                // We rely on MemoryMarshal.Cast here to deal with calculating the effective
                // size of the new span to return. This will also make the behavior consistent
                // for users that are both using this type as well as casting spans directly.
                return MemoryMarshal.Cast<TFrom, TTo>(span);
            }

            /// <inheritdoc/>
            public override unsafe MemoryHandle Pin(int elementIndex = 0)
            {
                int
                    bytePrefix = this.offset + Unsafe.SizeOf<TFrom>(),
                    byteSuffix = elementIndex * Unsafe.SizeOf<TTo>(),
                    byteOffset = bytePrefix + byteSuffix;

                GCHandle handle = GCHandle.Alloc(this.array, GCHandleType.Pinned);

                ref TFrom r0 = ref this.array.DangerousGetReference();
                ref byte r1 = ref Unsafe.As<TFrom, byte>(ref r0);
                ref byte r2 = ref Unsafe.Add(ref r1, byteOffset);
                void* pi = Unsafe.AsPointer(ref r2);

                return new MemoryHandle(pi, handle);
            }

            /// <inheritdoc/>
            public override void Unpin()
            {
            }

            /// <inheritdoc/>
            protected override void Dispose(bool disposing)
            {
            }

            /// <inheritdoc/>
            public MemoryManager<T> Cast<T>(int offset, int length)
                where T : unmanaged
            {
                return new ArrayMemoryManager<TFrom, T>(this.array, this.offset + offset, length);
            }
        }

        /// <summary>
        /// A custom <see cref="MemoryManager{T}"/> that casts data from a <see cref="MemoryManager{T}"/> of <typeparamref name="TFrom"/>, to <typeparamref name="TTo"/> values.
        /// </summary>
        /// <typeparam name="TFrom">The source type of items to read.</typeparam>
        /// <typeparam name="TTo">The target type to cast the source items to.</typeparam>
        internal sealed class ProxyMemoryManager<TFrom, TTo> : MemoryManager<TTo>, IMemoryManager
            where TFrom : unmanaged
            where TTo : unmanaged
        {
            /// <summary>
            /// The source <see cref="MemoryManager{T}"/> to read data from.
            /// </summary>
            private readonly MemoryManager<TFrom> memoryManager;

            /// <summary>
            /// The starting offset within <see name="memoryManager"/>.
            /// </summary>
            private readonly int offset;

            /// <summary>
            /// The original used length for <see name="memoryManager"/>.
            /// </summary>
            private readonly int length;

            /// <summary>
            /// Initializes a new instance of the <see cref="ProxyMemoryManager{TFrom, TTo}"/> class.
            /// </summary>
            /// <param name="memoryManager">The source <see cref="MemoryManager{T}"/> to read data from.</param>
            /// <param name="offset">The starting offset within <paramref name="memoryManager"/>.</param>
            /// <param name="length">The original used length for <paramref name="memoryManager"/>.</param>
            public ProxyMemoryManager(MemoryManager<TFrom> memoryManager, int offset, int length)
            {
                this.memoryManager = memoryManager;
                this.offset = offset;
                this.length = length;
            }

            /// <inheritdoc/>
            public override Span<TTo> GetSpan()
            {
                Span<TFrom> span = this.memoryManager.GetSpan().Slice(this.offset, this.length);

                return MemoryMarshal.Cast<TFrom, TTo>(span);
            }

            /// <inheritdoc/>
            public override MemoryHandle Pin(int elementIndex = 0)
            {
                int byteOffset = elementIndex * Unsafe.SizeOf<TTo>();

#if NETSTANDARD1_4
                int
                    shiftedOffset = byteOffset / Unsafe.SizeOf<TFrom>(),
                    remainder = byteOffset - (shiftedOffset * Unsafe.SizeOf<TFrom>());
#else
                int shiftedOffset = Math.DivRem(byteOffset, Unsafe.SizeOf<TFrom>(), out int remainder);
#endif

                if (remainder != 0)
                {
                    ThrowArgumentExceptionForInvalidAlignment();
                }

                return this.memoryManager.Pin(this.length + shiftedOffset);
            }

            /// <inheritdoc/>
            public override void Unpin()
            {
                this.memoryManager.Unpin();
            }

            /// <inheritdoc/>
            protected override void Dispose(bool disposing)
            {
                ((IDisposable)this.memoryManager).Dispose();
            }

            /// <inheritdoc/>
            public MemoryManager<T> Cast<T>(int offset, int length)
                where T : unmanaged
            {
                return new ProxyMemoryManager<TFrom, T>(this.memoryManager, this.offset + offset, length);
            }

            /// <summary>
            /// Throws an <see cref="ArgumentOutOfRangeException"/> when <see cref="Pin"/> receives an invalid target index.
            /// </summary>
            private static void ThrowArgumentExceptionForInvalidAlignment()
            {
                throw new ArgumentOutOfRangeException("elementIndex", "The input index doesn't result in an aligned item access");
            }
        }
    }
}
