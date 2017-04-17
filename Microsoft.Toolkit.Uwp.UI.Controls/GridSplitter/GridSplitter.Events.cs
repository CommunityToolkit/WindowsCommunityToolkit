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

using Windows.System;
using Windows.UI.Core;
using Windows.UI.Xaml;
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
            _resizeBehavior = GetResizeBehavior();

            GridSplitterGripper gripper;

            // Adding Grip to Grid Splitter
            if (Element == default(UIElement))
            {
                gripper = new GridSplitterGripper(
                    _resizeDirection,
                    GripperForeground);
            }
            else
            {
                var content = Element;
                Element = null;
                gripper = new GridSplitterGripper(content, _resizeDirection);
            }

            Element = gripper;

            gripper.KeyDown += Gripper_KeyDown;

            var hoverWrapper = new GripperHoverWrapper(
                CursorBehavior == SplitterCursorBehavior.ChangeOnSplitterHover
                ? this
                : Element,
                _resizeDirection,
                GripperCursor,
                GripperCustomCursorResource);
            ManipulationStarted += hoverWrapper.SplitterManipulationStarted;
            ManipulationCompleted += hoverWrapper.SplitterManipulationCompleted;

            _hoverWrapper = hoverWrapper;
        }

        private void Gripper_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            var gripper = sender as GridSplitterGripper;

            if (gripper == null)
            {
                return;
            }

            var step = 1;
            var ctrl = Window.Current.CoreWindow.GetKeyState(VirtualKey.Control);
            if (ctrl.HasFlag(CoreVirtualKeyStates.Down))
            {
                step = 5;
            }

            if (gripper.ResizeDirection == GridResizeDirection.Columns)
            {
                if (e.Key == VirtualKey.Left)
                {
                    HorizontalMove(-step);
                }
                else if (e.Key == VirtualKey.Right)
                {
                    HorizontalMove(step);
                }
                else
                {
                    return;
                }

                e.Handled = true;
                return;
            }

            if (gripper.ResizeDirection == GridResizeDirection.Rows)
            {
                if (e.Key == VirtualKey.Up)
                {
                    VerticalMove(-step);
                }
                else if (e.Key == VirtualKey.Down)
                {
                    VerticalMove(step);
                }
                else
                {
                    return;
                }

                e.Handled = true;
            }
        }

        /// <inheritdoc />
        protected override void OnManipulationStarted(ManipulationStartedRoutedEventArgs e)
        {
            // saving the previous state
            PreviousCursor = Window.Current.CoreWindow.PointerCursor;
            _resizeDirection = GetResizeDirection();
            _resizeBehavior = GetResizeBehavior();

            if (_resizeDirection == GridResizeDirection.Columns)
            {
                Window.Current.CoreWindow.PointerCursor = ColumnsSplitterCursor;
            }
            else if (_resizeDirection == GridResizeDirection.Rows)
            {
                Window.Current.CoreWindow.PointerCursor = RowSplitterCursor;
            }

            base.OnManipulationStarted(e);
        }

        /// <inheritdoc />
        protected override void OnManipulationCompleted(ManipulationCompletedRoutedEventArgs e)
        {
            Window.Current.CoreWindow.PointerCursor = PreviousCursor;

            base.OnManipulationCompleted(e);
        }

        /// <inheritdoc />
        protected override void OnManipulationDelta(ManipulationDeltaRoutedEventArgs e)
        {
            var horizontalChange = e.Delta.Translation.X;
            var verticalChange = e.Delta.Translation.Y;

            if (_resizeDirection == GridResizeDirection.Columns)
            {
                if (HorizontalMove(horizontalChange))
                {
                    return;
                }
            }
            else if (_resizeDirection == GridResizeDirection.Rows)
            {
                if (VerticalMove(verticalChange))
                {
                    return;
                }
            }

            base.OnManipulationDelta(e);
        }

        private bool VerticalMove(double verticalChange)
        {
            if (CurrentRow == null || SiblingRow == null)
            {
                return true;
            }

            // if current row has fixed height then resize it
            if (!IsStarRow(CurrentRow))
            {
                // No need to check for the row Min height because it is automatically respected
                SetRowHeight(CurrentRow, verticalChange, GridUnitType.Pixel);
            }

            // if sibling row has fixed width then resize it
            else if (!IsStarRow(SiblingRow))
            {
                SetRowHeight(SiblingRow, verticalChange * -1, GridUnitType.Pixel);
            }

            // if both row haven't fixed height (auto *)
            else
            {
                // change current row height to the new height with respecting the auto
                // change sibling row height to the new height relative to current row
                // respect the other star row height by setting it's height to it's actual height with stars

                // We need to validate current and sibling height to not cause any un expected behavior
                if (!IsValidRowHeight(CurrentRow, verticalChange) || !IsValidRowHeight(SiblingRow, verticalChange * -1))
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

        private bool HorizontalMove(double horizontalChange)
        {
            if (CurrentColumn == null || SiblingColumn == null)
            {
                return true;
            }

            // if current column has fixed width then resize it
            if (!IsStarColumn(CurrentColumn))
            {
                // No need to check for the Column Min width because it is automatically respected
                SetColumnWidth(CurrentColumn, horizontalChange, GridUnitType.Pixel);
            }

            // if sibling column has fixed width then resize it
            else if (!IsStarColumn(SiblingColumn))
            {
                SetColumnWidth(SiblingColumn, horizontalChange * -1, GridUnitType.Pixel);
            }

            // if both column haven't fixed width (auto *)
            else
            {
                // change current column width to the new width with respecting the auto
                // change sibling column width to the new width relative to current column
                // respect the other star column width by setting it's width to it's actual width with stars

                // We need to validate current and sibling width to not cause any un expected behavior
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
