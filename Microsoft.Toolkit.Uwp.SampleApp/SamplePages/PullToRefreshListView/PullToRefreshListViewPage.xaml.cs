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
using Windows.UI.Xaml.Navigation;

namespace Microsoft.Toolkit.Uwp.SampleApp.SamplePages
{
    public sealed partial class PullToRefreshListViewPage
    {
        private readonly ObservableCollection<Item> _items;
        private DelegateCommand _refreshIntentCanceledCommand;

        public PullToRefreshListViewPage()
        {
            InitializeComponent();
            _items = new ObservableCollection<Item>();
            AddItems();
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

        private void ListView_RefreshIntentCanceled(object sender, EventArgs e)
        {

        }

        private DelegateCommand RefreshIntentCanceled => _refreshIntentCanceledCommand ?? (_refreshIntentCanceledCommand = new DelegateCommand(
            () => { }
        ));
    }
}
