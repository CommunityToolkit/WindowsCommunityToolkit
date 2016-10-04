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
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Data;

namespace Microsoft.Toolkit.Uwp.SampleApp.Data
{
    [Bindable]
    public class PhotosDataSource
    {
        private static ObservableCollection<PhotoDataItem> _photos;
        private static ObservableCollection<IEnumerable<PhotoDataItem>> _groupedPhotos;
        private static bool _isOnlineCached;

        public async Task<ObservableCollection<PhotoDataItem>> GetItemsAsync(bool online = false, int maxCount = -1)
        {
            CheckCacheState(online);

            if (_photos == null)
            {
                await LoadAsync(online, maxCount);
            }

            return _photos;
        }

        public async Task<ObservableCollection<IEnumerable<PhotoDataItem>>> GetGroupedItemsAsync(bool online = false, int maxCount = -1)
        {
            CheckCacheState(online);

            if (_groupedPhotos == null)
            {
                await LoadAsync(online, maxCount);
            }

            return _groupedPhotos;
        }

        private static async Task LoadAsync(bool online, int maxCount)
        {
            _isOnlineCached = online;
            _photos = new ObservableCollection<PhotoDataItem>();
            _groupedPhotos = new ObservableCollection<IEnumerable<PhotoDataItem>>();

            foreach (var item in await GetPhotosAsync(online))
            {
                _photos.Add(item);

                if (maxCount != -1)
                {
                    maxCount--;

                    if (maxCount == 0)
                    {
                        break;
                    }
                }
            }

            foreach (var group in _photos.GroupBy(x => x.Category))
            {
                _groupedPhotos.Add(group);
            }
        }

        private static async Task<IEnumerable<PhotoDataItem>> GetPhotosAsync(bool online)
        {
            var prefix = online ? "Online" : string.Empty;
            var uri = new Uri($"ms-appx:///Assets/Photos/{prefix}Photos.json");
            StorageFile file = await StorageFile.GetFileFromApplicationUriAsync(uri);
            IRandomAccessStreamWithContentType randomStream = await file.OpenReadAsync();

            using (StreamReader r = new StreamReader(randomStream.AsStreamForRead()))
            {
                return Parse(await r.ReadToEndAsync());
            }
        }

        private static IEnumerable<PhotoDataItem> Parse(string jsonData)
        {
            return JsonConvert.DeserializeObject<IList<PhotoDataItem>>(jsonData);
        }

        private static void CheckCacheState(bool online)
        {
            if (_isOnlineCached != online)
            {
                _photos = null;
                _groupedPhotos = null;
            }
        }
    }
}
