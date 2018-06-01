// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Microsoft.Toolkit.Services.Bing
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
        /// <param name="pageIndex">The zero-based index of the page that corresponds to the items to retrieve.</param>
        /// <param name="parser">IParser implementation for interpreting results.</param>
        /// <returns>Strongly typed list of results.</returns>
        protected override async Task<IEnumerable<TSchema>> GetDataAsync<TSchema>(BingSearchConfig config, int maxRecords, int pageIndex, Parsers.IParser<TSchema> parser)
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

            var uri = new Uri($"{BaseUrl}{queryTypeParameter}/search?q={locParameter}{languageParameter}{WebUtility.UrlEncode(config.Query)}&format=rss&count={maxRecords}&first={(pageIndex * maxRecords) + (pageIndex > 0 ? 1 : 0)}");

            using (HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, uri))
            {
                using (var response = await HttpClient.SendAsync(request).ConfigureAwait(false))
                {
                    string data = response.Content == null ? null : await response.Content.ReadAsStringAsync();

                    if (response.IsSuccessStatusCode && !string.IsNullOrEmpty(data))
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
        protected override Parsers.IParser<BingResult> GetDefaultParser(BingSearchConfig config)
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