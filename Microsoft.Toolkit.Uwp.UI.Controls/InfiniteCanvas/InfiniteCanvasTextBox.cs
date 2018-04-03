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
    public class InfiniteCanvasToolbarFormatter : RichTextFormatter
    {
        public InfiniteCanvasToolbarFormatter(TextToolbar model)
            : base(model)
        {
            CommonButtons = new CommonButtons(model);
            ButtonActions = new RichTextButtonActions(this);
        }

        public override ButtonMap DefaultButtons
        {
            get
            {
                var bold = CommonButtons.Bold;
                var italic = CommonButtons.Italics;
                return new ButtonMap
                {
                    bold,
                    italic
                };
            }
        }

        private CommonButtons CommonButtons { get; }
    }

    public class InfiniteCanvasTextBox : Control
    {
        public InfiniteCanvasTextBox()
        {
            this.DefaultStyleKey = typeof(InfiniteCanvasTextBox);
        }

        private float currentFontSize = 12;
        private TextToolbar _textToolbar;
        private RichEditBox _editZone;

        protected override void OnApplyTemplate()
        {
            _textToolbar = (TextToolbar)GetTemplateChild("TextToolbar");
            _editZone = (RichEditBox)GetTemplateChild("EditZone");

            _editZone.TextChanged += _editZone_TextChanged;

            _textToolbar.Loaded += _textToolbar_Loaded;
            base.OnApplyTemplate();
        }

        private InfiniteCanvasToolbarFormatter formatter;
        private void _textToolbar_Loaded(object sender, RoutedEventArgs e)
        {
            formatter = new InfiniteCanvasToolbarFormatter(_textToolbar);
            _textToolbar.Formatter = formatter;

            var fontIncrease = new ToolbarButton
            {
                Icon = new SymbolIcon { Symbol = Symbol.FontIncrease },
                ToolTip = "Font Increase",
                Activation = (b) =>
                {
                    _textToolbar.Formatter.Selected.CharacterFormat.Size++;
                }
            };

            var fontdecrease = new ToolbarButton
            {
                Icon = new SymbolIcon { Symbol = Symbol.FontDecrease },
                ToolTip = "Font Decrease",
                Activation = (b) =>
                {
                    _textToolbar.Formatter.Selected.CharacterFormat.Size--;
                }
            };

            _textToolbar.CustomButtons.Add(fontIncrease);
            _textToolbar.CustomButtons.Add(fontdecrease);
        }

        public event EventHandler<string> TextChanged;

        private void _editZone_TextChanged(object sender, RoutedEventArgs e)
        {
            string value = string.Empty;

            _editZone.Document.GetText(Windows.UI.Text.TextGetOptions.FormatRtf, out value);

            TextChanged?.Invoke(this, value);
        }
    }
}
