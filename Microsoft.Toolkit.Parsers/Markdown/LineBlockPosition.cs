// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;

namespace Microsoft.Toolkit.Parsers.Markdown
{
    /// <summary>
    /// Specifys the position in a <see cref="LineBlock"/>.
    /// </summary>
    [System.Diagnostics.DebuggerDisplay("({Line},{Column},{FromStart})")]
    public readonly struct LineBlockPosition : IEquatable<LineBlockPosition>
    {
        internal static readonly LineBlockPosition NotFound = new LineBlockPosition(-1, -1, -1);

        /// <summary>
        /// Initializes a new instance of the <see cref="LineBlockPosition"/> struct.
        /// </summary>
        /// <param name="line">The line index.</param>
        /// <param name="column">The column.</param>
        /// <param name="fromStart">The index from the beginning of the block.</param>
        public LineBlockPosition(int line, int column, int fromStart)
        {
            this.Line = line;
            this.Column = column;
            this.FromStart = fromStart;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LineBlockPosition"/> struct. Calculates FromStart.
        /// </summary>
        /// <param name="line">The line index.</param>
        /// <param name="column">The coulmn.</param>
        /// <param name="block">The Lineblock for which FromStart should be calculated.</param>
        public LineBlockPosition(int line, int column, in LineBlock block)
        {
            this.Line = line;
            this.Column = column;
            this.FromStart = 0;
            for (int i = 0; i < this.Line; i++)
            {
                this.FromStart += block[i].Length + 1;
            }

            this.FromStart += this.Column;
        }

        /// <summary>
        /// Checks if this position is valid in a <see cref="LineBlock"/>.
        /// </summary>
        /// <param name="block">The LineBlock.</param>
        /// <returns>True if the position is valid. false otherwise.</returns>
        public bool IsIn(in LineBlock block)
        {
            return this.Line >= 0
                && this.Column >= 0
                && this.Line < block.LineCount
                && this.Column <= block[this.Line].Length
                && this.FromStart < block.TextLength;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LineBlockPosition"/> struct. Calculates Line and Column.
        /// </summary>
        /// <param name="fromStart">The index from the beginning of the block.</param>
        /// <param name="block">The Lineblock for which FromStart should be calculated.</param>
        public LineBlockPosition(int fromStart, in LineBlock block)
        {
            this.FromStart = fromStart;
            this.Line = 0;
            this.Column = -1;
            var currentProgress = 0;
            for (int i = 0; i < block.LineCount; i++)
            {
                var currentLineLength = block[i].Length;
                if (currentProgress + currentLineLength > FromStart)
                {
                    this.Column = currentLineLength - (FromStart - currentProgress);
                }
                else
                {
                    currentProgress += currentLineLength + 1;
                    this.Line += 1;
                }
            }
        }

        /// <summary>
        /// Gets the index of the Line.
        /// </summary>
        public int Line { get; }

        /// <summary>
        /// Gets the index in the line.
        /// </summary>
        public int Column { get; }

        /// <summary>
        /// Gets the index from start of the Block.
        /// </summary>
        public int FromStart { get; }

        /// <inheritdoc/>
        public override bool Equals(object obj)
        {
            return obj is LineBlockPosition position && this.Equals(position);
        }

        /// <inheritdoc/>
        public bool Equals(LineBlockPosition other)
        {
            return this.Line == other.Line &&
                   this.Column == other.Column &&
                   this.FromStart == other.FromStart;
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            var hashCode = 278512901;
            hashCode = (hashCode * -1521134295) + this.Line.GetHashCode();
            hashCode = (hashCode * -1521134295) + this.Column.GetHashCode();
            hashCode = (hashCode * -1521134295) + this.FromStart.GetHashCode();
            return hashCode;
        }

        /// <summary>
        /// Checks for Equality.
        /// </summary>
        public static bool operator ==(LineBlockPosition left, LineBlockPosition right)
        {
            return left.Equals(right);
        }

        /// <summary>
        /// Checks for Inequality.
        /// </summary>
        public static bool operator !=(LineBlockPosition left, LineBlockPosition right)
        {
            return !(left == right);
        }

        internal LineBlockPosition Add(int toAdd, LineBlock markdown)
        {
            if (this.Column + toAdd <= markdown[this.Line].Length || this.Line == markdown.LineCount - 1)
            {
                return new LineBlockPosition(this.Line, this.Column + toAdd, this.FromStart + toAdd);
            }
            else
            {
                var diff = toAdd - (markdown[this.Line].Length - this.Column) - 1;

                return new LineBlockPosition(this.Line + 1, 0, this.FromStart + (toAdd - diff)).Add(diff, markdown);
            }
        }
    }
}