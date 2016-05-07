using System;
using System.Collections;

namespace AppStudio.DataProviders.Facebook
{
    public class FacebookSchema : SchemaBase
    {
        public string Title { get; set; }

        public string Summary { get; set; }

        public string Content { get; set; }

        public string ImageUrl { get; set; }

        public string FeedUrl { get; set; }

        public string Author { get; set; }

        public DateTime PublishDate { get; set; }
    }
}
