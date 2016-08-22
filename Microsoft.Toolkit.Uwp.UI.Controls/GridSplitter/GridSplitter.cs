using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Media;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    /// <summary>
    /// Represents the control that redistributes space between columns or rows of a Grid control.
    /// </summary>
    [TemplatePart(Name = SPLITTERRNAME, Type = typeof(Thumb))]
    [TemplatePart(Name = ICONDISPLAYNAME, Type = typeof(Thumb))]
    public partial class GridSplitter : Control
    {
        private const string SPLITTERRNAME = "Splitter";
        private const string ICONDISPLAYNAME = "IconDisplay";

        // Symbol GripperBarVertical in Segoe MDL2 Assets
        private const string GripperBarVertical = "\xE784";

        // Symbol GripperBarHorizontal in Segoe MDL2 Assets
        private const string GripperBarHorizontal = "\xE76F";

        private static readonly CoreCursor ColumnsSplitterCursor = new CoreCursor(CoreCursorType.SizeWestEast, 1);
        private static readonly CoreCursor RowSplitterCursor = new CoreCursor(CoreCursorType.SizeNorthSouth, 1);
        private static readonly CoreCursor ArrowCursor = new CoreCursor(CoreCursorType.Arrow, 1);
        private Thumb _splitter;
        private TextBlock _iconDisplay;

        /// <summary>
        /// Gets GridSplitter Container Grid
        /// </summary>
        private Grid Resizable => this.Parent as Grid;

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

                var gridSplitterTargetedColumnIndex = Grid.GetColumn(this);

                if ((gridSplitterTargetedColumnIndex >= 0)
                    && (gridSplitterTargetedColumnIndex < Resizable.ColumnDefinitions.Count))
                {
                    return Resizable.ColumnDefinitions[gridSplitterTargetedColumnIndex];
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

                var gridSplitterTargetedRowIndex = Grid.GetRow(this);

                if ((gridSplitterTargetedRowIndex >= 0)
                    && (gridSplitterTargetedRowIndex < Resizable.RowDefinitions.Count))
                {
                    return Resizable.RowDefinitions[gridSplitterTargetedRowIndex];
                }

                return null;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GridSplitter"/> class.
        /// </summary>
        public GridSplitter()
        {
            this.DefaultStyleKey = typeof(GridSplitter);
        }

        /// <inheritdoc />
        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            if (_splitter != null)
            {
                // Unhook registered events
                _splitter.DragStarted -= Splitter_DragStarted;
                _splitter.DragDelta -= Splitter_DragDelta;
                _splitter.DragCompleted -= Splitter_DragCompleted;
            }

            _splitter = GetTemplateChild(SPLITTERRNAME) as Thumb;
            _iconDisplay = GetTemplateChild(ICONDISPLAYNAME) as TextBlock;
            if (_splitter == null)
            {
                return;
            }

            _resizeDirection = GetEffectiveResizeDirection();
            UpdateDisplayIcon();

            // Register Events
            _splitter.DragStarted += Splitter_DragStarted;
            _splitter.DragDelta += Splitter_DragDelta;
            _splitter.DragCompleted += Splitter_DragCompleted;

            if (_resizeDirection == GridResizeDirection.Columns)
            {
                // To overcome the relative column width resize issues etc: Width=*
                CurrentColumn.Width = new GridLength(CurrentColumn.ActualWidth);
            }
            else if (_resizeDirection == GridResizeDirection.Rows)
            {
                // To overcome the relative row height resize issues etc: height=*
                CurrentRow.Height = new GridLength(CurrentRow.ActualHeight);
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
    }
}
