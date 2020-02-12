// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

#if NETSTANDARD2_0
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
#else
using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
#endif

namespace Microsoft.Toolkit.HighPerformance
{
    /// <summary>
    /// A <see langword="struct"/> that can store a reference to a value of a specified type.
    /// </summary>
    /// <typeparam name="T">The type of value to reference.</typeparam>
    [SuppressMessage("StyleCop.CSharp.OrderingRules", "SA1206", Justification = "The type is a ref struct")]
    public readonly ref struct ByReference<T>
    {
#if NETSTANDARD2_0
        /// <summary>
        /// The owner <see cref="object"/> the current instance belongs to
        /// </summary>
        internal readonly object Owner;

        /// <summary>
        /// The target offset within <see cref="Owner"/> the current instance is pointing to
        /// </summary>
        internal readonly int Offset;

        /// <summary>
        /// Initializes a new instance of the <see cref="ByReference{T}"/> struct.
        /// </summary>
        /// <param name="owner">The owner <see cref="object"/> to create a portable reference for.</param>
        /// <param name="value">The target reference to point to (it must be within <paramref name="owner"/>).</param>
        /// <remarks>The <paramref name="value"/> parameter is not validated, and it's responsability of the caller to ensure it's valid.</remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ByReference(object owner, ref T value)
        {
            Owner = owner;

            var data = Unsafe.As<RawObjectData>(owner);
            ref byte r0 = ref data.Data;
            ref byte r1 = ref Unsafe.As<T, byte>(ref value);

            Offset = Unsafe.ByteOffset(ref r0, ref r1).ToInt32();
        }

        /// <summary>
        /// Gets the <typeparamref name="T"/> reference represented by the current <see cref="ByReference{T}"/> instance.
        /// </summary>
        public ref T Value
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                var data = Unsafe.As<RawObjectData>(Owner);
                ref byte r0 = ref data.Data;
                ref byte r1 = ref Unsafe.Add(ref r0, Offset);

                return ref Unsafe.As<byte, T>(ref r1);
            }
        }

        // Description adapted from CoreCLR: see https://source.dot.net/#System.Private.CoreLib/src/System/Runtime/CompilerServices/RuntimeHelpers.CoreCLR.cs,285.
        // CLR objects are laid out in memory as follows:
        // [ sync block || pMethodTable || raw data .. ]
        //                 ^               ^
        //                 |               \-- ref Unsafe.As<RawObjectData>(owner).Data
        //                 \-- object
        // The reference to RawObjectData.Data points to the first data byte in the
        // target object, skipping over the sync block, method table and string length.
        internal sealed class RawObjectData
        {
#pragma warning disable CS0649 // Unassigned fields
#pragma warning disable SA1401 // Fields should be private
            public byte Data;
#pragma warning restore CS0649
#pragma warning restore SA1401
        }
#else
        /// <summary>
        /// The 1-length <see cref="Span{T}"/> instance used to track the target <typeparamref name="T"/> value.
        /// </summary>
        private readonly Span<T> span;

        /// <summary>
        /// Initializes a new instance of the <see cref="ByReference{T}"/> struct.
        /// </summary>
        /// <param name="value">The reference to the target <typeparamref name="T"/> value.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ByReference(ref T value)
        {
            span = MemoryMarshal.CreateSpan(ref value, 1);
        }

        /// <summary>
        /// Gets the <typeparamref name="T"/> reference represented by the current <see cref="ByReference{T}"/> instance.
        /// </summary>
        public ref T Value
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => ref MemoryMarshal.GetReference(span);
        }

        /// <summary>
        /// Implicitly creates a new <see cref="ByReference{T}"/> instance from the specified readonly reference.
        /// </summary>
        /// <param name="value">The readonly reference to the target <typeparamref name="T"/> value.</param>
        /// <remarks>This operator converts a readonly reference in a mutable one, so make sure that's the intended behavior.</remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator ByReference<T>(in T value)
        {
            ref T r0 = ref Unsafe.AsRef(value);

            return new ByReference<T>(ref r0);
        }
#endif

        /// <summary>
        /// Implicitly gets the <typeparamref name="T"/> value from a given <see cref="ByReference{T}"/> instance.
        /// </summary>
        /// <param name="reference">The input <see cref="ByReference{T}"/> instance.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator T(ByReference<T> reference)
        {
            return reference.Value;
        }
    }
}
