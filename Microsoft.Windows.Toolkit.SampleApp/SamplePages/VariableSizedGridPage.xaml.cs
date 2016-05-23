using Windows.UI.Xaml.Navigation;
using Microsoft.Windows.Toolkit.SampleApp.Models;

namespace Microsoft.Windows.Toolkit.SampleApp.SamplePages
{
    public sealed partial class VariableSizedGridPage
    {
        public VariableSizedGridPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            var propertyDesc = e.Parameter as PropertyDescriptor;

            if (propertyDesc != null)
            {
                DataContext = propertyDesc.Expando;
            }

            control.ItemsSource = new Data.PhotosDataSource().GetItems();
        }
    }
}
