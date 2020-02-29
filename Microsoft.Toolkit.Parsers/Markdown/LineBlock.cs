// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Buffers;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Microsoft.Toolkit.Parsers.Markdown
{
    /// <summary>
    /// Callback provides new start and length for the line.
    /// </summary>
    /// <param name="line">The original line.</param>
    /// <param name="lineNumber">The index of the Line.</param>
    /// <returns>A tuple containing start and length of the line.</returns>
    public delegate (int start, int length, bool skipLine, bool lastLine) RemoveLineCallback(ReadOnlySpan<char> line, int lineNumber);

    /// <summary>
    /// Filters parts of a string.
    /// </summary>
    public readonly ref struct LineBlock
    {
        private readonly ReadOnlySpan<(int start, int length)> lines;

        private readonly ReadOnlySpan<char> text;

        private readonly int start;
        private readonly int fromEnd;

        /// <summary>
        /// Gets the number of lines in this Block.
        /// </summary>
        public int LineCount => this.lines.Length;

        /// <summary>
        /// Gets a single line.
        /// </summary>
        public ReadOnlySpan<char> this[int line]
        {
            get
            {

                var result = this.text.Slice(this.lines[line].start, this.lines[line].length);
                if (line == 0)
                {
                    result = result.Slice(this.start, result.Length - this.start);
                }
                if (line == this.LineCount - 1)
                {
                    result = result.Slice(0, result.Length - this.fromEnd);
                }

                return result;
            }
        }

        public char this[int line, int column] => this[line][column];
        public char this[LineBlockPosition pos] => this[pos.Line][pos.Column];


        /// <summary>
        /// Gets the number of characters of this text. (Without counting linbreaks).
        /// </summary>
        public int TextLength
        {
            get
            {
                int length = 0;
                for (int i = 0; i < this.lines.Length; i++)
                {
                    length += this.lines[i].length;
                }

                return length - this.start - this.fromEnd;
            }
        }

        /// <summary>
        /// Concatinates the lines and uses \n as an line seperator.
        /// </summary>
        /// <returns>The concatinated StringBuilder.</returns>
        public StringBuilder ToStringBuilder()
        {
            var builderSize = this.TextLength + this.LineCount - 1;

            var builder = new StringBuilder(builderSize);

            PopulateBuilder(builder);

            return builder;
        }

        internal void PopulateBuilder(StringBuilder builder)
        {
            for (int i = 0; i < this.lines.Length; i++)
            {
                var from = this[i];

                builder.Append(from);
                if (i < this.LineCount - 1)
                {
                    builder.AppendLine();
                }
            }
        }

        /// <summary>
        /// Concatinates the lines and uses \n as an line seperator.
        /// </summary>
        /// <returns>The concatinated string.</returns>
        public override string ToString()
        {
            var bufferSize = this.TextLength + ((this.LineCount - 1) * Environment.NewLine.Length);
            char[] arrayBuffer;
            if (bufferSize <= SpanExtensions.MAX_STACK_BUFFER_SIZE)
            {
                arrayBuffer = null;
            }
            else
            {
                arrayBuffer = ArrayPool<char>.Shared.Rent(bufferSize);
            }

            Span<char> buffer = arrayBuffer != null
                ? arrayBuffer.AsSpan(0, bufferSize)
                : stackalloc char[bufferSize];

            var index = 0;
            for (int i = 0; i < this.lines.Length; i++)
            {
                var from = this[i];
                from.CopyTo(buffer.Slice(index));
                index += from.Length;
                if (index < buffer.Length)
                {
                    Environment.NewLine.AsSpan().CopyTo(buffer.Slice(index));
                    index += Environment.NewLine.Length;
                }
            }

            var result = buffer.ToString();

            if (arrayBuffer != null)
            {
                ArrayPool<char>.Shared.Return(arrayBuffer, false);
            }

            return result;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LineBlock"/> struct.
        /// </summary>
        /// <param name="data">The text that is the base for this LineBlock.</param>
        public LineBlock(ReadOnlySpan<char> data)
        {
            this.text = data;
            var current = data;
            int lineStart = 0;
            var lines = new List<(int start, int length)>();
            this.start = 0;
            this.fromEnd = 0;
            while (true)
            {
                var indexCurent = current.IndexOfAny('\r', '\n');

                if (indexCurent == -1)
                {
                    lines.Add((lineStart, data.Length - lineStart));
                    break;
                }

                var length = indexCurent;

                var nextLine = lineStart + indexCurent + 1;

                if (current[indexCurent] == '\r' && indexCurent + 1 < current.Length && current[indexCurent + 1] == '\n')
                {
                    nextLine += 1;
                }

                lines.Add((lineStart, length));

                if (nextLine >= data.Length)
                {
                    break;
                }

                current = data.Slice(nextLine);
                lineStart = nextLine;
            }

            this.lines = new ReadOnlySpan<(int start, int length)>(lines.ToArray());
            System.Diagnostics.Debug.Assert(this.TextLength >= 0, "TextLength is negative");
        }

        private LineBlock(ReadOnlySpan<(int start, int length)> lines, ReadOnlySpan<char> text, int start, int fromEnd)
        {
            this.lines = lines;
            this.text = text;
            this.start = start;
            this.fromEnd = fromEnd;
            System.Diagnostics.Debug.Assert(this.TextLength >= 0, "TextLength is negative");
        }

        /// <summary>
        /// Slices the Lines.
        /// </summary>
        /// <param name="start">The startindex.</param>
        /// <param name="length">The number of lines.</param>
        /// <returns>A new LineBlock that will only have the lines specified.</returns>
        public LineBlock SliceLines(int start, int length)
        {
            int startModification;
            int endModification;
            if (start == 0)
            {
                startModification = this.start;
            }
            else
            {
                startModification = 0;
            }

            if (length == this.LineCount - start)
            {
                endModification = this.fromEnd;
            }
            else
            {
                endModification = 0;
            }

            var slicedLines = this.lines.Slice(start, length);
            if (slicedLines.Length == 0)
            {
                startModification = 0;
                endModification = 0;
            }

            var lineBlock = new LineBlock(slicedLines, this.text, startModification, endModification);

            System.Diagnostics.Debug.Assert(lineBlock.TextLength <= this.TextLength, "TextLength must be less then or equals this");
            return lineBlock;
        }

        /// <summary>
        /// Slices the Lines.
        /// </summary>
        /// <param name="start">The startindex.</param>
        /// <returns>A new LineBlock that will only have the lines specified.</returns>
        public LineBlock SliceLines(int start)
        {
            return this.SliceLines(start, this.LineCount - start);
        }

        /// <summary>
        /// Will remove a specific number of characters from the start of each line.
        /// If a line has less charaters then removed the line will have 0 characers.
        /// </summary>
        /// <param name="length">The number of characters to remove.</param>
        /// <returns>A new Instance of LineBlock with the lines modified.</returns>
        public LineBlock RemoveFromLineStart(int length)
        {
            var temp = new (int start, int length)[this.lines.Length];

            for (int i = 0; i < temp.Length; i++)
            {
                ref var toSet = ref temp[i];
                toSet = this.lines[i];
                toSet.length -= length;

                if (i == 0)
                {
                    toSet.length -= this.start;
                    toSet.start += this.start;
                }

                if (i == temp.Length - 1)
                {
                    toSet.length -= this.fromEnd;
                }

                if (toSet.length < 0)
                {
                    toSet.length = 0;
                    toSet.start = 0;
                }
                else
                {
                    toSet.start += length;
                }
            }

            var lineBlock = new LineBlock(temp.AsSpan(), this.text, 0, 0);

            System.Diagnostics.Debug.Assert(lineBlock.TextLength <= this.TextLength, "TextLength must be less then or equals this");
            return lineBlock;

        }

        /// <summary>
        /// Will remove a specific number of characters from the end of each line.
        /// If a line has less charaters then removed the line will have 0 characers.
        /// </summary>
        /// <param name="length">The number of characters to remove.</param>
        /// <returns>A new Instance of LineBlock with the lines modified.</returns>
        public LineBlock RemoveFromLineEnd(int length)
        {
            var temp = new (int start, int length)[this.lines.Length];

            for (int i = 0; i < temp.Length; i++)
            {
                ref var toSet = ref temp[i];
                toSet = this.lines[i];
                toSet.length -= length;

                if (i == 0)
                {
                    toSet.length -= this.start;
                    toSet.start += this.start;
                }

                if (i == temp.Length - 1)
                {
                    toSet.length -= this.fromEnd;
                }

                if (toSet.length < 0)
                {
                    toSet.length = 0;
                    toSet.start = 0;
                }
            }

            var lineBlock = new LineBlock(temp.AsSpan(), this.text, 0, 0);
            System.Diagnostics.Debug.Assert(lineBlock.TextLength <= this.TextLength, "TextLength must be less then or equals this");

            return lineBlock;
        }

        /// <summary>
        /// Will remove a specific number of characters from each line.
        /// A callback is called for each line and returns the new start and length.
        /// </summary>
        /// <param name="callback">The callback that will be called for each line.</param>
        /// <returns>A new Instance of LineBlock with the lines modified.</returns>
        public LineBlock RemoveFromLine(RemoveLineCallback callback)
        {
            var temp = new (int start, int length)[this.lines.Length];
            var skipedLines = 0;
            var linesTaken = 0;
            for (int i = 0; i < temp.Length; i++)
            {
                ref readonly var from = ref this.lines[i];
                ref var toSet = ref temp[i - skipedLines];

                var (newStart, newLength, skip, isLastLine) = callback(this[i], i);

                if (skip)
                {
                    skipedLines++;
                }
                else
                {
                    linesTaken++;

                    if (newStart < 0 || newStart > from.length || newStart + newLength > from.length)
                    {
                        throw new ArgumentOutOfRangeException($"The supplied argument were out of range. New string must <={from.length}");
                    }

                    toSet.start = newStart + from.start;
                    toSet.length = newLength;
                    if (i == 0)
                    {
                        toSet.start += this.start;
                        toSet.length -= this.start;
                    }

                    if (i == temp.Length - 1)
                    {
                        toSet.length -= this.fromEnd;
                    }
                }

                if (isLastLine)
                {
                    break;
                }

            }

            var lineBlock = new LineBlock(temp.AsSpan(0, linesTaken), this.text, 0, 0);
            System.Diagnostics.Debug.Assert(lineBlock.TextLength <= this.TextLength, "TextLength must be less then or equals this");

            return lineBlock;
        }

        /// <summary>
        /// Removes A specific amouts of charactesr. Empty lines will be removed.
        /// </summary>
        /// <param name="start">The position from where characters will be kept.</param>
        /// <returns>The modified Block.</returns>
        public LineBlock SliceText(int start) => this.SliceText(start, -1);

        /// <summary>
        /// Removes A specific amouts of charactesr. Empty lines will be removed.
        /// </summary>
        /// <param name="start">The position from where characters will be kept.</param>
        /// <returns>The modified Block.</returns>
        public LineBlock SliceText(LineBlockPosition start) => this.SliceText(start, -1);

        /// <summary>
        /// Removes A specific amouts of charactesr. Empty lines will be removed.
        /// </summary>
        /// <param name="start">The position from where characters will be kept.</param>
        /// <param name="length">The number of characters taken.</param>
        /// <returns>The modified Block.</returns>
        public LineBlock SliceText(LineBlockPosition start, int length)
        {
            // it is more prformant to remove the lines first.
            var slicedLines = this.SliceLines(start.Line);
            var slicedStart = slicedLines.SliceText(start.Column);
            var slicedEnd = slicedStart.SliceText(0, length);
            return slicedEnd;
        }

        /// <summary>
        /// Removes A specific amouts of charactesr. Empty lines will be removed.
        /// </summary>
        /// <param name="start">The position from where characters will be kept.</param>
        /// <param name="length">The number of characters taken.</param>
        /// <returns>The modified Block.</returns>
        public LineBlock SliceText(int start, int length)
        {
            if (start + length > this.TextLength)
            {
                throw new ArgumentOutOfRangeException();
            }

            var removedLines = 0;
            var newStart = this.start;
            var newEnd = this.fromEnd;
            LineBlock temp;
            if (start != 0)
            {
                for (int i = 0; i < this.lines.Length; i++)
                {
                    var currentLine = this[i];

                    if (start >= currentLine.Length)
                    {
                        start -= currentLine.Length;
                        removedLines++;
                        newStart = 0;
                    }
                    else
                    {
                        newStart += start;
                        if (start == currentLine.Length)
                        {
                            removedLines++;
                            newStart = 0;
                        }

                        break;
                    }
                }

                var slicedLines = this.lines.Slice(removedLines);
                if (slicedLines.Length == 0)
                {
                    newStart = 0;
                    newEnd = 0;
                }

                temp = new LineBlock(slicedLines, this.text, newStart, newEnd);
                System.Diagnostics.Debug.Assert(temp.TextLength >= 0, "TextLength is negative");
                System.Diagnostics.Debug.Assert(temp.TextLength <= this.TextLength, "TextLength must be less then or equals this");
            }
            else
            {
                temp = this;
            }

            if (length == -1)
            {
                return temp;
            }

            removedLines = temp.LineCount;
            for (int i = 0; i < temp.lines.Length; i++)
            {
                var currentLine = temp[i];

                if (length >= currentLine.Length)
                {
                    length -= currentLine.Length;

                    removedLines--;
                }
                else
                {
                    var newCalculatedEnd = currentLine.Length - length;
                    if (i == this.LineCount - 1)
                    {
                        newEnd += newCalculatedEnd;
                    }
                    else
                    {
                        newEnd = newCalculatedEnd;
                    }

                    if (newCalculatedEnd == currentLine.Length)
                    {
                        newEnd = 0;
                    }
                    else
                    {
                        removedLines--;
                    }

                    break;
                }
            }

            var newLines = temp.lines.Slice(0, temp.lines.Length - removedLines);
            newStart = temp.start;
            if (newLines.Length == 0)
            {
                newStart = 0;
                newEnd = 0;
            }

            var lineBlock = new LineBlock(newLines, temp.text, newStart, newEnd);

            System.Diagnostics.Debug.Assert(lineBlock.TextLength <= this.TextLength, "TextLength must be less then or equals this");
            return lineBlock;
        }

        public LineBlock TrimEnd()
        {
            for (int i = this.LineCount - 1; i >= 0; i--)
            {
                var currentLine = this[i];
                // find the last line that has text.
                if (!currentLine.IsWhiteSpace())
                {
                    var lastLine = this[i];
                    var trimed = lastLine.TrimEnd();
                    var lastLineEntry = this.lines[i];
                    var newFromEnd = lastLineEntry.length - trimed.Length;

                    var lineBlock = new LineBlock(this.lines.Slice(0, i + 1), this.text, this.start, newFromEnd);

                    System.Diagnostics.Debug.Assert(lineBlock.TextLength <= this.TextLength, "TextLength must be less then or equals this");
                    return lineBlock;
                }
            }

            return default;
        }

        public LineBlock TrimStart()
        {
            for (int i = 0; i < this.LineCount; i++)
            {
                var currentLine = this[i];
                // find the last line that has text.
                if (!currentLine.IsWhiteSpace())
                {
                    var lastLine = this[i];
                    var trimed = lastLine.TrimStart();
                    var lastLineEntry = this.lines[i];
                    var newStart = lastLineEntry.length - trimed.Length;

                    // when this is the last line we may not forget fromEnd
                    if (i == this.LineCount - 1)
                    {
                        newStart -= this.fromEnd;
                    }

                    var lineBlock = new LineBlock(this.lines.Slice(i), this.text, newStart, this.fromEnd);

                    System.Diagnostics.Debug.Assert(lineBlock.TextLength <= this.TextLength, "TextLength must be less then or equals this");
                    return lineBlock;
                }
            }

            return default;
        }

        /// <summary>
        /// Returns the position of the string.
        /// </summary>
        /// <param name="value">The text to search.</param>
        /// <returns>The line and index in the line.</returns>
        public LineBlockPosition IndexOf(ReadOnlySpan<char> value)
        {
            return this.IndexOf(value, StringComparison.InvariantCulture);
        }

        /// <summary>
        /// Returns the position of the string.
        /// </summary>
        /// <param name="value">The text to search.</param>
        /// <param name="comparisonType">Defines the comparision.</param>
        /// <returns>The line and index in the line.</returns>
        public LineBlockPosition IndexOf(ReadOnlySpan<char> value, StringComparison comparisonType)
        {
            int lengthOfPreviouseLies = 0;
            for (int i = 0; i < this.LineCount; i++)
            {
                var index = this[i].IndexOf(value, comparisonType);
                if (index >= 0)
                {
                    return new LineBlockPosition(i, index, index + lengthOfPreviouseLies);
                }

                lengthOfPreviouseLies += this[i].Length;
            }

            return LineBlockPosition.NotFound;
        }

        /// <summary>
        /// Returns the position of any supled chars.
        /// </summary>
        /// <param name="value">The text to search.</param>
        /// <param name="fromPosition">The position from where to start the search.</param>
        /// <returns>The line and index in the line.</returns>
        public LineBlockPosition IndexOfAny(ReadOnlySpan<char> value, LineBlockPosition fromPosition)
        {
            int lengthOfPreviouseLies = 0;
            for (int i = 0; i < fromPosition.Line; i++)
            {
                lengthOfPreviouseLies += this[i].Length;
            }

            for (int i = fromPosition.Line; i < this.LineCount; i++)
            {
                int index;
                if (i == fromPosition.Line)
                {
                    index = this[i].Slice(fromPosition.Column).IndexOfAny(value) + fromPosition.Column;
                    if (index < fromPosition.Column)
                    {
                        index = -1;
                    }
                }
                else
                {
                    index = this[i].IndexOfAny(value);
                }

                if (index >= 0)
                {
                    return new LineBlockPosition(i, index, index + lengthOfPreviouseLies);
                }

                lengthOfPreviouseLies += this[i].Length;
            }

            return LineBlockPosition.NotFound;
        }

        /// <summary>
        /// Returns the position of the string.
        /// </summary>
        /// <param name="value">The text to search.</param>
        /// <returns>The line and index in the line.</returns>
        public LineBlockPosition IndexOf(char value)
        {
            ReadOnlySpan<char> toSearch = stackalloc char[]
            {
                value,
            };

            return this.IndexOf(toSearch);
        }
    }
}