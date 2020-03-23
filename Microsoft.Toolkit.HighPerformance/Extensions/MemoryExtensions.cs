// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Diagnostics.Contracts;
using System.IO;
using System.Runtime.CompilerServices;
using MemoryStream = Microsoft.Toolkit.HighPerformance.Streams.MemoryStream;

#nullable enable

namespace Microsoft.Toolkit.HighPerformance.Extensions
{
    /// <summary>
    /// Helpers for working with the <see cref="Memory{T}"/> type.
    /// </summary>
    public static class MemoryExtensions
    {
        /// <summary>
        /// Returns a <see cref="Stream"/> wrapping the contents of the given <see cref="Memory{T}"/> instance.
        /// </summary>
        /// <param name="memory">The input <see cref="Memory{T}"/> instance.</param>
        /// <returns>A <see cref="Stream"/> wrapping the data within <paramref name="memory"/>.</returns>
        [Pure]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Stream AsStream(this Memory<byte> memory)
        {
            return new MemoryStream(memory);
        }
    }
}
