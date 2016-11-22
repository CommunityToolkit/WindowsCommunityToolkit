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

using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Graph;
using Newtonsoft.Json;
using Windows.Storage.Streams;
using static Microsoft.Toolkit.Uwp.Services.MicrosoftGraph.MicrosoftGraphEnums;

namespace Microsoft.Toolkit.Uwp.Services.MicrosoftGraph
{
    /// <summary>
    ///  Class of Microsoft Graph API Extension
    /// </summary>
    public static class OneDriveItemExtension
    {
        /// <summary>
        /// Is the DriveItem is type of file
        /// </summary>
        /// <param name="onedriveItem">An instance of OneDriveStorageItem</param>
        /// <returns>True if it's a file otherwise false</returns>
        public static bool IsFile(this OneDriveStorageItem onedriveItem)
        {
            return onedriveItem.OneDriveItem.File != null ? true : false;
        }

        /// <summary>
        /// Is the DriveItem is type of folder
        /// </summary>
        /// <param name="onedriveItem">An instance of OneDriveStorageItem</param>
        /// <returns>True if it's a folder otherwise false</returns>
        public static bool IsFolder(this OneDriveStorageItem onedriveItem)
        {
            return onedriveItem.OneDriveItem.Folder != null ? true : false;
        }

        /// <summary>
        /// Creates a copy of the item in the specified folder and renames the item according to the desired name.
        /// </summary>
        /// <param name="builder">Http request builder</param>
        /// <param name="provider">Http provider to execute the request</param>
        /// <param name="destinationFolder">The folder where the item will be created</param>
        /// <param name="desiredNewName">The new name for the copy of the item created in the destination Folder</param>
        /// <returns>return success or failure</returns>
        public static Task<bool> CopyAsync(this IDriveItemRequestBuilder builder, GraphServiceClient provider, OneDriveStorageFolder destinationFolder, string desiredNewName)
        {
            var requestUrl = builder.RequestUrl + "/copy";
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, requestUrl);
            request.Headers.Add("Prefer", "respond-async");
            return ExecuteMoveOrCopyRequestAsync(request, provider, destinationFolder, desiredNewName);
        }

        /// <summary>
        /// Moves the current item to the specified folder and renames the item according to the desired name.
        /// </summary>
        /// <param name="builder">Http request builder</param>
        /// <param name="provider">Http provider to execute the request</param>
        /// <param name="destinationFolder">The folder where the item will be created</param>
        /// <param name="desiredNewName">The new name for the copy of the item created in the destination Folder</param>
        /// <returns>return success or failure</returns>
        public static Task<bool> MoveAsync(this IDriveItemRequestBuilder builder, GraphServiceClient provider, OneDriveStorageFolder destinationFolder, string desiredNewName)
        {
            var requestUrl = builder.RequestUrl;
            HttpMethod patchMethod = new HttpMethod("PATCH");
            HttpRequestMessage request = new HttpRequestMessage(patchMethod, requestUrl);
            return ExecuteMoveOrCopyRequestAsync(request, provider, destinationFolder, desiredNewName);
        }

        /// <summary>
        /// Creates a new file in the current folder. This method also specifies what to
        /// do if a file with the same name already exists in the current folder.
        /// </summary>
        /// <param name="builder">Http request builder</param>
        /// <param name="provider">Http provider to execute the request</param>
        /// <param name="desiredName">The name of the new file to create in the current folder.</param>
        /// <param name="content">The data's stream to push into the file</param>
        /// <remarks>With OneDrive Consumer, the content could not be null</remarks>
        /// <param name="option">
        ///  One of the enumeration values that determines how to handle the collision if
        ///  a file with the specified desiredNewName already exists in the destination folder.
        ///  Default : fail</param>
        /// <returns>When this method completes, it returns a MicrosoftGraphOneDriveFile that represents the new file.</returns>
        public static async Task<DriveItem> CreateFileAsync(this IDriveItemRequestBuilder builder, GraphServiceClient provider, string desiredName, IRandomAccessStream content, OneDriveItemNameCollisionOption option)
        {
            var streamContent = content.AsStreamForRead();
            var encodeDesiredName = Uri.EscapeDataString(desiredName);
            string url = builder.Children.Request().RequestUrl + $"/{encodeDesiredName}/content?@name.conflictBehavior={option.ToString().ToLower()}";

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Put, url);

            request.Content = new StreamContent(streamContent);

            await provider.Me.Client.AuthenticationProvider.AuthenticateRequestAsync(request).ConfigureAwait(false);

