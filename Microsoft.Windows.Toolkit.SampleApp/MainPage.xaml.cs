using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Microsoft.Windows.Toolkit.SampleApp.Pages;
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

                HamburgerMenu.ItemsSource = samplesCategories;
            }

            // Options
            HamburgerMenu.OptionsItemsSource = new[] {new Option {Glyph = "", Name = "About", PageType = typeof(About)}};
        }

        void HamburgerMenu_OnItemClick(object sender, ItemClickEventArgs e)
        {
            var category = e.ClickedItem as SampleCategory;

            if (category != null)
            {
                
            }
        }

        private void HamburgerMenu_OnOptionsItemClick(object sender, ItemClickEventArgs e)
        {
            var option = e.ClickedItem as Option;
            if (option != null)
            {
                NavigationFrame.Navigate(option.PageType);
            }
        }
    }
}
