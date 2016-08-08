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
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Streams;

namespace Microsoft.Toolkit.Uwp
{
    using global::Windows.ApplicationModel;

    /// <summary>
    /// This class provides static helper methods for <see cref="StorageFile" />.
    /// </summary>
    public static class StorageFileHelper
    {
        /// <summary>
        /// Saves a string value to a <see cref="StorageFile"/> in application local folder/>.
        /// </summary>
        /// <param name="text">
        /// The <see cref="string"/> value to save to the file.
        /// </param>
        /// <param name="fileName">
        /// The <see cref="string"/> name for the file.
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
        public static async Task<StorageFile> WriteTextToLocalFileAsync(
            string text,
            string fileName,
            CreationCollisionOption options = CreationCollisionOption.ReplaceExisting)
        {
            if (string.IsNullOrWhiteSpace(fileName))
            {
                throw new ArgumentNullException(nameof(fileName));
            }

            var folder = ApplicationData.Current.LocalFolder;
            return await WriteTextToFileAsync(folder, text, fileName, options);
        }

        /// <summary>
        /// Saves a string value to a <see cref="StorageFile"/> in application local cache folder/>.
        /// </summary>
        /// <param name="text">
        /// The <see cref="string"/> value to save to the file.
        /// </param>
        /// <param name="fileName">
        /// The <see cref="string"/> name for the file.
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
        public static async Task<StorageFile> WriteTextToLocalCacheFileAsync(
            string text,
            string fileName,
            CreationCollisionOption options = CreationCollisionOption.ReplaceExisting)
        {
            if (string.IsNullOrWhiteSpace(fileName))
            {
                throw new ArgumentNullException(nameof(fileName));
            }

            var folder = ApplicationData.Current.LocalCacheFolder;
            return await WriteTextToFileAsync(ApplicationData.Current.LocalCacheFolder, text, fileName, options);
        }

        /// <summary>
        /// Saves a string value to a <see cref="StorageFile"/> in well known folder/>.
        /// </summary>
        /// <param name="knownFolderId">
        /// The well known folder ID to use.
        /// </param>
        /// <param name="text">
        /// The <see cref="string"/> value to save to the file.
        /// </param>
        /// <param name="fileName">
        /// The <see cref="string"/> name for the file.
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
        public static async Task<StorageFile> WriteTextToKnownFolderFileAsync(
            KnownFolderId knownFolderId,
            string text,
            string fileName,
            CreationCollisionOption options = CreationCollisionOption.ReplaceExisting)
        {
            if (string.IsNullOrWhiteSpace(fileName))
            {
                throw new ArgumentNullException(nameof(fileName));
            }

            var folder = GetFolderFromKnownFolderId(knownFolderId);
            return await WriteTextToFileAsync(folder, text, fileName, options);
        }

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
        /// <param name="options">
        /// The creation collision options. Default is ReplaceExisting.
        /// </param>
        /// <returns>
        /// Returns the saved <see cref="StorageFile"/> containing the text.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Exception thrown if the file location or file name are null or empty.
        /// </exception>
        public static async Task<StorageFile> WriteTextToFileAsync(
            this StorageFolder fileLocation,
            string text,
            string fileName,
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

            var storageFile = await fileLocation.CreateFileAsync(fileName, options);
            await FileIO.WriteTextAsync(storageFile, text);

            return storageFile;
        }

        /// <summary>
        /// Saves an array of bytes to a <see cref="StorageFile"/> to application local folder/>.
        /// </summary>
        /// <param name="bytes">
        /// The <see cref="byte"/> array to save to the file.
        /// </param>
        /// <param name="fileName">
        /// The <see cref="string"/> name for the file.
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
        public static async Task<StorageFile> WriteBytesToLocalFileAsync(
            byte[] bytes,
            string fileName,
            CreationCollisionOption options = CreationCollisionOption.ReplaceExisting)
        {
            if (string.IsNullOrWhiteSpace(fileName))
            {
                throw new ArgumentNullException(nameof(fileName));
            }

            var folder = ApplicationData.Current.LocalFolder;
            return await WriteBytesToFileAsync(folder, bytes, fileName, options);
        }

        /// <summary>
        /// Saves an array of bytes to a <see cref="StorageFile"/> to application local cache folder/>.
        /// </summary>
        /// <param name="bytes">
        /// The <see cref="byte"/> array to save to the file.
        /// </param>
        /// <param name="fileName">
        /// The <see cref="string"/> name for the file.
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
        public static async Task<StorageFile> WriteBytesToLocalCacheFileAsync(
            byte[] bytes,
            string fileName,
            CreationCollisionOption options = CreationCollisionOption.ReplaceExisting)
        {
            if (string.IsNullOrWhiteSpace(fileName))
            {
                throw new ArgumentNullException(nameof(fileName));
            }

            var folder = ApplicationData.Current.LocalCacheFolder;
            return await WriteBytesToFileAsync(folder, bytes, fileName, options);
        }

