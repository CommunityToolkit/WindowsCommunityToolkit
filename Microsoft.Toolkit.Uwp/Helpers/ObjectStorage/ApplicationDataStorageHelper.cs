// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Toolkit.Helpers;
using Windows.Storage;
using Windows.System;

#nullable enable

namespace Microsoft.Toolkit.Uwp.Helpers
{
    /// <summary>
    /// Storage helper for files and folders living in Windows.Storage.ApplicationData storage endpoints.
    /// </summary>
    public partial class ApplicationDataStorageHelper : IFileStorageHelper, ISettingsStorageHelper<string>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationDataStorageHelper"/> class.
        /// </summary>
        /// <param name="appData">The data store to interact with.</param>
        /// <param name="objectSerializer">Serializer for converting stored values. Defaults to <see cref="Toolkit.Helpers.SystemSerializer"/>.</param>
        public ApplicationDataStorageHelper(ApplicationData appData, Toolkit.Helpers.IObjectSerializer? objectSerializer = null)
        {
            AppData = appData ?? throw new ArgumentNullException(nameof(appData));
            Serializer = objectSerializer ?? new Toolkit.Helpers.SystemSerializer();
        }

        /// <summary>
        /// Gets the settings container.
        /// </summary>
        public ApplicationDataContainer Settings => AppData.LocalSettings;

        /// <summary>
        ///  Gets the storage folder.
        /// </summary>
        public StorageFolder Folder => AppData.LocalFolder;

        /// <summary>
        /// Gets the storage host.
        /// </summary>
        protected ApplicationData AppData { get; }

        /// <summary>
        /// Gets the serializer for converting stored values.
        /// </summary>
        protected Toolkit.Helpers.IObjectSerializer Serializer { get; }

        /// <summary>
        /// Get a new instance using ApplicationData.Current and the provided serializer.
        /// </summary>
        /// <param name="objectSerializer">Serializer for converting stored values. Defaults to <see cref="Toolkit.Helpers.SystemSerializer"/>.</param>
        /// <returns>A new instance of ApplicationDataStorageHelper.</returns>
        public static ApplicationDataStorageHelper GetCurrent(Toolkit.Helpers.IObjectSerializer? objectSerializer = null)
        {
            var appData = ApplicationData.Current;
            return new ApplicationDataStorageHelper(appData, objectSerializer);
        }

        /// <summary>
        /// Get a new instance using the ApplicationData for the provided user and serializer.
        /// </summary>
        /// <param name="user">App data user owner.</param>
        /// <param name="objectSerializer">Serializer for converting stored values. Defaults to <see cref="Toolkit.Helpers.SystemSerializer"/>.</param>
        /// <returns>A new instance of ApplicationDataStorageHelper.</returns>
        public static async Task<ApplicationDataStorageHelper> GetForUserAsync(User user, Toolkit.Helpers.IObjectSerializer? objectSerializer = null)
        {
            var appData = await ApplicationData.GetForUserAsync(user);
            return new ApplicationDataStorageHelper(appData, objectSerializer);
        }

        /// <summary>
        /// Determines whether a setting already exists.
        /// </summary>
        /// <param name="key">Key of the setting (that contains object).</param>
        /// <returns>True if a value exists.</returns>
        public bool KeyExists(string key)
        {
            return Settings.Values.ContainsKey(key);
        }

        /// <summary>
        /// Retrieves a single item by its key.
        /// </summary>
        /// <typeparam name="T">Type of object retrieved.</typeparam>
        /// <param name="key">Key of the object.</param>
        /// <param name="default">Default value of the object.</param>
        /// <returns>The TValue object.</returns>
        public T? Read<T>(string key, T? @default = default)
        {
            if (Settings.Values.TryGetValue(key, out var valueObj) && valueObj is string valueString)
            {
                return Serializer.Deserialize<T>(valueString);
            }

            return @default;
        }

        /// <inheritdoc />
        public bool TryRead<T>(string key, out T? value)
        {
            if (Settings.Values.TryGetValue(key, out var valueObj) && valueObj is string valueString)
            {
                value = Serializer.Deserialize<T>(valueString);
                return true;
            }

            value = default;
            return false;
        }

        /// <inheritdoc />
        public void Save<T>(string key, T value)
        {
            Settings.Values[key] = Serializer.Serialize(value);
        }

        /// <inheritdoc />
        public bool TryDelete(string key)
        {
            return Settings.Values.Remove(key);
        }

        /// <inheritdoc />
        public void Clear()
        {
            Settings.Values.Clear();
        }

        /// <summary>
        /// Determines whether a setting already exists in composite.
        /// </summary>
        /// <param name="compositeKey">Key of the composite (that contains settings).</param>
        /// <param name="key">Key of the setting (that contains object).</param>
        /// <returns>True if a value exists.</returns>
        public bool KeyExists(string compositeKey, string key)
        {
            if (TryRead(compositeKey, out ApplicationDataCompositeValue? composite) && composite != null)
            {
                return composite.ContainsKey(key);
            }

            return false;
        }

        /// <summary>
        /// Attempts to retrieve a single item by its key in composite.
        /// </summary>
        /// <typeparam name="T">Type of object retrieved.</typeparam>
        /// <param name="compositeKey">Key of the composite (that contains settings).</param>
        /// <param name="key">Key of the object.</param>
        /// <param name="value">The value of the object retrieved.</param>
        /// <returns>The T object.</returns>
        public bool TryRead<T>(string compositeKey, string key, out T? value)
        {
            if (TryRead(compositeKey, out ApplicationDataCompositeValue? composite) && composite != null)
            {
                string compositeValue = (string)composite[key];
                if (compositeValue != null)
                {
                    value = Serializer.Deserialize<T>(compositeValue);
                    return true;
                }
            }

            value = default;
            return false;
        }

