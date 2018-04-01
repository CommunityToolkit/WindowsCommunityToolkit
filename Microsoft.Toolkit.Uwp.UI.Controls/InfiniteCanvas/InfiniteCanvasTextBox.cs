using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Microsoft.Toolkit.Uwp.UI.Controls.TextToolbarButtons;
using Microsoft.Toolkit.Uwp.UI.Controls.TextToolbarFormats.MarkDown;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    public class InfiniteCanvasTextBox : Control
    {
        public InfiniteCanvasTextBox()
        {
            this.DefaultStyleKey = typeof(InfiniteCanvasTextBox);
        }

        private float currentFontSize = 12;
        private TextToolbar _textToolbar;

        protected override void OnApplyTemplate()
        {
            _textToolbar = (TextToolbar)GetTemplateChild("TextToolbar");

            var fontIncrease = new ToolbarButton
            {
                Icon = new SymbolIcon { Symbol = Symbol.FontIncrease },
                ToolTip = "Font Increase",
                Activation = (b) =>
                {
                    _textToolbar.Formatter.Selected.CharacterFormat.Size = ++currentFontSize;
                }
            };

            var fontdecrease = new ToolbarButton
            {
                Icon = new SymbolIcon { Symbol = Symbol.FontDecrease },
                ToolTip = "Font Decrease",
                Activation = (b) =>
                {
                    _textToolbar.Formatter.Selected.CharacterFormat.Size = --currentFontSize;
                }
            };

            _textToolbar.CustomButtons.Add(fontIncrease);
            _textToolbar.CustomButtons.Add(fontdecrease);


            _textToolbar.Loaded += _textToolbar_Loaded;

            base.OnApplyTemplate();
        }

        private void _textToolbar_Loaded(object sender, RoutedEventArgs e)
        {
            foreach (var item in _textToolbar.DefaultButtons)
            {
                if (item is ToolbarButton button)
                {
                    button.Visibility = Visibility.Collapsed;
                }
            }

            var bold = _textToolbar?.GetDefaultButton(ButtonType.Bold);
            if (bold != null)
            {
                bold.Visibility = Visibility.Visible;
            }

            var italic = _textToolbar?.GetDefaultButton(ButtonType.Italics);
            if (italic != null)
            {
                italic.Visibility = Visibility.Visible;
            }
        }
    }
}
