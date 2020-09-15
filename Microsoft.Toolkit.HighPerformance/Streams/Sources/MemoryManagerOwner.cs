using System;
using System.Buffers;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Microsoft.Toolkit.HighPerformance.Extensions;

namespace Microsoft.Toolkit.HighPerformance.Streams
{
    /// <summary>
    /// An <see cref="ISpanOwner"/> implementation wrapping a <see cref="MemoryManager{T}"/> of <see cref="byte"/> instance.
    /// </summary>
    internal readonly struct MemoryManagerOwner : ISpanOwner
    {
        /// <summary>
        /// The wrapped <see cref="MemoryManager{T}"/> instance.
        /// </summary>
        private readonly MemoryManager<byte> memoryManager;

        /// <summary>
        /// The starting offset within <see cref="memoryManager"/>.
        /// </summary>
        private readonly int offset;

        /// <summary>
        /// The usable length within <see cref="memoryManager"/>.
        /// </summary>
        private readonly int length;

        /// <summary>
        /// Initializes a new instance of the <see cref="MemoryManagerOwner"/> struct.
        /// </summary>
        /// <param name="memoryManager">The wrapped <see cref="MemoryManager{T}"/> instance.</param>
        /// <param name="offset">The starting offset within <paramref name="memoryManager"/>.</param>
        /// <param name="length">The usable length within <paramref name="memoryManager"/>.</param>
        public MemoryManagerOwner(MemoryManager<byte> memoryManager, int offset, int length)
        {
            this.memoryManager = memoryManager;
            this.offset = offset;
            this.length = length;
        }

        /// <inheritdoc/>
        public int Length
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => this.length;
        }

        /// <inheritdoc/>
        public Span<byte> Span
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
#if SPAN_RUNTIME_SUPPORT
                ref byte r0 = ref this.memoryManager.GetSpan().DangerousGetReferenceAt(this.offset);

                return MemoryMarshal.CreateSpan(ref r0, this.length);
#else
                return this.memoryManager.GetSpan();
#endif
            }
        }
    }
}
