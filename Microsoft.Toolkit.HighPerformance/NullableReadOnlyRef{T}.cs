// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

#if NETSTANDARD2_1

using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Microsoft.Toolkit.HighPerformance
{
    /// <summary>
    /// A <see langword="struct"/> that can store an optional readonly reference to a value of a specified type.
    /// </summary>
    /// <typeparam name="T">The type of value to reference.</typeparam>
    [SuppressMessage("StyleCop.CSharp.OrderingRules", "SA1206", Justification = "The type is a ref struct")]
    public readonly ref struct NullableReadOnlyRef<T>
    {
        /// <summary>
        /// The 1-length <see cref="ReadOnlySpan{T}"/> instance used to track the target <typeparamref name="T"/> value.
        /// </summary>
        private readonly ReadOnlySpan<T> span;

        /// <summary>
        /// Initializes a new instance of the <see cref="NullableReadOnlyRef{T}"/> struct.
        /// </summary>
        /// <param name="value">The readonly reference to the target <typeparamref name="T"/> value.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public NullableReadOnlyRef(in T value)
        {
            ref T r0 = ref Unsafe.AsRef(value);

            span = MemoryMarshal.CreateReadOnlySpan(ref r0, 1);
        }

        /// <summary>
        /// Gets a value indicating whether or not the current <see cref="NullableReadOnlyRef{T}"/> instance wraps a valid reference that can be accessed.
        /// </summary>
        public bool HasValue
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                // See comment in NullableRef<T> about this
                byte length = unchecked((byte)span.Length);

                return Unsafe.As<byte, bool>(ref length);
            }
        }

        /// <summary>
        /// Gets the <typeparamref name="T"/> reference represented by the current <see cref="NullableReadOnlyRef{T}"/> instance.
        /// </summary>
        /// <exception cref="NullReferenceException">Thrown if <see cref="HasValue"/> is <see langword="false"/>.</exception>
        public ref readonly T Value
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                if (!HasValue)
                {
                    ThrowNullReferenceException();
                }

                return ref MemoryMarshal.GetReference(span);
            }
        }

        /// <summary>
        /// Throws a <see cref="NullReferenceException"/> when trying to access <see cref="Value"/> for a default instance.
        /// </summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        private static void ThrowNullReferenceException()
        {
            throw new NullReferenceException("The current instance doesn't have a value that can be accessed");
        }
    }
}

#endif
