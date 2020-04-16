// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Windows.Foundation;
using Windows.System;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Input;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    /// <summary>
    /// A control that manages as the item logic for the <see cref="TokenizingTextBox"/> control.
    /// </summary>
    [TemplatePart(Name = PART_ClearButton, Type = typeof(ButtonBase))]
    [TemplatePart(Name = PART_TextBox, Type = typeof(TextBox))]
    public class TokenizingTextBoxItem : ListViewItem
    {
        private const string PART_ClearButton = "PART_ClearButton";
        private const string PART_TextBox = "PART_TextBox";

        private Button _clearButton;
        private TextBox _dummyText;

        /// <summary>
        /// Event raised when the 'Clear' Button is clicked.
        /// </summary>
        public event TypedEventHandler<TokenizingTextBoxItem, RoutedEventArgs> ClearClicked;

        /// <summary>
        /// Event raised when the delete key or a backspace is pressed.
        /// </summary>
        public event TypedEventHandler<TokenizingTextBoxItem, RoutedEventArgs> ClearAllAction;

        /// <summary>
        /// Event raised when a keypress happens on the item.
        /// </summary>
        public event TypedEventHandler<TokenizingTextBoxItem, RoutedEventArgs> KeyPressAction;

        /// <summary>
        /// Identifies the <see cref="ClearButtonStyle"/> property.
        /// </summary>
        public static readonly DependencyProperty ClearButtonStyleProperty = DependencyProperty.Register(
            nameof(ClearButtonStyle),
            typeof(Style),
            typeof(TokenizingTextBoxItem),
            new PropertyMetadata(Visibility.Collapsed));

        /// <summary>
        /// Gets or sets the Style for the 'Clear' Button
        /// </summary>
        public Style ClearButtonStyle
        {
            get => (Style)GetValue(ClearButtonStyleProperty);
            set => SetValue(ClearButtonStyleProperty, value);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TokenizingTextBoxItem"/> class.
        /// </summary>
        public TokenizingTextBoxItem()
        {
            // TODO: Check if the ListView ItemClick event works still...
            DefaultStyleKey = typeof(TokenizingTextBoxItem);

            RightTapped += TokenizingTextBoxItem_RightTapped;
            PreviewKeyDown += this.TokenizingTextBoxItem_PreviewKeyDown;
            KeyDown += TokenizingTextBoxItem_KeyDown;
        }

        private void TokenizingTextBoxItem_PreviewKeyDown(object sender, KeyRoutedEventArgs e)
        {
            // check if this is a key stroke that would cause input to a text box
            // If CTRL or ALT modifier are applied then no-op
            bool processAsInput =
                !(CoreWindow.GetForCurrentThread().GetKeyState(VirtualKey.Control).HasFlag(CoreVirtualKeyStates.Down) ||
                CoreWindow.GetForCurrentThread().GetKeyState(VirtualKey.Application).HasFlag(CoreVirtualKeyStates.Down) ||
                CoreWindow.GetForCurrentThread().GetKeyState(VirtualKey.Menu).HasFlag(CoreVirtualKeyStates.Down));

            if (processAsInput)
            {
                int code = (int)e.Key;

                // TODO: verify this list is complete - need to call something like ToAscii() to confirm if the key is a printable character.
                if (e.Key == VirtualKey.Space ||
                    (e.Key >= VirtualKey.Number0 && e.Key <= VirtualKey.Z) ||
                    (e.Key >= VirtualKey.NumberPad0 && e.Key <= VirtualKey.Divide) ||
                    (code >= 0xBA && code <= 0xF5))
                {
                    ////e.Handled = true;
                    ////KeyPressAction?.Invoke(this, e);
                }
            }
        }

        /// <inheritdoc/>
        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            if (_clearButton != null)
            {
                _clearButton.Click -= ClearButton_Click;
            }

            _clearButton = GetTemplateChild(PART_ClearButton) as Button;
            _dummyText = GetTemplateChild(PART_TextBox) as TextBox;

            if (_clearButton != null)
            {
                _clearButton.Click += ClearButton_Click;
            }

            _dummyText.KeyDown += this._dummyText_KeyDown;
            _dummyText.TextChanged += this._dummyText_TextChanged;
        }

        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            ClearClicked?.Invoke(this, e);
        }

        private void TokenizingTextBoxItem_RightTapped(object sender, RightTappedRoutedEventArgs e)
        {
            ContextFlyout.ShowAt(this);
        }

        private void TokenizingTextBoxItem_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            switch (e.Key)
            {
                case VirtualKey.Back:
                case VirtualKey.Delete:
                {
                    ClearAllAction?.Invoke(this, e);
                    break;
                }
            }
        }

        protected override void OnGotFocus(RoutedEventArgs e)
        {
            base.OnGotFocus(e);

            _dummyText.Focus(FocusState.Programmatic);
        }

        private void _dummyText_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            
        }

        private void _dummyText_TextChanged(object sender, TextChangedEventArgs e)
        {
            
        }
    }
}