            var response = await provider.HttpProvider.SendAsync(request).ConfigureAwait(false);

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            string jsonData = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<DriveItem>(jsonData);
        }

        /// <summary>
        /// Asynchronously writes a Stream to the current file
        /// </summary>
        /// <param name="builder">Http request builder</param>
        /// <param name="content">The stream to write data from.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> for the request.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public static async Task WriteAsync(this IDriveItemRequestBuilder builder, IRandomAccessStream content, CancellationToken cancellationToken)
        {
            var streamContent = content.AsStreamForRead();
            await builder.Content.Request().PutAsync<DriveItem>(streamContent, cancellationToken, HttpCompletionOption.ResponseContentRead);
        }

        /// <summary>
        /// Opens stream with the specified options over the specified file.
        /// </summary>
        /// <param name="builder">Http request builder</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> for the request.</param>
        /// <returns>When this method completes, it returns a Stream.</returns>
        public static Task<Stream> OpenAsync(this IDriveItemRequestBuilder builder, CancellationToken cancellationToken)
        {
            return builder.Content.Request().GetAsync(cancellationToken);
        }

        /// <summary>
        /// Gets a file's thumnnail
        /// </summary>
        /// <param name="builder">Http request builder</param>
        /// <param name="provider">Http provider to execute the request</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> for the request.</param>
        /// <param name="optionSize"> A value from the enumeration that specifies the size of the image to retrieve. Small ,Medium, Large</param>
        /// <returns>When this method completes, return a stream containing the thumbnail, or null if no thumbnail are available</returns>
        public static async Task<Stream> GetThumbnailAsync(this IDriveItemRequestBuilder builder, GraphServiceClient provider, CancellationToken cancellationToken, ThumbnailSize optionSize)
        {
            // Requests the differente size of the thumbnail
            var requestThumbnail = await builder.Thumbnails.Request().GetAsync(cancellationToken).ConfigureAwait(false);
            if (requestThumbnail.Count == 1)
            {
                var thumbnailInfos = requestThumbnail.CurrentPage[0];
                string requestUrl = null;

                if (optionSize == ThumbnailSize.Small)
                {
                    requestUrl = thumbnailInfos.Small.Url;
                }
                else if (optionSize == ThumbnailSize.Medium)
                {
                    requestUrl = thumbnailInfos.Medium.Url;
                }
                else if (optionSize == ThumbnailSize.Large)
                {
                    requestUrl = thumbnailInfos.Large.Url;
                }

                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, requestUrl);
                await provider.AuthenticationProvider.AuthenticateRequestAsync(request).ConfigureAwait(false);
                var response = await provider.HttpProvider.SendAsync(request).ConfigureAwait(false);
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsStreamAsync();
                }
            }

            // No thumbNail
            return null;
        }

        /// <summary>
        /// Renames the current item.
        /// </summary>
        /// <param name="builder">Http request builder</param>
        /// <param name="oneDriveItem">The item to rename</param>
        /// <param name="desiredNewName">The desired, new name for the current item.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> for the request.</param>
        /// <returns>When this method completes successfully, it returns a DriveItem.</returns>
        public static async Task<DriveItem> RenameAsync(this IDriveItemRequestBuilder builder, DriveItem oneDriveItem, string desiredNewName, CancellationToken cancellationToken)
        {
            DriveItem newOneDriveItem = new DriveItem();
            newOneDriveItem.Name = Uri.EscapeDataString(desiredNewName);
            newOneDriveItem.Description = "Rename file by UWP Toolkit";

            var itemRenamed = await builder.Request().UpdateAsync(newOneDriveItem, cancellationToken).ConfigureAwait(false);
            return itemRenamed;
        }

        /// <summary>
        /// Create an upload session
        /// </summary>
        /// <param name="folder">The folder where the item will be created</param>
        /// <param name="provider">Http provider to execute the request</param>
        /// <param name="desiredName">Name of the new file to create in the current folder.</param>
        /// <param name="option">
        ///  One of the enumeration values that determines how to handle the collision if
        ///  a file with the specified desiredNewName already exists in the destination folder.</param>
        /// <returns>Return an UploadSession</returns>
        internal static async Task<OneDriveUploadSession> CreateUploadSessionAsync(this OneDriveStorageFolder folder, GraphServiceClient provider, string desiredName, OneDriveItemNameCollisionOption option)
        {
            var encodeDesiredName = Uri.EscapeDataString(desiredName);
            var uploadSessionUri = $"https://graph.microsoft.com/beta/me/drive/items/{folder.OneDriveItem.Id}:/{encodeDesiredName}:/createUploadSession";
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, uploadSessionUri);
            var conflictBehavior = new OneDriveItemConflictBehavior { Item = new Item { MicrosoftGraphConflictBehavior = option.ToString().ToLower() } };
            var jsonConflictBehavior = JsonConvert.SerializeObject(conflictBehavior);
            request.Content = new StringContent(jsonConflictBehavior, Encoding.UTF8, "application/json");

            await provider.Me.Client.AuthenticationProvider.AuthenticateRequestAsync(request).ConfigureAwait(false);

            var response = await provider.Me.Client.HttpProvider.SendAsync(request).ConfigureAwait(false);
            if (response.IsSuccessStatusCode)
            {
                var jsonData = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                return JsonConvert.DeserializeObject<OneDriveUploadSession>(jsonData);
            }

            throw new ServiceException(new Error { Message = "Could not create an UploadSession", Code = "NoUploadSession", ThrowSite = "UWP Community Toolkit" });
        }

        /// <summary>
        /// Delete an upload session
        /// </summary>
        /// <param name="folder">The folder where the item will be created</param>
        /// <param name="provider">Http provider to execute the request</param>
        /// <param name="uploadSession">The upload session</param>
        internal static async Task DeleteSession(this OneDriveStorageFolder folder, GraphServiceClient provider, OneDriveUploadSession uploadSession)
        {
            if (uploadSession == null)
            {
                return;
            }

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Delete, uploadSession.UploadUrl);
            await provider.Me.Client.HttpProvider.SendAsync(request);
        }

        /// <summary>
        /// Private method
        /// </summary>
        /// <param name="request">Http request</param>
        /// <param name="provider">Http provider to execute the request</param>
        /// <param name="destinationFolder">The folder where the item will be Moved Or Copied</param>
        /// <param name="desiredNewName">The desired, new name for the current item.</param>
        /// <returns>Success or failure</returns>
        private static async Task<bool> ExecuteMoveOrCopyRequestAsync(HttpRequestMessage request, GraphServiceClient provider, OneDriveStorageFolder destinationFolder, string desiredNewName)
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

            await provider.Me.Client.AuthenticationProvider.AuthenticateRequestAsync(request).ConfigureAwait(false);

            HttpResponseMessage response = null;

            response = await provider.Me.Client.HttpProvider.SendAsync(request).ConfigureAwait(false);

            return response.IsSuccessStatusCode;
        }
    }
}
