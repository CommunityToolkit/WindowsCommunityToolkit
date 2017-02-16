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

namespace Microsoft.Toolkit.Uwp.UI.Controls.Markdown.Parse
{
    internal enum MarkdownInlineType
    {
        /// <summary>
        /// A text run
        /// </summary>
        TextRun,

        /// <summary>
        /// A bold run
        /// </summary>
        Bold,

        /// <summary>
        /// An italic run
        /// </summary>
        Italic,

        /// <summary>
        /// A link in markdown syntax
        /// </summary>
        MarkdownLink,

        /// <summary>
        /// A raw hyper link
        /// </summary>
        RawHyperlink,

        /// <summary>
        /// A raw subreddit link
        /// </summary>
        RawSubreddit,

        /// <summary>
        /// A strike through run
        /// </summary>
        Strikethrough,

        /// <summary>
        /// A superscript run
        /// </summary>
        Superscript,

        /// <summary>
        /// A code run
        /// </summary>
        Code,

        /// <summary>
        /// An image
        /// </summary>
        Image
    }

    /// <summary>
    /// An internal class that is the base class for all inline elements.
    /// </summary>
    internal abstract class MarkdownInline : MarkdownElement
    {
        /// <summary>
        /// Gets or sets this element is.
        /// </summary>
        internal MarkdownInlineType Type { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="MarkdownInline"/> class.
        /// </summary>
        internal MarkdownInline(MarkdownInlineType type)
        {
            Type = type;
        }
    }
}
