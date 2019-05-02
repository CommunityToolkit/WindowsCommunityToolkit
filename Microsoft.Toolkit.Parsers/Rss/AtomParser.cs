// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Xml.Linq;
using Microsoft.Toolkit.Extensions;

namespace Microsoft.Toolkit.Parsers.Rss
{
    /// <summary>
    /// Parser for Atom endpoints.
    /// </summary>
    internal class AtomParser : BaseRssParser
    {
        /// <summary>
        /// Atom reader implementation to parse Atom content.
        /// </summary>
        /// <param name="doc">XDocument to parse.</param>
        /// <returns>Strong typed response.</returns>
        public override IEnumerable<RssSchema> LoadFeed(XDocument doc)
        {
            Collection<RssSchema> feed = new Collection<RssSchema>();

            if (doc.Root == null)
            {
                return feed;
            }

            var items = doc.Root.Elements(doc.Root.GetDefaultNamespace() + "entry").Select(item => GetRssSchema(item)).ToList<RssSchema>();

            feed = new Collection<RssSchema>(items);

            return feed;
        }

        /// <summary>
        /// Retieves strong type for passed item.
        /// </summary>
        /// <param name="item">XElement to parse.</param>
        /// <returns>Strong typed object.</returns>
        private static RssSchema GetRssSchema(XElement item)
        {
            RssSchema rssItem = new RssSchema
            {
                Author = GetItemAuthor(item),
                Title = item.GetSafeElementString("title").Trim().DecodeHtml(),
                ImageUrl = GetItemImage(item),
                PublishDate = item.GetSafeElementDate("published"),
                FeedUrl = item.GetLink("alternate"),
            };

            var content = GetItemContent(item);

            // Removes scripts from html
            if (!string.IsNullOrEmpty(content))
            {
                rssItem.Summary = ProcessHtmlSummary(content);
                rssItem.Content = ProcessHtmlContent(content);
            }

            string id = item.GetSafeElementString("guid").Trim();
            if (string.IsNullOrEmpty(id))
            {
                id = item.GetSafeElementString("id").Trim();
                if (string.IsNullOrEmpty(id))
                {
                    id = rssItem.FeedUrl;
                }
            }

            rssItem.InternalID = id;
            return rssItem;
        }

        /// <summary>
        /// Retrieves item author from XElement.
        /// </summary>
        /// <param name="item">XElement item.</param>
        /// <returns>String of Item Author.</returns>
        private static string GetItemAuthor(XElement item)
        {
            var content = string.Empty;

            if (item != null && item.Element(item.GetDefaultNamespace() + "author") != null)
            {
                content = item.Element(item.GetDefaultNamespace() + "author").GetSafeElementString("name");
            }

            if (string.IsNullOrEmpty(content))
            {
                content = item.GetSafeElementString("author");
            }

            return content;
        }

        /// <summary>
        /// Returns item image from XElement item.
        /// </summary>
        /// <param name="item">XElement item.</param>
        /// <returns>String pointing to item image.</returns>
        private static string GetItemImage(XElement item)
        {
            if (!string.IsNullOrEmpty(item.GetSafeElementString("image")))
            {
                return item.GetSafeElementString("image");
            }

            return item.GetImage();
        }

        /// <summary>
        /// Returns item content from XElement item.
        /// </summary>
        /// <param name="item">XElement item.</param>
        /// <returns>String of item content.</returns>
        private static string GetItemContent(XElement item)
        {
            var content = item.GetSafeElementString("description");
            if (string.IsNullOrEmpty(content))
            {
                content = item.GetSafeElementString("content");
            }

            if (string.IsNullOrEmpty(content))
            {
                content = item.GetSafeElementString("summary");
            }

            return content;
        }
    }
}