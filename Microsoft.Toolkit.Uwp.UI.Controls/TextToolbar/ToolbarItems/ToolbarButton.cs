﻿// ******************************************************************
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

using System;
using System.ComponentModel;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Automation;
using Windows.UI.Xaml.Controls;

namespace Microsoft.Toolkit.Uwp.UI.Controls.TextToolbarButtons
{
    /// <summary>
    /// An Interactive button in the TextToolbar, to perform a formatting task.
    /// </summary>
    public class ToolbarButton : AppBarButton, IToolbarItem, INotifyPropertyChanged
    {
        /// <summary>
        /// Identifies the <see cref="ToolTip"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ToolTipProperty =
            DependencyProperty.Register(nameof(ToolTip), typeof(string), typeof(ToolbarButton), new PropertyMetadata(null, ToolTipChanged));

        /// <summary>
        /// Identifies the <see cref="ShortcutKey"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ShortcutKeyProperty =
            DependencyProperty.Register(nameof(ShortcutKey), typeof(VirtualKey?), typeof(ToolbarButton), new PropertyMetadata(null));

        /// <summary>
        /// Identifies the <see cref="IsToggled"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty IsToggledProperty =
            DependencyProperty.Register(nameof(Toggled), typeof(Visibility), typeof(ToolbarButton), new PropertyMetadata(Visibility.Collapsed));

        /// <summary>
        /// Identifies the <see cref="ShortcutFancyName"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ShortcutFancyNameProperty =
            DependencyProperty.Register(nameof(ShortcutFancyName), typeof(string), typeof(ToolbarButton), new PropertyMetadata(null));

        /// <summary>
        /// Initializes a new instance of the <see cref="ToolbarButton"/> class.
        /// </summary>
        public ToolbarButton()
        {
            this.DefaultStyleKey = typeof(ToolbarButton);
            Click += ToolbarButton_Click;
        }

        /// <inheritdoc/>
        protected override void OnApplyTemplate()
        {
            UpdateTooltip();
            base.OnApplyTemplate();
        }

        private static void ToolTipChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            if (obj is ToolbarButton button)
            {
                button.UpdateTooltip();
            }
        }

        private void UpdateTooltip()
        {
            string tooltip = ToolTip;
            if (ShortcutKey.HasValue)
            {
                tooltip += $" (Ctrl + {ShortcutFancyName ?? ShortcutKey.Value.ToString()})";
            }

            if (!string.IsNullOrWhiteSpace(ToolTip))
            {
                ToolTipService.SetToolTip(this, tooltip);
                AutomationProperties.SetName(this, ToolTip);
            }
        }

        private void ToolbarButton_Click(object sender, RoutedEventArgs e)
        {
            Activate(Model.ShiftKeyDown);
        }

        /// <summary>
        /// Determines if this is the Correct shortcut, if not it continues along the bar to find a matching shortcut.
        /// </summary>
        /// <param name="args">Shortcut Request Args</param>
        internal void ShortcutRequested(ref ShortcutKeyRequestArgs args)
        {
            if (args.Key == ShortcutKey)
            {
                Activate(args.ShiftKeyHeld);
                args.Handled = true;
            }
        }

        private void Activate(bool isShift)
        {
            if (Model.Editor == null)
            {
                return;
            }

            if (isShift && ShiftActivation != null)
            {
                ShiftActivation(this);
            }
            else
            {
                Activation?.Invoke(this);
            }
        }

        /// <summary>
        /// Gets or sets the designated formatting task.
        /// </summary>
        public Action<ToolbarButton> Activation { get; set; }

        /// <summary>
        /// Gets or sets the designated formatting task when pressing shift at the same time.
        /// </summary>
        public Action<ToolbarButton> ShiftActivation { get; set; }

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

        /// <summary>
        /// Gets or sets the name that represents the <see cref="ShortcutKey"/> as the Keyboard Character
        /// </summary>
        public string ShortcutFancyName
        {
            get { return (string)GetValue(ShortcutFancyNameProperty); }
            set { SetValue(ShortcutFancyNameProperty, value); }
        }

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

        /// <summary>
        /// Gets or sets a value indicating whether the <see cref="ToolbarButton"/> is Toggled
        /// </summary>
        public bool IsToggled
        {
            get
            {
                return Toggled == Visibility.Visible;
            }

            set
            {
                if (value)
                {
                    Toggled = Visibility.Visible;
                }
                else
                {
                    Toggled = Visibility.Collapsed;
                }
            }
        }

        private Visibility Toggled
        {
            get { return (Visibility)GetValue(IsToggledProperty); }
            set { SetValue(IsToggledProperty, value); }
        }

        /// <summary>
        /// Gets or sets the Attached TextToolbar
        /// </summary>
        internal TextToolbar Model
        {
            get { return _model; }
            set { _model = value; }
        }

        private TextToolbar _model;

        private int _position = -1;

        /// <inheritdoc/>
        public event PropertyChangedEventHandler PropertyChanged;
    }
}