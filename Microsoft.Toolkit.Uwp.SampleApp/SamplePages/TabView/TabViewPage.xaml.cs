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
        public ObservableCollection<DataItem> TabItemCollection { get; } = new ObservableCollection<DataItem>();

        private TabView _tabs;
        private TabView _tabItems;

        private int _counter = 1;

        public TabViewPage()
        {
            this.InitializeComponent();

            TabItemCollection.Add(new DataItem() { MyText = "First Tab", Value = 100 });
            TabItemCollection.Add(new DataItem() { MyText = "Tab 2", Value = 200 });
            TabItemCollection.Add(new DataItem() { MyText = "Third Tab", Value = 300 });
            TabItemCollection.Add(new DataItem() { MyText = "Tab Plus", Value = 400 });
        }

        public void OnXamlRendered(FrameworkElement control)
        {
            control.DataContext = this;

            _tabs = control.FindChildByName("Tabs") as TabView;
            if (_tabs != null)
            {
                _tabs.TabDraggedOutside += Tabs_TabDraggedOutside;
                _tabs.TabClosing += Tabs_TabClosing;
            }

            _tabItems = control.FindChildByName("TabItems") as TabView;
            if (_tabItems != null)
            {
                _tabItems.ItemClick += TabItems_ItemClick;
            }

            var btn = control.FindDescendantByName("AddTabButtonUpper") as Button;
            if (btn != null)
            {
                btn.Click += AddUpperTabClick;
            }

            btn = control.FindDescendantByName("AddTabButtonLower") as Button;
            if (btn != null)
            {
                btn.Click += AddTabButtonClicked_Lower;
            }
        }

        private async void TabItems_ItemClick(object sender, ItemClickEventArgs e)
        {
            await new MessageDialog("You clicked the '" + e.ClickedItem + "' tab.").ShowAsync();
        }

        private void AddUpperTabClick(object sender, RoutedEventArgs e)
        {
            _tabs.Items.Add(new TabViewItem()
            {
                Header = "Untitled " + _counter++,
                HeaderIcon = new SymbolIcon(Symbol.Document),
                Content = "This is a new tab."
            });
        }

        private void AddTabButtonClicked_Lower(object sender, RoutedEventArgs e)
        {
            TabItemCollection.Add(new DataItem() { MyText = "New Tab", Value = 500 });
        }

        private void Tabs_TabClosing(object sender, TabClosingEventArgs e)
        {
            if (e.Tab.Header.ToString() == "Not Closable")
            {
                e.Cancel = true;
            }

            #pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            new MessageDialog("You're closing the '" + e.Tab.Header + "' tab.").ShowAsync();
            #pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
        }

        private async void Tabs_TabDraggedOutside(object sender, TabDraggedOutsideEventArgs e)
        {
            await new MessageDialog("Tore Tab Outside App.  TODO: Pop-open a Window.").ShowAsync();
        }
    }

    #pragma warning disable SA1402 // File may only contain a single class
    public class DataItem
    #pragma warning restore SA1402 // File may only contain a single class
    {
        public string MyText { get; set; }

        public int Value { get; set; }

        public override bool Equals(object obj)
        {
            if (obj is DataItem di)
            {
                return MyText == di.MyText && Value == di.Value;
            }

            return false;
        }

        public override int GetHashCode()
        {
            return MyText.GetHashCode() ^ Value.GetHashCode();
        }
    }
}
