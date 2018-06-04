// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Graph;
using static Microsoft.Toolkit.Services.MicrosoftGraph.MicrosoftGraphEnums;

namespace Microsoft.Toolkit.Services.OneDrive
{
    /// <summary>
    /// Type to handle paged requests to OneDrive.
    /// </summary>
    /// <typeparam name="T">Strong type to return.</typeparam>
    public class OneDriveRequestSource<T> : Collections.IIncrementalSource<T>
    {
        private IBaseClient _provider;
        private IBaseRequestBuilder _requestBuilder;
        private IDriveItemChildrenCollectionRequest _nextPageGraph = null;
        private OrderBy _orderBy;
        private string _filter;
        private bool _isFirstCall = true;

        /// <summary>
        /// Initializes a new instance of the <see cref="OneDriveRequestSource{T}"/> class.
        /// </summary>
        public OneDriveRequestSource()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OneDriveRequestSource{T}"/> class.
        /// </summary>
        /// <param name="provider">OneDrive Data Client Provider</param>
        /// <param name="requestBuilder">Http request to execute</param>
        /// <param name="orderBy">Sort the order of items in the response collection</param>
        /// <param name="filter">Filters the response based on a set of criteria.</param>
        public OneDriveRequestSource(IBaseClient provider, IBaseRequestBuilder requestBuilder, OrderBy orderBy, string filter)
        {
            _provider = provider;
            _requestBuilder = requestBuilder;
            _orderBy = orderBy;
            _filter = filter;
        }

        /// <summary>
        /// Returns strong typed page of data.
        /// </summary>
        /// <param name="pageIndex">Page number.</param>
        /// <param name="pageSize">Size of page.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Strong typed page of data.</returns>
        public Task<IEnumerable<T>> GetPagedItemsAsync(int pageIndex, int pageSize, CancellationToken cancellationToken = default(CancellationToken))
        {
            return GetPageGraphSdkAsync(pageSize, cancellationToken);
        }

        private async Task<IEnumerable<T>> GetPageGraphSdkAsync(int pageSize, CancellationToken cancellationToken = default(CancellationToken))
        {
            // First Call
            if (_isFirstCall)
            {
                _nextPageGraph = ((IDriveItemRequestBuilder)_requestBuilder).CreateChildrenRequest(pageSize, _orderBy, _filter);

                _isFirstCall = false;
            }

            if (cancellationToken.IsCancellationRequested)
            {
                return null;
            }

            if (_nextPageGraph != null)
            {
                var oneDriveItems = await _nextPageGraph.GetAsync(cancellationToken);
                _nextPageGraph = oneDriveItems.NextPageRequest;
                return ProcessResultGraphSdk(oneDriveItems);
            }

            // no more data
            return null;
        }

        private IEnumerable<T> ProcessResultGraphSdk(IDriveItemChildrenCollectionPage oneDriveItems)
        {
            List<T> items = new List<T>(oneDriveItems.Count);

            foreach (var oneDriveItem in oneDriveItems)
            {
                T item = (T)CreateItemGraphSdk(oneDriveItem);
                items.Add(item);
            }

            return items;
        }

        private object CreateItemGraphSdk(DriveItem oneDriveItem)
        {
            IBaseRequestBuilder requestBuilder = (IBaseRequestBuilder)((IGraphServiceClient)_provider).Drive.Items[oneDriveItem.Id];

            if (oneDriveItem.Folder != null)
            {
                return new OneDriveStorageFolder(_provider, requestBuilder, oneDriveItem);
            }

            if (oneDriveItem.File != null)
            {
                return new OneDriveStorageFile(_provider, requestBuilder, oneDriveItem);
            }

            return new OneDriveStorageItem(_provider, requestBuilder, oneDriveItem);
        }
    }
}
