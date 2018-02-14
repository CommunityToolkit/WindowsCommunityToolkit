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

namespace Microsoft.Toolkit.Parsers.Markdown.Enums
{
    internal enum InlineParseMethod
    {
        /// <summary>
        /// A Comment text
        /// </summary>
        Comment,

        /// <summary>
        /// A Link Reference
        /// </summary>
        LinkReference,

        /// <summary>
        /// A bold element
        /// </summary>
        Bold,

        /// <summary>
        /// An bold and italic block
        /// </summary>
        BoldItalic,

        /// <summary>
        /// A code element
        /// </summary>
        Code,

        /// <summary>
        /// An italic block
        /// </summary>
        Italic,

        /// <summary>
        /// A link block
        /// </summary>
        MarkdownLink,

        /// <summary>
        /// An angle bracket link.
        /// </summary>
        AngleBracketLink,

        /// <summary>
        /// A url block
        /// </summary>
        Url,

        /// <summary>
        /// A reddit style link
        /// </summary>
        RedditLink,

        /// <summary>
        /// An in line text link
        /// </summary>
        PartialLink,

        /// <summary>
        /// An email element
        /// </summary>
        Email,

        /// <summary>
        /// strike through element
        /// </summary>
        Strikethrough,

        /// <summary>
        /// Super script element.
        /// </summary>
        Superscript,

        /// <summary>
        /// Image element.
        /// </summary>
        Image,

        /// <summary>
        /// Emoji element.
        /// </summary>
        Emoji
    }
}