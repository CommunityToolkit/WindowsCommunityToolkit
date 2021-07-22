// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Toolkit.Helpers;

namespace Microsoft.Toolkit.Extensions
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
            try
            {
                return storageHelper.Read<TValue>(key);
            }
            catch (KeyNotFoundException)
            {
                return fallback;
            }
        }

        /// <summary>
        /// Attempts to perform read the provided key and returns a boolean indicator of success.
        /// If the key is not found, the fallback value will be used instead.
        /// </summary>
        /// <typeparam name="TKey">The type of key used to lookup the object.</typeparam>
        /// <typeparam name="TValue">The type of object value expected.</typeparam>
        /// <param name="storageHelper">The storage helper instance to read from.</param>
        /// <param name="key">The key of the target object.</param>
        /// <param name="value">The value of the target object, or the fallback value.</param>
        /// <param name="fallback">An alternate value returned if the read fails.</param>
        /// <returns>A boolean indicator of success.</returns>
        public static bool TryRead<TKey, TValue>(this ISettingsStorageHelper<TKey> storageHelper, TKey key, out TValue? value, TValue? fallback = default)
            where TKey : notnull
        {
            try
            {
                value = storageHelper.Read<TValue>(key);
                return true;
            }
            catch (KeyNotFoundException)
            {
                value = fallback;
                return false;
            }
        }

        /// <summary>
        /// Attempts to remove an object by key and returns a boolean indicator of success.
        /// If the key is not found, the method will return false.
        /// </summary>
        /// <typeparam name="TKey">The type of key used to lookup the object.</typeparam>
        /// <param name="storageHelper">The storage helper instance to delete from.</param>
        /// <param name="key">The key of the target object.</param>
        /// <returns>A boolean indicator of success.</returns>
        public static bool TryDelete<TKey>(this ISettingsStorageHelper<TKey> storageHelper, TKey key)
            where TKey : notnull
        {
            try
            {
                storageHelper.Delete(key);
                return true;
            }
            catch (KeyNotFoundException)
            {
                return false;
            }
        }
    }
}
