using System;
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
