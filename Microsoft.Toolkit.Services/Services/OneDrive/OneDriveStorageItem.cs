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
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Graph;
using Microsoft.Toolkit.Services.OneDrive.Platform;
using Newtonsoft.Json;

namespace Microsoft.Toolkit.Services.OneDrive
{
    /// <summary>
    /// GraphOneDriveStorageItem class.
    /// </summary>
    public class OneDriveStorageItem
    {
        /// <summary>
        /// Gets or sets platform-specific implementation of platform services.
        /// </summary>
        public IOneDriveStorageItemPlatform StorageItemPlatformService { get; set; }

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
            StorageItemPlatformService = OneDriveService.ServicePlatformInitializer.CreateOneDriveStorageItemPlatformInstance(OneDriveService.Instance, this);

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
                string rootMarker = "/root:";
                int index = oneDriveItem.ParentReference.Path.LastIndexOf(rootMarker) + rootMarker.Length;
                if (index >= 0)
                {
                    _path = oneDriveItem.ParentReference.Path.Substring(index);
                }
            }
        }

        /// <summary>
        /// Deletes the current item.
        /// </summary>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> for the request.</param>
        /// <returns> No object or value is returned by this method when it completes.</returns>
        public Task DeleteAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            if (_name == "root")
            {
                throw new Microsoft.Graph.ServiceException(new Error { Message = "Could not delete the root folder" });
            }

            return ((IDriveItemRequestBuilder)RequestBuilder).Request().DeleteAsync(cancellationToken);
        }

        /// <summary>
        /// Retrieves a thumbnail set for the file
        /// </summary>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> for the request.</param>
        /// <returns>When this method completes, return a thumbnail set, or null if no thumbnail are available</returns>
        public Task<OneDriveThumbnailSet> GetThumbnailSetAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            return ((IDriveItemRequestBuilder)RequestBuilder).GetThumbnailSetAsync(cancellationToken);
        }

        /// <summary>
        /// Renames the current folder.
        /// </summary>
        /// <param name="desiredName">The desired, new name for the current folder.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> for the request.</param>
        /// <returns>When this method completes successfully, it returns an IOneDriveStorageItem that represents the specified folder.</returns>
        public async Task<OneDriveStorageItem> RenameAsync(string desiredName, CancellationToken cancellationToken = default(CancellationToken))
        {
            DriveItem newOneDriveItem = new DriveItem { Name = desiredName, Description = "Item Renamed from Windows Community Toolkit" };
            var itemRenamed = await ((IDriveItemRequestBuilder)RequestBuilder).Request().UpdateAsync(newOneDriveItem, cancellationToken).ConfigureAwait(false);
            return new OneDriveStorageItem(_oneDriveProvider, RequestBuilder, itemRenamed);
        }

        /// <summary>
        /// Moves the current item to the specified folder and renames the item according to the desired name.
        /// </summary>
        /// <param name="destinationFolder">The destination folder where the item is moved.</param>
        /// <param name="desiredNewName">The desired name of the item after it is moved.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> for the request.</param>
        /// <returns>return success or failure</returns>
        public Task<bool> MoveAsync(OneDriveStorageFolder destinationFolder, string desiredNewName = null, CancellationToken cancellationToken = default(CancellationToken))
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

            return ((IGraphServiceClient)Provider).MoveAsync(request, destinationFolder, desiredNewName, cancellationToken);
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

            var copyRequest = ((IGraphServiceClient)Provider).Drive.Items[OneDriveItem.Id].Copy(desiredNewName);

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
            return OneDriveItem.Folder != null;
        }

        /// <summary>
        /// Check if the item is a file
        /// </summary>
        /// <returns>Return true if it's a file</returns>
        public bool IsFile()
        {
            return OneDriveItem.File != null;
        }

        /// <summary>
        /// Check if the item is a OneNote focument
        /// </summary>
        /// <returns>Return true if it's a OneNote document</returns>
        public bool IsOneNote()
        {
            return !IsFile() && !IsFolder();
        }

        /// <summary>
        /// Initialize a GraphOneDriveStorageFolder
        /// </summary>
        /// <param name="oneDriveItem">A OneDrive item</param>
        /// <returns>New instance of GraphOneDriveStorageFolder</returns>
        internal OneDriveStorageFolder InitializeOneDriveStorageFolder(DriveItem oneDriveItem)
        {
            IBaseRequestBuilder requestBuilder = GetDriveRequestBuilderFromDriveId(oneDriveItem.ParentReference.DriveId).Items[oneDriveItem.Id];
            return new OneDriveStorageFolder(Provider, requestBuilder, oneDriveItem);
        }

        /// <summary>
        /// Initialize a GraphOneDriveStorageItem
        /// </summary>
        /// <param name="oneDriveItem">A OneDrive item</param>
        /// <returns>New instance of GraphOneDriveStorageItem</returns>
        internal OneDriveStorageItem InitializeOneDriveStorageItem(DriveItem oneDriveItem)
        {
            IBaseRequestBuilder requestBuilder = GetDriveRequestBuilderFromDriveId(oneDriveItem.ParentReference.DriveId).Items[oneDriveItem.Id];
            return new OneDriveStorageItem(Provider, requestBuilder, oneDriveItem);
        }

        /// <summary>
        /// Initialize a GraphOneDriveStorageFile
        /// </summary>
        /// <param name="oneDriveItem">A OneDrive item</param>
        /// <returns>New instance of GraphOneDriveStorageFile</returns>
        internal OneDriveStorageFile InitializeOneDriveStorageFile(DriveItem oneDriveItem)
        {
            IBaseRequestBuilder requestBuilder = GetDriveRequestBuilderFromDriveId(oneDriveItem.ParentReference.DriveId).Items[oneDriveItem.Id];
            return new OneDriveStorageFile(Provider, requestBuilder, oneDriveItem);
        }

        internal IDriveRequestBuilder GetDriveRequestBuilderFromDriveId(string driveId)
        {
            return (IBaseRequestBuilder)((IGraphServiceClient)Provider).Drives[driveId] as IDriveRequestBuilder;
        }
    }
}
