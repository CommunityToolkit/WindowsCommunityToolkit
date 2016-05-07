using System;

namespace AppStudio.DataProviders.WordPress
{
    /// <summary>
    /// Implementation of the WordPressCommentSchema class.
    /// </summary>
    public class WordPressCommentSchema : SchemaBase
    {
   
        public string Author { get; set; }
        public DateTime PublishDate { get; set; }
        public string Content { get; set; }
        public string AuthorImage { get; set; }
    }

  
}
