// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Security.Authentication.Web;
using Windows.Storage.Streams;
using winsdkfb;
using winsdkfb.Graph;

namespace Microsoft.Toolkit.Uwp.Services.Facebook
{
    /// <summary>
    /// Class for connecting to Facebook.
    /// </summary>
    public class FacebookService
    {
        /// <summary>
        /// Field for tracking initialization status.
        /// </summary>
        private bool isInitialized;

        /// <summary>
        /// List of permissions required by the app.
        /// </summary>
        private FBPermissions permissions;

        /// <summary>
        /// Gets a Windows Store ID associated with the current app
        /// </summary>
        public string WindowsStoreId => WebAuthenticationBroker.GetCurrentApplicationCallbackUri().ToString();

        /// <summary>
        /// Initializes a new instance of the <see cref="FacebookService"/> class.
        /// </summary>
        public FacebookService()
        {
        }

        /// <summary>
        /// Initialize underlying provider with relevent token information.
        /// </summary>
        /// <param name="oAuthTokens">Token instance.</param>
        /// <param name="requiredPermissions">List of required required permissions. public_profile and user_posts permissions will be used by default.</param>
        /// <returns>Success or failure.</returns>
        public bool Initialize(FacebookOAuthTokens oAuthTokens, FacebookPermissions requiredPermissions = FacebookPermissions.PublicProfile | FacebookPermissions.UserPosts | FacebookPermissions.PublishActions)
        {
            if (oAuthTokens == null)
            {
                throw new ArgumentNullException(nameof(oAuthTokens));
            }

            return Initialize(oAuthTokens.AppId, requiredPermissions, oAuthTokens.WindowsStoreId);
        }

        /// <summary>
        /// Initialize underlying provider with relevent token information.
        /// </summary>
        /// <param name="appId">Application ID (Provided by Facebook developer site)</param>
        /// <param name="requiredPermissions">List of required required permissions. public_profile and user_posts permissions will be used by default.</param>
        /// <param name="windowsStoreId">Windows Store SID</param>
        /// <returns>Success or failure.</returns>
        public bool Initialize(string appId, FacebookPermissions requiredPermissions = FacebookPermissions.PublicProfile | FacebookPermissions.UserPosts | FacebookPermissions.PublishActions, string windowsStoreId = null)
        {
            if (string.IsNullOrEmpty(appId))
            {
                throw new ArgumentNullException(nameof(appId));
            }

            if (string.IsNullOrEmpty(windowsStoreId))
            {
                windowsStoreId = WindowsStoreId;
            }

            isInitialized = true;

            Provider.FBAppId = appId;
            Provider.WinAppId = windowsStoreId;

            // Permissions
            var permissionList = new List<string>();

            foreach (FacebookPermissions value in Enum.GetValues(typeof(FacebookPermissions)))
            {
                if ((requiredPermissions & value) != 0)
                {
                    var name = value.ToString();
                    var finalName = new StringBuilder();

                    foreach (var c in name)
                    {
                        if (char.IsUpper(c))
                        {
                            if (finalName.Length > 0)
                            {
                                finalName.Append('_');
                            }

                            finalName.Append(char.ToLower(c));
                        }
                        else
                        {
                            finalName.Append(c);
                        }
                    }

                    permissionList.Add(finalName.ToString());
                }
            }

            permissions = new FBPermissions(permissionList);

            return true;
        }

        /// <summary>
        /// Private singleton field.
        /// </summary>
        private static FacebookService instance;

        /// <summary>
        /// Gets public singleton property.
        /// </summary>
        public static FacebookService Instance => instance ?? (instance = new FacebookService());

        /// <summary>
        /// Gets a reference to an instance of the underlying data provider.
        /// </summary>
        public FBSession Provider
        {
            get
            {
                if (!isInitialized)
                {
                    throw new InvalidOperationException("Provider not initialized.");
                }

                return FBSession.ActiveSession;
            }
        }

        /// <summary>
        /// Gets the current logged user name.
        /// </summary>
        public string LoggedUser => !Provider.LoggedIn ? null : FBSession.ActiveSession.User.Name;

