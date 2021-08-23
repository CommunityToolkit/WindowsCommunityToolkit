// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CommunityToolkit.Common.Helpers
{
    /// <summary>
    /// Service interface used to store data in a directory/file-system via files and folders.
    ///
    /// This interface is meant to help abstract file storage operations across platforms in a library,
    /// but the actual behavior will be up to the implementer. Such as, we don't provide a sense of a current directory,
    /// so an implementor should consider using full paths to support any file operations. Otherwise, a "directory aware"
    /// implementation could be achieved with a current directory field and traversal functions, in which case relative paths would be applicable.
    /// </summary>
    public interface IFileStorageHelper
    {
        /// <summary>
        /// Retrieves an object from a file.
        /// </summary>
        /// <typeparam name="T">Type of object retrieved.</typeparam>
        /// <param name="filePath">Path to the file that contains the object.</param>
        /// <param name="default">Default value of the object.</param>
        /// <returns>Waiting task until completion with the object in the file.</returns>
        Task<T?> ReadFileAsync<T>(string filePath, T? @default = default);

        /// <summary>
        /// Retrieves the listings for a folder and the item types.
        /// </summary>
        /// <param name="folderPath">The path to the target folder.</param>
        /// <returns>A list of item types and names in the target folder.</returns>
        Task<IEnumerable<(DirectoryItemType ItemType, string Name)>> ReadFolderAsync(string folderPath);

        /// <summary>
        /// Saves an object inside a file.
        /// </summary>
        /// <typeparam name="T">Type of object saved.</typeparam>
        /// <param name="filePath">Path to the file that will contain the object.</param>
        /// <param name="value">Object to save.</param>
        /// <returns>Waiting task until completion.</returns>
        Task CreateFileAsync<T>(string filePath, T value);

        /// <summary>
        /// Ensure a folder exists at the folder path specified.
        /// </summary>
        /// <param name="folderPath">The path and name of the target folder.</param>
        /// <returns>Waiting task until completion.</returns>
        Task CreateFolderAsync(string folderPath);

        /// <summary>
        /// Deletes a file or folder item.
        /// </summary>
        /// <param name="itemPath">The path to the item for deletion.</param>
        /// <returns>Waiting task until completion.</returns>
        Task<bool> TryDeleteItemAsync(string itemPath);

        /// <summary>
        /// Rename an item.
        /// </summary>
        /// <param name="itemPath">The path to the target item.</param>
        /// <param name="newName">The new nam for the target item.</param>
        /// <returns>Waiting task until completion.</returns>
        Task<bool> TryRenameItemAsync(string itemPath, string newName);
    }
}
