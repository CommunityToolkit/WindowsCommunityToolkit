using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppStudio.DataProviders.Core;

namespace AppStudio.DataProviders.WordPress
{
    public class WordPressCommentParser : IParser<WordPressCommentSchema>
    {
        public IEnumerable<WordPressCommentSchema> Parse(string data)
        {
            if (string.IsNullOrEmpty(data))
            {
                return null;
            }

            var wordPressResponse = JsonConvert.DeserializeObject<WordPressCommentsResponse>(data);

            return wordPressResponse.comments
                                        .OrderByDescending(c => c.date)
                                        .Select( r => new WordPressCommentSchema()
                                        {
                                            _id = r.id,
                                            Content = r.content.DecodeHtml(),
                                            Author = r.author.name.DecodeHtml(),
                                            AuthorImage = r.author.avatar_url,
                                            PublishDate = r.date
                                        });
        }
    }
}