        /// <summary>
        /// Login with set of required requiredPermissions.
        /// </summary>
        /// <returns>Success or failure.</returns>
        public async Task<bool> LoginAsync()
        {
            if (Provider != null)
            {
                var result = await Provider.LoginAsync(permissions, SessionLoginBehavior.WebView);

                if (result.Succeeded)
                {
                    return true;
                }

                if (result.ErrorInfo != null)
                {
                    Debug.WriteLine(string.Format("Error logging in: {0}", result.ErrorInfo.Message));
                }

                return false;
            }

            Debug.WriteLine("Error logging in - no Active session found");
            return false;
        }

        /// <summary>
        /// Log out of the underlying service instance.
        /// </summary>
        /// <returns>Task to support await of async call.</returns>
        public Task LogoutAsync()
        {
            return Provider.LogoutAsync().AsTask();
        }

        /// <summary>
        /// Request list data from service provider based upon a given config / query.
        /// </summary>
        /// <param name="config">FacebookDataConfig instance.</param>
        /// <param name="maxRecords">Upper limit of records to return.</param>
        /// <returns>Strongly typed list of data returned from the service.</returns>
        public Task<List<FacebookPost>> RequestAsync(FacebookDataConfig config, int maxRecords = 20)
        {
            return RequestAsync<FacebookPost>(config, maxRecords, FacebookPost.Fields);
        }

        /// <summary>
        /// Request list data from service provider based upon a given config / query.
        /// </summary>
        /// <typeparam name="T">Strong type of model.</typeparam>
        /// <param name="config">FacebookDataConfig instance.</param>
        /// <param name="maxRecords">Upper limit of records to return.</param>
        /// <param name="fields">A comma seperated string of required fields, which will have strongly typed representation in the model passed in.</param>
        /// <returns>Strongly typed list of data returned from the service.</returns>
        public async Task<List<T>> RequestAsync<T>(FacebookDataConfig config, int maxRecords = 20, string fields = "id,message,from,created_time,link,full_picture")
        {
            if (Provider.LoggedIn)
            {
                var requestSource = new FacebookRequestSource<T>(config, fields, maxRecords.ToString(), 1);

                var list = await requestSource.GetPagedItemsAsync(0, maxRecords);

                return new List<T>(list);
            }

            var isLoggedIn = await LoginAsync();
            if (isLoggedIn)
            {
                return await RequestAsync<T>(config, maxRecords, fields);
            }

            return null;
        }

        /// <summary>
        /// Request list data from service provider based upon a given config / query.
        /// </summary>
        /// <param name="config">FacebookDataConfig instance.</param>
        /// <param name="pageSize">Upper limit of records to return.</param>
        /// <param name="maxPages">Upper limit of pages to return.</param>
        /// <returns>Strongly typed list of data returned from the service.</returns>
        public Task<IncrementalLoadingCollection<FacebookRequestSource<FacebookPost>, FacebookPost>> RequestAsync(FacebookDataConfig config, int pageSize, int maxPages)
        {
            return RequestAsync<FacebookPost>(config, pageSize, maxPages, FacebookPost.Fields);
        }

        /// <summary>
        /// Request generic list data from service provider based upon a given config / query.
        /// </summary>
        /// <typeparam name="T">Strong type of model.</typeparam>
        /// <param name="config">FacebookDataConfig instance.</param>
        /// <param name="pageSize">Upper limit of records to return.</param>
        /// <param name="maxPages">Upper limit of pages to return.</param>
        /// <param name="fields">A comma seperated string of required fields, which will have strongly typed representation in the model passed in.</param>
        /// <returns>Strongly typed list of data returned from the service.</returns>
        public async Task<IncrementalLoadingCollection<FacebookRequestSource<T>, T>> RequestAsync<T>(FacebookDataConfig config, int pageSize, int maxPages, string fields = "id,message,from,created_time,link,full_picture")
        {
            if (Provider.LoggedIn)
            {
                var requestSource = new FacebookRequestSource<T>(config, fields, pageSize.ToString(), maxPages);

                return new IncrementalLoadingCollection<FacebookRequestSource<T>, T>(requestSource);
            }

            var isLoggedIn = await LoginAsync();
            if (isLoggedIn)
            {
                return await RequestAsync<T>(config, pageSize, maxPages, fields);
            }

            return null;
        }

