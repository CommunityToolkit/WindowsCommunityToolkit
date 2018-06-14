// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.ObjectModel;
using Microsoft.Toolkit.Uwp.SampleApp.Common;
using Microsoft.Toolkit.Uwp.SampleApp.Models;
using Microsoft.Toolkit.Uwp.UI.Controls;
using Microsoft.Toolkit.Uwp.UI.Extensions;
using Windows.UI.Xaml;

namespace Microsoft.Toolkit.Uwp.SampleApp.SamplePages
{
    public sealed partial class PullToRefreshListViewPage : IXamlRenderListener
    {
#pragma warning disable CS0618
        private readonly ObservableCollection<Item> _items;

        public PullToRefreshListViewPage()
        {
            InitializeComponent();
            _items = new ObservableCollection<Item>();
            AddItems();
        }

        public void OnXamlRendered(FrameworkElement control)
        {
            var listView = control.FindChildByName("ListView") as PullToRefreshListView;
            listView.ItemsSource = _items;
            listView.RefreshRequested += ListView_RefreshCommand;
        }

        private void AddItems()
        {
            for (int i = 0; i < 10; i++)
            {
                _items.Insert(0, new Item { Title = "Item " + _items.Count });
            }
        }

        private void ListView_RefreshCommand(object sender, EventArgs e)
        {
            AddItems();
        }
#pragma warning restore CS0618
    }
}
