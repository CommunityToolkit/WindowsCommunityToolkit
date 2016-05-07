using System;
using AppStudio.DataProviders.Core;

namespace AppStudio.DataProviders.Rss
{
    /// <summary>
    /// Implementation of the RssSchema class.
    /// </summary>
    public class RssSchema : SchemaBase
    {
        public string Title { get; set; }

        public string Summary { get; set; }

        public string Content { get; set; }

        public string ImageUrl { get; set; }

        public string ExtraImageUrl { get; set; }

        public string MediaUrl { get; set; }

        public string FeedUrl { get; set; }

        public string Author { get; set; }

        public DateTime PublishDate { get; set; }
    }
}
