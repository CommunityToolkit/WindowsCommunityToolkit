// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Diagnostics.Contracts;
using System.IO;
using System.Runtime.CompilerServices;
using MemoryStream = Microsoft.Toolkit.HighPerformance.Streams.MemoryStream;

namespace Microsoft.Toolkit.HighPerformance.Extensions
{
    /// <summary>
    /// Helpers for working with the <see cref="Memory{T}"/> type.
    /// </summary>
    public static class MemoryExtensions
    {
        /// <summary>
        /// Returns a <see cref="Stream"/> wrapping the contents of the given <see cref="Memory{T}"/> of <see cref="byte"/> instance.
        /// </summary>
        /// <param name="memory">The input <see cref="Memory{T}"/> of <see cref="byte"/> instance.</param>
        /// <returns>A <see cref="Stream"/> wrapping the data within <paramref name="memory"/>.</returns>
        /// <remarks>
        /// Since this method only receives a <see cref="Memory{T}"/> instance, which does not track
        /// the lifetime of its underlying buffer, it is responsability of the caller to manage that.
        /// In particular, the caller must ensure that the target buffer is not disposed as long
        /// as the returned <see cref="Stream"/> is in use, to avoid unexpected issues.
        /// </remarks>
        [Pure]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Stream AsStream(this Memory<byte> memory)
        {
            return new MemoryStream(memory);
        }
    }
}
