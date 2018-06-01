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