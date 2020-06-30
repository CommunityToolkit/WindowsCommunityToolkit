// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

#if SPAN_RUNTIME_SUPPORT

using System;
using System.Runtime.CompilerServices;
#if !NETCORE_RUNTIME
using System.Runtime.InteropServices;
#endif

namespace Microsoft.Toolkit.HighPerformance
{
    /// <summary>
    /// A <see langword="struct"/> that can store an optional readonly reference to a value of a specified type.
    /// </summary>
    /// <typeparam name="T">The type of value to reference.</typeparam>
    public readonly ref struct NullableReadOnlyRef<T>
    {
#if NETCORE_RUNTIME
        /// <summary>
        /// The <see cref="ByReference{T}"/> instance holding the current reference.
        /// </summary>
        private readonly ByReference<T> byReference;

        /// <summary>
        /// Initializes a new instance of the <see cref="NullableReadOnlyRef{T}"/> struct.
        /// </summary>
        /// <param name="value">The readonly reference to the target <typeparamref name="T"/> value.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public NullableReadOnlyRef(in T value)
        {
            this.byReference = new ByReference<T>(ref Unsafe.AsRef(value));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NullableReadOnlyRef{T}"/> struct.
        /// </summary>
        /// <param name="byReference">The <see cref="ByReference{T}"/> instance holding the target reference.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private NullableReadOnlyRef(ByReference<T> byReference)
        {
            this.byReference = byReference;
        }
#else
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

            this.span = MemoryMarshal.CreateReadOnlySpan(ref r0, 1);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NullableReadOnlyRef{T}"/> struct.
        /// </summary>
        /// <param name="span">The <see cref="ReadOnlySpan{T}"/> instance to track the target <typeparamref name="T"/> reference.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private NullableReadOnlyRef(ReadOnlySpan<T> span)
        {
            this.span = span;
        }
#endif

        /// <summary>
        /// Gets a <see cref="NullableReadOnlyRef{T}"/> instance representing a <see langword="null"/> reference.
        /// </summary>
        public static NullableReadOnlyRef<T> Null
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => default;
        }

        /// <summary>
        /// Gets a value indicating whether or not the current <see cref="NullableReadOnlyRef{T}"/> instance wraps a valid reference that can be accessed.
        /// </summary>
        public bool HasValue
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
#if NETCORE_RUNTIME
                unsafe
                {
                    return !Unsafe.AreSame(ref this.byReference.Value, ref Unsafe.AsRef<T>(null));
                }
#else
                // See comment in NullableRef<T> about this
                byte length = unchecked((byte)this.span.Length);

                return Unsafe.As<byte, bool>(ref length);
#endif
            }
        }

        /// <summary>
        /// Gets the <typeparamref name="T"/> reference represented by the current <see cref="NullableReadOnlyRef{T}"/> instance.
        /// </summary>
        /// <exception cref="InvalidOperationException">Thrown if <see cref="HasValue"/> is <see langword="false"/>.</exception>
        public ref readonly T Value
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                if (!HasValue)
                {
                    ThrowInvalidOperationException();
                }

#if NETCORE_RUNTIME
                return ref this.byReference.Value;
#else
                return ref MemoryMarshal.GetReference(this.span);
#endif
            }
        }

        /// <summary>
        /// Implicitly converts a <see cref="Ref{T}"/> instance into a <see cref="NullableReadOnlyRef{T}"/> one.
        /// </summary>
        /// <param name="reference">The input <see cref="Ref{T}"/> instance.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator NullableReadOnlyRef<T>(Ref<T> reference)
        {
#if NETCORE_RUNTIME
            return new NullableReadOnlyRef<T>(reference.ByReference);
#else
            return new NullableReadOnlyRef<T>(reference.Span);
#endif
        }

        /// <summary>
        /// Implicitly converts a <see cref="ReadOnlyRef{T}"/> instance into a <see cref="NullableReadOnlyRef{T}"/> one.
        /// </summary>
        /// <param name="reference">The input <see cref="ReadOnlyRef{T}"/> instance.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator NullableReadOnlyRef<T>(ReadOnlyRef<T> reference)
        {
#if NETCORE_RUNTIME
            return new NullableReadOnlyRef<T>(reference.ByReference);
#else
            return new NullableReadOnlyRef<T>(reference.Span);
#endif
        }

        /// <summary>
        /// Implicitly converts a <see cref="NullableRef{T}"/> instance into a <see cref="NullableReadOnlyRef{T}"/> one.
        /// </summary>
        /// <param name="reference">The input <see cref="Ref{T}"/> instance.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator NullableReadOnlyRef<T>(NullableRef<T> reference)
        {
#if NETCORE_RUNTIME
            return new NullableReadOnlyRef<T>(reference.ByReference);
#else
            return new NullableReadOnlyRef<T>(reference.Span);
#endif
        }

        /// <summary>
        /// Explicitly gets the <typeparamref name="T"/> value from a given <see cref="NullableReadOnlyRef{T}"/> instance.
        /// </summary>
        /// <param name="reference">The input <see cref="NullableReadOnlyRef{T}"/> instance.</param>
        /// <exception cref="InvalidOperationException">Thrown if <see cref="HasValue"/> is <see langword="false"/>.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator T(NullableReadOnlyRef<T> reference)
        {
            return reference.Value;
        }

        /// <summary>
        /// Throws a <see cref="InvalidOperationException"/> when trying to access <see cref="Value"/> for a default instance.
        /// </summary>
        private static void ThrowInvalidOperationException()
        {
            throw new InvalidOperationException("The current instance doesn't have a value that can be accessed");
        }
    }
}

#endif
