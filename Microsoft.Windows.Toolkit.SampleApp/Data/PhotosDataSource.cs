using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

using Windows.Storage;
using Windows.Storage.Streams;

using Newtonsoft.Json;
using System.Collections.ObjectModel;
using Windows.UI.Xaml.Data;

namespace Microsoft.Windows.Toolkit.SampleApp.Data
{
    [Bindable]
    public class PhotosDataSource
    {
        private static ObservableCollection<PhotoDataItem> _photos;
        private static ObservableCollection<IEnumerable<PhotoDataItem>> _groupedPhotos;
        private static bool _isOnlineCached;

        private static void CheckCacheState(bool online)
        {
            if (_isOnlineCached != online)
            {
                _photos = null;
                _groupedPhotos = null;
            }
        }

        public IEnumerable<PhotoDataItem> GetItems(bool online = false)
        {
            CheckCacheState(online);

            if(_photos == null)
            {
                var _ = Load(online);
            }
            return _photos;
        }

        public IEnumerable<IEnumerable<PhotoDataItem>> GetGroupedItems(bool online = false)
        {
            CheckCacheState(online);

            if (_groupedPhotos == null)
            {
                var _ = Load(online);
            }
            return _groupedPhotos;
        }

        private static async Task Load(bool online)
        {
            _isOnlineCached = online;
            _photos = new ObservableCollection<PhotoDataItem>();
            _groupedPhotos = new ObservableCollection<IEnumerable<PhotoDataItem>>();
            foreach (var item in await GetPhotos(online))
                _photos.Add(item);
            foreach (var group in _photos.GroupBy(x => x.Category))
                _groupedPhotos.Add(group);
        }

        private static async Task<IEnumerable<PhotoDataItem>> GetPhotos(bool online)
        {
            var prefix = online ? "Online" : "";
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
    }

    public class PhotoDataItem
    {
        public string Title { get; set; }
        public string Category { get; set; }        
        public string Thumbnail { get; set; }
    }
}
