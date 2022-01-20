// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

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

        /// <summary>
        ///  Enum to indicate the behavior of window cursor on grid splitter hover
        /// </summary>
        public enum SplitterCursorBehavior
        {
            /// <summary>
            /// Update window cursor on Grid Splitter hover
            /// </summary>
            ChangeOnSplitterHover,

            /// <summary>
            /// Update window cursor on Grid Splitter Gripper hover
            /// </summary>
            ChangeOnGripperHover
        }
    }
}