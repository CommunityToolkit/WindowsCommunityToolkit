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

namespace Microsoft.Toolkit.Services.Bing
{
    /// <summary>
    /// Implementation of the Bing result class.
    /// </summary>
    public class BingResult : SchemaBase
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
