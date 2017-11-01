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
using Newtonsoft.Json;
using static Microsoft.Toolkit.Services.MicrosoftGraph.MicrosoftGraphEnums;

namespace Microsoft.Toolkit.Services.OneDrive
{
    /// <summary>
    /// GraphOneDriveStorageItem class.
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
        private IBaseClient _oneDriveProvider;

        /// <summary>
        /// Store a reference to an instance of current request builder
        /// </summary>
        private IBaseRequestBuilder _requestBuilder;

        /// <summary>
        /// Gets an Item Request Builder instance
        /// </summary>
        public IBaseRequestBuilder RequestBuilder => _requestBuilder;

        /// <summary>
        /// Gets or sets IOneDriveServiceClient instance
        /// </summary>
        public IBaseClient Provider
        {
            get { return _oneDriveProvider; }
            set { _oneDriveProvider = value; }
        }

        private DriveItem _oneDriveItem;

        /// <summary>
        /// Gets an instance of a DriveItem
        /// </summary>
        public DriveItem OneDriveItem
        {
            get { return _oneDriveItem; }
        }

        /// <summary>
        ///  Initializes a new instance of the <see cref="OneDriveStorageItem"/> class.
        /// </summary>
        /// <param name="oneDriveProvider">Instance of Graph Client class</param>
        /// <param name="requestBuilder">Http request builder.</param>
        /// <param name="oneDriveItem">OneDrive's item</param>
        public OneDriveStorageItem(IBaseClient oneDriveProvider, IBaseRequestBuilder requestBuilder, DriveItem oneDriveItem)
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
            if (oneDriveItem.ParentReference?.Path != null)
            {
                _path = oneDriveItem.ParentReference.Path.Replace("/drive/root:", string.Empty);
            }
        }

        /// <inheritdoc/>
        public async Task DeleteAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            if (_name == "root")
            {
                throw new Microsoft.Graph.ServiceException(new Error { Message = "Could not delete the root folder" });
            }

            await ((IDriveItemRequestBuilder)RequestBuilder).Request().DeleteAsync(cancellationToken).ConfigureAwait(false);
        }

        /// <inheritdoc/>
        public async Task<OneDriveThumbnailSet> GetThumbnailSetAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            return await ((IDriveItemRequestBuilder)RequestBuilder).GetThumbnailSetAsync(cancellationToken);
        }

        /// <inheritdoc/>
        public async Task<IRandomAccessStream> GetThumbnailAsync(ThumbnailSize optionSize = ThumbnailSize.Small, CancellationToken cancellationToken = default(CancellationToken))
        {
            var thumbnailStream = await ((IDriveItemRequestBuilder)RequestBuilder).GetThumbnailAsync(Provider, cancellationToken, optionSize);
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

        /// <inheritdoc/>
        public async Task<OneDriveStorageItem> RenameAsync(string desiredName, CancellationToken cancellationToken = default(CancellationToken))
        {
            DriveItem newOneDriveItem = new DriveItem { Name = desiredName, Description = "Item Renamed from UWP Toolkit" };
            var itemRenamed = await ((IDriveItemRequestBuilder)RequestBuilder).Request().UpdateAsync(newOneDriveItem, cancellationToken).ConfigureAwait(false);
            return new OneDriveStorageItem(_oneDriveProvider, RequestBuilder, itemRenamed);
        }

        /// <inheritdoc/>
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

            return await ((IGraphServiceClient)Provider).MoveAsync(request, destinationFolder, desiredNewName, cancellationToken);
        }

        /// <inheritdoc/>
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

            var copyRequest = ((IGraphServiceClient)Provider).Drive.Items[OneDriveItem.Id].Copy(desiredNewName);

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, copyRequest.Request().RequestUrl);
            request.Headers.Add("Prefer", "respond-async");
            var content = JsonConvert.SerializeObject(parentReference);
            request.Content = new StringContent(content, Encoding.UTF8, "application/json");
            await Provider.AuthenticationProvider.AuthenticateRequestAsync(request).ConfigureAwait(false);
            var response = await Provider.HttpProvider.SendAsync(request, HttpCompletionOption.ResponseContentRead, cancellationToken).ConfigureAwait(false);
            return response.IsSuccessStatusCode;
        }

        /// <inheritdoc/>
        public bool IsFolder()
        {
            return OneDriveItem.Folder != null ? true : false;
        }

        /// <inheritdoc/>
        public bool IsFile()
        {
            return OneDriveItem.File != null ? true : false;
        }

        /// <inheritdoc/>
        public bool IsOneNote()
        {
            return !IsFile() && !IsFolder() ? true : false;
        }

        /// <summary>
        /// Initialize a GraphOneDriveStorageFolder
        /// </summary>
        /// <param name="oneDriveItem">A OneDrive item</param>
        /// <returns>New instance of GraphOneDriveStorageFolder</returns>
        protected OneDriveStorageFolder InitializeOneDriveStorageFolder(DriveItem oneDriveItem)
        {
            IBaseRequestBuilder requestBuilder = (IBaseRequestBuilder)((IGraphServiceClient)Provider).Drive.Items[oneDriveItem.Id];
            return new OneDriveStorageFolder(Provider, requestBuilder, oneDriveItem);
        }

        /// <summary>
        /// Initialize a GraphOneDriveStorageItem
        /// </summary>
        /// <param name="oneDriveItem">A OneDrive item</param>
        /// <returns>New instance of GraphOneDriveStorageItem</returns>
        protected OneDriveStorageItem InitializeOneDriveStorageItem(DriveItem oneDriveItem)
        {
            IBaseRequestBuilder requestBuilder = (IBaseRequestBuilder)((IGraphServiceClient)Provider).Drive.Items[oneDriveItem.Id];
            return new OneDriveStorageItem(Provider, requestBuilder, oneDriveItem);
        }

        /// <summary>
        /// Initialize a GraphOneDriveStorageFile
        /// </summary>
        /// <param name="oneDriveItem">A OneDrive item</param>
        /// <returns>New instance of GraphOneDriveStorageFile</returns>
        protected OneDriveStorageFile InitializeOneDriveStorageFile(DriveItem oneDriveItem)
        {
            IBaseRequestBuilder requestBuilder = (IBaseRequestBuilder)((IGraphServiceClient)Provider).Drive.Items[oneDriveItem.Id];
            return new OneDriveStorageFile(Provider, requestBuilder, oneDriveItem);
        }
    }
}
