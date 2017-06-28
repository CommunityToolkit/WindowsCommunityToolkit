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

using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Storage;

namespace Microsoft.Toolkit.Uwp
{
    /// <summary>
    /// Service used to store data
    /// </summary>
    public interface IObjectStorageHelper
    {
        /// <summary>
        /// Detect if a setting already exists
        /// </summary>
        /// <param name="key">Key of the setting (that contains object)</param>
        /// <returns>True if a value exists</returns>
        bool KeyExists(string key);

        /// <summary>
        /// Detect if a setting already exists in composite.
        /// </summary>
        /// <param name="compositeKey">Key of the composite (that contains settings)</param>
        /// <param name="key">Key of the setting (that contains object)</param>
        /// <returns>True if a value exists</returns>
        bool KeyExists(string compositeKey, string key);

        /// <summary>
        /// Retrieve single item by its key
        /// </summary>
        /// <typeparam name="T">Type of object retrieved</typeparam>
        /// <param name="key">Key of the object</param>
        /// <param name="default">Default value of the object</param>
        /// <returns>The T object</returns>
        T Read<T>(string key, T @default = default(T));

        /// <summary>
        /// Retrieve single item by its key in composite.
        /// </summary>
        /// <typeparam name="T">Type of object retrieved</typeparam>
        /// <param name="compositeKey">Key of the composite (that contains settings)</param>
        /// <param name="key">Key of the object</param>
        /// <param name="default">Default value of the object</param>
        /// <returns>The T object</returns>
        T Read<T>(string compositeKey, string key, T @default = default(T));

        /// <summary>
        /// Save single item by its key
        /// </summary>
        /// <typeparam name="T">Type of object saved</typeparam>
        /// <param name="key">Key of the value saved</param>
        /// <param name="value">Object to save</param>
        void Save<T>(string key, T value);

        /// <summary>
        /// Save a group of items by its key in a composite.
        /// This method should be considered for objects that do not exceed 8k bytes during the lifetime of the application
        /// (refers to <see cref="SaveFileAsync{T}(string, T)"/> for complex/large objects) and for groups of settings which
        /// need to be treated in an atomic way.
        /// </summary>
        /// <typeparam name="T">Type of object saved</typeparam>
        /// <param name="compositeKey">Key of the composite (that contains settings)</param>
        /// <param name="values">Objects to save</param>
        void Save<T>(string compositeKey, IDictionary<string, T> values);

        /// <summary>
        /// Detect if a file already exists
        /// </summary>
        /// <param name="filePath">Key of the file (that contains object)</param>
        /// <returns>True if a value exists</returns>
        Task<bool> FileExistsAsync(string filePath);

        /// <summary>
        /// Retrieve object from file
        /// </summary>
        /// <typeparam name="T">Type of object retrieved</typeparam>
        /// <param name="filePath">Path to the file that contains the object</param>
        /// <param name="default">Default value of the object</param>
        /// <returns>Waiting task until completion with the object in the file</returns>
        Task<T> ReadFileAsync<T>(string filePath, T @default = default(T));

        /// <summary>
        /// Save object inside file
        /// </summary>
        /// <typeparam name="T">Type of object saved</typeparam>
        /// <param name="filePath">Path to the file that will contain the object</param>
        /// <param name="value">Object to save</param>
        /// <returns>Waiting task until completion</returns>
        Task<StorageFile> SaveFileAsync<T>(string filePath, T value);
    }
}
