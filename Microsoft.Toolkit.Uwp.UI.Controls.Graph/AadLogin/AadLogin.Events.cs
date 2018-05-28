// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Windows.UI.Xaml.Controls;

namespace Microsoft.Toolkit.Uwp.UI.Controls.Graph
{
    /// <summary>
    /// Defines the events for the <see cref="AadLogin"/> control.
    /// </summary>
    public partial class AadLogin : Button
    {
        /// <summary>
        /// Occurs when the user is logged in.
        /// </summary>
        public event EventHandler<SignInEventArgs> SignInCompleted;

        /// <summary>
        /// Occurs when sign in failed when attempting to sign in
        /// </summary>
        public event EventHandler<SignInFailedEventArgs> SignInFailed;

        /// <summary>
        /// Occurs when the user is logged out.
        /// </summary>
        public event EventHandler SignOutCompleted;
    }
}
