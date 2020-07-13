// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;
using Microsoft.Toolkit.HighPerformance.Enumerables;
#if SPAN_RUNTIME_SUPPORT
using System.Runtime.InteropServices;
#endif
using Microsoft.Toolkit.HighPerformance.Helpers.Internals;
using Microsoft.Toolkit.HighPerformance.Memory;

namespace Microsoft.Toolkit.HighPerformance.Extensions
{
    /// <summary>
    /// Helpers for working with the <see cref="Array"/> type.
    /// </summary>
    public static partial class ArrayExtensions
    {
        /// <summary>
        /// Returns a reference to the first element within a given 3D <typeparamref name="T"/> array, with no bounds checks.
        /// </summary>
        /// <typeparam name="T">The type of elements in the input 3D <typeparamref name="T"/> array instance.</typeparam>
        /// <param name="array">The input <typeparamref name="T"/> array instance.</param>
        /// <returns>A reference to the first element within <paramref name="array"/>, or the location it would have used, if <paramref name="array"/> is empty.</returns>
        /// <remarks>This method doesn't do any bounds checks, therefore it is responsibility of the caller to perform checks in case the returned value is dereferenced.</remarks>
        [Pure]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ref T DangerousGetReference<T>(this T[,,] array)
        {
#if NETCORE_RUNTIME
            var arrayData = Unsafe.As<RawArray3DData>(array);
            ref T r0 = ref Unsafe.As<byte, T>(ref arrayData.Data);

            return ref r0;
#else
            if (array.Length > 0)
            {
                return ref array[0, 0, 0];
            }

            unsafe
            {
                return ref Unsafe.AsRef<T>(null);
            }
#endif
        }

