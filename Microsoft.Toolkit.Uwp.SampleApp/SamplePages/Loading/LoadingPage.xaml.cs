using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;

namespace Microsoft.Toolkit.Uwp.SampleApp.SamplePages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class LoadingPage
    {
        public LoadingPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            foreach (var key in Resources.Keys)
            {
                Templates.Add(Resources[key.ToString()] as DataTemplate);
            }

            GridViewControl.ItemsSource = Templates;
            LoadingControl.LoadingContent = Templates.FirstOrDefault();

            base.OnNavigatedTo(e);
        }

        public List<DataTemplate> Templates { get; set; } = new List<DataTemplate>();

        private async void ShowLoadingDialogDelegateAsync(object sender, TappedRoutedEventArgs e)
        {
            LoadingControl.IsLoading = true;
            await Task.Delay(3000);
            LoadingControl.IsLoading = false;
        }

        private void GridViewControl_OnTapped(object sender, TappedRoutedEventArgs e)
        {
            LoadingControl.LoadingContent = (sender as GridView).SelectedItem as DataTemplate;
        }
    }
}
