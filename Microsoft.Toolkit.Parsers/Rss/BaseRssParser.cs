// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Xml.Linq;
using Microsoft.Toolkit.Extensions;

namespace Microsoft.Toolkit.Parsers.Rss
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