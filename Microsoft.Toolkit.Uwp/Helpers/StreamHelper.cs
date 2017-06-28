﻿// ******************************************************************
// Copyright (c) Microsoft. All rights reserved.
// This code is licensed under the MIT License (MIT).
// THE CODE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
// TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH
// THE CODE OR THE USE OR OTHER DEALINGS IN THE CODE.
// ******************************************************************

using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.Web.Http;

namespace Microsoft.Toolkit.Uwp
{
    /// <summary>
    /// This class provides static helper methods for streams.
    /// </summary>
    public static class StreamHelper
    {
        /// <summary>
        /// Get the response stream returned by a HTTP get request.
        /// </summary>
        /// <param name="uri">Uri to request.</param>
        /// <param name="cancellationToken">instance of <see cref="CancellationToken"/></param>
        /// <returns>Response stream</returns>
        public static async Task<IRandomAccessStream> GetHttpStreamAsync(this Uri uri, CancellationToken cancellationToken = default(CancellationToken))
        {
            var outputStream = new InMemoryRandomAccessStream();

            using (var request = new HttpHelperRequest(uri, HttpMethod.Get))
            {
                using (var response = await HttpHelper.Instance.SendRequestAsync(request, cancellationToken).ConfigureAwait(false))
                {
                    if (response.Success)
                    {
                        await response.Content.WriteToStreamAsync(outputStream).AsTask().ConfigureAwait(false);

                        outputStream.Seek(0);
                    }
                }
            }

            return outputStream;
        }

