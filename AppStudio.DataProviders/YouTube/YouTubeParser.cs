using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Newtonsoft.Json;

namespace AppStudio.DataProviders.YouTube
{
    public class YouTubePlaylistParser : IParser<YouTubeSchema>
    {
        public IEnumerable<YouTubeSchema> Parse(string data)
        {
            if (string.IsNullOrEmpty(data))
            {
                return null;
            }

            Collection<YouTubeSchema> resultToReturn = new Collection<YouTubeSchema>();
            var playlist = JsonConvert.DeserializeObject<YouTubeResult<YouTubePlaylistResult>>(data);
            if (playlist != null && playlist.items != null)
            {
                foreach (var item in playlist.items)
                {
                    resultToReturn.Add(new YouTubeSchema()
                    {
                        _id = item.id,
                        Title = item.snippet.title,
                        ImageUrl = item.snippet.thumbnails != null ? item.snippet.thumbnails.high.url : string.Empty,
                        Summary = item.snippet.description,
                        Published = item.snippet.publishedAt,
                        VideoId = item.snippet.resourceId.videoId
                    });
                }
            }

            return resultToReturn;

        }
    }

    public class YouTubeSearchParser : IParser<YouTubeSchema>
    {
        public IEnumerable<YouTubeSchema> Parse(string data)
        {
            if (string.IsNullOrEmpty(data))
            {
                return null;
            }

            Collection<YouTubeSchema> resultToReturn = new Collection<YouTubeSchema>();
            var searchList = JsonConvert.DeserializeObject<YouTubeResult<YouTubeSearchResult>>(data);
            if (searchList != null && searchList.items != null)
            {
                foreach (var item in searchList.items)
                {
                    resultToReturn.Add(new YouTubeSchema
                    {
                        _id = item.id.videoId,
                        Title = item.snippet.title,
                        ImageUrl = item.snippet.thumbnails != null ? item.snippet.thumbnails.high.url : string.Empty,
                        Summary = item.snippet.description,
                        Published = item.snippet.publishedAt,
                        VideoId = item.id.videoId
                    });
                }
            }

            return resultToReturn;
        }
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses", Justification = "This class is used in serialization.")]
    internal class YouTubeResult<T>
    {
        public string error { get; set; }
        public List<T> items { get; set; }
    }

    internal class YouTubeSearchResult
    {
        public string kind { get; set; }
        public string etag { get; set; }
        public YouTubeSearchId id { get; set; }
        public YouTubeSearchSnippet snippet { get; set; }
    }

    internal class YouTubeSearchId
    {
        public string kind { get; set; }
        public string videoId { get; set; }
        public string playlistId { get; set; }
    }

    internal class YouTubeSearchSnippet
    {
        public DateTime publishedAt { get; set; }
        public string channelId { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public Thumbnails thumbnails { get; set; }
        public string channelTitle { get; set; }
        public string liveBroadcastContent { get; set; }
    }

    internal class YouTubePlaylistResult
    {
        public string kind { get; set; }
        public string etag { get; set; }
        public string id { get; set; }
        public YouTubePlaylistSnippet snippet { get; set; }
    }

    internal class YouTubePlaylistSnippet
    {
        public DateTime publishedAt { get; set; }
        public string channelId { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public Thumbnails thumbnails { get; set; }
        public string channelTitle { get; set; }
        public string playlistId { get; set; }
        public int position { get; set; }
        public ResourceId resourceId { get; set; }
    }

    internal class YouTubeChannelLookupResult
    {
        public string kind { get; set; }
        public string etag { get; set; }
        public string id { get; set; }
        public ContentDetails contentDetails { get; set; }
    }

    internal class Thumbnails
    {
        public DefaultThumbnail _default { get; set; }
        public MediumThumbnail medium { get; set; }
        public HighThumbnail high { get; set; }
    }

    internal class ResourceId
    {
        public string kind { get; set; }
        public string videoId { get; set; }
    }

    internal class RelatedPlaylists
    {
        public string favorites { get; set; }
        public string uploads { get; set; }
    }

    internal class MediumThumbnail
    {
        public string url { get; set; }
        public int width { get; set; }
        public int height { get; set; }
    }

    internal class LookupResult
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string ThumbnailUrl { get; set; }
        public string ThumbnailBackground { get; set; }
        public string ImageUrl { get; set; }
        public string Summary { get; set; }
        public string Description { get; set; }
        public string Url { get; set; }
    }

    internal class HighThumbnail
    {
        public string url { get; set; }
        public int width { get; set; }
        public int height { get; set; }
    }

    internal class DefaultThumbnail
    {
        public string Url { get; set; }
        [JsonProperty("width")]
        public int Width { get; set; }
        [JsonProperty("height")]
        public int Height { get; set; }
    }

    internal class ContentDetails
    {
        public RelatedPlaylists relatedPlaylists { get; set; }
    }
}
