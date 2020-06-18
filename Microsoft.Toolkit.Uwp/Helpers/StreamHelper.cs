// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.Storage;
using Windows.Storage.Streams;

namespace Microsoft.Toolkit.Uwp.Helpers
{
    /// <summary>
    /// This class provides static helper methods for streams.
    /// </summary>
    public static class StreamHelper
    {
        private static HttpClient client = new HttpClient();

        /// <summary>
        /// AsStream helper for uno compatibility
        /// </summary>
        /// <param name="s">The source stream</param>
        /// <returns>The stream</returns>
        public static Stream AsStream(this Stream s) => s;

        /// <summary>
        /// Gets the response stream returned by a HTTP get request.
        /// </summary>
        /// <param name="uri">Uri to request.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> used to cancel the operation.</param>
        /// <returns>The response stream</returns>
        public static async Task<IRandomAccessStream> GetHttpStreamAsync(this Uri uri, CancellationToken cancellationToken = default(CancellationToken))
        {
            var outputStream = new InMemoryRandomAccessStream();

            using (var request = new HttpRequestMessage(HttpMethod.Get, uri))
            {
                using (var response = await client.SendAsync(request, cancellationToken).ConfigureAwait(false))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        await response.Content.CopyToAsync(outputStream.AsStreamForWrite()).ConfigureAwait(false);
                        outputStream.Seek(0);
                    }
                }
            }

            return outputStream;
        }

        /// <summary>
        /// Gets the response stream returned by a HTTP get request and save it to a local file.
        /// </summary>
        /// <param name="uri">Uri to request.</param>
        /// <param name="targetFile">StorageFile to save the stream to.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public static async Task GetHttpStreamToStorageFileAsync(
            this Uri uri,
            StorageFile targetFile)
        {
            using (var fileStream = await targetFile.OpenAsync(FileAccessMode.ReadWrite).AsTask().ConfigureAwait(false))
            {
                using (var request = new HttpRequestMessage(HttpMethod.Get, uri))
                {
                    using (var response = await client.SendAsync(request).ConfigureAwait(false))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            await response.Content.CopyToAsync(fileStream.AsStreamForWrite()).ConfigureAwait(false);
                        }
                    }
                }
            }
        }

#if NETFX_CORE
        /// <summary>
        /// Gets a stream to a specified file from the installation folder.
        /// </summary>
        /// <param name="fileName">Relative name of the file to open. Can contains subfolders.</param>
        /// <param name="accessMode">File access mode. Default is read.</param>
        /// <returns>The file stream</returns>
        public static Task<IRandomAccessStream> GetPackagedFileStreamAsync(
            string fileName,
            FileAccessMode accessMode = FileAccessMode.Read)
        {
            StorageFolder workingFolder = Package.Current.InstalledLocation;
            return GetFileRandomAccessStreamAsync(fileName, accessMode, workingFolder);
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
            return GetFileRandomAccessStreamAsync(fileName, accessMode, workingFolder);
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
            return GetFileRandomAccessStreamAsync(fileName, accessMode, workingFolder);
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
            return GetFileRandomAccessStreamAsync(fileName, accessMode, workingFolder);
        }
