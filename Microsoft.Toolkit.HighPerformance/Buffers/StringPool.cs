// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
#if NETCOREAPP3_1
using System.Numerics;
#endif
using System.Runtime.CompilerServices;
using System.Text;
using Microsoft.Toolkit.HighPerformance.Extensions;
#if !NETSTANDARD1_4
using Microsoft.Toolkit.HighPerformance.Helpers;
#endif

namespace Microsoft.Toolkit.HighPerformance.Buffers
{
    /// <summary>
    /// A configurable pool for <see cref="string"/> instances. This can be used to minimize allocations
    /// when creating multiple <see cref="string"/> instances from buffers of <see cref="char"/> values.
    /// The <see cref="GetOrAdd(ReadOnlySpan{char})"/> method provides a best-effort alternative to just creating
    /// a new <see cref="string"/> instance every time, in order to minimize the number of duplicated instances.
    /// </summary>
    public sealed class StringPool
    {
        /// <summary>
        /// The size for the <see cref="Default"/> instance.
        /// </summary>
        private const int DefaultSize = 2048;

        /// <summary>
        /// The minimum size for <see cref="StringPool"/> instances.
        /// </summary>
        private const int MinimumSize = 32;

        /// <summary>
        /// A bitmask used to avoid branches when computing the absolute value of an <see cref="int"/>.
        /// This will ignore overflows, as we just need this to map hashcodes into the valid bucket range.
        /// </summary>
        private const int SignMask = ~(1 << 31);

        /// <summary>
        /// The current array of <see cref="Bucket"/> instances in use.
        /// </summary>
        private readonly Bucket[] buckets;

        /// <summary>
        /// The total number of buckets in use.
        /// </summary>
        private readonly int numberOfBuckets;

        /// <summary>
        /// Initializes a new instance of the <see cref="StringPool"/> class.
        /// </summary>
        public StringPool()
            : this(DefaultSize)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StringPool"/> class.
        /// </summary>
        /// <param name="minimumSize">The minimum size for the pool to create.</param>
        public StringPool(int minimumSize)
        {
            if (minimumSize <= 0)
            {
                ThrowArgumentOutOfRangeException();
            }

            // Set the minimum size
            minimumSize = Math.Max(minimumSize, MinimumSize);

            // Calculates the rounded up factors for a specific size/factor pair
            static void FindFactors(int size, int factor, out int x, out int y)
            {
                double
                    a = Math.Sqrt((double)size / factor),
                    b = factor * a;

                x = RoundUpPowerOfTwo((int)a);
                y = RoundUpPowerOfTwo((int)b);
            }

            // We want to find two powers of 2 factors that produce a number
            // that is at least equal to the requested size. In order to find the
            // combination producing the optimal factors (with the product being as
            // close as possible to the requested size), we test a number of ratios
            // that we consider acceptable, and pick the best results produced.
            // The ratio between buckets influences the number of objects being allocated,
            // as well as the multithreading performance when locking on buckets.
            // We still want to contraint this number to avoid situations where we
            // have a way too high number of buckets compared to total size.
            FindFactors(minimumSize, 2, out int x2, out int y2);
            FindFactors(minimumSize, 3, out int x3, out int y3);
            FindFactors(minimumSize, 4, out int x4, out int y4);

            int
                p2 = x2 * y2,
                p3 = x3 * y3,
                p4 = x4 * y4;

            if (p3 < p2)
            {
                p2 = p3;
                x2 = x3;
                y2 = y3;
            }

            if (p4 < p2)
            {
                p2 = p4;
                x2 = x4;
                y2 = y4;
            }

            Span<Bucket> span = this.buckets = new Bucket[x2];

            // We preallocate the buckets in advance, since each bucket only contains the
            // array field, which is not preinitialized, so the allocations are minimal.
            // This lets us lock on each individual buckets when retrieving a string instance.
            foreach (ref Bucket bucket in span)
            {
                bucket = new Bucket(y2);
            }

            this.numberOfBuckets = x2;

            Size = p2;
        }

