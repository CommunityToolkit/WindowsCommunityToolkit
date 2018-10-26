// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;

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

        /// <summary>
        /// Initializes a new instance of the <see cref="TabViewItem"/> class.
        /// </summary>
        public TabViewItem()
        {
            DefaultStyleKey = typeof(TabViewItem);
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
            Closing?.Invoke(this, new TabClosingEventArgs(Content, this));
        }
    }
}