        /// <summary>
        /// Returns a reference to an element at a specified coordinate within a given 3D <typeparamref name="T"/> array, with no bounds checks.
        /// </summary>
        /// <typeparam name="T">The type of elements in the input 3D <typeparamref name="T"/> array instance.</typeparam>
        /// <param name="array">The input 2D <typeparamref name="T"/> array instance.</param>
        /// <param name="i">The depth index of the element to retrieve within <paramref name="array"/>.</param>
        /// <param name="j">The vertical index of the element to retrieve within <paramref name="array"/>.</param>
        /// <param name="k">The horizontal index of the element to retrieve within <paramref name="array"/>.</param>
        /// <returns>A reference to the element within <paramref name="array"/> at the coordinate specified by <paramref name="i"/> and <paramref name="j"/>.</returns>
        /// <remarks>
        /// This method doesn't do any bounds checks, therefore it is responsibility of the caller to ensure the <paramref name="i"/>
        /// and <paramref name="j"/> parameters are valid. Furthermore, this extension will ignore the lower bounds for the input
        /// array, and will just assume that the input index is 0-based. It is responsability of the caller to adjust the input
        /// indices to account for the actual lower bounds, if the input array has either axis not starting at 0.
        /// </remarks>
        [Pure]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe ref T DangerousGetReferenceAt<T>(this T[,,] array, int i, int j, int k)
        {
#if NETCORE_RUNTIME
            var arrayData = Unsafe.As<RawArray3DData>(array);
            int offset = (i * arrayData.Height * arrayData.Width) + (j * arrayData.Width) + k;
            ref T r0 = ref Unsafe.As<byte, T>(ref arrayData.Data);
            ref T ri = ref Unsafe.Add(ref r0, (IntPtr)(void*)(uint)offset);

            return ref ri;
#else
            if ((uint)i < (uint)array.GetLength(0) &&
                (uint)j < (uint)array.GetLength(1) &&
                (uint)k < (uint)array.GetLength(2))
            {
                return ref array[i, j, k];
            }

            return ref Unsafe.AsRef<T>(null);
#endif
        }

#if NETCORE_RUNTIME
        // See description for this in the 2D partial file.
        // Using the CHW naming scheme here (like with RGB images).
        [StructLayout(LayoutKind.Sequential)]
        private sealed class RawArray3DData
        {
#pragma warning disable CS0649 // Unassigned fields
#pragma warning disable SA1401 // Fields should be private
            public IntPtr Length;
            public int Channel;
            public int Height;
            public int Width;
            public int ChannelLowerBound;
            public int HeightLowerBound;
            public int WidthLowerBound;
            public byte Data;
#pragma warning restore CS0649
#pragma warning restore SA1401
        }
#endif

#if SPAN_RUNTIME_SUPPORT
        /// <summary>
        /// Creates a new <see cref="Span{T}"/> over an input 3D <typeparamref name="T"/> array.
        /// </summary>
        /// <typeparam name="T">The type of elements in the input 3D <typeparamref name="T"/> array instance.</typeparam>
        /// <param name="array">The input 3D <typeparamref name="T"/> array instance.</param>
        /// <returns>A <see cref="Span{T}"/> instance with the values of <paramref name="array"/>.</returns>
        [Pure]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Span<T> AsSpan<T>(this T[,,] array)
        {
#if NETCORE_RUNTIME
            var arrayData = Unsafe.As<RawArray3DData>(array);

            // See comments for this in the 2D overload
            int length = checked((int)Unsafe.As<IntPtr, uint>(ref arrayData.Length));
            ref T r0 = ref Unsafe.As<byte, T>(ref arrayData.Data);
#else
            int length = array.Length;

            if (length == 0)
            {
                return default;
            }

            ref T r0 = ref array[0, 0, 0];
#endif
            return MemoryMarshal.CreateSpan(ref r0, length);
        }
#endif

#if SPAN_RUNTIME_SUPPORT
        /// <summary>
        /// Creates a new instance of the <see cref="Span{T}"/> struct wrapping a layer in a 3D array.
        /// </summary>
        /// <typeparam name="T">The type of elements in the input 3D <typeparamref name="T"/> array instance.</typeparam>
        /// <param name="array">The given 3D array to wrap.</param>
        /// <param name="depth">The target layer to map within <paramref name="array"/>.</param>
        /// <exception cref="ArrayTypeMismatchException">
        /// Thrown when <paramref name="array"/> doesn't match <typeparamref name="T"/>.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when either <paramref name="depth"/> is invalid.</exception>
        /// <returns>A <see cref="Span{T}"/> instance wrapping the target layer within <paramref name="array"/>.</returns>
        [Pure]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Span<T> GetLayerSpan<T>(this T[,,] array, int depth)
        {
            if ((uint)depth >= (uint)array.GetLength(0))
            {
                static void Throw() => throw new ArgumentOutOfRangeException(nameof(depth));

                Throw();
            }

            ref T r0 = ref array.DangerousGetReferenceAt(depth, 0, 0);
            int length = array.GetLength(1) * array.GetLength(2);

            return MemoryMarshal.CreateSpan(ref r0, length);
        }
#endif

        /// <summary>
        /// Returns a <see cref="RefEnumerable{T}"/> over a row in a given 3D <typeparamref name="T"/> array instance.
        /// </summary>
        /// <typeparam name="T">The type of elements in the input 3D <typeparamref name="T"/> array instance.</typeparam>
        /// <param name="array">The input <typeparamref name="T"/> array instance.</param>
        /// <param name="depth">The target layer to map within <paramref name="array"/>.</param>
        /// <param name="row">The target row to retrieve (0-based index).</param>
        /// <returns>A <see cref="RefEnumerable{T}"/> with the items from the target row within <paramref name="array"/>.</returns>
        /// <remarks>The returned <see cref="RefEnumerable{T}"/> value shouldn't be used directly: use this extension in a <see langword="foreach"/> loop.</remarks>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when one of the input parameters is out of range.</exception>
        [Pure]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static RefEnumerable<T> GetRow<T>(this T[,,] array, int depth, int row)
        {
            int
                layers = array.GetLength(0),
                height = array.GetLength(1);

            if ((uint)depth >= (uint)layers)
            {
                static void Throw() => throw new ArgumentOutOfRangeException(nameof(depth));

                Throw();
            }

            if ((uint)row >= (uint)height)
            {
                static void Throw() => throw new ArgumentOutOfRangeException(nameof(row));

                Throw();
            }

            int width = array.GetLength(2);

#if SPAN_RUNTIME_SUPPORT
            ref T r0 = ref array.DangerousGetReferenceAt(depth, row, 0);

            return new RefEnumerable<T>(ref r0, width, 1);
#else
            ref T r0 = ref array.DangerousGetReferenceAt(depth, row, 0);
            IntPtr offset = array.DangerousGetObjectDataByteOffset(ref r0);

            return new RefEnumerable<T>(array, offset, width, 1);
#endif
        }

