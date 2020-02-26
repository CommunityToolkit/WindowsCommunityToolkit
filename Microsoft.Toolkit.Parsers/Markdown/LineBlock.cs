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
    public delegate (int start, int length) RemoveLineCallback(ReadOnlySpan<char> line, int lineNumber);

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
                if (line == 0)
                {
                    return this.text.Slice(this.lines[line].start + this.start, this.lines[line].length - this.start);
                }
                else if (line == this.LineCount - 1)
                {
                    return this.text.Slice(this.lines[line].start, this.lines[line].length - this.fromEnd);
                }

                return this.text.Slice(this.lines[line].start, this.lines[line].length);
            }
        }

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

            for (int i = 0; i < this.lines.Length; i++)
            {
                var from = this[i];

                builder.Append(from);
                builder.AppendLine();
            }

            return builder;
        }

        /// <summary>
        /// Concatinates the lines and uses \n as an line seperator.
        /// </summary>
        /// <returns>The concatinated string.</returns>
        public override string ToString()
        {
            const int MAX_STACK_BUFFER_SIZE = 1024;
            var bufferSize = this.TextLength + this.LineCount - 1;
            char[] arrayBuffer;
            if (bufferSize <= MAX_STACK_BUFFER_SIZE)
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
                    buffer[index] = '\n';
                    index++;
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
        }

        private LineBlock(ReadOnlySpan<(int start, int length)> lines, ReadOnlySpan<char> text, int start, int fromEnd)
        {
            this.lines = lines;
            this.text = text;
            this.start = start;
            this.fromEnd = fromEnd;
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

            if (length == this.LineCount - start - 1)
            {
                endModification = this.fromEnd;
            }
            else
            {
                endModification = 0;
            }

            return new LineBlock(this.lines.Slice(start, length), this.text, startModification, endModification);
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

            return new LineBlock(temp.AsSpan(), this.text, 0, 0);
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

            return new LineBlock(temp.AsSpan(), this.text, 0, 0);
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
            for (int i = 0; i < temp.Length; i++)
            {
                ref readonly var from = ref this.lines[i];
                ref var toSet = ref temp[i];

                var (newStart, newLength) = callback(this[i], i);

                if (newStart < 0 || newStart >= from.length || newLength + newLength > from.length)
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

            return new LineBlock(temp.AsSpan(), this.text, 0, 0);
        }

        /// <summary>
        /// Removes A specific amouts of charactesr. Empty lines will be removed.
        /// </summary>
        /// <param name="start">The position from where characters will be kept.</param>
        /// <returns>The modified Block.</returns>
        public LineBlock Slice(int start) => this.Slice(start, -1);

        /// <summary>
        /// Removes A specific amouts of charactesr. Empty lines will be removed.
        /// </summary>
        /// <param name="start">The position from where characters will be kept.</param>
        /// <returns>The modified Block.</returns>
        public LineBlock Slice(LineBlockPosition start) => this.Slice(start, -1);

        /// <summary>
        /// Removes A specific amouts of charactesr. Empty lines will be removed.
        /// </summary>
        /// <param name="start">The position from where characters will be kept.</param>
        /// <param name="length">The number of characters taken.</param>
        /// <returns>The modified Block.</returns>
        public LineBlock Slice(LineBlockPosition start, int length)
        {
            // it is more prformant to remove the lines first.
            return this.SliceLines(start.Line).Slice(start.Column).Slice(0, length);
        }

        /// <summary>
        /// Removes A specific amouts of charactesr. Empty lines will be removed.
        /// </summary>
        /// <param name="start">The position from where characters will be kept.</param>
        /// <param name="length">The number of characters taken.</param>
        /// <returns>The modified Block.</returns>
        public LineBlock Slice(int start, int length)
        {
            if (start + length > this.TextLength)
            {
                throw new ArgumentOutOfRangeException();
            }

            var removedLines = 0;
            var newStart = this.start;
            var newEnd = 0;
            LineBlock temp;
            if (start != 0)
            {
                for (int i = 0; i < this.lines.Length; i++)
                {
                    ref readonly var currentLine = ref this.lines[i];

                    if (start >= currentLine.length)
                    {
                        start -= currentLine.length;
                        removedLines++;
                    }
                    else
                    {
                        newStart += start;
                        if (newStart == currentLine.length)
                        {
                            removedLines++;
                            newStart = 0;
                        }

                        break;
                    }
                }

                temp = new LineBlock(this.lines.Slice(removedLines), this.text, newStart, this.fromEnd);
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
                ref readonly var currentLine = ref temp.lines[i];

                if (length >= currentLine.length)
                {
                    if (i == 0)
                    {
                        length -= currentLine.length - temp.start;
                    }
                    else
                    {
                        length -= currentLine.length;
                    }

                    removedLines--;
                }
                else
                {
                    newEnd = currentLine.length - length;
                    if (newEnd == currentLine.length)
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

            return new LineBlock(temp.lines.Slice(0, temp.lines.Length - removedLines), temp.text, temp.start, newEnd);
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