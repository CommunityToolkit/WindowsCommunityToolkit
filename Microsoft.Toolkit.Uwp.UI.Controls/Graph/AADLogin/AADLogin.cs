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
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Identity.Client;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Automation;
using Windows.UI.Xaml.Controls;

namespace Microsoft.Toolkit.Uwp.UI.Controls.Graph
{
    /// <summary>
    /// The AAD Login Control leverages MSAL libraries to support basic AAD sign-in processes for Microsoft Graph and beyond.
    /// </summary>
    [TemplatePart(Name = "RootGrid", Type = typeof(Grid))]
    [TemplatePart(Name = "ContentPresenter", Type = typeof(ContentPresenter))]
    public partial class AadLogin : Button
    {
        private AadAuthenticationManager _aadAuthenticationManager = AadAuthenticationManager.Instance;

        /// <summary>
        /// Initializes a new instance of the <see cref="AadLogin"/> class.
        /// </summary>
        public AadLogin()
        {
            DefaultStyleKey = typeof(AadLogin);
        }

        /// <summary>
        /// Override default OnApplyTemplate to capture child controls
        /// </summary>
        protected override void OnApplyTemplate()
        {
            ApplyTemplate();

            AutomationProperties.SetName(this, SignInDefaultText);

            Click += async (object sender, RoutedEventArgs e) =>
            {
                if (!_aadAuthenticationManager.IsAuthenticated)
                {
                    if (await _aadAuthenticationManager.SignInAsync())
                    {
                        AutomationProperties.SetName(this, string.Empty);
                        Flyout = GenerateMenuItems();
                    }
                }
                else
                {
                    Flyout = GenerateMenuItems();
                }
            };
        }

        /// <summary>
        /// This method is used to prompt to login screen.
        /// </summary>
        /// <returns>True if sign in successfully, otherwise false</returns>
        public async Task<bool> SignInAsync()
        {
            await _aadAuthenticationManager.SignInAsync();
            //if (!string.IsNullOrEmpty(ClientId) && !string.IsNullOrEmpty(Scopes))
            //{
            //    var token = await GetTokenForUserAsync();

            //    if (!string.IsNullOrEmpty(token))
            //    {
            //        var graphClient = Common.GetAuthenticatedClient(token);

            //        GraphAccessToken = token;

            //        CurrentUserID = (await graphClient.Me.Request().GetAsync()).Id;

            //        SignInCompleted?.Invoke(this, new SignInEventArgs()
            //        {
            //            GraphClient = graphClient,
            //            GraphAccessToken = token,
            //            CurrentSignInUserId = CurrentUserID
            //        });

            //        return true;
            //    }
            //}

            return false;
        }

        /// <summary>
        /// This method is used to sign out the currently signed on user
        /// </summary>
        public void SignOut()
        {
            //if (_identityClientApp.Users != null)
            //{
            //    foreach (var user in _identityClientApp.Users)
            //    {
            //        _identityClientApp.Remove(user);
            //    }

            //    CurrentUserID = string.Empty;

            //    this.Flyout = null;

            //    SignOutCompleted?.Invoke(this, EventArgs.Empty);
            //}

            //AutomationProperties.SetName(this, SignInDefaultText);
        }

        private void InitializePublicClientApplication()
        {
            //if (!string.IsNullOrEmpty(ClientId) && !string.IsNullOrEmpty(Scopes))
            //{
            //    _identityClientApp = new PublicClientApplication(ClientId);
            //    IsEnabled = true;
            //}
        }

        private async Task<string> GetTokenForUserAsync()
        {
            string tokenForUser = string.Empty;

            //AuthenticationResult authResult;
            //try
            //{
            //    authResult = await _identityClientApp.AcquireTokenSilentAsync(Scopes.Split(','), _identityClientApp.Users.First());
            //    tokenForUser = authResult.AccessToken;
            //}
            //catch
            //{
            //    tokenForUser = await GetTokenWithPromptAsync();
            //}

            return tokenForUser;
        }

        private async Task<string> GetTokenWithPromptAsync()
        {
            string tokenForUser = string.Empty;

            //try
            //{
            //    AuthenticationResult authResult = await _identityClientApp.AcquireTokenAsync(Scopes.Split(','));
            //    tokenForUser = authResult.AccessToken;
            //}
            //catch
            //{
            //}

            return tokenForUser;
        }

        private MenuFlyout GenerateMenuItems()
        {
            MenuFlyout menuFlyout = new MenuFlyout();

            //if (AllowSignInAsDifferentUser)
            //{
            //    MenuFlyoutItem signinanotherItem = new MenuFlyoutItem();
            //    signinanotherItem.Text = SignInAnotherUserDefaultText;
            //    AutomationProperties.SetName(signinanotherItem, SignInAnotherUserDefaultText);
            //    signinanotherItem.Click += async (object sender, RoutedEventArgs e) =>
            //    {
            //        var token = await GetTokenWithPromptAsync();
            //        if (!string.IsNullOrEmpty(token))
            //        {
            //            var graphClient = Common.GetAuthenticatedClient(token);

            //            GraphAccessToken = token;
            //            CurrentUserID = (await graphClient.Me.Request().GetAsync()).Id;

            //            SignInCompleted?.Invoke(this, new SignInEventArgs()
            //            {
            //                GraphClient = graphClient,
            //                GraphAccessToken = token,
            //                CurrentSignInUserId = CurrentUserID
            //            });
            //        }
            //    };
            //    menuFlyout.Items.Add(signinanotherItem);
            //}

            //MenuFlyoutItem signoutItem = new MenuFlyoutItem();
            //signoutItem.Text = SignOutDefaultText;
            //AutomationProperties.SetName(signoutItem, SignOutDefaultText);
            //signoutItem.Click += (object sender, RoutedEventArgs e) => SignOut();
            //menuFlyout.Items.Add(signoutItem);

            return menuFlyout;
        }
    }
}
