using System;
using System.Collections;

namespace AppStudio.DataProviders.Instagram
{
    public class InstagramSchema : SchemaBase
    {
        public string Title { get; set; }

        public string ImageUrl { get; set; }

        public string ThumbnailUrl { get; set; }

        public string SourceUrl { get; set; }

        public DateTime Published { get; set; }

        public string Author { get; set; }
    }
}
