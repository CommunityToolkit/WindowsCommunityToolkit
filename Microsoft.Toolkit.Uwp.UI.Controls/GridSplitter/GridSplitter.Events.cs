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
            if (ResizeDirection == ResizeDirection.Columns)
            {
                Window.Current.CoreWindow.PointerCursor = ColumnsSplitterCursor;
            }
            else if (ResizeDirection == ResizeDirection.Rows)
            {
                Window.Current.CoreWindow.PointerCursor = RowSplitterCursor;
            }
        }

        private void Splitter_DragDelta(object sender, DragDeltaEventArgs e)
        {
            if (CurrentColumn == null)
            {
                return;
            }

            if (ResizeDirection == ResizeDirection.Columns)
            {
                var newWidth = CurrentColumn.ActualWidth + e.HorizontalChange;

                if (newWidth > 0)
                {
                    CurrentColumn.Width = new GridLength(newWidth);
                }
            }
            else if (ResizeDirection == ResizeDirection.Rows)
            {
                var newHeight = CurrentRow.ActualHeight + e.VerticalChange;

                if (newHeight > 0)
                {
                    CurrentRow.Height = new GridLength(newHeight);
                }
            }
        }
    }
}
