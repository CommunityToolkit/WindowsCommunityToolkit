using System;
using System.Collections;
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

        private GridResizeDirection _resizeDirection;
        private GridResizeBehavior _resizeBehavior;

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
            _resizeBehavior = GetEffectiveResizeBehavior();
            UpdateDisplayIcon();

            // Register Events
            _splitter.DragStarted += Splitter_DragStarted;
            _splitter.DragDelta += Splitter_DragDelta;
            _splitter.DragCompleted += Splitter_DragCompleted;

            if (_resizeDirection == GridResizeDirection.Columns)
            {
                if (CurrentColumn != null)
                {
                    // To overcome the relative column width resize issues etc: Width=*
                    CurrentColumn.Width = new GridLength(CurrentColumn.ActualWidth);
                }
            }
            else if (_resizeDirection == GridResizeDirection.Rows)
            {
                if (CurrentColumn != null)
                {
                    // To overcome the relative row height resize issues etc: height=*
                    CurrentRow.Height = new GridLength(CurrentRow.ActualHeight);
                }
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

        // Return the targeted Column based on the resize behavior
        private int GetTargetedColumn()
        {
            var currentIndex = Grid.GetColumn(this);
            switch (_resizeBehavior)
            {
                case GridResizeBehavior.CurrentAndNext:
                    return currentIndex;
                case GridResizeBehavior.PreviousAndCurrent:
                    return currentIndex - 1;
                default:
                    return -1;
            }
        }

        // Return the targeted Row based on the resize behavior
        private int GetTargetedRow()
        {
            return Grid.GetRow(this);
        }

        // Converts BasedOnAlignment direction to Rows, Columns, or Both depending on its width/height
        private GridResizeDirection GetEffectiveResizeDirection()
        {
            GridResizeDirection direction = ResizeDirection;

            if (direction == GridResizeDirection.Auto)
            {
                // When HorizontalAlignment is Left, Right or Center, resize Columns
                if (HorizontalAlignment != HorizontalAlignment.Stretch)
                {
                    direction = GridResizeDirection.Columns;
                }
                else if (VerticalAlignment != VerticalAlignment.Stretch)
                {
                    direction = GridResizeDirection.Rows;
                }

                // Fall back to Width vs Height
                else if (ActualWidth <= ActualHeight)
                {
                    direction = GridResizeDirection.Columns;
                }
                else
                {
                    direction = GridResizeDirection.Rows;
                }
            }

            return direction;
        }

        // Convert BasedOnAlignment to Next/Prev/Both depending on alignment and Direction
        private GridResizeBehavior GetEffectiveResizeBehavior()
        {
            GridResizeBehavior resizeBehavior = ResizeBehavior;

            if (resizeBehavior == GridResizeBehavior.BasedOnAlignment)
            {
                if (_resizeDirection == GridResizeDirection.Columns)
                {
                    switch (HorizontalAlignment)
                    {
                        case HorizontalAlignment.Left:
                            resizeBehavior = GridResizeBehavior.PreviousAndCurrent;
                            break;
                        default:
                            resizeBehavior = GridResizeBehavior.CurrentAndNext;
                            break;
                    }
                }
                else
                {
                    switch (VerticalAlignment)
                    {
                        case VerticalAlignment.Top:
                            resizeBehavior = GridResizeBehavior.PreviousAndCurrent;
                            break;
                        default:
                            resizeBehavior = GridResizeBehavior.CurrentAndNext;
                            break;
                    }
                }
            }

            return resizeBehavior;
        }
    }
}
