// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Toolkit.Services.Weibo;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

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

            JObject rawObject = JObject.Parse(data);

            IList<JToken> rawStatuses = rawObject["statuses"].Children().ToList();

            IList<WeiboStatus> statuses = new List<WeiboStatus>();
            foreach (JToken result in rawStatuses)
            {
                WeiboStatus searchResult = result.ToObject<WeiboStatus>();
                statuses.Add(searchResult);
            }

            return statuses;
        }
    }
}
