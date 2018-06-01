// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Linq;
using Windows.UI;
using Windows.UI.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    internal class InfiniteCanvasTextBox : Control
    {
        private TextBox _editZone;

        public event EventHandler<string> TextChanged;

        public InfiniteCanvasTextBox()
        {
            DefaultStyleKey = typeof(InfiniteCanvasTextBox);
        }

        protected override void OnApplyTemplate()
        {
            _editZone = (TextBox)GetTemplateChild("EditZone");
            _editZone.TextChanged -= EditZone_TextChanged;
            _editZone.TextChanged += EditZone_TextChanged;
            _editZone.FontSize = FontSize;
            _editZone.SelectionHighlightColorWhenNotFocused.Color = Color.FromArgb(
                    1,
                    _editZone.SelectionHighlightColor.Color.R,
                    _editZone.SelectionHighlightColor.Color.G,
                    _editZone.SelectionHighlightColor.Color.B);
            _editZone.SelectionHighlightColorWhenNotFocused.Opacity = .1;

            _editZone.SelectionHighlightColor.Color =
                Color.FromArgb(
                    1,
                    _editZone.SelectionHighlightColor.Color.R,
                    _editZone.SelectionHighlightColor.Color.G,
                    _editZone.SelectionHighlightColor.Color.B);

            _editZone.SelectionHighlightColor.Opacity = .1;

            base.OnApplyTemplate();
        }

        private void EditZone_TextChanged(object sender, RoutedEventArgs e)
        {
            TextChanged?.Invoke(this, _editZone.Text);
        }

        public double GetEditZoneWidth()
        {
            return _editZone.ActualWidth;
        }

        public double GetEditZoneHeight()
        {
            return _editZone.ActualHeight;
        }

        public void Clear()
        {
            if (_editZone == null)
            {
                return;
            }

            _editZone.TextChanged -= EditZone_TextChanged;
            _editZone.Text = string.Empty;
            _editZone.TextChanged += EditZone_TextChanged;
        }

        public void SetText(string text)
        {
            if (_editZone == null)
            {
                if (!ApplyTemplate())
                {
                    return;
                }
            }

            _editZone.Text = text;
            _editZone.SelectionStart = text.Length;
        }

        public void UpdateFontSize(float textFontSize)
        {
            FontSize = textFontSize;

            if (_editZone != null)
            {
                _editZone.FontSize = textFontSize;
            }
        }

        public void UpdateFontStyle(bool isItalic)
        {
            if (_editZone != null)
            {
                _editZone.FontStyle = isItalic ? FontStyle.Italic : FontStyle.Normal;
            }
        }

        public void UpdateFontWeight(bool isBold)
        {
            if (_editZone != null)
            {
                _editZone.FontWeight = isBold ? FontWeights.Bold : FontWeights.Normal;
            }
        }

        public bool CannotGoRight()
        {
            return (_editZone.SelectionStart + _editZone.SelectionLength) == _editZone.Text.Length;
        }

        public bool CannotGoLeft()
        {
            return _editZone.SelectionStart == 0;
        }

        public bool CannotGoUp()
        {
            var lines = _editZone.Text.Split('\r');
            if (lines.Count() == 1)
            {
                return true;
            }

            var firstLine = lines.First();
            if (firstLine.Length >= _editZone.SelectionStart)
            {
                return true;
            }

            return false;
        }

        public bool CannotGoDown()
        {
            var lines = _editZone.Text.Split('\r');
            if (lines.Count() == 1)
            {
                return true;
            }

            var lastLine = lines.ElementAt(lines.Length - 1);
            if ((_editZone.Text.Length - lastLine.Length) <= (_editZone.SelectionStart + _editZone.SelectionLength))
            {
                return true;
            }

            return false;
        }
    }
}
