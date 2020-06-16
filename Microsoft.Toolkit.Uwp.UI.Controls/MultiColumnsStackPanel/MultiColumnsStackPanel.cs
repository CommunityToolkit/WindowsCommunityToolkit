// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
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
            var columnsItemsHeights = new List<double>[columnsCount];
            for (var i = 0; i < columnsCount; i++)
            {
                columnsItemsHeights[i] = new List<double>();
            }

            var overflowItemsHeights = new List<double>();
            var adjustementColumnIndex = 0;
            while (true)
            {
                var height = 0.0;
                var maxHeigth = 0.0;
                var columnsIndex = 0;
                foreach (var child in Children)
                {
                    if (columnsIndex < columnsCount)
                    {
                        columnsItemsHeights[columnsIndex].Add(child.DesiredSize.Height);
                    }
                    else
                    {
                        overflowItemsHeights.Add(child.DesiredSize.Height);
                    }

                    height += child.DesiredSize.Height;
                    if (height > columnHeight)
                    {
                        maxHeigth = Math.Max(maxHeigth, height);
                        height = 0;
                        columnsIndex++;
                    }
                }

                var requiredColumnsCount = columnsItemsHeights.TakeWhile(x => x.Any()).Count();
                var requiredColumnsWidth = requiredColumnsCount * columnsWidth;
                var requiredSpacingWidth = Math.Max(0, requiredColumnsCount - 1) * ColumnsSpacing;

                if (overflowItemsHeights.Count == 0)
                {
                    // No overflow, we've been able to put all our items in our columns.
                    return new Size(
                        requiredColumnsWidth + requiredSpacingWidth,
                        maxHeigth); // <-- Need to handle ItemsSpacing
                }

                // We move the first item of the second column to the first one
                columnsItemsHeights[adjustementColumnIndex].Add(columnsItemsHeights[adjustementColumnIndex + 1].FirstOrDefault());
                columnsItemsHeights[adjustementColumnIndex + 1].RemoveAt(0);
            }
        }

        private double GetColumnHeight(int columnsCount, Size availableSize)
        {
            var totalChildrenHeight = Children.Select(c => c.DesiredSize.Height).Sum();
            //var availableColumnsHeight = columnsCount * availableSize.Height;
            //if (totalChildrenHeight <= availableColumnsHeight)
            //{
            //    // We have enough space for all our items
            //    return Math.Ceiling(availableSize.Height);
            //}

            // Not enough place for all items. We required longer columns
            return Math.Ceiling(totalChildrenHeight / columnsCount);
        }

        /// <inheritdoc/>
        protected override Size ArrangeOverride(Size finalSize)
        {
            // We measure all our children with the minimum width between MaxColumnWidth and availableSize.Width.
            var (columnsCount, columnsWidth) = GetAvailableColumnsInformation(finalSize);

            // We split the items across all the columns
            var columnHeight = GetColumnHeight(columnsCount, finalSize);

            var height = 0.0;
            var maxHeigth = 0.0;
            var requiredColumnsCount = 0;
            var rect = new Rect(0, 0, columnsWidth, 0);
            foreach (var child in Children)
            {
                rect.Height = child.DesiredSize.Height;
                rect.Y = height;

                child.Arrange(rect);

                height += child.DesiredSize.Height;
                if (height >= columnHeight)
                {
                    maxHeigth = Math.Max(maxHeigth, height);
                    height = 0;
                    requiredColumnsCount++;

                    rect.X += columnsWidth + ColumnsSpacing;
                    rect.Y = 0;
                }
            }

            return new Size(
                rect.X + columnsWidth,
                maxHeigth);
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
