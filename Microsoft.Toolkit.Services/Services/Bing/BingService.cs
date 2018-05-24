// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Toolkit.Collections;

namespace Microsoft.Toolkit.Services.Bing
{
    /// <summary>
    /// Class for connecting to Bing.
    /// </summary>
    public class BingService : IDataService<BingDataProvider, BingResult, BingSearchConfig>, IIncrementalSource<BingResult>
    {
        private readonly BingSearchConfig _config;

        private BingDataProvider bingDataProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="BingService"/> class.
        /// </summary>
        public BingService()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BingService"/> class.
        /// </summary>
        /// <param name="config">BingSearchConfig instance.</param>
        protected BingService(BingSearchConfig config)
        {
            _config = config;
        }

        /// <summary>
        /// Private singleton field.
        /// </summary>
        private static BingService instance;

        /// <summary>
        /// Gets public singleton property.
        /// </summary>
        public static BingService Instance => instance ?? (instance = new BingService());

        /// <summary>
        /// Gets a reference to an instance of the underlying data provider.
        /// </summary>
        public BingDataProvider Provider => bingDataProvider ?? (bingDataProvider = new BingDataProvider());

#if WINRT
        /// <summary>
        /// Gets an instance of <see cref="Uwp.IncrementalLoadingCollection{TSource, IType}"/> class that is able to load search data incrementally.
        /// </summary>
        /// <param name="config">BingSearchConfig instance.</param>
        /// <param name="maxRecords">Upper limit of records to return.</param>
        /// <returns>An instance of <see cref="Uwp.IncrementalLoadingCollection{TSource, IType}"/> class that is able to load search data incrementally.</returns>
        public static Uwp.IncrementalLoadingCollection<BingService, BingResult> GetAsIncrementalLoading(BingSearchConfig config, int maxRecords = 20)
        {
            var service = new BingService(config);
            return new Uwp.IncrementalLoadingCollection<BingService, BingResult>(service, maxRecords);
        }
#endif

        /// <summary>
        /// Request list data from service provider based upon a given config / query.
        /// </summary>
        /// <param name="config">BingSearchConfig instance.</param>
        /// <param name="maxRecords">Upper limit of records to return.</param>
        /// <param name="pageIndex">The zero-based index of the page that corresponds to the items to retrieve.</param>
        /// <returns>Strongly typed list of data returned from the service.</returns>
        public async Task<List<BingResult>> RequestAsync(BingSearchConfig config, int maxRecords = 20, int pageIndex = 0)
        {
            List<BingResult> queryResults = new List<BingResult>();

            var results = await Provider.LoadDataAsync(config, maxRecords, pageIndex);

            foreach (var result in results)
            {
                queryResults.Add(result);
            }

            return queryResults;
        }

        /// <summary>
        /// Retrieves items based on <paramref name="pageIndex"/> and <paramref name="pageSize"/> arguments.
        /// </summary>
        /// <param name="pageIndex">
        /// The zero-based index of the page that corresponds to the items to retrieve.
        /// </param>
        /// <param name="pageSize">
        /// The number of records to retrieve for the specified <paramref name="pageIndex"/>.
        /// </param>
        /// <param name="cancellationToken">
        /// Used to propagate notification that operation should be canceled.
        /// </param>
        /// <returns>
        /// Strongly typed list of data returned from the service.
        /// </returns>
        public async Task<IEnumerable<BingResult>> GetPagedItemsAsync(int pageIndex, int pageSize, CancellationToken cancellationToken = default(CancellationToken))
        {
            var queryResult = await RequestAsync(_config, pageSize, pageIndex);
            return queryResult.AsEnumerable();
        }
    }
}
