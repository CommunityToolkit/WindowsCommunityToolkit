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
using Microsoft.Toolkit.Services.Core;

namespace Microsoft.Toolkit.Services.Rss
{
    /// <summary>
    /// Base class for Rss Parser(s).
    /// </summary>
    internal abstract class BaseRssParser
    {
        /// <summary>
        /// Retrieve feed type from XDocument.
        /// </summary>
        /// <param name="doc">XDocument doc.</param>
        /// <returns>Return feed type.</returns>
        public static RssType GetFeedType(XDocument doc)
        {
            if (doc.Root == null)
            {
                return RssType.Unknown;
            }

            XNamespace defaultNamespace = doc.Root.GetDefaultNamespace();
            return defaultNamespace.NamespaceName.EndsWith("Atom") ? RssType.Atom : RssType.Rss;
        }

        /// <summary>
        /// Abstract method to be override by specific implementations of the reader.
        /// </summary>
        /// <param name="doc">XDocument doc.</param>
        /// <returns>Returns list of strongly typed results.</returns>
        public abstract IEnumerable<RssSchema> LoadFeed(XDocument doc);

        /// <summary>
        /// Fix up the HTML content.
        /// </summary>
        /// <param name="htmlContent">Content to be fixed up.</param>
        /// <returns>Fixed up content.</returns>
        protected internal static string ProcessHtmlContent(string htmlContent)
        {
            return htmlContent.FixHtml().SanitizeString();
        }

        /// <summary>
        /// Create a summary of the HTML content.
        /// </summary>
        /// <param name="htmlContent">Content to be processed.</param>
        /// <returns>Summary of the content.</returns>
        protected internal static string ProcessHtmlSummary(string htmlContent)
        {
            return htmlContent.DecodeHtml().Trim().Truncate(500).SanitizeString();
        }
    }
}
