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
using System.ComponentModel;
using System.Threading.Tasks;
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
                    Flyout = null;
                    await SignInAsync();
                }
                else
                {
                    Flyout = GenerateMenuItems();
                }
            };

            _aadAuthenticationManager.PropertyChanged += (object sender, PropertyChangedEventArgs e) =>
            {
                if (e.PropertyName == nameof(_aadAuthenticationManager.CurrentUserId))
                {
                    CurrentUserId = _aadAuthenticationManager.CurrentUserId;
                }
            };

            if (_aadAuthenticationManager.IsAuthenticated)
            {
                CurrentUserId = _aadAuthenticationManager.CurrentUserId;
            }
        }

        /// <summary>
        /// This method is used to prompt to login screen.
        /// </summary>
        /// <returns>True if sign in successfully, otherwise false</returns>
        public async Task<bool> SignInAsync()
        {
            if (await _aadAuthenticationManager.ConnectAsync())
            {
                AutomationProperties.SetName(this, string.Empty);

                Flyout = GenerateMenuItems();

                SignInCompleted?.Invoke(this, new SignInEventArgs()
                {
                    GraphClient = _aadAuthenticationManager.GraphProvider
                });

                return true;
            }

            return false;
        }

        /// <summary>
        /// This method is used to sign out the currently signed on user
        /// </summary>
        public void SignOut()
        {
            _aadAuthenticationManager.SignOut();
            SignOutCompleted?.Invoke(this, EventArgs.Empty);
        }

        private MenuFlyout GenerateMenuItems()
        {
            MenuFlyout menuFlyout = new MenuFlyout();

            if (AllowSignInAsDifferentUser)
            {
                MenuFlyoutItem signinanotherItem = new MenuFlyoutItem
                {
                    Text = SignInAnotherUserDefaultText
                };
                AutomationProperties.SetName(signinanotherItem, SignInAnotherUserDefaultText);
                signinanotherItem.Click += async (object sender, RoutedEventArgs e) =>
                {
                    if (await _aadAuthenticationManager.ConnectForAnotherUserAsync())
                    {
                        var graphClient = _aadAuthenticationManager.GraphProvider;

                        SignInCompleted?.Invoke(this, new SignInEventArgs()
                        {
                            GraphClient = graphClient
                        });
                    }
                };
                menuFlyout.Items.Add(signinanotherItem);
            }

            MenuFlyoutItem signoutItem = new MenuFlyoutItem
            {
                Text = SignOutDefaultText
            };
            AutomationProperties.SetName(signoutItem, SignOutDefaultText);
            signoutItem.Click += (object sender, RoutedEventArgs e) => _aadAuthenticationManager.SignOut();
            menuFlyout.Items.Add(signoutItem);

            return menuFlyout;
        }
    }
}
