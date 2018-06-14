// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Windows.Storage;

namespace Microsoft.Toolkit.Uwp.Helpers
{
    /// <summary>
    /// Shared implementation of ObjectStorageHelper
    /// </summary>
    public abstract class BaseObjectStorageHelper : IObjectStorageHelper
    {
        private JsonSerializer serializer = new JsonSerializer();

        /// <summary>
        /// Gets or sets settings container
        /// </summary>
        protected ApplicationDataContainer Settings { get; set; }

        /// <summary>
        /// Gets or sets storage folder
        /// </summary>
        protected StorageFolder Folder { get; set; }

        /// <summary>
        /// Detect if a setting already exists
        /// </summary>
        /// <param name="key">Key of the setting (that contains object)</param>
        /// <returns>True if a value exists</returns>
        public bool KeyExists(string key)
        {
            return Settings.Values.ContainsKey(key);
        }

        /// <summary>
        /// Detect if a setting already exists in composite.
        /// </summary>
        /// <param name="compositeKey">Key of the composite (that contains settings)</param>
        /// <param name="key">Key of the setting (that contains object)</param>
        /// <returns>True if a value exists</returns>
        public bool KeyExists(string compositeKey, string key)
        {
            if (KeyExists(compositeKey))
            {
                ApplicationDataCompositeValue composite = (ApplicationDataCompositeValue)Settings.Values[compositeKey];
                if (composite != null)
                {
                    return composite.ContainsKey(key);
                }
            }

            return false;
        }

        /// <summary>
        /// Retrieve single item by its key.
        /// </summary>
        /// <typeparam name="T">Type of object retrieved</typeparam>
        /// <param name="key">Key of the object</param>
        /// <param name="default">Default value of the object</param>
        /// <returns>The T object</returns>
        public T Read<T>(string key, T @default = default(T))
        {
            object value = null;

            if (!Settings.Values.TryGetValue(key, out value))
            {
                return @default;
            }

            if (value == null)
            {
                return @default;
            }

            var type = typeof(T);
            var typeInfo = type.GetTypeInfo();

            if (typeInfo.IsPrimitive || type == typeof(string))
            {
                return (T)Convert.ChangeType(value, type);
            }

            return JsonConvert.DeserializeObject<T>((string)value);
        }

        /// <summary>
        /// Retrieve single item by its key in composite.
        /// </summary>
        /// <typeparam name="T">Type of object retrieved</typeparam>
        /// <param name="compositeKey">Key of the composite (that contains settings)</param>
        /// <param name="key">Key of the object</param>
        /// <param name="default">Default value of the object</param>
        /// <returns>The T object</returns>
        public T Read<T>(string compositeKey, string key, T @default = default(T))
        {
            ApplicationDataCompositeValue composite = (ApplicationDataCompositeValue)Settings.Values[compositeKey];
            if (composite != null)
            {
                string value = (string)composite[key];
                if (value != null)
                {
                    return JsonConvert.DeserializeObject<T>(value);
                }
            }

            return @default;
        }

        /// <summary>
        /// Save single item by its key.
        /// This method should be considered for objects that do not exceed 8k bytes during the lifetime of the application
        /// (refers to <see cref="SaveFileAsync{T}(string, T)"/> for complex/large objects).
        /// </summary>
        /// <typeparam name="T">Type of object saved</typeparam>
        /// <param name="key">Key of the value saved</param>
        /// <param name="value">Object to save</param>
        public void Save<T>(string key, T value)
        {
            var type = typeof(T);
            var typeInfo = type.GetTypeInfo();

            if (typeInfo.IsPrimitive || type == typeof(string))
            {
                Settings.Values[key] = value;
            }
            else
            {
                Settings.Values[key] = JsonConvert.SerializeObject(value);
            }
        }

        /// <summary>
        /// Save a group of items by its key in a composite.
        /// This method should be considered for objects that do not exceed 8k bytes during the lifetime of the application
        /// (refers to <see cref="SaveFileAsync{T}(string, T)"/> for complex/large objects) and for groups of settings which
        /// need to be treated in an atomic way.
        /// </summary>
        /// <typeparam name="T">Type of object saved</typeparam>
        /// <param name="compositeKey">Key of the composite (that contains settings)</param>
        /// <param name="values">Objects to save</param>
        public void Save<T>(string compositeKey, IDictionary<string, T> values)
        {
            if (KeyExists(compositeKey))
            {
                ApplicationDataCompositeValue composite = (ApplicationDataCompositeValue)Settings.Values[compositeKey];

                foreach (KeyValuePair<string, T> setting in values)
                {
                    if (composite.ContainsKey(setting.Key))
                    {
                        composite[setting.Key] = JsonConvert.SerializeObject(setting.Value);
                    }
                    else
                    {
                        composite.Add(setting.Key, JsonConvert.SerializeObject(setting.Value));
                    }
                }
            }
            else
            {
                ApplicationDataCompositeValue composite = new ApplicationDataCompositeValue();
                foreach (KeyValuePair<string, T> setting in values)
                {
                    composite.Add(setting.Key, JsonConvert.SerializeObject(setting.Value));
                }

                Settings.Values[compositeKey] = composite;
            }
        }

        /// <summary>
        /// Detect if a file already exists
        /// </summary>
        /// <param name="filePath">Key of the file (that contains object)</param>
        /// <returns>True if a value exists</returns>
        public Task<bool> FileExistsAsync(string filePath)
        {
            return Folder.FileExistsAsync(filePath);
        }

        /// <summary>
        /// Retrieve object from file.
        /// </summary>
        /// <typeparam name="T">Type of object retrieved</typeparam>
        /// <param name="filePath">Path to the file that contains the object</param>
        /// <param name="default">Default value of the object</param>
        /// <returns>Waiting task until completion with the object in the file</returns>
        public async Task<T> ReadFileAsync<T>(string filePath, T @default = default(T))
        {
            string value = await StorageFileHelper.ReadTextFromFileAsync(Folder, filePath);
            return (value != null) ? JsonConvert.DeserializeObject<T>(value) : @default;
        }

        /// <summary>
        /// Save object inside file.
        /// There is no limitation to use this method (refers to <see cref="Save{T}(string, T)"/> method for simple objects).
        /// </summary>
        /// <typeparam name="T">Type of object saved</typeparam>
        /// <param name="filePath">Path to the file that will contain the object</param>
        /// <param name="value">Object to save</param>
        /// <returns>When this method completes, it returns the <see cref="StorageFile"/> where the object was saved</returns>
        public Task<StorageFile> SaveFileAsync<T>(string filePath, T value)
        {
            return StorageFileHelper.WriteTextToFileAsync(Folder, JsonConvert.SerializeObject(value), filePath, CreationCollisionOption.ReplaceExisting);
        }
    }
}
