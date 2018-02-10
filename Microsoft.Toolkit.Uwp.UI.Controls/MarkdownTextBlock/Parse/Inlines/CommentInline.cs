﻿// ******************************************************************
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

using System.Collections.Generic;
using Microsoft.Toolkit.Uwp.UI.Controls.Markdown.Helpers;

namespace Microsoft.Toolkit.Uwp.UI.Controls.Markdown.Parse
{
    /// <summary>
    /// Represents a span that contains comment.
    /// </summary>
    internal class CommentInline : MarkdownInline, IInlineContainer
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CommentInline"/> class.
        /// </summary>
        public CommentInline()
            : base(MarkdownInlineType.Comment)
        {
        }

        /// <summary>
        /// Gets or sets the contents of the inline.
        /// </summary>
        public IList<MarkdownInline> Inlines { get; set; }

        /// <summary>
        /// Returns the chars that if found means we might have a match.
        /// </summary>
        internal static void AddTripChars(List<Common.InlineTripCharHelper> tripCharHelpers)
        {
            tripCharHelpers.Add(new Common.InlineTripCharHelper() { FirstChar = '<', Method = Common.InlineParseMethod.Comment });
        }

        /// <summary>
        /// Attempts to parse a comment span.
        /// </summary>
        /// <param name="markdown"> The markdown text. </param>
        /// <param name="start"> The location to start parsing. </param>
        /// <param name="maxEnd"> The location to stop parsing. </param>
        /// <returns> A parsed bold text span, or <c>null</c> if this is not a bold text span. </returns>
        internal static Common.InlineParseResult Parse(string markdown, int start, int maxEnd)
        {
            if (start >= maxEnd - 1)
            {
                return null;
            }

            // Check the start sequence.
            string startSequence = markdown.Substring(start, 4);
            if (startSequence != "<!--")
            {
                return null;
            }

            // Find the end of the span.  The end sequence ('-->')
            var innerStart = start + 4;
            int innerEnd = Common.IndexOf(markdown, "-->", innerStart, maxEnd);
            if (innerEnd == -1)
            {
                return null;
            }

            // We found something!
            var result = new CommentInline();
            result.Inlines = Common.ParseInlineChildren(string.Empty, 0, 0);
            return new Common.InlineParseResult(result, start, innerEnd + 3);
        }

        /// <summary>
        /// Converts the object into it's textual representation.
        /// </summary>
        /// <returns> The textual representation of this object. </returns>
        public override string ToString()
        {
            if (Inlines == null)
            {
                return base.ToString();
            }

            return "<!--" + string.Join(string.Empty, Inlines) + "-->";
        }
    }
}
