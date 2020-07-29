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
        /// The DP to store the HorizontalSpacing value.
        /// </summary>
        public static readonly DependencyProperty HorizontalSpacingProperty = DependencyProperty.Register(
            nameof(HorizontalSpacing),
            typeof(double),
            typeof(MultiColumnsStackPanel),
            new PropertyMetadata(0.0, OnLayoutPropertyChanged));

        /// <summary>
        /// The DP to store the VerticalSpacing value.
        /// </summary>
        public static readonly DependencyProperty VerticalSpacingProperty = DependencyProperty.Register(
            nameof(VerticalSpacing),
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
        /// The DP to store the Padding value.
        /// </summary>
        public static readonly DependencyProperty PaddingProperty = DependencyProperty.Register(
            nameof(Padding),
            typeof(Thickness),
            typeof(MultiColumnsStackPanel),
            new PropertyMetadata(new Thickness(0), OnLayoutPropertyChanged));

        /// <summary>
        /// Gets or sets the padding inside the control.
        /// </summary>
        public Thickness Padding
        {
            get => (Thickness)GetValue(PaddingProperty);
            set => SetValue(PaddingProperty, value);
        }

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
        public double HorizontalSpacing
        {
            get => (double)GetValue(HorizontalSpacingProperty);
            set => SetValue(HorizontalSpacingProperty, value);
        }

        /// <summary>
        /// Gets or sets the spacing between two items.
        /// </summary>
        public double VerticalSpacing
        {
            get => (double)GetValue(VerticalSpacingProperty);
            set => SetValue(VerticalSpacingProperty, value);
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

            var rect = new Rect(Padding.Left, Padding.Top, columnsWidth, 0);
            var currentColumnIndex = 0;
            for (var childIndex = 0; childIndex < Children.Count; childIndex++)
            {
                var child = Children[childIndex];
                rect.Height = child.DesiredSize.Height;

                ArrangeChild(child, rect);

                if (currentColumnIndex < columnLastIndex.Length && childIndex == columnLastIndex[currentColumnIndex])
                {
                    // We've reached the last item for the current column. We move to the next one.
                    currentColumnIndex++;

                    rect.X += columnsWidth + HorizontalSpacing;
                    rect.Y = Padding.Top;
                }
                else
                {
                    rect.Y += child.DesiredSize.Height + VerticalSpacing;
                }
            }

            return GetSize(columnsCount, columnsWidth, columnHeight);
        }

        private void ArrangeChild(UIElement element, Rect position)
        {
            var requestedAlignement = HorizontalContentAlignment;
            if (element is Control control)
            {
                // We use the control defined value
                requestedAlignement = control.HorizontalAlignment;
            }

            if (element.DesiredSize.Width >= position.Width)
            {
                requestedAlignement = HorizontalAlignment.Stretch;
            }

            switch (requestedAlignement)
            {
                case HorizontalAlignment.Left:
                    position.Width = element.DesiredSize.Width;
                    break;
                case HorizontalAlignment.Center:
                    position.X = (position.Width - element.DesiredSize.Width) / 2.0;
                    position.Width = element.DesiredSize.Width;
                    break;
                case HorizontalAlignment.Right:
                    position.X = position.Width - element.DesiredSize.Width;
                    position.Width = element.DesiredSize.Width;
                    break;
                case HorizontalAlignment.Stretch:
                default:
                    // no adjustment needed, we use the received value.
                    break;
            }

            element.Arrange(position);
        }

        private Size GetSize(int columnsCount, double columnsWidth, double columnHeight)
        {
            // We use this trick to fix rounding errors when scaling is greater than 100%
            // The value we return from MeasureOverride() if converted using floor((value * scalefactor) + .5)/scalefactor  before being provided to ArrangeOverride() and in some
            // cases, we are receiving a value lower than what we're expecting. For example, when we return a desired width of 707 px, we receive 706,8571 in arrange and we're dropping
            // one column. Forcing even numbers is an easy way to limit the issue.
            // See: https://github.com/microsoft/microsoft-ui-xaml/issues/1441
            var requiredColumnWidth = Math.Ceiling((columnsCount * columnsWidth) + (Math.Max(0, columnsCount - 1) * HorizontalSpacing) + Padding.Left + Padding.Right);
            var evenColumnWidth = requiredColumnWidth % 2 == 0 ? requiredColumnWidth : (requiredColumnWidth + 1);

            return new Size(
                    width: evenColumnWidth,
                    height: columnHeight);
        }

        private double GetChildrenHeight(int index) => Children[index].DesiredSize.Height;

        /// <summary>
        /// Partition our <see cref="Panel.Children"/> list into columns.
        /// </summary>
        /// <param name="columnsCount">The number of columns.</param>
        /// <param name="availableColumnHeight">The available height for the columns.</param>
        /// <returns>
        /// - columnLastIndexes: An array containing the index of the last item of each column or -1 if the column is not used and
        /// the required height for the columns.
        /// - columnHeight: the height required to draw our columns.
        /// </returns>
        private (int[] columnLastIndexes, double columnHeight) Partition(int columnsCount, double availableColumnHeight)
        {
            availableColumnHeight = availableColumnHeight - Padding.Top - Padding.Bottom;

            var columnLastIndexes = new int[columnsCount];

            var totalHeight = Children.Sum(child => child.DesiredSize.Height) + (Math.Max(Children.Count - 1, 0) * VerticalSpacing);
            var expectedColumnHeight = totalHeight / columnsCount;
            if (!double.IsInfinity(availableColumnHeight))
            {
                expectedColumnHeight = Math.Max(availableColumnHeight, expectedColumnHeight);
            }

            // If we are wrapped in a scroll viewer, we get the size of the scroll viewer to fill as much as possible the columns
            if (Parent is ScrollViewer scrollviewer)
            {
                expectedColumnHeight = Math.Max(expectedColumnHeight, scrollviewer.ViewportHeight);
            }

            // We ensure that we have enough space to place the first item.
            if (Children.Count > 0)
            {
                expectedColumnHeight = Math.Max(expectedColumnHeight, GetChildrenHeight(0));
            }

            var columnIndex = 0;
            var (hasFoundPartition, adjustedExpectedColumnHeight) = DoPartition(
                columnLastIndexes,
                columnIndex,
                childStartIndex: 0,
                expectedColumnHeight: expectedColumnHeight);

            // We have some overflow items, we move the first element of column 1 to column 0 and restart the logic.
            if (columnLastIndexes.Length > 1)
            {
                while (!hasFoundPartition)
                {
                    columnLastIndexes[0]++;
                    expectedColumnHeight = GetColumnHeight(columnLastIndexes[0]);

                    columnIndex = 1;
                    (hasFoundPartition, adjustedExpectedColumnHeight) = DoPartition(
                        columnLastIndexes,
                        columnIndex,
                        childStartIndex: columnLastIndexes[0] + 1,
                        expectedColumnHeight: expectedColumnHeight);
                }
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
        /// <returns>
        /// - partitionSuceeded: true if we've been able to partition all the children in columns.
        /// - expectedColumnHeight: the adjusted height of the first column (fitting exactly all the first column items).
        /// </returns>
        private (bool partitionSuceeded, double expectedColumnHeight) DoPartition(
            int[] columnLastIndexes,
            int columnIndex,
            int childStartIndex,
            double expectedColumnHeight)
        {
            var currentColumnHeight = Padding.Top + Padding.Bottom;
            var partitionSucceeded = true;

            for (var i = childStartIndex; i < Children.Count; i++)
            {
                var currentChildHeight = GetChildrenHeight(i);
                var columnHeightAfterAdd = currentColumnHeight + currentChildHeight;

                if (columnHeightAfterAdd > expectedColumnHeight)
                {
                    if (columnIndex == 0)
                    {
                        // Now that we have the items for our first column, we adjust expectedColumnHeight
                        // to be the height of this first column in order to have a more natural layout.
                        expectedColumnHeight = currentColumnHeight - VerticalSpacing;
                    }

                    columnIndex++;
                    if (columnIndex < columnLastIndexes.Length)
                    {
                        columnLastIndexes[columnIndex] = i;
                        currentColumnHeight = Padding.Top + Padding.Bottom + currentChildHeight + VerticalSpacing;
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
                    currentColumnHeight = columnHeightAfterAdd + VerticalSpacing;
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
            var availableWidth = Math.Max(availableSize.Width - Padding.Left - Padding.Right, 0);
            var columnsWidth = MaxColumnWidth > 0 ? Math.Min(MaxColumnWidth, availableWidth) : availableWidth;
            var columnsCount = 1;
            if (columnsWidth < availableWidth)
            {
                var additionalColumns = (int)((availableWidth - columnsWidth) / (columnsWidth + HorizontalSpacing));
                columnsCount += additionalColumns;
            }

            return (columnsCount, columnsWidth);
        }

        private double GetColumnHeight(int columnLastIndex) => Children.Take(columnLastIndex + 1).Sum(child => child.DesiredSize.Height) + (columnLastIndex * VerticalSpacing) + Padding.Top + Padding.Bottom;

    }
}
