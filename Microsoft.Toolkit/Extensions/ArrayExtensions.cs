// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Microsoft.Toolkit.Extensions
{
    /// <summary>
    /// Helpers for working with arrays.
    /// </summary>
    public static class ArrayExtensions
    {
        /// <summary>
        /// Fills elements of a rectangular array at the given position and size to a specific value.
        /// Ranges given will fill in as many elements as possible, ignoring positions outside the bounds of the array.
        /// </summary>
        /// <typeparam name="T">The element type of the array.</typeparam>
        /// <param name="array">The source array.</param>
        /// <param name="value">Value to fill with.</param>
        /// <param name="row">Row to start on (inclusive, zero-index).</param>
        /// <param name="col">Column to start on (inclusive, zero-index).</param>
        /// <param name="width">Positive width of area to fill.</param>
        /// <param name="height">Positive height of area to fill.</param>
        [Obsolete("This helper will be removed in a future release, use the APIs from Microsoft.Toolkit.HighPerformance to replace it")]
        public static void Fill<T>(this T[,] array, T value, int row, int col, int width, int height)
        {
            for (int r = row; r < row + height; r++)
            {
                for (int c = col; c < col + width; c++)
                {
                    if (r >= 0 && c >= 0 && r < array.GetLength(0) && c < array.GetLength(1))
                    {
                        array[r, c] = value;
                    }
                }
            }
        }

        /// <summary>
        /// Yields a row from a rectangular array.
        /// </summary>
        /// <typeparam name="T">The element type of the array.</typeparam>
        /// <param name="rectarray">The source array.</param>
        /// <param name="row">Row record to retrieve, 0-based index.</param>
        /// <returns>Yielded row.</returns>
        [Obsolete("This helper will be removed in a future release, use the APIs from Microsoft.Toolkit.HighPerformance to replace it")]
        public static IEnumerable<T> GetRow<T>(this T[,] rectarray, int row)
        {
            if (row < 0 || row >= rectarray.GetLength(0))
            {
                throw new ArgumentOutOfRangeException(nameof(row));
            }

            for (int c = 0; c < rectarray.GetLength(1); c++)
            {
                yield return rectarray[row, c];
            }
        }

        /// <summary>
        /// Yields a column from a rectangular array.
        /// </summary>
        /// <typeparam name="T">The element type of the array.</typeparam>
        /// <param name="rectarray">The source array.</param>
        /// <param name="column">Column record to retrieve, 0-based index.</param>
        /// <returns>Yielded column.</returns>
        [Obsolete("This helper will be removed in a future release, use the APIs from Microsoft.Toolkit.HighPerformance to replace it")]
        public static IEnumerable<T> GetColumn<T>(this T[,] rectarray, int column)
        {
            if (column < 0 || column >= rectarray.GetLength(1))
            {
                throw new ArgumentOutOfRangeException(nameof(column));
            }

            for (int r = 0; r < rectarray.GetLength(0); r++)
            {
                yield return rectarray[r, column];
            }
        }

        /// <summary>
        /// Yields a column from a jagged array.
        /// An exception will be thrown if the column is out of bounds, and return default in places where there are no elements from inner arrays.
        /// Note: There is no equivalent GetRow method, as you can use array[row] to retrieve.
        /// </summary>
        /// <typeparam name="T">The element type of the array.</typeparam>
        /// <param name="rectarray">The source array.</param>
        /// <param name="column">Column record to retrieve, 0-based index.</param>
        /// <returns>Yielded enumerable of column elements for given column, and default values for smaller inner arrays.</returns>
        [Obsolete("This helper will be removed in a future release, use the APIs from Microsoft.Toolkit.HighPerformance to replace it")]
        public static IEnumerable<T> GetColumn<T>(this T[][] rectarray, int column)
        {
            if (column < 0 || column >= rectarray.Max(array => array.Length))
            {
                throw new ArgumentOutOfRangeException(nameof(column));
            }

            for (int r = 0; r < rectarray.GetLength(0); r++)
            {
                if (column >= rectarray[r].Length)
                {
                    yield return default(T);

                    continue;
                }

                yield return rectarray[r][column];
            }
        }

        /// <summary>
        /// Returns a simple string representation of an array.
        /// </summary>
        /// <typeparam name="T">The element type of the array.</typeparam>
        /// <param name="array">The source array.</param>
        /// <returns>The <see cref="string"/> representation of the array.</returns>
        public static string ToArrayString<T>(this T[] array)
        {
            // The returned string will be in the following format:
            // [1, 2,  3]
            StringBuilder builder = new StringBuilder();

            builder.Append('[');

            for (int i = 0; i < array.Length; i++)
            {
                if (i != 0)
                {
                    builder.Append(",\t");
                }

                builder.Append(array[i].ToString());
            }

            builder.Append(']');

            return builder.ToString();
        }

        /// <summary>
        /// Returns a simple string representation of a jagged array.
        /// </summary>
        /// <typeparam name="T">The element type of the array.</typeparam>
        /// <param name="mdarray">The source array.</param>
        /// <returns>String representation of the array.</returns>
        public static string ToArrayString<T>(this T[][] mdarray)
        {
            // The returned string uses the same format as the overload for 2D arrays
            StringBuilder builder = new StringBuilder();

            builder.Append('[');

            for (int i = 0; i < mdarray.Length; i++)
            {
                if (i != 0)
                {
                    builder.Append(',');
                    builder.Append(Environment.NewLine);
                    builder.Append(' ');
                }

                builder.Append('[');

                T[] row = mdarray[i];

                for (int j = 0; j < row.Length; j++)
                {
                    if (j != 0)
                    {
                        builder.Append(",\t");
                    }

                    builder.Append(row[j].ToString());
                }

                builder.Append(']');
            }

            builder.Append(']');

            return builder.ToString();
        }

        /// <summary>
        /// Returns a simple string representation of a 2D array.
        /// </summary>
        /// <typeparam name="T">The element type of the array.</typeparam>
        /// <param name="array">The source array.</param>
        /// <returns>The <see cref="string"/> representation of the array.</returns>
        public static string ToArrayString<T>(this T[,] array)
        {
            // The returned string will be in the following format:
            // [[1, 2,  3],
            //  [4, 5,  6],
            //  [7, 8,  9]]
            StringBuilder builder = new StringBuilder();

            builder.Append('[');

            int
                height = array.GetLength(0),
                width = array.GetLength(1);

            for (int i = 0; i < height; i++)
            {
                if (i != 0)
                {
                    builder.Append(',');
                    builder.Append(Environment.NewLine);
                    builder.Append(' ');
                }

                builder.Append('[');

                for (int j = 0; j < width; j++)
                {
                    if (j != 0)
                    {
                        builder.Append(",\t");
                    }

                    builder.Append(array[i, j].ToString());
                }

                builder.Append(']');
            }

            builder.Append(']');

            return builder.ToString();
        }
    }
}
