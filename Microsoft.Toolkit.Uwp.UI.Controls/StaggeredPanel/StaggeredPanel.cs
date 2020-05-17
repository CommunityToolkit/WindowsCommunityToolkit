// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Linq;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Windows.Foundation;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    /// <summary>
    /// Arranges child elements into a staggered grid pattern where items are added to the column that has used least amount of space.
    /// </summary>
    public class StaggeredPanel : Panel
    {
        private float _columnWidth;

        /// <summary>
        /// Initializes a new instance of the <see cref="StaggeredPanel"/> class.
        /// </summary>
        public StaggeredPanel()
        {
            RegisterPropertyChangedCallback(Panel.HorizontalAlignmentProperty, OnHorizontalAlignmentChanged);
        }

        /// <summary>
        /// Gets or sets the desired width for each column.
        /// </summary>
        /// <remarks>
        /// The width of columns can exceed the DesiredColumnWidth if the HorizontalAlignment is set to Stretch.
        /// </remarks>
        public float DesiredColumnWidth
        {
            get { return (float)GetValue(DesiredColumnWidthProperty); }
            set { SetValue(DesiredColumnWidthProperty, value); }
        }

        /// <summary>
        /// Identifies the <see cref="DesiredColumnWidth"/> dependency property.
        /// </summary>
        /// <returns>The identifier for the <see cref="DesiredColumnWidth"/> dependency property.</returns>
        public static readonly DependencyProperty DesiredColumnWidthProperty = DependencyProperty.Register(
            nameof(DesiredColumnWidth),
            typeof(float),
            typeof(StaggeredPanel),
            new PropertyMetadata(250f, OnDesiredColumnWidthChanged));

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
            typeof(StaggeredPanel),
            new PropertyMetadata(default(Thickness), OnPaddingChanged));

        /// <summary>
        /// Gets or sets the spacing between columns of items.
        /// </summary>
        public float ColumnSpacing
        {
            get { return (float)GetValue(ColumnSpacingProperty); }
            set { SetValue(ColumnSpacingProperty, value); }
        }

        /// <summary>
        /// Identifies the <see cref="ColumnSpacing"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ColumnSpacingProperty = DependencyProperty.Register(
            nameof(ColumnSpacing),
            typeof(float),
            typeof(StaggeredPanel),
            new PropertyMetadata(0f, OnPaddingChanged));

        /// <summary>
        /// Gets or sets the spacing between rows of items.
        /// </summary>
        public float RowSpacing
        {
            get { return (float)GetValue(RowSpacingProperty); }
            set { SetValue(RowSpacingProperty, value); }
        }

        /// <summary>
        /// Identifies the <see cref="RowSpacing"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty RowSpacingProperty = DependencyProperty.Register(
            nameof(RowSpacing),
            typeof(float),
            typeof(StaggeredPanel),
            new PropertyMetadata(0f, OnPaddingChanged));

        /// <inheritdoc/>
        protected override Size MeasureOverride(Size availableSize)
        {
            float availableWidth = (float)(availableSize.Width - Padding.Left - Padding.Right);
            float availableHeight = (float)(availableSize.Height - Padding.Top - Padding.Bottom);

            _columnWidth = Math.Min(DesiredColumnWidth, availableWidth);
            int numColumns = Math.Max(1, (int)Math.Floor(availableWidth / _columnWidth));

            // adjust for column spacing on all columns expect the first
            float totalWidth = _columnWidth + ((numColumns - 1) * (_columnWidth + ColumnSpacing));
            if (totalWidth > availableWidth)
            {
                numColumns--;
            }
            else if (float.IsInfinity(availableWidth))
            {
                availableWidth = totalWidth;
            }

            if (HorizontalAlignment == HorizontalAlignment.Stretch)
            {
                availableWidth = availableWidth - ((numColumns - 1) * ColumnSpacing);
                _columnWidth = availableWidth / numColumns;
            }

            if (Children.Count == 0)
            {
                return new Size(0, 0);
            }

            var columnHeights = new float[numColumns];
            var itemsPerColumn = new float[numColumns];

            for (int i = 0; i < Children.Count; i++)
            {
                var columnIndex = GetColumnIndex(columnHeights);

                var child = Children[i];
                child.Measure(new Size(_columnWidth, availableHeight));
                var elementSize = child.DesiredSize;
                columnHeights[columnIndex] += elementSize.Height + (itemsPerColumn[columnIndex] > 0 ? RowSpacing : 0);
                itemsPerColumn[columnIndex]++;
            }

            float desiredHeight = columnHeights.Max();

            return new Size(availableWidth, desiredHeight);
        }

        /// <inheritdoc/>
        protected override Size ArrangeOverride(Size finalSize)
        {
            float horizontalOffset = (float)Padding.Left;
            float verticalOffset = (float)Padding.Top;
            int numColumns = Math.Max(1, (int)Math.Floor(finalSize.Width / _columnWidth));

            // adjust for horizontal spacing on all columns expect the first
            float totalWidth = _columnWidth + ((numColumns - 1) * (_columnWidth + ColumnSpacing));
            if (totalWidth > finalSize.Width)
            {
                numColumns--;

                // Need to recalculate the totalWidth for a correct horizontal offset
                totalWidth = _columnWidth + ((numColumns - 1) * (_columnWidth + ColumnSpacing));
            }

            if (HorizontalAlignment == HorizontalAlignment.Right)
            {
                horizontalOffset += finalSize.Width - totalWidth;
            }
            else if (HorizontalAlignment == HorizontalAlignment.Center)
            {
                horizontalOffset += (finalSize.Width - totalWidth) / 2;
            }

            var columnHeights = new float[numColumns];
            var itemsPerColumn = new float[numColumns];

            for (int i = 0; i < Children.Count; i++)
            {
                var columnIndex = GetColumnIndex(columnHeights);

                var child = Children[i];
                var elementSize = child.DesiredSize;

                float elementHeight = elementSize.Height;

                float itemHorizontalOffset = horizontalOffset + (_columnWidth * columnIndex) + (ColumnSpacing * columnIndex);
                float itemVerticalOffset = columnHeights[columnIndex] + verticalOffset + (RowSpacing * itemsPerColumn[columnIndex]);

                Rect bounds = new Rect(itemHorizontalOffset, itemVerticalOffset, _columnWidth, elementHeight);
                child.Arrange(bounds);

                columnHeights[columnIndex] += elementSize.Height;
                itemsPerColumn[columnIndex]++;
            }

            return base.ArrangeOverride(finalSize);
        }

        private static void OnDesiredColumnWidthChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var panel = (StaggeredPanel)d;
            panel.InvalidateMeasure();
        }

        private static void OnPaddingChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var panel = (StaggeredPanel)d;
            panel.InvalidateMeasure();
        }

        private void OnHorizontalAlignmentChanged(DependencyObject sender, DependencyProperty dp)
        {
            InvalidateMeasure();
        }

        private int GetColumnIndex(float[] columnHeights)
        {
            int columnIndex = 0;
            float height = columnHeights[0];
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
