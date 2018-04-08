using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Microsoft.Toolkit.Uwp.UI.Controls.TextToolbarButtons;
using Microsoft.Toolkit.Uwp.UI.Controls.TextToolbarButtons.Common;
using Microsoft.Toolkit.Uwp.UI.Controls.TextToolbarFormats;
using Microsoft.Toolkit.Uwp.UI.Controls.TextToolbarFormats.MarkDown;
using Microsoft.Toolkit.Uwp.UI.Controls.TextToolbarFormats.RichText;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    public class InfiniteCanvasTextBox : Control
    {
        public event EventHandler<string> TextChanged;

        public InfiniteCanvasTextBox()
        {
            this.DefaultStyleKey = typeof(InfiniteCanvasTextBox);
        }

        private TextBox _editZone;

        protected override void OnApplyTemplate()
        {
            _editZone = (TextBox)GetTemplateChild("EditZone");
            _editZone.TextChanged += _editZone_TextChanged;
            _editZone.FontSize = FontSize;
            base.OnApplyTemplate();
        }

        private void _editZone_TextChanged(object sender, RoutedEventArgs e)
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

            _editZone.TextChanged -= _editZone_TextChanged;
            _editZone.Text = string.Empty;
            _editZone.TextChanged += _editZone_TextChanged;
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
