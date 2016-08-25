using System;
using System.Collections;
using Windows.Graphics.Display;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    /// <summary>
    /// Represents the control that redistributes space between columns or rows of a Grid control.
    /// </summary>
    public partial class GridSplitter
    {
        private static bool IsStarColumn(DependencyObject definition)
        {
            return ((GridLength)definition.GetValue(ColumnDefinition.WidthProperty)).IsStar;
        }

        private static bool IsStarRow(DependencyObject definition)
        {
            return ((GridLength)definition.GetValue(RowDefinition.HeightProperty)).IsStar;
        }

        private void SetColumnWidth(ColumnDefinition columnDefinition, double horizontalChange, GridUnitType unitType)
        {
            var newWidth = columnDefinition.ActualWidth + horizontalChange;
            if (newWidth > ActualWidth)
            {
                columnDefinition.Width = new GridLength(newWidth, unitType);
            }
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
                case GridResizeBehavior.PreviousAndCurrent | GridResizeBehavior.PreviousAndNext:
                    return currentIndex - 1;
                default:
                    return -1;
            }
        }

        private int GetSiblingColumn()
        {
            var currentIndex = Grid.GetColumn(this);
            switch (_resizeBehavior)
            {
                case GridResizeBehavior.CurrentAndNext | GridResizeBehavior.PreviousAndNext:
                    return currentIndex + 1;
                case GridResizeBehavior.PreviousAndCurrent:
                    return currentIndex;
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
                        case HorizontalAlignment.Right:
                            resizeBehavior = GridResizeBehavior.CurrentAndNext;
                            break;
                        default:
                            resizeBehavior = GridResizeBehavior.PreviousAndNext;
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
                        case VerticalAlignment.Bottom:
                            resizeBehavior = GridResizeBehavior.CurrentAndNext;
                            break;
                        default:
                            resizeBehavior = GridResizeBehavior.PreviousAndNext;
                            break;
                    }
                }
            }

            return resizeBehavior;
        }
    }
}
