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
using System.Linq;
using Microsoft.Toolkit.Extensions;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    /// <summary>
    /// The UniformGrid control presents information within a Grid with even spacing.
    /// </summary>
    [Bindable]
    public partial class UniformGrid : Grid
    {
        /// <summary>
        /// Measure the controls before layout.
        /// </summary>
        /// <param name="availableSize">Size available from parent.</param>
        /// <returns>Desired Size</returns>
        protected override Size MeasureOverride(Size availableSize)
        {
            // Get all Visible FrameworkElement Children
            var visible = Children.Where(item => item.Visibility != Visibility.Collapsed && item is FrameworkElement).Select(item => item as FrameworkElement);

            var dim = GetDimensions(ref visible, Rows, Columns, FirstColumn);

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

            // Remove non-autolayout rows we've added and then add them in the right spots.
            if (dim.rows != this.RowDefinitions.Count)
            {
                for (int r = this.RowDefinitions.Count - 1; r >= 0; r--)
                {
                    if (GetAutoLayout(RowDefinitions[r]) == true)
                    {
                        this.RowDefinitions.RemoveAt(r);
                    }
                }

                for (int r = 0; r < dim.rows; r++)
                {
                    if (!(this.RowDefinitions.Count >= r + 1 && GetRow(RowDefinitions[r]) == r))
                    {
                        var rd = new RowDefinition();
                        SetAutoLayout(rd, true);
                        this.RowDefinitions.Insert(r, rd);
                    }
                }
            }

            // Remove non-autolayout columns we've added and then add them in the right spots.
            if (dim.columns != ColumnDefinitions.Count)
            {
                for (int c = ColumnDefinitions.Count - 1; c >= 0; c--)
                {
                    if (GetAutoLayout(ColumnDefinitions[c]) == true)
                    {
                        this.ColumnDefinitions.RemoveAt(c);
                    }
                }

                for (int c = 0; c < dim.columns; c++)
                {
                    if (!(ColumnDefinitions.Count >= c + 1 && GetColumn(ColumnDefinitions[c]) == c))
                    {
                        var cd = new ColumnDefinition();
                        SetAutoLayout(cd, true);
                        ColumnDefinitions.Insert(c, cd);
                    }
                }
            }

            bool[,] spots = new bool[dim.rows, dim.columns];

            // Figure out which children we should automatically layout and where available openings are.
            foreach (var child in visible)
            {
                var row = GetRow(child);
                var col = GetColumn(child);
                var rowspan = GetRowSpan(child);
                var colspan = GetColumnSpan(child);

                // TODO: Document
                // If an element needs to be forced in the 0, 0 position, 
                // they should manually set UniformGrid.AutoLayout to False for that element.
                if (row == 0 && col == 0 && GetAutoLayout(child) == null)
                {
                    SetAutoLayout(child, true);
                }
                else
                {
                    SetAutoLayout(child, false);
                    spots.Fill(true, row, col, rowspan, colspan);
                }
            }

            // Setup available size with our known dimensions now.
            // UniformGrid expands size based on largest singular item.
            var columnSpacingSize = ColumnSpacing * (dim.columns - 1);
            var rowSpacingSize = RowSpacing * (dim.rows - 1);

            Size childSize = new Size(
                (availableSize.Width - columnSpacingSize) / dim.columns,
                (availableSize.Height - rowSpacingSize) / dim.rows);

            double maxWidth = 0.0;
            double maxHeight = 0.0;

            // Set Grid Row/Col for every child with autolayout = true
            // Backwards with FlowDirection
            var freespots = GetFreeSpot(spots, FirstColumn, FlowDirection == FlowDirection.RightToLeft).GetEnumerator();
            foreach (var child in visible)
            {
                // Set location if we're in charge
                if (GetAutoLayout(child) == true)
                {
                    if (freespots.MoveNext())
                    {
                        var loc = freespots.Current;

                        SetRow(child, loc.row);
                        SetColumn(child, loc.column);
                    }
                    else
                    {
                        // TODO: We've run out of spots somehow? Now What?
                    }
                }

                // Get measurement for max child
                child.Measure(childSize);

                maxWidth = Math.Max(child.DesiredSize.Width, maxWidth);
                maxHeight = Math.Max(child.DesiredSize.Height, maxHeight);
            }

            // Return our desired size based on the largest child we found, our dimensions, and spacing.
            var desiredSize = new Size((maxWidth * (double)dim.columns) + columnSpacingSize, (maxHeight * (double)dim.rows) + rowSpacingSize);

            // Required to perform regular grid measurement, but ignore result.
            base.MeasureOverride(desiredSize);

            return desiredSize;
        }
    }
}
