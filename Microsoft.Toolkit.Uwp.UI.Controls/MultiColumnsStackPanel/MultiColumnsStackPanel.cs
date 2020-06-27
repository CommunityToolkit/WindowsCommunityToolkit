// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Linq;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    /// <summary>
    /// <see cref="MultiColumnsStackPanel"/> positions its child elements vertically in one or several columns based on the <see cref="MultiColumnsStackPanel.MaxColumnWidth"/> property.
    /// </summary>
    public class MultiColumnsStackPanel : Panel
    {
        /// <summary>
        /// The DP to store the MaxColumnWidth value.
        /// </summary>
        public static readonly DependencyProperty MaxColumnWidthProperty = DependencyProperty.Register(
            nameof(MaxColumnWidth),
            typeof(double),
            typeof(MultiColumnsStackPanel),
            new PropertyMetadata(0.0, OnLayoutPropertyChanged));

        /// <summary>
        /// The DP to store the ColumnsSpacing value.
        /// </summary>
        public static readonly DependencyProperty ColumnsSpacingProperty = DependencyProperty.Register(
            nameof(ColumnsSpacing),
            typeof(double),
            typeof(MultiColumnsStackPanel),
            new PropertyMetadata(0.0, OnLayoutPropertyChanged));

        /// <summary>
        /// The DP to store the ItemsSpacing value.
        /// </summary>
        public static readonly DependencyProperty ItemsSpacingProperty = DependencyProperty.Register(
            nameof(ItemsSpacing),
            typeof(double),
            typeof(MultiColumnsStackPanel),
            new PropertyMetadata(0.0, OnLayoutPropertyChanged));

        /// <summary>
        /// The DP to store the HorizontalContentAlignment value.
        /// </summary>
        public static readonly DependencyProperty HorizontalContentAlignmentProperty = DependencyProperty.Register(
            nameof(HorizontalContentAlignment),
            typeof(HorizontalAlignment),
            typeof(MultiColumnsStackPanel),
            new PropertyMetadata(HorizontalAlignment.Stretch, OnLayoutPropertyChanged));

        /// <summary>
        /// Gets or sets the maximum width for the columns.
        /// If the value is 0, it will display a single column (like a vertical <see cref="StackPanel"/>).
        /// </summary>
        public double MaxColumnWidth
        {
            get => (double)GetValue(MaxColumnWidthProperty);
            set => SetValue(MaxColumnWidthProperty, value);
        }

        /// <summary>
        /// Gets or sets the spacing between two columns.
        /// </summary>
        public double ColumnsSpacing
        {
            get => (double)GetValue(ColumnsSpacingProperty);
            set => SetValue(ColumnsSpacingProperty, value);
        }

        /// <summary>
        /// Gets or sets the spacing between two items.
        /// </summary>
        public double ItemsSpacing
        {
            get => (double)GetValue(ItemsSpacingProperty);
            set => SetValue(ItemsSpacingProperty, value);
        }

        /// <summary>
        /// Gets or sets the horizontal alignment of the control's content.
        /// </summary>
        public HorizontalAlignment HorizontalContentAlignment
        {
            get => (HorizontalAlignment)GetValue(HorizontalContentAlignmentProperty);
            set => SetValue(HorizontalContentAlignmentProperty, value);
        }

        private static void OnLayoutPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (MultiColumnsStackPanel)d;

            control.InvalidateMeasure();
            control.InvalidateArrange();
        }

        /// <inheritdoc/>
        protected override Size MeasureOverride(Size availableSize)
        {
            // We measure all our children with our column width. We let them have the height they want.
            var (columnsCount, columnsWidth) = GetAvailableColumnsInformation(availableSize);
            var childAvailableSize = new Size(columnsWidth, double.PositiveInfinity);
            foreach (var child in Children)
            {
                child.Measure(childAvailableSize);
            }

            // We evaluate how the children are filling the columns to get the size that we will use.
            var (_, columnHeight) = Partition(columnsCount, availableSize.Height);
            return GetSize(columnsCount, columnsWidth, columnHeight);
        }

        /// <inheritdoc/>
        protected override Size ArrangeOverride(Size finalSize)
        {
            // We measure all our children with the minimum width between MaxColumnWidth and availableSize.Width.
            var (columnsCount, columnsWidth) = GetAvailableColumnsInformation(finalSize);

            // We split the items across all the columns
            var (columnLastIndex, columnHeight) = Partition(columnsCount, finalSize.Height);

            var rect = new Rect(0, 0, columnsWidth, 0);
            var height = 0.0;
            var currentColumnIndex = 0;
            for (var childIndex = 0; childIndex < Children.Count; childIndex++)
            {
                var child = Children[childIndex];
                rect.Y = height;
                rect.Height = child.DesiredSize.Height;

                child.Arrange(rect);

                height += child.DesiredSize.Height + ItemsSpacing;
                if (childIndex == columnLastIndex[currentColumnIndex])
                {
                    // We've reached the last item for the current column. We move to the next one.
                    height = 0.0;
                    currentColumnIndex++;

                    rect.X += columnsWidth + ColumnsSpacing;
                    rect.Y = 0;
                }
            }

            return GetSize(columnsCount, columnsWidth, columnHeight);
        }

        private Size GetSize(int columnsCount, double columnsWidth, double columnHeight)
            => new Size(
                    width: (columnsCount * columnsWidth) + (Math.Max(0, columnsCount - 1) * ColumnsSpacing),
                    height: columnHeight);

        private double GetHeight(int index) => Children[index].DesiredSize.Height;

        /// <summary>
        /// Partition our <see cref="Panel.Children"/> list into columns.
        /// </summary>
        /// <param name="columnsCount">The number of columns.</param>
        /// <param name="availableColumnHeight">The available height for the columns.</param>
        /// <returns>
        /// An array containing the index of the last item of each column or -1 if the column is not used and
        /// the required height for the columns.
        /// </returns>
        private (int[] columnLastIndexes, double columnHeight) Partition(int columnsCount, double availableColumnHeight)
        {
            var columnLastIndexes = new int[columnsCount];

            var totalHeight = Children.Sum(child => child.DesiredSize.Height);
            var expectedColumnHeight = Math.Max(availableColumnHeight, totalHeight / columnsCount);

            // We ensure that we have enough space to place the first item.
            expectedColumnHeight = Math.Max(expectedColumnHeight, GetHeight(0));

            var columnIndex = 0;
            var (hasFoundPartition, adjustedExpectedColumnHeight) = DoPartition(
                columnLastIndexes,
                columnIndex,
                childStartIndex: 0,
                expectedColumnHeight: expectedColumnHeight);

            // We have some overflow items, we move the first element of column 1 to column 0 and restart the logic.
            while (!hasFoundPartition)
            {
                columnLastIndexes[0]++;
                expectedColumnHeight = Children.Take(columnLastIndexes[0] + 1).Sum(child => child.DesiredSize.Height) + (columnLastIndexes[0] * ItemsSpacing);

                columnIndex = 1;
                (hasFoundPartition, adjustedExpectedColumnHeight) = DoPartition(
                    columnLastIndexes,
                    columnIndex,
                    childStartIndex: columnLastIndexes[0] + 1,
                    expectedColumnHeight: expectedColumnHeight);
            }

            return (columnLastIndexes, adjustedExpectedColumnHeight);
        }

        /// <summary>
        /// Partition our <see cref="Panel.Children"/> list in buckets which have all a size lower than <paramref name="expectedColumnHeight"/>.
        /// </summary>
        /// <param name="columnLastIndexes">The array containing the indexes of the last item of each column.</param>
        /// <param name="columnIndex">The index of the first column where to apply the partition logic.</param>
        /// <param name="childStartIndex">The index of the first child to consider.</param>
        /// <param name="expectedColumnHeight">The expected height for our columns.</param>
        /// <returns>True if we've been able to partition all the children in columns.</returns>
        private (bool partitionSuceeded, double expectedColumnHeight) DoPartition(
            int[] columnLastIndexes,
            int columnIndex,
            int childStartIndex,
            double expectedColumnHeight)
        {
            var currentColumnHeight = 0.0;
            var partitionSucceeded = true;

            for (var i = childStartIndex; i < Children.Count; i++)
            {
                var currentChildHeight = GetHeight(i);
                var columnHeightAfterAdd = currentColumnHeight + currentChildHeight;

                if (columnHeightAfterAdd > expectedColumnHeight)
                {
                    if (columnIndex == 0)
                    {
                        // Now that we have the items for our first column, we adjust expectedColumnHeight
                        // to be the height of this first column in order to have a more natural layout.
                        expectedColumnHeight = Children.Take(columnLastIndexes[0] + 1).Sum(child => child.DesiredSize.Height) + (columnLastIndexes[0] * ItemsSpacing);
                    }

                    columnIndex++;
                    if (columnIndex < columnLastIndexes.Length)
                    {
                        columnLastIndexes[columnIndex] = i;
                        currentColumnHeight = currentChildHeight;
                    }
                    else
                    {
                        partitionSucceeded = false;
                        break;
                    }
                }
                else
                {
                    columnLastIndexes[columnIndex] = i;
                    currentColumnHeight = columnHeightAfterAdd + ItemsSpacing;
                }
            }

            // We set the indexes of the empty columns to -1
            columnIndex++;
            for (; columnIndex < columnLastIndexes.Length; columnIndex++)
            {
                columnLastIndexes[columnIndex] = -1;
            }

            return (partitionSucceeded, expectedColumnHeight);
        }

        private (int columnsCount, double columnsWidth) GetAvailableColumnsInformation(Size availableSize)
        {
            var columnsWidth = MaxColumnWidth > 0 ? Math.Min(MaxColumnWidth, availableSize.Width) : availableSize.Width;
            var columnsCount = 1;
            if (columnsWidth < availableSize.Width)
            {
                var additionalColumns = (int)((availableSize.Width - columnsWidth) / (columnsWidth + ColumnsSpacing));
                columnsCount += additionalColumns;
            }

            return (columnsCount, columnsWidth);
        }
    }
}
