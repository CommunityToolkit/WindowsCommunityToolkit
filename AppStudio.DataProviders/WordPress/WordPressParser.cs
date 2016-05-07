using AppStudio.DataProviders.Core;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace AppStudio.DataProviders.WordPress
{
    public class WordPressParser : IParser<WordPressSchema>
    {
        public IEnumerable<WordPressSchema> Parse(string data)
        {
            if (string.IsNullOrEmpty(data))
            {
                return null;
            }

            var wordPressItems = JsonConvert.DeserializeObject<WordPressResponse>(data);

            return (from r in wordPressItems.posts
                    select new WordPressSchema()
                    {
                        _id = r.id,
                        Title = r.title.DecodeHtml(),
                        Summary = r.excerpt.DecodeHtml(),
                        Content = r.content,
                        Author = r.author.name.DecodeHtml(),                  
                        ImageUrl = r.featured_image,
                        PublishDate = r.date,
                        FeedUrl = r.url
                    });
        }
    }
}
