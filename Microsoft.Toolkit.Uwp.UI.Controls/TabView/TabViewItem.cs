// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Windows.Devices.Input;
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

        /// <summary>
        /// Fired when the Tab's close button is clicked.
        /// </summary>
        public event EventHandler<TabClosingEventArgs> Closing;

        private bool isMiddleClick;

        /// <summary>
        /// Initializes a new instance of the <see cref="TabViewItem"/> class.
        /// </summary>
        public TabViewItem()
        {
            DefaultStyleKey = typeof(TabViewItem);

            PointerPressed += TabViewItem_PointerPressed;
            PointerReleased += TabViewItem_PointerReleased;
        }

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

        private void TabCloseButton_Click(object sender, RoutedEventArgs e)
        {
            if (IsClosable)
            {
                Closing?.Invoke(this, new TabClosingEventArgs(Content, this));
            }
        }

        private void TabViewItem_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            isMiddleClick = false;

            if (e.Pointer.PointerDeviceType == PointerDeviceType.Mouse)
            {
                PointerPoint pointerPoint = e.GetCurrentPoint(this);

                // Record if middle button is pressed
                if (pointerPoint.Properties.IsMiddleButtonPressed)
                {
                    isMiddleClick = true;
                }
            }
        }

        private void TabViewItem_PointerReleased(object sender, PointerRoutedEventArgs e)
        {
            // Close on Middle-Click
            if (isMiddleClick)
            {
                TabCloseButton_Click(this, null);
            }

            isMiddleClick = false;
        }
    }
}
