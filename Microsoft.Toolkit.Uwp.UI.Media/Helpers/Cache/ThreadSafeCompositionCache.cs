// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Toolkit.Uwp.UI.Media.Extensions.System.Threading.Tasks;
using Microsoft.Toolkit.Uwp.UI.Media.Extensions.Windows.UI.Composition;
using Windows.UI.Composition;
using Windows.UI.Core;

namespace Microsoft.Toolkit.Uwp.UI.Media.Helpers.Cache
{
    /// <summary>
    /// A <see langword="class"/> used to cache reusable <see cref="CompositionObject"/> instances
    /// </summary>
    /// <typeparam name="T">The type of <see cref="CompositionObject"/> instances to cache</typeparam>
    internal sealed class ThreadSafeCompositionCache<T>
        where T : CompositionObject
    {
        /// <summary>
        /// The cache of weak references, to avoid memory leaks
        /// </summary>
        private readonly List<WeakReference<T>> cache = new List<WeakReference<T>>();

        /// <summary>
        /// The <see cref="AsyncMutex"/> instance used to synchronize concurrent operations on the cache
        /// </summary>
        private readonly AsyncMutex mutex = new AsyncMutex();

        /// <summary>
        /// Tries to retrieve a valid instance from the cache, and uses the provided factory if an existing item is not found
        /// </summary>
        /// <param name="factory">A <see cref="Func{TResult}"/> used when the requested value is not present in the cache</param>
        /// <returns>A <see cref="Task{T}"/> with the retrieved <typeparamref name="T"/> instance</returns>
        public async Task<T> TryGetInstanceAsync(Func<T> factory)
        {
            using (await this.mutex.LockAsync())
            {
                // Try to retrieve an valid instance from the cache
                foreach (var value in this.cache)
                {
                    if (value.TryGetTarget(out T instance) &&
                        instance.TryGetDispatcher(out CoreDispatcher dispatcher) && dispatcher.HasThreadAccess)
                    {
                        return instance;
                    }
                }

                // Create a new instance when needed
                T fallback = factory();
                this.cache.Add(new WeakReference<T>(fallback));
                return fallback;
            }
        }

        /// <summary>
        /// Performs a cleanup of the cache by removing invalid references
        /// </summary>
        public async void Cleanup()
        {
            using (await this.mutex.LockAsync())
            {
                this.cache.RemoveAll(reference => !reference.TryGetTarget(out T target) || !target.TryGetDispatcher(out _));
            }
        }
    }
}
