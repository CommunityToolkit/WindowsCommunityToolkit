using Microsoft.Windows.Toolkit.SampleApp.Models;
using Microsoft.Windows.Toolkit.UI.Controls;
using System;
using System.Collections.ObjectModel;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Microsoft.Windows.Toolkit.SampleApp.SamplePages
{

    public sealed partial class SlidableListItemPage : Page
    {
        private ObservableCollection<Item> _items;

        public SlidableListItemPage()
        {
            this.InitializeComponent();
            ObservableCollection<Item> items = new ObservableCollection<Item>();

            for (var i = 0; i < 1000; i++)
            {
                items.Add(new Item() { Title = "Item " + i });

            }

            _items = items;
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

        private void SlidableListItem_RightCommandActivated(object sender, EventArgs e)
        {
            _items.Remove((sender as SlidableListItem).DataContext as Item);
        }
    }
}
