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

            ClientId.Text = string.Empty;

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

            _graphService.AuthenticationModel = MicrosoftGraphEnums.AuthenticationModel.V2;
            _graphService.Initialize(ClientId.Text.Trim(), MicrosoftGraphEnums.ServicesToInitialize.UserProfile, _scopes);

            IsEnableSignInButton = true;
        }
    }
}
