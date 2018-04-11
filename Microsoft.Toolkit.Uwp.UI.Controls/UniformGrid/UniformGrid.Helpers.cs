// ******************************************************************
// Copyright (c) Microsoft. All rights reserved.
// This code is licensed under the MIT License (MIT).
// THE CODE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
// TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH
// THE CODE OR THE USE OR OTHER DEALINGS IN THE CODE.
// ******************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

[assembly: InternalsVisibleTo("UnitTests")]
namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    /// <summary>
    /// The UniformGrid control presents information within a Grid with even spacing.
    /// </summary>
    public partial class UniformGrid : Grid
    {
        // Provides the next spot in the boolean array with a 'false' value.
        #pragma warning disable SA1009 // Closing parenthesis must be followed by a space.
        internal static IEnumerable<(int row, int column)> GetFreeSpot(bool[,] array, int firstcolumn, bool topdown)
        #pragma warning restore SA1009 // Closing parenthesis must be followed by a space.
        {
            if (topdown)
            {
                // Layout spots from Top-Bottom, Left-Right (right-left handled automatically by Grid with Flow-Direction).
                // Effectively transpose the Grid Layout.
                for (int c = 0; c < array.GetLength(1); c++)
                {
                    int start = (c == 0 && firstcolumn > 0) ? firstcolumn : 0;
                    for (int r = start; r < array.GetLength(0); r++)
                    {
                        if (!array[r, c])
                        {
                            yield return (r, c);
                        }
                    }
                }
            }
            else
            {
                // Layout spots as normal from Left-Right.
                // (right-left handled automatically by Grid with Flow-Direction
                // during its layout, internal model is always left-right).
                for (int r = 0; r < array.GetLength(0); r++)
                {
                    int start = (r == 0 && firstcolumn > 0) ? firstcolumn : 0;
                    for (int c = start; c < array.GetLength(1); c++)
                    {
                        if (!array[r, c])
                        {
                            yield return (r, c);
                        }
                    }
                }
            }
        }

        // Based on the number of visible children,
        // returns the dimensions of the
        // grid we need to hold all elements.
        #pragma warning disable SA1008 // Opening parenthesis must be spaced correctly
        internal static (int rows, int columns) GetDimensions(ref IEnumerable<FrameworkElement> visible, int rows, int cols, int firstColumn) //// TODO: Switch to 'in' for C# 7.2
        #pragma warning restore SA1008 // Opening parenthesis must be spaced correctly
        {
            // If a dimension isn't specified, we need to figure out the other one (or both).
            if (rows == 0 || cols == 0)
            {
                // Calculate the size & area of all objects in the grid to know how much space we need.
                // TODO: Need to trim size of objects that go out of bounds?
                // TODO: Do we need to worry if there aren't enough small items to fill in the gaps?
                var count = Math.Max(1, visible.Sum(item => GetRowSpan(item) * GetColumnSpan(item)));

                if (rows == 0)
                {
                    if (cols > 0)
                    {
                        var first = Math.Min(firstColumn, cols - 1); // Bound check

                        // If we have columns but no rows, calculate rows based on column offset and number of children.
                        rows = (count + first + (cols - 1)) / cols;
                        return (rows, cols);
                    }

                    // Otherwise, determine square layout if both are zero.
                    var size = (int)Math.Ceiling(Math.Sqrt(count));

                    // Figure out if firstColumn in bounds
                    var first2 = Math.Min(firstColumn, size - 1); // Bound check

                    rows = (int)Math.Ceiling(Math.Sqrt(count + first2));
                    return (rows, rows);
                }
                else if (cols == 0)
                {
                    // If we have rows and no columns, then calculate columns needed based on rows
                    cols = (count + (rows - 1)) / rows;

                    // Now that we know a rough size of our shape, see if the FirstColumn effects that:
                    var first = Math.Min(firstColumn, cols - 1); // Bound check

                    cols = (count + first + (rows - 1)) / rows;
                }
            }

            return (rows, cols);
        }

        // Used to interleave specified row dimensions with automatic rows added to use
        // underlying Grid layout for main arrange of UniformGrid.
        internal void SetupRowDefinitions(int rows)
        {
            // Mark existing dev-defined definitions so we don't erase them.
            foreach (var rd in RowDefinitions)
            {
                if (GetAutoLayout(rd) == null)
                {
                    SetAutoLayout(rd, false);

                    // If we don't have our attached property, assign it based on index.
                    if (GetRow(rd) == 0)
                    {
                        SetRow(rd, RowDefinitions.IndexOf(rd));
                    }
                }
            }

            // Remove non-autolayout rows we've added and then add them in the right spots.
            if (rows != RowDefinitions.Count)
            {
                for (int r = RowDefinitions.Count - 1; r >= 0; r--)
                {
                    if (GetAutoLayout(RowDefinitions[r]) == true)
                    {
                        RowDefinitions.RemoveAt(r);
                    }
                }

                for (int r = 0; r < rows; r++)
                {
                    if (!(this.RowDefinitions.Count >= r + 1 && GetRow(RowDefinitions[r]) == r))
                    {
                        var rd = new RowDefinition();
                        SetAutoLayout(rd, true);
                        this.RowDefinitions.Insert(r, rd);
                    }
                }
            }
        }

        // Used to interleave specified column dimensions with automatic columns added to use
        // underlying Grid layout for main arrange of UniformGrid.
        internal void SetupColumnDefinitions(int columns)
        {
            foreach (var cd in ColumnDefinitions)
            {
                if (GetAutoLayout(cd) == null)
                {
                    SetAutoLayout(cd, false);

                    // If we don't have our attached property, assign it based on index.
                    if (GetColumn(cd) == 0)
                    {
                        SetColumn(cd, ColumnDefinitions.IndexOf(cd));
                    }
                }
            }

            // Remove non-autolayout columns we've added and then add them in the right spots.
            if (columns != ColumnDefinitions.Count)
            {
                for (int c = ColumnDefinitions.Count - 1; c >= 0; c--)
                {
                    if (GetAutoLayout(ColumnDefinitions[c]) == true)
                    {
                        this.ColumnDefinitions.RemoveAt(c);
                    }
                }

                for (int c = 0; c < columns; c++)
                {
                    if (!(ColumnDefinitions.Count >= c + 1 && GetColumn(ColumnDefinitions[c]) == c))
                    {
                        var cd = new ColumnDefinition();
                        SetAutoLayout(cd, true);
                        ColumnDefinitions.Insert(c, cd);
                    }
                }
            }
        }
    }
}
