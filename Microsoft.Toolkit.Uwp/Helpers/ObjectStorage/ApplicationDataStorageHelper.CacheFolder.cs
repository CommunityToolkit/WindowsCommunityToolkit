// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Toolkit.Helpers;
using Windows.Storage;

namespace Microsoft.Toolkit.Uwp.Helpers
{
    /// <summary>
    /// An extension of ApplicationDataStorageHelper with additional features for interop with the LocalCacheFolder.
    /// </summary>
    public partial class ApplicationDataStorageHelper
    {
        /// <summary>
        ///  Gets the local cache folder.
        /// </summary>
        public StorageFolder CacheFolder => AppData.LocalCacheFolder;

        /// <summary>
        /// Retrieves an object from a file in the LocalCacheFolder.
        /// </summary>
        /// <typeparam name="T">Type of object retrieved.</typeparam>
        /// <param name="filePath">Path to the file that contains the object.</param>
        /// <param name="default">Default value of the object.</param>
        /// <returns>Waiting task until completion with the object in the file.</returns>
        public Task<T> ReadCacheFileAsync<T>(string filePath, T @default = default)
        {
            return ReadFileAsync<T>(CacheFolder, filePath, @default);
        }

        /// <summary>
        /// Retrieves the listings for a folder and the item types in the LocalCacheFolder.
        /// </summary>
        /// <param name="folderPath">The path to the target folder.</param>
        /// <returns>A list of file types and names in the target folder.</returns>
        public Task<IEnumerable<(DirectoryItemType ItemType, string Name)>> ReadCacheFolderAsync(string folderPath)
        {
            return ReadFolderAsync(CacheFolder, folderPath);
        }

        /// <summary>
        /// Saves an object inside a file in the LocalCacheFolder.
        /// </summary>
        /// <typeparam name="T">Type of object saved.</typeparam>
        /// <param name="filePath">Path to the file that will contain the object.</param>
        /// <param name="value">Object to save.</param>
        /// <returns>Waiting task until completion.</returns>
        public Task CreateCacheFileAsync<T>(string filePath, T value)
        {
            return CreateFileAsync<T>(CacheFolder, filePath, value);
        }

        /// <summary>
        /// Ensure a folder exists at the folder path specified in the LocalCacheFolder.
        /// </summary>
        /// <param name="folderPath">The path and name of the target folder.</param>
        /// <returns>Waiting task until completion.</returns>
        public Task CreateCacheFolderAsync(string folderPath)
        {
            return CreateFolderAsync(CacheFolder, folderPath);
        }

        /// <summary>
        /// Deletes a file or folder item in the LocalCacheFolder.
        /// </summary>
        /// <param name="itemPath">The path to the item for deletion.</param>
        /// <returns>Waiting task until completion.</returns>
        public Task<bool> TryDeleteCacheItemAsync(string itemPath)
        {
            return TryDeleteItemAsync(CacheFolder, itemPath);
        }

        /// <summary>
        /// Rename an item in the LocalCacheFolder.
        /// </summary>
        /// <param name="itemPath">The path to the target item.</param>
        /// <param name="newName">The new nam for the target item.</param>
        /// <returns>Waiting task until completion.</returns>
        public Task<bool> TryRenameCacheItemAsync(string itemPath, string newName)
        {
            return TryRenameItemAsync(CacheFolder, itemPath, newName);
        }
    }
}
