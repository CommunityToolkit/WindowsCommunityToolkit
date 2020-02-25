// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Microsoft.Toolkit.Parsers.Markdown
{
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
                else if (line == LineCount - 1)
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
        /// <returns>The concatinated string.</returns>
        public override string ToString()
        {
            var bufferSize = this.TextLength + this.LineCount - 1;
            Span<char> buffer = bufferSize < 1024
                ? stackalloc char[bufferSize]
                : new char[bufferSize];

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

            return buffer.ToString();
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
        /// <param name="length">The length.</param>
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
            return SliceLines(start, this.LineCount - start);
        }

        /// <summary>
        /// Will remove a specific number of characters from the start of each line.
        /// If a line has less charaters then removed the line will have 0 characers.
        /// </summary>
        /// <param name="length">The number of characters to remove.</param>
        /// <returns>A new Instance of LineBlock with the lines modified.</returns>
        public LineBlock RemoveFromLineStart(int length)
        {
            throw new NotImplementedException("Need to rewrite");
            var temp = new (int start, int length)[this.lines.Length];

            for (int i = 0; i < temp.Length; i++)
            {
                ref var toSet = ref temp[i];
                toSet = this.lines[i];
                toSet.length -= length;
                if (toSet.length < 0)
                {
                    toSet.length = 0;
                }
                else
                {
                    toSet.start += length;
                }
            }

            //return new LineBlock(temp.AsSpan(), this.text);
        }

        /// <summary>
        /// Will remove a specific number of characters from the end of each line.
        /// If a line has less charaters then removed the line will have 0 characers.
        /// </summary>
        /// <param name="length">The number of characters to remove.</param>
        /// <returns>A new Instance of LineBlock with the lines modified.</returns>
        public LineBlock RemoveFromLineEnd(int length)
        {
            throw new NotImplementedException("Need to rewrite");
            var temp = new (int start, int length)[this.lines.Length];

            for (int i = 0; i < temp.Length; i++)
            {
                ref var toSet = ref temp[i];
                toSet = this.lines[i];
                toSet.length -= length;
                if (toSet.length < 0)
                {
                    toSet.length = 0;
                }
            }

            //return new LineBlock(temp.AsSpan(), this.text);
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
                }

                if (i == temp.Length - 1)
                {
                    toSet.length -= this.fromEnd;
                }

            }

            return new LineBlock(temp.AsSpan(), this.text, 0, 0);
        }

        /// <summary>
        /// Removes A specific amouts of charactesr from the beginning. Empty lines will be removed.
        /// </summary>
        /// <param name="start">The number of characters.</param>
        /// <returns>The modified Block.</returns>
        public LineBlock Slice(int start, int length)
        {

            var removedLines = 0;
            var newStart = 0;
            var newEnd = 0;
            for (int i = 0; i < this.lines.Length; i++)
            {
                ref readonly var currentLine = ref this.lines[i];

                if (start > currentLine.length)
                {
                    start -= currentLine.length;
                    removedLines++;
                }
                else
                {
                    newStart = start;
                    if (newStart == 0)
                    {
                        removedLines++;
                    }

                    break;
                }
            }

            var temp = new LineBlock(lines.Slice(removedLines), this.text, newStart, this.fromEnd);
            removedLines = 0;
            for (int i = 0; i < temp.lines.Length; i++)
            {
                ref readonly var currentLine = ref temp.lines[i];

                if (length > currentLine.length)
                {
                    length -= currentLine.length;
                    removedLines++;
                }
                else
                {
                    newEnd = currentLine.length - length;
                    if (newEnd == currentLine.length)
                    {
                        removedLines++;
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
        public (int line, int posInLine) IndexOf(ReadOnlySpan<char> value)
        {
            for (int i = 0; i < this.LineCount; i++)
            {
                var index = this[i].IndexOf(value);
                if (index >= 0)
                {
                    return (i, index);
                }
            }

            return (-1, -1);
        }

        /// <summary>
        /// Returns the position of the string.
        /// </summary>
        /// <param name="value">The text to search.</param>
        /// <param name="comparisonType">Defines the comparision.</param>
        /// <returns>The line and index in the line.</returns>
        public (int line, int posInLine) IndexOf(ReadOnlySpan<char> value, StringComparison comparisonType)
        {
            for (int i = 0; i < this.LineCount; i++)
            {
                var index = this[i].IndexOf(value, comparisonType);
                if (index >= 0)
                {
                    return (i, index);
                }
            }

            return (-1, -1);
        }

        /// <summary>
        /// Returns the position of the string.
        /// </summary>
        /// <param name="value">The text to search.</param>
        /// <returns>The line and index in the line.</returns>
        public (int line, int posInLine) IndexOf(char value)
        {
            for (int i = 0; i < this.LineCount; i++)
            {
                var index = this[i].IndexOf(value);
                if (index >= 0)
                {
                    return (i, index);
                }
            }

            return (-1, -1);
        }
    }

    /// <summary>
    /// Callback provides new start and length for the line.
    /// </summary>
    /// <param name="line">The original line.</param>
    /// <param name="lineNumber">The index of the Line.</param>
    /// <returns>A tuple containing start and length of the line.</returns>
    public delegate (int start, int length) RemoveLineCallback(ReadOnlySpan<char> line, int lineNumber);
}