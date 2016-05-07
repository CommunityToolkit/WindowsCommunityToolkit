using AppStudio.DataProviders;
using AppStudio.DataProviders.$ServiceInstance.Name$;
using $ProjectDefaultNamespace$;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;

namespace Microsoft.Windows.Toolkit.Services
{
    public sealed class DataProviderSource : Control
    {
        public DataProviderSource()
        {
            this.DefaultStyleKey = typeof(DataProviderSource);
            Loaded += DataProviderSource_Loaded;
        }

        private async void DataProviderSource_Loaded(object sender, RoutedEventArgs e)
        {
            this.DataSource = await $ServiceInstance.Name$ProviderHelper.Instance.RequestAsync();
        }

        public List<$ServiceInstance.Name$Schema> DataSource
        {
            get { return (List<$ServiceInstance.Name$Schema>)GetValue(DataSourceProperty); }
            set { SetValue(DataSourceProperty, value); }
        }

        public static readonly DependencyProperty DataSourceProperty =
            DependencyProperty.Register("DataSource", typeof(List<$ServiceInstance.Name$Schema>), typeof(DataProviderSource), new PropertyMetadata(null, OnDataSourceChangedCallback));

        public static void OnDataSourceChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {

        }

    }
}