#else
        /// <summary>
        /// Return a stream to a specified file from the installation folder.
        /// </summary>
        /// <param name="fileName">Relative name of the file to open. Can contains subfolders.</param>
        /// <param name="accessMode">File access mode. Default is read.</param>
        /// <returns>File stream</returns>
        public static Task<Stream> GetPackagedFileStreamAsync(
            string fileName,
            FileAccessMode accessMode = FileAccessMode.Read)
        {
            StorageFolder workingFolder = Package.Current.InstalledLocation;
            return GetFileStreamAsync(fileName, accessMode, workingFolder);
        }

        /// <summary>
        /// Gets a stream to a specified file from the application local folder.
        /// </summary>
        /// <param name="fileName">Relative name of the file to open. Can contains subfolders.</param>
        /// <param name="accessMode">File access mode. Default is read.</param>
        /// <returns>The file stream</returns>
        public static Task<Stream> GetLocalFileStreamAsync(
            string fileName,
            FileAccessMode accessMode = FileAccessMode.Read)
        {
            StorageFolder workingFolder = ApplicationData.Current.LocalFolder;
            return GetFileStreamAsync(fileName, accessMode, workingFolder);
        }

        /// <summary>
        /// Gets a stream to a specified file from the application local cache folder.
        /// </summary>
        /// <param name="fileName">Relative name of the file to open. Can contain subfolders.</param>
        /// <param name="accessMode">File access mode. Default is read.</param>
        /// <returns>The file stream</returns>
        public static Task<Stream> GetLocalCacheFileStreamAsync(
            string fileName,
            FileAccessMode accessMode = FileAccessMode.Read)
        {
            StorageFolder workingFolder = ApplicationData.Current.LocalCacheFolder;
            return GetFileStreamAsync(fileName, accessMode, workingFolder);
        }

        /// <summary>
        /// Gets a stream to a specified file from the application local cache folder.
        /// </summary>
        /// <param name="knownFolderId">The well known folder ID to use</param>
        /// <param name="fileName">Relative name of the file to open. Can contains subfolders.</param>
        /// <param name="accessMode">File access mode. Default is read.</param>
        /// <returns>The file stream</returns>
        public static Task<Stream> GetKnowFoldersFileStreamAsync(
            KnownFolderId knownFolderId,
            string fileName,
            FileAccessMode accessMode = FileAccessMode.Read)
        {
            StorageFolder workingFolder = StorageFileHelper.GetFolderFromKnownFolderId(knownFolderId);
            return GetFileStreamAsync(fileName, accessMode, workingFolder);
        }

#endif

        /// <summary>
        /// Return a stream to a specified file from the installation folder.
        /// </summary>
        /// <param name="assemblyType">The owner type for the embedded file</param>
        /// <param name="fileName">Relative name of the file to open. Can contains subfolders.</param>
        /// <returns>File stream</returns>
        public static async Task<Stream> GetEmbeddedFileStreamAsync(Type assemblyType, string fileName)
        {
            await Task.Yield();

            var manifestName = assemblyType.GetTypeInfo().Assembly
                .GetManifestResourceNames()
                .FirstOrDefault(n => n.EndsWith(fileName.Replace(" ", "_"), StringComparison.OrdinalIgnoreCase) || n.EndsWith(fileName.Replace("/", "."), StringComparison.OrdinalIgnoreCase));

            if (manifestName == null)
            {
                throw new InvalidOperationException($"Failed to find resource [{fileName}]");
            }

            return assemblyType.GetTypeInfo().Assembly.GetManifestResourceStream(manifestName);
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

        private static async Task<IRandomAccessStream> GetFileRandomAccessStreamAsync(
            string fullFileName,
            FileAccessMode accessMode,
            StorageFolder workingFolder)
        {
            var fileName = Path.GetFileName(fullFileName);
            workingFolder = await GetSubFolderAsync(fullFileName, workingFolder);

            var file = await workingFolder.GetFileAsync(fileName);

            return await file.OpenAsync(accessMode);
        }

        /// <summary>
        /// Read stream content as a string.
        /// </summary>
        /// <param name="stream">Stream to read from.</param>
        /// <param name="encoding">Encoding to use. Can be set to null (ASCII will be used in this case).</param>
        /// <returns>Stream content.</returns>
        public static async Task<string> ReadTextAsync(
            this Stream stream,
            Encoding encoding = null)
        {
            using (var reader = new StreamReader(stream))
            {
                return await reader.ReadToEndAsync();
            }
        }

        private static async Task<Stream> GetFileStreamAsync(
            string fullFileName,
            FileAccessMode accessMode,
            StorageFolder workingFolder)
        {
            var fileName = Path.GetFileName(fullFileName);
            workingFolder = await GetSubFolderAsync(fullFileName, workingFolder);

            var file = await workingFolder.GetFileAsync(fileName);

            return File.OpenRead(file.Path);
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
