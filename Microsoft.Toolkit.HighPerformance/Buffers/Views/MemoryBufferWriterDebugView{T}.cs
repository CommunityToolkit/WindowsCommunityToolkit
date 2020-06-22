// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Diagnostics;

namespace Microsoft.Toolkit.HighPerformance.Buffers.Views
{
    /// <summary>
    /// A debug proxy used to display items for the <see cref="MemoryBufferWriter{T}"/> type.
    /// </summary>
    /// <typeparam name="T">The type of items stored in the input <see cref="ArrayPoolBufferWriter{T}"/> instances.</typeparam>
    internal sealed class MemoryBufferWriterDebugView<T>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MemoryBufferWriterDebugView{T}"/> class with the specified parameters.
        /// </summary>
        /// <param name="memoryBufferWriter">The input <see cref="MemoryBufferWriter{T}"/> instance with the items to display.</param>
        public MemoryBufferWriterDebugView(MemoryBufferWriter<T>? memoryBufferWriter)
        {
            this.Items = memoryBufferWriter?.WrittenSpan.ToArray();
        }

        /// <summary>
        /// Gets the items to display for the current instance
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
        public T[]? Items { get; }
    }
}
