namespace AppStudio.DataProviders.Twitter
{
    public class TwitterDataConfig
    {
        public TwitterQueryType QueryType { get; set; }

        public string Query { get; set; }
    }

    public enum TwitterQueryType
    {
        Home,
        User,
        Search
    }

    public class TwitterOAuthTokens
    {
        public string ConsumerKey { get; set; }

        public string ConsumerSecret { get; set; }

        public string AccessToken { get; set; }

        public string AccessTokenSecret { get; set; }
    }
}
