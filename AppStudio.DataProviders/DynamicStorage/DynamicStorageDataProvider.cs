using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AppStudio.DataProviders.Core;
using AppStudio.DataProviders.Exceptions;

namespace AppStudio.DataProviders.DynamicStorage
{
    public class DynamicStorageDataProvider<T> : DataProviderBase<DynamicStorageDataConfig, T> where T : SchemaBase
    {
        protected override async Task<IEnumerable<TSchema>> GetDataAsync<TSchema>(DynamicStorageDataConfig config, int maxRecords, IParser<TSchema> parser)
        {
            var settings = new HttpRequestSettings
            {
                RequestedUri = new Uri(string.Format("{0}&pageIndex={1}&blockSize={2}", config.Url, config.PageIndex, maxRecords)),
                UserAgent = "NativeHost"
            };

            settings.Headers["WAS-APPID"] = config.AppId;
            settings.Headers["WAS-STOREID"] = config.StoreId;
            settings.Headers["WAS-DEVICETYPE"] = config.DeviceType;
            settings.Headers["WAS-ISBACKGROUND"] = config.IsBackgroundTask.ToString();

            HttpRequestResult result = await HttpRequest.DownloadAsync(settings);
            if (result.Success)
            {
                return parser.Parse(result.Result);
            }

            throw new RequestFailedException(result.StatusCode, result.Result);
        }

        protected override IParser<T> GetDefaultParserInternal(DynamicStorageDataConfig config)
        {
            return new JsonParser<T>();
        }

        protected override void ValidateConfig(DynamicStorageDataConfig config)
        {
            if (config.Url == null)
            {
                throw new ConfigParameterNullException("Url");
            }
        }
    }
}
