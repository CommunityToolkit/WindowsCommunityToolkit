﻿// ******************************************************************
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
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.OneDrive.Sdk;
using Newtonsoft.Json;
using static Microsoft.Toolkit.Uwp.Services.OneDrive.OneDriveEnums;

namespace Microsoft.Toolkit.Uwp.Services.OneDrive
{
    /// <summary>
    /// Type OneDriveItemExtension
    /// </summary>
    [Obsolete("This class is being deprecated.")]
    public static class OneDriveItemExtension
    {
        /// <summary>
        /// Gets a file's thumbnail set
        /// </summary>
        /// <param name="builder">Http request builder</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> for the request.</param>
        /// <returns>When this method completes, return a thumbnail set, or null if no thumbnail are available</returns>
        public static async Task<OneDriveThumbnailSet> GetThumbnailSetAsync(this IItemRequestBuilder builder, CancellationToken cancellationToken)
        {
            // Requests the differente size of the thumbnail
            var requestThumbnail = await builder.Thumbnails.Request().GetAsync(cancellationToken).ConfigureAwait(false);

            var thumbnailSet = requestThumbnail.FirstOrDefault();

            if (thumbnailSet == null)
            {
                return null;
            }

            return new OneDriveThumbnailSet(thumbnailSet);
        }

        /// <summary>
        /// Gets a file's thumbnail
        /// </summary>
        /// <param name="builder">Http request builder</param>
        /// <param name="provider">Http provider to execute the request</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> for the request.</param>
        /// <param name="optionSize"> A value from the enumeration that specifies the size of the image to retrieve. Small ,Medium, Large</param>
        /// <returns>When this method completes, return a stream containing the thumbnail, or null if no thumbnail are available</returns>
        public static async Task<Stream> GetThumbnailAsync(this IItemRequestBuilder builder, IOneDriveClient provider, CancellationToken cancellationToken, ThumbnailSize optionSize)
        {
            // Requests the different sizes of the thumbnail
            var thumbnailSet = await builder.GetThumbnailSetAsync(cancellationToken).ConfigureAwait(false);

            if (thumbnailSet == null)
            {
                return null;
            }

            string requestUrl = null;

            if (optionSize == ThumbnailSize.Small)
            {
                requestUrl = thumbnailSet.Small;
            }
            else if (optionSize == ThumbnailSize.Medium)
            {
                requestUrl = thumbnailSet.Medium;
            }
            else if (optionSize == ThumbnailSize.Large)
            {
                requestUrl = thumbnailSet.Large;
            }

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, requestUrl);
            await provider.AuthenticationProvider.AuthenticateRequestAsync(request).ConfigureAwait(false);
            var response = await provider.HttpProvider.SendAsync(request).ConfigureAwait(false);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStreamAsync();
            }

            return null;
        }

        /// <summary>
        /// Send an httpRequest to get an Onedrive Item
        /// </summary>
        /// <param name="provider">OneDriveClient Provider</param>
        /// <param name="request">Http Request to execute</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> for the request.</param>
        /// <returns>a OneDrive item or null if the request fail</returns>
        public static async Task<Item> SendAuthenticatedRequestAsync(this IOneDriveClient provider, HttpRequestMessage request, CancellationToken cancellationToken)
        {
            Item oneDriveItem = null;
            await provider.AuthenticationProvider.AuthenticateRequestAsync(request).ConfigureAwait(false);
            var response = await provider.HttpProvider.SendAsync(request, HttpCompletionOption.ResponseContentRead, cancellationToken).ConfigureAwait(false);
            if (response.IsSuccessStatusCode)
            {
                var jsonData = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                oneDriveItem = JsonConvert.DeserializeObject<Item>(jsonData);
            }

            return oneDriveItem;
        }

        /// <summary>
        /// Send an httpRequest to get an Onedrive Item
        /// </summary>
        /// <param name="provider">OneDriveClient Provider</param>
        /// <param name="request">Http Request to execute</param>
        /// <param name="destinationFolder">Destination folder</param>
        /// <param name="desiredNewName">New name</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> for the request.</param>
        /// <returns>a OneDrive item or null if the request fail</returns>
        public static async Task<bool> MoveAsync(this IOneDriveClient provider, HttpRequestMessage request, OneDriveStorageFolder destinationFolder, string desiredNewName, CancellationToken cancellationToken)
        {
            Toolkit.Services.OneDrive.OneDriveParentReference rootParentReference = new Toolkit.Services.OneDrive.OneDriveParentReference();
            if (destinationFolder.OneDriveItem.Name == "root")
            {
                rootParentReference.Parent.Path = "/drive/root:/";
            }
            else
            {
                rootParentReference.Parent.Path = destinationFolder.OneDriveItem.ParentReference.Path + $"/{destinationFolder.OneDriveItem.Name}";
            }

            rootParentReference.Name = desiredNewName;

            var content = JsonConvert.SerializeObject(rootParentReference);
            request.Content = new StringContent(content, Encoding.UTF8, "application/json");
            await provider.AuthenticationProvider.AuthenticateRequestAsync(request).ConfigureAwait(false);
            var response = await provider.HttpProvider.SendAsync(request, HttpCompletionOption.ResponseContentRead, cancellationToken).ConfigureAwait(false);

            return response.IsSuccessStatusCode;
        }

        /// <summary>
        /// Create children http request with specific options
        /// </summary>
        /// <param name="requestBuilder">request builder</param>
        /// <param name="top">The number of items to return in a result set.</param>
        /// <param name="orderBy">Sort the order of items in the response collection</param>
        /// <param name="filter">Filters the response based on a set of criteria.</param>
        /// <returns>Returns the http request</returns>
        public static IItemChildrenCollectionRequest CreateChildrenRequest(this IItemRequestBuilder requestBuilder, int top, OrderBy orderBy = OrderBy.None, string filter = null)
        {
            IItemChildrenCollectionRequest oneDriveitemsRequest = null;
            if (orderBy == OrderBy.None && string.IsNullOrEmpty(filter))
            {
                return requestBuilder.Children.Request().Top(top);
            }

            if (orderBy == OrderBy.None)
            {
                return requestBuilder.Children.Request().Top(top).Filter(filter);
            }

            string order = OneDriveHelper.TransformOrderByToODataString(orderBy);

            if (string.IsNullOrEmpty(filter))
            {
                oneDriveitemsRequest = requestBuilder.Children.Request().Top(top).OrderBy(order);
            }
            else
            {
                oneDriveitemsRequest = requestBuilder.Children.Request().Top(top).OrderBy(order).Filter(filter);
            }

            return oneDriveitemsRequest;
        }
    }
}
