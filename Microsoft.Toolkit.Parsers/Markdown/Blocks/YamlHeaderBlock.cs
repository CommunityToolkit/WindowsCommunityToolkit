﻿using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Toolkit.Parsers.Markdown.Helpers;

namespace Microsoft.Toolkit.Parsers.Markdown.Blocks
{
    /// <summary>
    /// Yaml Header. use for blog.
    /// e.g.
    /// ---
    /// title: something
    /// tag: something
    /// ---
    /// </summary>
    public class YamlHeaderBlock : MarkdownBlock
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="YamlHeaderBlock"/> class.
        /// </summary>
        public YamlHeaderBlock()
            : base(MarkdownBlockType.YamlHeader)
        {
        }

        /// <summary>
        /// Gets or sets yaml header properties
        /// </summary>
        public Dictionary<string, string> Children { get; set; }

        /// <summary>
        /// Parse yaml header
        /// </summary>
        /// <param name="markdown"> The markdown text. </param>
        /// <param name="start"> The location of the first hash character. </param>
        /// <param name="end"> The location of the end of the line. </param>
        /// <param name="realEndIndex"> The location of the actual end of the aprse. </param>
        /// <returns>Parsed <see cref="YamlHeaderBlock"/> class</returns>
        internal static YamlHeaderBlock Parse(string markdown, int start, int end, out int realEndIndex)
        {
            // As yaml header, must be start a line with "---"
            // and end with a line "---"
            realEndIndex = start;
            int lineStart = start;
            if (lineStart != 0 || markdown.Substring(start, 3) != "---")
            {
                return null;
            }

            int startUnderlineIndex = Common.FindNextSingleNewLine(markdown, lineStart, end, out int startOfNextLine);
            if (startUnderlineIndex - lineStart != 3)
            {
                return null;
            }

            bool lockedFinalUnderline = false;

            // if current line not contain the ": ", check it is end of parse, if not, exit
            // if next line is the end, exit
            int pos = startOfNextLine;
            List<string> elements = new List<string>();
            while (pos < end)
            {
                int nextUnderLineIndex = Common.FindNextSingleNewLine(markdown, pos, end, out startOfNextLine);
                bool haveSeparator = markdown.Substring(pos, nextUnderLineIndex - pos).Contains(": ");
                if (haveSeparator)
                {
                    elements.Add(markdown.Substring(pos, nextUnderLineIndex - pos));
                }
                else if (markdown.Substring(pos, 3) == "---")
                {
                    lockedFinalUnderline = true;
                    realEndIndex = pos + 3;
                    break;
                }
                else if (startOfNextLine == pos + 1)
                {
                    pos = startOfNextLine;
                    continue;
                }
                else
                {
                    return null;
                }

                pos = startOfNextLine;
            }

            // if not have the end, return
            if (!lockedFinalUnderline)
            {
                return null;
            }

            // parse yaml header properties
            if (elements.Count < 1)
            {
                return null;
            }

            var result = new YamlHeaderBlock();
            foreach (var item in elements)
            {
                string[] splits = item.Split(new string[] { ": " }, StringSplitOptions.None);
                if (splits.Length < 2)
                {
                    continue;
                }
                else
                {
                    if (result.Children == null)
                    {
                        result.Children = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
                    }

                    if (!result.Children.ContainsKey(splits[0].Trim()))
                    {
                        result.Children.Add(splits[0].Trim(), splits[1].Trim());
                    }
                    else
                    {
                        result.Children[splits[0].Trim()] = splits[1].Trim();
                    }
                }
            }

            if (result.Children.Count == 0)
            {
                return null;
            }

            return result;
        }

        /// <summary>
        /// Converts the object into it's textual representation.
        /// </summary>
        /// <returns> The textual representation of this object. </returns>
        public override string ToString()
        {
            if (Children == null)
            {
                return base.ToString();
            }
            else
            {
                return string.Join(string.Empty, Children);
            }
        }
    }
}
