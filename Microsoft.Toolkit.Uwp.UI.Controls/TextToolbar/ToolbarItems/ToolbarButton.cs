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

namespace Microsoft.Toolkit.Uwp.UI.Controls.TextToolbarButtons
{
    using System;
    using System.ComponentModel;
    using Windows.System;
    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Controls;

    /// <summary>
    /// An Interactive button in the TextToolbar, to perform a formatting task.
    /// </summary>
    public sealed class ToolbarButton : AppBarButton, IToolbarItem, INotifyPropertyChanged
    {
        // Using a DependencyProperty as the backing store for Label.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ToolTipProperty =
            DependencyProperty.Register(nameof(ToolTip), typeof(string), typeof(ToolbarButton), new PropertyMetadata(null, ToolTipChanged));

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ShortcutKeyProperty =
            DependencyProperty.Register(nameof(ShortcutKey), typeof(VirtualKey?), typeof(ToolbarButton), new PropertyMetadata(null));

        // Using a DependencyProperty as the backing store for ShortcutKeySymbol.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ShortcutKeySymbolProperty =
            DependencyProperty.Register(nameof(ShortcutKeySymbol), typeof(string), typeof(ToolbarButton), new PropertyMetadata(null));

        public event PropertyChangedEventHandler PropertyChanged;

        public ToolbarButton()
        {
            this.DefaultStyleKey = typeof(ToolbarButton);
            base.Click += ToolbarButton_Click;
        }

        private void ToolbarButton_Click(object sender, RoutedEventArgs e)
        {
            if (Model.ShiftKeyDown && ShiftActivation != null)
            {
                ShiftActivation();
            }
            else
            {
                Click();
            }
        }

        /// <summary>
        /// Gets or sets the designated formatting task.
        /// </summary>
        public new Action Click { get; set; }

        /// <summary>
        /// Gets or sets the designated formatting task when pressing shift at the same time.
        /// </summary>
        public Action ShiftActivation { get; set; }

        /// <summary>
        /// Gets or sets the Tooltip message, explaining what the button does.
        /// </summary>
        public string ToolTip
        {
            get { return (string)GetValue(ToolTipProperty); }
            set { SetValue(ToolTipProperty, value); }
        }

        /// <summary>
        /// Gets or sets a key to activate this button from the keyboard.
        /// </summary>
        public VirtualKey? ShortcutKey
        {
            get { return (VirtualKey?)GetValue(ShortcutKeyProperty); }
            set { SetValue(ShortcutKeyProperty, value); }
        }

        private int _position = -1;

        /// <summary>
        /// Gets or sets the position in the Toolbar to place this Button.
        /// </summary>
        public int Position
        {
            get
            {
                return _position;
            }

            set
            {
                _position = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Position)));
            }
        }

        private static void ToolTipChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            if (obj is ToolbarButton button)
            {
                button.UpdateTooltip();
            }
        }

        protected override void OnApplyTemplate()
        {
            UpdateTooltip();
            base.OnApplyTemplate();
        }

        private void UpdateTooltip()
        {
            string tooltip = ToolTip;
            if (ShortcutKey.HasValue)
            {
                tooltip += $" (Ctrl + {ShortcutKey.Value.ToString()})";
            }

            ToolTipService.SetToolTip(this, tooltip);
        }

        /// <summary>
        /// Gets or sets the Attached TextToolbar
        /// </summary>
        internal TextToolbar Model { get; set; }

        /// <summary>
        /// Determines if this is the Correct shortcut, if not it continues along the bar to find a matching shortcut.
        /// </summary>
        /// <param name="args">Shortcut Request Args</param>
        internal void ShortcutRequested(ref ShortcutKeyRequestArgs args)
        {
            if (args.Key == ShortcutKey)
            {
                if (args.ShiftKeyHeld && ShiftActivation != null)
                {
                    ShiftActivation();
                }
                else
                {
                    Click();
                }

                args.Handled = true;
            }
        }
    }
}