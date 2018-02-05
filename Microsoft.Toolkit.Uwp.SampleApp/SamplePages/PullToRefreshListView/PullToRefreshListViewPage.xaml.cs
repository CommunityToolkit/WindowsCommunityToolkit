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
    }
}
