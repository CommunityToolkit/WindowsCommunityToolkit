// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Microsoft.Toolkit.Services.Core
{
    /// <summary>
    /// AuthenticationResult class, parameters: ResponseErrorDetail(uint), ResponseData(string) and ResponseStatus(AuthenticationResultStatus)
    /// </summary>
    public class AuthenticationResult
    {
        /// <summary>
        /// Gets or sets the authentication error detail
        /// </summary>
        public uint ResponseErrorDetail { get; set; }

        /// <summary>
        /// Gets or sets the authentication result data
        /// </summary>
        public string ResponseData { get; set; }

        /// <summary>
        /// Gets or sets the authentication status, could be UserCancel, ErrorHttp and Success.
        /// </summary>
        public AuthenticationResultStatus ResponseStatus { get; set; }
    }
}
