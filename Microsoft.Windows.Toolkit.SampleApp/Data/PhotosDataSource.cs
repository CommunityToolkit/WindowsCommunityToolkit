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
        static private ObservableCollection<PhotoDataItem> _photos = null;
        static private ObservableCollection<IEnumerable<PhotoDataItem>> _groupedPhotos = null;

        public IEnumerable<PhotoDataItem> GetItems()
        {
            if(_photos == null)
            {
                var _ = Load();
            }
            return _photos;
        }

        public IEnumerable<IEnumerable<PhotoDataItem>> GetGroupedItems()
        {
            if (_groupedPhotos == null)
            {
                var _ = Load();
            }
            return _groupedPhotos;
        }

        private static async Task Load()
        {
            _photos = new ObservableCollection<PhotoDataItem>();
            _groupedPhotos = new ObservableCollection<IEnumerable<PhotoDataItem>>();
            foreach (var item in await GetPhotos())
                _photos.Add(item);
            foreach (var group in _photos.GroupBy(x => x.Category))
                _groupedPhotos.Add(group);
        }

        private static async Task<IEnumerable<PhotoDataItem>> GetPhotos()
        {
            var uri = new Uri("ms-appx:///Assets/Photos/Photos.json");
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
