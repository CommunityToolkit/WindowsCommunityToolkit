// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Buffers;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Toolkit.Parsers.Markdown
{
    public static class SpanExtensions
    {
        public const int MAX_STACK_BUFFER_SIZE = 1024;

        public static int IndexOfNonWhiteSpace(this ReadOnlySpan<char> text)
        {
            for (int i = 0; i < text.Length; i++)
            {
                if (!char.IsWhiteSpace(text[i]))
                {
                    return i;
                }
            }

            return -1;
        }

        public static int IndexOfNexWhiteSpace(this ReadOnlySpan<char> text)
        {
            for (int i = 0; i < text.Length; i++)
            {
                if (char.IsWhiteSpace(text[i]))
                {
                    return i;
                }
            }

            return -1;
        }

        public static int FindClosingBrace(this ReadOnlySpan<char> line)
        {
            var opening = line[0];
            var closing = opening switch
            {
                '(' => ')',
                '[' => ']',
                '{' => '}',
                '<' => '>',
                _ => opening
            };
            return FindClosingBrace(line, opening, closing);
        }

        public static int FindClosingBrace(this ReadOnlySpan<char> line, char opening, char closing)
        {
            var pos = 1;
            int openSquareBracketCount = 0;
            var closingBraceIndex = -1;
            while (true)
            {
                if (pos >= line.Length)
                {
                    break;
                }

                var linkTextClose = line.Slice(pos).IndexOfAny(opening, closing) + pos;
                if (linkTextClose < pos)
                {
                    break;
                }

                if (line[linkTextClose] == closing)
                {
                    if (openSquareBracketCount > 0)
                    {
                        openSquareBracketCount--;
                    }
                    else
                    {
                        closingBraceIndex = linkTextClose;
                        break;
                    }
                }
                else
                {
                    openSquareBracketCount++;
                }

                pos = linkTextClose + 1;
            }

            return closingBraceIndex;
        }

#if NETSTANDARD2_0
        internal static StringBuilder Append(this StringBuilder stringBuilder, ReadOnlySpan<char> txt)
        {
            var buffer = ArrayPool<char>.Shared.Rent(txt.Length);
            txt.CopyTo(buffer);
            stringBuilder.Append(buffer, 0, txt.Length);
            ArrayPool<char>.Shared.Return(buffer, false);
            return stringBuilder;
        }
#endif
    }
}