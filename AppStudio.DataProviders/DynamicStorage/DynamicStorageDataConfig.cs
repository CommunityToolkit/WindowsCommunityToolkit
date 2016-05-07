using System;

namespace AppStudio.DataProviders.DynamicStorage
{
    public class DynamicStorageDataConfig
    {
        public DynamicStorageDataConfig()
        {
            BlockSize = 40;
        }

        public Uri Url { get; set; }

        public string AppId { get; set; }

        public string StoreId { get; set; }

        public string DeviceType { get; set; }

        public bool IsBackgroundTask { get; set; }

        public int PageIndex { get; set; }

        public int BlockSize { get; set; }
    }
}
