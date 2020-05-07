﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Runtime.CompilerServices;
using Microsoft.Toolkit.HighPerformance.Helpers;

#nullable enable

namespace Microsoft.Toolkit.HighPerformance.Extensions
{
    /// <summary>
    /// Helpers for working with the <see cref="HashCode"/> type.
    /// </summary>
    public static class HashCodeExtensions
    {
        /// <summary>
        /// Adds a sequence of <typeparamref name="T"/> values to the hash code.
        /// </summary>
        /// <typeparam name="T">The type of elements in the input <see cref="ReadOnlySpan{T}"/> instance.</typeparam>
        /// <param name="hashCode">The input <see cref="HashCode"/> instance.</param>
        /// <param name="span">The input <see cref="ReadOnlySpan{T}"/> instance.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Add<T>(ref this HashCode hashCode, ReadOnlySpan<T> span)
#if NETSTANDARD2_1
            where T : notnull
#else
            // Same type constraints as HashCode<T>, see comments there
            where T : unmanaged
#endif
        {
            int hash = HashCode<T>.CombineValues(span);

            hashCode.Add(hash);
        }
    }
}
