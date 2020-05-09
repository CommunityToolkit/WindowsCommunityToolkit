// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;

#nullable enable

namespace Microsoft.Toolkit.HighPerformance.Extensions
{
    /// <summary>
    /// Helpers for working with <see cref="object"/> instances.
    /// </summary>
    public static class ObjectExtensions
    {
        /// <summary>
        /// Tries to get a boxed <typeparamref name="T"/> value from an input <see cref="object"/> instance.
        /// </summary>
        /// <typeparam name="T">The type of value to try to unbox.</typeparam>
        /// <param name="obj">The input <see cref="object"/> instance to check.</param>
        /// <param name="value">The resulting <typeparamref name="T"/> value, if <paramref name="obj"/> was in fact a boxed <typeparamref name="T"/> value.</param>
        /// <returns><see langword="true"/> if a <typeparamref name="T"/> value was retrieved correctly, <see langword="false"/> otherwise.</returns>
        /// <remarks>
        /// This extension behaves just like the following method:
        /// <code>
        /// public static bool TryUnbox&lt;T>(this object obj, out T value)
        /// {
        ///     if (obj is T)
        ///     {
        ///         value = (T)obj;
        ///
        ///         return true;
        ///     }
        ///
        ///     value = default;
        ///
        ///     return false;
        /// }
        /// </code>
        /// But in a more efficient way, and with the ability to also assign the unboxed value
        /// directly on an existing T variable, which is not possible with the code above.
        /// </remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryUnbox<T>(this object obj, out T value)
            where T : struct
        {
            if (obj.GetType() == typeof(T))
            {
                value = Unsafe.Unbox<T>(obj);

                return true;
            }

            value = default;

            return false;
        }

        /// <summary>
        /// Unboxes a <typeparamref name="T"/> value from an input <see cref="object"/> instance.
        /// </summary>
        /// <typeparam name="T">The type of value to unbox.</typeparam>
        /// <param name="obj">The input <see cref="object"/> instance, representing a boxed <typeparamref name="T"/> value.</param>
        /// <returns>The <typeparamref name="T"/> value boxed in <paramref name="obj"/>.</returns>
        /// <remarks>
        /// This method doesn't check the actual type of <paramref name="obj"/>, so it is responsability of the caller
        /// to ensure it actually represents a boxed <typeparamref name="T"/> value and not some other instance.
        /// </remarks>
        [Pure]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ref T DangerousUnbox<T>(this object obj)
            where T : struct
        {
            return ref Unsafe.Unbox<T>(obj);
        }
    }
}
