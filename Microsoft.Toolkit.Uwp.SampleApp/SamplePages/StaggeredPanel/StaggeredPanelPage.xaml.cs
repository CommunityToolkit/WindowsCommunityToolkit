using System.Linq;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Microsoft.Toolkit.Uwp.SampleApp.SamplePages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class StaggeredPanelPage : Page
    {
        public StaggeredPanelPage()
        {
            this.InitializeComponent();
        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            var items = await new Data.PhotosDataSource().GetItemsAsync();
            GridView.ItemsSource = items
                .Select((p, i) => new
                {
                    Item = p,
                    Index = i + 1
                });
        }
    }
}
