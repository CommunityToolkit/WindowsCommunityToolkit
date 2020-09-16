// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Text.Json;

namespace Microsoft.Toolkit.Services.Weibo
{
    /// <summary>
    /// Weibo Timeline Parser.
    /// </summary>
    public class WeiboStatusParser : Parsers.IParser<WeiboStatus>
    {
        /// <summary>
        /// Parse string data into strongly typed list.
        /// </summary>
        /// <param name="data">Input string.</param>
        /// <returns>List of strongly typed objects.</returns>
        public IEnumerable<WeiboStatus> Parse(string data)
        {
            if (string.IsNullOrEmpty(data))
            {
                return null;
            }

            var rawObject = JsonDocument.Parse(data);

            var rawStatuses = rawObject.RootElement.GetProperty("statuses");

            IList<WeiboStatus> statuses = new List<WeiboStatus>();
            foreach (var rawStatus in rawStatuses.EnumerateArray())
            {
                WeiboStatus searchResult = JsonSerializer.Deserialize<WeiboStatus>(rawStatus.ToString());
                statuses.Add(searchResult);
            }

            return statuses;
        }
    }
}
