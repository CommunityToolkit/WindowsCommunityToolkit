// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;

namespace Microsoft.Collections.Extensions
{
    /// <summary>
    /// An interface providing key type contravariant access to a <see cref="DictionarySlim{TKey,TValue}"/> instance.
    /// </summary>
    /// <typeparam name="TKey">The contravariant type of keys in the dictionary.</typeparam>
    internal interface IDictionarySlim<in TKey>
        where TKey : notnull, IEquatable<TKey>
    {
        /// <summary>
        /// Gets the count of entries in the dictionary.
        /// </summary>
        int Count { get; }

        /// <inheritdoc cref="Dictionary{TKey,TValue}.Remove"/>
        bool Remove(TKey key);
    }
}
