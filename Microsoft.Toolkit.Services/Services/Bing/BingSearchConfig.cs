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
