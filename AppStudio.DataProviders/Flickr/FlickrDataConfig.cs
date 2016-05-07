namespace AppStudio.DataProviders.Flickr
{
    public class FlickrDataConfig
    {
        public FlickrQueryType QueryType { get; set; }

        public string Query { get; set; }
    }

    public enum FlickrQueryType
    {
        Id,
        Tags
    }
}
