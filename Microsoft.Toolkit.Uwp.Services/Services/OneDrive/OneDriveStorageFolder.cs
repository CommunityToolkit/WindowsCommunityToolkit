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

using Microsoft.OneDrive.Sdk;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Storage;
using Windows.Storage.FileProperties;

namespace Microsoft.Toolkit.Uwp.Services.OneDrive
{
    /// <summary>
    /// OneDrive Helper class.
    /// </summary>
    public class OneDriveStorageFolder : OneDriveStorageItem
    {
        /// <summary>
        /// Requests from OneDrive the file or folder with the specified name from the current folder.
        /// </summary>
        /// <param name="name">The name (or path relative to the current folder) of the file or folder to get.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> for the request.</param>
        /// <returns>When this method completes successfully, it returns a DriveItem that represents the specified file or folder.</returns>
        private async Task<Item> RequestChildrenAsync(string name, CancellationToken cancellationToken)
        {
            Item oneDriveItem = null;
            var requestUrl = $"{RequestBuilder.RequestUrl}:/{name}";

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, requestUrl);
            await Provider.AuthenticationProvider.AuthenticateRequestAsync(request).ConfigureAwait(false);
            var response = await Provider.HttpProvider.SendAsync(request).ConfigureAwait(false);
            if (response.IsSuccessStatusCode)
            {
                var jsonData = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                oneDriveItem = JsonConvert.DeserializeObject<Item>(jsonData);
            }

            return oneDriveItem;
        }

        public OneDriveStorageFolder(IOneDriveClient oneDriveProvider, IItemRequestBuilder requestBuilder, Item oneDriveItem)
            : base(oneDriveProvider, requestBuilder, oneDriveItem)
        {
            
        }

        public IAsyncOperation<StorageFile> CreateFileAsync(string desiredName)
        {
            throw new NotImplementedException();
        }

        public IAsyncOperation<StorageFile> CreateFileAsync(string desiredName, CreationCollisionOption options)
        {
            throw new NotImplementedException();
        }

        public IAsyncOperation<StorageFolder> CreateFolderAsync(string desiredName)
        {
            throw new NotImplementedException();
        }

        public IAsyncOperation<StorageFolder> CreateFolderAsync(string desiredName, CreationCollisionOption options)
        {
            throw new NotImplementedException();
        }

        public IAsyncOperation<BasicProperties> GetBasicPropertiesAsync()
        {
            throw new NotImplementedException();
        }

        public IAsyncOperation<StorageFile> GetFileAsync(string name)
        {
            throw new NotImplementedException();
        }

        public IAsyncOperation<IReadOnlyList<StorageFile>> GetFilesAsync()
        {
            throw new NotImplementedException();
        }


        public async Task<OneDriveStorageFolder> GetFolderAsync(string name, CancellationToken cancellationToken = default(CancellationToken))
        {
            var oneDriveItem = await RequestChildrenAsync(name, cancellationToken);
            return InitializeOneDriveFolder(oneDriveItem);
        }

        public IAsyncOperation<IReadOnlyList<StorageFolder>> GetFoldersAsync()
        {
            throw new NotImplementedException();
        }

        public IAsyncOperation<IStorageItem> GetItemAsync(string name)
        {
            throw new NotImplementedException();
        }

        public IAsyncOperation<IReadOnlyList<IStorageItem>> GetItemsAsync()
        {
            throw new NotImplementedException();
        }
    }
}
