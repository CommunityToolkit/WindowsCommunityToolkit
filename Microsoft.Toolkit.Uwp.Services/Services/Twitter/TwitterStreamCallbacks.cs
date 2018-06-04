// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Microsoft.Toolkit.Uwp.Services.Twitter
{
    /// <summary>
    /// Callbacks used for Twitter streams.
    /// </summary>
    public class TwitterStreamCallbacks
    {
        /// <summary>
        /// Callback converting json to Tweet
        /// </summary>
        /// <param name="json">Raw Json from Twitter API</param>
        public delegate void RawJsonCallback(string json);

        /// <summary>
        /// Callback returning the parsed tweet
        /// </summary>
        /// <param name="tweet">Strongly typed tweet</param>
        public delegate void TwitterStreamCallback(ITwitterResult tweet);
    }
}
