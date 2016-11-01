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
using Windows.UI.Xaml.Media.Imaging;

namespace Microsoft.Toolkit.Uwp.UI
{
    /// <summary>
    /// Provides methods and tools to cache files in a folder
    /// </summary>
    public class ImageCache : CacheBase<BitmapImage>
    {
        /// <summary>
        /// Private singleton field.
        /// </summary>
        private static ImageCache _instance;

        /// <summary>
        /// Gets public singleton property.
        /// </summary>
        public static ImageCache Instance => _instance ?? (_instance = new ImageCache() { MaintainContext = true });

        /// <summary>
        /// Cache specific hooks to proccess items from http response
        /// </summary>
        /// <param name="stream">inpupt stream</param>
        /// <returns>awaitable task</returns>
        protected override async Task<BitmapImage> InitializeTypeAsync(IRandomAccessStream stream)
        {
            BitmapImage image = new BitmapImage();
            await image.SetSourceAsync(stream).AsTask().ConfigureAwait(false);

            return image;
        }

        /// <summary>
        /// Cache specific hooks to proccess items from http response
        /// </summary>
        /// <param name="baseFile">storage file</param>
        /// <returns>awaitable task</returns>
        protected override async Task<BitmapImage> InitializeTypeAsync(StorageFile baseFile)
        {
            using (var stream = await baseFile.OpenReadAsync().AsTask().ConfigureAwait(MaintainContext))
            {
                return await InitializeTypeAsync(stream).ConfigureAwait(false);
            }
        }
    }
}
