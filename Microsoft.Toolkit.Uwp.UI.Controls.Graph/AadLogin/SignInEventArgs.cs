// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.Graph;

namespace Microsoft.Toolkit.Uwp.UI.Controls.Graph
{
    /// <summary>
    /// Arguments relating to a sign-in event of Aadlogin control
    /// </summary>
    public class SignInEventArgs
    {
        internal SignInEventArgs()
        {
        }

        /// <summary>
        /// Gets the graph service client with authorized token.
        /// </summary>
        public GraphServiceClient GraphClient
        {
            get; internal set;
        }
    }
}
