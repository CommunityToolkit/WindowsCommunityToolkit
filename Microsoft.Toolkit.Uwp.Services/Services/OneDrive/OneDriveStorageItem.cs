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
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Graph;
using Microsoft.OneDrive.Sdk;
using Newtonsoft.Json;
using Windows.Storage;
using Windows.Storage.Streams;

namespace Microsoft.Toolkit.Uwp.Services.OneDrive
{
    /// <summary>
    /// OneDrive Helper class.
    /// </summary>
    public class OneDriveStorageItem
    {
        private IRandomAccessStream _thumbNail;

        /// <summary>
        /// Gets the thumbnail's item.
        /// </summary>
        public IRandomAccessStream ThumbNail
        {
            get
            {
                return _thumbNail;
            }
        }

        private DateTimeOffset? _dateCreated;

        /// <summary>
        /// Gets the date and time that the current OneDrive item was created.
        /// </summary>
        public DateTimeOffset? DateCreated
        {
            get
            {
                return _dateCreated;
            }
        }

        private DateTimeOffset? _dateModified;

        /// <summary>
        /// Gets the date and time that the current OneDrive item was last modified.
        /// </summary>
        public DateTimeOffset? DateModified
        {
            get { return _dateModified; }
        }

        private string _displayName;

        /// <summary>
        /// Gets the user-friendly name of the current folder.
        /// </summary>
        public string DisplayName
        {
            get
            {
                return _displayName;
            }
        }

        private string _displayType;

        /// <summary>
        /// Gets The user-friendly type of the item.
        /// </summary>
        public string DisplayType
        {
            get
            {
                return _displayType;
            }
        }

        private string _folderId;

        /// <summary>
        /// Gets the id of the current OneDrive Item.
        /// </summary>
        public string FolderRelativeId
        {
            get
            {
                return _folderId;
            }
        }

        private string _name;

        /// <summary>
        /// Gets the name of the current OneDrive Item.
        /// </summary>
        public string Name
        {
            get
            {
                return _name;
            }
        }

        private string _path;

        /// <summary>
        /// Gets the path of the current item if the path is available
        /// </summary>
        public string Path
        {
            get
            {
                return _path;
            }
        }

        /// <summary>
        /// Store a reference to an instance of the underlying data provider.
        /// </summary>
        private IOneDriveClient _oneDriveProvider;

        /// <summary>
        /// Store a reference to an instance of current request builder
        /// </summary>
        private IItemRequestBuilder _requestBuilder;

        /// <summary>
        /// Gets an IItemRequestBuilder instance
        /// </summary>
        internal IItemRequestBuilder RequestBuilder
        {
            get
            {
                return _requestBuilder;
            }
        }

        /// <summary>
        /// Gets or sets GraphServiceClient instance
        /// </summary>
        internal IOneDriveClient Provider
        {
            get { return _oneDriveProvider; }
            set { _oneDriveProvider = value; }
        }

        private Item _oneDriveItem;

        /// <summary>
        /// Gets an instance of a DriveItem
        /// </summary>
        public Item OneDriveItem
        {
            get { return _oneDriveItem; }
        }

        /// <summary>
        ///  Initializes a new instance of the <see cref="OneDriveStorageItem"/> class.
        /// </summary>
        /// <param name="oneDriveProvider">Instance of OneDriveClient class</param>
        /// <param name="requestBuilder">Http request builder.</param>
        /// <param name="oneDriveItem">OneDrive's item</param>
        public OneDriveStorageItem(IOneDriveClient oneDriveProvider, IItemRequestBuilder requestBuilder, Item oneDriveItem)
        {
            _requestBuilder = requestBuilder;
            _oneDriveProvider = oneDriveProvider;
            _oneDriveItem = oneDriveItem;
            _name = oneDriveItem.Name;
            _dateCreated = oneDriveItem.CreatedDateTime;
            _dateModified = oneDriveItem.LastModifiedDateTime;
            _displayName = _name;
            _folderId = oneDriveItem.Id;
            if (IsFile())
            {
                _displayType = "File";
            }
            else if (IsFolder())
            {
                _displayType = "Folder";
            }
            else
            {
                _displayType = "OneNote";
            }

            // ParentReference null means is root
            if(oneDriveItem.ParentReference?.Path != null)
            {
                _path = oneDriveItem.ParentReference.Path.Replace("/drive/root:", string.Empty);
            }
        }

