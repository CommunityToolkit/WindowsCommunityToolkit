using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
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

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var propertyDesc = e.Parameter as PropertyDescriptor;

            if (propertyDesc != null)
            {
                DataContext = propertyDesc.Expando;
            }

            foreach (var key in Resources.Keys)
            {
                Templates.Add(Resources[key.ToString()] as DataTemplate);
            }

            GridViewControl.ItemsSource = Templates;
            LoadingContentControl.ContentTemplate = Templates.FirstOrDefault();

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
            LoadingContentControl.ContentTemplate = (sender as GridView).SelectedItem as DataTemplate;
        }
    }
}
