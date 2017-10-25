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
  using System.Threading;
  using System.Threading.Tasks;
  using Microsoft.Graph;
  using Microsoft.OneDrive.Sdk;
  using static Microsoft.Toolkit.Uwp.Services.OneDrive.OneDriveEnums;

namespace Microsoft.Toolkit.Uwp.Services.OneDrive
{
    /// <summary>
    /// Type to handle paged requests to OneDrive.
    /// </summary>
    /// <typeparam name="T">Strong type to return.</typeparam>
    public class OneDriveRequestSource<T> : Collections.IIncrementalSource<T>
    {
        private IBaseClient _provider;
        private IBaseRequestBuilder _requestBuilder;
        private IItemChildrenCollectionRequest _nextPage = null;
        private IDriveItemChildrenCollectionRequest _nextPageGraph = null;
        private OrderBy _orderBy;
        private string _filter;
        private bool _isFirstCall = true;
        private bool _useOneDriveSdk = true;

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
        public async Task<IEnumerable<T>> GetPagedItemsAsync(int pageIndex, int pageSize, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (_requestBuilder.GetType() == typeof(ItemRequestBuilder))
            {
                return await GetPageOneDriveSdkAsync(pageIndex, pageSize, cancellationToken);
            }

            return await GetPageGraphSdkAsync(pageIndex, pageSize, cancellationToken);
        }

        private async Task<IEnumerable<T>> GetPageOneDriveSdkAsync(int pageIndex, int pageSize, CancellationToken cancellationToken = default(CancellationToken))
        {
            // First Call
            if (_isFirstCall)
            {
                _nextPage = _requestBuilder.CreateChildrenRequest(pageSize, _orderBy, _filter);

                _isFirstCall = false;
            }

            if (cancellationToken.IsCancellationRequested)
            {
                return null;
            }

            if (_nextPage != null)
            {
                var oneDriveItems = await _nextPage.GetAsync(cancellationToken);
                _nextPage = oneDriveItems.NextPageRequest;
                return ProcessResultOneDriveSdk(oneDriveItems);
            }

            // no more data
            return null;
        }

        private async Task<IEnumerable<T>> GetPageGraphSdkAsync(int pageIndex, int pageSize, CancellationToken cancellationToken = default(CancellationToken))
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

        private IEnumerable<T> ProcessResultOneDriveSdk(IItemChildrenCollectionPage oneDriveItems)
        {
            List<T> items = new List<T>(oneDriveItems.Count);

            foreach (var oneDriveItem in oneDriveItems)
            {
                T item = (T)CreateItemOneDriveSdk(oneDriveItem);
                items.Add(item);
            }

            return items;
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

        private object CreateItemOneDriveSdk(Item oneDriveItem)
        {
            IBaseRequestBuilder requestBuilder = (IBaseRequestBuilder)((IOneDriveClient)_provider).Drive.Items[oneDriveItem.Id];

            if (oneDriveItem.Folder != null)
            {
                return new OneDriveStorageFolder(_provider, requestBuilder, oneDriveItem.CopyToDriveItem());
            }

            if (oneDriveItem.File != null)
            {
                return new OneDriveStorageFile(_provider, requestBuilder, oneDriveItem.CopyToDriveItem());
            }

            return new OneDriveStorageItem(_provider, requestBuilder, oneDriveItem.CopyToDriveItem());
        }

        private object CreateItemGraphSdk(DriveItem oneDriveItem)
        {
            IBaseRequestBuilder requestBuilder = (IBaseRequestBuilder)((IGraphServiceClient)_provider).Drive.Items[oneDriveItem.Id];

            if (oneDriveItem.Folder != null)
            {
                return new GraphOneDriveStorageFolder(_provider, requestBuilder, oneDriveItem);
            }

            if (oneDriveItem.File != null)
            {
                return new GraphOneDriveStorageFile(_provider, requestBuilder, oneDriveItem);
            }

            return new GraphOneDriveStorageItem(_provider, requestBuilder, oneDriveItem);
        }
    }
}