        /// <summary>
        /// Deletes the current item.
        /// </summary>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> for the request.</param>
        /// <returns> No object or value is returned by this method when it completes.</returns>
        public async Task DeleteAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            if (_name == "root")
            {
                throw new Microsoft.Graph.ServiceException(new Error { Message = "Could not delete the root folder" });
            }

            await RequestBuilder.Request().DeleteAsync(cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Retrieves a thumbnail set for the file
        /// </summary>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> for the request.</param>
        /// <returns>When this method completes, return a thumbnail set, or null if no thumbnail are available</returns>
        public async Task<OneDriveThumbnailSet> GetThumbnailSetAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            return await RequestBuilder.GetThumbnailSetAsync(cancellationToken);
        }

        /// <summary>
        /// Retrieves a thumbnail image for the file
        /// </summary>
        /// <param name="optionSize"> A value from the enumeration that specifies the size of the image to retrieve. Small ,Medium, Large</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> for the request.</param>
        /// <returns>When this method completes, return a stream containing the thumbnail, or null if no thumbnail are available</returns>
        public async Task<IRandomAccessStream> GetThumbnailAsync(OneDriveEnums.ThumbnailSize optionSize = OneDriveEnums.ThumbnailSize.Small, CancellationToken cancellationToken = default(CancellationToken))
        {
            var thumbnailStream = await RequestBuilder.GetThumbnailAsync(Provider, cancellationToken, optionSize);
            if (thumbnailStream == null)
            {
                return null;
            }

            _thumbNail = thumbnailStream.AsRandomAccessStream();
            return _thumbNail;
        }

        /// <summary>
        ///  Determines whether the current IStorageItem matches the specified StorageItemTypes value.
        /// </summary>
        /// <param name="type">The value to match against.</param>
        /// <returns>True if the IStorageItem matches the specified value; otherwise false.</returns>
        public bool IsOfType(StorageItemTypes type)
        {
            if (IsFolder() && type == StorageItemTypes.Folder)
            {
                return true;
            }

            if (IsFile() && type == StorageItemTypes.File)
            {
                return true;
            }

            if (IsOneNote() && type == StorageItemTypes.None)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Renames the current folder.
        /// </summary>
        /// <param name="desiredName">The desired, new name for the current folder.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> for the request.</param>
        /// <returns>When this method completes successfully, it returns an OneDriveStorageItem that represents the specified folder.</returns>
        public async Task<OneDriveStorageItem> RenameAsync(string desiredName, CancellationToken cancellationToken = default(CancellationToken))
        {
            Item newOneDriveItem = new Item();
            newOneDriveItem.Name = desiredName;
            newOneDriveItem.Description = "Item Renamed from UWP Toolkit";

            var itemRenamed = await RequestBuilder.Request().UpdateAsync(newOneDriveItem, cancellationToken).ConfigureAwait(false);
            return new OneDriveStorageItem(_oneDriveProvider, RequestBuilder, newOneDriveItem);
        }

        /// <summary>
        /// Moves the current item to the specified folder and renames the item according to the desired name.
        /// </summary>
        /// <param name="destinationFolder">The destination folder where the item is moved.</param>
        /// <param name="desiredNewName">The desired name of the item after it is moved.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> for the request.</param>
        /// <returns>return success or failure</returns>
        public async Task<bool> MoveAsync(OneDriveStorageFolder destinationFolder, string desiredNewName = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (destinationFolder == null)
            {
                throw new ArgumentNullException(nameof(destinationFolder));
            }

            if (OneDriveItem.Name == "root")
            {
                throw new Microsoft.Graph.ServiceException(new Error { Message = "Could not move the root folder" });
            }

            var requestUri = RequestBuilder.RequestUrl;
            HttpMethod patchMethod = new HttpMethod("PATCH");
            HttpRequestMessage request = new HttpRequestMessage(patchMethod, requestUri);

            if (string.IsNullOrEmpty(desiredNewName))
            {
                desiredNewName = OneDriveItem.Name;
            }

            return await Provider.MoveAsync(request, destinationFolder, desiredNewName, cancellationToken);
        }

        /// <summary>
        /// Copy the current item to the specified folder and renames the item according to the desired name.
        /// </summary>
        /// <param name="destinationFolder">The destination folder where the item is moved.</param>
        /// <param name="desiredNewName">The desired name of the item after it is moved.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> for the request.</param>
        /// <returns>return success or failure</returns>
        public async Task<bool> CopyAsync(OneDriveStorageFolder destinationFolder, string desiredNewName = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (destinationFolder == null)
            {
                throw new ArgumentNullException(nameof(destinationFolder));
            }

            if (OneDriveItem.Name == "root")
            {
                throw new Microsoft.Graph.ServiceException(new Error { Message = "Could not copy the root folder" });
            }

            if (string.IsNullOrEmpty(desiredNewName))
            {
                desiredNewName = this.OneDriveItem.Name;
            }

            OneDriveParentReference parentReference = new OneDriveParentReference();
            if (destinationFolder.OneDriveItem.Name == "root")
            {
                parentReference.Parent.Path = "/drive/root:/";
            }
            else
            {
                parentReference.Parent.Path = destinationFolder.OneDriveItem.ParentReference.Path + $"/{destinationFolder.OneDriveItem.Name}";
            }

            parentReference.Name = desiredNewName;

            var copyRequest = Provider.Drive.Items[OneDriveItem.Id].Copy(desiredNewName);

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, copyRequest.Request().RequestUrl);
            request.Headers.Add("Prefer", "respond-async");
            var content = JsonConvert.SerializeObject(parentReference);
            request.Content = new StringContent(content, Encoding.UTF8, "application/json");
            await Provider.AuthenticationProvider.AuthenticateRequestAsync(request).ConfigureAwait(false);
            var response = await Provider.HttpProvider.SendAsync(request, HttpCompletionOption.ResponseContentRead, cancellationToken).ConfigureAwait(false);
            return response.IsSuccessStatusCode;
        }

