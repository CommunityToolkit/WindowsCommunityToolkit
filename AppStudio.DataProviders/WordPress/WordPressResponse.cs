using System;

namespace AppStudio.DataProviders.WordPress
{
    /// <summary>
    /// Implementation of the WordPressResponse class.
    /// </summary>
    public class WordPressResponse
    {
        public WordPressPost[] posts { get; set; }
    }

    public class WordPressPost
    {
        public string id { get; set; }
        public Author author { get; set; }
        public DateTime date { get; set; }
        public string title { get; set; }
        public string content { get; set; }
        public string excerpt { get; set; }
        public string url { get; set; }
        public string featured_image { get; set; }
    }

    public class Author
    {
        public string id { get; set; }
        public string name { get; set; }
        public string avatar_url { get; set; }
    }
}
