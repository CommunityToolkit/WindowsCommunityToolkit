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
using Microsoft.Toolkit.Uwp.UI.Controls.Graph;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Microsoft.Toolkit.Uwp.SampleApp.Controls
{
    public sealed partial class AadAuthControl : UserControl
    {
        public static readonly DependencyProperty IsShowAadMetaDataControlProperty = DependencyProperty.Register(
            nameof(IsShowAadMetaDataControl),
            typeof(Visibility),
            typeof(AadAuthControl),
            new PropertyMetadata(Visibility.Visible));

        public Visibility IsShowAadMetaDataControl
        {
            get { return (Visibility)GetValue(IsShowAadMetaDataControlProperty); }
            private set { SetValue(IsShowAadMetaDataControlProperty, value); }
        }

        public static readonly DependencyProperty IsShowGraphControlProperty = DependencyProperty.Register(
            nameof(IsShowGraphControl),
            typeof(Visibility),
            typeof(AadAuthControl),
            new PropertyMetadata(Visibility.Collapsed));

        public Visibility IsShowGraphControl
        {
            get { return (Visibility)GetValue(IsShowGraphControlProperty); }
            private set { SetValue(IsShowGraphControlProperty, value); }
        }

        public static readonly DependencyProperty IsShowSignInButtonProperty = DependencyProperty.Register(
            nameof(IsShowSignInButton),
            typeof(Visibility),
            typeof(AadAuthControl),
            new PropertyMetadata(Visibility.Visible));

        public Visibility IsShowSignInButton
        {
            get { return (Visibility)GetValue(IsShowSignInButtonProperty); }
            set { SetValue(IsShowSignInButtonProperty, value); }
        }

        public static readonly DependencyProperty IsEnableSignInButtonProperty = DependencyProperty.Register(
            nameof(IsEnableSignInButton),
            typeof(bool),
            typeof(AadAuthControl),
            new PropertyMetadata(false));

        public bool IsEnableSignInButton
        {
            get { return (bool)GetValue(IsEnableSignInButtonProperty); }
            set { SetValue(IsEnableSignInButtonProperty, value); }
        }

        private AadAuthenticationManager _aadAuthenticationManager = AadAuthenticationManager.Instance;

        public AadAuthControl()
        {
            InitializeComponent();

            _aadAuthenticationManager.PropertyChanged += AadAuthenticationManager_PropertyChanged;
            ClientId.Text = _aadAuthenticationManager.ClientId ?? string.Empty;

            ClientId.TextChanged += (object sender, TextChangedEventArgs e) =>
            {
                if (string.IsNullOrEmpty(ClientId.Text.Trim()))
                {
                    IsEnableSignInButton = false;
                    return;
                }

                _aadAuthenticationManager.Initialize(
                    ClientId.Text.Trim(),
                    AadLogin.RequiredDelegatedPermissions
                        .Concat(ProfileCard.RequiredDelegatedPermissions)
                        .Concat(PeoplePicker.RequiredDelegatedPermissions)
                        .Concat(SharePointFileList.RequiredDelegatedPermissions)
                        .ToArray());

                IsEnableSignInButton = true;
            };

            Scopes.Text = string.Join(", ", AadLogin.RequiredDelegatedPermissions
                .Union(ProfileCard.RequiredDelegatedPermissions)
                .Union(PeoplePicker.RequiredDelegatedPermissions)
                .Union(SharePointFileList.RequiredDelegatedPermissions)
                .Distinct());

            IsEnableSignInButton = ClientId.Text.Trim().Length > 0;

            RefreshControlVisibility();
        }

        private void AadAuthenticationManager_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(_aadAuthenticationManager.IsAuthenticated))
            {
                RefreshControlVisibility();

                if (_aadAuthenticationManager.IsAuthenticated)
                {
                    ClientId.IsReadOnly = true;
                }
                else
                {
                    ClientId.IsReadOnly = false;
                }
            }
        }

        private void RefreshControlVisibility()
        {
            if (_aadAuthenticationManager.IsAuthenticated)
            {
                IsShowAadMetaDataControl = Visibility.Collapsed;
                IsShowGraphControl = Visibility.Visible;
            }
            else
            {
                IsShowAadMetaDataControl = Visibility.Visible;
                IsShowGraphControl = Visibility.Collapsed;
            }
        }
    }
}
