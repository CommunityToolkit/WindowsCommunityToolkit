using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls.Primitives;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    /// <summary>
    /// Represents the control that redistributes space between columns or rows of a Grid control.
    /// </summary>
    public partial class GridSplitter
    {
        private static void Splitter_DragCompleted(object sender, DragCompletedEventArgs e)
        {
            Window.Current.CoreWindow.PointerCursor = ArrowCursor;
        }

        private void Splitter_DragStarted(object sender, DragStartedEventArgs e)
        {
            if (_resizeDirection == GridResizeDirection.Columns)
            {
                Window.Current.CoreWindow.PointerCursor = ColumnsSplitterCursor;
            }
            else if (_resizeDirection == GridResizeDirection.Rows)
            {
                Window.Current.CoreWindow.PointerCursor = RowSplitterCursor;
            }
        }

        private void Splitter_DragDelta(object sender, DragDeltaEventArgs e)
        {
            if (_resizeDirection == GridResizeDirection.Columns)
            {
                if (CurrentColumn == null)
                {
                    return;
                }

                // if current column has fixed width then resize it
                if (!IsStarColumn(CurrentColumn))
                {
                    // No need to check for the Column Min width because it is automatically respected
                    SetColumnWidth(CurrentColumn, e.HorizontalChange, GridUnitType.Pixel);
                }

                // if sibling column has fixed width then resize it
                else if (!IsStarColumn(SiblingColumn))
                {
                    SetColumnWidth(SiblingColumn, e.HorizontalChange * -1, GridUnitType.Pixel);
                }

                // if both column haven't fixed width (auto *)
                else
                {
                    // change current column width to the new width with respecting the auto
                    // change sibling column width to the new width relative to current column
                    // respect the other star column width by setting it's width to it's actual width with stars
                    foreach (var columnDefinition in Resizable.ColumnDefinitions)
                    {
                        if (columnDefinition == CurrentColumn)
                        {
                            SetColumnWidth(CurrentColumn, e.HorizontalChange, GridUnitType.Star);
                        }
                        else if (SiblingColumn == CurrentColumn)
                        {
                            SetColumnWidth(SiblingColumn, e.HorizontalChange * -1, GridUnitType.Star);
                        }
                        else
                        {
                            columnDefinition.Width = new GridLength(columnDefinition.ActualWidth, GridUnitType.Star);
                        }
                    }
                }
            }
            else if (_resizeDirection == GridResizeDirection.Rows)
            {
                if (CurrentRow == null)
                {
                    return;
                }

                // No need to check for the Row Min height because it is automatically respected
                var newHeight = CurrentRow.ActualHeight + e.VerticalChange;

                if (newHeight > 0)
                {
                    CurrentRow.Height = new GridLength(newHeight);
                }
            }
        }
    }
}
