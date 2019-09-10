// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Windows.Devices.Input;
using Windows.System;
using Windows.UI.Core;
using Windows.UI.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Input;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    /// <summary>
    /// Item Container for a <see cref="TabView"/>.
    /// </summary>
    [TemplatePart(Name = TabCloseButtonName, Type = typeof(ButtonBase))]
    public partial class TabViewItem : ListViewItem
    {
        private const string TabCloseButtonName = "CloseButton";

        private ButtonBase _tabCloseButton;

        private bool _isMiddleClick;

        /// <summary>
        /// Initializes a new instance of the <see cref="TabViewItem"/> class.
        /// </summary>
        public TabViewItem()
        {
            DefaultStyleKey = typeof(TabViewItem);
        }

        /// <summary>
        /// Fired when the Tab's close button is clicked.
        /// </summary>
        public event EventHandler<TabClosingEventArgs> Closing;

        /// <inheritdoc/>
        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            if (_tabCloseButton != null)
            {
                _tabCloseButton.Click -= TabCloseButton_Click;
            }

            _tabCloseButton = GetTemplateChild(TabCloseButtonName) as ButtonBase;

            if (_tabCloseButton != null)
            {
                _tabCloseButton.Click += TabCloseButton_Click;
            }
        }

        /// <inheritdoc/>
        protected override void OnPointerPressed(PointerRoutedEventArgs e)
        {
            _isMiddleClick = false;

            if (e.Pointer.PointerDeviceType == PointerDeviceType.Mouse)
            {
                PointerPoint pointerPoint = e.GetCurrentPoint(this);

                // Record if middle button is pressed
                if (pointerPoint.Properties.IsMiddleButtonPressed)
                {
                    _isMiddleClick = true;
                }

                // Disable unwanted behaviour inherited by ListViewItem:
                // Disable "Ctrl + Left click" to deselect tab
                // Or variant like "Ctrl + Shift + Left click"
                // Or "Ctrl + Alt + Left click"
                if (pointerPoint.Properties.IsLeftButtonPressed)
                {
                    var ctrl = Window.Current.CoreWindow.GetKeyState(VirtualKey.Control);
                    if (ctrl.HasFlag(CoreVirtualKeyStates.Down))
                    {
                        // return here so the event won't be picked up by the base class
                        // but keep this event unhandled so it can be picked up further
                        return;
                    }
                }
            }

            base.OnPointerPressed(e);
        }

        /// <inheritdoc/>
        protected override void OnPointerReleased(PointerRoutedEventArgs e)
        {
            base.OnPointerReleased(e);

            // Close on Middle-Click
            if (_isMiddleClick)
            {
                TabCloseButton_Click(this, null);
            }

            _isMiddleClick = false;
        }

        private void TabCloseButton_Click(object sender, RoutedEventArgs e)
        {
            if (IsClosable)
            {
                Closing?.Invoke(this, new TabClosingEventArgs(Content, this));
            }
        }
    }
}
