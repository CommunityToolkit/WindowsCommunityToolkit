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
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.Graph;

namespace Microsoft.Toolkit.Uwp.Services.MicrosoftGraph
{
    /// <summary>
    ///  Class for connecting to Office 365 Microsoft Graph
    /// </summary>
    public partial class MicrosoftGraphService : Toolkit.Services.MicrosoftGraph.MicrosoftGraphService
    {
        /// <summary>
        /// Gets or sets field to store the model of authentication
        /// V1 Only for Work or Scholar account
        /// V2 for MSA and Work or Scholar account
        /// </summary>
        public Toolkit.Services.MicrosoftGraph.MicrosoftGraphEnums.AuthenticationModel AuthenticationModel { get; set; }

        /// <summary>
        /// Private singleton field.
        /// </summary>
        private static MicrosoftGraphService _instance;

        /// <summary>
        /// Gets public singleton property.
        /// </summary>
        public static new MicrosoftGraphService Instance => _instance ?? (_instance = new MicrosoftGraphService());

        /// <summary>
        /// Gets or sets fields to store a MicrosoftGraphServiceMessages instance
        /// </summary>
        public new MicrosoftGraphUserService User { get; set; }

        /// <summary>
        /// Logout the current user
        /// </summary>
        /// <returns>success or failure</returns>
        public override async Task<bool> Logout()
        {
            if (!IsInitialized)
            {
                throw new InvalidOperationException("Microsoft Graph not initialized.");
            }

            var authenticationModel = AuthenticationModel.ToString();
            return await ((MicrosoftGraphAuthenticationHelper)Authentication).LogoutAsync(authenticationModel);
        }

        /// <summary>
        /// Login the user from Azure AD and Get Microsoft Graph access token.
        /// </summary>
        /// <remarks>Need Sign in and read user profile scopes (User.Read)</remarks>
        /// <returns>Returns success or failure of login attempt.</returns>
        public override async Task<bool> LoginAsync()
        {
            return await LoginAsync(string.Empty);
        }

        /// <summary>
        /// Login the user from Azure AD and Get Microsoft Graph access token.
        /// </summary>
        /// <remarks>Need Sign in and read user profile scopes (User.Read)</remarks>
        /// <param name="loginHint">UPN for user - avoids realm discovery prompt</param>
        /// <returns>Returns success or failure of login attempt.</returns>
        public async Task<bool> LoginAsync(string loginHint)
        {
            IsConnected = false;
            if (!IsInitialized)
            {
                throw new InvalidOperationException("Microsoft Graph not initialized.");
            }

            Authentication = new MicrosoftGraphAuthenticationHelper(DelegatedPermissionScopes);
            string accessToken = null;
            if (AuthenticationModel == Toolkit.Services.MicrosoftGraph.MicrosoftGraphEnums.AuthenticationModel.V1)
            {
                accessToken = await ((MicrosoftGraphAuthenticationHelper)Authentication).GetUserTokenAsync(AppClientId);
            }
            else
            {
                accessToken = await Authentication.GetUserTokenV2Async(AppClientId, loginHint);
            }

            if (string.IsNullOrEmpty(accessToken))
            {
                return IsConnected;
            }

            IsConnected = true;

            User = new MicrosoftGraphUserService(GraphProvider);

            if ((ServicesToInitialize & Toolkit.Services.MicrosoftGraph.MicrosoftGraphEnums.ServicesToInitialize.UserProfile) == Toolkit.Services.MicrosoftGraph.MicrosoftGraphEnums.ServicesToInitialize.UserProfile)
            {
                await GetUserAsyncProfile();
            }

            // if ((_servicesToInitialize & ServicesToInitialize.OneDrive) == ServicesToInitialize.OneDrive)
            // {
            //    _user.InitializeDrive();
            // }
            if ((ServicesToInitialize & Toolkit.Services.MicrosoftGraph.MicrosoftGraphEnums.ServicesToInitialize.Message) == Toolkit.Services.MicrosoftGraph.MicrosoftGraphEnums.ServicesToInitialize.Message)
            {
                User.InitializeMessage();
            }

            if ((ServicesToInitialize & Toolkit.Services.MicrosoftGraph.MicrosoftGraphEnums.ServicesToInitialize.Event) == Toolkit.Services.MicrosoftGraph.MicrosoftGraphEnums.ServicesToInitialize.Event)
            {
                User.InitializeEvent();
            }

            return IsConnected;
        }

        /// <summary>
        /// Create Microsoft Graph client
        /// </summary>
        /// <param name='appClientId'>Azure AD's App client id</param>
        /// <returns>instance of the GraphServiceclient</returns>
        internal override GraphServiceClient CreateGraphClientProvider(string appClientId)
        {
            if (AuthenticationModel == Toolkit.Services.MicrosoftGraph.MicrosoftGraphEnums.AuthenticationModel.V1)
            {
                return new GraphServiceClient(
                  new DelegateAuthenticationProvider(
                     async (requestMessage) =>
                     {
                        requestMessage.Headers.Authorization =
                            new AuthenticationHeaderValue(
                                        "bearer",
                                        await ((MicrosoftGraphAuthenticationHelper)Authentication).GetUserTokenAsync(appClientId).ConfigureAwait(false));
                         return;
                     }));
            }
            else
            {
                return base.CreateGraphClientProvider(appClientId);
            }
        }

        /// <summary>
        /// Initialize a instance of MicrosoftGraphUserService class
        /// </summary>
        /// <returns><see cref="Task"/> representing the asynchronous operation.</returns>
        protected override async Task GetUserAsyncProfile()
        {
            Toolkit.Services.MicrosoftGraph.MicrosoftGraphUserFields[] selectedFields =
            {
                Toolkit.Services.MicrosoftGraph.MicrosoftGraphUserFields.Id
            };

            await User.GetProfileAsync(selectedFields);
        }
    }
}
