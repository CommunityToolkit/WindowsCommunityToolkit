// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Threading.Tasks;
using Microsoft.Toolkit.Services.Core;
using Windows.Security.Authentication.Web;

namespace Microsoft.Toolkit.Services.PlatformSpecific.Uwp
{
    /// <summary>
    /// Authentication Broker
    /// </summary>
    internal class UwpAuthenticationBroker : IAuthenticationBroker
    {
        /// <summary>
        /// Authentication process
        /// </summary>
        /// <param name="requestUri"> Request Uri</param>
        /// <param name="callbackUri"> Uri result</param>
        /// <returns> Returns login status</returns>
        public async Task<AuthenticationResult> Authenticate(Uri requestUri, Uri callbackUri)
        {
            WebAuthenticationResult result = await WebAuthenticationBroker.AuthenticateAsync(
                WebAuthenticationOptions.None,
                requestUri,
                callbackUri);

            switch (result.ResponseStatus)
            {
                case WebAuthenticationStatus.Success:
                    return new AuthenticationResult { ResponseData = result.ResponseData, ResponseStatus = AuthenticationResultStatus.Success };
                case WebAuthenticationStatus.UserCancel:
                    return new AuthenticationResult { ResponseData = result.ResponseData, ResponseStatus = AuthenticationResultStatus.UserCancel, ResponseErrorDetail = result.ResponseErrorDetail };
                case WebAuthenticationStatus.ErrorHttp:
                    return new AuthenticationResult { ResponseData = result.ResponseData, ResponseStatus = AuthenticationResultStatus.ErrorHttp, ResponseErrorDetail = result.ResponseErrorDetail };
                default:
                    // TODO: Change with correct name;
                    throw new ArgumentException("error");
            }
        }
    }
}
