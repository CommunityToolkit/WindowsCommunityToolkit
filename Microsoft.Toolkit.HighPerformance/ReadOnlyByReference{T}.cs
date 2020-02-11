using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Microsoft.Toolkit.HighPerformance
{
    /// <summary>
    /// A <see langword="struct"/> that can store a readonly reference to a value of a specified type.
    /// </summary>
    /// <typeparam name="T">The type of value to reference.</typeparam>
    [SuppressMessage("StyleCop.CSharp.OrderingRules", "SA1206", Justification = "The type is a ref struct")]
    public readonly ref struct ReadOnlyByReference<T>
    {
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
