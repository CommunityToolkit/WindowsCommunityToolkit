// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Diagnostics.Contracts;
using System.Drawing;
using System.Runtime.CompilerServices;
#if SPAN_RUNTIME_SUPPORT
using System.Runtime.InteropServices;
#endif
using Microsoft.Toolkit.HighPerformance.Enumerables;
using Microsoft.Toolkit.HighPerformance.Helpers.Internals;

namespace Microsoft.Toolkit.HighPerformance.Extensions
{
    /// <summary>
    /// Helpers for working with the <see cref="Array"/> type.
    /// </summary>
    public static partial class ArrayExtensions
    {
        /// <summary>
        /// Returns a reference to the first element within a given 2D <typeparamref name="T"/> array, with no bounds checks.
        /// </summary>
        /// <typeparam name="T">The type of elements in the input 2D <typeparamref name="T"/> array instance.</typeparam>
        /// <param name="array">The input <typeparamref name="T"/> array instance.</param>
        /// <returns>A reference to the first element within <paramref name="array"/>, or the location it would have used, if <paramref name="array"/> is empty.</returns>
        /// <remarks>This method doesn't do any bounds checks, therefore it is responsibility of the caller to perform checks in case the returned value is dereferenced.</remarks>
        [Pure]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ref T DangerousGetReference<T>(this T[,] array)
        {
#if NETCORE_RUNTIME
            var arrayData = Unsafe.As<RawArray2DData>(array);
            ref T r0 = ref Unsafe.As<byte, T>(ref arrayData.Data);

            return ref r0;
#else
#pragma warning disable SA1131 // Inverted comparison to remove JIT bounds check
            if (0u < (uint)array.Length)
            {
                return ref array[0, 0];
            }

            unsafe
            {
                return ref Unsafe.AsRef<T>(null);
            }
#pragma warning restore SA1131
#endif
        }

        /// <summary>
        /// Returns a reference to an element at a specified coordinate within a given 2D <typeparamref name="T"/> array, with no bounds checks.
        /// </summary>
        /// <typeparam name="T">The type of elements in the input 2D <typeparamref name="T"/> array instance.</typeparam>
        /// <param name="array">The input 2D <typeparamref name="T"/> array instance.</param>
        /// <param name="i">The vertical index of the element to retrieve within <paramref name="array"/>.</param>
        /// <param name="j">The horizontal index of the element to retrieve within <paramref name="array"/>.</param>
        /// <returns>A reference to the element within <paramref name="array"/> at the coordinate specified by <paramref name="i"/> and <paramref name="j"/>.</returns>
        /// <remarks>
        /// This method doesn't do any bounds checks, therefore it is responsibility of the caller to ensure the <paramref name="i"/>
        /// and <paramref name="j"/> parameters are valid. Furthermore, this extension will ignore the lower bounds for the input
        /// array, and will just assume that the input index is 0-based. It is responsability of the caller to adjust the input
        /// indices to account for the actual lower bounds, if the input array has either axis not starting at 0.
        /// </remarks>
        [Pure]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ref T DangerousGetReferenceAt<T>(this T[,] array, int i, int j)
        {
#if NETCORE_RUNTIME
            var arrayData = Unsafe.As<RawArray2DData>(array);
            int offset = (i * arrayData.Width) + j;
            ref T r0 = ref Unsafe.As<byte, T>(ref arrayData.Data);
            ref T ri = ref Unsafe.Add(ref r0, offset);

            return ref ri;
#else
            if ((uint)i < (uint)array.GetLength(0) &&
                (uint)j < (uint)array.GetLength(1))
            {
                return ref array[i, j];
            }

            unsafe
            {
                return ref Unsafe.AsRef<T>(null);
            }
#endif
        }

#if NETCORE_RUNTIME
        // Description adapted from CoreCLR: see https://source.dot.net/#System.Private.CoreLib/src/System/Runtime/CompilerServices/RuntimeHelpers.CoreCLR.cs,285.
        // CLR 2D arrays are laid out in memory as follows:
        // [ sync block || pMethodTable || Length (padded to IntPtr) || HxW || HxW bounds || array data .. ]
        //                 ^                                                                 ^
        //                 |                                                                 \-- ref Unsafe.As<RawArray2DData>(array).Data
        //                 \-- array
        // The length is always padded to IntPtr just like with SZ arrays.
        // The total data padding is therefore 20 bytes on x86 (4 + 4 + 4 + 4 + 4), or 24 bytes on x64.
        [StructLayout(LayoutKind.Sequential)]
        private sealed class RawArray2DData
        {
#pragma warning disable CS0649 // Unassigned fields
#pragma warning disable SA1401 // Fields should be private
            public IntPtr Length;
            public int Height;
            public int Width;
            public int HeightLowerBound;
            public int WidthLowerBound;
            public byte Data;
#pragma warning restore CS0649
#pragma warning restore SA1401
        }
#endif

