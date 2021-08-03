// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;

namespace CommunityToolkit.Common.Helpers
{
    /// <summary>
    /// Service interface used to store data using key value pairs.
    /// </summary>
    /// <typeparam name="TKey">The type of keys to use for accessing values.</typeparam>
    public interface ISettingsStorageHelper<in TKey>
        where TKey : notnull
    {
        /// <summary>
        /// Retrieves a single item by its key.
        /// </summary>
        /// <typeparam name="TValue">Type of object retrieved.</typeparam>
        /// <param name="key">Key of the object.</param>
        /// <param name="value">The <see typeparamref="TValue"/> object for <see typeparamref="TKey"/> key.</param>
        /// <returns>A boolean indicator of success.</returns>
        bool TryRead<TValue>(TKey key, out TValue? value);

        /// <summary>
        /// Saves a single item by its key.
        /// </summary>
        /// <typeparam name="TValue">Type of object saved.</typeparam>
        /// <param name="key">Key of the value saved.</param>
        /// <param name="value">Object to save.</param>
        void Save<TValue>(TKey key, TValue value);

        /// <summary>
        /// Deletes a single item by its key.
        /// </summary>
        /// <param name="key">Key of the object.</param>
        /// <returns>A boolean indicator of success.</returns>
        bool TryDelete(TKey key);

        /// <summary>
        /// Clear all keys and values from the settings store.
        /// </summary>
        void Clear();
    }
}
