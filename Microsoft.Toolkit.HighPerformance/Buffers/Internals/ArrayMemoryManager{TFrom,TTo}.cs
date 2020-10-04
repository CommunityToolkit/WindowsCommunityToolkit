using System;
using System.Buffers;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Microsoft.Toolkit.HighPerformance.Buffers.Internals.Interfaces;
using Microsoft.Toolkit.HighPerformance.Extensions;

namespace Microsoft.Toolkit.HighPerformance.Buffers.Internals
{
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
}
