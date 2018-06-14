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
    /// Arranges child elements into a staggered grid pattern where items are added to the column that has used least amount of space.
    /// </summary>
    public class StaggeredPanel : Panel
    {
        private double _columnWidth;

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
            typeof(StaggeredPanel),
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
            typeof(StaggeredPanel),
            new PropertyMetadata(default(Thickness), OnPaddingChanged));

        /// <inheritdoc/>
        protected override Size MeasureOverride(Size availableSize)
        {
            availableSize.Width = availableSize.Width - Padding.Left - Padding.Right;
            availableSize.Height = availableSize.Height - Padding.Top - Padding.Bottom;

            _columnWidth = Math.Min(DesiredColumnWidth, availableSize.Width);
            int numColumns = (int)Math.Floor(availableSize.Width / _columnWidth);
            if (HorizontalAlignment == HorizontalAlignment.Stretch)
            {
                _columnWidth = availableSize.Width / numColumns;
            }

            var columnHeights = new double[numColumns];

            for (int i = 0; i < Children.Count; i++)
            {
                var columnIndex = GetColumnIndex(columnHeights);

                var child = Children[i];
                child.Measure(new Size(_columnWidth, availableSize.Height));
                var elementSize = child.DesiredSize;
                columnHeights[columnIndex] += elementSize.Height;
            }

            double desiredHeight = columnHeights.Max();

            return new Size(availableSize.Width, desiredHeight);
        }

        /// <inheritdoc/>
        protected override Size ArrangeOverride(Size finalSize)
        {
            double horizontalOffset = Padding.Left;
            double verticalOffset = Padding.Top;
            int numColumns = (int)Math.Floor(finalSize.Width / _columnWidth);
            if (HorizontalAlignment == HorizontalAlignment.Right)
            {
                horizontalOffset += finalSize.Width - (numColumns * _columnWidth);
            }
            else if (HorizontalAlignment == HorizontalAlignment.Center)
            {
                horizontalOffset += (finalSize.Width - (numColumns * _columnWidth)) / 2;
            }

            var columnHeights = new double[numColumns];

            for (int i = 0; i < Children.Count; i++)
            {
                var columnIndex = GetColumnIndex(columnHeights);

                var child = Children[i];
                var elementSize = child.DesiredSize;

                double elementWidth = elementSize.Width;
                double elementHeight = elementSize.Height;
                if (elementWidth > _columnWidth)
                {
                    double differencePercentage = _columnWidth / elementWidth;
                    elementHeight = elementHeight * differencePercentage;
                    elementWidth = _columnWidth;
                }

                Rect bounds = new Rect(horizontalOffset + (_columnWidth * columnIndex), columnHeights[columnIndex] + verticalOffset, elementWidth, elementHeight);
                child.Arrange(bounds);

                columnHeights[columnIndex] += elementSize.Height;
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