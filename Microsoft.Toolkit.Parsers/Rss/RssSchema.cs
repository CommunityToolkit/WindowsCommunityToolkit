// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;

namespace Microsoft.Toolkit.Parsers.Rss
{
    /// <summary>
    /// Implementation of the RssSchema class.
    /// </summary>
    public class RssSchema : SchemaBase
    {
        /// <summary>
        /// Gets or sets title.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets summary.
        /// </summary>
        public string Summary { get; set; }

        /// <summary>
        /// Gets or sets content.
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// Gets or sets image Url.
        /// </summary>
        public string ImageUrl { get; set; }

        /// <summary>
        /// Gets or sets extra Image Url.
        /// </summary>
        public string ExtraImageUrl { get; set; }

        /// <summary>
        /// Gets or sets media Url.
        /// </summary>
        public string MediaUrl { get; set; }

        /// <summary>
        /// Gets or sets feed Url.
        /// </summary>
        public string FeedUrl { get; set; }

        /// <summary>
        /// Gets or sets author.
        /// </summary>
        public string Author { get; set; }

        /// <summary>
        /// Gets or sets publish Date.
        /// </summary>
        public DateTime PublishDate { get; set; }

        /// <summary>
        /// Gets or sets item's categories.
        /// </summary>
        public IEnumerable<string> Categories { get; set; }
    }
}