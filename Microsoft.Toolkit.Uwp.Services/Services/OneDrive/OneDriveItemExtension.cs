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
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Graph;
using Microsoft.OneDrive.Sdk;
using Newtonsoft.Json;
using static Microsoft.Toolkit.Uwp.Services.OneDrive.OneDriveEnums;

namespace Microsoft.Toolkit.Uwp.Services.OneDrive
{
    /// <summary>
    /// Type OneDriveItemExtension
    /// </summary>
    public static class OneDriveItemExtension
    {
        /// <summary>
        /// Copy a OneDrive SDK Item into a Ms Graph DriveItem
        /// </summary>
        /// <param name="item">Item to clone</param>
        /// <returns>a cloned DriveItem</returns>
        public static DriveItem CopyToDriveItem(this Item item)
        {
            var driveItem = new DriveItem();
            driveItem.AdditionalData = item.AdditionalData;
            if (item.Audio != null)
            {
                driveItem.Audio = new Graph.Audio { AdditionalData = item.Audio.AdditionalData, Album = item.Audio.Album, AlbumArtist = item.Audio.AlbumArtist, Artist = item.Audio.Artist, Bitrate = item.Audio.Bitrate, Composers = item.Audio.Composers, Copyright = item.Audio.Copyright, Disc = item.Audio.Disc, DiscCount = item.Audio.DiscCount, Duration = item.Audio.Duration, Genre = item.Audio.Genre, HasDrm = item.Audio.HasDrm, IsVariableBitrate = item.Audio.IsVariableBitrate, Title = item.Audio.Title, Track = item.Audio.Track, TrackCount = item.Audio.TrackCount, Year = item.Audio.Year };
            }

            if (item.Children != null)
            {
                driveItem.Children = new DriveItemChildrenCollectionPage();
                driveItem.Children.AdditionalData = item.Children.AdditionalData;
            }

            driveItem.Content = item.Content;
            if (item.CreatedBy != null)
            {
                driveItem.CreatedBy = item.CreatedBy.CopyTo();
            }

            driveItem.CreatedByUser = null;
            driveItem.CreatedDateTime = item.CreatedDateTime;
            driveItem.CTag = item.CTag;
            if (item.Deleted != null)
            {
                driveItem.Deleted = new Graph.Deleted { AdditionalData = item.Deleted.AdditionalData, State = item.Deleted.State };
            }

            driveItem.Description = driveItem.Description;
            driveItem.ETag = driveItem.ETag;
            if (item.File != null)
            {
                driveItem.File = new Graph.File();
                if (item.File.Hashes != null)
                {
                    driveItem.File.Hashes = new Graph.Hashes { AdditionalData = item.File.Hashes.AdditionalData, Crc32Hash = item.File.Hashes.Crc32Hash, Sha1Hash = item.File.Hashes.Sha1Hash };
                }
            }

            if (item.FileSystemInfo != null)
            {
                driveItem.FileSystemInfo = new Graph.FileSystemInfo { AdditionalData = item.FileSystemInfo.AdditionalData, CreatedDateTime = item.FileSystemInfo.CreatedDateTime, LastAccessedDateTime = item.FileSystemInfo.LastModifiedDateTime };
            }

            if (item.Folder != null)
            {
                driveItem.Folder = new Graph.Folder { AdditionalData = item.AdditionalData, ChildCount = item.Folder.ChildCount };
            }

            driveItem.Id = item.Id;
            if (item.Image != null)
            {
                driveItem.Image = new Graph.Image { AdditionalData = item.Image.AdditionalData, Height = item.Image.Height, Width = item.Image.Width };
            }

            driveItem.LastModifiedBy = item.LastModifiedBy.CopyTo();
            driveItem.LastModifiedDateTime = item.LastModifiedDateTime;
            if (item.Location != null)
            {
                driveItem.Location = new GeoCoordinates { AdditionalData = item.Location.AdditionalData, Altitude = item.Location.Altitude, Latitude = item.Location.Latitude, Longitude = item.Location.Longitude };
            }

            driveItem.Name = item.Name;
            driveItem.ODataType = item.ODataType;
            if (item.ParentReference != null)
            {
                driveItem.ParentReference = item.ParentReference.CopyTo();
            }

            if (item.Permissions != null)
            {
                driveItem.Permissions = new DriveItemPermissionsCollectionPage();
                driveItem.Permissions.AdditionalData = item.Permissions.AdditionalData;
                foreach (var permission in item.Permissions)
                {
                    var driveItemPermission = new Graph.Permission { AdditionalData = permission.AdditionalData, GrantedTo = permission.GrantedTo.CopyTo(), Id = permission.Id, InheritedFrom = permission.InheritedFrom.CopyTo(), ODataType = permission.ODataType, Roles = permission.Roles, ShareId = permission.ShareId };
                    if (permission.Invitation != null)
                    {
                        driveItemPermission.Invitation = new Graph.SharingInvitation { AdditionalData = permission.Invitation.AdditionalData, Email = permission.Invitation.Email, InvitedBy = permission.Invitation.InvitedBy.CopyTo(), SignInRequired = permission.Invitation.SignInRequired };
                    }

                    if (permission.Link != null)
                    {
                        driveItemPermission.Link = new Graph.SharingLink { AdditionalData = permission.Link.AdditionalData, Type = permission.Link.Type, WebUrl = permission.Link.WebUrl };
                    }

                    driveItem.Permissions.Add(driveItemPermission);
                }
            }

            if (item.Photo != null)
            {
                driveItem.Photo = new Graph.Photo { AdditionalData = item.Photo.AdditionalData, CameraMake = item.Photo.CameraMake, CameraModel = item.Photo.CameraModel, ExposureDenominator = item.Photo.ExposureDenominator, ExposureNumerator = item.Photo.ExposureNumerator, FNumber = item.Photo.FNumber, FocalLength = item.Photo.FocalLength, Iso = item.Photo.Iso, TakenDateTime = item.Photo.TakenDateTime };
            }

            driveItem.RemoteItem = null;
            if (item.SearchResult != null)
            {
                driveItem.SearchResult = new Graph.SearchResult { AdditionalData = item.SearchResult.AdditionalData, OnClickTelemetryUrl = item.SearchResult.OnClickTelemetryUrl };
            }

            if (item.Shared != null)
            {
                driveItem.Shared = new Graph.Shared { AdditionalData = item.Shared.AdditionalData, Owner = item.Shared.Owner.CopyTo(), Scope = item.Shared.Scope };
            }

            driveItem.Size = item.Size;

            if (item.SpecialFolder != null)
            {
                driveItem.SpecialFolder = new Graph.SpecialFolder { AdditionalData = item.SpecialFolder.AdditionalData, Name = item.SpecialFolder.Name };
            }

            if (item.Thumbnails != null)
            {
                driveItem.Thumbnails = new DriveItemThumbnailsCollectionPage();
                foreach (var thumbnail in item.Thumbnails)
                {
                    var driveItemThumbNail = new Graph.Thumbnail { AdditionalData = thumbnail.Medium.AdditionalData, Content = thumbnail.Medium.Content, Height = thumbnail.Medium.Height, Url = thumbnail.Medium.Url, Width = thumbnail.Medium.Width };
                }
            }

            if (item.Video != null)
            {
                driveItem.Video = new Graph.Video { AdditionalData = item.Video.AdditionalData, Bitrate = item.Video.Bitrate, Duration = item.Video.Duration, Height = item.Video.Height, Width = item.Video.Width };
            }

            return driveItem;
        }

