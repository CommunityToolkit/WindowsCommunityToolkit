// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Windows.System;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    /// <summary>
    /// Represents the control that redistributes space between columns or rows of a Grid control.
    /// </summary>
    public partial class GridSplitter
    {
        private void GridSplitter_Loaded(object sender, RoutedEventArgs e)
        {
            _resizeDirection = GetResizeDirection();
            Orientation = _resizeDirection == GridResizeDirection.Rows ?
                Orientation.Horizontal : Orientation.Vertical;
            _resizeBehavior = GetResizeBehavior();

            // Adding Grip to Grid Splitter
            if (Content == null)
            {
                Content =
                    _resizeDirection == GridResizeDirection.Columns ? GripperBarVertical : GripperBarHorizontal;
            }

            GripperCursor = _resizeDirection == GridResizeDirection.Columns ? CoreCursorType.SizeWestEast : CoreCursorType.SizeNorthSouth;
        }

        /// <inheritdoc />
        protected override void OnManipulationStarted(ManipulationStartedRoutedEventArgs e)
        {
            _resizeDirection = GetResizeDirection();
            Orientation = _resizeDirection == GridResizeDirection.Rows ?
                Orientation.Horizontal : Orientation.Vertical;
            _resizeBehavior = GetResizeBehavior();

            base.OnManipulationStarted(e);
        }

        /// <inheritdoc/>
        protected override bool VerticalMove(double verticalChange)
        {
            if (CurrentRow == null || SiblingRow == null)
            {
                return true;
            }

            // if current row has fixed height then resize it
            if (!IsStarRow(CurrentRow))
            {
                // No need to check for the row Min height because it is automatically respected
                if (!SetRowHeight(CurrentRow, verticalChange, GridUnitType.Pixel))
                {
                    return true;
                }
            }

            // if sibling row has fixed width then resize it
            else if (!IsStarRow(SiblingRow))
            {
                // Would adding to this column make the current column violate the MinWidth?
                if (IsValidRowHeight(CurrentRow, verticalChange) == false)
                {
                    return false;
                }

                if (!SetRowHeight(SiblingRow, verticalChange * -1, GridUnitType.Pixel))
                {
                    return true;
                }
            }

            // if both row haven't fixed height (auto *)
            else
            {
                // change current row height to the new height with respecting the auto
                // change sibling row height to the new height relative to current row
                // respect the other star row height by setting it's height to it's actual height with stars

                // We need to validate current and sibling height to not cause any unexpected behavior
                if (!IsValidRowHeight(CurrentRow, verticalChange) ||
                    !IsValidRowHeight(SiblingRow, verticalChange * -1))
                {
                    return true;
                }

                foreach (var rowDefinition in Resizable.RowDefinitions)
                {
                    if (rowDefinition == CurrentRow)
                    {
                        SetRowHeight(CurrentRow, verticalChange, GridUnitType.Star);
                    }
                    else if (rowDefinition == SiblingRow)
                    {
                        SetRowHeight(SiblingRow, verticalChange * -1, GridUnitType.Star);
                    }
                    else if (IsStarRow(rowDefinition))
                    {
                        rowDefinition.Height = new GridLength(rowDefinition.ActualHeight, GridUnitType.Star);
                    }
                }
            }

            return false;
        }

        /// <inheritdoc/>
        protected override bool HorizontalMove(double horizontalChange)
        {
            if (CurrentColumn == null || SiblingColumn == null)
            {
                return true;
            }

            // if current column has fixed width then resize it
            if (!IsStarColumn(CurrentColumn))
            {
                // No need to check for the Column Min width because it is automatically respected
                if (!SetColumnWidth(CurrentColumn, horizontalChange, GridUnitType.Pixel))
                {
                    return true;
                }
            }

            // if sibling column has fixed width then resize it
            else if (!IsStarColumn(SiblingColumn))
            {
                // Would adding to this column make the current column violate the MinWidth?
                if (IsValidColumnWidth(CurrentColumn, horizontalChange) == false)
                {
                    return false;
                }

                if (!SetColumnWidth(SiblingColumn, horizontalChange * -1, GridUnitType.Pixel))
                {
                    return true;
                }
            }

            // if both column haven't fixed width (auto *)
            else
            {
                // change current column width to the new width with respecting the auto
                // change sibling column width to the new width relative to current column
                // respect the other star column width by setting it's width to it's actual width with stars

                // We need to validate current and sibling width to not cause any unexpected behavior
                if (!IsValidColumnWidth(CurrentColumn, horizontalChange) ||
                    !IsValidColumnWidth(SiblingColumn, horizontalChange * -1))
                {
                    return true;
                }

                foreach (var columnDefinition in Resizable.ColumnDefinitions)
                {
                    if (columnDefinition == CurrentColumn)
                    {
                        SetColumnWidth(CurrentColumn, horizontalChange, GridUnitType.Star);
                    }
                    else if (columnDefinition == SiblingColumn)
                    {
                        SetColumnWidth(SiblingColumn, horizontalChange * -1, GridUnitType.Star);
                    }
                    else if (IsStarColumn(columnDefinition))
                    {
                        columnDefinition.Width = new GridLength(columnDefinition.ActualWidth, GridUnitType.Star);
                    }
                }
            }

            return false;
        }
    }
}