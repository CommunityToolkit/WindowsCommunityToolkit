// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Microsoft.Toolkit.HighPerformance.Extensions;
using Microsoft.Toolkit.HighPerformance.Helpers;

namespace Microsoft.Toolkit.HighPerformance.Buffers
{
    /// <summary>
    /// A configurable pool for <see cref="string"/> instances. This can be used to minimize allocations
    /// when creating multiple <see cref="string"/> instances from buffers of <see cref="char"/> values.
    /// The <see cref="GetOrAdd"/> method provides a best-effort alternative to just creating a new
    /// <see cref="string"/> instance every time, in order to minimize the number of duplicated instances.
    /// </summary>
    public sealed class StringPool
    {
        /// <summary>
        /// The default number of <see cref="Bucket"/> instances in <see cref="buckets"/>.
        /// </summary>
        private const int DefaultNumberOfBuckets = 20;

        /// <summary>
        /// The default number of entries stored in each <see cref="Bucket"/> instance.
        /// </summary>
        private const int DefaultEntriesPerBucket = 64;

        /// <summary>
        /// The current array of <see cref="Bucket"/> instances in use.
        /// </summary>
        private readonly Bucket[] buckets;

        /// <summary>
        /// Initializes a new instance of the <see cref="StringPool"/> class.
        /// </summary>
        /// <param name="numberOfBuckets">The total number of buckets to use.</param>
        /// <param name="entriesPerBucket">The maximum number of <see cref="string"/> entries per bucket.</param>
        public StringPool(int numberOfBuckets, int entriesPerBucket)
        {
            Span<Bucket> span = this.buckets = new Bucket[numberOfBuckets];

            // We preallocate the buckets in advance, since each bucket only contains the
            // array field, which is not preinitialized, so the allocations are minimal.
            // This lets us lock on each individual buckets when retrieving a string instance.
            foreach (ref Bucket bucket in span)
            {
                bucket = new Bucket();
            }

            NumberOfBuckets = numberOfBuckets;
            EntriesPerBucket = entriesPerBucket;
        }

        /// <summary>
        /// Gets the default <see cref="StringPool"/> instance.
        /// </summary>
        public static StringPool Default { get; } = new StringPool(DefaultNumberOfBuckets, DefaultEntriesPerBucket);

        /// <summary>
        /// Gets the total number of buckets in use.
        /// </summary>
        public int NumberOfBuckets { get; }

        /// <summary>
        /// Gets the maximum number of <see cref="string"/> entries stored in each bucket.
        /// </summary>
        public int EntriesPerBucket { get; }

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

            int bucketIndex = span.Length % NumberOfBuckets;

            Bucket bucket = this.buckets.DangerousGetReferenceAt(bucketIndex);

            lock (bucket)
            {
                return bucket.GetOrAdd(span, EntriesPerBucket);
            }
        }

        /// <summary>
        /// Resets the current instance and its associated buckets.
        /// </summary>
        public void Reset()
        {
            foreach (Bucket bucket in this.buckets)
            {
                lock (bucket)
                {
                    bucket.Reset();
                }
            }
        }

        /// <summary>
        /// A configurable bucket containing a group of cached <see cref="string"/> instances.
        /// </summary>
        private sealed class Bucket
        {
            /// <summary>
            /// A bitmask used to avoid branches when computing the absolute value of an <see cref="int"/>.
            /// This will ignore overflows, as we just need this to map hashcodes into the valid bucket range.
            /// </summary>
            private const int SignMask = ~(1 << 31);

            /// <summary>
            /// The array of entries currently in use.
            /// </summary>
            private string?[]? entries;

            /// <summary>
            /// Implements <see cref="StringPool.GetOrAdd"/> for the current <see cref="Bucket"/> instance.
            /// </summary>
            /// <param name="span">The input <see cref="ReadOnlySpan{T}"/> with the contents to use.</param>
            /// <param name="entriesPerBucket">The number of entries being used in the current instance.</param>
            /// <returns>A <see cref="string"/> instance with the contents of <paramref name="span"/>, cached if possible.</returns>
            public string GetOrAdd(ReadOnlySpan<char> span, int entriesPerBucket)
            {
                ref string?[]? entries = ref this.entries;

                entries ??= new string[entriesPerBucket];

                int entryIndex =
#if NETSTANDARD1_4
                    (span.GetDjb2HashCode() & SignMask) % entriesPerBucket;
#else
                    (HashCode<char>.Combine(span) & SignMask) % entriesPerBucket;
#endif

                ref string? entry = ref entries.DangerousGetReferenceAt(entryIndex);

                if (!(entry is null) &&
                    entry.AsSpan().SequenceEqual(span))
                {
                    return entry;
                }

#if SPAN_RUNTIME_SUPPORT
                return entry = new string(span);
#else
                unsafe
                {
                    fixed (char* c = span)
                    {
                        return entry = new string(c, 0, span.Length);
                    }
                }
#endif
            }

            /// <summary>
            /// Resets the current array of entries.
            /// </summary>
            public void Reset()
            {
                this.entries = null;
            }
        }
    }
}
