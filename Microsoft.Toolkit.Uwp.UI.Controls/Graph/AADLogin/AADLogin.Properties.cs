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
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;

namespace Microsoft.Toolkit.Uwp.UI.Controls.Graph
{
    /// <summary>
    /// Defines the properties for the <see cref="AadLogin"/> control.
    /// </summary>
    public partial class AadLogin : Button
    {
        /// <summary>
        /// Gets required delegated permissions for the <see cref="AadLogin"/> control
        /// </summary>
        public static string[] RequiredDelegatedPermissions
        {
            get
            {
                return new string[] { "User.Read" };
            }
        }

        /// <summary>
        /// Identifies the <see cref="DefaultImage"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty DefaultImageProperty = DependencyProperty.Register(
            nameof(DefaultImage),
            typeof(BitmapImage),
            typeof(AadLogin),
            new PropertyMetadata(null));

        /// <summary>
        /// Identifies the <see cref="View"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ViewProperty = DependencyProperty.Register(
            nameof(View),
            typeof(ViewType),
            typeof(AadLogin),
            new PropertyMetadata(ViewType.PictureOnly));

        /// <summary>
        /// Identifies the <see cref="AllowSignInAsDifferentUser"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty AllowSignInAsDifferentUserProperty = DependencyProperty.Register(
            nameof(AllowSignInAsDifferentUser),
            typeof(bool),
            typeof(AadLogin),
            new PropertyMetadata(true));

        /// <summary>
        /// Identifies the <see cref="SignInDefaultText"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty SignInDefaultTextProperty = DependencyProperty.Register(
            nameof(SignInDefaultText),
            typeof(string),
            typeof(AadLogin),
            new PropertyMetadata("Sign In"));

        /// <summary>
        /// Identifies the <see cref="SignOutDefaultText"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty SignOutDefaultTextProperty = DependencyProperty.Register(
            nameof(SignOutDefaultText),
            typeof(string),
            typeof(AadLogin),
            new PropertyMetadata("Sign Out"));

        /// <summary>
        /// Identifies the <see cref="SignInAnotherUserDefaultText"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty SignInAnotherUserDefaultTextProperty = DependencyProperty.Register(
            nameof(SignInAnotherUserDefaultText),
            typeof(string),
            typeof(AadLogin),
            new PropertyMetadata("Sign in with another account"));

        /// <summary>
        /// Identifies the <see cref="CurrentUserID"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty CurrentUserIdProperty = DependencyProperty.Register(
            nameof(CurrentUserID),
            typeof(string),
            typeof(AadLogin),
            new PropertyMetadata(null));

        /// <summary>
        /// Gets or sets the default user photo
        /// </summary>
        public BitmapImage DefaultImage
        {
            get { return (BitmapImage)GetValue(DefaultImageProperty); }
            set { SetValue(DefaultImageProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating which view type will be presented, the default value is PictureOnly.
        /// </summary>
        public ViewType View
        {
            get { return (ViewType)GetValue(ViewProperty); }
            set { SetValue(ViewProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether AllowSignInAsDifferentUser menu button is enabled for logged in user.
        /// </summary>
        public bool AllowSignInAsDifferentUser
        {
            get { return (bool)GetValue(AllowSignInAsDifferentUserProperty); }
            set { SetValue(AllowSignInAsDifferentUserProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value for default sign-in text.
        /// </summary>
        public string SignInDefaultText
        {
            get { return (string)GetValue(SignInDefaultTextProperty); }
            set { SetValue(SignInDefaultTextProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value for default sign-out text.
        /// </summary>
        public string SignOutDefaultText
        {
            get { return (string)GetValue(SignOutDefaultTextProperty); }
            set { SetValue(SignOutDefaultTextProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value for default text of the Sign-in-with-another-account button.
        /// </summary>
        public string SignInAnotherUserDefaultText
        {
            get { return (string)GetValue(SignInAnotherUserDefaultTextProperty); }
            set { SetValue(SignInAnotherUserDefaultTextProperty, value); }
        }

        /// <summary>
        /// Gets the unique identifier for current signed in user.
        /// </summary>
        public string CurrentUserID
        {
            get { return (string)GetValue(CurrentUserIdProperty); }
            private set { SetValue(CurrentUserIdProperty, value); }
        }
    }
}
