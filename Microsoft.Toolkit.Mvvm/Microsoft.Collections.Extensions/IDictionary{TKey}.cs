using System;
using System.Collections.Generic;

namespace Microsoft.Collections.Extensions
{
    /// <summary>
    /// An interface providing value-invariant access to a <see cref="DictionarySlim{TKey,TValue}"/> instance.
    /// </summary>
    /// <typeparam name="TKey">The type of keys in the dictionary.</typeparam>
    internal interface IDictionary<in TKey>
        where TKey : struct, IEquatable<TKey>
    {
        /// <inheritdoc cref="Dictionary{TKey,TValue}.Remove"/>
        bool Remove(TKey key);
    }
}
