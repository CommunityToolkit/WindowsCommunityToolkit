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
    /// Rss reader implementation to parse Rss content.
    /// </summary>
    internal class Rss2Parser : BaseRssParser
    {
        /// <summary>
        /// RDF Namespace Uri.
        /// </summary>
        private static readonly XNamespace NsRdfNamespaceUri = "http://www.w3.org/1999/02/22-rdf-syntax-ns#";

        /// <summary>
        /// RDF Elements Namespace Uri.
        /// </summary>
        private static readonly XNamespace NsRdfElementsNamespaceUri = "http://purl.org/dc/elements/1.1/";

        /// <summary>
        /// RDF Content Namespace Uri.
        /// </summary>
        private static readonly XNamespace NsRdfContentNamespaceUri = "http://purl.org/rss/1.0/modules/content/";

        /// <summary>
        /// This override load and parses the document and return a list of RssSchema values.
        /// </summary>
        /// <param name="doc">XDocument to be loaded.</param>
        /// <returns>Strongly typed list of feeds.</returns>
        public override IEnumerable<RssSchema> LoadFeed(XDocument doc)
        {
            bool isRDF = false;
            var feed = new Collection<RssSchema>();
            XNamespace defaultNamespace = string.Empty;

            if (doc.Root != null)
            {
                isRDF = doc.Root.Name == (NsRdfNamespaceUri + "RDF");
                defaultNamespace = doc.Root.GetDefaultNamespace();
            }

            foreach (var item in doc.Descendants(defaultNamespace + "item"))
            {
                var rssItem = isRDF ? ParseRDFItem(item) : ParseRssItem(item);
                feed.Add(rssItem);
            }

            return feed;
        }

        /// <summary>
        /// Parses XElement item into strong typed object.
        /// </summary>
        /// <param name="item">XElement item to parse.</param>
        /// <returns>Strong typed object.</returns>
        private static RssSchema ParseItem(XElement item)
        {
            var rssItem = new RssSchema();
            rssItem.Title = item.GetSafeElementString("title").Trim().DecodeHtml();
            rssItem.FeedUrl = item.GetSafeElementString("link");

            rssItem.Author = GetItemAuthor(item);

            string content = item.GetSafeElementString("encoded", NsRdfContentNamespaceUri);
            if (string.IsNullOrEmpty(content))
            {
                content = item.GetSafeElementString("description");
                if (string.IsNullOrEmpty(content))
                {
                    content = item.GetSafeElementString("content");
                }
            }

            var summary = item.GetSafeElementString("description");
            if (string.IsNullOrEmpty(summary))
            {
                summary = item.GetSafeElementString("encoded", NsRdfContentNamespaceUri);
            }

            // Removes scripts from html
            if (!string.IsNullOrEmpty(summary))
            {
                rssItem.Summary = ProcessHtmlSummary(summary);
            }

            if (!string.IsNullOrEmpty(content))
            {
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
        /// Parses RSS version 1.0 objects.
        /// </summary>
        /// <param name="item">XElement item.</param>
        /// <returns>Strong typed object.</returns>
        private static RssSchema ParseRDFItem(XElement item)
        {
            XNamespace ns = "http://search.yahoo.com/mrss/";
            var rssItem = ParseItem(item);

            rssItem.PublishDate = item.GetSafeElementDate("date", NsRdfElementsNamespaceUri);

            string image = item.GetSafeElementString("image");
            if (string.IsNullOrEmpty(image) && item.Elements(ns + "thumbnail").LastOrDefault() != null)
            {
                var element = item.Elements(ns + "thumbnail").Last();
                image = element.Attribute("url").Value;
            }

            if (string.IsNullOrEmpty(image) && item.ToString().Contains("thumbnail"))
            {
                image = item.GetSafeElementString("thumbnail");
            }

            if (string.IsNullOrEmpty(image))
            {
                image = item.GetImage();
            }

            rssItem.ImageUrl = image;

            return rssItem;
        }

        /// <summary>
        /// Parses RSS version 2.0 objects.
        /// </summary>
        /// <param name="item">XElement item.</param>
        /// <returns>Strong typed object.</returns>
        private static RssSchema ParseRssItem(XElement item)
        {
            XNamespace ns = "http://search.yahoo.com/mrss/";
            var rssItem = ParseItem(item);

            rssItem.PublishDate = item.GetSafeElementDate("pubDate");

            string image = item.GetSafeElementString("image");
            if (string.IsNullOrEmpty(image))
            {
                image = item.GetImageFromEnclosure();
            }

            if (string.IsNullOrEmpty(image) && item.Elements(ns + "content").LastOrDefault() != null)
            {
                var element = item.Elements(ns + "content").Last();
                if (element.Attribute("type") != null && element.Attribute("type").Value.Contains("image/"))
                {
                    image = element.Attribute("url").Value;
                }
            }

            if (string.IsNullOrEmpty(image) && item.Elements(ns + "thumbnail").LastOrDefault() != null)
            {
                var element = item.Elements(ns + "thumbnail").Last();
                image = element.Attribute("url").Value;
            }

            if (string.IsNullOrEmpty(image) && item.ToString().Contains("thumbnail"))
            {
                image = item.GetSafeElementString("thumbnail");
            }

            if (string.IsNullOrEmpty(image))
            {
                image = item.GetImage();
            }

            rssItem.Categories = item.GetSafeElementsString("category");

            rssItem.ImageUrl = image;

            return rssItem;
        }

        /// <summary>
        /// Retrieve item author from item.
        /// </summary>
        /// <param name="item">XElement item.</param>
        /// <returns>String of item author.</returns>
        private static string GetItemAuthor(XElement item)
        {
            var content = item.GetSafeElementString("creator", NsRdfElementsNamespaceUri).Trim();
            if (string.IsNullOrEmpty(content))
            {
                content = item.GetSafeElementString("author");
            }

            return content;
        }
    }
}