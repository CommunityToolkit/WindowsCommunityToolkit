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
            if (_resizeData.ResizeDirection == GridResizeDirection.Columns)
            {
                Window.Current.CoreWindow.PointerCursor = ColumnsSplitterCursor;
            }
            else if (_resizeData.ResizeDirection == GridResizeDirection.Rows)
            {
                Window.Current.CoreWindow.PointerCursor = RowSplitterCursor;
            }
        }

        private void Splitter_DragDelta(object sender, DragDeltaEventArgs e)
        {
            double horizontalChange = e.HorizontalChange;
            double verticalChange = e.VerticalChange;

            // Round change to nearest multiple of DragIncrement
            double dragIncrement = DragIncrement;
            horizontalChange = Math.Round(horizontalChange / dragIncrement) * dragIncrement;
            verticalChange = Math.Round(verticalChange / dragIncrement) * dragIncrement;

            // Directly update the grid
            MoveSplitter(horizontalChange, verticalChange);
        }
    }
}
