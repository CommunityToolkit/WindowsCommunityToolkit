// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Text;
using CommunityToolkit.Common.Helpers;

namespace CommunityToolkit.Common.Extensions
{
    /// <summary>
    /// Helpers methods for working with <see cref="ISettingsStorageHelper{TKey}"/> implementations.
    /// </summary>
    public static class ISettingsStorageHelperExtensions
    {
        /// <summary>
        /// Attempts to read the provided key and return the value.
        /// If the key is not found, the fallback value will be used instead.
        /// </summary>
        /// <typeparam name="TKey">The type of key used to lookup the object.</typeparam>
        /// <typeparam name="TValue">The type of object value expected.</typeparam>
        /// <param name="storageHelper">The storage helper instance fo read from.</param>
        /// <param name="key">The key of the target object.</param>
        /// <param name="fallback">An alternative value returned if the read fails.</param>
        /// <returns>The value of the target object, or the fallback value.</returns>
        public static TValue? GetValueOrDefault<TKey, TValue>(this ISettingsStorageHelper<TKey> storageHelper, TKey key, TValue? fallback = default)
            where TKey : notnull
        {
            if (storageHelper.TryRead<TValue>(key, out TValue? storedValue))
            {
                return storedValue;
            }
            else
            {
                return fallback;
            }
        }

        /// <summary>
        /// Read the key in the storage helper instance and get the value.
        /// </summary>
        /// <typeparam name="TKey">The type of key used to lookup the object.</typeparam>
        /// <typeparam name="TValue">The type of object value expected.</typeparam>
        /// <param name="storageHelper">The storage helper instance fo read from.</param>
        /// <param name="key">The key of the target object.</param>
        /// <returns>The value of the target object</returns>
        /// <exception cref="KeyNotFoundException">Throws when the key is not found in storage.</exception>
        public static TValue? Read<TKey, TValue>(this ISettingsStorageHelper<TKey> storageHelper, TKey key)
            where TKey : notnull
        {
            if (storageHelper.TryRead<TValue>(key, out TValue? value))
            {
                return value;
            }
            else
            {
                ThrowKeyNotFoundException(key);
                return default;
            }
        }

        /// <summary>
        /// Deletes a key from storage.
        /// </summary>
        /// <typeparam name="TKey">The type of key used to lookup the object.</typeparam>
        /// <param name="storageHelper">The storage helper instance to delete from.</param>
        /// <param name="key">The key of the target object.</param>
        /// <exception cref="KeyNotFoundException">Throws when the key is not found in storage.</exception>
        public static void Delete<TKey>(this ISettingsStorageHelper<TKey> storageHelper, TKey key)
            where TKey : notnull
        {
            if (!storageHelper.TryDelete(key))
            {
                ThrowKeyNotFoundException(key);
            }
        }

        private static void ThrowKeyNotFoundException<TKey>(TKey key)
        {
            throw new KeyNotFoundException($"The given key '{key}' was not present");
        }
    }
}
