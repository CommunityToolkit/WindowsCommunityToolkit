// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Concurrent;

namespace Microsoft.Toolkit.Helpers
{
    /// <summary>
    /// Obsolete see https://github.com/windows-toolkit/WindowsCommunityToolkit/issues/3134.
    /// </summary>
    /// <typeparam name="T">The type to be used for creating the Singleton instance.</typeparam>
    /// <example>
    /// Instead of this helper, migrate your code to this pattern instead:
    /// <code>
    /// // Setup Singleton
    /// public class MyClass
    /// {
    ///     public static MyClass Instance { get; } = new MyClass();
    /// }
    /// </code>
    /// </example>
    [Obsolete("This helper will be removed in a future release, see example tag for code replacement. https://github.com/windows-toolkit/WindowsCommunityToolkit/issues/3134")]
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