        /// <summary>
        /// Returns the <see cref="FacebookPicture"/> object associated with the logged user
        /// </summary>
        /// <returns>A <see cref="FacebookPicture"/> object</returns>
        public async Task<FacebookPicture> GetUserPictureInfoAsync()
        {
            if (Provider.LoggedIn)
            {
                var factory = new FBJsonClassFactory(JsonConvert.DeserializeObject<FacebookDataHost<FacebookPicture>>);

                PropertySet propertySet = new PropertySet { { "redirect", "0" } };
                var singleValue = new FBSingleValue("/me/picture", propertySet, factory);

                var result = await singleValue.GetAsync();

                if (result.Succeeded)
                {
                    return ((FacebookDataHost<FacebookPicture>)result.Object).Data;
                }

                throw new Exception(result.ErrorInfo?.Message);
            }

            var isLoggedIn = await LoginAsync();
            if (isLoggedIn)
            {
                return await GetUserPictureInfoAsync();
            }

            return null;
        }

        /// <summary>
        /// Retrieves list of user photo albums.
        /// </summary>
        /// <param name="maxRecords">Upper limit of records to return.</param>
        /// <param name="fields">Custom list of Album fields to retrieve.</param>
        /// <returns>List of User Photo Albums.</returns>
        public async Task<List<FacebookAlbum>> GetUserAlbumsAsync(int maxRecords = 20, string fields = null)
        {
            fields = fields ?? FacebookAlbum.Fields;
            var config = new FacebookDataConfig { Query = "/me/albums" };

            return await RequestAsync<FacebookAlbum>(config, maxRecords, fields);
        }

        /// <summary>
        /// Retrieves list of user photo albums.
        /// </summary>
        /// <param name="pageSize">Number of records to retrieve per page.</param>
        /// <param name="maxPages">Upper limit of pages to return.</param>
        /// <param name="fields">Custom list of Album fields to retrieve.</param>
        /// <returns>List of User Photo Albums.</returns>
        public async Task<IncrementalLoadingCollection<FacebookRequestSource<FacebookAlbum>, FacebookAlbum>> GetUserAlbumsAsync(int pageSize, int maxPages, string fields = null)
        {
            fields = fields ?? FacebookAlbum.Fields;
            var config = new FacebookDataConfig { Query = "/me/albums" };

            return await RequestAsync<FacebookAlbum>(config, pageSize, maxPages, fields);
        }

        /// <summary>
        /// Retrieves list of user photos by album id.
        /// </summary>
        /// <param name="albumId">Albums Id for photos.</param>
        /// <param name="maxRecords">Upper limit of records to return</param>
        /// <param name="fields">Custom list of Photo fields to retrieve.</param>
        /// <returns>List of User Photos.</returns>
        public async Task<List<FacebookPhoto>> GetUserPhotosByAlbumIdAsync(string albumId, int maxRecords = 20, string fields = null)
        {
            fields = fields ?? FacebookPhoto.Fields;
            var config = new FacebookDataConfig { Query = $"/{albumId}/photos" };

            return await RequestAsync<FacebookPhoto>(config, maxRecords, fields);
        }

        /// <summary>
        /// Retrieves list of user photos by album id.
        /// </summary>
        /// <param name="albumId">Albums Id for photos.</param>
        /// <param name="pageSize">Number of records to retrieve per page.</param>
        /// <param name="maxPages">Upper limit of pages to return.</param>
        /// <param name="fields">Custom list of Photo fields to retrieve.</param>
        /// <returns>List of User Photos.</returns>
        public async Task<IncrementalLoadingCollection<FacebookRequestSource<FacebookPhoto>, FacebookPhoto>> GetUserPhotosByAlbumIdAsync(string albumId, int pageSize, int maxPages, string fields = null)
        {
            fields = fields ?? FacebookPhoto.Fields;
            var config = new FacebookDataConfig { Query = $"/{albumId}/photos" };

            return await RequestAsync<FacebookPhoto>(config, pageSize, maxPages, fields);
        }

