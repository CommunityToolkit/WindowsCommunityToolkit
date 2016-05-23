using System;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Microsoft.Windows.Toolkit.SampleApp.Controls;
using Microsoft.Windows.Toolkit.SampleApp.Pages;
using Newtonsoft.Json;

namespace Microsoft.Windows.Toolkit.SampleApp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Shell : Page
    {
        public static Shell Current { get; private set; }

        public Shell()
        {
            InitializeComponent();

            Current = this;
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            // Get list of samples
            HamburgerMenu.ItemsSource = await Samples.GetCategoriesAsync();

            // Options
            HamburgerMenu.OptionsItemsSource = new[] { new Option { Glyph = "", Name = "About", PageType = typeof(About) } };
        }

        void HamburgerMenu_OnItemClick(object sender, ItemClickEventArgs e)
        {
            var category = e.ClickedItem as SampleCategory;

            if (category != null)
            {
                SetHeadersVisibility(false);
                NavigationFrame.Navigate(typeof(SamplePicker), category);
            }
        }

        private void HamburgerMenu_OnOptionsItemClick(object sender, ItemClickEventArgs e)
        {
            var option = e.ClickedItem as Option;
            if (option != null)
            {
                SetHeadersVisibility(false);
                NavigationFrame.Navigate(option.PageType);
            }
        }

        public void ShowOnlyHeader(string title)
        {
            Header.Visibility = Visibility.Visible;
            Title.Text = title;
        }

        private void SetHeadersVisibility(bool visible)
        {
            Header.Visibility = visible ? Visibility.Visible : Visibility.Collapsed;
            Footer.IsOpen = false;
            Footer.ClosedDisplayMode = visible ? AppBarClosedDisplayMode.Compact : AppBarClosedDisplayMode.Hidden;
            Properties.Visibility = visible ? Visibility.Visible : Visibility.Collapsed;
        }

        public async Task NavigateToSampleAsync(Sample sample)
        {
            var pageType = Type.GetType("Microsoft.Windows.Toolkit.SampleApp.SamplePages." + sample.Type);

            if (pageType != null)
            {
                SetHeadersVisibility(true);
                var propertyDesc = await sample.GetPropertyDescriptorAsync();
                DataContext = sample;
                Title.Text = sample.Name;

                NavigationFrame.Navigate(pageType, propertyDesc);
            }
        }

        private void XAMLSampleButton_OnClick(object sender, RoutedEventArgs e)
        {
            var sample = DataContext as Sample;

            if (sample != null)
            {
                CodeRenderer.XamlSource = sample.UpdatedXamlCode;
            }
            CodePanel.Visibility = Visibility.Visible;
        }

        private void CloseButton_OnClick(object sender, RoutedEventArgs e)
        {
            CodePanel.Visibility = Visibility.Collapsed;
        }
    }
}
