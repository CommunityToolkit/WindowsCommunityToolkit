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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    /// <summary>
    /// The UniformGrid control presents information within a Grid with even spacing.
    /// </summary>
    public partial class UniformGrid : Grid
    {
        /// <summary>
        /// Measure the controls before layout.
        /// </summary>
        /// <param name="availableSize">Size available from parent.</param>
        /// <returns>Desired Size</returns>
        protected override Size MeasureOverride(Size availableSize)
        {
            var dim = this.GetDimensions();

            Size childSize = new Size(availableSize.Width / dim.columns, availableSize.Height / dim.rows);

            double maxWidth = 0.0;
            double maxHeight = 0.0;

            foreach (var child in this.Children.Where(item => item.Visibility != Visibility.Collapsed))
            {
                child.Measure(childSize);

                maxWidth = Math.Max(child.DesiredSize.Width, maxWidth);
                maxHeight = Math.Max(child.DesiredSize.Height, maxHeight);
            }

            return new Size(maxWidth * (double)dim.columns, maxHeight * (double)dim.rows);
        }

        /// <summary>
        /// Arrange the controls in the grid.
        /// </summary>
        /// <param name="finalSize">Final size to work with.</param>
        /// <returns>Final size used.</returns>
        protected override Size ArrangeOverride(Size finalSize)
        {
            var dim = this.GetDimensions();

            Rect currentSpot = new Rect(0.0, 0.0, finalSize.Width / (double)dim.columns, finalSize.Height / (double)dim.rows);

            double boundary = finalSize.Width - 1.0;

            currentSpot.X += currentSpot.Width * (double)Math.Min(this.FirstColumn, dim.columns - 1);

            // TODO: Handle FlowDirection?
            foreach (var child in this.Children.Where(item => item.Visibility != Visibility.Collapsed))
            {
                child.Arrange(currentSpot);

                // Advance to next position.
                currentSpot.X += currentSpot.Width;

                // Wrap around
                if (currentSpot.X >= boundary)
                {
                    currentSpot.Y += currentSpot.Height;
                    currentSpot.X = 0.0;
                }
            }

            return finalSize;
        }

        #pragma warning disable SA1008 // Opening parenthesis must be spaced correctly
        private (int rows, int columns) GetDimensions()
        #pragma warning restore SA1008 // Opening parenthesis must be spaced correctly
        {
            int rows = this.Rows;
            int cols = this.Columns;

            if (rows == 0 || cols == 0)
            {
                var children = this.Children.Where(item => item.Visibility != Visibility.Collapsed);

                if (rows == 0)
                {
                    if (cols > 0)
                    {
                        var first = Math.Min(this.FirstColumn, cols - 1);

                        // If we have columns but no rows, calculate rows based on column offset and number of children.
                        rows = (children.Count() + first + (cols - 1)) / cols;
                        return (rows, cols);
                    }

                    // Otherwise, determine square layout if both are zero.
                    rows = (int)Math.Ceiling(Math.Sqrt(children.Count()));
                    return (rows, rows);
                }
                else if (cols == 0)
                {
                    // If we have rows and no columns, then calculate columns needed based on rows
                    // TODO: Do we need to account for FirstColumn here too?
                    cols = (children.Count() + (rows - 1)) / rows;
                }
            }

            return (rows, cols);
        }
    }
}
