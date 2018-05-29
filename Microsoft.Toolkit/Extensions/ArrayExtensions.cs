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
    /// Helper extension methods for Arrays.
    /// </summary>
    public static class ArrayExtensions
    {
        /// <summary>
        /// Fills in values of a multi-dimensional rectangular array to specified value based on the position and size given.
        /// Ranges given outside the bounds of the array will fill in as much as possible and ignore elements that should appear outside it.
        /// Won't throw bounds exception, just won't do work if ranges out of bounds.
        /// </summary>
        /// <typeparam name="T">Type of array values.</typeparam>
        /// <param name="array">Extended type instance.</param>
        /// <param name="value">Value to fill with.</param>
        /// <param name="row">Row to start on (inclusive, zero-index).</param>
        /// <param name="col">Column to start on (inclusive, zero-index).</param>
        /// <param name="width">Positive Width of area to fill.</param>
        /// <param name="height">Positive Height of area to fill.</param>
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
        /// Retrieve a row as an enumerable from a multi-dimensional rectangular array.
        /// </summary>
        /// <typeparam name="T">Type of rectangular array.</typeparam>
        /// <param name="rectarray">Extended type instance.</param>
        /// <param name="row">Row record to retrieve, 0-based index.</param>
        /// <returns>Yielded Enumerable of results.</returns>
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
        /// Retrieve a column from a multi-dimensional rectangular array.
        /// </summary>
        /// <typeparam name="T">Type of rectangular array.</typeparam>
        /// <param name="rectarray">Extended type instance.</param>
        /// <param name="column">Column record to retrieve, 0-based index.</param>
        /// <returns>Yielded Enumerable of results.</returns>
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
        /// Retrieve a column from a multi-dimensional jagged array.
        /// Will throw an exception if the column is out of bounds, and return default in places where there are no elements from inner arrays.
        /// Note: No equivalent GetRow method, as you can use array[row] to retrieve.
        /// </summary>
        /// <typeparam name="T">Type of jagged array.</typeparam>
        /// <param name="rectarray">Extended type instance.</param>
        /// <param name="column">Column record to retrieve, 0-based index.</param>
        /// <returns>Yielded Enumerable of column elements for given column and default values for smaller inner arrays.</returns>
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
        /// Joins the array together in a simple string representation.
        /// </summary>
        /// <typeparam name="T">Type of array.</typeparam>
        /// <param name="array">Extended type instance.</param>
        /// <returns>String representation of array.</returns>
        public static string ToArrayString<T>(this T[] array)
        {
            return "[" + string.Join(",\t", array) + "]";
        }

        /// <summary>
        /// Joins the multi-dimensional array together in a string representation.
        /// </summary>
        /// <typeparam name="T">Type of jagged array.</typeparam>
        /// <param name="mdarray">Extended type instance.</param>
        /// <returns>String representation of array.</returns>
        public static string ToArrayString<T>(this T[][] mdarray)
        {
            string[] inner = new string[mdarray.GetLength(0)];

            for (int r = 0; r < mdarray.GetLength(0); r++)
            {
                inner[r] = string.Join(",\t", mdarray[r]);
            }

            return "[[" + string.Join("]," + Environment.NewLine + " [", inner) + "]]";
        }

        /// <summary>
        /// Joins the rectangular-array together in a string representation.
        /// </summary>
        /// <typeparam name="T">Type of rectangular array.</typeparam>
        /// <param name="rectarray">Extended type instance.</param>
        /// <returns>String representation of array.</returns>
        public static string ToArrayString<T>(this T[,] rectarray)
        {
            string[] inner = new string[rectarray.GetLength(0)];

            for (int r = 0; r < rectarray.GetLength(0); r++)
            {
                inner[r] = string.Join(",\t", rectarray.GetRow(r));
            }

            return "[[" + string.Join("]," + Environment.NewLine + " [", inner) + "]]";
        }
    }
}