        /// <summary>
        /// Saves an array of bytes to a <see cref="StorageFile"/> to well known folder/>.
        /// </summary>
        /// <param name="knownFolderId">
        /// The well known folder ID to use.
        /// </param>
        /// <param name="bytes">
        /// The <see cref="byte"/> array to save to the file.
        /// </param>
        /// <param name="fileName">
        /// The <see cref="string"/> name for the file.
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
        public static async Task<StorageFile> WriteBytesToKnownFolderFileAsync(
            KnownFolderId knownFolderId,
            byte[] bytes,
            string fileName,
            CreationCollisionOption options = CreationCollisionOption.ReplaceExisting)
        {
            if (string.IsNullOrWhiteSpace(fileName))
            {
                throw new ArgumentNullException(nameof(fileName));
            }

            var folder = GetFolderFromKnownFolderId(knownFolderId);
            return await WriteBytesToFileAsync(folder, bytes, fileName, options);
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
        /// <param name="options">
        /// The creation collision options. Default is ReplaceExisting.
        /// </param>
        /// <returns>
        /// Returns the saved <see cref="StorageFile"/> containing the bytes.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Exception thrown if the file location or file name are null or empty.
        /// </exception>
        public static async Task<StorageFile> WriteBytesToFileAsync(
            this StorageFolder fileLocation,
            byte[] bytes,
            string fileName,
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

            var storageFile = await fileLocation.CreateFileAsync(fileName, options);
            await FileIO.WriteBytesAsync(storageFile, bytes);

            return storageFile;
        }

        /// <summary>
        /// Gets a string value from a <see cref="StorageFile"/> located in the application installation folder.
        /// </summary>
        /// <param name="fileName">
        /// The relative <see cref="string"/> file path.
        /// </param>
        /// <returns>
        /// Returns the stored <see cref="string"/> value.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Exception thrown if the <paramref name="fileName"/> is null or empty.
        /// </exception>
        public static async Task<string> ReadTextFromPackagedFileAsync(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName))
            {
                throw new ArgumentNullException(nameof(fileName));
            }

            var folder = Package.Current.InstalledLocation;
            return await ReadTextFromFileAsync(folder, fileName);
        }

