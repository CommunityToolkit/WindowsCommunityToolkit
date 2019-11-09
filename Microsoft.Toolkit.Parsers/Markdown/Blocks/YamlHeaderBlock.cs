// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
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
                string result = string.Empty;
                foreach (KeyValuePair<string, string> item in Children)
                {
                    result += item.Key + ": " + item.Value + "\n";
                }

                result.TrimEnd('\n');
                return result;
            }
        }

        /// <summary>
        /// Parses YAML header
        /// </summary>
        public new class Parser : Parser<YamlHeaderBlock>
        {
            /// <inheritdoc/>
            protected override void ConfigureDefaults(DefaultParserConfiguration configuration)
            {
                base.ConfigureDefaults(configuration);
                configuration.Before<HorizontalRuleBlock.Parser>();
            }

            /// <inheritdoc/>
            protected override YamlHeaderBlock ParseInternal(string markdown, int startOfLine, int firstNonSpace, int endOfFirstLine, int maxEnd, out int actualEnd, StringBuilder paragraphText, bool lineStartsNewParagraph, MarkdownDocument document)
            {
                // As yaml header, must be start a line with "---"
                // and end with a line "---"
                actualEnd = startOfLine;
                int lineStart = startOfLine;

                if (markdown[firstNonSpace] != '-' && firstNonSpace == startOfLine)
                {
                    return null;
                }

                if (maxEnd - startOfLine < 3)
                {
                    return null;
                }

                if (lineStart != 0 || markdown.Substring(startOfLine, 3) != "---")
                {
                    return null;
                }

                int startUnderlineIndex = Common.FindNextSingleNewLine(markdown, lineStart, maxEnd, out int startOfNextLine);
                if (startUnderlineIndex - lineStart != 3)
                {
                    return null;
                }

                bool lockedFinalUnderline = false;

                // if current line not contain the ": ", check it is end of parse, if not, exit
                // if next line is the end, exit
                int pos = startOfNextLine;
                List<string> elements = new List<string>();
                while (pos < maxEnd)
                {
                    int nextUnderLineIndex = Common.FindNextSingleNewLine(markdown, pos, maxEnd, out startOfNextLine);
                    bool haveSeparator = markdown.Substring(pos, nextUnderLineIndex - pos).Contains(": ");
                    if (haveSeparator)
                    {
                        elements.Add(markdown.Substring(pos, nextUnderLineIndex - pos));
                    }
                    else if (maxEnd - pos >= 3 && markdown.Substring(pos, 3) == "---")
                    {
                        lockedFinalUnderline = true;
                        actualEnd = pos + 3;
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
                var keys = new List<string>();
                var values = new List<string>();
                result.Children = new Dictionary<string, string>();
                foreach (var item in elements)
                {
                    string[] splits = item.Split(new string[] { ": " }, StringSplitOptions.None);
                    if (splits.Length < 2)
                    {
                        continue;
                    }
                    else
                    {
                        string key = splits[0];
                        string value = splits[1];
                        if (key.Trim().Length == 0)
                        {
                            continue;
                        }

                        value = string.IsNullOrEmpty(value.Trim()) ? string.Empty : value;
                        result.Children.Add(key, value);
                    }
                }

                if (result.Children == null)
                {
                    return null;
                }

                return result;
            }
        }
    }
}
