// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.ObjectModel;
using Microsoft.Toolkit.Uwp.UI.Controls;
using Microsoft.Toolkit.Uwp.UI.Extensions;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Microsoft.Toolkit.Uwp.SampleApp.SamplePages
{
    public sealed partial class TabViewPage : Page, IXamlRenderListener
    {
        #pragma warning disable CS0618 // Type or member is obsolete
        private TabView _tabs;

        private int _counter = 1;

        public TabViewPage()
        {
            this.InitializeComponent();
        }

        public void OnXamlRendered(FrameworkElement control)
        {
            _tabs = control.FindChildByName("Tabs") as TabView;
            if (_tabs != null)
            {
                _tabs.TabDraggedOutside += Tabs_TabDraggedOutside;
                _tabs.TabClosing += Tabs_TabClosing;
            }

            var btn = control.FindDescendantByName("AddTabButtonUpper") as Button;
            if (btn != null)
            {
                btn.Click += AddUpperTabClick;
            }
        }

        private void AddUpperTabClick(object sender, RoutedEventArgs e)
        {
            _tabs.Items.Add(new TabViewItem()
            {
                Header = "Untitled " + _counter,
                Icon = new SymbolIcon(Symbol.Document),
                Content = "This is new tab #" + _counter++ + "."
            });
        }

        private void Tabs_TabClosing(object sender, TabClosingEventArgs e)
        {
            TabViewNotification.Show("You're closing the '" + e.Tab.Header + "' tab.", 2000);
        }

        private void Tabs_TabDraggedOutside(object sender, TabDraggedOutsideEventArgs e)
        {
            // The sample app let's you drag items from a static TabView with TabViewItem's pre-defined.
            // In the case of databound scenarios e.Item should be your data item, and e.Tab should always be the TabViewItem.
            var str = e.Item.ToString();

            if (e.Tab != null)
            {
                str = e.Tab.Header.ToString();
            }

            TabViewNotification.Show("Tore Tab '" + str + "' Outside of TabView.", 2000);
        }
        #pragma warning restore CS0618 // Type or member is obsolete
    }
}
