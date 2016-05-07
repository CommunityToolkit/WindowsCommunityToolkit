using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using AppStudio.DataProviders.Core;
using AppStudio.DataProviders.Exceptions;

namespace AppStudio.DataProviders.Bing
{
    public class BingDataProvider : DataProviderBase<BingDataConfig, BingSchema>
    {
        private const string BaseUrl = "http://www.bing.com";

        protected override async Task<IEnumerable<TSchema>> GetDataAsync<TSchema>(BingDataConfig config, int maxRecords, IParser<TSchema> parser)
        {
            var countryValue = config.Country.GetStringValue();
            var locParameter = string.IsNullOrEmpty(countryValue) ? string.Empty : $"loc:{countryValue}+";
            var settings = new HttpRequestSettings()
            {
                RequestedUri = new Uri($"{BaseUrl}/search?q={locParameter}{ WebUtility.UrlEncode(config.Query)}&format=rss&count={maxRecords}")
            };

            HttpRequestResult result = await HttpRequest.DownloadAsync(settings);
            if (result.Success)
            {
                return parser.Parse(result.Result);
            }

            throw new RequestFailedException(result.StatusCode, result.Result);
        }

        protected override IParser<BingSchema> GetDefaultParserInternal(BingDataConfig config)
        {
            return new BingParser();
        }

        protected override void ValidateConfig(BingDataConfig config)
        {
            if (config.Query == null)
            {
                throw new ConfigParameterNullException("Query");
            }
        }
    }
}
