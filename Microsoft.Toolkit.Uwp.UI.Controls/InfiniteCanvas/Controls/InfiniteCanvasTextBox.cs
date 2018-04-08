using System;
using System.Linq;
using Windows.UI.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    internal class InfiniteCanvasTextBox : Control
    {
        public event EventHandler<string> TextChanged;

        public InfiniteCanvasTextBox()
        {
            DefaultStyleKey = typeof(InfiniteCanvasTextBox);
        }

        private TextBox _editZone;

        protected override void OnApplyTemplate()
        {
            _editZone = (TextBox)GetTemplateChild("EditZone");
            _editZone.TextChanged += EditZone_TextChanged;
            _editZone.FontSize = FontSize;
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
