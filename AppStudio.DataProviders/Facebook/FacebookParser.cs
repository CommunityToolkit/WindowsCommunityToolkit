using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using AppStudio.DataProviders.Core;
using Newtonsoft.Json;


namespace AppStudio.DataProviders.Facebook
{
    public class FacebookParser : IParser<FacebookSchema>
    {
        public IEnumerable<FacebookSchema> Parse(string data)
        {
            if (string.IsNullOrEmpty(data))
            {
                return null;
            }

            Collection<FacebookSchema> resultToReturn = new Collection<FacebookSchema>();
            var searchList = JsonConvert.DeserializeObject<FacebookGraphResponse>(data);
            foreach (var i in searchList.data.Where(w => !string.IsNullOrEmpty(w.message)).OrderByDescending(o => o.created_time))
            {
                var item = new FacebookSchema
                {
                    _id = i.id,
                    Author = i.from.name,
                    PublishDate = i.created_time,
                    Title = i.message.DecodeHtml(),
                    Summary = i.message.DecodeHtml(),
                    Content = i.message,
                    ImageUrl = ConvertImageUrlFromParameter(i.full_picture),
                    FeedUrl = BuildFeedUrl(i.from.id, i.id, i.link)
                };
                resultToReturn.Add(item);
            }

            return resultToReturn;
        }

        private static string ConvertImageUrlFromParameter(string imageUrl)
        {
            string parsedImageUrl = null;
            if (!string.IsNullOrEmpty(imageUrl) && imageUrl.IndexOf("url=") > 0)
            {
                Uri imageUri = new Uri(imageUrl);
                var imageUriQuery = imageUri.Query.Replace("?", string.Empty).Replace("&amp;", "&");

                var imageUriQueryParameters = imageUriQuery.Split('&').Select(q => q.Split('='))
                        .Where(s => s != null && s.Length >= 2)
                        .ToDictionary(k => k[0], v => v[1]);

                string url;
                if (imageUriQueryParameters.TryGetValue("url", out url) && !string.IsNullOrEmpty(url))
                {
                    parsedImageUrl = WebUtility.UrlDecode(url);
                }
            }
            else if (!string.IsNullOrEmpty(imageUrl))
            {
                parsedImageUrl = WebUtility.HtmlDecode(imageUrl);
            }

            return parsedImageUrl;
        }

        private static string BuildFeedUrl(string authorId, string id, string link)
        {
            if (!string.IsNullOrEmpty(link))
            {
                return link;
            }

            const string baseUrl = "https://www.facebook.com";
            var Ids = id.Split('_');
            if (Ids.Length > 1)
            {
                var postId = id.Split('_')[1];
                return $"{baseUrl}/{authorId}/posts/{postId}";
            }

            return $"{baseUrl}/{authorId}";
        }

    }

    internal class FacebookGraphResponse
    {
        public GraphData[] data { get; set; }
    }

    internal class From
    {
        public string category { get; set; }
        public string name { get; set; }
        public string id { get; set; }
    }

    internal class GraphData
    {
        public string id { get; set; }
        public From from { get; set; }
        public string type { get; set; }
        public DateTime created_time { get; set; }
        public DateTime updated_time { get; set; }
        public string message { get; set; }
        public string full_picture { get; set; }
        public string link { get; set; }
    }
}
