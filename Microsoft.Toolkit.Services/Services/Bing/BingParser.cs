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

using Microsoft.Toolkit.Services.Rss;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.Toolkit.Services.Bing
{
    /// <summary>
    /// Parse Bing results into strong type.
    /// </summary>
    public class BingParser : IParser<BingResult>
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
