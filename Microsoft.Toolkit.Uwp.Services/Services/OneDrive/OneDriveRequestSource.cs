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
        private IOneDriveClient _provider;
        private IItemRequestBuilder _requestBuilder;
        private IItemChildrenCollectionRequest _nextPage = null;
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
        public OneDriveRequestSource(IOneDriveClient provider, IItemRequestBuilder requestBuilder, OrderBy orderBy, string filter)
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
                return ProcessResult(oneDriveItems);
            }

            // no more data
            return null;
        }

        private IEnumerable<T> ProcessResult(IItemChildrenCollectionPage oneDriveItems)
        {
            List<T> items = new List<T>(oneDriveItems.Count);

            foreach (var oneDriveItem in oneDriveItems)
            {
                T item = (T)CreateItem(oneDriveItem);
                items.Add(item);
            }

            return items;
        }

        private object CreateItem(Item oneDriveItem)
        {
            var requestBuilder = _provider.Drive.Items[oneDriveItem.Id];

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
