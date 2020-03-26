// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;

namespace Microsoft.Toolkit.HighPerformance
{
    /// <summary>
    /// Helpers for working with the <see cref="Box{T}"/> type.
    /// </summary>
    public static class BoxExtensions
    {
        /// <summary>
        /// Gets a <typeparamref name="T"/> reference from a <see cref="Box{T}"/> instance.
        /// </summary>
        /// <typeparam name="T">The type of reference to retrieve.</typeparam>
        /// <param name="box">The input <see cref="Box{T}"/> instance.</param>
        /// <returns>A <typeparamref name="T"/> reference to the boxed value within <paramref name="box"/>.</returns>
        [Pure]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ref T GetReference<T>(this Box<T> box)
            where T : struct
        {
            /* The reason why this method is an extension and is not part of
             * the Box<T> type itself is that Box<T> is really just a mask
             * used over object references, but it is never actually instantiated.
             * Because of this, the method table of the objects in the heap will
             * be the one of type T created by the runtime, and not the one of
             * the Box<T> type, so we wouldn't be able to call instance Box<T>
             * methods anyway. To work around this, we use an extension method,
             * which is just syntactic sugar for a static method belonging to
             * another class. Here we just reuse our mapping type to get a
             * reference to the first byte of the object data, and then add
             * the precomputed byte offset to get to the start of the actual
             * boxed type. Then we just use Unsafe.As to cast to the right type. */
            var rawObj = Unsafe.As<RawObjectData>(box);
            ref byte r0 = ref rawObj.Data;
            ref byte r1 = ref Unsafe.AddByteOffset(ref r0, Box<T>.DataOffset);
            ref T r2 = ref Unsafe.As<byte, T>(ref r1);

            return ref r2;
        }
    }
}
