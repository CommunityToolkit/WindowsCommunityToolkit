using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;

// The Templated Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234235

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    [TemplatePart(Name = RESIZERNAME, Type = typeof(Grid))]
    public sealed class ColumnResizer : Control
    {
        public ColumnResizer()
        {
            this.DefaultStyleKey = typeof(ColumnResizer);
        }

        private const string RESIZERNAME = "Resizer";

        private Grid _resizer;
        private readonly CoreCursor _resizeCursor = new CoreCursor(CoreCursorType.SizeWestEast, 1);
        private readonly CoreCursor _arrowCursor = new CoreCursor(CoreCursorType.Arrow, 1);
        private readonly double _enteredOpacity = .7;
        private readonly double _defaultOpacity = 1;
        private const int FixedOffset = 5;

        /// <summary>
        /// Gets Column Resizer Container Grid
        /// </summary>
        private Grid Resizable => _resizer.FindVisualAscendant<Grid>();

        private bool _isResizing;

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _resizer = GetTemplateChild(RESIZERNAME) as Grid;
            if (_resizer == null)
            {
                return;
            }

            _resizer.PointerPressed += _resizer_PointerPressed;
            _resizer.PointerEntered += _resizer_PointerEntered;
            _resizer.PointerExited += _resizer_PointerExited;

            Resizable.PointerReleased += _resizable_PointerReleased;
            Resizable.PointerMoved += _resizable_PointerMoved;
        }

        private ColumnDefinition CurrentColumn
        {
            get
            {
                var columnResizerColumnProperty = (int)_resizer.GetValue(Grid.ColumnProperty);
                return Resizable.ColumnDefinitions[columnResizerColumnProperty];
            }
        }

        private void _resizable_PointerMoved(object sender, PointerRoutedEventArgs e)
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

        private void _resizable_PointerReleased(object sender, PointerRoutedEventArgs e)
        {
            _isResizing = false;
        }

        private void _resizer_PointerPressed(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            if (!_isResizing)
            {
                _isResizing = true;
            }
        }

        private void _resizer_PointerEntered(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            Window.Current.CoreWindow.PointerCursor = _resizeCursor;
            _resizer.Opacity = _enteredOpacity;
        }

        private void _resizer_PointerExited(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            Window.Current.CoreWindow.PointerCursor = _arrowCursor;
            _resizer.Opacity = _defaultOpacity;
        }
    }
}
