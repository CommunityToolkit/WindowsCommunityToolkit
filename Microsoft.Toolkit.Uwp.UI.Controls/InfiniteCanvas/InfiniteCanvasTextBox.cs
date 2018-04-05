using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            _editZone.PointerWheelChanged += _editZone_PointerWheelChanged;
            base.OnApplyTemplate();
        }

        private void _editZone_PointerWheelChanged(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            this.Visibility = Visibility.Collapsed;
        }

        public event EventHandler<string> TextChanged;

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
    }
}
