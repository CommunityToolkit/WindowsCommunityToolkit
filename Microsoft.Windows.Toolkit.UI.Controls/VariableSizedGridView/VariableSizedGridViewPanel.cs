// *********************************************************
//  Copyright (c) Microsoft. All rights reserved.
//  This code is licensed under the MIT License (MIT).
//  THE CODE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, 
//  INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, 
//  FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. 
//  IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, 
//  DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, 
//  TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH 
//  THE CODE OR THE USE OR OTHER DEALINGS IN THE CODE.
// *********************************************************

using System;
using System.Collections.Generic;
using System.Linq;

using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Microsoft.Windows.Toolkit.UI.Controls.Primitives
{
    /// <summary>
    /// The VariableSizedGridPanel is used to support the <see cref="VariableSizedGridView"/> control.
    /// </summary>
    public class VariableSizedGridViewPanel : Panel
    {
        private List<Rect> _cells;

        internal bool IsReady { get; set; } = false;

        /// <summary>
        /// Gets or sets the dimension by which child elements are stacked.
        /// </summary>
        /// <value>One of the enumeration values that specifies the orientation of child elements.</value>
        public Orientation Orientation
        {
            get { return (Orientation)GetValue(OrientationProperty); }
            set { SetValue(OrientationProperty, value); }
        }

        private static void OrientationChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as VariableSizedGridViewPanel;
            control.InvalidateMeasure();
        }

        /// <summary>
        /// Identifies the <see cref="Orientation"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty OrientationProperty = DependencyProperty.Register("Orientation", typeof(Orientation), typeof(VariableSizedGridViewPanel), new PropertyMetadata(Orientation.Horizontal, OrientationChanged));

        /// <summary>
        /// Gets or sets the maximum number of rows or columns.
        /// </summary>
        /// <value>The maximum rows or columns.</value>
        public int MaximumRowsOrColumns
        {
            get { return (int)GetValue(MaximumRowsOrColumnsProperty); }
            set { SetValue(MaximumRowsOrColumnsProperty, value); }
        }

        private static void MaximumRowsOrColumnsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as VariableSizedGridViewPanel;
            control.InvalidateMeasure();
        }

        /// <summary>
        /// Identifies the <see cref="MaximumRowsOrColumns"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty MaximumRowsOrColumnsProperty = DependencyProperty.Register("MaximumRowsOrColumns", typeof(int), typeof(VariableSizedGridViewPanel), new PropertyMetadata(0, MaximumRowsOrColumnsChanged));


        /// <summary>
        /// Gets or sets the height-to-width aspect ratio for each tile.
        /// </summary>
        /// <value>The aspect ratio.</value>
        public double AspectRatio
        {
            get { return (double)GetValue(AspectRatioProperty); }
            set { SetValue(AspectRatioProperty, value); }
        }

        private static void AspectRatioChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as VariableSizedGridViewPanel;
            control.InvalidateMeasure();
        }

        /// <summary>
        /// Identifies the <see cref="AspectRatio"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty AspectRatioProperty = DependencyProperty.Register("AspectRatio", typeof(double), typeof(VariableSizedGridViewPanel), new PropertyMetadata(1.0, AspectRatioChanged));


        /// <summary>
        /// Provides the behavior for the Measure pass of the layout cycle. Classes can override this method to define their own Measure pass behavior.
        /// </summary>
        /// <param name="availableSize">The available size that this object can give to child objects. Infinity can be specified as a value to indicate that the object will size to whatever content is available.</param>
        /// <returns>The size that this object determines it needs during layout, based on its calculations of the allocated sizes for child objects or based on other considerations such as a fixed container size.</returns>
        protected override Size MeasureOverride(Size availableSize)
        {
            if (this.IsReady && base.Children.Count > 0)
            {
                _cells = new List<Rect>();

                double sizeWidth = availableSize.Width;
                double sizeHeight = availableSize.Height;

                if (double.IsInfinity(sizeWidth))
                {
                    sizeWidth = Window.Current.Bounds.Width;
                }

                if (double.IsInfinity(sizeHeight))
                {
                    sizeHeight = Window.Current.Bounds.Height;
                }

                double cw = sizeWidth / this.MaximumRowsOrColumns;
                double ch = cw * this.AspectRatio;
                if (Orientation == Orientation.Vertical)
                {
                    ch = sizeHeight / this.MaximumRowsOrColumns;
                    cw = ch / this.AspectRatio;
                }

                cw = Math.Round(cw);
                ch = Math.Round(ch);

                int n = 0;
                foreach (FrameworkElement item in base.Children)
                {
                    int colSpan = 1;
                    int rowSpan = 1;
                    PrepareItem(n, item, ref colSpan, ref rowSpan);
                    double w = cw * colSpan;
                    double h = ch * rowSpan;
                    var rect = GetNextPosition(_cells, new Size(cw, ch), new Size(w, h));
                    item.Measure(new Size(w, h));
                    n++;
                }

                return MeasureSize(_cells);
            }

            return base.MeasureOverride(availableSize);
        }

        /// <summary>
        /// Provides the behavior for the Arrange pass of layout. Classes can override this method to define their own Arrange pass behavior.
        /// </summary>
        /// <param name="finalSize">The final area within the parent that this object should use to arrange itself and its children.</param>
        /// <returns>The actual size that is used after the element is arranged in layout.</returns>
        protected override Size ArrangeOverride(Size finalSize)
        {
            if (this.IsReady && base.Children.Count > 0)
            {
                int n = 0;
                foreach (var item in base.Children)
                {
                    var rect = _cells[n++];
                    item.Arrange(rect);
                }

                return MeasureSize(_cells);
            }

            return base.ArrangeOverride(finalSize);
        }

        private Rect GetNextPosition(List<Rect> cells, Size cellSize, Size itemSize)
        {
            if (Orientation == Orientation.Horizontal)
            {
                for (int y = 0; ; y++)
                {
                    for (int x = 0; x < this.MaximumRowsOrColumns; x++)
                    {
                        var rect = new Rect(new Point(x * cellSize.Width, y * cellSize.Height), itemSize);
                        if (RectFitInCells(rect, cells))
                        {
                            cells.Add(rect);
                            return rect;
                        }
                    }
                }
            }
            else
            {
                for (int x = 0; ; x++)
                {
                    for (int y = 0; y < this.MaximumRowsOrColumns; y++)
                    {
                        var rect = new Rect(new Point(x * cellSize.Width, y * cellSize.Height), itemSize);
                        if (RectFitInCells(rect, cells))
                        {
                            cells.Add(rect);
                            return rect;
                        }
                    }
                }
            }
        }

        private bool RectFitInCells(Rect rect, List<Rect> cells)
        {
            return !cells.Any(r => !(r.Left >= rect.Right || r.Right <= rect.Left || r.Top >= rect.Bottom || r.Bottom <= rect.Top));
        }

        /// <summary>
        /// Assigned the row and column span for a given item.
        /// </summary>
        /// <param name="index">The index of the element.</param>
        /// <param name="element">The element.</param>
        /// <param name="colSpan">The col span to use for the item.</param>
        /// <param name="rowSpan">The row span to use for the item.</param>
        protected virtual void PrepareItem(int index, UIElement element, ref int colSpan, ref int rowSpan)
        {
            colSpan = index % 3 == 0 ? 2 : 1;
            rowSpan = index % 3 == 0 ? 2 : 1;
        }

        private Size MeasureSize(List<Rect> cells)
        {
            double mx = cells.Max(r => r.Right);
            double my = cells.Max(r => r.Bottom);
            return new Size(mx, my);
        }
    }
}
