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

        /// <inheritdoc/>
        protected override Size MeasureOverride(Size availableSize)
        {
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
            double offset = 0;
            int numColumns = (int)Math.Floor(finalSize.Width / _columnWidth);
            if (HorizontalAlignment == HorizontalAlignment.Right)
            {
                offset = finalSize.Width - (numColumns * _columnWidth);
            }
            else if (HorizontalAlignment == HorizontalAlignment.Center)
            {
                offset = (finalSize.Width - (numColumns * _columnWidth)) / 2;
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

                Rect bounds = new Rect(offset + (_columnWidth * columnIndex), columnHeights[columnIndex], elementWidth, elementHeight);
                child.Arrange(bounds);

                columnHeights[columnIndex] += elementSize.Height;
            }

            return base.ArrangeOverride(finalSize);
        }

        private static void OnDesiredColumnWidthChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is StaggeredPanel panel)
            {
                panel.InvalidateMeasure();
            }
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