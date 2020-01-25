// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Toolkit.Uwp.UI.Media.Extensions.Windows.UI.Composition;
using Windows.UI.Composition;
using Windows.UI.Core;

namespace Microsoft.Toolkit.Uwp.UI.Media.Helpers.Cache
{
    /// <summary>
    /// A <see langword="class"/> used to cache reusable <see cref="CompositionObject"/> instances with an associated key
    /// </summary>
    /// <typeparam name="TKey">The type of key to classify the items in the cache</typeparam>
    /// <typeparam name="TValue">The type of items stored in the cache</typeparam>
    internal sealed class ThreadSafeCompositionMapCache<TKey, TValue>
        where TValue : CompositionObject
    {
        /// <summary>
        /// The cache of weak references, to avoid memory leaks
        /// </summary>
        private readonly Dictionary<TKey, List<WeakReference<TValue>>> cache = new Dictionary<TKey, List<WeakReference<TValue>>>();

        /// <summary>
        /// Tries to retrieve a valid instance from the cache, and uses the provided factory if an existing item is not found
        /// </summary>
        /// <param name="key">The key to look for</param>
        /// <param name="result">The resulting value, if existing</param>
        /// <returns><see langword="true"/> if the target value has been found, <see langword="false"/> otherwise</returns>
        public bool TryGetInstance(TKey key, out TValue result)
        {
            // Try to retrieve an valid instance from the cache
            if (this.cache.TryGetValue(key, out var values))
            {
                foreach (var value in values)
                {
                    if (value.TryGetTarget(out TValue instance) && instance.TryGetDispatcher(out CoreDispatcher dispatcher) && dispatcher.HasThreadAccess)
                    {
                        result = instance;
                        return true;
                    }
                }
            }

            // Not found
            result = null;
            return false;
        }

        /// <summary>
        /// Adds a new value with the specified key to the cache
        /// </summary>
        /// <param name="key">The key of the item to add</param>
        /// <param name="value">The value to add</param>
        public void Add(TKey key, TValue value)
        {
            if (this.cache.TryGetValue(key, out var list))
            {
                list.Add(new WeakReference<TValue>(value));
            }
            else
            {
                this.cache.Add(key, new List<WeakReference<TValue>> { new WeakReference<TValue>(value) });
            }
        }

        /// <summary>
        /// Adds a new value and removes previous values with the same key, if any
        /// </summary>
        /// <param name="key">The key of the item to add</param>
        /// <param name="value">The value to add</param>
        public void Overwrite(TKey key, TValue value)
        {
            this.cache.Remove(key);

            this.cache.Add(key, new List<WeakReference<TValue>> { new WeakReference<TValue>(value) });
        }

        /// <summary>
        /// Performs a cleanup of the cache by removing invalid references
        /// </summary>
        public void Cleanup()
        {
            foreach (var list in this.cache.Values)
            {
                list.RemoveAll(reference => !reference.TryGetTarget(out TValue value) || !value.TryGetDispatcher(out _));
            }

            foreach (var key in this.cache.Keys.ToArray())
            {
                if (this.cache[key].Count == 0)
                {
                    this.cache.Remove(key);
                }
            }
        }

        /// <summary>
        /// Clears the cache by removing all the stored items
        /// </summary>
        /// <returns>An <see cref="IReadOnlyList{T}"/> instance with the existing values before the cleanup</returns>
        public IReadOnlyList<TValue> Clear()
        {
            var values = new List<TValue>();

            foreach (var reference in this.cache.Values.SelectMany(list => list))
            {
                if (reference.TryGetTarget(out TValue value))
                {
                    values.Add(value);
                }
            }

            this.cache.Clear();

            return values;
        }
    }
}