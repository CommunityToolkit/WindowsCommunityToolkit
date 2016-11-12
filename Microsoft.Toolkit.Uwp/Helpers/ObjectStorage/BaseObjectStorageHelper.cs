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

using System.Threading.Tasks;
using Newtonsoft.Json;
using Windows.Storage;

namespace Microsoft.Toolkit.Uwp
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
        /// Detect if a setting already exists in a composite.
        /// </summary>
        /// <param name="compositeKey">Key of the composite (that contains settings)</param>
        /// <param name="key">Key of the setting (that contains object)</param>
        /// <returns>True if a value exists</returns>
        public bool KeyExists(string compositeKey, string key)
        {
            if (KeyExists(compositeKey))
            {
                Windows.Storage.ApplicationDataCompositeValue composite = (Windows.Storage.ApplicationDataCompositeValue)Settings.Values[compositeKey];
                if (composite != null)
                {
                    return composite.ContainsKey(key);
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
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
            string value = (string)Settings.Values[key];
            if (value != null)
            {
                return JsonConvert.DeserializeObject<T>(value);
            }

            return @default;
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
            Windows.Storage.ApplicationDataCompositeValue composite = (Windows.Storage.ApplicationDataCompositeValue)Settings.Values[compositeKey];
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
            if (KeyExists(key))
            {
                Settings.Values[key] = JsonConvert.SerializeObject(value);
            }
            else
            {
                Settings.Values.Add(key, JsonConvert.SerializeObject(value));
            }
        }

        /// <summary>
        /// Save single item by its key in composite.
        /// This method should be considered for objects that do not exceed 8k bytes during the lifetime of the application
        /// (refers to <see cref="SaveFileAsync{T}(string, T)"/> for complex/large objects).
        /// </summary>
        /// <typeparam name="T">Type of object saved</typeparam>
        /// <param name="compositeKey">Key of the composite (that contains settings)</param>
        /// <param name="key">Key of the value saved</param>
        /// <param name="value">Object to save</param>
        public void Save<T>(string compositeKey, string key, T value)
        {
            if (KeyExists(compositeKey))
            {
                Windows.Storage.ApplicationDataCompositeValue composite = (Windows.Storage.ApplicationDataCompositeValue)Settings.Values[compositeKey];
                if (KeyExists(compositeKey, key))
                {
                    composite[key] = JsonConvert.SerializeObject(value);
                }
                else
                {
                    composite.Add(key, JsonConvert.SerializeObject(value));
                }
            }
            else
            {
                Windows.Storage.ApplicationDataCompositeValue composite = new Windows.Storage.ApplicationDataCompositeValue();
                composite.Add(key, JsonConvert.SerializeObject(value));
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
