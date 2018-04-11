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
        // Internal list we use to keep track of items that we don't have space to layout.
        private List<UIElement> _overflow = new List<UIElement>();

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

            // Now that we know size, setup automatic rows/columns
            // to utilize Grid for UniformGrid behavior.
            // We also interleave any specified rows/columns with fixed sizes.
            SetupRowDefinitions(dim.rows);
            SetupColumnDefinitions(dim.columns);

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
                if ((row == 0 && col == 0 && GetAutoLayout(child) == null) ||
                    GetAutoLayout(child) == true)
                {
                    SetAutoLayout(child, true);
                }
                else
                {
                    SetAutoLayout(child, false);
                    spots.Fill(true, row, col, colspan, rowspan); // row, col, width, height
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
            var freespots = GetFreeSpot(spots, FirstColumn, Orientation == Orientation.Vertical).GetEnumerator();
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
                        // We've run out of spots as the developer has
                        // most likely given us a fixed size and too many elements
                        // Therefore, tell this element it has no size and move on.
                        child.Measure(Size.Empty);

                        _overflow.Add(child);

                        continue;
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

        /// <inheritdoc/>
        protected override Size ArrangeOverride(Size finalSize)
        {
            // Have grid to the bulk of our heavy lifting.
            var size = base.ArrangeOverride(finalSize);

            // Make sure all overflown elements have no size.
            foreach (var child in _overflow)
            {
                child.Arrange(new Rect(0, 0, 0, 0));
            }

            _overflow = new List<UIElement>(); // Reset for next time.

            return size;
        }
    }
}
