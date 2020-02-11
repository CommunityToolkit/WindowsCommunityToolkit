using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Microsoft.Toolkit.HighPerformance
{
    /// <summary>
    /// A <see langword="struct"/> that can store a reference to a value of a specified type
    /// </summary>
    /// <typeparam name="T">The type of value to reference</typeparam>
    public readonly ref struct ByReference<T>
    {
        /// <summary>
        /// The 1-length <see cref="Span{T}"/> instance used to track the target <typeparamref name="T"/> value
        /// </summary>
        private readonly Span<T> span;

        /// <summary>
        /// Initializes a new instance of the <see cref="ByReference{T}"/> struct
        /// </summary>
        /// <param name="value">The reference to the target <typeparamref name="T"/> value</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ByReference(ref T value)
        {
            span = MemoryMarshal.CreateSpan(ref value, 1);
        }

        /// <summary>
        /// Gets the <typeparamref name="T"/> reference represented by the current <see cref="ByReference{T}"/> instance
        /// </summary>
        public ref T Value
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => ref MemoryMarshal.GetReference(span);
        }

        /// <summary>
        /// Implicitly creates a new <see cref="ByReference{T}"/> instance from the specified readonly reference
        /// </summary>
        /// <param name="value">The readonly reference to the target <typeparamref name="T"/> value</param>
        /// <remarks>This operator converts a readonly reference in a mutable one, so make sure that's the intended behavior</remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator ByReference<T>(in T value)
        {
            ref T r0 = ref Unsafe.AsRef(value);

            return new ByReference<T>(ref r0);
        }

        /// <summary>
        /// Implicitly gets the <typeparamref name="T"/> value from a given <see cref="ByReference{T}"/> instance
        /// </summary>
        /// <param name="reference">The input <see cref="ByReference{T}"/> instance</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator T(ByReference<T> reference)
        {
            return reference.Value;
        }
    }
}
