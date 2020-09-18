// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Buffers;
using System.IO;

namespace Microsoft.Toolkit.HighPerformance.Streams
{
    /// <summary>
    /// A <see cref="Stream"/> implementation wrapping an <see cref="IMemoryOwner{T}"/> of <see cref="byte"/> instance.
    /// </summary>
    internal sealed class IMemoryOwnerStream : MemoryStream
    {
        /// <summary>
        /// The <see cref="IMemoryOwner{T}"/> of <see cref="byte"/> instance currently in use.
        /// </summary>
        private readonly IMemoryOwner<byte> memory;

        /// <summary>
        /// Initializes a new instance of the <see cref="IMemoryOwnerStream"/> class.
        /// </summary>
        /// <param name="memory">The input <see cref="IMemoryOwner{T}"/> of <see cref="byte"/> instance to use.</param>
        public IMemoryOwnerStream(IMemoryOwner<byte> memory)
            : base(memory.Memory)
        {
            this.memory = memory;
        }

        /// <inheritdoc/>
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            this.memory.Dispose();
        }
    }
}
