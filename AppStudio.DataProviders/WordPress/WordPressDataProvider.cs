using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using AppStudio.DataProviders.Core;
using AppStudio.DataProviders.Exceptions;
using System.Linq;

namespace AppStudio.DataProviders.WordPress
{
    public class WordPressDataProvider : DataProviderBase<WordPressDataConfig, WordPressSchema>
    {
        private const string BaseUrl = "https://public-api.wordpress.com/rest/v1.1";     

        protected override async Task<IEnumerable<TSchema>> GetDataAsync<TSchema>(WordPressDataConfig config, int maxRecords, IParser<TSchema> parser)
        {      
            var wordPressUrlRequest = string.Empty;
            switch (config.QueryType)
            {
                case WordPressQueryType.Tag:
                    wordPressUrlRequest = $"{BaseUrl}/sites/{config.Query}/posts/?tag={config.FilterBy}&number={maxRecords}";
                    break;
                case WordPressQueryType.Category:
                    wordPressUrlRequest = $"{BaseUrl}/sites/{config.Query}/posts/?category={config.FilterBy}&number={maxRecords}";
                    break;
                default:
                    wordPressUrlRequest = $"{BaseUrl}/sites/{config.Query}/posts/?number={maxRecords}";
                    break;

            }

            var settings = new HttpRequestSettings()
            {
                RequestedUri = new Uri(wordPressUrlRequest)
            };

            HttpRequestResult result = await HttpRequest.DownloadAsync(settings);
            if (result.Success)
            {
                return parser.Parse(result.Result);
            }

            throw new RequestFailedException(result.StatusCode, result.Result);
        }

        public async Task<IEnumerable<WordPressCommentSchema>> GetComments(string site, string postId, int maxRecords)
        {
            return await GetComments(site, postId, maxRecords, new WordPressCommentParser());
        }

        public async Task<IEnumerable<TSchema>> GetComments<TSchema>(string site, string postId, int maxRecords, IParser<TSchema> parser) where TSchema : SchemaBase
        {
            var wordPressUrlRequest = $"{BaseUrl}/sites/{site}/posts/{postId}/replies";

            var settings = new HttpRequestSettings()
            {
                RequestedUri = new Uri(wordPressUrlRequest)
            };

            HttpRequestResult result = await HttpRequest.DownloadAsync(settings);
            if (result.Success)
            {
                var comments = parser.Parse(result.Result);
                if (comments != null)
                {
                    return comments
                                .Take(maxRecords)
                                .ToList();
                }
                else
                {
                    return new TSchema[0];
                }
            }

            throw new RequestFailedException(result.StatusCode, result.Result);
        }

        protected override IParser<WordPressSchema> GetDefaultParserInternal(WordPressDataConfig config)
        {
            return new WordPressParser();
        }

        protected override void ValidateConfig(WordPressDataConfig config)
        {
            if (config.Query == null)
            {
                throw new ConfigParameterNullException("Query");
            }
        }
    }
}