        /// <summary>
        /// Get the response stream returned by a HTTP get request and save it to a local file.
        /// </summary>
        /// <param name="uri">Uri to request.</param>
        /// <param name="targetFile">StorageFile to save the stream to.</param>
        /// <returns>True if success.</returns>
        public static async Task GetHttpStreamToStorageFileAsync(
            this Uri uri,
            StorageFile targetFile)
        {
            using (var fileStream = await targetFile.OpenAsync(FileAccessMode.ReadWrite).AsTask().ConfigureAwait(false))
            {
                using (var request = new HttpHelperRequest(uri, HttpMethod.Get))
                {
                    using (var response = await HttpHelper.Instance.SendRequestAsync(request).ConfigureAwait(false))
                    {
                        if (response.Success)
                        {
                            await response.Content.WriteToStreamAsync(fileStream).AsTask().ConfigureAwait(false);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Return a stream to a specified file from the installation folder.
        /// </summary>
        /// <param name="fileName">Relative name of the file to open. Can contains subfolders.</param>
        /// <param name="accessMode">File access mode. Default is read.</param>
        /// <returns>File stream</returns>
        public static Task<IRandomAccessStream> GetPackagedFileStreamAsync(
            string fileName,
            FileAccessMode accessMode = FileAccessMode.Read)
        {
            StorageFolder workingFolder = Package.Current.InstalledLocation;
            return GetFileStreamAsync(fileName, accessMode, workingFolder);
        }

        /// <summary>
        /// Return a stream to a specified file from the application local folder.
        /// </summary>
        /// <param name="fileName">Relative name of the file to open. Can contains subfolders.</param>
        /// <param name="accessMode">File access mode. Default is read.</param>
        /// <returns>File stream</returns>
        public static Task<IRandomAccessStream> GetLocalFileStreamAsync(
            string fileName,
            FileAccessMode accessMode = FileAccessMode.Read)
        {
            StorageFolder workingFolder = ApplicationData.Current.LocalFolder;
            return GetFileStreamAsync(fileName, accessMode, workingFolder);
        }

        /// <summary>
        /// Return a stream to a specified file from the application local cache folder.
        /// </summary>
        /// <param name="fileName">Relative name of the file to open. Can contains subfolders.</param>
        /// <param name="accessMode">File access mode. Default is read.</param>
        /// <returns>File stream</returns>
        public static Task<IRandomAccessStream> GetLocalCacheFileStreamAsync(
            string fileName,
            FileAccessMode accessMode = FileAccessMode.Read)
        {
            StorageFolder workingFolder = ApplicationData.Current.LocalCacheFolder;
            return GetFileStreamAsync(fileName, accessMode, workingFolder);
        }

        /// <summary>
        /// Return a stream to a specified file from the application local cache folder.
        /// </summary>
        /// <param name="knownFolderId">The well known folder ID to use</param>
        /// <param name="fileName">Relative name of the file to open. Can contains subfolders.</param>
        /// <param name="accessMode">File access mode. Default is read.</param>
        /// <returns>File stream</returns>
        public static Task<IRandomAccessStream> GetKnowFoldersFileStreamAsync(
            KnownFolderId knownFolderId,
            string fileName,
            FileAccessMode accessMode = FileAccessMode.Read)
        {
            StorageFolder workingFolder = StorageFileHelper.GetFolderFromKnownFolderId(knownFolderId);
            return GetFileStreamAsync(fileName, accessMode, workingFolder);
        }

        /// <summary>
        /// Test if a file exists in the application installation folder.
        /// </summary>
        /// <param name="fileName">Relative name of the file to open. Can contains subfolders.</param>
        /// <returns>True if file exists.</returns>
        [Obsolete("Use StorageFileHelper.FileExistsAsync instead.")]
        public static Task<bool> IsPackagedFileExistsAsync(string fileName)
        {
            StorageFolder workingFolder = Package.Current.InstalledLocation;
            return workingFolder.IsFileExistsAsync(fileName);
        }

        /// <summary>
        /// Test if a file exists in the application local folder.
        /// </summary>
        /// <param name="fileName">Relative name of the file to open. Can contains subfolders.</param>
        /// <returns>True if file exists.</returns>
        [Obsolete("Use StorageFileHelper.FileExistsAsync instead.")]
        public static Task<bool> IsLocalFileExistsAsync(string fileName)
        {
            StorageFolder workingFolder = ApplicationData.Current.LocalFolder;
            return workingFolder.IsFileExistsAsync(fileName);
        }

        /// <summary>
        /// Test if a file exists in the application local cache folder.
        /// </summary>
        /// <param name="fileName">Relative name of the file to open. Can contains subfolders.</param>
        /// <returns>True if file exists.</returns>
        [Obsolete("Use StorageFileHelper.FileExistsAsync instead.")]
        public static Task<bool> IsLocalCacheFileExistsAsync(string fileName)
        {
            StorageFolder workingFolder = ApplicationData.Current.LocalCacheFolder;
            return workingFolder.IsFileExistsAsync(fileName);
        }

        /// <summary>
        /// Test if a file exists in the application local cache folder.
        /// </summary>
        /// <param name="knownFolderId">The well known folder ID to use</param>
        /// <param name="fileName">Relative name of the file to open. Can contains subfolders.</param>
        /// <returns>True if file exists.</returns>
        [Obsolete("Use StorageFileHelper.FileExistsAsync instead.")]
        public static Task<bool> IsKnownFolderFileExistsAsync(
            KnownFolderId knownFolderId,
            string fileName)
        {
            StorageFolder workingFolder = StorageFileHelper.GetFolderFromKnownFolderId(knownFolderId);
            return workingFolder.IsFileExistsAsync(fileName);
        }

        /// <summary>
        /// Test if a file exists in the application local folder.
        /// </summary>
        /// <param name="workingFolder">Folder to use.</param>
        /// <param name="fileName">Relative name of the file to open. Can contains subfolders.</param>
        /// <returns>True if file exists.</returns>
        [Obsolete("Use StorageFileHelper.FileExistsAsync instead.")]
        public static async Task<bool> IsFileExistsAsync(
            this StorageFolder workingFolder,
            string fileName)
        {
            var name = Path.GetFileName(fileName);
            workingFolder = await GetSubFolderAsync(fileName, workingFolder);

            var item = await workingFolder.TryGetItemAsync(name);

            return item != null;
        }

        /// <summary>
        /// Read stream content as a string.
        /// </summary>
        /// <param name="stream">Stream to read from.</param>
        /// <param name="encoding">Encoding to use. Can be set to null (ASCII will be used in this case).</param>
        /// <returns>Stream content.</returns>
        public static async Task<string> ReadTextAsync(
            this IRandomAccessStream stream,
            Encoding encoding = null)
        {
            var reader = new DataReader(stream.GetInputStreamAt(0));
            await reader.LoadAsync((uint)stream.Size);

            var bytes = new byte[stream.Size];
            reader.ReadBytes(bytes);

            if (encoding == null)
            {
                // Let's revert back to Unicode
                encoding = Encoding.ASCII;
            }

            return encoding.GetString(bytes);
        }

        private static async Task<IRandomAccessStream> GetFileStreamAsync(
            string fullFileName,
            FileAccessMode accessMode,
            StorageFolder workingFolder)
        {
            var fileName = Path.GetFileName(fullFileName);
            workingFolder = await GetSubFolderAsync(fullFileName, workingFolder);

            var file = await workingFolder.GetFileAsync(fileName);

            return await file.OpenAsync(accessMode);
        }

        private static async Task<StorageFolder> GetSubFolderAsync(
            string fullFileName,
            StorageFolder workingFolder)
        {
            var folderName = Path.GetDirectoryName(fullFileName);

            if (!string.IsNullOrEmpty(folderName) && folderName != @"\")
            {
                return await workingFolder.GetFolderAsync(folderName);
            }

            return workingFolder;
        }
    }
}