        /// <summary>
        /// Fills an area in a given 2D <typeparamref name="T"/> array instance with a specified value.
        /// This API will try to fill as many items as possible, ignoring positions outside the bounds of the array.
        /// If invalid coordinates are given, they will simply be ignored and no exception will be thrown.
        /// </summary>
        /// <typeparam name="T">The type of elements in the input 2D <typeparamref name="T"/> array instance.</typeparam>
        /// <param name="array">The input <typeparamref name="T"/> array instance.</param>
        /// <param name="value">The <typeparamref name="T"/> value to fill the target area with.</param>
        /// <param name="row">The row to start on (inclusive, 0-based index).</param>
        /// <param name="column">The column to start on (inclusive, 0-based index).</param>
        /// <param name="width">The positive width of area to fill.</param>
        /// <param name="height">The positive height of area to fill.</param>
        public static void Fill<T>(this T[,] array, T value, int row, int column, int width, int height)
        {
            Rectangle bounds = new Rectangle(0, 0, array.GetLength(1), array.GetLength(0));

            // Precompute bounds to skip branching in main loop
            bounds.Intersect(new Rectangle(column, row, width, height));

            for (int i = bounds.Top; i < bounds.Bottom; i++)
            {
#if SPAN_RUNTIME_SUPPORT
#if NETCORE_RUNTIME
                ref T r0 = ref array.DangerousGetReferenceAt(i, bounds.Left);
#else
                ref T r0 = ref array[i, bounds.Left];
#endif

                // Span<T>.Fill will use vectorized instructions when possible
                MemoryMarshal.CreateSpan(ref r0, bounds.Width).Fill(value);
#else
                ref T r0 = ref array[i, bounds.Left];

                for (int j = 0; j < bounds.Width; j++)
                {
                    // Storing the initial reference and only incrementing
                    // that one in each iteration saves one additional indirect
                    // dereference for every loop iteration compared to using
                    // the DangerousGetReferenceAt<T> extension on the array.
                    Unsafe.Add(ref r0, j) = value;
                }
#endif
            }
        }

        /// <summary>
        /// Returns a <see cref="Span{T}"/> over a row in a given 2D <typeparamref name="T"/> array instance.
        /// </summary>
        /// <typeparam name="T">The type of elements in the input 2D <typeparamref name="T"/> array instance.</typeparam>
        /// <param name="array">The input <typeparamref name="T"/> array instance.</param>
        /// <param name="row">The target row to retrieve (0-based index).</param>
        /// <returns>A <see cref="Span{T}"/> with the items from the target row within <paramref name="array"/>.</returns>
        [Pure]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static
#if SPAN_RUNTIME_SUPPORT
            Span<T>
#else
            // .NET Standard 2.0 lacks MemoryMarshal.CreateSpan<T>(ref T, int),
            // which is necessary to create arbitrary Span<T>-s over a 2D array.
            // To work around this, we use a custom ref struct enumerator,
            // which makes the lack of that API completely transparent to the user.
            // If a user then moves from .NET Standard 2.0 to 2.1, all the previous
            // features will be perfectly supported, and in addition to that it will
            // also gain the ability to use the Span<T> value elsewhere.
            // The only case where this would be a breaking change for a user upgrading
            // the target framework is when the returned enumerator type is used directly,
            // but since that's specifically discouraged from the docs, we don't
            // need to worry about that scenario in particular, as users doing that
            // would be willingly go against the recommended usage of this API.
            Array2DRowEnumerable<T>
#endif
            GetRow<T>(this T[,] array, int row)
        {
            if ((uint)row >= (uint)array.GetLength(0))
            {
                throw new ArgumentOutOfRangeException(nameof(row));
            }

#if SPAN_RUNTIME_SUPPORT
            ref T r0 = ref array.DangerousGetReferenceAt(row, 0);

            return MemoryMarshal.CreateSpan(ref r0, array.GetLength(1));
#else
            return new Array2DRowEnumerable<T>(array, row);
#endif
        }

