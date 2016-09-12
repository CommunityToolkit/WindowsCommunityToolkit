using Windows.UI.Core;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    /// <summary>
    /// Represents the control that redistributes space between columns or rows of a Grid control.
    /// </summary>
    public partial class GridSplitter : Control
    {
        private static readonly CoreCursor ColumnsSplitterCursor = new CoreCursor(CoreCursorType.SizeWestEast, 1);
        private static readonly CoreCursor RowSplitterCursor = new CoreCursor(CoreCursorType.SizeNorthSouth, 1);
        private CoreCursor _previousCursor;

        private GridResizeDirection _resizeDirection;
        private GridResizeBehavior _resizeBehavior;

        /// <summary>
        /// Gets GridSplitter Container Grid
        /// </summary>
        private Grid Resizable => Parent as Grid;

        /// <summary>
        /// Gets the current Column definition of the parent Grid
        /// </summary>
        private ColumnDefinition CurrentColumn
        {
            get
            {
                if (Resizable == null)
                {
                    return null;
                }

                var gridSplitterTargetedColumnIndex = GetTargetedColumn();

                if ((gridSplitterTargetedColumnIndex >= 0)
                    && (gridSplitterTargetedColumnIndex < Resizable.ColumnDefinitions.Count))
                {
                    return Resizable.ColumnDefinitions[gridSplitterTargetedColumnIndex];
                }

                return null;
            }
        }

        /// <summary>
        /// Gets the Sibling Column definition of the parent Grid
        /// </summary>
        private ColumnDefinition SiblingColumn
        {
            get
            {
                if (Resizable == null)
                {
                    return null;
                }

                var gridSplitterSiblingColumnIndex = GetSiblingColumn();

                if ((gridSplitterSiblingColumnIndex >= 0)
                    && (gridSplitterSiblingColumnIndex < Resizable.ColumnDefinitions.Count))
                {
                    return Resizable.ColumnDefinitions[gridSplitterSiblingColumnIndex];
                }

                return null;
            }
        }

        /// <summary>
        /// Gets the current Row definition of the parent Grid
        /// </summary>
        private RowDefinition CurrentRow
        {
            get
            {
                if (Resizable == null)
                {
                    return null;
                }

                var gridSplitterTargetedRowIndex = GetTargetedRow();

                if ((gridSplitterTargetedRowIndex >= 0)
                    && (gridSplitterTargetedRowIndex < Resizable.RowDefinitions.Count))
                {
                    return Resizable.RowDefinitions[gridSplitterTargetedRowIndex];
                }

                return null;
            }
        }

        /// <summary>
        /// Gets the Sibling Column definition of the parent Grid
        /// </summary>
        private RowDefinition SiblingRow
        {
            get
            {
                if (Resizable == null)
                {
                    return null;
                }

                var gridSplitterSiblingRowIndex = GetSiblingRow();

                if ((gridSplitterSiblingRowIndex >= 0)
                    && (gridSplitterSiblingRowIndex < Resizable.ColumnDefinitions.Count))
                {
                    return Resizable.RowDefinitions[gridSplitterSiblingRowIndex];
                }

                return null;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GridSplitter"/> class.
        /// </summary>
        public GridSplitter()
        {
            DefaultStyleKey = typeof(GridSplitter);
        }

        /// <inheritdoc />
        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            // Unhook registered events
            Loaded -= GridSplitter_Loaded;

            // Register Events
            Loaded += GridSplitter_Loaded;

            ManipulationMode = ManipulationModes.TranslateX | ManipulationModes.TranslateY;
        }
    }
}
