using System;
using System.Collections;

namespace AppStudio.DataProviders.Flickr
{
    /// <summary>
    /// Implementation of the FlickrSchema class.
    /// </summary>
    public class FlickrSchema : SchemaBase
    {
        public string Title { get; set; }

        public string Summary { get; set; }

        public string ImageUrl { get; set; }

        public DateTime Published { get; set; }

        public string FeedUrl { get; set; }
    }
}
