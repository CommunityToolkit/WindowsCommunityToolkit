using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    /// <summary>
    /// Represents the control that redistributes space between columns or rows of a Grid control.
    /// </summary>
    public partial class GridSplitter
    {
        // GridSplitter has special Behavior when columns are fixed
        // If the left column is fixed, splitter will only resize that column
        // Else if the right column is fixed, splitter will only resize the right column
        private enum SplitBehavior
        {
            Split, // Both columns/rows are star lengths
            Resize1, // resize 1 only
            Resize2, // resize 2 only
        }

        // Only store resize data if we are resizing
        private class ResizeData
        {
            // The constraints to keep the Preview within valid ranges
            public double MinChange { get; set; }

            public double MaxChange { get; set; }

            // The grid to Resize
            public Grid Grid { get; set; }

            // cache of Resize Direction and Behavior
            public GridResizeDirection ResizeDirection { get; set; }

            public GridResizeBehavior ResizeBehavior { get; set; }

            // The columns/rows to resize
            public DependencyObject Definition1 { get; set; }

            public DependencyObject Definition2 { get; set; }

            // Are the columns/rows star lengths
            public SplitBehavior SplitBehavior { get; set; }

            // The index of the splitter
            public int SplitterIndex { get; set; }

            // The indices of the columns/rows
            public int Definition1Index { get; set; }

            public int Definition2Index { get; set; }

            // The minimum of Width/Height of Splitter.  Used to ensure splitter
            // isn't hidden by resizing a row/column smaller than the splitter
            public double SplitterLength { get; set; }
        }

        /// <summary>
        /// Enum to indicate whether GridSplitter resizes Columns or Rows
        /// </summary>
        public enum GridResizeDirection
        {
            /// <summary>
            /// Determines whether to resize rows or columns based on its Alignment and
            /// width compared to height
            /// </summary>
            Auto,

            /// <summary>
            /// Resize columns when dragging Splitter.
            /// </summary>
            Columns,

            /// <summary>
            /// Resize rows when dragging Splitter.
            /// </summary>
            Rows
        }

        /// <summary>
        /// Enum to indicate what Columns or Rows the GridSplitter resizes
        /// </summary>
        public enum GridResizeBehavior
        {
            /// <summary>
            /// Determine which columns or rows to resize based on its Alignment.
            /// </summary>
            BasedOnAlignment,

            /// <summary>
            /// Resize the current and next Columns or Rows.
            /// </summary>
            CurrentAndNext,

            /// <summary>
            /// Resize the previous and current Columns or Rows.
            /// </summary>
            PreviousAndCurrent,

            /// <summary>
            /// Resize the previous and next Columns or Rows.
            /// </summary>
            PreviousAndNext
        }
    }
}
