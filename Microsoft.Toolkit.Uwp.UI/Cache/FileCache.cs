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
        public static FileCache Instance => _instance ?? (_instance = new FileCache() { MaintainContext = false });

        /// <summary>
        /// Cache specific hooks to proccess items from http response
        /// </summary>
        /// <param name="stream">input stream</param>
        /// <returns>awaitable task</returns>
        protected override Task<StorageFile> InitializeTypeAsync(IRandomAccessStream stream)
        {
            // nothing to do in this instance;
            return null;
        }

        /// <summary>
        /// Cache specific hooks to proccess items from http response
        /// </summary>
        /// <param name="baseFile">storage file</param>
        /// <returns>awaitable task</returns>
        protected override Task<StorageFile> InitializeTypeAsync(StorageFile baseFile)
        {
            return Task.Run(() => baseFile);
        }
    }
}
