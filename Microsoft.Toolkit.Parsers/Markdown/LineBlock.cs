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

        /// <summary>
        /// Gets the number of lines in this Block.
        /// </summary>
        public int LineCount => this.lines.Length;

        /// <summary>
        /// Gets a single line.
        /// </summary>
        public LineWrapper Lines => new LineWrapper(this);

        /// <summary>
        /// Gets the number of characters of this text. (Without counting linbreaks).
        /// </summary>
        public int TextLength
        {
            get
            {
                int length = 0;
                for (int i = 0; i < lines.Length; i++)
                {
                    length += lines[i].length;
                }

                return length;
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
            for (int i = 0; i < lines.Length; i++)
            {
                var from = Lines[i];
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
        /// Allows access to the lines.
        /// </summary>
        public readonly ref struct LineWrapper
        {
            private readonly LineBlock parent;

            internal LineWrapper(LineBlock parent)
            {
                this.parent = parent;
            }

            /// <summary>
            /// Gets the selected line.
            /// </summary>
            /// <param name="line">The index of the line.</param>
            /// <returns>The Line.</returns>
            public ReadOnlySpan<char> this[int line] => this.parent.text.Slice(this.parent.lines[line].start, this.parent.lines[line].length);
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

        private LineBlock(ReadOnlySpan<(int start, int length)> lines, ReadOnlySpan<char> text)
        {
            this.lines = lines;
            this.text = text;
        }

        /// <summary>
        /// Slices the Lines.
        /// </summary>
        /// <param name="start">The startindex.</param>
        /// <param name="length">The length.</param>
        /// <returns>A new LineBlock that will only have the lines specified.</returns>
        public LineBlock Slice(int start, int length)
        {
            return new LineBlock(this.lines.Slice(start, length), this.text);
        }

        /// <summary>
        /// Slices the Lines.
        /// </summary>
        /// <param name="start">The startindex.</param>
        /// <returns>A new LineBlock that will only have the lines specified.</returns>
        public LineBlock Slice(int start)
        {
            return new LineBlock(this.lines.Slice(start), this.text);
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
                if (toSet.length < 0)
                {
                    toSet.length = 0;
                }
                else
                {
                    toSet.start += length;
                }
            }

            return new LineBlock(temp.AsSpan(), this.text);
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
                if (toSet.length < 0)
                {
                    toSet.length = 0;
                }
            }

            return new LineBlock(temp.AsSpan(), this.text);
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

                var (newStart, newLength) = callback(this.Lines[i], i);

                if (newStart < 0 || newStart >= from.length || newLength + newLength > from.length)
                {
                    throw new ArgumentOutOfRangeException($"The supplied argument were out of range. New string must <={from.length}");
                }

                toSet.start = newStart;
                toSet.length = newLength;
            }

            return new LineBlock(temp.AsSpan(), this.text);
        }

        /// <summary>
        /// Removes A specific amouts of charactesr from the beginning. Empty lines will be removed.
        /// </summary>
        /// <param name="start">The number of characters.</param>
        /// <returns>The modified Block.</returns>
        public LineBlock RemoveFromStart(int start)
        {
            var temp = new (int start, int length)[this.lines.Length];
            var removedLines = 0;
            for (int i = 0; i < temp.Length; i++)
            {
                ref var toSet = ref temp[i];
                toSet = this.lines[i];

                if (start > toSet.length)
                {
                    start -= toSet.length;
                    removedLines++;
                }
                else
                {
                    toSet.start += start;
                    toSet.length -= start;
                    if (toSet.length == 0)
                    {
                        removedLines++;
                    }

                    break;
                }
            }

            return new LineBlock(temp.AsSpan(removedLines), this.text);
        }

        /// <summary>
        /// Removes A specific amouts of charactesr from the beginning. Empty lines will be removed.
        /// </summary>
        /// <param name="end">The number of characters.</param>
        /// <returns>The modified Block.</returns>
        public LineBlock RemoveFromEnd(int end)
        {
            var temp = new (int start, int length)[this.lines.Length];
            var removedLines = 0;
            for (int i = temp.Length - 1; i >= 0; i--)
            {
                ref var toSet = ref temp[i];
                toSet = this.lines[i];

                if (end > toSet.length)
                {
                    end -= toSet.length;
                    removedLines++;
                }
                else
                {
                    toSet.length -= end;
                    if (toSet.length == 0)
                    {
                        removedLines++;
                    }

                    break;
                }
            }

            return new LineBlock(temp.AsSpan(0, temp.Length - removedLines), this.text);
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
                var index = this.Lines[i].IndexOf(value);
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
                var index = this.Lines[i].IndexOf(value, comparisonType);
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
                var index = this.Lines[i].IndexOf(value);
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