using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Microsoft.Toolkit.Uwp.UI.Controls.Graph
{
    /// <summary>
    /// Defines the events for the <see cref="AADLogin"/> control.
    /// </summary>
    public partial class AADLogin : Control
    {
        /// <summary>
        /// Occurs when the user is logged in.
        /// </summary>
        public event EventHandler<SignInEventArgs> SignInCompleted;

        /// <summary>
        /// Occurs when the user is logged out.
        /// </summary>
        public event EventHandler SignOutCompleted;

        /// <summary>
        /// Occurs when ClientId or Scopes property value changed
        /// </summary>
        /// <param name="obj">AADLogin</param>
        /// <param name="args">Property Changed Args</param>
        private static void OnPropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args) => (obj as AADLogin).InitialPublicClientApplication();

        private void OnSignInCompleted(SignInEventArgs e)
        {
            SignInCompleted?.Invoke(this, e);
        }

        private void OnSignOutCompleted()
        {
            SignOutCompleted?.Invoke(this, EventArgs.Empty);
        }
    }
}
