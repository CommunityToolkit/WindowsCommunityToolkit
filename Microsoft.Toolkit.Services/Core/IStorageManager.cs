// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Threading.Tasks;

namespace Microsoft.Toolkit.Services.Core
{
    /// <summary>
    /// This interface store the key value
    /// </summary>
    public interface IStorageManager
    {
        /// <summary>
        /// Gets the key value
        /// </summary>
        /// <param name="key"> Token value </param>
        /// <returns> Returns a string value</returns>
        Task<string> GetAsync(string key);

        /// <summary>
        /// Sets the key value
        /// </summary>
        /// <param name="key"> Token key </param>
        /// <param name="value"> String value </param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task SetAsync(string key, string value);
    }
}
