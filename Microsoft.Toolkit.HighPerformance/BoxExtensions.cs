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
             * another class. Here we just call the Unsafe.Unbox<T>(object)
             * API, which is hidden away for users of the type for simplicity.
             * Note that this API will always actually involve a conditional
             * branch, which is introduced by the JIT compiler to validate the
             * object instance being unboxed. But since the alternative of
             * manually tracking the offset to the boxed data would be both
             * more error prone, and it would still introduce some overhead,
             * this doesn't really matter in this case anyway. */
            return ref Unsafe.Unbox<T>(box);
        }
    }
}