        /// <summary>
        /// Gets a string value from a <see cref="StorageFile"/> located in the application local cache folder.
        /// </summary>
        /// <param name="fileName">
        /// The relative <see cref="string"/> file path.
        /// </param>
        /// <returns>
        /// Returns the stored <see cref="string"/> value.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Exception thrown if the <paramref name="fileName"/> is null or empty.
        /// </exception>
        public static async Task<string> ReadTextFromLocalCacheFileAsync(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName))
            {
                throw new ArgumentNullException(nameof(fileName));
            }

            var folder = ApplicationData.Current.LocalCacheFolder;
            return await ReadTextFromFileAsync(folder, fileName);
        }

        /// <summary>
        /// Gets a string value from a <see cref="StorageFile"/> located in the application local folder.
        /// </summary>
        /// <param name="fileName">
        /// The relative <see cref="string"/> file path.
        /// </param>
        /// <returns>
        /// Returns the stored <see cref="string"/> value.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Exception thrown if the <paramref name="fileName"/> is null or empty.
        /// </exception>
        public static async Task<string> ReadTextFromLocalFileAsync(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName))
            {
                throw new ArgumentNullException(nameof(fileName));
            }

            var folder = ApplicationData.Current.LocalFolder;
            return await ReadTextFromFileAsync(folder, fileName);
        }

        /// <summary>
        /// Gets a string value from a <see cref="StorageFile"/> located in a well known folder.
        /// </summary>
        /// <param name="knownFolderId">
        /// The well known folder ID to use.
        /// </param>
        /// <param name="fileName">
        /// The relative <see cref="string"/> file path.
        /// </param>
        /// <returns>
        /// Returns the stored <see cref="string"/> value.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Exception thrown if the <paramref name="fileName"/> is null or empty.
        /// </exception>
        public static async Task<string> ReadTextFromKnownFoldersFileAsync(
            KnownFolderId knownFolderId,
            string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName))
            {
                throw new ArgumentNullException(nameof(fileName));
            }

            var folder = GetFolderFromKnownFolderId(knownFolderId);
            return await ReadTextFromFileAsync(folder, fileName);
        }

        /// <summary>
        /// Gets a string value from a <see cref="StorageFile"/> located in the given <see cref="StorageFolder"/>.
        /// </summary>
        /// <param name="fileLocation">
        /// The <see cref="StorageFolder"/> to save the file in.
        /// </param>
        /// <param name="fileName">
        /// The relative <see cref="string"/> file path.
        /// </param>
        /// <returns>
        /// Returns the stored <see cref="string"/> value.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Exception thrown if the <paramref name="fileName"/> is null or empty.
        /// </exception>
        public static async Task<string> ReadTextFromFileAsync(
            this StorageFolder fileLocation,
            string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName))
            {
                throw new ArgumentNullException(nameof(fileName));
            }

            var file = await fileLocation.GetFileAsync(fileName);
            return await FileIO.ReadTextAsync(file);
        }

        /// <summary>
        /// Gets an array of bytes from a <see cref="StorageFile"/> located in the application installation folder.
        /// </summary>
        /// <param name="fileName">
        /// The relative <see cref="string"/> file path.
        /// </param>
        /// <returns>
        /// Returns the stored <see cref="byte"/> array.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Exception thrown if the <paramref name="fileName"/> is null or empty.
        /// </exception>
        public static async Task<byte[]> ReadBytesFromPackagedFileAsync(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName))
            {
                throw new ArgumentNullException(nameof(fileName));
            }

            var folder = Package.Current.InstalledLocation;
            return await ReadBytesFromFileAsync(folder, fileName);
        }

        /// <summary>
        /// Gets an array of bytes from a <see cref="StorageFile"/> located in the application local cache folder.
        /// </summary>
        /// <param name="fileName">
        /// The relative <see cref="string"/> file path.
        /// </param>
        /// <returns>
        /// Returns the stored <see cref="byte"/> array.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Exception thrown if the <paramref name="fileName"/> is null or empty.
        /// </exception>
        public static async Task<byte[]> ReadBytesFromLocalCacheFileAsync(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName))
            {
                throw new ArgumentNullException(nameof(fileName));
            }

            var folder = ApplicationData.Current.LocalCacheFolder;
            return await ReadBytesFromFileAsync(folder, fileName);
        }

        /// <summary>
        /// Gets an array of bytes from a <see cref="StorageFile"/> located in the application local folder.
        /// </summary>
        /// <param name="fileName">
        /// The relative <see cref="string"/> file path.
        /// </param>
        /// <returns>
        /// Returns the stored <see cref="byte"/> array.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Exception thrown if the <paramref name="fileName"/> is null or empty.
        /// </exception>
        public static async Task<byte[]> ReadBytesFromLocalFileAsync(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName))
            {
                throw new ArgumentNullException(nameof(fileName));
            }

            var folder = ApplicationData.Current.LocalFolder;
            return await ReadBytesFromFileAsync(folder, fileName);
        }

        /// <summary>
        /// Gets an array of bytes from a <see cref="StorageFile"/> located in a well known folder.
        /// </summary>
        /// <param name="knownFolderId">
        /// The well known folder ID to use.
        /// </param>
        /// <param name="fileName">
        /// The relative <see cref="string"/> file path.
        /// </param>
        /// <returns>
        /// Returns the stored <see cref="byte"/> array.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Exception thrown if the <paramref name="fileName"/> is null or empty.
        /// </exception>
        public static async Task<byte[]> ReadBytesFromKnownFoldersFileAsync(
            KnownFolderId knownFolderId,
            string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName))
            {
                throw new ArgumentNullException(nameof(fileName));
            }

            var folder = GetFolderFromKnownFolderId(knownFolderId);
            return await ReadBytesFromFileAsync(folder, fileName);
        }

        /// <summary>
        /// Gets an array of bytes from a <see cref="StorageFile"/> located in the given <see cref="StorageFolder"/>.
        /// </summary>
        /// <param name="fileLocation">
        /// The <see cref="StorageFolder"/> to save the file in.
        /// </param>
        /// <param name="fileName">
        /// The relative <see cref="string"/> file path.
        /// </param>
        /// <returns>
        /// Returns the stored <see cref="byte"/> array.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Exception thrown if the <paramref name="fileName"/> is null or empty.
        /// </exception>
        public static async Task<byte[]> ReadBytesFromFileAsync(
            this StorageFolder fileLocation,
            string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName))
            {
                throw new ArgumentNullException(nameof(fileName));
            }

            var file = await fileLocation.GetFileAsync(fileName);
            return await StorageFileHelper.ReadBytesAsync(file);
        }

        /// <summary>
        /// Gets an array of bytes from a <see cref="StorageFile"/>.
        /// </summary>
        /// <param name="file">
        /// The <see cref="StorageFile"/>.
        /// </param>
        /// <returns>
        /// Returns the stored <see cref="byte"/> array.
        /// </returns>
        public static async Task<byte[]> ReadBytesAsync(this StorageFile file)
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
    }
}