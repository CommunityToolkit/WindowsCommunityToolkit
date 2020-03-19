// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

#nullable enable

namespace Microsoft.Toolkit.HighPerformance
{
    /// <summary>
    /// A <see langword="class"/> that represents a boxed <typeparamref name="T"/> value on the managed heap.
    /// </summary>
    /// <typeparam name="T">The type of value being bxoed.</typeparam>
    [DebuggerDisplay("{ToString(),raw}")]
    [StructLayout(LayoutKind.Sequential)]
    public sealed class Box<T>
        where T : struct
    {
        // Boxed value types in the CLR are represented in memory as simple objects that store the method table of
        // the corresponding T value type being boxed, and then the data of the value being boxed:
        // [ sync block || pMethodTable || boxed T value ]
        //                 ^               ^
        //                 |               \-- Box<T>.Value
        //                 \-- Box<T> reference
        // For more info, see: https://mattwarren.org/2017/08/02/A-look-at-the-internals-of-boxing-in-the-CLR/.

        /// <summary>
        /// The wrapped <typeparamref name="T"/> value for the current instance.
        /// </summary>
        [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401", Justification = "Public field for performance reasons")]
        public T Value;

        /// <summary>
        /// Initializes a new instance of the <see cref="Box{T}"/> class.
        /// </summary>
        /// <remarks>
        /// This constructor is never used, it is only declared in order to mark it with
        /// the <see langword="private"/> visibility modifier and prevent direct use.
        /// </remarks>
        private Box()
        {
        }

        /// <summary>
        /// Returns a <see cref="Box{T}"/> reference from the input <see cref="object"/> instance.
        /// </summary>
        /// <param name="obj">The input <see cref="object"/> instance, representing a boxed <typeparamref name="T"/> value.</param>
        /// <returns>A <see cref="Box{T}"/> reference pointing to <paramref name="obj"/>.</returns>
        [Pure]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Box<T> GetFrom(object obj)
        {
            if (obj.GetType() != typeof(T))
            {
                ThrowInvalidCastExceptionForGetFrom();
            }

            return Unsafe.As<Box<T>>(obj);
        }

        /// <summary>
        /// Returns a <see cref="Box{T}"/> reference from the input <see cref="object"/> instance.
        /// </summary>
        /// <param name="obj">The input <see cref="object"/> instance, representing a boxed <typeparamref name="T"/> value.</param>
        /// <returns>A <see cref="Box{T}"/> reference pointing to <paramref name="obj"/>.</returns>
        /// <remarks>
        /// This method doesn't check the actual type of <paramref name="obj"/>, so it is responsability of the caller
        /// to ensure it actually represents a boxed <typeparamref name="T"/> value and not some other instance.
        /// </remarks>
        [Pure]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Box<T> DangerousGetFrom(object obj)
        {
            return Unsafe.As<Box<T>>(obj);
        }

        /// <summary>
        /// Tries to get a <see cref="Box{T}"/> reference from an input <see cref="object"/> representing a boxed <typeparamref name="T"/> value.
        /// </summary>
        /// <param name="obj">The input <see cref="object"/> instance to check.</param>
        /// <param name="box">The resulting <see cref="Box{T}"/> reference, if <paramref name="obj"/> was a boxed <typeparamref name="T"/> value.</param>
        /// <returns><see langword="true"/> if a <see cref="Box{T}"/> instance was retrieved correctly, <see langword="false"/> otherwise.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1115", Justification = "Comment for [NotNullWhen] attribute")]
        public static bool TryGetFrom(
            object obj,
#if NETSTANDARD2_1
            /* On .NET Standard 2.1, we can add the [NotNullWhen] attribute
             * to let the code analysis engine know that whenever this method
             * returns true, box will always be assigned to a non-null value.
             * This will eliminate the null warnings when in a branch that
             * is only taken when this method returns true. */
            [NotNullWhen(true)]
#endif
            out Box<T>? box)
        {
            if (obj.GetType() == typeof(T))
            {
                box = Unsafe.As<Box<T>>(obj);

                return true;
            }

            box = null;

            return false;
        }

        /// <summary>
        /// Implicitly gets the <typeparamref name="T"/> value from a given <see cref="Box{T}"/> instance.
        /// </summary>
        /// <param name="box">The input <see cref="Box{T}"/> instance.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator T(Box<T> box)
        {
            return box.Value;
        }

        /// <summary>
        /// Implicitly creates a new <see cref="Box{T}"/> instance from a given <typeparamref name="T"/> value.
        /// </summary>
        /// <param name="value">The input <typeparamref name="T"/> value to wrap.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator Box<T>(T value)
        {
            /* The Box<T> type is never actually instantiated.
             * Here we are just boxing the input T value, and then reinterpreting
             * that object reference as a Box<T> reference. As such, the Box<T>
             * type is really only used as an interface to access the contents
             * of a boxed value type. This also makes it so that additional methods
             * like ToString() or GetHashCode() will automatically be referenced from
             * the method table of the boxed object, meaning that they don't need to
             * manually be implemented in the Box<T> type. For instance, boxing a float
             * and calling ToString() on it directly, on its boxed object or on a Box<T>
             * reference retrieved from it will produce the same result in all cases. */
            return Unsafe.As<Box<T>>(value);
        }

        /// <summary>
        /// Throws an <see cref="InvalidCastException"/> when a cast from an invalid <see cref="object"/> is attempted.
        /// </summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        private static void ThrowInvalidCastExceptionForGetFrom()
        {
            throw new InvalidCastException($"Can't cast the input object to the type Box<{typeof(T)}>");
        }
    }
}
