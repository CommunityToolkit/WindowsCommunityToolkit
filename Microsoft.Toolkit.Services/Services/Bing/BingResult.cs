// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;

namespace Microsoft.Toolkit.Services.Bing
{
    /// <summary>
    /// Implementation of the Bing result class.
    /// </summary>
    public class BingResult : Parsers.SchemaBase
    {
        /// <summary>
        /// Gets or sets title of the search result.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets summary of the search result.
        /// </summary>
        public string Summary { get; set; }

        /// <summary>
        /// Gets or sets link to the Search result.
        /// </summary>
        public string Link { get; set; }

        /// <summary>
        /// Gets or sets date of publication.
        /// </summary>
        public DateTime Published { get; set; }
    }
}