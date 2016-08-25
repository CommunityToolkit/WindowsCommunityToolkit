namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    /// <summary>
    /// Represents the control that redistributes space between columns or rows of a Grid control.
    /// </summary>
    public partial class GridSplitter
    {
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
