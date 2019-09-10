// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.Toolkit.Services.Core;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Microsoft.Toolkit.Services.Weibo
{
    /// <summary>
    /// Weibo Oauth request extensions to add utilities for internal use.
    /// </summary>
    internal static class WeiboOAuthRequestExtensions
    {
        public static void ThrowIfNotValid(this HttpResponseMessage response)
        {
            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                throw new UserNotFoundException();
            }

            if ((int)response.StatusCode == 429)
            {
                throw new TooManyRequestsException();
            }

            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                throw new OAuthKeysRevokedException();
            }
        }
    }
}
