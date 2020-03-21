// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Buffers;
using System.Collections.Generic;
using System.Text;
using Microsoft.Toolkit.Parsers.Core;
using Microsoft.Toolkit.Parsers.Markdown.Helpers;
using Microsoft.Toolkit.Parsers.Markdown.Inlines;

namespace Microsoft.Toolkit.Parsers.Markdown.Blocks
{
    /// <summary>
    /// Represents a block which contains tabular data.
    /// </summary>
    public class TableBlock : MarkdownBlock
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TableBlock"/> class.
        /// </summary>
        public TableBlock()
            : base(MarkdownBlockType.Table)
        {
        }

        /// <summary>
        /// Gets or sets the table rows.
        /// </summary>
        public IList<TableRow> Rows { get; set; }

        /// <summary>
        /// Gets or sets describes the columns in the table.  Rows can have more or less cells than the number
        /// of columns.  Rows with fewer cells should be padded with empty cells.  For rows with
        /// more cells, the extra cells should be hidden.
        /// </summary>
        public IList<TableColumnDefinition> ColumnDefinitions { get; set; }

        /// <summary>
        /// Describes a column in the markdown table.
        /// </summary>
        public class TableColumnDefinition
        {
            /// <summary>
            /// Gets or sets the alignment of content in a table column.
            /// </summary>
            public ColumnAlignment Alignment { get; set; }

            /// <summary>
            /// Parses the contents of the row, ignoring whitespace at the beginning and end of each cell.
            /// </summary>
            /// <returns> The position of the start of the next line. </returns>
            internal static List<TableColumnDefinition> ParseContents(ReadOnlySpan<char> line, int expectedNumberOfCoulumns)
            {
                var list = new List<TableColumnDefinition>(expectedNumberOfCoulumns);

                while (true)
                {
                    if (list.Count == expectedNumberOfCoulumns)
                    {
                        break;
                    }

                    if (line.Length == 0)
                    {
                        break;
                    }

                    if (line[0] == '|')
                    {
                        line = line.Slice(1);
                    }

                    if (line.Length == 0)
                    {
                        break;
                    }

                    var cell = new TableColumnDefinition() { Alignment = ColumnAlignment.Unspecified };
                    list.Add(cell);

                    var endOfCell = line.IndexOf('|');
                    if (endOfCell == -1)
                    {
                        endOfCell = line.Length;
                    }

                    var content = line.Slice(0, endOfCell);

                    if (content.Length == 0)
                    {
                        return null;
                    }

                    // check any invalid char;
                    for (int i = 0; i < content.Length; i++)
                    {
                        if (!(content[i] == '-' || ((i == 0 || i == content.Length - 1) && content[i] == ':')))
                        {
                            return null;
                        }
                    }

                    if (content[0] == ':')
                    {
                        cell.Alignment = ColumnAlignment.Left;
                    }

                    if (content.Length > 1 && content[content.Length - 1] == ':')
                    {
                        // left is 1 and right is 2 center is 3
                        cell.Alignment |= ColumnAlignment.Right;
                    }

                    line = line.Slice(endOfCell);
                }

                if (list.Count < expectedNumberOfCoulumns)
                {
                    return null;
                }

                return list;
            }
        }

        /// <summary>
        /// Represents a single row in the table.
        /// </summary>
        public class TableRow
        {
            /// <summary>
            /// Gets or sets the table cells.
            /// </summary>
            public IList<TableCell> Cells { get; set; }

