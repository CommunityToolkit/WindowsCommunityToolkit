// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Toolkit.Parsers.Markdown.Inlines;

namespace Microsoft.Toolkit.Parsers.Markdown.Helpers
{
    /// <summary>
    /// Helpers for Markdown.
    /// </summary>
    internal class Common
    {
        /// <summary>
        /// Returns the next \n or \r\n in the markdown.
        /// </summary>
        /// <returns>the next single line</returns>
        public static int FindNextSingleNewLine(string markdown, int startingPos, int endingPos, out int startOfNextLine)
        {
            // A line can end with CRLF (\r\n) or just LF (\n).
            int lineFeedPos = markdown.IndexOf('\n', startingPos);
            if (lineFeedPos == -1)
            {
                // Trying with /r now
                lineFeedPos = markdown.IndexOf('\r', startingPos);
                if (lineFeedPos == -1)
                {
                    startOfNextLine = endingPos;
                    return endingPos;
                }
            }

            startOfNextLine = lineFeedPos + 1;

            // Check if it was a CRLF.
            if (lineFeedPos > startingPos && markdown[lineFeedPos - 1] == '\r')
            {
                return lineFeedPos - 1;
            }

            return lineFeedPos;
        }

        /// <summary>
        /// Returns the next \n or \r\n in the markdown.
        /// </summary>
        /// <returns>the next single line</returns>
        public static int FindPreviousSingleNewLine(string markdown, int startingPos, int endingPos, out int startOfNextLine)
        {
            // A line can end with CRLF (\r\n) or just LF (\n).
            int lineFeedPos = markdown.LastIndexOf('\n', startingPos);
            if (lineFeedPos == -1)
            {
                // Trying with /r now
                lineFeedPos = markdown.LastIndexOf('\r', startingPos);
                if (lineFeedPos == -1)
                {
                    startOfNextLine = endingPos;
                    return endingPos;
                }
            }

            startOfNextLine = lineFeedPos + 1;

            // Check if it was a CRLF.
            if (lineFeedPos > startingPos && markdown[lineFeedPos - 1] == '\r')
            {
                return lineFeedPos - 1;
            }

            return lineFeedPos;
        }

        /// <summary>
        /// Helper function for index of with a start and an ending.
        /// </summary>
        /// <returns>Pos of the searched for item</returns>
        public static int IndexOf(string markdown, string search, int startingPos, int endingPos, bool reverseSearch = false)
        {
            // Check the ending isn't out of bounds.
            if (endingPos > markdown.Length)
            {
                endingPos = markdown.Length;
                DebuggingReporter.ReportCriticalError("IndexOf endingPos > string length");
            }

            // Figure out how long to go
            int count = endingPos - startingPos;
            if (count < 0)
            {
                return -1;
            }

            // Make sure we don't go too far.
            int remainingCount = markdown.Length - startingPos;
            if (count > remainingCount)
            {
                DebuggingReporter.ReportCriticalError("IndexOf count > remaing count");
                count = remainingCount;
            }

            // Check the ending. Since we use inclusive ranges we need to -1 from this for
            // reverses searches.
            if (reverseSearch && endingPos > 0)
            {
                endingPos -= 1;
            }

            return reverseSearch ? markdown.LastIndexOf(search, endingPos, count, StringComparison.OrdinalIgnoreCase) : markdown.IndexOf(search, startingPos, count, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Helper function for index of with a start and an ending.
        /// </summary>
        /// <returns>Pos of the searched for item</returns>
        public static int IndexOf(string markdown, char search, int startingPos, int endingPos, bool reverseSearch = false)
        {
            // Check the ending isn't out of bounds.
            if (endingPos > markdown.Length)
            {
                endingPos = markdown.Length;
                DebuggingReporter.ReportCriticalError("IndexOf endingPos > string length");
            }

            // Figure out how long to go
            int count = endingPos - startingPos;
            if (count < 0)
            {
                return -1;
            }

            // Make sure we don't go too far.
            int remainingCount = markdown.Length - startingPos;
            if (count > remainingCount)
            {
                DebuggingReporter.ReportCriticalError("IndexOf count > remaing count");
                count = remainingCount;
            }

            // Check the ending. Since we use inclusive ranges we need to -1 from this for
            // reverses searches.
            if (reverseSearch && endingPos > 0)
            {
                endingPos -= 1;
            }

            return reverseSearch ? markdown.LastIndexOf(search, endingPos, count) : markdown.IndexOf(search, startingPos, count);
        }

        /// <summary>
        /// Finds the next whitespace in a range.
        /// </summary>
        /// <returns>pos of the white space</returns>
        public static int FindNextWhiteSpace(string markdown, int startingPos, int endingPos, bool ifNotFoundReturnLength)
        {
            int currentPos = startingPos;
            while (currentPos < markdown.Length && currentPos < endingPos)
            {
                if (char.IsWhiteSpace(markdown[currentPos]))
                {
                    return currentPos;
                }

                currentPos++;
            }

            return ifNotFoundReturnLength ? endingPos : -1;
        }

        /// <summary>
        /// Finds the next whitespace in a range.
        /// </summary>
        /// <returns>pos of the white space</returns>
        public static int FindNextNoneWhiteSpace(string markdown, int startingPos, int endingPos, bool ifNotFoundReturnLength)
        {
            int currentPos = startingPos;
            while (currentPos < markdown.Length && currentPos < endingPos)
            {
                if (!char.IsWhiteSpace(markdown[currentPos]))
                {
                    return currentPos;
                }

                currentPos++;
            }

            return ifNotFoundReturnLength ? endingPos : -1;
        }

        /// <summary>
        /// Parses lines.
        /// </summary>
        /// <returns>LineInfo</returns>
        public static IEnumerable<LineInfo> ParseLines(string markdown, int start, int end)
        {
            int pos = start;

            while (pos < end)
            {
                int startOfLine = pos;
                int nonSpacePos = pos;

                // Find the end of the current line.
                int endOfLine = FindNextSingleNewLine(markdown, nonSpacePos, end, out int startOfNextLine);

                // Return the line info to the caller.
                yield return new LineInfo
                {
                    StartOfLine = startOfLine,
                    FirstNonWhitespaceChar = nonSpacePos,
                    EndOfLine = endOfLine,
                    StartOfNextLine = startOfNextLine,
                };

                // Repeat.
                pos = startOfNextLine;
            }
        }

        /// <summary>
        /// Checks if the given URL is allowed in a markdown link.
        /// </summary>
        /// <param name="url"> The URL to check. </param>
        /// <returns> <c>true</c> if the URL is valid; <c>false</c> otherwise. </returns>
        public static bool IsUrlValid(string url)
        {
            // URLs can be relative.
            if (!Uri.TryCreate(url, UriKind.Absolute, out Uri result))
            {
                return true;
            }

            // Check the scheme is allowed.
            foreach (var scheme in MarkdownDocument.KnownSchemes)
            {
                if (result.Scheme.Equals(scheme))
                {
                    return true;
                }
            }

            return false;
        }
    }
}