// ******************************************************************
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
using System.Threading.Tasks;

using Windows.Storage;
using Windows.Storage.Streams;

namespace Microsoft.Windows.Toolkit
{
    using global::Windows.ApplicationModel;

    /// <summary>
    /// This class provides static helper methods for <see cref="StorageFile" />.
    /// </summary>
    public static class StorageFileHelper
    {
        /// <summary>
        /// Saves a string value to a <see cref="StorageFile"/> in the given <see cref="StorageFolder"/>.
        /// </summary>
        /// <param name="fileLocation">
        /// The <see cref="StorageFolder"/> to save the file in.
        /// </param>
        /// <param name="text">
        /// The <see cref="string"/> value to save to the file.
        /// </param>
        /// <param name="fileName">
        /// The <see cref="string"/> name for the file.
        /// </param>
        /// <param name="fileExtension">
        /// The extension for the file. Default is .txt.
        /// </param>
        /// <param name="options">
        /// The creation collision options. Default is ReplaceExisting.
        /// </param>
        /// <returns>
        /// Returns the saved <see cref="StorageFile"/> containing the text.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Exception thrown if the file location or file name are null or empty.
        /// </exception>
        public static async Task<StorageFile> SaveTextToFileAsync(
            StorageFolder fileLocation,
            string text,
            string fileName,
            string fileExtension = ".txt",
            CreationCollisionOption options = CreationCollisionOption.ReplaceExisting)
        {
            if (fileLocation == null)
            {
                throw new ArgumentNullException(nameof(fileLocation));
            }

            if (string.IsNullOrWhiteSpace(fileName))
            {
                throw new ArgumentNullException(nameof(fileName));
            }

            var storageFile = await fileLocation.CreateFileAsync($"{fileName}{fileExtension}", options);
            await FileIO.WriteTextAsync(storageFile, text);

            return storageFile;
        }

        /// <summary>
        /// Saves an array of bytes to a <see cref="StorageFile"/> in the given <see cref="StorageFolder"/>.
        /// </summary>
        /// <param name="fileLocation">
        /// The <see cref="StorageFolder"/> to save the file in.
        /// </param>
        /// <param name="bytes">
        /// The <see cref="byte"/> array to save to the file.
        /// </param>
        /// <param name="fileName">
        /// The <see cref="string"/> name for the file.
        /// </param>
        /// <param name="fileExtension">
        /// The extension for the file.
        /// </param>
        /// <param name="options">
        /// The creation collision options. Default is ReplaceExisting.
        /// </param>
        /// <returns>
        /// Returns the saved <see cref="StorageFile"/> containing the bytes.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Exception thrown if the file location or file name are null or empty.
        /// </exception>
        public static async Task<StorageFile> SaveBytesToFileAsync(
            StorageFolder fileLocation,
            byte[] bytes,
            string fileName,
            string fileExtension,
            CreationCollisionOption options = CreationCollisionOption.ReplaceExisting)
        {
            if (fileLocation == null)
            {
                throw new ArgumentNullException(nameof(fileLocation));
            }

            if (string.IsNullOrWhiteSpace(fileName))
            {
                throw new ArgumentNullException(nameof(fileName));
            }

            var storageFile = await fileLocation.CreateFileAsync($"{fileName}{fileExtension}", options);
            await FileIO.WriteBytesAsync(storageFile, bytes);

            return storageFile;
        }

        /// <summary>
        /// Gets a string value from a <see cref="StorageFile"/> located in the application installation folder.
        /// </summary>
        /// <param name="relativePath">
        /// The relative <see cref="string"/> file path.
        /// </param>
        /// <returns>
        /// Returns the stored <see cref="string"/> value.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Exception thrown if the <paramref name="relativePath"/> is null or empty.
        /// </exception>
        public static async Task<string> ReadTextFromPackagedFile(string relativePath)
        {
            if (string.IsNullOrWhiteSpace(relativePath))
            {
                throw new ArgumentNullException(nameof(relativePath));
            }

            var workingFolder = Package.Current.InstalledLocation;
            var file = await workingFolder.GetFileAsync(relativePath);

            return await FileIO.ReadTextAsync(file);
        }

        /// <summary>
        /// Gets a string value from a <see cref="StorageFile"/> located in the application local cache folder.
        /// </summary>
        /// <param name="relativePath">
        /// The relative <see cref="string"/> file path.
        /// </param>
        /// <returns>
        /// Returns the stored <see cref="string"/> value.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Exception thrown if the <paramref name="relativePath"/> is null or empty.
        /// </exception>
        public static async Task<string> ReadTextFromLocalCacheFile(string relativePath)
        {
            if (string.IsNullOrWhiteSpace(relativePath))
            {
                throw new ArgumentNullException(nameof(relativePath));
            }

            var workingFolder = ApplicationData.Current.LocalCacheFolder;
            var file = await workingFolder.GetFileAsync(relativePath);

            return await FileIO.ReadTextAsync(file);
        }

