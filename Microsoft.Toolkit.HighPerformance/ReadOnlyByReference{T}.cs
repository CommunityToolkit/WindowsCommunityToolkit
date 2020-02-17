// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

#if NETSTANDARD2_1
using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
#else
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
#endif

namespace Microsoft.Toolkit.HighPerformance
{
    /// <summary>
    /// A <see langword="struct"/> that can store a readonly reference to a value of a specified type.
    /// </summary>
    /// <typeparam name="T">The type of value to reference.</typeparam>
    [SuppressMessage("StyleCop.CSharp.OrderingRules", "SA1206", Justification = "The type is a ref struct")]
    public readonly ref struct ReadOnlyByReference<T>
    {
#if NETSTANDARD2_1
        /// <summary>
        /// The 1-length <see cref="Span{T}"/> instance used to track the target <typeparamref name="T"/> value.
        /// </summary>
        private readonly Span<T> span;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReadOnlyByReference{T}"/> struct.
        /// </summary>
        /// <param name="value">The readonly reference to the target <typeparamref name="T"/> value.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ReadOnlyByReference(in T value)
        {
            ref T r0 = ref Unsafe.AsRef(value);

            span = MemoryMarshal.CreateSpan(ref r0, 1);
        }

        /// <summary>
        /// Gets the readonly <typeparamref name="T"/> reference represented by the current <see cref="ByReference{T}"/> instance.
        /// </summary>
        public ref readonly T Value
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => ref MemoryMarshal.GetReference(span);
        }

        /// <summary>
        /// Implicitly creates a new <see cref="ReadOnlyByReference{T}"/> instance from the specified readonly reference.
        /// </summary>
        /// <param name="value">The readonly reference to the target <typeparamref name="T"/> value.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator ReadOnlyByReference<T>(in T value)
        {
            return new ReadOnlyByReference<T>(value);
        }

        /// <summary>
        /// Implicitly converts a <see cref="ByReference{T}"/> instance into a <see cref="ReadOnlyByReference{T}"/> one.
        /// </summary>
        /// <param name="reference">The input <see cref="ByReference{T}"/> instance.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator ReadOnlyByReference<T>(ByReference<T> reference)
        {
            return new ReadOnlyByReference<T>(reference.Value);
        }
#else
        /// <summary>
        /// The owner <see cref="object"/> the current instance belongs to
        /// </summary>
        private readonly object owner;

        /// <summary>
        /// The target offset within <see cref="owner"/> the current instance is pointing to
        /// </summary>
        private readonly int offset;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReadOnlyByReference{T}"/> struct.
        /// </summary>
        /// <param name="owner">The owner <see cref="object"/> to create a portable reference for.</param>
        /// <param name="offset">The target offset within <paramref name="owner"/> for the target reference.</param>
        /// <remarks>The <paramref name="offset"/> parameter is not validated, and it's responsability of the caller to ensure it's valid.</remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private ReadOnlyByReference(object owner, int offset)
        {
            this.owner = owner;
            this.offset = offset;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ReadOnlyByReference{T}"/> struct.
        /// </summary>
        /// <param name="owner">The owner <see cref="object"/> to create a portable reference for.</param>
        /// <param name="value">The target reference to point to (it must be within <paramref name="owner"/>).</param>
        /// <remarks>The <paramref name="value"/> parameter is not validated, and it's responsability of the caller to ensure it's valid.</remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ReadOnlyByReference(object owner, in T value)
        {
            this.owner = owner;

            ref T valueRef = ref Unsafe.AsRef<T>(value);
            var data = Unsafe.As<ByReference<T>.RawObjectData>(owner);
            ref byte r0 = ref data.Data;
            ref byte r1 = ref Unsafe.As<T, byte>(ref valueRef);

            offset = Unsafe.ByteOffset(ref r0, ref r1).ToInt32();
        }

        /// <summary>
        /// Gets the readonly <typeparamref name="T"/> reference represented by the current <see cref="ByReference{T}"/> instance.
        /// </summary>
        public ref readonly T Value
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                var data = Unsafe.As<ByReference<T>.RawObjectData>(owner);
                ref byte r0 = ref data.Data;
                ref byte r1 = ref Unsafe.Add(ref r0, offset);

                return ref Unsafe.As<byte, T>(ref r1);
            }
        }

        /// <summary>
        /// Implicitly converts a <see cref="ByReference{T}"/> instance into a <see cref="ReadOnlyByReference{T}"/> one.
        /// </summary>
        /// <param name="reference">The input <see cref="ByReference{T}"/> instance.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator ReadOnlyByReference<T>(ByReference<T> reference)
        {
            return new ReadOnlyByReference<T>(reference.Owner, reference.Offset);
        }
#endif

        /// <summary>
        /// Implicitly gets the <typeparamref name="T"/> value from a given <see cref="ReadOnlyByReference{T}"/> instance.
        /// </summary>
        /// <param name="reference">The input <see cref="ReadOnlyByReference{T}"/> instance.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator T(ReadOnlyByReference<T> reference)
        {
            return reference.Value;
        }
    }
}
