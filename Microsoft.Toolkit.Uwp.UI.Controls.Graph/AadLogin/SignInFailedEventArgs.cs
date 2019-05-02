// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;

namespace Microsoft.Toolkit.Uwp.UI.Controls.Graph
{
    /// <summary>
    /// Arguments relating to a sign-in event of Aadlogin control
    /// </summary>
    public class SignInFailedEventArgs
    {
        internal SignInFailedEventArgs(Exception exception)
        {
            Exception = exception;
        }

        /// <summary>
        /// Gets the exception thrown by the Microsoft Graph when attempting to sign in
        /// </summary>
        public Exception Exception { get; private set; }
    }
}
