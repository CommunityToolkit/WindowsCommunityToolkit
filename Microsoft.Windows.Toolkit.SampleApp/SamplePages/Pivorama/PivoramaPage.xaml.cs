using Microsoft.Windows.Toolkit.SampleApp.Data;
using Microsoft.Windows.Toolkit.SampleApp.Models;

using Windows.UI.Xaml.Navigation;

namespace Microsoft.Windows.Toolkit.SampleApp.SamplePages
{
    /// <summary>
    /// Defines the page for the Pivorama sample.
    /// </summary>
    public sealed partial class PivoramaPage
    {
        public PivoramaPage()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Called on navigating to the Pivorama sample page.
        /// </summary>
        /// <param name="e">
        /// The navigation event arguments.
        /// </param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            var propertyDesc = e.Parameter as PropertyDescriptor;

            if (propertyDesc != null)
            {
                DataContext = propertyDesc.Expando;
            }

            control.ItemsSource = new PhotosDataSource().GetGroupedItems();
        }
    }
}