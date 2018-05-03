using System.Linq;
using System.Threading.Tasks;
using Microsoft.Identity.Client;
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

        /// <summary>
        /// Initializes a new instance of the <see cref="AADLogin"/> class.
        /// </summary>
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
            ApplyTemplate();

            _mainButton = GetTemplateChild("btnMain") as Button;

            _mainButton.Click += async (object sender, RoutedEventArgs e) =>
            {
                var btn = sender as Button;

                if (string.IsNullOrEmpty(CurrentUserID))
                {
                    btn.IsEnabled = false;
                    if (await SignInAsync())
                    {
                        btn.Flyout = GenerateMenuItems();
                    }

                    btn.IsEnabled = true;
                }
            };
        }

        /// <summary>
        /// This method is used to prompt to login screen.
        /// </summary>
        /// <returns>True if sign in successfully, otherwise false</returns>
        public async Task<bool> SignInAsync()
        {
            if (!string.IsNullOrEmpty(ClientId) && !string.IsNullOrEmpty(Scopes))
            {
                var token = await GetTokenForUserAsync();

                if (!string.IsNullOrEmpty(token))
                {
                    var graphClient = Common.GetAuthenticatedClient(token);

                    GraphAccessToken = token;

                    CurrentUserID = (await graphClient.Me.Request().GetAsync()).Id;

                    OnSignInCompleted(new SignInEventArgs()
                    {
                        GraphClient = graphClient,
                        GraphAccessToken = token,
                        CurrentSignInUserId = CurrentUserID
                    });

                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// This method is used to sign out the currently signed on user
        /// </summary>
        public void SignOut()
        {
            if (_identityClientApp.Users != null)
            {
                foreach (var user in _identityClientApp.Users)
                {
                    _identityClientApp.Remove(user);
                }

                CurrentUserID = string.Empty;
                _mainButton.Flyout = null;
                OnSignOutCompleted();
            }
        }

        private void InitialPublicClientApplication()
        {
            if (!string.IsNullOrEmpty(ClientId.Trim()) && !string.IsNullOrEmpty(Scopes.Trim()))
            {
                _identityClientApp = new PublicClientApplication(ClientId);
                IsEnabled = true;
            }
        }

        private async Task<string> GetTokenForUserAsync()
        {
            string tokenForUser = string.Empty;

            AuthenticationResult authResult;
            try
            {
                authResult = await _identityClientApp.AcquireTokenSilentAsync(Scopes.Split(','), _identityClientApp.Users.First());
                tokenForUser = authResult.AccessToken;
            }
            catch
            {
                try
                {
                    authResult = await _identityClientApp.AcquireTokenAsync(Scopes.Split(','));
                    tokenForUser = authResult.AccessToken;
                }
                catch
                {
                }
            }

            return tokenForUser;
        }

        private async Task<string> GetTokenForAnotherUserAsync()
        {
            string tokenForUser = string.Empty;

            try
            {
                AuthenticationResult authResult = await _identityClientApp.AcquireTokenAsync(Scopes.Split(','));
                tokenForUser = authResult.AccessToken;
            }
            catch
            {
            }

            return tokenForUser;
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

                        GraphAccessToken = token;
                        CurrentUserID = (await graphClient.Me.Request().GetAsync()).Id;

                        OnSignInCompleted(new SignInEventArgs()
                        {
                            GraphClient = graphClient,
                            GraphAccessToken = token,
                            CurrentSignInUserId = CurrentUserID
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
