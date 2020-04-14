// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;

namespace Microsoft.Collections.Extensions
{
    /// <summary>
    /// An interface providing value-invariant access to a <see cref="DictionarySlim{TKey,TValue}"/> instance.
    /// </summary>
    /// <typeparam name="TKey">The type of keys in the dictionary.</typeparam>
    /// <typeparam name="TValue">The type of values in the dictionary.</typeparam>
    internal interface IReadOnlyDictionary<in TKey, out TValue>
        where TKey : notnull, IEquatable<TKey>
    {
        /// <summary>
        /// Gets the value with the specified key.
        /// </summary>
        /// <param name="key">The key to look for</param>
        /// <returns>The returned value</returns>
        /// <exception cref="ArgumentException">Thrown if the key wasn't present.</exception>
        TValue this[TKey key] { get; }
    }
}
