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
    /// ---.
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
        /// Gets or sets yaml header properties.
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
        /// Parses YAML header.
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
            protected override BlockParseResult<YamlHeaderBlock> ParseInternal(in LineBlock markdown, int startLine, bool lineStartsNewParagraph, MarkdownDocument document)
            {
                // As yaml header, must be start a line with "---"
                // and end with a line "---"
                if (!markdown[startLine].StartsWith("---".AsSpan()))
                {
                    return null;
                }

                if (startLine != 0)
                {
                    return null;
                }

                if (markdown[startLine].Length != 3)
                {
                    return null;
                }

                var yamlBlock = markdown.SliceLines(startLine + 1);

                var end = yamlBlock.IndexOf("---".AsSpan());

                if (end.Column != 0)
                {
                    return null;
                }

                yamlBlock = yamlBlock.SliceLines(0, end.Line);

                var resultDictionary = new Dictionary<string, string>();
                int lastLine = -1;
                for (int i = 0; i < yamlBlock.LineCount; i++)
                {
                    var line = yamlBlock[i];

                    var seperatorIndex = line.IndexOf(": ".AsSpan(), StringComparison.InvariantCulture);
                    if (seperatorIndex != -1)
                    {
                        var key = line.Slice(0, seperatorIndex);
                        var value = line.Slice(seperatorIndex + 2);
                        if (key.Length == 0 || value.Length == 0)
                        {
                            continue;
                        }

                        resultDictionary.Add(key.ToString(), value.ToString());
                    }
                    else if (line.StartsWith("---".AsSpan()) && line.Length == 3)
                    {
                        lastLine = i;
                        break;
                    }
                    else
                    {
                        return null;
                    }
                }

                var result = new YamlHeaderBlock
                {
                    Children = resultDictionary,
                };

                int numberOfLines;
                if (lastLine == -1)
                {
                    numberOfLines = yamlBlock.LineCount;
                }
                else
                {
                    numberOfLines = lastLine + 1;
                }

                return BlockParseResult.Create(result, startLine, numberOfLines);
            }
        }
    }
}
