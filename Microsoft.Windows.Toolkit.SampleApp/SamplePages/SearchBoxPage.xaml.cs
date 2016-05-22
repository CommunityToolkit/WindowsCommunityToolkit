using System.Windows.Input;
using Microsoft.Windows.Toolkit.SampleApp.Common;
using Microsoft.Windows.Toolkit.SampleApp.Models;
using Windows.UI.Xaml.Navigation;

namespace Microsoft.Windows.Toolkit.SampleApp.SamplePages
{
    /// <summary>
    /// Defines the page used for the SearchBox sample.
    /// </summary>
    public sealed partial class SearchBoxPage
    {
        public SearchBoxPage()
        {
            this.InitializeComponent();

            this.ExecuteSearchCommand = new DelegateCommand<string>(
                str =>
                    {
                        if (this.SearchedText != null) this.SearchedText.Text = $"You searched for '{str}'.";
                    });
        }

        /// <summary>
        /// Gets or sets the execute search command.
        /// </summary>
        public ICommand ExecuteSearchCommand { get; set; }

        /// <summary>
        /// Called on navigating to the search box page.
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