        /// <summary>
        /// Retrieves a photo by id.
        /// </summary>
        /// <param name="photoId">Photo Id for the photo.</param>
        /// <returns>A single photo.</returns>
        public async Task<FacebookPhoto> GetPhotoByPhotoIdAsync(string photoId)
        {
            if (Provider.LoggedIn)
            {
                var factory = new FBJsonClassFactory(JsonConvert.DeserializeObject<FacebookPhoto>);

                PropertySet propertySet = new PropertySet { { "fields", "images" } };
                var singleValue = new FBSingleValue($"/{photoId}", propertySet, factory);

                var result = await singleValue.GetAsync();

                if (result.Succeeded)
                {
                    return (FacebookPhoto)result.Object;
                }

                throw new Exception(result.ErrorInfo?.Message);
            }

            var isLoggedIn = await LoginAsync();
            if (isLoggedIn)
            {
                return await GetPhotoByPhotoIdAsync(photoId);
            }

            return null;
        }

        /// <summary>
        /// Enables direct posting data to the timeline.
        /// </summary>
        /// <param name="link">Link contained as part of the post. Cannot be null.</param>
        /// <returns>Task to support await of async call.</returns>
        public async Task<bool> PostToFeedAsync(string link)
        {
            if (Provider.LoggedIn)
            {
                var parameters = new PropertySet { { "link", link } };

                string path = FBSession.ActiveSession.User.Id + "/feed";
                var factory = new FBJsonClassFactory(JsonConvert.DeserializeObject<FacebookPost>);

                var singleValue = new FBSingleValue(path, parameters, factory);
                var result = await singleValue.PostAsync();
                if (result.Succeeded)
                {
                    var postResponse = result.Object as FacebookPost;
                    if (postResponse != null)
                    {
                        return true;
                    }
                }

                Debug.WriteLine(string.Format("Could not post. {0}", result.ErrorInfo?.ErrorUserMessage));
                return false;
            }

            var isLoggedIn = await LoginAsync();
            if (isLoggedIn)
            {
                return await PostToFeedAsync(link);
            }

            return false;
        }

        /// <summary>
        /// Enables posting data to the timeline using Facebook dialog.
        /// </summary>
        /// <param name="link">Link contained as part of the post. Cannot be null.</param>
        /// <returns>Task to support await of async call.</returns>
        public async Task<bool> PostToFeedWithDialogAsync(string link)
        {
            if (Provider.LoggedIn)
            {
                var parameters = new PropertySet { { "link", link } };

                var result = await Provider.ShowFeedDialogAsync(parameters);

                if (result.Succeeded)
                {
                    return true;
                }

                Debug.WriteLine(string.Format("Could not post. {0}", result.ErrorInfo?.ErrorUserMessage));
                return false;
            }

            var isLoggedIn = await LoginAsync();
            if (isLoggedIn)
            {
                return await PostToFeedWithDialogAsync(link);
            }

            return false;
        }

        /// <summary>
        /// Enables posting a picture to the timeline
        /// </summary>
        /// <param name="title">Title of the post.</param>
        /// <param name="pictureName">Picture name.</param>
        /// <param name="pictureStream">Picture stream to upload.</param>
        /// <returns>Return ID of the picture</returns>
        public async Task<string> PostPictureToFeedAsync(string title, string pictureName, IRandomAccessStreamWithContentType pictureStream)
        {
            if (pictureStream == null)
            {
                return null;
            }

            if (Provider.LoggedIn)
            {
                var facebookPictureStream = new FBMediaStream(pictureName, pictureStream);
                var parameters = new PropertySet
                {
                    { "source", facebookPictureStream },
                    { "name", title }
                };

                string path = FBSession.ActiveSession.User.Id + "/photos";
                var factory = new FBJsonClassFactory(JsonConvert.DeserializeObject<FacebookPicture>);

                var singleValue = new FBSingleValue(path, parameters, factory);
                var result = await singleValue.PostAsync();
                if (result.Succeeded)
                {
                    var photoResponse = result.Object as FacebookPicture;
                    if (photoResponse != null)
                    {
                        return photoResponse.Id;
                    }
                }

                return null;
            }

            var isLoggedIn = await LoginAsync();
            if (isLoggedIn)
            {
                return await PostPictureToFeedAsync(title, pictureName, pictureStream);
            }

            return null;
        }
    }
}
