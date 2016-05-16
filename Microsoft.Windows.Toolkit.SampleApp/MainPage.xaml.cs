using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Newtonsoft.Json;

namespace Microsoft.Windows.Toolkit.SampleApp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            InitializeComponent();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            // Get list of samples
            using (var jsonStream = await Helpers.GetPackagedFileAsync("Samples/samples.json"))
            {
                var jsonString = await jsonStream.ReadTextAsync();
                var samplesCategories = JsonConvert.DeserializeObject<SampleCategory[]>(jsonString);

              //  HamburgerMenu.ItemsSource = samplesCategories;
            }
        }

        void HamburgerMenu_OnItemClick(object sender, ItemClickEventArgs e)
        {
            var category = e.ClickedItem as SampleCategory;
        }
    }
}
