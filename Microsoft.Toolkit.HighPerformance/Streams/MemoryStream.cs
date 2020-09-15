using System;
using System.Buffers;
using System.Diagnostics.Contracts;
using System.IO;
using System.Runtime.InteropServices;

namespace Microsoft.Toolkit.HighPerformance.Streams
{
    /// <summary>
    /// A factory class to produce <see cref="MemoryStream{TSource}"/> instances.
    /// </summary>
    internal static partial class MemoryStream
    {
        /// <summary>
        /// Creates a new <see cref="Stream"/> from the input <see cref="ReadOnlyMemory{T}"/> of <see cref="byte"/> instance.
        /// </summary>
        /// <param name="memory">The input <see cref="ReadOnlyMemory{T}"/> instance.</param>
        /// <param name="isReadOnly">Indicates whether or not <paramref name="memory"/> can be written to.</param>
        /// <returns>A <see cref="Stream"/> wrapping the underlying data for <paramref name="memory"/>.</returns>
        [Pure]
        public static Stream Create(ReadOnlyMemory<byte> memory, bool isReadOnly)
        {
            if (MemoryMarshal.TryGetArray(memory, out ArraySegment<byte> segment))
            {
                var arraySpanSource = new ArrayOwner(segment.Array!, segment.Offset, segment.Count);

                return new MemoryStream<ArrayOwner>(arraySpanSource, isReadOnly);
            }

            if (MemoryMarshal.TryGetMemoryManager<byte, MemoryManager<byte>>(memory, out var memoryManager, out int start, out int length))
            {
                MemoryManagerOwner memoryManagerSpanSource = new MemoryManagerOwner(memoryManager, start, length);

                return new MemoryStream<MemoryManagerOwner>(memoryManagerSpanSource, isReadOnly);
            }

            // Return an empty stream if the memory was empty
            return new MemoryStream<ArrayOwner>(ArrayOwner.Empty, isReadOnly);
        }

        /// <summary>
        /// Creates a new <see cref="Stream"/> from the input <see cref="IMemoryOwner{T}"/> of <see cref="byte"/> instance.
        /// </summary>
        /// <param name="memoryOwner">The input <see cref="IMemoryOwner{T}"/> instance.</param>
        /// <returns>A <see cref="Stream"/> wrapping the underlying data for <paramref name="memoryOwner"/>.</returns>
        [Pure]
        public static Stream Create(IMemoryOwner<byte> memoryOwner)
        {
            Memory<byte> memory = memoryOwner.Memory;

            if (MemoryMarshal.TryGetArray(memory, out ArraySegment<byte> segment))
            {
                var arraySpanSource = new ArrayOwner(segment.Array!, segment.Offset, segment.Count);

                return new IMemoryOwnerStream<ArrayOwner>(arraySpanSource, false, memoryOwner);
            }

            if (MemoryMarshal.TryGetMemoryManager<byte, MemoryManager<byte>>(memory, out var memoryManager, out int start, out int length))
            {
                MemoryManagerOwner memoryManagerSpanSource = new MemoryManagerOwner(memoryManager, start, length);

                return new IMemoryOwnerStream<MemoryManagerOwner>(memoryManagerSpanSource, false, memoryOwner);
            }

            // Return an empty stream if the memory was empty
            return new IMemoryOwnerStream<ArrayOwner>(ArrayOwner.Empty, false, memoryOwner);
        }
    }
}