        /// <summary>
        /// Returns an enumerable that returns the items from a given column in a given 2D <typeparamref name="T"/> array instance.
        /// This extension should be used directly within a <see langword="foreach"/> loop:
        /// <code>
        /// int[,,] matrix =
        /// {
        ///     {
        ///         { 1, 2, 3 },
        ///         { 4, 5, 6 },
        ///     },
        ///     {
        ///         { 7, 8, 9 },
        ///         { 10, 11, 12 },
        ///     }
        /// };
        ///
        /// foreach (ref int number in matrix.GetColumn(1, 1))
        /// {
        ///     // Access the current number by reference here...
        /// }
        /// </code>
        /// The compiler will take care of properly setting up the <see langword="foreach"/> loop with the type returned from this method.
        /// </summary>
        /// <typeparam name="T">The type of elements in the input 2D <typeparamref name="T"/> array instance.</typeparam>
        /// <param name="array">The input <typeparamref name="T"/> array instance.</param>
        /// <param name="depth">The target layer to map within <paramref name="array"/>.</param>
        /// <param name="column">The target column to retrieve (0-based index).</param>
        /// <returns>A wrapper type that will handle the column enumeration for <paramref name="array"/>.</returns>
        /// <remarks>The returned <see cref="RefEnumerable{T}"/> value shouldn't be used directly: use this extension in a <see langword="foreach"/> loop.</remarks>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when one of the input parameters is out of range.</exception>
        [Pure]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static RefEnumerable<T> GetColumn<T>(this T[,,] array, int depth, int column)
        {
            int
                layers = array.GetLength(0),
                width = array.GetLength(2);

            if ((uint)depth >= (uint)layers)
            {
                static void Throw() => throw new ArgumentOutOfRangeException(nameof(depth));

                Throw();
            }

            if ((uint)column >= (uint)width)
            {
                static void Throw() => throw new ArgumentOutOfRangeException(nameof(column));

                Throw();
            }

            int height = array.GetLength(1);

#if SPAN_RUNTIME_SUPPORT
            ref T r0 = ref array.DangerousGetReferenceAt(depth, 0, column);

            return new RefEnumerable<T>(ref r0, height, width);
#else
            ref T r0 = ref array.DangerousGetReferenceAt(depth, 0, column);
            IntPtr offset = array.DangerousGetObjectDataByteOffset(ref r0);

            return new RefEnumerable<T>(array, offset, height, width);
#endif
        }

        /// <summary>
        /// Creates a new instance of the <see cref="Span2D{T}"/> struct wrapping a layer in a 3D array.
        /// </summary>
        /// <typeparam name="T">The type of elements in the input 3D <typeparamref name="T"/> array instance.</typeparam>
        /// <param name="array">The given 3D array to wrap.</param>
        /// <param name="depth">The target layer to map within <paramref name="array"/>.</param>
        /// <exception cref="ArrayTypeMismatchException">
        /// Thrown when <paramref name="array"/> doesn't match <typeparamref name="T"/>.
        /// </exception>
        /// <exception cref="ArgumentException">Thrown when either <paramref name="depth"/> is invalid.</exception>
        /// <returns>A <see cref="Span2D{T}"/> instance wrapping the target layer within <paramref name="array"/>.</returns>
        [Pure]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Span2D<T> GetLayerSpan2D<T>(this T[,,] array, int depth)
        {
            return new Span2D<T>(array, depth);
        }

