using System.Collections.Generic;
using System.Linq;
using Microsoft.Toolkit.Uwp.SampleApp.Models;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Microsoft.Toolkit.Uwp.SampleApp.SamplePages
{
    /// <summary>
    /// Sample page showing the MasterDetail control
    /// </summary>
    public sealed partial class MasterDetailPage
    {
        public MasterDetailPage()
        {
            InitializeComponent();

            var list = new List<DetailItem>();
            for (var i = 0; i < 30; i++)
            {
                list.Add(new DetailItem
                {
                    Header = $"Header number {i}",
                    Subject = $"This is the subject matter for item number {i}"
                });
            }

            ItemsList.ItemsSource = list;
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

        private void ItemsList_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Any())
            {
                MasterDetail.Detail = e.AddedItems.First();
            }
        }

        public class DetailItem
        {
            public string Header { get; set; }

            public string Subject { get; set; }
        }
    }
}
