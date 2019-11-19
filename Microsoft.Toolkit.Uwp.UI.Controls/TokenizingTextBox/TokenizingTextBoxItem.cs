// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    /// <summary>
    /// A control that manages as the item logic for the <see cref="TokenizingTextBox"/> control.
    /// </summary>
    [TemplatePart(Name = PART_ClearButton, Type = typeof(Button))]
    public class TokenizingTextBoxItem : Button
    {
        private const string PART_ClearButton = "PART_ClearButton";

        private Button _clearButton;

        /// <summary>
        /// Event raised when the 'Clear' Button is clicked.
        /// </summary>
        public event TypedEventHandler<TokenizingTextBoxItem, RoutedEventArgs> ClearClicked;

        /// <summary>
        /// Identifies the <see cref="IsSelected"/> property.
        /// </summary>
        public static readonly DependencyProperty IsSelectedProperty = DependencyProperty.Register(
            nameof(IsSelected),
            typeof(bool),
            typeof(TokenizingTextBoxItem),
            new PropertyMetadata(false, new PropertyChangedCallback(TokenizingTextBoxItem_IsSelectedChanged)));

        /// <summary>
        /// Identifies the <see cref="ClearButtonStyle"/> property.
        /// </summary>
        public static readonly DependencyProperty ClearButtonStyleProperty = DependencyProperty.Register(
            nameof(ClearButtonStyle),
            typeof(Style),
            typeof(TokenizingTextBoxItem),
            new PropertyMetadata(Visibility.Collapsed));

        /// <summary>
        /// Gets or sets a value indicating whether this item is currently in a selected state.
        /// </summary>
        public bool IsSelected
        {
            get => (bool)GetValue(IsSelectedProperty);
            set => SetValue(IsSelectedProperty, value);
        }

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

            var pointerEventHandler = new PointerEventHandler((s, e) => UpdateVisualState());
            var dependencyPropertyChangedEventHandler = new DependencyPropertyChangedEventHandler((d, e) => UpdateVisualState());

            PointerEntered += pointerEventHandler;
            PointerExited += pointerEventHandler;
            PointerCanceled += pointerEventHandler;
            PointerPressed += pointerEventHandler;
            PointerReleased += pointerEventHandler;
            IsEnabledChanged += dependencyPropertyChangedEventHandler;
            RightTapped += TokenizingTextBoxItem_RightTapped;
            KeyDown += TokenizingTextBoxItem_KeyDown;
        }

        private static void TokenizingTextBoxItem_IsSelectedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is TokenizingTextBoxItem item)
            {
                if (item.IsSelected)
                {
                    VisualStateManager.GoToState(item, "Selected", true);
                }
                else
                {
                    VisualStateManager.GoToState(item, "Unselected", true);
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

            _clearButton = (Button)GetTemplateChild(PART_ClearButton);

            if (_clearButton != null)
            {
                _clearButton.Click += ClearButton_Click;
            }
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
                case Windows.System.VirtualKey.Back:
                case Windows.System.VirtualKey.Delete:
                    ClearButton_Click(sender, e);
                    break;
            }
        }

        private void UpdateVisualState(bool useTransitions = true)
        {
            if (!IsEnabled)
            {
                VisualStateManager.GoToState(this, "Disabled", useTransitions);
            }
            else if (IsPressed)
            {
                VisualStateManager.GoToState(this, "Pressed", useTransitions);
            }
            else if (IsPointerOver)
            {
                VisualStateManager.GoToState(this, "PointerOver", useTransitions);
            }
            else
            {
                VisualStateManager.GoToState(this, "Normal", useTransitions);
            }
        }
    }
}
