using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;

namespace Microsoft.Toolkit.Uwp.UI.Controls.Graph
{
    /// <summary>
    /// Defines the properties for the <see cref="AADLogin"/> control.
    /// </summary>
    public partial class AADLogin : Control
    {
        /// <summary>
        /// Identifies the <see cref="ClientId"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ClientIdProperty = DependencyProperty.Register(
            nameof(ClientId),
            typeof(string),
            typeof(AADLogin),
            new PropertyMetadata(string.Empty)
        );

        /// <summary>
        /// Identifies the <see cref="Scopes"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ScopesProperty = DependencyProperty.Register(
            nameof(Scopes),
            typeof(string),
            typeof(AADLogin),
            new PropertyMetadata(string.Empty)
        );

        /// <summary>
        /// Identifies the <see cref="DefaultImage"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty DefaultImageProperty = DependencyProperty.Register(
            nameof(DefaultImage),
            typeof(BitmapImage),
            typeof(AADLogin),
            new PropertyMetadata(null)
        );

        /// <summary>
        /// Identifies the <see cref="View"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ViewProperty = DependencyProperty.Register(
            nameof(View),
            typeof(ViewType),
            typeof(AADLogin),
            new PropertyMetadata(ViewType.PictureOnly)
        );

        /// <summary>
        /// Identifies the <see cref="AllowSignInAsDifferentUser"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty AllowSignInAsDifferentUserProperty = DependencyProperty.Register(
            nameof(AllowSignInAsDifferentUser),
            typeof(bool),
            typeof(AADLogin),
            new PropertyMetadata(true)
        );

        /// <summary>
        /// Identifies the <see cref="SignInDefaultText"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty SignInDefaultTextProperty = DependencyProperty.Register(
            nameof(SignInDefaultText),
            typeof(string),
            typeof(AADLogin),
            new PropertyMetadata("Sign In")
        );

        /// <summary>
        /// Identifies the <see cref="SignOutDefaultText"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty SignOutDefaultTextProperty = DependencyProperty.Register(
            nameof(SignOutDefaultText),
            typeof(string),
            typeof(AADLogin),
            new PropertyMetadata("Sign Out")
        );

        /// <summary>
        /// Identifies the <see cref="SignInAnotherUserDefaultText"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty SignInAnotherUserDefaultTextProperty = DependencyProperty.Register(
            nameof(SignInAnotherUserDefaultText),
            typeof(string),
            typeof(AADLogin),
            new PropertyMetadata("Sign in with another account")
        );

        internal static readonly DependencyProperty _graphAccessTokenProperty = DependencyProperty.Register(
            nameof(_graphAccessToken),
            typeof(string),
            typeof(AADLogin),
            new PropertyMetadata(null)
        );

        internal static readonly DependencyProperty _currentUserIdProperty = DependencyProperty.Register(
            nameof(_currentUserID),
            typeof(string),
            typeof(AADLogin),
            new PropertyMetadata(null)
        );

        /// <summary>
        /// 
        /// </summary>
        public string ClientId
        {
            get
            {
                return (string)GetValue(ClientIdProperty);
            }
            set
            {
                if (string.IsNullOrEmpty(ClientId))
                {
                    SetValue(ClientIdProperty, value);
                    InitialPublicClientApplication();
                }
                else
                    throw new ArgumentException("The Client Id field only allow be set once.");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string Scopes
        {
            get
            {
                return (string)GetValue(ScopesProperty);
            }
            set
            {
                if (string.IsNullOrEmpty(Scopes))
                {
                    SetValue(ScopesProperty, value);
                    InitialPublicClientApplication();
                }
                else
                    throw new ArgumentException("The Scopes field only allow be set once.");
            }
        }

        /// <summary>
        /// Gets or sets the default user photo
        /// </summary>
        public BitmapImage DefaultImage
        {
            get
            {
                return (BitmapImage)GetValue(DefaultImageProperty);
            }
            set
            {
                SetValue(DefaultImageProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating which view type will be presented, the default value is PictureOnly.
        /// </summary>
        public ViewType View
        {
            get
            {
                return (ViewType)GetValue(ViewProperty);
            }
            set
            {
                SetValue(ViewProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether AllowSignInAsDifferentUser menu button is enabled for logged in user.
        /// </summary>
        public bool AllowSignInAsDifferentUser
        {
            get
            {
                return (bool)GetValue(AllowSignInAsDifferentUserProperty);
            }
            set
            {
                SetValue(AllowSignInAsDifferentUserProperty, value);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string SignInDefaultText
        {
            get
            {
                return (string)GetValue(SignInDefaultTextProperty);
            }
            set
            {
                SetValue(SignInDefaultTextProperty, value);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string SignOutDefaultText
        {
            get
            {
                return (string)GetValue(SignOutDefaultTextProperty);
            }
            set
            {
                SetValue(SignOutDefaultTextProperty, value);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string SignInAnotherUserDefaultText
        {
            get
            {
                return (string)GetValue(SignInAnotherUserDefaultTextProperty);
            }
            set
            {
                SetValue(SignInAnotherUserDefaultTextProperty, value);
            }
        }

        internal string _currentUserID
        {
            get
            {
                return (string)GetValue(_currentUserIdProperty);
            }
            private set
            {
                SetValue(_currentUserIdProperty, value);
            }
        }

        internal string _graphAccessToken
        {
            get
            {
                return (string)GetValue(_graphAccessTokenProperty);
            }
            private set
            {
                SetValue(_graphAccessTokenProperty, value);
            }
        }
    }
}
