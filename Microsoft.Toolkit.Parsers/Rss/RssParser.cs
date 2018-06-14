// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Xml.Linq;

namespace Microsoft.Toolkit.Parsers.Rss
{
    /// <summary>
    /// The RSS Parser allows you to parse an RSS content String into RSS Schema.
    /// </summary>
    public class RssParser : IParser<RssSchema>
    {
        /// <summary>
        /// Parse an RSS content string into RSS Schema.
        /// </summary>
        /// <param name="data">Input string.</param>
        /// <returns>Strong type.</returns>
        public IEnumerable<RssSchema> Parse(string data)
        {
            if (string.IsNullOrEmpty(data))
            {
                return null;
            }

            var doc = XDocument.Parse(data);
            var type = BaseRssParser.GetFeedType(doc);

            BaseRssParser rssParser;
            if (type == RssType.Rss)
            {
                rssParser = new Rss2Parser();
            }
            else
            {
                rssParser = new AtomParser();
            }

            return rssParser.LoadFeed(doc);
        }
    }
}