        /// <summary>
        /// Gets a string value from a <see cref="StorageFile"/> located in the application local folder.
        /// </summary>
        /// <param name="relativePath">
        /// The relative <see cref="string"/> file path.
        /// </param>
        /// <returns>
        /// Returns the stored <see cref="string"/> value.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Exception thrown if the <paramref name="relativePath"/> is null or empty.
        /// </exception>
        public static async Task<string> ReadTextFromLocalFile(string relativePath)
        {
            if (string.IsNullOrWhiteSpace(relativePath))
            {
                throw new ArgumentNullException(nameof(relativePath));
            }

            var workingFolder = ApplicationData.Current.LocalFolder;
            var file = await workingFolder.GetFileAsync(relativePath);

            return await FileIO.ReadTextAsync(file);
        }

        /// <summary>
        /// Gets a string value from a <see cref="StorageFile"/> located in a well known folder.
        /// </summary>
        /// <param name="knownFolderId">
        /// The well known folder ID to use.
        /// </param>
        /// <param name="relativePath">
        /// The relative <see cref="string"/> file path.
        /// </param>
        /// <returns>
        /// Returns the stored <see cref="string"/> value.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Exception thrown if the <paramref name="relativePath"/> is null or empty.
        /// </exception>
        public static async Task<string> ReadTextFromKnownFoldersFile(KnownFolderId knownFolderId, string relativePath)
        {
            if (string.IsNullOrWhiteSpace(relativePath))
            {
                throw new ArgumentNullException(nameof(relativePath));
            }

            var workingFolder = GetFolderFromKnownFolderId(knownFolderId);

            var file = await workingFolder.GetFileAsync(relativePath);

            return await FileIO.ReadTextAsync(file);
        }

        /// <summary>
        /// Returns a <see cref="StorageFolder"/> from a <see cref="KnownFolderId"/>
        /// </summary>
        /// <param name="knownFolderId">Folder Id</param>
        /// <returns><see cref="StorageFolder"/></returns>
        internal static StorageFolder GetFolderFromKnownFolderId(KnownFolderId knownFolderId)
        {
            StorageFolder workingFolder;

            switch (knownFolderId)
            {
                case KnownFolderId.AppCaptures:
                    workingFolder = KnownFolders.AppCaptures;
                    break;
                case KnownFolderId.CameraRoll:
                    workingFolder = KnownFolders.CameraRoll;
                    break;
                case KnownFolderId.DocumentsLibrary:
                    workingFolder = KnownFolders.DocumentsLibrary;
                    break;
                case KnownFolderId.HomeGroup:
                    workingFolder = KnownFolders.HomeGroup;
                    break;
                case KnownFolderId.MediaServerDevices:
                    workingFolder = KnownFolders.MediaServerDevices;
                    break;
                case KnownFolderId.MusicLibrary:
                    workingFolder = KnownFolders.MusicLibrary;
                    break;
                case KnownFolderId.Objects3D:
                    workingFolder = KnownFolders.Objects3D;
                    break;
                case KnownFolderId.PicturesLibrary:
                    workingFolder = KnownFolders.PicturesLibrary;
                    break;
                case KnownFolderId.Playlists:
                    workingFolder = KnownFolders.Playlists;
                    break;
                case KnownFolderId.RecordedCalls:
                    workingFolder = KnownFolders.RecordedCalls;
                    break;
                case KnownFolderId.RemovableDevices:
                    workingFolder = KnownFolders.RemovableDevices;
                    break;
                case KnownFolderId.SavedPictures:
                    workingFolder = KnownFolders.SavedPictures;
                    break;
                case KnownFolderId.VideosLibrary:
                    workingFolder = KnownFolders.VideosLibrary;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(knownFolderId), knownFolderId, null);
            }

            return workingFolder;
        }

        /// <summary>
        /// Gets a byte array from a <see cref="StorageFile"/> based on a file path string.
        /// </summary>
        /// <param name="filePath">
        /// The <see cref="string"/> file path.
        /// </param>
        /// <returns>
        /// Returns the stored <see cref="byte"/> array.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Exception thrown if the <paramref name="filePath"/> is null or empty.
        /// </exception>
        public static async Task<byte[]> GetBytesFromFilePathAsync(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
            {
                throw new ArgumentNullException(nameof(filePath));
            }

            var file = await StorageFile.GetFileFromPathAsync(filePath);
            return await GetBytesFromFileAsync(file);
        }

        /// <summary>
        /// Gets a byte array from a <see cref="StorageFile"/>.
        /// </summary>
        /// <param name="file">
        /// The <see cref="StorageFile"/>.
        /// </param>
        /// <returns>
        /// Returns the stored <see cref="byte"/> array.
        /// </returns>
        public static async Task<byte[]> GetBytesFromFileAsync(StorageFile file)
        {
            if (file == null)
            {
                return null;
            }

            using (IRandomAccessStream stream = await file.OpenReadAsync())
            {
                using (var reader = new DataReader(stream.GetInputStreamAt(0)))
                {
                    await reader.LoadAsync((uint)stream.Size);
                    var bytes = new byte[stream.Size];
                    reader.ReadBytes(bytes);
                    return bytes;
                }
            }
        }
    }
}