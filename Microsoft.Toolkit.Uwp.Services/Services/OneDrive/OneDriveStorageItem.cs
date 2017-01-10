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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Storage;
using Windows.Storage.FileProperties;
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
        /// Gets or sets gets GraphServiceClient instance
        /// </summary>
        public IOneDriveClient Provider
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
            if (oneDriveItem.ParentReference != null)
            {
                //_path = oneDriveItem.ParentReference.Path.Replace("/drive/root:", string.Empty);
                _path = oneDriveItem.ParentReference.Path;
            }
        }

        /// <summary>
        /// Initialize a OneDriveStorageFolder
        /// </summary>
        /// <param name="driveItem">A OneDrive item</param>
        /// <returns>New instance of OneDriveStorageFolder</returns>
        protected OneDriveStorageFolder InitializeOneDriveFolder(Item driveItem)
        {
            var requestBuilder = Provider.Drive.Items[driveItem.Id];
            return new OneDriveStorageFolder(Provider, requestBuilder, driveItem);
        }

        public IAsyncAction DeleteAsync()
        {
            throw new NotImplementedException();
        }

        public IAsyncAction DeleteAsync(StorageDeleteOption option)
        {
            throw new NotImplementedException();
        }


        public IAsyncOperation<StorageItemThumbnail> GetThumbnailAsync(ThumbnailMode mode)
        {
            throw new NotImplementedException();
        }

        public IAsyncOperation<StorageItemThumbnail> GetThumbnailAsync(ThumbnailMode mode, uint requestedSize)
        {
            throw new NotImplementedException();
        }

        public IAsyncOperation<StorageItemThumbnail> GetThumbnailAsync(ThumbnailMode mode, uint requestedSize, ThumbnailOptions options)
        {
            throw new NotImplementedException();
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
        /// Check if the item is a folder
        /// </summary>
        /// <returns>Return true if it's a folder</returns>
        internal bool IsFolder()
        {
            return OneDriveItem.Folder != null ? true : false;
        }

        /// <summary>
        /// Check if the item is a file
        /// </summary>
        /// <returns>Return true if it's a file</returns>
        internal bool IsFile()
        {
            return OneDriveItem.File != null ? true : false;
        }

        /// <summary>
        /// Check if the item is a OneNote focument
        /// </summary>
        /// <returns>Return true if it's a OneNote document</returns>
        internal bool IsOneNote()
        {
            return !IsFile() && !IsFolder() ? true : false;
        }

        public IAsyncAction RenameAsync(string desiredName)
        {
            throw new NotImplementedException();
        }

        public IAsyncAction RenameAsync(string desiredName, NameCollisionOption option)
        {
            throw new NotImplementedException();
        }
    }
}
