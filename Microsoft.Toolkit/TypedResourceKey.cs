using System;

namespace Microsoft.Toolkit
{
    /// <summary>
    /// A generic classed that can be used to retrieve keyed resources of the specified type.
    /// </summary>
    /// <typeparam name="TValue">The <see cref="Type"/> of resource the <see cref="TypedResourceKey{TValue}"/> will retrieve.</typeparam>
    public class TypedResourceKey<TValue>
    {
        /// <summary>
        /// Create a new <see cref="TypedResourceKey{TValue}"/> with the specified key
        /// </summary>
        /// <param name="key">The resource's key</param>
        public TypedResourceKey(string key) => Key = key;

        /// <summary>
        /// The key of the resource to be retrieved.
        /// </summary>
        public string Key { get; }

        public static implicit operator TypedResourceKey<TValue>(string key)
        {
            return new TypedResourceKey<TValue>(key);
        }
    }
}