// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Windows.Foundation.Collections;
using winsdkfb;
using winsdkfb.Graph;

namespace Microsoft.Toolkit.Uwp.Services.Facebook
{
    /// <summary>
    /// Type to handle paged requests to Facebook Graph.
    /// </summary>
    /// <typeparam name="T">Strong type to return.</typeparam>
    public class FacebookRequestSource<T> : Collections.IIncrementalSource<T>
    {
        private bool _isFirstCall = true;

        private FBPaginatedArray _paginatedArray;

        private FacebookDataConfig _config;

        private string _fields;

        private PropertySet _propertySet;

        private FBJsonClassFactory _factory;

        private string _limit;

        private int _maxPages;

        /// <summary>
        /// Initializes a new instance of the <see cref="FacebookRequestSource{T}"/> class.
        /// </summary>
        public FacebookRequestSource()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FacebookRequestSource{T}"/> class.
        /// </summary>
        /// <param name="config">Config containing query information.</param>
        /// <param name="fields">Comma-separated list of properties expected in the JSON response.  Accompanying properties must be found on the strong-typed T.</param>
        /// <param name="limit">A string representation of the number of records for page - i.e. pageSize.</param>
        /// <param name="maxPages">Upper limit of pages to return.</param>
        public FacebookRequestSource(FacebookDataConfig config, string fields, string limit, int maxPages)
        {
            _config = config;
            _fields = fields;
            _limit = limit;
            _maxPages = maxPages;

            _propertySet = new PropertySet { { "fields", _fields }, { "limit", _limit } };

            _factory = new FBJsonClassFactory(s => JsonConvert.DeserializeObject(s, typeof(T)));

            // FBPaginatedArray does not allow us to set page size per request so we must go with first supplied - see https://github.com/Microsoft/winsdkfb/issues/221
            _paginatedArray = new FBPaginatedArray(_config.Query, _propertySet, _factory);
        }

        /// <summary>
        /// Returns strong typed page of data.
        /// </summary>
        /// <param name="pageIndex">Page number.</param>
        /// <param name="pageSize">Size of page.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Strong typed page of data.</returns>
        public async Task<IEnumerable<T>> GetPagedItemsAsync(int pageIndex, int pageSize, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (_isFirstCall)
            {
                var result = await _paginatedArray.FirstAsync();

                return ProcessResult(result);
            }
            else
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    return null;
                }

                if (_paginatedArray.HasNext && (pageIndex < _maxPages))
                {
                    var result = await _paginatedArray.NextAsync();
                    return ProcessResult(result);
                }
                else
                {
                    return null;
                }
            }
        }

        private IEnumerable<T> ProcessResult(FBResult result)
        {
            List<T> items = new List<T>();

            if (result.Succeeded)
            {
                IReadOnlyList<object> processedResults = (IReadOnlyList<object>)result.Object;

                foreach (T processedResult in processedResults)
                {
                    items.Add(processedResult);
                }

                _isFirstCall = false;
                return items;
            }

            throw new Exception(result.ErrorInfo?.Message);
        }
    }
}
