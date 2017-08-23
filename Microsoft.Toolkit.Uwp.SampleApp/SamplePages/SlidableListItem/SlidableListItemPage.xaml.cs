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

using System.Collections.ObjectModel;
using Microsoft.Toolkit.Uwp.SampleApp.Common;
using Microsoft.Toolkit.Uwp.SampleApp.Models;
using Microsoft.Toolkit.Uwp.UI.Extensions;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using System.Windows.Input;
using System;

namespace Microsoft.Toolkit.Uwp.SampleApp.SamplePages
{
    public sealed partial class SlidableListItemPage : IXamlRenderListener
    {
        public static ObservableCollection<Item> Items { get; set; }

        public SlidableListItemPage()
        {
            InitializeComponent();
            if (Items == null)
            {
                Items = new ObservableCollection<Item>();

                for (var i = 0; i < 1000; i++)
                {
                    Items.Add(new Item() { Title = "Item " + i });
                }
            }
        }

        public void OnXamlRendered(FrameworkElement control)
        {
            var page = control.FindChildByName("page") as Page;
            if (page != null)
            {
                page.DataContext = this;
            }

            var listView = control.FindChildByName("listView") as ListView;
            if (listView != null)
            {
                listView.ItemsSource = Items;
            }
        }

        private bool CanExecuteDeleteItemCommand(Item item)
        {
            return true;
        }
    }

#pragma warning disable SA1402 // File may only contain a single class
    internal class RemoveItemCommand : ICommand
#pragma warning restore SA1402 // File may only contain a single class
    {
        event EventHandler ICommand.CanExecuteChanged
        {
            add
            {
            }

            remove
            {
            }
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            SlidableListItemPage.Items?.Remove(parameter as Item);
        }
    }
}
