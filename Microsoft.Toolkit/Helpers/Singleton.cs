// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Concurrent;

namespace Microsoft.Toolkit.Helpers
{
    /// <summary>
    /// Provides an easy-to-use thread-safe Singleton Pattern via <see cref="System.Collections.Concurrent.ConcurrentDictionary{TKey, TValue}">ConcurrentDictionary</see>.
    /// </summary>
    /// <typeparam name="T">The type to be used for creating the Singleton instance.</typeparam>
    /// <example>
    /// Use by adding a static property to your class for a traditional access pattern:
    /// <code>
    /// // Setup Singleton
    /// public static class MyClass {
    ///     public static MyClass Instance => Singleton&lt;MyClass&gt;.Instance;
    ///
    ///     public void MyMethod() { }
    /// }
    ///
    /// // Use Singleton Instance
    /// MyClass.Instance.MyMethod();
    /// </code>
    /// </example>
    public static class Singleton<T>
        where T : new()
    {
        // Use ConcurrentDictionary for thread safety.
        private static readonly ConcurrentDictionary<Type, T> _instances = new ConcurrentDictionary<Type, T>();

        /// <summary>
        /// Gets the instance of the Singleton class.
        /// </summary>
        public static T Instance
        {
            get
            {
                // Safely creates the first instance or retrieves the existing instance across threads.
                return _instances.GetOrAdd(typeof(T), (t) => new T());
            }
        }
    }
}
