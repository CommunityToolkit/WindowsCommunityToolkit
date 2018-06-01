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

namespace Microsoft.Toolkit.Uwp.Services.LinkedIn
{
    /// <summary>
    /// Strong type representation of Content.
    /// </summary>
    public class LinkedInContent
    {
        /// <summary>
        /// Gets or sets title property.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets description property.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets submitted url property.
        /// </summary>
        public string SubmittedUrl { get; set; }

        /// <summary>
        /// Gets or sets submitted image url property.
        /// </summary>
        public string SubmittedImageUrl { get; set; }
    }
}
