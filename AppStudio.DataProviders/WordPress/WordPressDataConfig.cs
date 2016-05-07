namespace AppStudio.DataProviders.WordPress
{
    public class WordPressDataConfig
    {
        public WordPressQueryType QueryType { get; set; }

        public string Query { get; set; }

        public string FilterBy { get; set; }
    }

    public enum WordPressQueryType
    {
        Posts,
        Tag,
        Category
    }
}
