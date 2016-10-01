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

using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Toolkit.Uwp.Services.Core;

namespace Microsoft.Toolkit.Uwp.Services.Bing
{
    /// <summary>
    /// Class for connecting to Bing.
    /// </summary>
    public class BingService : IDataService<BingDataProvider, BingResult, BingSearchConfig>
    {
        private BingDataProvider bingDataProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="BingService"/> class.
        /// </summary>
        public BingService()
        {
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

        /// <summary>
        /// Request list data from service provider based upon a given config / query.
        /// </summary>
        /// <param name="config">BingSearchConfig instance.</param>
        /// <param name="maxRecords">Upper limit of records to return.</param>
        /// <returns>Strongly typed list of data returned from the service.</returns>
        public async Task<List<BingResult>> RequestAsync(BingSearchConfig config, int maxRecords = 20)
        {
            List<BingResult> queryResults = new List<BingResult>();

            var results = await Provider.LoadDataAsync(config, maxRecords);

            foreach (var result in results)
            {
                queryResults.Add(result);
            }

            return queryResults;
        }
    }
}
