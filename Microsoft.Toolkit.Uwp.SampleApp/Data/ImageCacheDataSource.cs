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
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Data;

namespace Microsoft.Toolkit.Uwp.SampleApp.Data
{
    [Bindable]
    public class ImageCacheDataSource
    {
        private static ObservableCollection<Uri> _photos;

        /// <summary>
        /// Get Photo Uris and return to caller
        /// </summary>
        /// <returns>Observable Collection of Uri</returns>
        public async Task<ObservableCollection<Uri>> GetItemsAsync()
        {
            if (_photos == null)
            {
                await LoadAsync();
            }

            return _photos;
        }

        private static async Task LoadAsync()
        {
            _photos = new ObservableCollection<Uri>();

            foreach (var item in await GetImageCacheData())
            {
                _photos.Add(new Uri(item));
            }
        }

        private static async Task<IEnumerable<string>> GetImageCacheData()
        {
            var uri = new Uri($"ms-appx:///Assets/Photos/ImageCache.json");
            StorageFile file = await StorageFile.GetFileFromApplicationUriAsync(uri);
            IRandomAccessStreamWithContentType randomStream = await file.OpenReadAsync();

            using (StreamReader r = new StreamReader(randomStream.AsStreamForRead()))
            {
                return Parse(await r.ReadToEndAsync());
            }
        }

        private static IEnumerable<string> Parse(string jsonData)
        {
            return JsonConvert.DeserializeObject<IList<string>>(jsonData);
        }
    }
}
