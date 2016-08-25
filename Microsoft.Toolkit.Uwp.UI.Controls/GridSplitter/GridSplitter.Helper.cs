using System;
using System.Collections;
using Windows.Graphics.Display;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    /// <summary>
    /// Represents the control that redistributes space between columns or rows of a Grid control.
    /// </summary>
    public partial class GridSplitter
    {
        private static bool AreClose(double value1, double value2)
        {
            if (Math.Abs(value1 - value2) < Epsilon)
            {
                return true;
            }

            double delta = value1 - value2;
            return (delta < Epsilon) && (delta > -Epsilon);
        }

        private void UpdateDisplayIcon()
        {
            if (_iconDisplay == null)
            {
                return;
            }

            if (_resizeDirection == GridResizeDirection.Columns)
            {
                _iconDisplay.Text = GripperBarVertical;
            }
            else if (_resizeDirection == GridResizeDirection.Rows)
            {
                _iconDisplay.Text = GripperBarHorizontal;
            }
        }

        // Return the targeted Column based on the resize behavior
        private int GetTargetedColumn()
        {
            var currentIndex = Grid.GetColumn(this);
            switch (_resizeBehavior)
            {
                case GridResizeBehavior.CurrentAndNext:
                    return currentIndex;
                case GridResizeBehavior.PreviousAndCurrent:
                    return currentIndex - 1;
                default:
                    return -1;
            }
        }

        // Return the targeted Row based on the resize behavior
        private int GetTargetedRow()
        {
            return Grid.GetRow(this);
        }

        // Converts BasedOnAlignment direction to Rows, Columns, or Both depending on its width/height
        private GridResizeDirection GetEffectiveResizeDirection()
        {
            GridResizeDirection direction = ResizeDirection;

            if (direction == GridResizeDirection.Auto)
            {
                // When HorizontalAlignment is Left, Right or Center, resize Columns
                if (HorizontalAlignment != HorizontalAlignment.Stretch)
                {
                    direction = GridResizeDirection.Columns;
                }
                else if (VerticalAlignment != VerticalAlignment.Stretch)
                {
                    direction = GridResizeDirection.Rows;
                }

                // Fall back to Width vs Height
                else if (ActualWidth <= ActualHeight)
                {
                    direction = GridResizeDirection.Columns;
                }
                else
                {
                    direction = GridResizeDirection.Rows;
                }
            }

            return direction;
        }

        // Convert BasedOnAlignment to Next/Prev/Both depending on alignment and Direction
        private GridResizeBehavior GetEffectiveResizeBehavior()
        {
            GridResizeBehavior resizeBehavior = ResizeBehavior;

            if (resizeBehavior == GridResizeBehavior.BasedOnAlignment)
            {
                if (_resizeDirection == GridResizeDirection.Columns)
                {
                    switch (HorizontalAlignment)
                    {
                        case HorizontalAlignment.Left:
                            resizeBehavior = GridResizeBehavior.PreviousAndCurrent;
                            break;
                        default:
                            resizeBehavior = GridResizeBehavior.CurrentAndNext;
                            break;
                    }
                }
                else
                {
                    switch (VerticalAlignment)
                    {
                        case VerticalAlignment.Top:
                            resizeBehavior = GridResizeBehavior.PreviousAndCurrent;
                            break;
                        default:
                            resizeBehavior = GridResizeBehavior.CurrentAndNext;
                            break;
                    }
                }
            }

            return resizeBehavior;
        }

        // Returns true if the row/column has a Star length
        private bool IsStar(DependencyObject definition)
        {
            return ((GridLength)definition.GetValue(
                _resizeDirection == GridResizeDirection.Columns
                    ? ColumnDefinition.WidthProperty
                    : RowDefinition.HeightProperty)).IsStar;
        }
    }
}
