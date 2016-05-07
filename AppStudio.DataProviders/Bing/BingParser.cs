using System.Collections.Generic;
using System.Linq;
using AppStudio.DataProviders.Rss;

namespace AppStudio.DataProviders.Bing
{
    public class BingParser : IParser<BingSchema>
    {
        public IEnumerable<BingSchema> Parse(string data)
        {
            if (string.IsNullOrEmpty(data))
            {
                return null;
            }

            RssParser rssParser = new RssParser();
            IEnumerable<RssSchema> syndicationItems = rssParser.Parse(data);
            return (from r in syndicationItems
                    select new BingSchema()
                    {
                        _id = r._id,
                        Title = r.Title,
                        Summary = r.Summary,
                        Link = r.FeedUrl,
                        Published = r.PublishDate
                    });
        }
    }
}
