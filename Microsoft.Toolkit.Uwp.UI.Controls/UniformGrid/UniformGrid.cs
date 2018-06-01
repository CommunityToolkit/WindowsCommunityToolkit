// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Toolkit.Extensions;
using Windows.Foundation;
using Windows.Foundation.Metadata;
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
        // Guard for 15063 as Grid Spacing only works on 16299+.
        private static bool _hasGridSpacing = ApiInformation.IsPropertyPresent("Windows.UI.Xaml.Controls.Grid", "ColumnSpacing");

        // Internal list we use to keep track of items that we don't have space to layout.
        private List<UIElement> _overflow = new List<UIElement>();

        /// <inheritdoc/>
        protected override Size MeasureOverride(Size availableSize)
        {
            // Get all Visible FrameworkElement Children
            var visible = Children.Where(item => item.Visibility != Visibility.Collapsed && item is FrameworkElement).Select(item => item as FrameworkElement).ToArray();

#pragma warning disable SA1008 // Opening parenthesis must be spaced correctly
            var (rows, columns) = GetDimensions(visible, Rows, Columns, FirstColumn);
#pragma warning restore SA1008 // Opening parenthesis must be spaced correctly

            // Now that we know size, setup automatic rows/columns
            // to utilize Grid for UniformGrid behavior.
            // We also interleave any specified rows/columns with fixed sizes.
            SetupRowDefinitions(rows);
            SetupColumnDefinitions(columns);

            var spotref = new TakenSpotsReferenceHolder(rows, columns);

            // Figure out which children we should automatically layout and where available openings are.
            foreach (var child in visible)
            {
                var row = GetRow(child);
                var col = GetColumn(child);
                var rowspan = GetRowSpan(child);
                var colspan = GetColumnSpan(child);

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
                    spotref.SpotsTaken.Fill(true, row, col, colspan, rowspan); // row, col, width, height
                }
            }

            // Setup available size with our known dimensions now.
            // UniformGrid expands size based on largest singular item.
            double columnSpacingSize = 0;
            double rowSpacingSize = 0;

            // Guard for 15063 as Grid Spacing only works on 16299+.
            if (_hasGridSpacing)
            {
                columnSpacingSize = ColumnSpacing * (columns - 1);
                rowSpacingSize = RowSpacing * (rows - 1);
            }

            Size childSize = new Size(
                (availableSize.Width - columnSpacingSize) / columns,
                (availableSize.Height - rowSpacingSize) / rows);

            double maxWidth = 0.0;
            double maxHeight = 0.0;

            // Set Grid Row/Col for every child with autolayout = true
            // Backwards with FlowDirection
            var freespots = GetFreeSpot(spotref, FirstColumn, Orientation == Orientation.Vertical).GetEnumerator();
            foreach (var child in visible)
            {
                // Set location if we're in charge
                if (GetAutoLayout(child) == true)
                {
                    if (freespots.MoveNext())
                    {
#pragma warning disable SA1009 // Closing parenthesis must be followed by a space.
#pragma warning disable SA1008 // Opening parenthesis must be spaced correctly
                        var (row, column) = freespots.Current;
#pragma warning restore SA1008 // Opening parenthesis must be spaced correctly
#pragma warning restore SA1009 // Closing parenthesis must be followed by a space.

                        SetRow(child, row);
                        SetColumn(child, column);

                        var rowspan = GetRowSpan(child);
                        var colspan = GetColumnSpan(child);

                        if (rowspan > 1 || colspan > 1)
                        {
                            // TODO: Need to tie this into iterator
                            spotref.SpotsTaken.Fill(true, row, column, GetColumnSpan(child), GetRowSpan(child)); // row, col, width, height
                        }
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
                else if (GetRow(child) < 0 || GetRow(child) >= rows ||
                         GetColumn(child) < 0 || GetColumn(child) >= columns)
                {
                    // A child is specifying a location, but that location is outside
                    // of our grid space, so we should hide it instead.
                    child.Measure(Size.Empty);

                    _overflow.Add(child);

                    continue;
                }

                // Get measurement for max child
                child.Measure(childSize);

                maxWidth = Math.Max(child.DesiredSize.Width, maxWidth);
                maxHeight = Math.Max(child.DesiredSize.Height, maxHeight);
            }

            // Return our desired size based on the largest child we found, our dimensions, and spacing.
            var desiredSize = new Size((maxWidth * (double)columns) + columnSpacingSize, (maxHeight * (double)rows) + rowSpacingSize);

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
