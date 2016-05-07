using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AppStudio.DataProviders.Core;
using AppStudio.DataProviders.Exceptions;

namespace AppStudio.DataProviders.Rss
{
    public class RssDataProvider : DataProviderBase<RssDataConfig, RssSchema>
    {
        protected override async Task<IEnumerable<TSchema>> GetDataAsync<TSchema>(RssDataConfig config, int maxRecords, IParser<TSchema> parser)
        {
            var settings = new HttpRequestSettings()
            {
                RequestedUri = config.Url
            };

            HttpRequestResult result = await HttpRequest.DownloadAsync(settings);
            if (result.Success)
            {
                return parser.Parse(result.Result);
            }

            throw new RequestFailedException(result.StatusCode, result.Result);
        }

        protected override IParser<RssSchema> GetDefaultParserInternal(RssDataConfig config)
        {
            return new RssParser();
        }

        protected override void ValidateConfig(RssDataConfig config)
        {
            if (config.Url == null)
            {
                throw new ConfigParameterNullException("Url");
            }
        }
    }
}
