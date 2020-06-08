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

            // We get the number of available columns that we can fill.
            var totalChildrenHeight = Children.Select(c => c.DesiredSize.Height).Sum();

            // var availableCombinedColumnsHeight = availableSize.Height * columnsCount; <= This is not needed...
            var requiredColumns = Math.Ceiling(totalChildrenHeight / availableSize.Height);

            var requiredColumnsWidth = columnsCount * columnsWidth;
            var requiredSpacingWidth = Math.Max(0, columnsCount - 1) * ColumnsSpacing;
            return new Size(
                requiredColumnsWidth + requiredSpacingWidth,
                totalChildrenHeight / columnsCount); // <-- this will have to be improved. We need to get the height of the tallest column. Need to handle ItemsSpacing
        }

        /// <inheritdoc/>
        protected override Size ArrangeOverride(Size finalSize)
        {
            // We measure all our children with the minimum width between MaxColumnWidth and availableSize.Width.
            var (columnsCount, columnsWidth) = GetAvailableColumnsInformation(finalSize);

            // We split the items across all the columns
            var totalChildrenHeight = Children.Select(c => c.DesiredSize.Height).Sum();
            var totalAvailableColumnsHeight = finalSize.Height * columnsCount;
            var columnHeight = finalSize.Height;
            if (totalChildrenHeight > totalAvailableColumnsHeight)
            {
                // We cannot fit all our children in the columns we have. We will have to overflow.
                columnHeight = totalChildrenHeight / columnsCount;
            }

            var currentColumn = 0;
            var currentHeight = 0.0;
            var rect = new Rect(0, 0, columnsWidth, 0);
            var maxCurrentHeight = 0.0;
            foreach (var child in Children)
            {
                // <=== Need to handle ItemsSpacing.
                rect.Height = child.DesiredSize.Height;
                rect.Y = currentHeight;
                currentHeight += rect.Height;

                if (currentHeight >= columnHeight)
                {
                    maxCurrentHeight = Math.Max(maxCurrentHeight, currentHeight - rect.Height);

                    currentColumn++;
                    currentHeight = rect.Height;
                    rect.X += columnsWidth + ColumnsSpacing;
                    rect.Y = 0;

                }

                child.Arrange(rect);
            }

            maxCurrentHeight = Math.Max(maxCurrentHeight, currentHeight);

            return new Size(
                rect.X + columnsWidth,
                maxCurrentHeight);
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