        /// <summary>
        /// Returns an enumerable that returns the items from a given column in a given 2D <typeparamref name="T"/> array instance.
        /// This extension should be used directly within a <see langword="foreach"/> loop:
        /// <code>
        /// int[,] matrix =
        /// {
        ///     { 1, 2, 3 },
        ///     { 4, 5, 6 },
        ///     { 7, 8, 9 }
        /// };
        ///
        /// foreach (ref int number in matrix.GetColumn(1))
        /// {
        ///     // Access the current number by reference here...
        /// }
        /// </code>
        /// The compiler will take care of properly setting up the <see langword="foreach"/> loop with the type returned from this method.
        /// </summary>
        /// <typeparam name="T">The type of elements in the input 2D <typeparamref name="T"/> array instance.</typeparam>
        /// <param name="array">The input <typeparamref name="T"/> array instance.</param>
        /// <param name="column">The target column to retrieve (0-based index).</param>
        /// <returns>A wrapper type that will handle the column enumeration for <paramref name="array"/>.</returns>
        /// <remarks>The returned <see cref="Array2DColumnEnumerable{T}"/> value shouldn't be used directly: use this extension in a <see langword="foreach"/> loop.</remarks>
        [Pure]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Array2DColumnEnumerable<T> GetColumn<T>(this T[,] array, int column)
        {
            return new Array2DColumnEnumerable<T>(array, column);
        }

#if SPAN_RUNTIME_SUPPORT
        /// <summary>
        /// Cretes a new <see cref="Span{T}"/> over an input 2D <typeparamref name="T"/> array.
        /// </summary>
        /// <typeparam name="T">The type of elements in the input 2D <typeparamref name="T"/> array instance.</typeparam>
        /// <param name="array">The input 2D <typeparamref name="T"/> array instance.</param>
        /// <returns>A <see cref="Span{T}"/> instance with the values of <paramref name="array"/>.</returns>
        [Pure]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Span<T> AsSpan<T>(this T[,] array)
        {
#if NETCORE_RUNTIME
            var arrayData = Unsafe.As<RawArray2DData>(array);

            // On x64, the length is padded to x64, but it is represented in memory
            // as two sequential uint fields (one of which is padding).
            // So we can just reinterpret a reference to the IntPtr as one of type
            // uint, to access the first 4 bytes of that field, regardless of whether
            // we're running in a 32 or 64 bit process. This will work when on little
            // endian systems as well, as the memory layout for fields is the same,
            // the only difference is the order of bytes within each field of a given type.
            // We use checked here to follow suit with the CoreCLR source, where an
            // invalid value here should fail to perform the cast and throw an exception.
            int length = checked((int)Unsafe.As<IntPtr, uint>(ref arrayData.Length));
            ref T r0 = ref Unsafe.As<byte, T>(ref arrayData.Data);
#else
            int length = array.Length;
            ref T r0 = ref array[0, 0];
#endif
            return MemoryMarshal.CreateSpan(ref r0, length);
        }
#endif

        /// <summary>
        /// Counts the number of occurrences of a given value into a target 2D <typeparamref name="T"/> array instance.
        /// </summary>
        /// <typeparam name="T">The type of items in the input 2D <typeparamref name="T"/> array instance.</typeparam>
        /// <param name="array">The input 2D <typeparamref name="T"/> array instance.</param>
        /// <param name="value">The <typeparamref name="T"/> value to look for.</param>
        /// <returns>The number of occurrences of <paramref name="value"/> in <paramref name="array"/>.</returns>
        [Pure]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Count<T>(this T[,] array, T value)
            where T : IEquatable<T>
        {
            ref T r0 = ref array.DangerousGetReference();
            IntPtr length = (IntPtr)array.Length;

            return SpanHelper.Count(ref r0, length, value);
        }

        /// <summary>
        /// Gets a content hash from the input 2D <typeparamref name="T"/> array instance using the Djb2 algorithm.
        /// For more info, see the documentation for <see cref="ReadOnlySpanExtensions.GetDjb2HashCode{T}"/>.
        /// </summary>
        /// <typeparam name="T">The type of items in the input 2D <typeparamref name="T"/> array instance.</typeparam>
        /// <param name="array">The input 2D <typeparamref name="T"/> array instance.</param>
        /// <returns>The Djb2 value for the input 2D <typeparamref name="T"/> array instance.</returns>
        /// <remarks>The Djb2 hash is fully deterministic and with no random components.</remarks>
        [Pure]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int GetDjb2HashCode<T>(this T[,] array)
            where T : notnull
        {
            ref T r0 = ref array.DangerousGetReference();
            IntPtr length = (IntPtr)array.Length;

            return SpanHelper.GetDjb2HashCode(ref r0, length);
        }
    }
}
