using System;
using System.Collections.ObjectModel;
using Microsoft.Windows.Toolkit.SampleApp.Models;
using Microsoft.Windows.Toolkit.UI.Controls;
using Windows.UI;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace Microsoft.Windows.Toolkit.SampleApp.SamplePages
{
    public sealed partial class PullToRefreshListViewPage
    {
        private readonly ObservableCollection<Item> _items;

        public PullToRefreshListViewPage()
        {
            InitializeComponent();
            _items = new ObservableCollection<Item>();
            AddItems();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            var propertyDesc = e.Parameter as PropertyDescriptor;

            if (propertyDesc != null)
            {
                DataContext = propertyDesc.Expando;
            }
        }

        private void AddItems()
        {
            for (int i = 0; i < 10; i++)
            {
                _items.Insert(0, new Item { Title = "Item " + new Random().Next(10000) });
            }
        }

        private void ListView_RefreshCommand(object sender, EventArgs e)
        {
            AddItems();
        }

        private void ListView_PullProgressChanged(object sender, RefreshProgressEventArgs e)
        {
            refreshindicator.Opacity = e.PullProgress;

            refreshindicator.Background = e.PullProgress < 1.0 ? new SolidColorBrush(Colors.Red) : new SolidColorBrush(Colors.Blue);
        }
    }
}
