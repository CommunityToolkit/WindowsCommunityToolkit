using System;

namespace AppStudio.DataProviders.WordPress
{
    /// <summary>
    /// Implementation of the WordPressSchema class.
    /// </summary>
    public class WordPressSchema : SchemaBase
    {
   
        public string Author { get; set; }
        public DateTime PublishDate { get; set; }
        public string Title { get; set; }
        public string Summary { get; set; }
        public string Content { get; set; }
        public string ImageUrl { get; set; }
        public string FeedUrl { get; set; }
    }

  
}