        /// <summary>
        /// Copy a Microsoft.OneDrive.Sdk.ItemReference to a Graph.ItemReference.
        /// </summary>
        /// <param name="itemReference">Current One Drive ItemReference</param>
        /// <returns>A Graph ItemReference</returns>
        public static Graph.ItemReference CopyTo(this Microsoft.OneDrive.Sdk.ItemReference itemReference)
        {
            return new Graph.ItemReference { AdditionalData = itemReference.AdditionalData, DriveId = itemReference.DriveId, DriveType = "consumer", Id = itemReference.Id, Path = itemReference.Path };
        }

        /// <summary>
        /// Copy a Microsoft.OneDrive.Sdk.IdentitySet to a Graph.IdentitySet
        /// </summary>
        /// <param name="identitySet">Current one drive identitySet</param>
        /// <returns> A Graph IdenditySet</returns>
        public static Graph.IdentitySet CopyTo(this Microsoft.OneDrive.Sdk.IdentitySet identitySet)
        {
            var graphIdentitySet = new Graph.IdentitySet();
            graphIdentitySet.AdditionalData = identitySet.AdditionalData;
            if (identitySet.Application != null)
            {
                graphIdentitySet.Application = identitySet.Application.CopyTo();
            }

            if (identitySet.Device != null)
            {
                graphIdentitySet.Device = identitySet.Device.CopyTo();
            }

            if (identitySet.User != null)
            {
                graphIdentitySet.User = identitySet.User.CopyTo();
            }

            return graphIdentitySet;
        }

