// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;
using Microsoft.Toolkit.HighPerformance.Extensions;
#if !NETSTANDARD1_4
using Microsoft.Toolkit.HighPerformance.Helpers;
#endif

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
        public StringPool()
            : this(DefaultNumberOfBuckets, DefaultEntriesPerBucket)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StringPool"/> class.
        /// </summary>
        /// <param name="numberOfBuckets">The total number of buckets to use.</param>
        /// <param name="entriesPerBucket">The maximum number of <see cref="string"/> entries per bucket.</param>
        public StringPool(int numberOfBuckets, int entriesPerBucket)
        {
            if (numberOfBuckets <= 0)
            {
                ThrowArgumentOutOfRangeException(nameof(numberOfBuckets));
            }

            if (entriesPerBucket <= 0)
            {
                ThrowArgumentOutOfRangeException(nameof(entriesPerBucket));
            }

            Span<Bucket> span = this.buckets = new Bucket[numberOfBuckets];

            // We preallocate the buckets in advance, since each bucket only contains the
            // array field, which is not preinitialized, so the allocations are minimal.
            // This lets us lock on each individual buckets when retrieving a string instance.
            foreach (ref Bucket bucket in span)
            {
                bucket = new Bucket(entriesPerBucket);
            }

            NumberOfBuckets = numberOfBuckets;
            EntriesPerBucket = entriesPerBucket;
        }

        /// <summary>
        /// Gets the default <see cref="StringPool"/> instance.
        /// </summary>
        public static StringPool Default { get; } = new StringPool();

        /// <summary>
        /// Gets the total number of buckets in use.
        /// </summary>
        public int NumberOfBuckets { get; }

        /// <summary>
        /// Gets the maximum number of <see cref="string"/> entries stored in each bucket.
        /// </summary>
        public int EntriesPerBucket { get; }

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

            int bucketIndex = value.Length % NumberOfBuckets;

            ref Bucket bucket = ref this.buckets.DangerousGetReferenceAt(bucketIndex);

            bucket.Add(value);
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

            int bucketIndex = span.Length % NumberOfBuckets;

            ref Bucket bucket = ref this.buckets.DangerousGetReferenceAt(bucketIndex);

            return bucket.GetOrAdd(span);
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

            int bucketIndex = span.Length % NumberOfBuckets;

            ref Bucket bucket = ref this.buckets.DangerousGetReferenceAt(bucketIndex);

            return bucket.TryGet(span, out value);
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
            /// A bitmask used to avoid branches when computing the absolute value of an <see cref="int"/>.
            /// This will ignore overflows, as we just need this to map hashcodes into the valid bucket range.
            /// </summary>
            private const int SignMask = ~(1 << 31);

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
            public void Add(string value)
            {
                lock (this.dummy)
                {
                    ref string?[]? entries = ref this.entries;

                    entries ??= new string[entriesPerBucket];

                    int entryIndex = GetIndex(value.AsSpan());

                    entries.DangerousGetReferenceAt(entryIndex) = value;
                }
            }

            /// <summary>
            /// Implements <see cref="StringPool.GetOrAdd"/> for the current <see cref="Bucket"/> instance.
            /// </summary>
            /// <param name="span">The input <see cref="ReadOnlySpan{T}"/> with the contents to use.</param>
            /// <returns>A <see cref="string"/> instance with the contents of <paramref name="span"/>, cached if possible.</returns>
            public string GetOrAdd(ReadOnlySpan<char> span)
            {
                lock (this.dummy)
                {
                    ref string?[]? entries = ref this.entries;

                    entries ??= new string[entriesPerBucket];

                    int entryIndex = GetIndex(span);

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
            }

            /// <summary>
            /// Implements <see cref="StringPool.TryGet"/> for the current <see cref="Bucket"/> instance.
            /// </summary>
            /// <param name="span">The input <see cref="ReadOnlySpan{T}"/> with the contents to use.</param>
            /// <param name="value">The resulting cached <see cref="string"/> instance, if present</param>
            /// <returns>Whether or not the target <see cref="string"/> instance was found.</returns>
            public bool TryGet(ReadOnlySpan<char> span, [NotNullWhen(true)] out string? value)
            {
                lock (this.dummy)
                {
                    ref string?[]? entries = ref this.entries;

                    if (!(entries is null))
                    {
                        int entryIndex = GetIndex(span);

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

            /// <summary>
            /// Gets the target index for a given <see cref="ReadOnlySpan{T}"/> instance.
            /// </summary>
            /// <param name="span">The input <see cref="ReadOnlySpan{T}"/> instance.</param>
            /// <returns>The target bucket index for <paramref name="span"/>.</returns>
            [Pure]
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            private int GetIndex(ReadOnlySpan<char> span)
            {
#if NETSTANDARD1_4
                return (span.GetDjb2HashCode() & SignMask) % entriesPerBucket;
#else
                return (HashCode<char>.Combine(span) & SignMask) % entriesPerBucket;
#endif
            }
        }

        /// <summary>
        /// Throws an <see cref="ArgumentException"/> when the requested size exceeds the capacity.
        /// </summary>
        private static void ThrowArgumentOutOfRangeException(string name)
        {
            throw new ArgumentOutOfRangeException(name, "The input parameter must be greater than 0");
        }
    }
}