        /// <summary>
        /// Check if the item is a folder
        /// </summary>
        /// <returns>Return true if it's a folder</returns>
        public bool IsFolder()
        {
            return OneDriveItem.Folder != null ? true : false;
        }

        /// <summary>
        /// Check if the item is a file
        /// </summary>
        /// <returns>Return true if it's a file</returns>
        public bool IsFile()
        {
            return OneDriveItem.File != null ? true : false;
        }

        /// <summary>
        /// Check if the item is a OneNote focument
        /// </summary>
        /// <returns>Return true if it's a OneNote document</returns>
        public bool IsOneNote()
        {
            return !IsFile() && !IsFolder() ? true : false;
        }

        /// <summary>
        /// Initialize a OneDriveStorageFolder
        /// </summary>
        /// <param name="oneDriveItem">A OneDrive item</param>
        /// <returns>New instance of OneDriveStorageFolder</returns>
        protected OneDriveStorageFolder InitializeOneDriveStorageFolder(Item oneDriveItem)
        {
            var requestBuilder = Provider.Drive.Items[oneDriveItem.Id];
            return new OneDriveStorageFolder(Provider, requestBuilder, oneDriveItem);
        }

        /// <summary>
        /// Initialize a OneDriveStorageItem
        /// </summary>
        /// <param name="oneDriveItem">A OneDrive item</param>
        /// <returns>New instance of OneDriveStorageItem</returns>
        protected OneDriveStorageItem InitializeOneDriveStorageItem(Item oneDriveItem)
        {
            var requestBuilder = Provider.Drive.Items[oneDriveItem.Id];
            return new OneDriveStorageItem(Provider, requestBuilder, oneDriveItem);
        }

        /// <summary>
        /// Initialize a OneDriveStorageItem
        /// </summary>
        /// <param name="oneDriveItem">A OneDrive item</param>
        /// <returns>New instance of OneDriveStorageItem</returns>
        protected OneDriveStorageFile InitializeOneDriveStorageFile(Item oneDriveItem)
        {
            var requestBuilder = Provider.Drive.Items[oneDriveItem.Id];
            return new OneDriveStorageFile(Provider, requestBuilder, oneDriveItem);
        }
    }
}