        /// <summary>
        /// Copy a Microsoft.OneDrive.Sdk.Identity to a Graph.Identity
        /// </summary>
        /// <param name="identity">Current one drive identity</param>
        /// <returns> A Graph Idendity</returns>
        public static Graph.Identity CopyTo(this Microsoft.OneDrive.Sdk.Identity identity)
        {
            return new Graph.Identity { AdditionalData = identity.AdditionalData, DisplayName = identity.DisplayName, Id = identity.Id };
        }

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
        public static async Task<Stream> GetThumbnailAsync(this IItemRequestBuilder builder, IBaseClient provider, CancellationToken cancellationToken, ThumbnailSize optionSize)
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
        public static async Task<Item> SendAuthenticatedRequestAsync(this IBaseClient provider, HttpRequestMessage request, CancellationToken cancellationToken)
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
        public static async Task<bool> MoveAsync(this IBaseClient provider, HttpRequestMessage request, IOneDriveStorageFolder destinationFolder, string desiredNewName, CancellationToken cancellationToken)
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

        /// <summary>
        /// Create children http request with specific options
        /// </summary>
        /// <param name="requestBuilder">request builder</param>
        /// <param name="top">The number of items to return in a result set.</param>
        /// <param name="orderBy">Sort the order of items in the response collection</param>
        /// <param name="filter">Filters the response based on a set of criteria.</param>
        /// <returns>Returns the http request</returns>
        public static IItemChildrenCollectionRequest CreateChildrenRequest(this IBaseRequestBuilder requestBuilder, int top, OrderBy orderBy = OrderBy.None, string filter = null)
        {
            IItemChildrenCollectionRequest oneDriveitemsRequest = null;
            if (orderBy == OrderBy.None && string.IsNullOrEmpty(filter))
            {
                    return ((IItemRequestBuilder)requestBuilder).Children.Request().Top(top);
            }

            if (orderBy == OrderBy.None)
            {
                return ((IItemRequestBuilder)requestBuilder).Children.Request().Top(top).Filter(filter);
            }

            string order = OneDriveHelper.TransformOrderByToODataString(orderBy);

            if (string.IsNullOrEmpty(filter))
            {
                    oneDriveitemsRequest = ((IItemRequestBuilder)requestBuilder).Children.Request().Top(top).OrderBy(order);
            }
            else
            {
                    oneDriveitemsRequest = ((IItemRequestBuilder)requestBuilder).Children.Request().Top(top).OrderBy(order).Filter(filter);
            }

            return oneDriveitemsRequest;
        }
    }
}
