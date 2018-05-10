// ******************************************************************
// Copyright (c) Microsoft. All rights reserved.
// This code is licensed under the MIT License (MIT).
// THE CODE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
// TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH
// THE CODE OR THE USE OR OTHER DEALINGS IN THE CODE.
// ******************************************************************

using System.Text.RegularExpressions;
using Windows.Foundation;
using Windows.System;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    /// <summary>
    /// InfiniteCanvas is a canvas that supports Ink, Text, Format Text, Zoom in/out, Redo, Undo, Export canvas data, Import canvas data.
    /// </summary>
    public partial class InfiniteCanvas
    {
        private const int DefaultFontValue = 22;
        private Point _lastInputPoint;

        private TextDrawable SelectedTextDrawable => _drawingSurfaceRenderer.GetSelectedTextDrawable();

        private int _lastValidTextFontSizeValue = DefaultFontValue;

        private int TextFontSize
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(_canvasTextBoxFontSizeTextBox.Text) &&
                    Regex.IsMatch(_canvasTextBoxFontSizeTextBox.Text, "^[0-9]*$"))
                {
                    var fontSize = int.Parse(_canvasTextBoxFontSizeTextBox.Text);
                    _lastValidTextFontSizeValue = fontSize;
                }

                return _lastValidTextFontSizeValue;
            }
        }

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

        private void CanvasTextBoxBoldButton_Clicked(object sender, RoutedEventArgs e)
        {
            if (SelectedTextDrawable != null)
            {
                _drawingSurfaceRenderer.ExecuteUpdateTextBoxWeight(_canvasTextBoxBoldButton.IsChecked ?? false);
                _canvasTextBox.UpdateFontWeight(SelectedTextDrawable.IsBold);
                ReDrawCanvas();
            }
        }

        private void CanvasTextBoxItlaicButton_Clicked(object sender, RoutedEventArgs e)
        {
            if (SelectedTextDrawable != null)
            {
                _drawingSurfaceRenderer.ExecuteUpdateTextBoxStyle(_canvasTextBoxItlaicButton.IsChecked ?? false);
                _canvasTextBox.UpdateFontStyle(SelectedTextDrawable.IsItalic);
                ReDrawCanvas();
            }
        }

        private void CanvasTextBoxFontSizeTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            _canvasTextBox.UpdateFontSize(TextFontSize);
            if (SelectedTextDrawable != null)
            {
                _drawingSurfaceRenderer.ExecuteUpdateTextBoxFontSize(TextFontSize);
                ReDrawCanvas();
            }
        }

        private void CanvasTextBox_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            SelectedTextDrawable?.UpdateBounds(_canvasTextBox.ActualWidth, _canvasTextBox.ActualHeight);
        }

        private void CanvasTextBoxColorPicker_ColorChanged(ColorPicker sender, ColorChangedEventArgs args)
        {
            if (SelectedTextDrawable != null)
            {
                _drawingSurfaceRenderer.ExecuteUpdateTextBoxColor(_canvasTextBoxColorPicker.Color);
                ReDrawCanvas();
            }

            _fontColorIcon.Foreground = new SolidColorBrush(_canvasTextBoxColorPicker.Color);
        }

        private void CanvasTextBox_TextChanged(object sender, string text)
        {
            if (string.IsNullOrEmpty(text) && SelectedTextDrawable == null)
            {
                return;
            }

            if (SelectedTextDrawable != null)
            {
                if (string.IsNullOrEmpty(text))
                {
                    _drawingSurfaceRenderer.ExecuteRemoveTextBox();
                    _drawingSurfaceRenderer.ResetSelectedTextDrawable();
                }
                else
                {
                    if (SelectedTextDrawable.Text != text)
                    {
                        _drawingSurfaceRenderer.ExecuteUpdateTextBoxText(text);
                    }
                }

                ReDrawCanvas();
                return;
            }

            _drawingSurfaceRenderer.ExecuteCreateTextBox(
                _lastInputPoint.X,
                _lastInputPoint.Y,
                _canvasTextBox.GetEditZoneWidth(),
                _canvasTextBox.GetEditZoneHeight(),
                TextFontSize,
                text,
                _canvasTextBoxColorPicker.Color,
                _canvasTextBoxBoldButton.IsChecked ?? false,
                _canvasTextBoxItlaicButton.IsChecked ?? false);

            ReDrawCanvas();
            _drawingSurfaceRenderer.UpdateSelectedTextDrawable();
        }

        private void InkScrollViewer_PointerPressed(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            if (_enableTextButton.IsChecked ?? false)
            {
                var point = e.GetCurrentPoint(_infiniteCanvasScrollViewer);
                _lastInputPoint = new Point((point.Position.X + _infiniteCanvasScrollViewer.HorizontalOffset) / _infiniteCanvasScrollViewer.ZoomFactor, (point.Position.Y + _infiniteCanvasScrollViewer.VerticalOffset) / _infiniteCanvasScrollViewer.ZoomFactor);

                _drawingSurfaceRenderer.UpdateSelectedTextDrawableIfSelected(_lastInputPoint, ViewPort);

                if (SelectedTextDrawable != null)
                {
                    _canvasTextBox.Visibility = Visibility.Visible;
                    _canvasTextBox.SetText(SelectedTextDrawable.Text);

                    Canvas.SetLeft(_canvasTextBox, SelectedTextDrawable.Bounds.X);
                    Canvas.SetTop(_canvasTextBox, SelectedTextDrawable.Bounds.Y);
                    _canvasTextBox.UpdateFontSize(SelectedTextDrawable.FontSize);
                    _canvasTextBox.UpdateFontStyle(SelectedTextDrawable.IsItalic);
                    _canvasTextBox.UpdateFontWeight(SelectedTextDrawable.IsBold);

                    // Updating toolbar
                    _canvasTextBoxColorPicker.Color = SelectedTextDrawable.TextColor;
                    _canvasTextBoxFontSizeTextBox.Text = SelectedTextDrawable.FontSize.ToString();
                    _canvasTextBoxBoldButton.IsChecked = SelectedTextDrawable.IsBold;
                    _canvasTextBoxItlaicButton.IsChecked = SelectedTextDrawable.IsBold;

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
            _drawingSurfaceRenderer.ResetSelectedTextDrawable();
            _canvasTextBox.Clear();
        }
    }
}
