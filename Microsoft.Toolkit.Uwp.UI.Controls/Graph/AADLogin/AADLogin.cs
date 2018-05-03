using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Microsoft.Toolkit.Uwp.UI.Controls.Graph
{
    /// <summary>
    /// The AAD Login Control leverages MSAL libraries to support basic AAD sign-in processes for Microsoft Graph and beyond.
    /// </summary>
    public partial class AADLogin : Control
    {
        private static PublicClientApplication _identityClientApp = null;

        private Button _mainButton = null;

        public AADLogin()
        {
            DefaultStyleKey = typeof(AADLogin);
            IsEnabled = false;
        }

        /// <summary>
        /// Override default OnApplyTemplate to capture child controls
        /// </summary>
        protected override void OnApplyTemplate()
        {
            base.ApplyTemplate();

            _mainButton = GetTemplateChild("btnMain") as Button;

            _mainButton.Click += async (object sender, RoutedEventArgs e) =>
            {
                var btn = sender as Button;

                if (string.IsNullOrEmpty(_currentUserID))
                {
                    btn.IsEnabled = false;
                    if (await SignInAsync()) btn.Flyout = GenerateMenuItems();
                    btn.IsEnabled = true;
                }
            };
        }

        public async Task<bool> SignInAsync()
        {
            if (!string.IsNullOrEmpty(ClientId) && !string.IsNullOrEmpty(Scopes))
            {
                var token = await GetTokenForUserAsync();

                if (!string.IsNullOrEmpty(token))
                {
                    var graphClient = Common.GetAuthenticatedClient(token);

                    _graphAccessToken = token;

                    _currentUserID = (await graphClient.Me.Request().GetAsync()).Id;

                    OnSignInCompleted(new SignInEventArgs()
                    {
                        GraphClient = graphClient,
                        GraphAccessToken = token,
                        CurrentSignInUserId = _currentUserID
                    });

                    return true;
                }
            }

            return false;
        }

        public void SignOut()
        {
            if (_identityClientApp.Users != null)
            {
                foreach (var user in _identityClientApp.Users)
                {
                    _identityClientApp.Remove(user);
                }

                _currentUserID = "";
                _mainButton.Flyout = null;
                OnSignOutCompleted();
            }
        }

        private void InitialPublicClientApplication()
        {
            if (!string.IsNullOrEmpty(ClientId) && !string.IsNullOrEmpty(Scopes))
            {
                _identityClientApp = new PublicClientApplication(ClientId);
                IsEnabled = true;
            }
        }

        private async Task<string> GetTokenForUserAsync()
        {
            string TokenForUser = null;

            AuthenticationResult authResult;
            try
            {
                authResult = await _identityClientApp.AcquireTokenSilentAsync(Scopes.Split(','), _identityClientApp.Users.First());
                TokenForUser = authResult.AccessToken;
            }
            catch
            {
                try
                {
                    authResult = await _identityClientApp.AcquireTokenAsync(Scopes.Split(','));
                    TokenForUser = authResult.AccessToken;
                }
                catch
                { }
            }

            return TokenForUser;
        }

        private async Task<string> GetTokenForAnotherUserAsync()
        {
            string TokenForUser = null;

            try
            {
                AuthenticationResult authResult = await _identityClientApp.AcquireTokenAsync(Scopes.Split(','));
                TokenForUser = authResult.AccessToken;
            }
            catch
            { }

            return TokenForUser;
        }

        private MenuFlyout GenerateMenuItems()
        {
            MenuFlyout menuFlyout = new MenuFlyout();

            if (AllowSignInAsDifferentUser)
            {
                MenuFlyoutItem signinanotherItem = new MenuFlyoutItem();
                signinanotherItem.Text = SignInAnotherUserDefaultText;
                signinanotherItem.Click += async (object sender, RoutedEventArgs e) =>
                {
                    var token = await GetTokenForAnotherUserAsync();
                    if (!string.IsNullOrEmpty(token))
                    {
                        var graphClient = Common.GetAuthenticatedClient(token);

                        _graphAccessToken = token;
                        _currentUserID = (await graphClient.Me.Request().GetAsync()).Id;

                        OnSignInCompleted(new SignInEventArgs()
                        {
                            GraphClient = graphClient,
                            GraphAccessToken = token,
                            CurrentSignInUserId = _currentUserID
                        });
                    }
                };
                menuFlyout.Items.Add(signinanotherItem);
            }

            MenuFlyoutItem signoutItem = new MenuFlyoutItem();
            signoutItem.Text = SignOutDefaultText;
            signoutItem.Click += (object sender, RoutedEventArgs e) => SignOut();
            menuFlyout.Items.Add(signoutItem);

            return menuFlyout;
        }
    }
}
