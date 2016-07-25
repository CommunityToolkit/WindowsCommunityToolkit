using System.Collections.Generic;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Microsoft.Windows.Toolkit.SampleApp.Models;

namespace Microsoft.Windows.Toolkit.SampleApp.SamplePages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ParallaxBehaviorPage : Page
    {
        public ParallaxBehaviorPage()
        {
            this.InitializeComponent();

            var list = new List<string>();
            for (var i = 1; i < 100; i++)
            {
                list.Add(i.ToString());
            }

            ItemsList.ItemsSource = list;
        }

        /// <summary>
        /// Invoked when the Page is loaded and becomes the current source of a parent Frame.
        /// </summary>
        /// <param name="e">Event data that can be examined by overriding code. The event data is representative of the pending navigation that will load the current Page. Usually the most relevant property to examine is Parameter.</param>
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
