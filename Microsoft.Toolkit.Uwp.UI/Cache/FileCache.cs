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

using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Streams;

namespace Microsoft.Toolkit.Uwp.UI
{
    /// <summary>
    /// Provides methods and tools to cache files in a folder
    /// </summary>
    public class FileCache : CacheBase<StorageFile>
    {
        /// <summary>
        /// Private singleton field.
        /// </summary>
        private static FileCache _instance;

        /// <summary>
        /// Gets public singleton property.
        /// </summary>
        public static FileCache Instance => _instance ?? (_instance = new FileCache());

        /// <summary>
        /// Cache specific hooks to process items from HTTP response
        /// </summary>
        /// <param name="stream">input stream</param>
        /// <param name="initializerKeyValues">key value pairs used when initializing instance of generic type</param>
        /// <returns>awaitable task</returns>
        protected override Task<StorageFile> InitializeTypeAsync(Stream stream, List<KeyValuePair<string, object>> initializerKeyValues = null)
        {
            // nothing to do in this instance;
            return null;
        }

        /// <summary>
        /// Cache specific hooks to process items from HTTP response
        /// </summary>
        /// <param name="baseFile">storage file</param>
        /// <param name="initializerKeyValues">key value pairs used when initializing instance of generic type</param>
        /// <returns>awaitable task</returns>
        protected override Task<StorageFile> InitializeTypeAsync(StorageFile baseFile, List<KeyValuePair<string, object>> initializerKeyValues = null)
        {
            return Task.Run(() => baseFile);
        }
    }
}