        /// <summary>
        /// Creates a new instance of the <see cref="Memory2D{T}"/> struct wrapping a layer in a 3D array.
        /// </summary>
        /// <typeparam name="T">The type of elements in the input 3D <typeparamref name="T"/> array instance.</typeparam>
        /// <param name="array">The given 3D array to wrap.</param>
        /// <param name="depth">The target layer to map within <paramref name="array"/>.</param>
        /// <exception cref="ArrayTypeMismatchException">
        /// Thrown when <paramref name="array"/> doesn't match <typeparamref name="T"/>.
        /// </exception>
        /// <exception cref="ArgumentException">Thrown when either <paramref name="depth"/> is invalid.</exception>
        /// <returns>A <see cref="Memory2D{T}"/> instance wrapping the target layer within <paramref name="array"/>.</returns>
        [Pure]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Memory2D<T> GetLayerMemory2D<T>(this T[,,] array, int depth)
        {
            return new Memory2D<T>(array, depth);
        }

        /// <summary>
        /// Counts the number of occurrences of a given value into a target 3D <typeparamref name="T"/> array instance.
        /// </summary>
        /// <typeparam name="T">The type of items in the input 3D <typeparamref name="T"/> array instance.</typeparam>
        /// <param name="array">The input 3D <typeparamref name="T"/> array instance.</param>
        /// <param name="value">The <typeparamref name="T"/> value to look for.</param>
        /// <returns>The number of occurrences of <paramref name="value"/> in <paramref name="array"/>.</returns>
        [Pure]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Count<T>(this T[,,] array, T value)
            where T : IEquatable<T>
        {
            ref T r0 = ref array.DangerousGetReference();
            IntPtr length = (IntPtr)array.Length;

            return SpanHelper.Count(ref r0, length, value);
        }

        /// <summary>
        /// Gets a content hash from the input 3D <typeparamref name="T"/> array instance using the Djb2 algorithm.
        /// For more info, see the documentation for <see cref="ReadOnlySpanExtensions.GetDjb2HashCode{T}"/>.
        /// </summary>
        /// <typeparam name="T">The type of items in the input 3D <typeparamref name="T"/> array instance.</typeparam>
        /// <param name="array">The input 3D <typeparamref name="T"/> array instance.</param>
        /// <returns>The Djb2 value for the input 3D <typeparamref name="T"/> array instance.</returns>
        /// <remarks>The Djb2 hash is fully deterministic and with no random components.</remarks>
        [Pure]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int GetDjb2HashCode<T>(this T[,,] array)
            where T : notnull
        {
            ref T r0 = ref array.DangerousGetReference();
            IntPtr length = (IntPtr)array.Length;

            return SpanHelper.GetDjb2HashCode(ref r0, length);
        }

        /// <summary>
        /// Checks whether or not a given <typeparamref name="T"/> array is covariant.
        /// </summary>
        /// <typeparam name="T">The type of items in the input <typeparamref name="T"/> array instance.</typeparam>
        /// <param name="array">The input <typeparamref name="T"/> array instance.</param>
        /// <returns>Whether or not <paramref name="array"/> is covariant.</returns>
        [Pure]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsCovariant<T>(this T[,,] array)
        {
            return
#pragma warning disable SA1003 // Whitespace before ! operator
#if NETSTANDARD1_4

                !System.Reflection.IntrospectionExtensions.GetTypeInfo(typeof(T)).IsValueType &&
#else
                !typeof(T).IsValueType &&
#endif
#pragma warning restore SA1003
                array.GetType() != typeof(T[]);
        }
    }
}
