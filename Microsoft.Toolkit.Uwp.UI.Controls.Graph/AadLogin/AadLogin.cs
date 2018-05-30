// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Threading.Tasks;
using Microsoft.Identity.Client;
using Microsoft.Toolkit.Services.MicrosoftGraph;
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
        protected async override void OnApplyTemplate()
        {
            ApplyTemplate();

            AutomationProperties.SetName(this, SignInDefaultText);

            Click += async (object sender, RoutedEventArgs e) =>
            {
                if (!GraphService.IsAuthenticated)
                {
                    IsEnabled = false;
                    Flyout = null;
                    await SignInAsync();
                    IsEnabled = true;
                }
                else
                {
                    Flyout = GenerateMenuItems();
                }
            };

            GraphService.IsAuthenticatedChanged -= GraphService_StateChanged;
            GraphService.IsAuthenticatedChanged += GraphService_StateChanged;

            if (GraphService.IsAuthenticated)
            {
                CurrentUserId = (await GraphService.User.GetProfileAsync(new MicrosoftGraphUserFields[1] { MicrosoftGraphUserFields.Id })).Id;
            }
        }

        private async void GraphService_StateChanged(object sender, EventArgs e)
        {
            if (GraphService.IsAuthenticated)
            {
                CurrentUserId = (await GraphService.User.GetProfileAsync(new MicrosoftGraphUserFields[1] { MicrosoftGraphUserFields.Id })).Id;
            }
            else
            {
                CurrentUserId = string.Empty;
            }
        }

        /// <summary>
        /// This method is used to prompt to login screen.
        /// </summary>
        /// <returns>True if sign in successfully, otherwise false</returns>
        public async Task<bool> SignInAsync()
        {
            var success = false;

            try
            {
                success = await GraphService.TryLoginAsync();
            }
            catch (Exception ex)
            {
                SignInFailed?.Invoke(this, new SignInFailedEventArgs(ex));
            }

            if (success)
            {
                AutomationProperties.SetName(this, string.Empty);

                Flyout = GenerateMenuItems();

                SignInCompleted?.Invoke(this, new SignInEventArgs()
                {
                    GraphClient = GraphService.GraphProvider
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
            GraphService.Logout();
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
                    try
                    {
                        if (await GraphService.ConnectForAnotherUserAsync())
                        {
                            var graphClient = GraphService.GraphProvider;

                            SignInCompleted?.Invoke(this, new SignInEventArgs()
                            {
                                GraphClient = graphClient
                            });

                            CurrentUserId = (await GraphService.User.GetProfileAsync(new MicrosoftGraphUserFields[1] { MicrosoftGraphUserFields.Id })).Id;
                        }
                    }
                    catch (MsalServiceException ex)
                    {
                        // Swallow error in case of authentication cancellation.
                        if (ex.ErrorCode != "authentication_canceled"
                            && ex.ErrorCode != "access_denied")
                        {
                            throw ex;
                        }
                    }
                };
                menuFlyout.Items.Add(signinanotherItem);
            }

            MenuFlyoutItem signoutItem = new MenuFlyoutItem
            {
                Text = SignOutDefaultText
            };
            AutomationProperties.SetName(signoutItem, SignOutDefaultText);
            signoutItem.Click += (object sender, RoutedEventArgs e) => GraphService.Logout();
            menuFlyout.Items.Add(signoutItem);

            return menuFlyout;
        }
    }
}
