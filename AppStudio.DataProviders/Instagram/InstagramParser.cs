using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace AppStudio.DataProviders.Instagram
{
    public class InstagramParser : IParser<InstagramSchema>
    {
        public IEnumerable<InstagramSchema> Parse(string data)
        {
            var response = JsonConvert.DeserializeObject<InstagramResponse>(data);
            if (response != null)
            {
                return response.ToSchema();
            }
            return null;
        }
    }

    internal class InstagramResponse
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "This property is used in serialization.")]
        public InstagramResponseItem[] data { get; set; }

        public IEnumerable<InstagramSchema> ToSchema()
        {
            return this.data.Select(d => d.ToSchema());
        }
    }

    internal class InstagramResponseItem
    {
        private readonly DateTime UnixEpochDate = new DateTime(1970, 1, 1, 0, 0, 0, 0);

        public string id { get; set; }
        public string link { get; set; }
        public int created_time { get; set; }
        public InstagramUser user { get; set; }
        public InstagramImages images { get; set; }
        public InstagramCaption caption { get; set; }

        public InstagramSchema ToSchema()
        {
            var result = new InstagramSchema();

            result._id = this.id;
            result.SourceUrl = this.link;
            result.Published = UnixEpochDate.AddSeconds(this.created_time);
            if (this.user != null)
            {
                result.Author = this.user.username;
            }
            result.ThumbnailUrl = this.images.low_resolution.url;
            result.ImageUrl = this.images.standard_resolution.url;
            if (this.caption != null)
            {
                result.Title = this.caption.text;
            }

            return result;
        }
    }

    internal class InstagramCaption
    {
        public string text { get; set; }
    }

    internal class InstagramImages
    {
        public InstagramImage thumbnail { get; set; }
        public InstagramImage standard_resolution { get; set; }
        public InstagramImage low_resolution { get; set; }        
    }

    internal class InstagramImage
    {
        public string url { get; set; }
    }

    internal class InstagramUser
    {
        public string username { get; set; }
    }
}
