// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace Microsoft.Toolkit.Uwp.Services.Twitter
{
    /// <summary>
    /// Twitter Search Parser.
    /// </summary>
    public class TwitterSearchParser : Parsers.IParser<Tweet>
    {
        /// <summary>
        /// Parse string into strong typed list.
        /// </summary>
        /// <param name="data">Input string.</param>
        /// <returns>Strong typed list.</returns>
        public IEnumerable<Tweet> Parse(string data)
        {
            if (string.IsNullOrEmpty(data))
            {
                return null;
            }

            var result = JsonConvert.DeserializeObject<TwitterSearchResult>(data);

            return result.Statuses.ToList();
        }
    }
}
