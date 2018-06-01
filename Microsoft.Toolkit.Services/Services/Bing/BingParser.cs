// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Linq;
using Microsoft.Toolkit.Parsers.Rss;

namespace Microsoft.Toolkit.Services.Bing
{
    /// <summary>
    /// Parse Bing results into strong type.
    /// </summary>
    public class BingParser : Parsers.IParser<BingResult>
    {
        /// <summary>
        /// Take string data and parse into strong data type.
        /// </summary>
        /// <param name="data">String data.</param>
        /// <returns>Returns strong type.</returns>
        public IEnumerable<BingResult> Parse(string data)
        {
            if (string.IsNullOrEmpty(data))
            {
                return null;
            }

            RssParser rssParser = new RssParser();
            IEnumerable<RssSchema> syndicationItems = rssParser.Parse(data);
            return from r in syndicationItems
                   select new BingResult
                   {
                       InternalID = r.InternalID,
                       Title = r.Title,
                       Summary = r.Summary,
                       Link = r.FeedUrl,
                       Published = r.PublishDate
                   };
        }
    }
}