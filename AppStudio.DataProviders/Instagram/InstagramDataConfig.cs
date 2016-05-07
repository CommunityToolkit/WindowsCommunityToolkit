namespace AppStudio.DataProviders.Instagram
{
    public class InstagramDataConfig
    {
        public InstagramQueryType QueryType { get; set; }
        
        public string Query { get; set; }
    }

    public enum InstagramQueryType
    {
        Tag,
        Id
    }

    public class InstagramOAuthTokens
    {
        public string ClientId { get; set; }
    }
}
