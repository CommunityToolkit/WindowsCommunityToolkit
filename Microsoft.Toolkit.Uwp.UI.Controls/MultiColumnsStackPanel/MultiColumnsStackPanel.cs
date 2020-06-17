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
            // We measure all our children with the minimum width between MaxColumnWidth and availableSize.Width.
            var (columnsCount, columnsWidth) = GetAvailableColumnsInformation(availableSize);
            var childAvailableSize = new Size(columnsWidth, double.PositiveInfinity);
            foreach (var child in Children)
            {
                child.Measure(childAvailableSize);
            }

            // We evaluate how the children are filling the columns to get the size that we will use.
            var columnHeight = GetColumnHeight(columnsCount, availableSize);
            var columnsData = DoLayout(columnsCount, columnHeight);

            var size = GetFinalSize(columnsData, columnsWidth);
            return size;
        }

        /// <inheritdoc/>
        protected override Size ArrangeOverride(Size finalSize)
        {
            // We measure all our children with the minimum width between MaxColumnWidth and availableSize.Width.
            var (columnsCount, columnsWidth) = GetAvailableColumnsInformation(finalSize);

            // We split the items across all the columns
            var columnHeight = GetColumnHeight(columnsCount, finalSize);
            var columnsData = DoLayout(columnsCount, columnHeight);

            var rect = new Rect(0, 0, columnsWidth, 0);
            var height = 0.0;
            var currentColumn = 0;
            for (var childIndex = 0; childIndex < Children.Count; childIndex++)
            {

                var child = Children[childIndex];
                rect.Y = height;
                rect.Height = child.DesiredSize.Height;

                child.Arrange(rect);

                height += child.DesiredSize.Height;
                if (childIndex == columnsData[currentColumn].LastChildIndex)
                {
                    // We've reached the last item for the current column. We move to the next one.
                    height = 0.0;
                    currentColumn++;

                    rect.X += columnsWidth + ColumnsSpacing;
                    rect.Y = 0;
                }
            }

            var size = GetFinalSize(columnsData, columnsWidth);
            return size;
        }

        private double GetColumnHeight(int columnsCount, Size availableSize)
        {
            var totalChildrenHeight = Children.Select(c => c.DesiredSize.Height).Sum();
            var availableColumnsHeight = columnsCount * availableSize.Height;
            if (totalChildrenHeight <= availableColumnsHeight)
            {
                // We have enough space for all our items, we try with the available height
                return Math.Ceiling(availableSize.Height);
            }

            // Not enough place for all items. We try with a balanced split.
            return Math.Ceiling(totalChildrenHeight / columnsCount);
        }

        private struct ColumnData
        {
            public int LastChildIndex { get; set; }

            public double ColumnHeight { get; set; }
        }

        private ColumnData[] DoLayout(int columnsCount, double targetHeight)
        {
            var columnsData = new ColumnData[columnsCount];

            var currentColumnIndex = 0;
            var childIndex = 0;
            for (; childIndex < Children.Count; childIndex++)
            {
                columnsData[currentColumnIndex].LastChildIndex = childIndex;
                columnsData[currentColumnIndex].ColumnHeight += Children[childIndex].DesiredSize.Height;
                if (columnsData[currentColumnIndex].ColumnHeight > targetHeight)
                {
                    // We have pass the end, we move to the next column.
                    columnsData[currentColumnIndex].LastChildIndex = childIndex - 1; // TODO: handle the case of super big items that do not fit.
                    columnsData[currentColumnIndex].ColumnHeight -= Children[childIndex].DesiredSize.Height;
                    currentColumnIndex++;

                    if (currentColumnIndex >= columnsData.Length)
                    {
                        // We have filled our last column. We stop
                        break;
                    }

                    // We fill the data for our next column
                    columnsData[currentColumnIndex].LastChildIndex = childIndex;
                    columnsData[currentColumnIndex].ColumnHeight = Children[childIndex].DesiredSize.Height;
                }
            }

            while (childIndex < Children.Count)
            {
                // We have remaining items but no more space in our targetHeight height columns.
                // We move the first items of each column to the previous one.
                columnsData[0].LastChildIndex++;
                var nextChildHeight = Children[columnsData[0].LastChildIndex].DesiredSize.Height;
                columnsData[0].ColumnHeight += nextChildHeight;

                columnsData[1].ColumnHeight -= nextChildHeight;

                // We get our new target height
                targetHeight = columnsData[0].ColumnHeight; // columnsData.Max(cd => cd.ColumnHeight); we do not pick max to have a better alignment.

                // We adjust the other columns so we first reset our data and restart the loop
                for (var i = 1; i < columnsCount; i++)
                {
                    columnsData[i].ColumnHeight = 0.0;
                    columnsData[i].LastChildIndex = 0;
                }

                currentColumnIndex = 1;
                childIndex = columnsData[0].LastChildIndex + 1;
                for (; childIndex < Children.Count; childIndex++)
                {
                    columnsData[currentColumnIndex].LastChildIndex = childIndex;
                    columnsData[currentColumnIndex].ColumnHeight += Children[childIndex].DesiredSize.Height;
                    if (columnsData[currentColumnIndex].ColumnHeight > targetHeight)
                    {
                        // We have pass the end, we move to the next column.
                        columnsData[currentColumnIndex].LastChildIndex = childIndex - 1; // TODO: handle the case of super big items that do not fit.
                        columnsData[currentColumnIndex].ColumnHeight -= Children[childIndex].DesiredSize.Height;
                        currentColumnIndex++;

                        if (currentColumnIndex >= columnsData.Length)
                        {
                            // We have filled our last column. We stop
                            break;
                        }

                        // We fill the data for our next column
                        columnsData[currentColumnIndex].LastChildIndex = childIndex;
                        columnsData[currentColumnIndex].ColumnHeight = Children[childIndex].DesiredSize.Height;
                    }
                }
            }

            return columnsData;
        }

        private Size GetFinalSize(ColumnData[] columnsData, double columnWidth)
        {
            var requiredColumnsCount = columnsData.TakeWhile(cd => cd.LastChildIndex > 0).Count();
            var requiredColumnsWidth = requiredColumnsCount * columnWidth;
            var requiredSpacingWidth = Math.Max(0, requiredColumnsCount - 1) * ColumnsSpacing;
            var maxHeight = columnsData.Max(cd => cd.ColumnHeight);
            return new Size(
                requiredColumnsWidth + requiredSpacingWidth,
                maxHeight);
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