        /// <summary>
        /// Rounds up an <see cref="int"/> value to a power of 2.
        /// </summary>
        /// <param name="x">The input value to round up.</param>
        /// <returns>The smallest power of two greater than or equal to <paramref name="x"/>.</returns>
        [Pure]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static int RoundUpPowerOfTwo(int x)
        {
#if NETCOREAPP3_1
            return 1 << (32 - BitOperations.LeadingZeroCount((uint)(x - 1)));
#else
            x--;
            x |= x >> 1;
            x |= x >> 2;
            x |= x >> 4;
            x |= x >> 8;
            x |= x >> 16;
            x++;

            return x;
#endif
        }

        /// <summary>
        /// Gets the default <see cref="StringPool"/> instance.
        /// </summary>
        public static StringPool Default { get; } = new StringPool();

        /// <summary>
        /// Gets the total number of <see cref="string"/> that can be stored in the current instance.
        /// <remarks>
        /// This property only refers to the total number of available slots, ignoring collisions.
        /// The requested size should always be set to a higher value than the target number of items
        /// that will be stored in the cache, to reduce the number of collisions when caching values.
        /// </remarks>
        /// </summary>
        public int Size { get; }

        /// <summary>
        /// Stores a <see cref="string"/> instance in the internal cache.
        /// </summary>
        /// <param name="value">The input <see cref="string"/> instance to cache.</param>
        public void Add(string value)
        {
            if (value.Length == 0)
            {
                return;
            }

            int
                hashcode = GetHashCode(value.AsSpan()),
                bucketIndex = hashcode & (this.numberOfBuckets - 1);

            ref Bucket bucket = ref this.buckets.DangerousGetReferenceAt(bucketIndex);

            bucket.Add(value, hashcode);
        }

        /// <summary>
        /// Gets a cached <see cref="string"/> instance matching the input content, or stores the input one.
        /// </summary>
        /// <param name="value">The input <see cref="string"/> instance with the contents to use.</param>
        /// <returns>A <see cref="string"/> instance with the contents of <paramref name="value"/>, cached if possible.</returns>
        public string GetOrAdd(string value)
        {
            if (value.Length == 0)
            {
                return string.Empty;
            }

            int
                hashcode = GetHashCode(value.AsSpan()),
                bucketIndex = hashcode & (this.numberOfBuckets - 1);

            ref Bucket bucket = ref this.buckets.DangerousGetReferenceAt(bucketIndex);

            return bucket.GetOrAdd(value, hashcode);
        }

        /// <summary>
        /// Gets a cached <see cref="string"/> instance matching the input content, or creates a new one.
        /// </summary>
        /// <param name="span">The input <see cref="ReadOnlySpan{T}"/> with the contents to use.</param>
        /// <returns>A <see cref="string"/> instance with the contents of <paramref name="span"/>, cached if possible.</returns>
        public string GetOrAdd(ReadOnlySpan<char> span)
        {
            if (span.IsEmpty)
            {
                return string.Empty;
            }

            int
                hashcode = GetHashCode(span),
                bucketIndex = hashcode & (this.numberOfBuckets - 1);

            ref Bucket bucket = ref this.buckets.DangerousGetReferenceAt(bucketIndex);

            return bucket.GetOrAdd(span, hashcode);
        }

