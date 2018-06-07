// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Linq;
using Microsoft.Toolkit.Services.MicrosoftGraph;
using Microsoft.Toolkit.Uwp.UI.Controls.Graph;
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

        private readonly string[] _scopes = AadLogin.RequiredDelegatedPermissions
                .Union(ProfileCard.RequiredDelegatedPermissions)
                .Union(PeoplePicker.RequiredDelegatedPermissions)
                .Union(SharePointFileList.RequiredDelegatedPermissions)
                .Distinct()
                .ToArray();

        private MicrosoftGraphService _graphService = MicrosoftGraphService.Instance;

        public AadAuthControl()
        {
            InitializeComponent();

            ClientId.TextChanged += ClientId_TextChanged;

            ClientId.Text = _clientId;

            Scopes.Text = string.Join(", ", _scopes);

            IsEnableSignInButton = ClientId.Text.Trim().Length > 0;

            _graphService.IsAuthenticatedChanged += GraphService_IsAuthenticatedChanged;
            IsEnabled = !_graphService.IsAuthenticated;
        }

        private void GraphService_IsAuthenticatedChanged(object sender, System.EventArgs e)
        {
            IsEnabled = !_graphService.IsAuthenticated;
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
            _graphService.Initialize(_clientId, MicrosoftGraphEnums.ServicesToInitialize.UserProfile, _scopes);

            IsEnableSignInButton = true;
        }

        private void AadLogin_SignInFailed(object sender, SignInFailedEventArgs e)
        {
            SampleController.Current.ShowExceptionNotification(e.Exception);
        }
    }
}
