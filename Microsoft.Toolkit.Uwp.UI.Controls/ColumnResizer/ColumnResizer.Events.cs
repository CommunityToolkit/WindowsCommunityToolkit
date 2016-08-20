using Windows.UI.Xaml;
using Windows.UI.Xaml.Input;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    /// <summary>
    /// ColumnResizer is a UI control that add the resizing functionality to a Grid Column.
    /// </summary>
    public partial class ColumnResizer
    {
        /// <summary>
        /// Parent Grid On Pointer Moved Event
        /// </summary>
        private void Resizable_PointerMoved(object sender, PointerRoutedEventArgs e)
        {
            if (_isResizing)
            {
                var point = e.GetCurrentPoint(Resizable);
                if (point.Position.X - FixedOffset >= 0)
                {
                    CurrentColumn.Width = new GridLength(point.Position.X - FixedOffset);
                }
            }
        }

        /// <summary>
        /// Parent Grid On Pointer Moved Event
        /// </summary>
        private void Resizable_PointerReleased(object sender, PointerRoutedEventArgs e)
        {
            _isResizing = false;
        }

        /// <summary>
        /// Grid Resizer Control on Pointer Pressed Event
        /// </summary>
        private void Resizer_PointerPressed(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            if (!_isResizing)
            {
                _isResizing = true;
            }
        }

        /// <summary>
        /// Grid Resizer Control on Pointer Entered Event, used to change the cursor to resize cursor
        /// </summary>
        private void Resizer_PointerEntered(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            Window.Current.CoreWindow.PointerCursor = _resizeCursor;
            _resizer.Opacity = EnteredOpacity;
        }

        /// <summary>
        /// Grid Resizer Control on Pointer Exited Event, used to change the cursor to the normal cursor
        /// </summary>
        private void Resizer_PointerExited(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            Window.Current.CoreWindow.PointerCursor = _arrowCursor;
            _resizer.Opacity = DefaultOpacity;
        }
    }
}
