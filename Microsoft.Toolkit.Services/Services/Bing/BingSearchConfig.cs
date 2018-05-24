// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Microsoft.Toolkit.Services.Bing
{
    /// <summary>
    /// Search query configuration.
    /// </summary>
    public class BingSearchConfig
    {
        /// <summary>
        /// Gets or sets search query country.
        /// </summary>
        public BingCountry Country { get; set; }

        /// <summary>
        /// Gets or sets search query language.
        /// </summary>
        public BingLanguage Language { get; set; }

        /// <summary>
        /// Gets or sets search query.
        /// </summary>
        public string Query { get; set; }

        /// <summary>
        /// Gets or sets search query type.
        /// </summary>
        public BingQueryType QueryType { get; set; }
    }
}
