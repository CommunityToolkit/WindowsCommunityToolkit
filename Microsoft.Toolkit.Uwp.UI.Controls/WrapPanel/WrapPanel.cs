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
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    /// <summary>
    /// WrapPanel is a panel that position child control vertically or horizontally based on the orientation and when max width/ max height is recieved a new row(in case of horizontal) or column (in case of vertical) is created to fit new controls.
    /// </summary>
    public partial class WrapPanel : Panel
    {
        /// <summary>
        /// The maximum columns property
        /// </summary>
        public static readonly DependencyProperty MaxColumnsProperty =
            DependencyProperty.Register("MaxColumns", typeof(int), typeof(WrapPanel), new PropertyMetadata(0, MaxRowOrColumnChanged));

        /// <summary>
        /// The maximum rows property
        /// </summary>
        public static readonly DependencyProperty MaxRowsProperty =
            DependencyProperty.Register("MaxRows", typeof(int), typeof(WrapPanel), new PropertyMetadata(0, MaxRowOrColumnChanged));

        /// <summary>
        /// Identifies the <see cref="Orientation" /> dependency property.
        /// </summary>
        public static readonly DependencyProperty OrientationProperty =
            DependencyProperty.Register(
                "Orientation",
                typeof(Orientation),
                typeof(WrapPanel),
                new PropertyMetadata(Orientation.Horizontal, OrientationPropertyChanged));

        /// <summary>
        /// The empty rectangle.
        /// </summary>
        private static Rect emptyRect = new Rect(0, 0, 0, 0);

        /// <summary>
        /// Occurs when the current row overflows and the wrappanel moves rendering to the next row.
        /// </summary>
        public event EventHandler<OverflowEventArgs> RowChanged;

        /// <summary>
        /// Occurs when the current column overflows and the wrappanel moves rendering to the next column.
        /// </summary>
        public event EventHandler<OverflowEventArgs> ColumnChanged;

        /// <summary>
        /// Gets the number of columns.
        /// </summary>
        /// <value>
        /// The columns.
        /// </value>
        public int Columns { get; private set; }

        /// <summary>
        /// Gets or sets the maximum columns.
        /// </summary>
        /// <value>
        /// The maximum columns.
        /// </value>
        public int MaxColumns
        {
            get { return (int)GetValue(MaxColumnsProperty); }
            set { SetValue(MaxColumnsProperty, value); }
        }

        /// <summary>
        /// Gets or sets the maximum rows.
        /// </summary>
        /// <value>
        /// The maximum rows.
        /// </value>
        public int MaxRows
        {
            get { return (int)GetValue(MaxRowsProperty); }
            set { SetValue(MaxRowsProperty, value); }
        }

        /// <summary>
        /// Gets or sets the orientation of the WrapPanel, Horizontal or vertical means that child controls will be added horizontally until the width of the panel can't fit more control then a new row
        /// is added to fit new horizontal added child controls, vertical means that child will be added vertically until the height of the panel is recieved then a new column is added
        /// </summary>
        /// <value>
        /// The orientation.
        /// </value>
        public Orientation Orientation
        {
            get { return (Orientation)GetValue(OrientationProperty); }
            set { SetValue(OrientationProperty, value); }
        }

        /// <summary>
        /// Gets the number of rows.
        /// </summary>
        /// <value>
        /// The rows.
        /// </value>
        public int Rows { get; private set; }

        /// <inheritdoc />
        protected override Size ArrangeOverride(Size finalSize)
        {
            var parentMeasure = new UvMeasure(Orientation, finalSize.Width, finalSize.Height);
            var position = UvMeasure.Zero;

            double currentV = 0;
            this.Rows = 0;
            this.Columns = 0;

            foreach (var child in Children)
            {
                var desiredMeasure = new UvMeasure(Orientation, child.DesiredSize.Width, child.DesiredSize.Height);
                if ((desiredMeasure.U + position.U) > parentMeasure.U)
                {
                    // next row!
                    position.U = 0;
                    position.V += currentV;
                    currentV = 0;

                    if (this.Orientation == Orientation.Horizontal)
                    {
                        this.Rows++;

                        // Raise the event when new row is added.
                        this.RowChanged?.Invoke(this, new OverflowEventArgs(this.Rows));
                    }
                    else
                    {
                        this.Columns++;

                        // Raise the event when new row is added.
                        this.ColumnChanged?.Invoke(this, new OverflowEventArgs(this.Columns));
                    }
                }

                // Calculate the placement area of the child.
                Rect ctlArea = (this.Orientation == Orientation.Horizontal) ?
                                    new Rect(position.U, position.V, child.DesiredSize.Width, child.DesiredSize.Height) :
                                    new Rect(position.V, position.U, child.DesiredSize.Width, child.DesiredSize.Height);

                // Check if the child can be rendered.
                if ((this.MaxRows != 0 && this.MaxRows <= this.Rows) ||
                    (this.MaxColumns != 0 && this.MaxColumns <= this.Columns))
                {
                    ctlArea = emptyRect;
                }

                // Place the control.
                child.Arrange(ctlArea);

                // adjust the location for the next items
                position.U += desiredMeasure.U;
                currentV = Math.Max(desiredMeasure.V, currentV);
            }

            return finalSize;
        }

        /// <inheritdoc />
        protected override Size MeasureOverride(Size availableSize)
        {
            var totalMeasure = UvMeasure.Zero;
            var parentMeasure = new UvMeasure(Orientation, availableSize.Width, availableSize.Height);
            var lineMeasure = UvMeasure.Zero;
            foreach (var child in Children)
            {
                child.Measure(availableSize);

                var currentMeasure = new UvMeasure(Orientation, child.DesiredSize.Width, child.DesiredSize.Height);

                if (parentMeasure.U > currentMeasure.U + lineMeasure.U)
                {
                    lineMeasure.U += currentMeasure.U;
                    lineMeasure.V = Math.Max(lineMeasure.V, currentMeasure.V);
                }
                else
                {
                    // new line should be added to get the max U to provide it correctly to ui width ex: ---| or -----|
                    totalMeasure.U = Math.Max(lineMeasure.U, totalMeasure.U);
                    totalMeasure.V += lineMeasure.V;

                    // if the next new row still can handle more controls
                    if (parentMeasure.U > currentMeasure.U)
                    {
                        // set lineMeasure initial values to the currentMeasure to be calculated later on the new loop
                        lineMeasure = currentMeasure;
                    }

                    // the control will take one row alone
                    else
                    {
                        // validate the new control measures
                        totalMeasure.U = Math.Max(currentMeasure.U, totalMeasure.U);
                        totalMeasure.V += currentMeasure.V;

                        // add new empty line
                        lineMeasure = UvMeasure.Zero;
                    }
                }
            }

            // update value with the last line if the the last loop is(parentMeasure.U > currentMeasure.U + lineMeasure.U) the total isn't calculated then calculate it if the last loop is
            // (parentMeasure.U > currentMeasure.U) the currentMeasure isn't added to the total so add it here for the last condition it is zeros so adding it will make no difference this way is faster
            // than an if condition in every loop for checking the last item
            totalMeasure.U = Math.Max(lineMeasure.U, totalMeasure.U);
            totalMeasure.V += lineMeasure.V;

            totalMeasure.U = Math.Ceiling(totalMeasure.U);

            return Orientation == Orientation.Horizontal ? new Size(totalMeasure.U, totalMeasure.V) : new Size(totalMeasure.V, totalMeasure.U);
        }

        /// <summary>
        /// Maximums the row or column changed.
        /// </summary>
        /// <param name="d">The d.</param>
        /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs" /> instance containing the event data.</param>
        private static void MaxRowOrColumnChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Repaint(d);
        }

        /// <summary>
        /// Orientations the property changed.
        /// </summary>
        /// <param name="d">The d.</param>
        /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs" /> instance containing the event data.</param>
        private static void OrientationPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Repaint(d);
        }

        /// <summary>
        /// Repaints the specified d.
        /// </summary>
        /// <param name="d">The d.</param>
        private static void Repaint(DependencyObject d)
        {
            var wrapPanel = d as WrapPanel;
            wrapPanel?.InvalidateMeasure();
            wrapPanel?.InvalidateArrange();
        }
    }
}
