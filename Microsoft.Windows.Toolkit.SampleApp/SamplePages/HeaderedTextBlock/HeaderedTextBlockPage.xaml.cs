using Microsoft.Windows.Toolkit.SampleApp.Models;

using Windows.UI.Xaml.Navigation;

namespace Microsoft.Windows.Toolkit.SampleApp.SamplePages
{
    public sealed partial class HeaderedTextBlockPage
    {
        public HeaderedTextBlockPage()
        {
            this.InitializeComponent();
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
        }
    }
}