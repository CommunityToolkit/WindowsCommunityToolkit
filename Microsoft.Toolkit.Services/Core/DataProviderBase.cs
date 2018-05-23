// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Microsoft.Toolkit.Services
{
    /// <summary>
    /// Base class for data providers in this library.
    /// </summary>
    /// <typeparam name="TConfig">Query configuration type for given provider.</typeparam>
    public abstract class DataProviderBase<TConfig>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DataProviderBase{TConfig}"/> class.
        /// </summary>
        public DataProviderBase()
        {
        }

        /// <summary>
        /// Load data from provider endpoint.
        /// </summary>
        /// <typeparam name="TSchema">Strong typed object to parse the response items into.</typeparam>
        /// <param name="config">Query configuration.</param>
        /// <param name="maxRecords">Upper record limit.</param>
        /// <param name="pageIndex">The zero-based index of the page that corresponds to the items to retrieve.</param>
        /// <param name="parser">Parser to use for results.</param>
        /// <returns>Strong typed list of results.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "This is an async method, so nesting generic types is necessary.")]
        public async Task<IEnumerable<TSchema>> LoadDataAsync<TSchema>(TConfig config, int maxRecords, int pageIndex, Parsers.IParser<TSchema> parser)
            where TSchema : Parsers.SchemaBase
        {
            if (config == null)
            {
                throw new ConfigNullException();
            }

            if (parser == null)
            {
                throw new ParserNullException();
            }

            ValidateConfig(config);

            var result = await GetDataAsync(config, maxRecords, pageIndex, parser);
            if (result != null)
            {
                return result
                    .Take(maxRecords)
                    .ToList();
            }

            return Array.Empty<TSchema>();
        }

        private static HttpClient httpClient;

        /// <summary>
        /// Gets or sets static instance of HttpClient.
        /// </summary>
        public static HttpClient HttpClient
        {
            get { return httpClient ?? (httpClient = new HttpClient()); }
            set { httpClient = value; }
        }

        /// <summary>
        /// Derived classes will have to implement this method to return provider data
        /// </summary>
        /// <param name="config">Configuration to use</param>
        /// <param name="maxRecords">Maximum number of records to return</param>
        /// <param name="pageIndex">The zero-based index of the page that corresponds to the items to retrieve.</param>
        /// <param name="parser">Parser to use</param>
        /// <typeparam name="TSchema">Schema defining data returned</typeparam>
        /// <returns>List of data</returns>
        protected abstract Task<IEnumerable<TSchema>> GetDataAsync<TSchema>(TConfig config, int maxRecords, int pageIndex, Parsers.IParser<TSchema> parser)
            where TSchema : Parsers.SchemaBase;

        /// <summary>
        /// Method provided by derived class to validate specified configuration
        /// </summary>
        /// <param name="config">Configuration to validate</param>
        protected abstract void ValidateConfig(TConfig config);
    }
}