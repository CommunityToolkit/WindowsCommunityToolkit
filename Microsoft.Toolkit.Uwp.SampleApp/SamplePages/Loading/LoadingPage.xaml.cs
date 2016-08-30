using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Navigation;
using Microsoft.Toolkit.Uwp.SampleApp.Models;

namespace Microsoft.Toolkit.Uwp.SampleApp.SamplePages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class LoadingPage
    {
        public LoadingPage()
        {
            InitializeComponent();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            var propertyDesc = e.Parameter as PropertyDescriptor;

            if (propertyDesc != null)
            {
                DataContext = propertyDesc.Expando;
            }

            AdaptiveGridViewControl.ItemsSource = await new Data.PhotosDataSource().GetItemsAsync();

            Shell.Current.RegisterNewCommand("Loading control with wait ring", async (sender, args) =>
            {
                LoadingContentControl.ContentTemplate = Resources["WaitListTemplate"] as DataTemplate;
                await ShowLoadingDialogAsync();
            });

            Shell.Current.RegisterNewCommand("Loading control with progressbar", async (sender, args) =>
            {
                LoadingContentControl.ContentTemplate = Resources["ProgressBarTemplate"] as DataTemplate;
                await ShowLoadingDialogAsync();
            });

            Shell.Current.RegisterNewCommand("Loading control with logo", async (sender, args) =>
            {
                LoadingContentControl.ContentTemplate = Resources["LogoTemplate"] as DataTemplate;
                await ShowLoadingDialogAsync();
            });

            base.OnNavigatedTo(e);
        }

        private async Task ShowLoadingDialogAsync()
        {
            LoadingControl.IsLoading = true;
            await Task.Delay(3000);
            LoadingControl.IsLoading = false;
        }
    }
}
