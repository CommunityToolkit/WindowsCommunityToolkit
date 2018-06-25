// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.Toolkit.Services.Twitter;

namespace Microsoft.Toolkit.Uwp.Services
{
    /// <summary>
    /// Twitter service extensions to use for Uwp
    /// </summary>
    public static class TwitterServiceExtensions
    {
        /// <summary>
        /// Initialize underlying provider with relevent token information for Uwp.
        /// </summary>
        /// <param name="service">The TwitterService</param>
        /// <param name="consumerKey">Consumer key.</param>
        /// <param name="consumerSecret">Consumer secret.</param>
        /// <param name="callbackUri">Callback URI. Has to match callback URI defined at apps.twitter.com (can be arbitrary).</param>
        /// <returns>Success or failure.</returns>
        public static bool Initialize(this TwitterService service, string consumerKey, string consumerSecret, string callbackUri)
        {
            return service.Initialize(consumerKey, consumerSecret, callbackUri, new UwpAuthenticationBroker(), new UWpPasswordManager(), new UwpStorageManager(), new UwpSignatureManager());
        }
    }
}
