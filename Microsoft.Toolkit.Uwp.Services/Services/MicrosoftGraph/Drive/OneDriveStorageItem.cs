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
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Graph;
using Windows.Storage.Streams;
using static Microsoft.Toolkit.Uwp.Services.MicrosoftGraph.MicrosoftGraphEnums;

namespace Microsoft.Toolkit.Uwp.Services.MicrosoftGraph
{
    /// <summary>
    ///  Class using Office 365 Microsoft Graph Drive API and representing folder or file
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

        private GraphServiceClient _graphProvider;

        /// <summary>
        /// Gets or sets gets GraphServiceClient instance
        /// </summary>
        public GraphServiceClient Provider
        {
            get { return _graphProvider; }
            set { _graphProvider = value; }
        }

        private IDriveItemRequestBuilder _builder;

        /// <summary>
        /// Gets an IDriveItemRequestBuilder instance
        /// </summary>
        public IDriveItemRequestBuilder Builder
        {
            get
            {
                return _builder;
            }
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
        /// Initializes a new instance of the <see cref="OneDriveStorageItem"/> class.
        /// <para>Permissions : Have full access to user files and files shared with user</para>
        /// </summary>
        /// <param name="graphProvider">Instance of GraphClientService class</param>
        /// <param name="builder">Http request builder.</param>
        /// <param name="driveItem">OneDrive's item</param>
        public OneDriveStorageItem(GraphServiceClient graphProvider, IDriveItemRequestBuilder builder, DriveItem driveItem)
        {
            _builder = builder;
            _oneDriveItem = driveItem;
            _graphProvider = graphProvider;
            _name = driveItem.Name;

            // ParentReference null means is root
            if (driveItem.ParentReference != null)
            {
                _path = driveItem.ParentReference.Path.Replace("/drive/root:", string.Empty);
            }

            _dateCreated = driveItem.CreatedDateTime;
        }

        /// <summary>
        /// Deletes the current item.
        /// </summary>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> for the request.</param>
        /// <returns> No object or value is returned by this method when it completes.</returns>
        public async Task DeleteAsync(CancellationToken cancellationToken)
        {
            if (OneDriveItem.Name == "root")
            {
                throw new Microsoft.Graph.ServiceException(new Error { Message = "Could not delete the root folder" });
            }

            await Builder.Request().DeleteAsync(cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Deletes the current item.
        /// </summary>
        /// <returns> No object or value is returned by this method when it completes.</returns>
        public async Task DeleteAsync()
        {
            await DeleteAsync(CancellationToken.None);
        }

        /// <summary>
        /// Retrieves a thumbnail image for the file
        /// </summary>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> for the request.</param>
        /// <param name="optionSize"> A value from the enumeration that specifies the size of the image to retrieve. Small ,Medium, Large</param>
        /// <returns>When this method completes, return a stream containing the thumbnail, or null if no thumbnail are available</returns>
        public async Task<IRandomAccessStream> GetThumbnailAsync(CancellationToken cancellationToken, ThumbnailSize optionSize = ThumbnailSize.Small)
        {
            var thumbnailStream = await Builder.GetThumbnailAsync(Provider, cancellationToken, optionSize);
            if (thumbnailStream == null)
            {
                return null;
            }

            _thumbNail = thumbnailStream.AsRandomAccessStream();
            return _thumbNail;
        }

        /// <summary>
        /// Retrieves a thumbnail image for the file
        /// </summary>
        /// <param name="optionSize"> A value from the enumeration that specifies the size of the image to retrieve. Small ,Medium, Large</param>
        /// <returns>When this method completes, return a stream containing the thumbnail, or null if no thumbnail are available</returns>
        public Task<IRandomAccessStream> GetThumbnailAsync(ThumbnailSize optionSize = ThumbnailSize.Small)
        {
            return GetThumbnailAsync(CancellationToken.None, optionSize);
        }

        ///// <summary>
        ///// Creates a copy of the item in the specified folder and renames the item according to the desired name.
        ///// </summary>
        ///// <param name="destinationFolder">The destination folder where the copy of the item is created.</param>
        ///// <param name="desiredNewName">The new name for the copy of the item created in the destinationFolder.</param>
        ///// <remarks>Because the copy is asynchronous the item could not be available yet.</remarks>
        ///// <returns>return success or failure</returns>
        public Task<bool> CopyAsync(OneDriveStorageFolder destinationFolder, string desiredNewName = null)
        {
            if (OneDriveItem.Name == "root")
            {
                throw new Microsoft.Graph.ServiceException(new Error { Message = "Could not copy the root folder" });
            }

            if (destinationFolder == null)
            {
                throw new ArgumentNullException(nameof(destinationFolder));
            }

            if (desiredNewName == null)
            {
                desiredNewName = OneDriveItem.Name;
            }

            return Builder.CopyAsync(Provider, destinationFolder, desiredNewName);
        }

        /// <summary>
        /// Moves the current item to the specified folder and renames the item according to the desired name.
        /// </summary>
        /// <param name="destinationFolder">The destination folder where the item is moved.</param>
        /// <param name="desiredNewName">The desired name of the item after it is moved.</param>
        /// <returns>return success or failure</returns>
        public Task<bool> MoveAsync(OneDriveStorageFolder destinationFolder, string desiredNewName = null)
        {
            if (OneDriveItem.Name == "root")
            {
                throw new Microsoft.Graph.ServiceException(new Error { Message = "Could not move the root folder" });
            }

            return Builder.MoveAsync(Provider, destinationFolder, desiredNewName);
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
        /// <param name="driveItem">A OneDrive item</param>
        /// <returns>New instance of OneDriveStorageFolder</returns>
        protected OneDriveStorageFolder InitializeOneDriveFolder(DriveItem driveItem)
        {
            var builder = Provider.Me.Drive.Items[driveItem.Id];
            return new OneDriveStorageFolder(Provider, builder, driveItem);
        }

        /// <summary>
        /// Initialize a OneDriveStorageFile
        /// </summary>
        /// <param name="driveItem">A OneDrive item</param>
        /// <returns>New instance of OneDriveStorageFile</returns>
        protected OneDriveStorageFile InitializeOneDriveFile(DriveItem driveItem)
        {
            var builder = Provider.Me.Drive.Items[driveItem.Id];
            return new OneDriveStorageFile(Provider, builder, driveItem);
        }

        /// <summary>
        /// Initialize a OneDriveStorageItem
        /// </summary>
        /// <param name="driveItem">A OneDrive item</param>
        /// <returns>New instance of OneDriveStorageItem</returns>
        protected OneDriveStorageItem InitializeOneDriveItem(DriveItem driveItem)
        {
            var builder = Provider.Me.Drive.Items[driveItem.Id];
            return new OneDriveStorageItem(Provider, builder, driveItem);
        }
    }
}
