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
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

namespace Microsoft.Toolkit.Uwp.UI.Controls.Markdown.Display
{
    /// <summary>
    /// A custom panel control that arranges elements similar to how an HTML table would.
    /// </summary>
    internal class MarkdownTable : Panel
    {
        private int columnCount;
        private int rowCount;
        private double borderThickness;
        private double[] columnWidths;
        private double[] rowHeights;

        public MarkdownTable(int columnCount, int rowCount, double borderThickness, Brush borderBrush)
        {
            this.columnCount = columnCount;
            this.rowCount = rowCount;
            this.borderThickness = borderThickness;
            for (int col = 0; col < columnCount + 1; col++)
            {
                Children.Add(new Rectangle { Fill = borderBrush });
            }

            for (int row = 0; row < rowCount + 1; row++)
            {
                Children.Add(new Rectangle { Fill = borderBrush });
            }
        }

        // Helper method to enumerate FrameworkElements instead of UIElements.
        private IEnumerable<FrameworkElement> ContentChildren
        {
            get
            {
                for (int i = columnCount + rowCount + 2; i < Children.Count; i++)
                {
                    yield return (FrameworkElement)Children[i];
                }
            }
        }

        // Helper method to get table vertical edges.
        private IEnumerable<Rectangle> VerticalLines
        {
            get
            {
                for (int i = 0; i < columnCount + 1; i++)
                {
                    yield return (Rectangle)Children[i];
                }
            }
        }

        // Helper method to get table horizontal edges.
        private IEnumerable<Rectangle> HorizontalLines
        {
            get
            {
                for (int i = columnCount + 1; i < columnCount + rowCount + 2; i++)
                {
                    yield return (Rectangle)Children[i];
                }
            }
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            // Measure the width of each column, with no horizontal width restrictions.
            var naturalColumnWidths = new double[this.columnCount];
            foreach (var child in ContentChildren)
            {
                var columnIndex = Grid.GetColumn(child);
                child.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
                naturalColumnWidths[columnIndex] = Math.Max(naturalColumnWidths[columnIndex], child.DesiredSize.Width);
            }

            // Now figure out the actual column widths.
            var remainingContentWidth = availableSize.Width - ((this.columnCount + 1) * borderThickness);
            columnWidths = new double[this.columnCount];
            int remainingColumnCount = columnCount;
            while (remainingColumnCount > 0)
            {
                // Calculate the fair width of all columns.
                double fairWidth = Math.Max(0, remainingContentWidth / remainingColumnCount);

                // Are there any columns less than that?  If so, they get what they are asking for.
                bool recalculationNeeded = false;
                for (int i = 0; i < columnCount; i++)
                {
                    if (columnWidths[i] == 0 && naturalColumnWidths[i] < fairWidth)
                    {
                        columnWidths[i] = naturalColumnWidths[i];
                        remainingColumnCount--;
                        remainingContentWidth -= columnWidths[i];
                        recalculationNeeded = true;
                    }
                }

                // If there are no columns less than the fair width, every remaining column gets that width.
                if (recalculationNeeded == false)
                {
                    for (int i = 0; i < columnCount; i++)
                    {
                        if (columnWidths[i] == 0)
                        {
                            columnWidths[i] = fairWidth;
                        }
                    }

                    break;
                }
            }

            // TODO: we can skip this step if none of the column widths changed, and just re-use
            // the row heights we obtained earlier.

            // Now measure row heights.
            rowHeights = new double[this.rowCount];
            foreach (var child in ContentChildren)
            {
                var columnIndex = Grid.GetColumn(child);
                var rowIndex = Grid.GetRow(child);
                child.Measure(new Size(columnWidths[columnIndex], double.PositiveInfinity));
                rowHeights[rowIndex] = Math.Max(rowHeights[rowIndex], child.DesiredSize.Height);
            }

            return new Size(
                columnWidths.Sum() + (borderThickness * (columnCount + 1)),
                rowHeights.Sum() + ((rowCount + 1) * borderThickness));
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            if (columnWidths == null || rowHeights == null)
            {
                throw new InvalidOperationException("Expected Measure to be called first.");
            }

            // Arrange content.
            foreach (var child in ContentChildren)
            {
                var columnIndex = Grid.GetColumn(child);
                var rowIndex = Grid.GetRow(child);

                var rect = new Rect(0, 0, 0, 0);
                rect.X = borderThickness;
                for (int col = 0; col < columnIndex; col++)
                {
                    rect.X += borderThickness + columnWidths[col];
                }

                rect.Y = borderThickness;
                for (int row = 0; row < rowIndex; row++)
                {
                    rect.Y += borderThickness + rowHeights[row];
                }

                rect.Width = columnWidths[columnIndex];
                rect.Height = rowHeights[rowIndex];
                child.Arrange(rect);
            }

            // Arrange vertical border elements.
            {
                int colIndex = 0;
                double x = 0;
                foreach (var borderLine in VerticalLines)
                {
                    borderLine.Arrange(new Rect(x, 0, borderThickness, finalSize.Height));
                    if (colIndex >= columnWidths.Length)
                    {
                        break;
                    }

                    x += borderThickness + columnWidths[colIndex];
                    colIndex++;
                }
            }

            // Arrange horizontal border elements.
            {
                int rowIndex = 0;
                double y = 0;
                foreach (var borderLine in HorizontalLines)
                {
                    borderLine.Arrange(new Rect(0, y, finalSize.Width, borderThickness));
                    if (rowIndex >= rowHeights.Length)
                    {
                        break;
                    }

                    y += borderThickness + rowHeights[rowIndex];
                    rowIndex++;
                }
            }

            return finalSize;
        }
    }
}
