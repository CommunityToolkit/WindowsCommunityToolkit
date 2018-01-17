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

using System.Collections.Generic;
using Microsoft.Toolkit.Parsers.Markdown.Helpers;

namespace Microsoft.Toolkit.Parsers.Markdown.Inlines
{
    /// <summary>
    /// Represents a span containing superscript text.
    /// </summary>
    public class SuperscriptTextInline : MarkdownInline, IInlineContainer
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SuperscriptTextInline"/> class.
        /// </summary>
        public SuperscriptTextInline()
            : base(MarkdownInlineType.Superscript)
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
            tripCharHelpers.Add(new Common.InlineTripCharHelper() { FirstChar = '^', Method = Common.InlineParseMethod.Superscript });
        }

        /// <summary>
        /// Attempts to parse a superscript text span.
        /// </summary>
        /// <param name="markdown"> The markdown text. </param>
        /// <param name="start"> The location to start parsing. </param>
        /// <param name="maxEnd"> The location to stop parsing. </param>
        /// <returns> A parsed superscript text span, or <c>null</c> if this is not a superscript text span. </returns>
        internal static Common.InlineParseResult Parse(string markdown, int start, int maxEnd)
        {
            // Check the first character.
            if (start == maxEnd || markdown[start] != '^')
            {
                return null;
            }

            // The content might be enclosed in parentheses.
            int innerStart = start + 1;
            int innerEnd, end;
            if (innerStart < maxEnd && markdown[innerStart] == '(')
            {
                // Find the end parenthesis.
                innerStart++;
                innerEnd = Common.IndexOf(markdown, ')', innerStart, maxEnd);
                if (innerEnd == -1)
                {
                    return null;
                }

                end = innerEnd + 1;
            }
            else
            {
                // Search for the next whitespace character.
                innerEnd = Common.FindNextWhiteSpace(markdown, innerStart, maxEnd, ifNotFoundReturnLength: true);
                if (innerEnd == innerStart)
                {
                    // No match if the character after the caret is a space.
                    return null;
                }

                end = innerEnd;
            }

            // We found something!
            var result = new SuperscriptTextInline();
            result.Inlines = Common.ParseInlineChildren(markdown, innerStart, innerEnd);
            return new Common.InlineParseResult(result, start, end);
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

            return "^(" + string.Join(string.Empty, Inlines) + ")";
        }
    }
}