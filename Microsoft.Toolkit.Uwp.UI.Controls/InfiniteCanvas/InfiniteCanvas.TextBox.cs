using Windows.Foundation;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    public partial class InfiniteCanvas
    {
        Point _lastInputPoint;

        public int TextFontSize => string.IsNullOrWhiteSpace(_canvasTextBoxFontSizeTextBox.Text) ? 22 : int.Parse(_canvasTextBoxFontSizeTextBox.Text);

        private void InkScrollViewer_PreviewKeyDown(object sender, Windows.UI.Xaml.Input.KeyRoutedEventArgs e)
        {
            // fixing scroll viewer issue with text box when you hit UP/DOWN/Right/LEFT
            if (_canvasTextBox.Visibility != Visibility.Visible)
            {
                return;
            }

            if (((e.Key == VirtualKey.PageUp || e.Key == VirtualKey.Up) && _canvasTextBox.CannotGoUp()) || ((e.Key == VirtualKey.PageDown || e.Key == VirtualKey.Down) && _canvasTextBox.CannotGoDown()))
            {
                e.Handled = true;
                return;
            }

            if (((e.Key == VirtualKey.Right || e.Key == VirtualKey.End) && _canvasTextBox.CannotGoRight())
                || ((e.Key == VirtualKey.Left || e.Key == VirtualKey.Home) && _canvasTextBox.CannotGoLeft()))
            {
                e.Handled = true;
            }
        }

        private void _canvasTextBoxBoldButton_Checked(object sender, RoutedEventArgs e)
        {
            if (_selectedTextDrawable != null)
            {
                _selectedTextDrawable.IsBold = _canvasTextBoxBoldButton.IsChecked ?? false;
                _canvasTextBox.UpdateFontStyle(_selectedTextDrawable.IsBold);
                ReDrawCanvas();
            }
        }

        private void _canvasTextBoxItlaicButton_Checked(object sender, RoutedEventArgs e)
        {
            if (_selectedTextDrawable != null)
            {
                _selectedTextDrawable.IsItalic = _canvasTextBoxItlaicButton.IsChecked ?? false;
                _canvasTextBox.UpdateFontStyle(_selectedTextDrawable.IsItalic);
                ReDrawCanvas();
            }
        }
        
        private void _canvasTextBoxFontSizeTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            _canvasTextBox.UpdateFontSize(TextFontSize);
            if (_selectedTextDrawable != null)
            {
                _selectedTextDrawable.FontSize = TextFontSize;
                ReDrawCanvas();
            }
        }

        private void _canvasTextBox_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            _selectedTextDrawable?.UpdateBounds(_canvasTextBox.ActualWidth, _canvasTextBox.ActualHeight);
        }

        private void _canvasTextBoxColorPicker_ColorChanged(ColorPicker sender, ColorChangedEventArgs args)
        {
            if (_selectedTextDrawable != null)
            {
                _selectedTextDrawable.TextColor = _canvasTextBoxColorPicker.Color;
                ReDrawCanvas();
            }
        }

        private TextDrawable _selectedTextDrawable => _canvasOne.GetSelectedTextDrawable();

        private void _canvasTextBox_TextChanged(object sender, string text)
        {
            if (string.IsNullOrEmpty(text) && _selectedTextDrawable == null)
            {
                return;
            }

            if (_selectedTextDrawable != null)
            {
                if (string.IsNullOrEmpty(text))
                {
                    _canvasOne.RemoveDrawable(_selectedTextDrawable);
                    _canvasOne.ResetSelectedTextDrawable();
                }
                else
                {
                    if (_selectedTextDrawable.Text != text)
                    {
                        _selectedTextDrawable.Text = text;
                    }
                }

                ReDrawCanvas();
                return;
            }

            var textDrawable = new TextDrawable(
                _lastInputPoint.X,
                _lastInputPoint.Y,
                _canvasTextBox.GetEditZoneWidth(),
                _canvasTextBox.GetEditZoneHeight(),
                TextFontSize,
                text,
                _canvasTextBoxColorPicker.Color,
                _canvasTextBoxBoldButton.IsChecked ?? false,
                _canvasTextBoxItlaicButton.IsChecked ?? false);

            _canvasOne.AddDrawable(textDrawable);
            _canvasOne.ReDraw(ViewPort);
            _canvasOne.UpdateSelectedTextDrawable();
        }

        private void InkScrollViewer_PointerPressed(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            if (_enableTextButton.IsChecked ?? false)
            {
                var point = e.GetCurrentPoint(inkScrollViewer);
                _lastInputPoint = new Point((point.Position.X + inkScrollViewer.HorizontalOffset) / inkScrollViewer.ZoomFactor, (point.Position.Y + inkScrollViewer.VerticalOffset) / inkScrollViewer.ZoomFactor);

                _canvasOne.UpdateSelectedTextDrawableIfSelected(_lastInputPoint, ViewPort);

                if (_selectedTextDrawable != null)
                {
                    _canvasTextBox.Visibility = Visibility.Visible;
                    _canvasTextBox.SetText(_selectedTextDrawable.Text);

                    Canvas.SetLeft(_canvasTextBox, _selectedTextDrawable.Bounds.X);
                    Canvas.SetTop(_canvasTextBox, _selectedTextDrawable.Bounds.Y);
                    _canvasTextBoxColorPicker.Color = _selectedTextDrawable.TextColor;
                    _canvasTextBox.UpdateFontSize(_selectedTextDrawable.FontSize);
                    _canvasTextBox.UpdateFontStyle(_selectedTextDrawable.IsItalic);
                    _canvasTextBox.UpdateFontWeight(_selectedTextDrawable.IsBold);

                    return;
                }

                _canvasTextBox.UpdateFontSize(TextFontSize);
                _canvasTextBox.UpdateFontStyle(_canvasTextBoxItlaicButton.IsChecked ?? false);
                _canvasTextBox.UpdateFontWeight(_canvasTextBoxBoldButton.IsChecked ?? false);

                _inkCanvas.Visibility = Visibility.Collapsed;
                ClearTextBoxValue();
                _canvasTextBox.Visibility = Visibility.Visible;
                Canvas.SetLeft(_canvasTextBox, _lastInputPoint.X);
                Canvas.SetTop(_canvasTextBox, _lastInputPoint.Y);
            }
        }

        private void ClearTextBoxValue()
        {
            _canvasOne.ResetSelectedTextDrawable();
            _canvasTextBox.Clear();
        }
    }
}
