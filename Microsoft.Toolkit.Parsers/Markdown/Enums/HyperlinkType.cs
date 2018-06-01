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

namespace Microsoft.Toolkit.Parsers.Markdown
{
    /// <summary>
    /// Specifies the type of Hyperlink that is used.
    /// </summary>
    public enum HyperlinkType
    {
        /// <summary>
        /// A hyperlink surrounded by angle brackets (e.g. "http://www.reddit.com").
        /// </summary>
        BracketedUrl,

        /// <summary>
        /// A fully qualified hyperlink (e.g. "http://www.reddit.com").
        /// </summary>
        FullUrl,

        /// <summary>
        /// A URL without a scheme (e.g. "www.reddit.com").
        /// </summary>
        PartialUrl,

        /// <summary>
        /// An email address (e.g. "test@reddit.com").
        /// </summary>
        Email,

        /// <summary>
        /// A subreddit link (e.g. "/r/news").
        /// </summary>
        Subreddit,

        /// <summary>
        /// A user link (e.g. "/u/quinbd").
        /// </summary>
        User,
    }
}