        /// <summary>
        /// Gets a cached <see cref="string"/> instance matching the input content (converted to Unicode), or creates a new one.
        /// </summary>
        /// <param name="span">The input <see cref="ReadOnlySpan{T}"/> with the contents to use, in a specified encoding.</param>
        /// <param name="encoding">The <see cref="Encoding"/> instance to use to decode the contents of <paramref name="span"/>.</param>
        /// <returns>A <see cref="string"/> instance with the contents of <paramref name="span"/>, cached if possible.</returns>
        public unsafe string GetOrAdd(ReadOnlySpan<byte> span, Encoding encoding)
        {
            if (span.IsEmpty)
            {
                return string.Empty;
            }

            int maxLength = encoding.GetMaxCharCount(span.Length);

            using SpanOwner<char> buffer = SpanOwner<char>.Allocate(maxLength);

            fixed (byte* source = span)
            fixed (char* destination = &buffer.DangerousGetReference())
            {
                int effectiveLength = encoding.GetChars(source, span.Length, destination, maxLength);

                return GetOrAdd(new ReadOnlySpan<char>(destination, effectiveLength));
            }
        }

        /// <summary>
        /// Tries to get a cached <see cref="string"/> instance matching the input content, if present.
        /// </summary>
        /// <param name="span">The input <see cref="ReadOnlySpan{T}"/> with the contents to use.</param>
        /// <param name="value">The resulting cached <see cref="string"/> instance, if present</param>
        /// <returns>Whether or not the target <see cref="string"/> instance was found.</returns>
        public bool TryGet(ReadOnlySpan<char> span, [NotNullWhen(true)] out string? value)
        {
            if (span.IsEmpty)
            {
                value = string.Empty;

                return true;
            }

            int
                hashcode = GetHashCode(span),
                bucketIndex = hashcode & (this.numberOfBuckets - 1);

            ref Bucket bucket = ref this.buckets.DangerousGetReferenceAt(bucketIndex);

            return bucket.TryGet(span, hashcode, out value);
        }

        /// <summary>
        /// Resets the current instance and its associated buckets.
        /// </summary>
        public void Reset()
        {
            foreach (ref Bucket bucket in this.buckets.AsSpan())
            {
                bucket.Reset();
            }
        }

        /// <summary>
        /// A configurable bucket containing a group of cached <see cref="string"/> instances.
        /// </summary>
        private struct Bucket
        {
            /// <summary>
            /// The number of entries being used in the current instance.
            /// </summary>
            private readonly int entriesPerBucket;

            /// <summary>
            /// A dummy <see cref="object"/> used for synchronization.
            /// </summary>
            private readonly object dummy;

            /// <summary>
            /// The array of entries currently in use.
            /// </summary>
            private string?[]? entries;

            /// <summary>
            /// Initializes a new instance of the <see cref="Bucket"/> struct.
            /// </summary>
            /// <param name="entriesPerBucket">The number of entries being used in the current instance.</param>
            public Bucket(int entriesPerBucket)
            {
                this.entriesPerBucket = entriesPerBucket;
                this.dummy = new object();
                this.entries = null;
            }

            /// <summary>
            /// Implements <see cref="StringPool.Add"/> for the current <see cref="Bucket"/> instance.
            /// </summary>
            /// <param name="value">The input <see cref="string"/> instance to cache.</param>
            /// <param name="hashcode">The precomputed hashcode for <paramref name="value"/>.</param>
            public void Add(string value, int hashcode)
            {
                lock (this.dummy)
                {
                    ref string?[]? entries = ref this.entries;

                    entries ??= new string[entriesPerBucket];

                    int entryIndex = hashcode & (this.entriesPerBucket - 1);

                    entries.DangerousGetReferenceAt(entryIndex) = value;
                }
            }

            /// <summary>
            /// Implements <see cref="StringPool.GetOrAdd(string)"/> for the current <see cref="Bucket"/> instance.
            /// </summary>
            /// <param name="value">The input <see cref="string"/> instance with the contents to use.</param>
            /// <param name="hashcode">The precomputed hashcode for <paramref name="value"/>.</param>
            /// <returns>A <see cref="string"/> instance with the contents of <paramref name="value"/>.</returns>
            public string GetOrAdd(string value, int hashcode)
            {
                lock (this.dummy)
                {
                    ref string?[]? entries = ref this.entries;

                    entries ??= new string[entriesPerBucket];

                    int entryIndex = hashcode & (this.entriesPerBucket - 1);

                    ref string? entry = ref entries.DangerousGetReferenceAt(entryIndex);

                    if (!(entry is null) &&
                        entry.AsSpan().SequenceEqual(value.AsSpan()))
                    {
                        return entry;
                    }

                    return entry = value;
                }
            }

