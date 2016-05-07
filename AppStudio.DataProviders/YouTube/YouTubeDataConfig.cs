namespace AppStudio.DataProviders.YouTube
{
    public class YouTubeDataConfig
    {
        public YouTubeQueryType QueryType { get; set; }
        
        public string Query { get; set; }
    }

    public enum YouTubeQueryType
    {
        Videos,
        Channels,
        Playlist
    }

    public class YouTubeOAuthTokens
    {
        public string ApiKey { get; set; }
    }
}
