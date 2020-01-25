// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Microsoft.Toolkit.Uwp.UI.Media.Extensions.System.Threading.Tasks;
using Windows.UI.Composition;
using Windows.UI.Core;

namespace Microsoft.Toolkit.Uwp.UI.Media.Helpers.Cache
{
    /// <summary>
    /// A <see langword="class"/> used to cache reusable <see cref="CompositionObject"/> instances in each UI thread
    /// </summary>
    /// <typeparam name="T">The type of instances to cache</typeparam>
    internal sealed class CompositionObjectCache<T>
        where T : CompositionObject
    {
        /// <summary>
        /// The cache of weak references, to avoid memory leaks
        /// </summary>
        private readonly ConditionalWeakTable<Compositor, WeakReference<T>> cache = new ConditionalWeakTable<Compositor, WeakReference<T>>();

        /// <summary>
        /// The <see cref="AsyncMutex"/> instance used to synchronize concurrent operations on the cache
        /// </summary>
        private readonly AsyncMutex mutex = new AsyncMutex();

        /// <summary>
        /// Tries to retrieve a valid <typeparamref name="T"/> instance from the cache, and uses the provided factory if an existing item is not found
        /// </summary>
        /// <param name="dispatcher">The current <see cref="CoreDispatcher"/> instance to get the value for</param>
        /// <param name="producer">A <see cref="Func{TResult}"/> instance used to produce a <typeparamref name="T"/> instance</param>
        /// <returns>A <see cref="Task{T}"/> that returns a <typeparamref name="T"/> instance that is linked to <paramref name="dispatcher"/></returns>
        public async Task<T> TryGetInstanceAsync(Compositor dispatcher, Func<Compositor, T> producer)
        {
            using (await this.mutex.LockAsync())
            {
                if (this.cache.TryGetValue(dispatcher, out var reference) &&
                    reference.TryGetTarget(out var instance))
                {
                    return instance;
                }

                // Create a new instance when needed
                var fallback = producer(dispatcher);
                this.cache.AddOrUpdate(dispatcher, new WeakReference<T>(fallback));

                return fallback;
            }
        }
    }
}