        /// <summary>
        /// Retrieves a single item by its key in composite.
        /// </summary>
        /// <typeparam name="T">Type of object retrieved.</typeparam>
        /// <param name="compositeKey">Key of the composite (that contains settings).</param>
        /// <param name="key">Key of the object.</param>
        /// <param name="default">Default value of the object.</param>
        /// <returns>The T object.</returns>
        public T? Read<T>(string compositeKey, string key, T? @default = default)
        {
            if (TryRead(compositeKey, out ApplicationDataCompositeValue? composite) && composite != null)
            {
                if (composite.TryGetValue(key, out object valueObj) && valueObj is string value)
                {
                    return Serializer.Deserialize<T>(value);
                }
            }

            return @default;
        }

        /// <summary>
        /// Saves a group of items by its key in a composite.
        /// This method should be considered for objects that do not exceed 8k bytes during the lifetime of the application
        /// and for groups of settings which need to be treated in an atomic way.
        /// </summary>
        /// <typeparam name="T">Type of object saved.</typeparam>
        /// <param name="compositeKey">Key of the composite (that contains settings).</param>
        /// <param name="values">Objects to save.</param>
        public void Save<T>(string compositeKey, IDictionary<string, T> values)
        {
            if (TryRead(compositeKey, out ApplicationDataCompositeValue? composite) && composite != null)
            {
                foreach (KeyValuePair<string, T> setting in values)
                {
                    if (composite.ContainsKey(setting.Key))
                    {
                        composite[setting.Key] = Serializer.Serialize(setting.Value);
                    }
                    else
                    {
                        composite.Add(setting.Key, Serializer.Serialize(setting.Value));
                    }
                }
            }
            else
            {
                composite = new ApplicationDataCompositeValue();
                foreach (KeyValuePair<string, T> setting in values)
                {
                    composite.Add(setting.Key, Serializer.Serialize(setting.Value));
                }

                Settings.Values[compositeKey] = composite;
            }
        }

        /// <summary>
        /// Deletes a single item by its key in composite.
        /// </summary>
        /// <param name="compositeKey">Key of the composite (that contains settings).</param>
        /// <param name="key">Key of the object.</param>
        /// <returns>A boolean indicator of success.</returns>
        public bool TryDelete(string compositeKey, string key)
        {
            if (TryRead(compositeKey, out ApplicationDataCompositeValue? composite) && composite != null)
            {
                return composite.Remove(key);
            }

            return false;
        }

        /// <inheritdoc />
        public Task<T?> ReadFileAsync<T>(string filePath, T? @default = default)
        {
            return ReadFileAsync<T>(Folder, filePath, @default);
        }

        /// <inheritdoc />
        public Task<IEnumerable<(DirectoryItemType ItemType, string Name)>> ReadFolderAsync(string folderPath)
        {
            return ReadFolderAsync(Folder, folderPath);
        }

        /// <inheritdoc />
        public Task CreateFileAsync<T>(string filePath, T value)
        {
            return CreateFileAsync<T>(Folder, filePath, value);
        }

        /// <inheritdoc />
        public Task CreateFolderAsync(string folderPath)
        {
            return CreateFolderAsync(Folder, folderPath);
        }

        /// <inheritdoc />
        public Task<bool> TryDeleteItemAsync(string itemPath)
        {
            return TryDeleteItemAsync(Folder, itemPath);
        }

        /// <inheritdoc />
        public Task<bool> TryRenameItemAsync(string itemPath, string newName)
        {
            return TryRenameItemAsync(Folder, itemPath, newName);
        }

        private async Task<T?> ReadFileAsync<T>(StorageFolder folder, string filePath, T? @default = default)
        {
            string value = await StorageFileHelper.ReadTextFromFileAsync(folder, NormalizePath(filePath));
            return (value != null) ? Serializer.Deserialize<T>(value) : @default;
        }

        private async Task<IEnumerable<(DirectoryItemType, string)>> ReadFolderAsync(StorageFolder folder, string folderPath)
        {
            var targetFolder = await folder.GetFolderAsync(NormalizePath(folderPath));
            var items = await targetFolder.GetItemsAsync();

            return items.Select((item) =>
            {
                var itemType = item.IsOfType(StorageItemTypes.File) ? DirectoryItemType.File
                    : item.IsOfType(StorageItemTypes.Folder) ? DirectoryItemType.Folder
                    : DirectoryItemType.None;

                return (itemType, item.Name);
            });
        }

        private async Task<StorageFile> CreateFileAsync<T>(StorageFolder folder, string filePath, T value)
        {
            return await StorageFileHelper.WriteTextToFileAsync(folder, Serializer.Serialize(value)?.ToString(), NormalizePath(filePath), CreationCollisionOption.ReplaceExisting);
        }

        private async Task CreateFolderAsync(StorageFolder folder, string folderPath)
        {
            await folder.CreateFolderAsync(NormalizePath(folderPath), CreationCollisionOption.OpenIfExists);
        }

        private async Task<bool> TryDeleteItemAsync(StorageFolder folder, string itemPath)
        {
            try
            {
                var item = await folder.GetItemAsync(NormalizePath(itemPath));
                await item.DeleteAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        private async Task<bool> TryRenameItemAsync(StorageFolder folder, string itemPath, string newName)
        {
            try
            {
                var item = await folder.GetItemAsync(NormalizePath(itemPath));
                await item.RenameAsync(newName, NameCollisionOption.FailIfExists);
                return true;
            }
            catch
            {
                return false;
            }
        }

        private string NormalizePath(string path)
        {
            return Path.Combine(Path.GetDirectoryName(path), Path.GetFileName(path));
        }
    }
}
