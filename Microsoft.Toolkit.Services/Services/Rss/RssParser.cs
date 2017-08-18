// ******************************************************************
// Copyright (c) Microsoft. All rights reserved.
// This code is licensed under the MIT License (MIT).
// THE CODE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
// TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH
// THE CODE OR THE USE OR OTHER DEALINGS IN THE CODE.
// ******************************************************************

using System.Collections.Generic;
using System.Xml.Linq;

namespace Microsoft.Toolkit.Services.Rss
{
    /// <summary>
    /// RssParser.
    /// </summary>
    internal class RssParser : IParser<RssSchema>
    {
        /// <summary>
        /// Parse string to strong type.
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
