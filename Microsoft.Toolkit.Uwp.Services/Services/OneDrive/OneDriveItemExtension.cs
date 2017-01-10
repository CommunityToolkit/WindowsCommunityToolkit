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

using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.OneDrive.Sdk;
using Newtonsoft.Json;
using System.Threading;
using System.Text;

namespace Microsoft.Toolkit.Uwp.Services.OneDrive
{
    /// <summary>
    /// Type OneDriveItemExtension
    /// </summary>
    public static class OneDriveItemExtension
    {
        /// <summary>
        /// Send an httpRequest to get an Onedrive Item
        /// </summary>
        /// <param name="provider">OneDriveClient Provider</param>
        /// <param name="request">Http Request to execute</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> for the request.</param>
        /// <returns>a OneDrive item or null if the request fail</returns>
        public static async Task<Item> SendAuthenticatedRequestAsync(this IOneDriveClient provider, HttpRequestMessage request, CancellationToken cancellationToken )
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
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> for the request.</param>
        /// <returns>a OneDrive item or null if the request fail</returns>
        public static async Task<bool> MoveOrCopyAuthenticatedRequestAsync(this IOneDriveClient provider, HttpRequestMessage request, OneDriveStorageFolder destinationFolder, string desiredNewName, CancellationToken cancellationToken)
        {
            OneDriveParentReference rootParentReference = new OneDriveParentReference();
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
    }
}