            /// <summary>
            /// Parses the contents of the row, ignoring whitespace at the beginning and end of each cell.
            /// </summary>
            /// <returns> The position of the start of the next line. </returns>
            internal static List<TableCell> ParseContents(ReadOnlySpan<char> line, MarkdownDocument document, int? expectedNumberOfCoulumns)
            {
                var list = expectedNumberOfCoulumns.HasValue
                    ? new List<TableCell>(expectedNumberOfCoulumns.Value)
                    : new List<TableCell>();

                while (true)
                {
                    if (expectedNumberOfCoulumns.HasValue && list.Count == expectedNumberOfCoulumns.Value)
                    {
                        break;
                    }

                    if (line.Length == 0)
                    {
                        break;
                    }

                    if (line[0] == '|')
                    {
                        line = line.Slice(1);
                    }

                    if (line.Length == 0)
                    {
                        break;
                    }

                    var cell = new TableCell() { Inlines = Array.Empty<MarkdownInline>() };
                    list.Add(cell);

                    var endOfCell = 0;
                    while (true)
                    {
                        var nextPipe = line.Slice(endOfCell).IndexOf('|');
                        if (nextPipe == -1)
                        {
                            endOfCell = line.Length;
                            nextPipe = 0;
                        }

                        endOfCell = nextPipe + endOfCell;

                        if (endOfCell > 0 && line[endOfCell - 1] == '\\')
                        {
                            endOfCell++;
                            continue;
                        }

                        break;
                    }

                    // Ignore any whitespace at the start of the cell (except for a newline character).
                    cell.Inlines = document.ParseInlineChildren(line.Slice(0, endOfCell), true, true);

                    line = line.Slice(endOfCell);
                }

                if (expectedNumberOfCoulumns.HasValue && list.Count < expectedNumberOfCoulumns.Value)
                {
                    return null;
                }

                return list;
            }

            /// <summary>
            /// Called when this block type should parse out the goods. Given the markdown, a starting point, and a max ending point
            /// the block should find the start of the block, find the end and parse out the middle. The end most of the time will not be
            /// the max ending pos, but it sometimes can be. The function will return where it ended parsing the block in the markdown.
            /// </summary>
            /// <returns>the postiion parsed to.</returns>
            internal bool Parse(ReadOnlySpan<char> markdown, MarkdownDocument document, int? expectedNumberOfColumns)
            {
                Cells = ParseContents(
                    markdown,
                    document,
                    expectedNumberOfColumns);

                return Cells != null;
            }
        }

        /// <summary>
        /// Represents a cell in the table.
        /// </summary>
        public class TableCell
        {
            /// <summary>
            /// Gets or sets the cell contents.
            /// </summary>
            public IList<MarkdownInline> Inlines { get; set; }
        }

        /// <summary>
        /// Parses Tables.
        /// </summary>
        public new class Parser : Parser<TableBlock>
        {
            /// <inheritdoc/>
            protected override BlockParseResult<TableBlock> ParseInternal(in LineBlock markdown, int startLine, bool lineStartsNewParagraph, MarkdownDocument document)
            {
                // A table is a line of text, with at least one vertical bar (|), followed by a line of
                // of text that consists of alternating dashes (-) and vertical bars (|) and optionally
                // vertical bars at the start and end.  The second line must have at least as many
                // interior vertical bars as there are interior vertical bars on the first line.
                if (!lineStartsNewParagraph)
                {
                    return null;
                }

                // at least we need 2 lines.
                if (startLine + 1 >= markdown.LineCount)
                {
                    return null;
                }

                // First thing to do is to check if there is a vertical bar on the line.
                var allBarsEscaped = true;
                for (int i = 0; i < markdown[startLine].Length; i++)
                {
                    if (markdown[startLine][i] == '|' && (i == 0 || markdown[startLine][i - 1] != '\\'))
                    {
                        allBarsEscaped = false;
                        break;
                    }
                }

                if (allBarsEscaped)
                {
                    return null;
                }

                var rows = new List<TableRow>();

                // Parse the first row.
                var firstRow = new TableRow();
                firstRow.Parse(markdown[startLine], document, null);
                rows.Add(firstRow);

                var numberOfColumns = firstRow.Cells.Count;

                // Parse the contents of the second row.
                var columnDefinitions = TableColumnDefinition.ParseContents(markdown[startLine + 1], numberOfColumns);

                // There must be at least as many columns in the second row as in the first row.
                // Note: excess columns past firstRowColumnCount are ignored and can contain anything.
                if (columnDefinitions is null)
                {
                    return null;
                }

                for (int i = startLine + 2; i < markdown.LineCount; i++)
                {
                    var row = new TableRow();

                    var success = row.Parse(markdown[i], document, numberOfColumns);
                    if (!success)
                    {
                        break;
                    }

                    rows.Add(row);
                }

                // +1 for the coulm definition row
                var consumedLines = rows.Count + 1;

                var tableBlock = new TableBlock { ColumnDefinitions = columnDefinitions, Rows = rows };
                return BlockParseResult.Create(tableBlock, startLine, consumedLines);
            }
        }
    }
}