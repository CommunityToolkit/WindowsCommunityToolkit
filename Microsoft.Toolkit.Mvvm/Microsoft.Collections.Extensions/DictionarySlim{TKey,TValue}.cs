// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Microsoft.Collections.Extensions
{
    /// <summary>
    /// A lightweight Dictionary with three principal differences compared to <see cref="Dictionary{TKey, TValue}"/>
    ///
    /// 1) It is possible to do "get or add" in a single lookup using <see cref="GetOrAddValueRef(TKey)"/>. For
    /// values that are value types, this also saves a copy of the value.
    /// 2) It assumes it is cheap to equate values.
    /// 3) It assumes the keys implement <see cref="IEquatable{TKey}"/> or else Equals() and they are cheap and sufficient.
    /// </summary>
    /// <typeparam name="TKey">The type of keys in the dictionary.</typeparam>
    /// <typeparam name="TValue">The type of values in the dictionary.</typeparam>
    /// <remarks>
    /// 1) This avoids having to do separate lookups (<see cref="Dictionary{TKey, TValue}.TryGetValue(TKey, out TValue)"/>
    /// followed by <see cref="Dictionary{TKey, TValue}.Add(TKey, TValue)"/>.
    /// There is not currently an API exposed to get a value by ref without adding if the key is not present.
    /// 2) This means it can save space by not storing hash codes.
    /// 3) This means it can avoid storing a comparer, and avoid the likely virtual call to a comparer.
    /// </remarks>
    [DebuggerDisplay("Count = {Count}")]
    internal sealed class DictionarySlim<TKey, TValue> : IDictionary<TKey>, IReadOnlyDictionary<TKey, TValue>
        where TKey : notnull, IEquatable<TKey>
    {
        // See info in CoreFX labs for how this works
        private static readonly Entry[] InitialEntries = new Entry[1];
        private int _count;
        private int _freeList = -1;
        private int[] _buckets;
        private Entry[] _entries;

        private struct Entry
        {
            public TKey Key;
            public TValue Value;
            public int Next;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DictionarySlim{TKey, TValue}"/> class.
        /// </summary>
        public DictionarySlim()
        {
            _buckets = new int[1];
            _entries = InitialEntries;
        }

        /// <summary>
        /// Gets the count of entries in the dictionary.
        /// </summary>
        public int Count => _count;

        /// <inheritdoc/>
        public TValue this[TKey key]
        {
            get
            {
                Entry[] entries = _entries;

                for (int i = _buckets[key.GetHashCode() & (_buckets.Length - 1)] - 1;
                    (uint)i < (uint)entries.Length;
                    i = entries[i].Next)
                {
                    if (key.Equals(entries[i].Key))
                    {
                        return entries[i].Value;
                    }
                }

                ThrowArgumentExceptionForKeyNotFound(key);

                return default!;
            }
        }

        /// <inheritdoc cref="Dictionary{TKey,TValue}.ContainsKey"/>
        public bool ContainsKey(TKey key)
        {
            Entry[] entries = _entries;

            for (int i = _buckets[key.GetHashCode() & (_buckets.Length - 1)] - 1;
                (uint)i < (uint)entries.Length; i = entries[i].Next)
            {
                if (key.Equals(entries[i].Key))
                {
                    return true;
                }
            }

            return false;
        }

        /// <inheritdoc/>
        public bool Remove(TKey key)
        {
            Entry[] entries = _entries;
            int bucketIndex = key.GetHashCode() & (_buckets.Length - 1);
            int entryIndex = _buckets[bucketIndex] - 1;
            int lastIndex = -1;

            while (entryIndex != -1)
            {
                Entry candidate = entries[entryIndex];
                if (candidate.Key.Equals(key))
                {
                    if (lastIndex != -1)
                    {
                        entries[lastIndex].Next = candidate.Next;
                    }
                    else
                    {
                        _buckets[bucketIndex] = candidate.Next + 1;
                    }

                    entries[entryIndex] = default;

                    entries[entryIndex].Next = -3 - _freeList;
                    _freeList = entryIndex;

                    _count--;
                    return true;
                }

                lastIndex = entryIndex;
                entryIndex = candidate.Next;
            }

            return false;
        }

        /// <summary>
        /// Gets the value for the specified key, or, if the key is not present,
        /// adds an entry and returns the value by ref. This makes it possible to
        /// add or update a value in a single look up operation.
        /// </summary>
        /// <param name="key">Key to look for</param>
        /// <returns>Reference to the new or existing value</returns>
        public ref TValue GetOrAddValueRef(TKey key)
        {
            Entry[] entries = _entries;
            int bucketIndex = key.GetHashCode() & (_buckets.Length - 1);

            for (int i = _buckets[bucketIndex] - 1;
                 (uint)i < (uint)entries.Length;
                 i = entries[i].Next)
            {
                if (key.Equals(entries[i].Key))
                {
                    return ref entries[i].Value;
                }
            }

            return ref AddKey(key, bucketIndex);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private ref TValue AddKey(TKey key, int bucketIndex)
        {
            Entry[] entries = _entries;
            int entryIndex;

            if (_freeList != -1)
            {
                entryIndex = _freeList;
                _freeList = -3 - entries[_freeList].Next;
            }
            else
            {
                if (_count == entries.Length || entries.Length == 1)
                {
                    entries = Resize();
                    bucketIndex = key.GetHashCode() & (_buckets.Length - 1);
                }

                entryIndex = _count;
            }

            entries[entryIndex].Key = key;
            entries[entryIndex].Next = _buckets[bucketIndex] - 1;

            _buckets[bucketIndex] = entryIndex + 1;
            _count++;

            return ref entries[entryIndex].Value;
        }

        /// <summary>
        /// Resizes the current dictionary to reduce the number of collisions
        /// </summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        private Entry[] Resize()
        {
            int count = _count;
            int newSize = _entries.Length * 2;

            if ((uint)newSize > int.MaxValue)
            {
                throw new InvalidOperationException("Max capacity exceeded");
            }

            var entries = new Entry[newSize];

            Array.Copy(_entries, 0, entries, 0, count);

            var newBuckets = new int[entries.Length];

            while (count-- > 0)
            {
                int bucketIndex = entries[count].Key.GetHashCode() & (newBuckets.Length - 1);
                entries[count].Next = newBuckets[bucketIndex] - 1;
                newBuckets[bucketIndex] = count + 1;
            }

            _buckets = newBuckets;
            _entries = entries;

            return entries;
        }

        /// <inheritdoc cref="IEnumerable{T}.GetEnumerator"/>
        public Enumerator GetEnumerator() => new Enumerator(this);

        /// <summary>
        /// Enumerator for <see cref="DictionarySlim{TKey,TValue}"/>.
        /// </summary>
        public ref struct Enumerator
        {
            private readonly DictionarySlim<TKey, TValue> _dictionary;
            private int _index;
            private int _count;
            private KeyValuePair<TKey, TValue> _current;

            internal Enumerator(DictionarySlim<TKey, TValue> dictionary)
            {
                _dictionary = dictionary;
                _index = 0;
                _count = _dictionary._count;
                _current = default;
            }

            /// <inheritdoc cref="IEnumerator.MoveNext"/>
            public bool MoveNext()
            {
                if (_count == 0)
                {
                    _current = default;
                    return false;
                }

                _count--;

                while (_dictionary._entries[_index].Next < -1)
                {
                    _index++;
                }

                _current = new KeyValuePair<TKey, TValue>(
                    _dictionary._entries[_index].Key,
                    _dictionary._entries[_index++].Value);
                return true;
            }

            /// <inheritdoc cref="IEnumerator{T}.Current"/>
            public KeyValuePair<TKey, TValue> Current => _current;
        }

        /// <summary>
        /// Throws an <see cref="ArgumentException"/> when trying to load an element with a missing key.
        /// </summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        private static void ThrowArgumentExceptionForKeyNotFound(TKey key)
        {
            throw new ArgumentException($"The target key {key} was not present in the dictionary");
        }
    }
}