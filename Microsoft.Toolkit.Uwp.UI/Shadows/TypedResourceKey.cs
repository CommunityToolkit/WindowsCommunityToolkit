// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;

namespace Microsoft.Toolkit.Uwp.UI
{
    /// <summary>
    /// A generic class that can be used to retrieve keyed resources of the specified type.
    /// </summary>
    /// <typeparam name="TValue">The <see cref="Type"/> of resource the <see cref="TypedResourceKey{TValue}"/> will retrieve.</typeparam>
    internal sealed class TypedResourceKey<TValue>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TypedResourceKey{TValue}"/> class  with the specified key.
        /// </summary>
        /// <param name="key">The resource's key</param>
        public TypedResourceKey(string key) => Key = key;

        /// <summary>
        /// Gets the key of the resource to be retrieved.
        /// </summary>
        public string Key { get; }

        /// <summary>
        /// Implicit operator for transforming a string into a <see cref="TypedResourceKey{TValue}"/> key.
        /// </summary>
        /// <param name="key">The key string.</param>
        public static implicit operator TypedResourceKey<TValue>(string key)
        {
            return new TypedResourceKey<TValue>(key);
        }
    }
}
