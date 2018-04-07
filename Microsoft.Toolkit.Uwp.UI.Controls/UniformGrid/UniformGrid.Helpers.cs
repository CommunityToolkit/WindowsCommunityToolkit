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
        internal static IEnumerable<(int row, int column)> GetFreeSpot(bool[,] array, int firstcolumn, bool reverse)
        #pragma warning restore SA1009 // Closing parenthesis must be followed by a space.
        {
            if (!reverse)
            {
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
            else
            {
                for (int r = 0; r < array.GetLength(0); r++)
                {
                    int start = (r == 0 && firstcolumn > 0) ? firstcolumn : array.GetLength(1) - 1;
                    for (int c = start; c >= 0; c--)
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
        internal (int rows, int columns) GetDimensions(ref IEnumerable<FrameworkElement> visible)
        #pragma warning restore SA1008 // Opening parenthesis must be spaced correctly
        {
            // Make copy of our properties as we don't want to modify.
            int rows = Rows;
            int cols = Columns;

            // If a dimension isn't specified, we need to figure out the other one (or both).
            if (rows == 0 || cols == 0)
            {
                // Calculate the size & area of all objects in the grid to know how much space we need.
                // TODO: Need to trim size of objects that go out of bounds?
                // TODO: Do we need to worry if there aren't enough small items to fill in the gaps?
                var count = visible.Sum(item => GetRowSpan(item) * GetColumnSpan(item));

                if (rows == 0)
                {
                    if (cols > 0)
                    {
                        // TODO: Handle RightToLeft?
                        var first = Math.Min(FirstColumn, cols - 1); // Bound check

                        // If we have columns but no rows, calculate rows based on column offset and number of children.
                        rows = (count + first + (cols - 1)) / cols;
                        return (rows, cols);
                    }

                    // Otherwise, determine square layout if both are zero.
                    rows = (int)Math.Ceiling(Math.Sqrt(count));
                    return (rows, rows);
                }
                else if (cols == 0)
                {
                    // If we have rows and no columns, then calculate columns needed based on rows
                    cols = (count + (rows - 1)) / rows;

                    // Now that we know a rough size of our shape, see if the FirstColumn effects that:
                    var first = Math.Min(FirstColumn, cols - 1); // Bound check

                    cols = (count + first + (rows - 1)) / rows;
                }
            }

            return (rows, cols);
        }
    }
}
