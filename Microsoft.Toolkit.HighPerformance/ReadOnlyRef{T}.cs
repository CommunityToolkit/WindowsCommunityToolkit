// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;
#if NETCORE_RUNTIME
#elif SPAN_RUNTIME_SUPPORT
using System.Runtime.InteropServices;
#else
using Microsoft.Toolkit.HighPerformance.Extensions;
#endif

namespace Microsoft.Toolkit.HighPerformance
{
    /// <summary>
    /// A <see langword="struct"/> that can store a readonly reference to a value of a specified type.
    /// </summary>
    /// <typeparam name="T">The type of value to reference.</typeparam>
    public readonly ref struct ReadOnlyRef<T>
    {
#if NETCORE_RUNTIME
        /// <summary>
        /// The <see cref="ByReference{T}"/> instance holding the current reference.
        /// </summary>
        internal readonly ByReference<T> ByReference;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReadOnlyRef{T}"/> struct.
        /// </summary>
        /// <param name="value">The reference to the target <typeparamref name="T"/> value.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ReadOnlyRef(in T value)
        {
            ByReference = new ByReference<T>(ref Unsafe.AsRef(value));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ReadOnlyRef{T}"/> struct.
        /// </summary>
        /// <param name="byReference">The input <see cref="ByReference{T}"/> to the target <typeparamref name="T"/> value.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private ReadOnlyRef(ByReference<T> byReference)
        {
            ByReference = byReference;
        }
#elif SPAN_RUNTIME_SUPPORT
        /// <summary>
        /// The 1-length <see cref="ReadOnlySpan{T}"/> instance used to track the target <typeparamref name="T"/> value.
        /// </summary>
        internal readonly ReadOnlySpan<T> Span;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReadOnlyRef{T}"/> struct.
        /// </summary>
        /// <param name="value">The readonly reference to the target <typeparamref name="T"/> value.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ReadOnlyRef(in T value)
        {
            ref T r0 = ref Unsafe.AsRef(value);

            Span = MemoryMarshal.CreateReadOnlySpan(ref r0, 1);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ReadOnlyRef{T}"/> struct.
        /// </summary>
        /// <param name="span">The input <see cref="ReadOnlySpan{T}"/> targeting the current value.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private ReadOnlyRef(ReadOnlySpan<T> span)
        {
            Span = span;
        }
#else
        /// <summary>
        /// The owner <see cref="object"/> the current instance belongs to
        /// </summary>
        private readonly object owner;

        /// <summary>
        /// The target offset within <see cref="owner"/> the current instance is pointing to
        /// </summary>
        private readonly IntPtr offset;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReadOnlyRef{T}"/> struct.
        /// </summary>
        /// <param name="owner">The owner <see cref="object"/> to create a portable reference for.</param>
        /// <param name="offset">The target offset within <paramref name="owner"/> for the target reference.</param>
        /// <remarks>The <paramref name="offset"/> parameter is not validated, and it's responsability of the caller to ensure it's valid.</remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private ReadOnlyRef(object owner, IntPtr offset)
        {
            this.owner = owner;
            this.offset = offset;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ReadOnlyRef{T}"/> struct.
        /// </summary>
        /// <param name="owner">The owner <see cref="object"/> to create a portable reference for.</param>
        /// <param name="value">The target reference to point to (it must be within <paramref name="owner"/>).</param>
        /// <remarks>The <paramref name="value"/> parameter is not validated, and it's responsability of the caller to ensure it's valid.</remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ReadOnlyRef(object owner, in T value)
        {
            this.owner = owner;
            this.offset = owner.DangerousGetObjectDataByteOffset(ref Unsafe.AsRef(value));
        }
#endif

        /// <summary>
        /// Gets the <typeparamref name="T"/> reference represented by the current <see cref="ReadOnlyRef{T}"/> instance.
        /// </summary>
        public ref readonly T Value
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
#if NETCORE_RUNTIME
                return ref this.ByReference.Value;
#elif SPAN_RUNTIME_SUPPORT
                return ref MemoryMarshal.GetReference(Span);
#else
                return ref this.owner.DangerousGetObjectDataReferenceAt<T>(this.offset);
#endif
            }
        }

        /// <summary>
        /// Returns a mutable reference with the same target as <see cref="Value"/>.
        /// </summary>
        /// <returns>A mutable reference equivalent to the one returned by <see cref="Value"/>.</returns>
        [Pure]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ref T DangerousGetReference()
        {
            return ref Unsafe.AsRef(Value);
        }

        /// <summary>
        /// Returns a reference to an element at a specified offset with respect to <see cref="Value"/>.
        /// </summary>
        /// <param name="offset">The offset of the element to retrieve, starting from the reference provided by <see cref="Value"/>.</param>
        /// <returns>A reference to the element at the specified offset from <see cref="Value"/>.</returns>
        /// <remarks>
        /// This method offers a layer of abstraction over <see cref="Unsafe.Add{T}(ref T,int)"/>, and similarly it does not does not do
        /// any kind of input validation. It is responsability of the caller to ensure the supplied offset is valid.
        /// </remarks>
        [Pure]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ref T DangerousGetReferenceAt(int offset)
        {
#if NETCORE_RUNTIME
            return ref Unsafe.Add(ref ByReference.Value, offset);
#elif SPAN_RUNTIME_SUPPORT
            return ref Unsafe.Add(ref MemoryMarshal.GetReference(Span), offset);
#else
            return ref this.owner.DangerousGetObjectDataReferenceAt<T>(this.offset + offset);
#endif
        }

        /// <summary>
        /// Returns a reference to an element at a specified offset with respect to <see cref="Value"/>.
        /// </summary>
        /// <param name="offset">The offset of the element to retrieve, starting from the reference provided by <see cref="Value"/>.</param>
        /// <returns>A reference to the element at the specified offset from <see cref="Value"/>.</returns>
        /// <remarks>
        /// This method offers a layer of abstraction over <see cref="Unsafe.Add{T}(ref T,IntPtr)"/>, and similarly it does not does not do
        /// any kind of input validation. It is responsability of the caller to ensure the supplied offset is valid.
        /// </remarks>
        [Pure]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ref T DangerousGetReferenceAt(IntPtr offset)
        {
#if NETCORE_RUNTIME
            return ref Unsafe.Add(ref ByReference.Value, offset);
#elif SPAN_RUNTIME_SUPPORT
            return ref Unsafe.Add(ref MemoryMarshal.GetReference(Span), offset);
#else
            unsafe
            {
                if (sizeof(IntPtr) == sizeof(long))
                {
                    return ref this.owner.DangerousGetObjectDataReferenceAt<T>((IntPtr)((long)(byte*)this.offset + (long)(byte*)offset));
                }

                return ref this.owner.DangerousGetObjectDataReferenceAt<T>((IntPtr)((int)(byte*)this.offset + (int)(byte*)offset));
            }
#endif
        }

        /// <summary>
        /// Implicitly converts a <see cref="Ref{T}"/> instance into a <see cref="ReadOnlyRef{T}"/> one.
        /// </summary>
        /// <param name="reference">The input <see cref="Ref{T}"/> instance.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator ReadOnlyRef<T>(Ref<T> reference)
        {
#if NETCORE_RUNTIME
            return new ReadOnlyRef<T>(reference.ByReference);
#elif SPAN_RUNTIME_SUPPORT
            return new ReadOnlyRef<T>(reference.Span);
#else
            return new ReadOnlyRef<T>(reference.Owner, reference.Offset);
#endif
        }

        /// <summary>
        /// Implicitly gets the <typeparamref name="T"/> value from a given <see cref="ReadOnlyRef{T}"/> instance.
        /// </summary>
        /// <param name="reference">The input <see cref="ReadOnlyRef{T}"/> instance.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator T(ReadOnlyRef<T> reference)
        {
            return reference.Value;
        }
    }
}
