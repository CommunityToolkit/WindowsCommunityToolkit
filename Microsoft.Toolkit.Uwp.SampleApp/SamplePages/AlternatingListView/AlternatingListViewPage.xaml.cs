using System;
using System.Collections.ObjectModel;
using Windows.UI.Xaml.Controls;
using Microsoft.Toolkit.Uwp.SampleApp.Models;

namespace Microsoft.Toolkit.Uwp.SampleApp.SamplePages
{
    public sealed partial class AlternatingListViewPage : Page
    {
        public ObservableCollection<Item> Items { get; set; } = new ObservableCollection<Item>();

        public AlternatingListViewPage()
        {
            this.InitializeComponent();
            AddItems();
        }

        private void AddItems()
        {
            Items.Clear();
            for (int i = 1; i < 11; i++)
            {
                Items.Add(new Item { Title = $"Item {i}" });
            }
        }
    }
}
