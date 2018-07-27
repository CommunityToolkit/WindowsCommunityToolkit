// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Linq;
using Microsoft.Toolkit.Services.MicrosoftGraph;
using Microsoft.Toolkit.Uwp.UI.Controls.Graph;
using Microsoft.Toolkit.Uwp.UI.Extensions;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Microsoft.Toolkit.Uwp.SampleApp.Controls
{
    public sealed partial class AadAuthControl : UserControl
    {
        public static readonly DependencyProperty IsEnableSignInButtonProperty = DependencyProperty.Register(
            nameof(IsEnableSignInButton),
            typeof(bool),
            typeof(AadAuthControl),
            new PropertyMetadata(false));

        public static readonly DependencyProperty IsShowSignInButtonProperty = DependencyProperty.Register(
            nameof(IsShowSignInButton),
            typeof(bool),
            typeof(AadAuthControl),
            new PropertyMetadata(true));

        private static string _clientId = string.Empty;

        private static string[] _scopes = AadLogin.RequiredDelegatedPermissions
            .Union(ProfileCard.RequiredDelegatedPermissions)
            .Union(PeoplePicker.RequiredDelegatedPermissions)
            .Union(SharePointFileList.RequiredDelegatedPermissions)
            .Distinct()
            .ToArray();

        public bool IsEnableSignInButton
        {
            get { return (bool)GetValue(IsEnableSignInButtonProperty); }
            set { SetValue(IsEnableSignInButtonProperty, value); }
        }

        public bool IsShowSignInButton
        {
            get { return (bool)GetValue(IsShowSignInButtonProperty); }
            set { SetValue(IsShowSignInButtonProperty, value); }
        }

        private MicrosoftGraphService _graphService = MicrosoftGraphService.Instance;

        private string[] _scopesForReAuth;

        public AadAuthControl()
        {
            InitializeComponent();

            Loading += AadAuthControl_Loading;
            ClientId.TextChanged += ClientId_TextChanged;
            _graphService.SignInFailed += GraphService_SignInFailed;
            _graphService.IsAuthenticatedChanged += GraphService_IsAuthenticatedChanged;
        }

        private void AadAuthControl_Loading(FrameworkElement sender, object args)
        {
            IsEnabled = !_graphService.IsAuthenticated;

            // merge admin permissions
            var adminPerms = GetAdminPermissions();
            if (!adminPerms.All(o => _scopes.Contains(o)))
            {
                if (_graphService.IsAuthenticated)
                {
                    // re-auth required, it might fail, so store it in a local variable to avoid impacts to previous controls
                    _scopesForReAuth = _scopes.Concat(adminPerms).Distinct().ToArray();

                    var result = _graphService.Logout().Result;
                }
                else
                {
                    // never authenticated previously, merge permissions immediately
                    _scopes = _scopes.Concat(adminPerms).Distinct().ToArray();
                }
            }

            Scopes.Text = string.Join(", ", _scopesForReAuth ?? _scopes);

            ClientId.Text = _clientId;
            IsEnableSignInButton = ClientId.Text.Trim().Length > 0;
        }

        private void GraphService_IsAuthenticatedChanged(object sender, System.EventArgs e)
        {
            IsEnabled = !_graphService.IsAuthenticated;

            if (_graphService.IsAuthenticated && _scopesForReAuth != null)
            {
                _scopes = _scopesForReAuth;
            }
        }

        private void ClientId_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(ClientId.Text.Trim()))
            {
                IsEnableSignInButton = false;
                return;
            }

            _clientId = ClientId.Text.Trim();

            _graphService.AuthenticationModel = MicrosoftGraphEnums.AuthenticationModel.V2;
            _graphService.Initialize(_clientId, MicrosoftGraphEnums.ServicesToInitialize.UserProfile, _scopesForReAuth ?? _scopes);

            IsEnableSignInButton = true;
        }

        private void GraphService_SignInFailed(object sender, Toolkit.Services.MicrosoftGraph.SignInFailedEventArgs e)
        {
            Shell.Current.ShowExceptionNotification(e.Exception);
        }

        private string[] GetAdminPermissions()
        {
            var page = this.FindAscendant<Page>();

            var plannerCtl = page.FindDescendant<PlannerTaskList>();
            if (plannerCtl != null)
            {
                return PlannerTaskList.RequiredDelegatedPermissions;
            }

            return new string[0];
        }
    }
}
