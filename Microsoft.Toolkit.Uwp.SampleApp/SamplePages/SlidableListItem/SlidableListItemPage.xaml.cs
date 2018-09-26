// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Microsoft.Toolkit.Uwp.SampleApp.Models;
using Microsoft.Toolkit.Uwp.UI.Extensions;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Microsoft.Toolkit.Uwp.SampleApp.SamplePages
{
    public sealed partial class SlidableListItemPage : IXamlRenderListener
    {
        public static ObservableCollection<Item> Items { get; set; }

        public SlidableListItemPage()
        {
            InitializeComponent();
            Load();
        }

        public void OnXamlRendered(FrameworkElement control)
        {
            var listView = control.FindChildByName("listView") as ListView;
            if (listView != null)
            {
                listView.ItemsSource = Items;
            }
        }

        private void Load()
        {
            // Reset items when revisiting sample.
            Items = new ObservableCollection<Item>();

            for (var i = 0; i < 1000; i++)
            {
                Items.Add(new Item() { Title = "Item " + i });
            }
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
