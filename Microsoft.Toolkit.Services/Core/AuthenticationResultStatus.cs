// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Microsoft.Toolkit.Services.Core
{
    /// <summary>
    /// Contains the status of the authentication operation
    /// </summary>
    public enum AuthenticationResultStatus
    {
        /// <summary>
        /// The operation succeeded, and the response data is available.
        /// </summary>
        Success,

        /// <summary>
        /// The operation was canceled by the user
        /// </summary>
        UserCancel,

        /// <summary>
        /// The operation failed because a specific HTTP error was returned, for example 404
        /// </summary>
        ErrorHttp
    }
}
