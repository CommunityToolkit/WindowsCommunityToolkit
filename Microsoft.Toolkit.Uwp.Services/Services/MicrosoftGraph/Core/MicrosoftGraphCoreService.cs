// ******************************************************************
//
// Copyright (c) Microsoft. All rights reserved.
// This code is licensed under the MIT License (MIT).
// THE CODE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
// TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH
// THE CODE OR THE USE OR OTHER DEALINGS IN THE CODE.
//
// ******************************************************************

namespace Microsoft.Toolkit.Uwp.Services.MicrosoftGraph
{
    using System;
    using System.Net.Http.Headers;
    using System.Threading.Tasks;
    using Graph;
    using Microsoft.Toolkit.Uwp.Services.AzureAD;

    /// <summary>
    ///  Class for connecting to Office 365 Microsoft Graph
    /// </summary>
    public partial class MicrosoftGraphService
    {
        private MicrosoftGraphService()
        {
        }

        /// <summary>
        /// Private singleton field.
        /// </summary>
        private static MicrosoftGraphService instance;

        /// <summary>
        /// Gets public singleton property.
        /// </summary>
        public static MicrosoftGraphService Instance => instance ?? (instance = new MicrosoftGraphService());

        /// <summary>
        /// Field for tracking initialization status.
        /// </summary>
        private bool isInitialized;

        /// <summary>
        /// Field to store Azure AD Application clientid
        /// </summary>
        private string appClientId;

        /// <summary>
        /// Microsoft Graph client instance.
        /// </summary>
        private GraphServiceClient graphserviceClient = null;

        /// <summary>
        /// Initialize Microsoft Graph.
        /// </summary>
        /// <param name="appClientId">Azure AD's App client id</param>
        /// <returns>Success or failure.</returns>
        public bool Initialize(string appClientId)
        {
            if (string.IsNullOrEmpty(appClientId))
            {
                throw new ArgumentNullException(nameof(appClientId));
            }

            this.appClientId = appClientId;

            graphserviceClient = CreateGraphClient(appClientId);
            isInitialized = true;
            return true;
        }

        /// <summary>
        /// Create Microsoft Graph client
        /// </summary>
        /// <param name="appClientId">Azure AD's App client id</param>
        /// <returns>instance of the GraphServiceclient</returns>
        public GraphServiceClient CreateGraphClient(string appClientId)
        {
            return new GraphServiceClient(
                  new DelegateAuthenticationProvider(
                     async (requestMessage) =>
                     {
                         // requestMessage.Headers.Add("outlook.timezone", "Romance Standard Time");
                         requestMessage.Headers.Authorization =
                                            new AuthenticationHeaderValue(
                                                     "bearer",
                                                     await AuthenticationHelper.Instance.GetUserTokenAsync(appClientId).ConfigureAwait(false)
                                                     );
                         return;
                     }));
        }

        /// <summary>
        /// Log user from Azure AD and Get Microsoft Graph access token.
        /// </summary>
        /// <remarks>Need Sign in and read user profile scopes (User.Read)</remarks>
        /// <see cref="Http://graph.microsoft.io/en-us/docs/authorization/permission_scopes"/>
        /// <returns>Returns success or failure of login attempt.</returns>
        public async Task<bool> LoginAsync()
        {
            if (!isInitialized)
            {
                throw new InvalidOperationException("Microsoft Graph not initialized.");
            }

            var accessToken = await AuthenticationHelper.Instance.GetUserTokenAsync(appClientId);
            if (string.IsNullOrEmpty(accessToken))
            {
                return false;
            }

            return true;
        }
    }
}