            /// <summary>
            /// Implements <see cref="StringPool.GetOrAdd(ReadOnlySpan{char})"/> for the current <see cref="Bucket"/> instance.
            /// </summary>
            /// <param name="span">The input <see cref="ReadOnlySpan{T}"/> with the contents to use.</param>
            /// <param name="hashcode">The precomputed hashcode for <paramref name="span"/>.</param>
            /// <returns>A <see cref="string"/> instance with the contents of <paramref name="span"/>, cached if possible.</returns>
            public string GetOrAdd(ReadOnlySpan<char> span, int hashcode)
            {
                lock (this.dummy)
                {
                    ref string?[]? entries = ref this.entries;

                    entries ??= new string[entriesPerBucket];

                    int entryIndex = hashcode & (this.entriesPerBucket - 1);

                    ref string? entry = ref entries.DangerousGetReferenceAt(entryIndex);

                    if (!(entry is null) &&
                        entry.AsSpan().SequenceEqual(span))
                    {
                        return entry;
                    }

                    // ReadOnlySpan<char>.ToString() creates a string with the span contents.
                    // This is equivalent to doing new string(span) on Span<T>-enabled runtimes,
                    // or to using an unsafe block, a fixed statement and new string(char*, int, int).
                    return entry = span.ToString();
                }
            }

            /// <summary>
            /// Implements <see cref="StringPool.TryGet"/> for the current <see cref="Bucket"/> instance.
            /// </summary>
            /// <param name="span">The input <see cref="ReadOnlySpan{T}"/> with the contents to use.</param>
            /// <param name="hashcode">The precomputed hashcode for <paramref name="span"/>.</param>
            /// <param name="value">The resulting cached <see cref="string"/> instance, if present</param>
            /// <returns>Whether or not the target <see cref="string"/> instance was found.</returns>
            public bool TryGet(ReadOnlySpan<char> span, int hashcode, [NotNullWhen(true)] out string? value)
            {
                lock (this.dummy)
                {
                    ref string?[]? entries = ref this.entries;

                    if (!(entries is null))
                    {
                        int entryIndex = hashcode & (this.entriesPerBucket - 1);

                        ref string? entry = ref entries.DangerousGetReferenceAt(entryIndex);

                        if (!(entry is null) &&
                            entry.AsSpan().SequenceEqual(span))
                        {
                            value = entry;

                            return true;
                        }
                    }

                    value = null;

                    return false;
                }
            }

            /// <summary>
            /// Resets the current array of entries.
            /// </summary>
            public void Reset()
            {
                lock (this.dummy)
                {
                    this.entries = null;
                }
            }
        }

        /// <summary>
        /// Gets the (positive) hashcode for a given <see cref="ReadOnlySpan{T}"/> instance.
        /// </summary>
        /// <param name="span">The input <see cref="ReadOnlySpan{T}"/> instance.</param>
        /// <returns>The hashcode for <paramref name="span"/>.</returns>
        [Pure]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static int GetHashCode(ReadOnlySpan<char> span)
        {
#if NETSTANDARD1_4
            return span.GetDjb2HashCode() & SignMask;
#else
            return HashCode<char>.Combine(span) & SignMask;
#endif
        }

        /// <summary>
        /// Throws an <see cref="ArgumentException"/> when the requested size exceeds the capacity.
        /// </summary>
        private static void ThrowArgumentOutOfRangeException()
        {
            throw new ArgumentOutOfRangeException("minimumSize", "The requested size must be greater than 0");
        }
    }
}
