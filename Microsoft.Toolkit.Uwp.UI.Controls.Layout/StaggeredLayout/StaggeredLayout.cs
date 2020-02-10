// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Linq;
using Microsoft.UI.Xaml.Controls;
using Windows.Foundation;
using Windows.UI.Xaml;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    /// <summary>
    /// Arranges child elements into a staggered grid pattern where items are added to the column that has used least amount of space.
    /// </summary>
    public class StaggeredLayout : VirtualizingLayout
    {
        private double _columnWidth;

        /// <summary>
        /// Initializes a new instance of the <see cref="StaggeredLayout"/> class.
        /// </summary>
        public StaggeredLayout()
        {
        }

        /// <summary>
        /// Gets or sets the desired width for each column.
        /// </summary>
        /// <remarks>
        /// The width of columns can exceed the DesiredColumnWidth if the HorizontalAlignment is set to Stretch.
        /// </remarks>
        public double DesiredColumnWidth
        {
            get { return (double)GetValue(DesiredColumnWidthProperty); }
            set { SetValue(DesiredColumnWidthProperty, value); }
        }

        /// <summary>
        /// Identifies the <see cref="DesiredColumnWidth"/> dependency property.
        /// </summary>
        /// <returns>The identifier for the <see cref="DesiredColumnWidth"/> dependency property.</returns>
        public static readonly DependencyProperty DesiredColumnWidthProperty = DependencyProperty.Register(
            nameof(DesiredColumnWidth),
            typeof(double),
            typeof(StaggeredLayout),
            new PropertyMetadata(250d, OnDesiredColumnWidthChanged));

        /// <summary>
        /// Gets or sets the distance between the border and its child object.
        /// </summary>
        /// <returns>
        /// The dimensions of the space between the border and its child as a Thickness value.
        /// Thickness is a structure that stores dimension values using pixel measures.
        /// </returns>
        public Thickness Padding
        {
            get { return (Thickness)GetValue(PaddingProperty); }
            set { SetValue(PaddingProperty, value); }
        }

        /// <summary>
        /// Identifies the Padding dependency property.
        /// </summary>
        /// <returns>The identifier for the <see cref="Padding"/> dependency property.</returns>
        public static readonly DependencyProperty PaddingProperty = DependencyProperty.Register(
            nameof(Padding),
            typeof(Thickness),
            typeof(StaggeredLayout),
            new PropertyMetadata(default(Thickness), OnPaddingChanged));

        /// <summary>
        /// Gets or sets the spacing between columns of items.
        /// </summary>
        public double ColumnSpacing
        {
            get { return (double)GetValue(ColumnSpacingProperty); }
            set { SetValue(ColumnSpacingProperty, value); }
        }

        /// <summary>
        /// Identifies the <see cref="ColumnSpacing"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ColumnSpacingProperty = DependencyProperty.Register(
            nameof(ColumnSpacing),
            typeof(double),
            typeof(StaggeredLayout),
            new PropertyMetadata(0d, OnPaddingChanged));

        /// <summary>
        /// Gets or sets the spacing between rows of items.
        /// </summary>
        public double RowSpacing
        {
            get { return (double)GetValue(RowSpacingProperty); }
            set { SetValue(RowSpacingProperty, value); }
        }

        /// <summary>
        /// Identifies the <see cref="RowSpacing"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty RowSpacingProperty = DependencyProperty.Register(
            nameof(RowSpacing),
            typeof(double),
            typeof(StaggeredLayout),
            new PropertyMetadata(0d, OnPaddingChanged));

        /// <inheritdoc/>
        protected override void InitializeForContextCore(VirtualizingLayoutContext context)
        {
            context.LayoutState = new StaggeredLayoutState();
            base.InitializeForContextCore(context);
        }

        /// <inheritdoc/>
        protected override Size MeasureOverride(VirtualizingLayoutContext context, Size availableSize)
        {
            if (context.ItemCount == 0)
            {
                return new Size(availableSize.Width, 0);
            }
            if ((context.RealizationRect.Width == 0) && (context.RealizationRect.Height == 0))
            {
                return new Size(availableSize.Width, 0.0);
            }

            double availableWidth = availableSize.Width - Padding.Left - Padding.Right;
            double availableHeight = availableSize.Height - Padding.Top - Padding.Bottom;

            _columnWidth = Math.Min(DesiredColumnWidth, availableWidth);
            int numColumns = Math.Max(1, (int)Math.Floor(availableWidth / _columnWidth));

            // adjust for column spacing on all columns expect the first
            double totalWidth = _columnWidth + ((numColumns - 1) * (_columnWidth + ColumnSpacing));
            if (totalWidth > availableWidth)
            {
                numColumns--;
            }
            else if (double.IsInfinity(availableWidth))
            {
                availableWidth = totalWidth;
            }

            //if (HorizontalAlignment == HorizontalAlignment.Stretch)
            //{
            //    availableWidth = availableWidth - ((numColumns - 1) * ColumnSpacing);
            //    _columnWidth = availableWidth / numColumns;
            //}

            var columnHeights = new double[numColumns];
            var itemsPerColumn = new double[numColumns];

            for (int i = 0; i < context.ItemCount; i++)
            {
                var columnIndex = GetColumnIndex(columnHeights);

                var child = context.GetOrCreateElementAt(i);
                child.Measure(new Size(_columnWidth, availableHeight));
                var elementSize = child.DesiredSize;
                columnHeights[columnIndex] += elementSize.Height + (itemsPerColumn[columnIndex] > 0 ? RowSpacing : 0);
                itemsPerColumn[columnIndex]++;
            }

            double desiredHeight = columnHeights.Max();

            return new Size(availableWidth, desiredHeight);
        }

        /// <inheritdoc/>
        protected override Size ArrangeOverride(VirtualizingLayoutContext context, Size finalSize)
        {
            double horizontalOffset = Padding.Left;
            double verticalOffset = Padding.Top;
            int numColumns = Math.Max(1, (int)Math.Floor(finalSize.Width / _columnWidth));

            // adjust for horizontal spacing on all columns expect the first
            double totalWidth = _columnWidth + ((numColumns - 1) * (_columnWidth + ColumnSpacing));
            if (totalWidth > finalSize.Width)
            {
                numColumns--;

                // Need to recalculate the totalWidth for a correct horizontal offset
                totalWidth = _columnWidth + ((numColumns - 1) * (_columnWidth + ColumnSpacing));
            }

            //if (HorizontalAlignment == HorizontalAlignment.Right)
            //{
            //    horizontalOffset += finalSize.Width - totalWidth;
            //}
            //else if (HorizontalAlignment == HorizontalAlignment.Center)
            //{
            //    horizontalOffset += (finalSize.Width - totalWidth) / 2;
            //}

            var columnHeights = new double[numColumns];
            var itemsPerColumn = new double[numColumns];

            for (int i = 0; i < context.ItemCount; i++)
            {
                var columnIndex = GetColumnIndex(columnHeights);

                var child = context.GetOrCreateElementAt(i);
                var elementSize = child.DesiredSize;

                double elementHeight = elementSize.Height;

                double itemHorizontalOffset = horizontalOffset + (_columnWidth * columnIndex) + (ColumnSpacing * columnIndex);
                double itemVerticalOffset = columnHeights[columnIndex] + verticalOffset + (RowSpacing * itemsPerColumn[columnIndex]);

                Rect bounds = new Rect(itemHorizontalOffset, itemVerticalOffset, _columnWidth, elementHeight);
                child.Arrange(bounds);

                columnHeights[columnIndex] += elementSize.Height;
                itemsPerColumn[columnIndex]++;
            }

            return base.Arrange(context, finalSize);
        }

        private static void OnDesiredColumnWidthChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var panel = (StaggeredLayout)d;
            panel.InvalidateMeasure();
        }

        private static void OnPaddingChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var panel = (StaggeredLayout)d;
            panel.InvalidateMeasure();
        }

        private void OnHorizontalAlignmentChanged(DependencyObject sender, DependencyProperty dp)
        {
            InvalidateMeasure();
        }

        private int GetColumnIndex(double[] columnHeights)
        {
            int columnIndex = 0;
            double height = columnHeights[0];
            for (int j = 1; j < columnHeights.Length; j++)
            {
                if (columnHeights[j] < height)
                {
                    columnIndex = j;
                    height = columnHeights[j];
                }
            }

            return columnIndex;
        }
    }
}
