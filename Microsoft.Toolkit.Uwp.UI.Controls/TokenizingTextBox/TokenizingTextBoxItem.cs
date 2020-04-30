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
    public class TokenizingTextBoxItem : ListViewItem
    {
        private const string PART_ClearButton = "PART_ClearButton";

        private Button _clearButton;
        private ContentPresenter _contentPresenter;
        private bool IsClick = false;

        /// <summary>
        /// Event raised when the 'Clear' Button is clicked.
        /// </summary>
        public event TypedEventHandler<TokenizingTextBoxItem, RoutedEventArgs> ClearClicked;

        /// <summary>
        /// Event raised when the content is clicked.
        /// </summary>
        public event TypedEventHandler<TokenizingTextBoxItem, RoutedEventArgs> ContentClicked;

        /// <summary>
        /// Event raised when the delete key or a backspace is pressed.
        /// </summary>
        public event TypedEventHandler<TokenizingTextBoxItem, RoutedEventArgs> ClearAllAction;

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
            DefaultStyleKey = typeof(TokenizingTextBoxItem);

            RightTapped += TokenizingTextBoxItem_RightTapped;
            KeyDown += TokenizingTextBoxItem_KeyDown;
        }

        /// <inheritdoc/>
        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            if (_clearButton != null)
            {
                _clearButton.Click -= ClearButton_Click;
            }

            if (_contentPresenter != null)
            {
                _contentPresenter.PointerPressed -= ContentPresenter_PointerPressed;
                _contentPresenter.PointerReleased -= ContentPresenter_PointerReleased;
                _contentPresenter.PointerCanceled -= ContentPresenter_PointerCancel;
                _contentPresenter.PointerExited -= ContentPresenter_PointerCancel;
            }

            _clearButton = (Button)GetTemplateChild(PART_ClearButton);
            _contentPresenter = (ContentPresenter)GetTemplateChild("ContentPresenter");

            if (_clearButton != null)
            {
                _clearButton.Click += ClearButton_Click;
            }

            if (_contentPresenter != null)
            {
                _contentPresenter.PointerPressed += ContentPresenter_PointerPressed;
                _contentPresenter.PointerReleased += ContentPresenter_PointerReleased;
                _contentPresenter.PointerCanceled += ContentPresenter_PointerCancel;
                _contentPresenter.PointerExited += ContentPresenter_PointerCancel;
            }
        }

        private void ContentPresenter_PointerCancel(object sender, PointerRoutedEventArgs e)
        {
            IsClick = false;
        }

        private void ContentPresenter_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            IsClick = true;
        }

        private void ContentPresenter_PointerReleased(object sender, PointerRoutedEventArgs e)
        {
            if (IsClick)
            {
                ContentClicked?.Invoke(this, e);
            }

            IsClick = false;
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
    }
}
