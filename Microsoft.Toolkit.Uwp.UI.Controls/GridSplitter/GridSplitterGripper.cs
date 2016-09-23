using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    internal class GridSplitterGripper : Grid
    {
        // Symbol GripperBarVertical in Segoe MDL2 Assets
        private const string GripperBarVertical = "\xE784";

        // Symbol GripperBarHorizontal in Segoe MDL2 Assets
        private const string GripperBarHorizontal = "\xE76F";
        private const string GripperDisplayFont = "Segoe MDL2 Assets";
        private readonly GridSplitter.GridResizeDirection _gridSplitterDirection;
        private readonly TextBlock _gripperDisplay;

        private CoreCursor _splitterPreviousPointer;
        private CoreCursor _previousCursor;
        private GridSplitter.GripperCursorType _gripperCursor;
        private int _gripperCustomCursorResource;
        private bool _isDragging;

        internal Brush GripperForeground
        {
            get
            {
                return _gripperDisplay.Foreground;
            }

            set
            {
                _gripperDisplay.Foreground = value;
            }
        }

        internal GridSplitter.GripperCursorType GripperCursor
        {
            get
            {
                return _gripperCursor;
            }

            set
            {
                _gripperCursor = value;
            }
        }

        internal int GripperCustomCursorResource
        {
            get
            {
                return _gripperCustomCursorResource;
            }

            set
            {
                _gripperCustomCursorResource = value;
            }
        }

        internal GridSplitterGripper(
            GridSplitter.GridResizeDirection gridSplitterDirection,
            Brush gripForeground,
            GridSplitter.GripperCursorType gripperCursor,
            int gripperCustomCursorResource)
        {
            _gripperDisplay = new TextBlock();
            _gripperDisplay.FontFamily = new FontFamily(GripperDisplayFont);
            _gripperDisplay.HorizontalAlignment = HorizontalAlignment.Center;
            _gripperDisplay.VerticalAlignment = VerticalAlignment.Center;
            _gripperDisplay.Foreground = gripForeground;
            _gridSplitterDirection = gridSplitterDirection;
            _gripperCursor = gripperCursor;
            _gripperCustomCursorResource = gripperCustomCursorResource;

            if (_gridSplitterDirection == GridSplitter.GridResizeDirection.Columns)
            {
                _gripperDisplay.Text = GripperBarVertical;
            }
            else if (_gridSplitterDirection == GridSplitter.GridResizeDirection.Rows)
            {
                _gripperDisplay.Text = GripperBarHorizontal;
            }

            _gripperDisplay.PointerEntered += GripperDisplay_PointerEntered;
            _gripperDisplay.PointerExited += GripperDisplay_PointerExited;
            Children.Add(_gripperDisplay);
        }

        private void GripperDisplay_PointerExited(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            if (_isDragging)
            {
                // if dragging don't update the curser just update the splitter cursor with the last window cursor,
                // because the splitter is still using the arrow cursor and will revert to original case when drag completes
                _splitterPreviousPointer = _previousCursor;
            }
            else
            {
                Window.Current.CoreWindow.PointerCursor = _previousCursor;
            }
        }

        private void GripperDisplay_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            // if not dragging
            if (!_isDragging)
            {
                _previousCursor = _splitterPreviousPointer = Window.Current.CoreWindow.PointerCursor;
                UpdateDisplayCursor();
            }

            // if dragging
            else
            {
                _previousCursor = _splitterPreviousPointer;
            }
        }

        private void UpdateDisplayCursor()
        {
            if (_gripperCursor == GridSplitter.GripperCursorType.Default)
            {
                if (_gridSplitterDirection == GridSplitter.GridResizeDirection.Columns)
                {
                    Window.Current.CoreWindow.PointerCursor = GridSplitter.ColumnsSplitterCursor;
                }
                else if (_gridSplitterDirection == GridSplitter.GridResizeDirection.Rows)
                {
                    Window.Current.CoreWindow.PointerCursor = GridSplitter.RowSplitterCursor;
                }
            }
            else
            {
                var coreCursor = (CoreCursorType)((int)_gripperCursor);
                if (_gripperCursor == GridSplitter.GripperCursorType.Custom)
                {
                    if (_gripperCustomCursorResource > GridSplitter.GripperCustomCursorDefaultResource)
                    {
                        Window.Current.CoreWindow.PointerCursor = new CoreCursor(coreCursor, (uint)_gripperCustomCursorResource);
                    }
                }
                else
                {
                    Window.Current.CoreWindow.PointerCursor = new CoreCursor(coreCursor, 1);
                }
            }
        }

        internal void SplitterManipulationStarted(object sender, ManipulationStartedRoutedEventArgs e)
        {
            var splitter = sender as GridSplitter;
            if (splitter == null)
            {
                return;
            }

            _splitterPreviousPointer = splitter.PreviousCursor;
            _isDragging = true;
        }

        internal void SplitterManipulationCompleted(object sender, ManipulationCompletedRoutedEventArgs e)
        {
            var splitter = sender as GridSplitter;
            if (splitter == null)
            {
                return;
            }

            Window.Current.CoreWindow.PointerCursor = splitter.PreviousCursor = _splitterPreviousPointer;
            _isDragging = false;
        }
    }
}
