// ******************************************************************
// Copyright (c) Microsoft. All rights reserved.
// This code is licensed under the MIT License (MIT).
// THE CODE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
// TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH
// THE CODE OR THE USE OR OTHER DEALINGS IN THE CODE.
// ******************************************************************

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Toolkit.Uwp.Services.Core;
using Microsoft.Toolkit.Uwp.Services.Exceptions;
using Windows.Web.Http;

namespace Microsoft.Toolkit.Uwp.Services.Bing
{
    /// <summary>
    /// Data Provider for connecting to Bing service.
    /// </summary>
    public class BingDataProvider : DataProviderBase<BingSearchConfig, BingResult>
    {
        /// <summary>
        /// Base Url for service.
        /// </summary>
        private const string BaseUrl = "http://www.bing.com";

        /// <summary>
        /// Wrapper around REST API for making data request.
        /// </summary>
        /// <typeparam name="TSchema">Schema to use</typeparam>
        /// <param name="config">Query configuration.</param>
        /// <param name="maxRecords">Upper limit for records returned.</param>
        /// <param name="parser">IParser implementation for interpreting results.</param>
        /// <returns>Strongly typed list of results.</returns>
        protected override async Task<IEnumerable<TSchema>> GetDataAsync<TSchema>(BingSearchConfig config, int maxRecords, IParser<TSchema> parser)
        {
            var countryValue = config.Country.GetStringValue();
            var languageValue = config.Language.GetStringValue();
            var languageParameter = string.IsNullOrEmpty(languageValue) ? string.Empty : $"language:{languageValue}+";

            if (string.IsNullOrEmpty(countryValue))
            {
                if (CultureInfo.CurrentCulture.IsNeutralCulture)
                {
                    countryValue = BingCountry.None.GetStringValue();
                }
                else
                {
                    countryValue = CultureInfo.CurrentCulture.Name.Split('-')[1].ToLower();
                }
            }

            var locParameter = $"loc:{countryValue}+";
            var queryTypeParameter = string.Empty;

            switch (config.QueryType)
            {
                case BingQueryType.Search:
                    queryTypeParameter = string.Empty;
                    break;
                case BingQueryType.News:
                    queryTypeParameter = "/news";
                    break;
            }

            var uri = new Uri($"{BaseUrl}{queryTypeParameter}/search?q={locParameter}{languageParameter}{WebUtility.UrlEncode(config.Query)}&format=rss&count={maxRecords}");

            using (HttpHelperRequest request = new HttpHelperRequest(uri, HttpMethod.Get))
            {
                using (var response = await HttpHelper.Instance.SendRequestAsync(request).ConfigureAwait(false))
                {
                    var data = await response.GetTextResultAsync().ConfigureAwait(false);

                    if (response.Success && !string.IsNullOrEmpty(data))
                    {
                        return parser.Parse(data);
                    }

                    throw new RequestFailedException(response.StatusCode, data);
                }
            }
        }

        /// <summary>
        /// Returns parser implementation for specified configuration.
        /// </summary>
        /// <param name="config">Query configuration.</param>
        /// <returns>Strongly typed parser.</returns>
        protected override IParser<BingResult> GetDefaultParser(BingSearchConfig config)
        {
            return new BingParser();
        }

        /// <summary>
        /// Check validity of configuration.
        /// </summary>
        /// <param name="config">Query configuration.</param>
        protected override void ValidateConfig(BingSearchConfig config)
        {
            if (config?.Query == null)
            {
                throw new ConfigParameterNullException(nameof(config.Query));
            }
        }
    }
}
